using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Threading;
using Teamcollab.Engine.Helpers;
using Teamcollab.Engine.WorldGeneration;
using Teamcollab.Engine.WorldManagement;

namespace Teamcollab.Engine.DataManagement
{
  class AsyncClusterManager
  {
    #region Properties
    public int UnloadWaitLimit { get; set; }

    #endregion

    public delegate void ClusterLoadedHandler(Cluster cluster);
    public delegate void ClusterUnloadedHandler(Cluster cluster);
    public delegate void ClusterNotInDatabaseHandler(Coordinates coords);

    //public event ClusterUnloadedHandler ClusterUnloaded;
    public event ClusterLoadedHandler ClusterLoaded;
    //public event ClusterNotInDatabaseHandler ClusterNotLoaded;

    object databaseLock = new object();
    
    #region Members
    List<Cluster> unloadList;
    List<Coordinates> loadList;    
    BackgroundWorker databaseWorker;
    ClusterDatabase clusterDb;

    // This reference should not change.
    readonly World world;

    #endregion

    public AsyncClusterManager(World world)
    {
      this.world = world;
      clusterDb = new ClusterDatabase("ClusterData.s3db");
      unloadList = new List<Cluster>();
      loadList = new List<Coordinates>();
      databaseWorker = new BackgroundWorker();

      databaseWorker.DoWork += AsyncLoader;
      databaseWorker.DoWork += AsyncUnloader;
      databaseWorker.RunWorkerCompleted += WorkCompletedHandler;
    }

    public void UnloadCluster(Cluster cluster)
    {
      if(cluster == null)
      {
        throw new Exception("Unloading null cluster!");
      }

      unloadList.Add(cluster);
      if (loadList.Exists(
        delegate(Coordinates c) { return c == cluster.Coordinates; }))
      {
        loadList.Remove(cluster.Coordinates);
      }


      if (unloadList.Count > UnloadWaitLimit && databaseWorker.IsBusy == false)
      {
        StartBackgroundWorker();
      }
    }

    public void LoadCluster(Coordinates coordinates)
    {

      if (loadList.Exists(delegate(Coordinates c)
      {
        return c == coordinates;
      }))
        return;

      DevConsole.WriteLine(string.Format("Loading ({0}, {1}).",
        coordinates.X, coordinates.Y)
      );
      loadList.Add(coordinates);

      if (databaseWorker.IsBusy == false)
      {
        StartBackgroundWorker();
      }
    }

    private void WorkCompletedHandler(object obj, RunWorkerCompletedEventArgs args)
    {
      if (loadList.Count > 0 || unloadList.Count > 0)
      {
        DevConsole.WriteLine("Backgroundworker restarting.");
        databaseWorker.RunWorkerAsync();
      }
      DevConsole.WriteLine("Backgroundworker done.");
    }

    private void AsyncLoader(object obj, DoWorkEventArgs args)
    {
      while (loadList.Count > 0)
      {
        Coordinates coords = loadList[0];
        loadList.RemoveAt(0);
        Cluster cluster;

        cluster = DataManager.LoadCluster(world, coords.X, coords.Y);

        // If no cluster is found, generate a new one.
        if (cluster == null)
        {
          cluster = TerrainGenerator.CreateCluster(coords);
        }

        ClusterLoaded(cluster);
      }
    }

    private void AsyncUnloader(object obj, DoWorkEventArgs args)
    {
      while(unloadList.Count > 0)
      {
        Cluster cluster = unloadList[0];
        unloadList.RemoveAt(0);

        try
        {
          DataManager.SaveCluster(world, cluster);
          DevConsole.WriteLine("Saved {0} to file.", cluster);
        }
        catch (DataException ex)
        {
          DevConsole.WriteLine(ex.Message);
        }
        catch (SQLiteException sqlex)
        {
          DevConsole.WriteLine("EXCEPTION: " + sqlex.Message);
        }
        
        //TODO(Martin): wat?

        // Tell the cluster to unload its resources.
        //cluster.Unload();
        Thread.Yield();
      }
    }

    private void StartBackgroundWorker()
    {
      DevConsole.WriteLine("Backgroundworker starting.");
      databaseWorker.RunWorkerAsync();
    }
  }
}
