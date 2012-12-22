﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Teamcollab.Engine.WorldManagement;
using Teamcollab.Engine.Helpers;
using System.Data;
using System.Diagnostics;
using System.Data.SQLite;
using System.ComponentModel;

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

    public event ClusterUnloadedHandler ClusterUnloaded;
    public event ClusterLoadedHandler ClusterLoaded;
    public event ClusterNotInDatabaseHandler ClusterNotLoaded;

    object databaseLock = new object();
    
    #region Members
    List<Cluster> unloadList;
    List<Coordinates> loadList;
    Thread unloadThread;
    Thread loadThread;

    BackgroundWorker databaseWorker;

    ClusterDatabase clusterDb;
    #endregion

    public AsyncClusterManager()
    {
      //loadThread = new Thread(AsyncLoader);
      //unloadThread = new Thread(AsyncUnloader);
      clusterDb = new ClusterDatabase("ClusterData.s3db");
      unloadList = new List<Cluster>();
      loadList = new List<Coordinates>();
      databaseWorker = new BackgroundWorker();
      databaseWorker.DoWork += AsyncLoader;
      databaseWorker.DoWork += AsyncUnloader;
      databaseWorker.RunWorkerCompleted += WorkCompletedHandler;
    }

    public void Abort()
    {
      unloadThread.Abort();
      loadThread.Abort();
    }

    public void UnloadCluster(Cluster cluster)
    {
      if(cluster == null)
      {
        throw new Exception("Unloading null cluster!");
      }

      unloadList.Add(cluster);
      if (loadList.Exists(delegate(Coordinates c) { return c == cluster.Coordinates; }))
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

      DevConsole.WriteLine(string.Format("Loading ({0}, {1}).", coordinates.X, coordinates.Y));
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

        // Lock the database lock to prevent the unloader from trying to connect.
        lock (databaseLock)
        {
          cluster = clusterDb.Find(coords.X, coords.Y);
        }

        if (cluster == null && ClusterNotLoaded != null)
        {
          ClusterNotLoaded(coords);
        }
        else if (ClusterLoaded != null)
        {
          ClusterLoaded(cluster);
        }
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
          lock (databaseLock)
          {
            clusterDb.InsertCluster(cluster);
            DevConsole.WriteLine("Inserted {0} into database", cluster);
          }
        }
        catch (DataException ex)
        {
          DevConsole.WriteLine(ex.Message);
        }
        catch (SQLiteException sqlex)
        {
          DevConsole.WriteLine("EXCEPTION: " + sqlex.Message);
        }
        
        // Tell the cluster to unload its resources.
        cluster.Unload();
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
