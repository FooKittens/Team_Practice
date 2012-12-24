using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Teamcollab.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Teamcollab.GUI;
using Teamcollab.Engine.DataManagement;
using System.Threading;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using Teamcollab.Engine.WorldGeneration;

namespace Teamcollab.Engine.WorldManagement
{
  [Serializable]
  public class World
  {
    private Cluster[] clusters;
    private bool isSorted;
    private int insertIndex;

    Stack<Cluster> loadedClusters;

    AsyncClusterManager asyncManager;

    public World(int startSize = 10)
    {
      loadedClusters = new Stack<Cluster>();
      clusters = new Cluster[startSize];
      isSorted = true;
      insertIndex = 0;

      asyncManager = new AsyncClusterManager();
      asyncManager.ClusterUnloaded += ClusterUnloadedHandler;
      asyncManager.ClusterLoaded += ClusterLoadedHandler;
      asyncManager.ClusterNotLoaded += ClusterNotLoadedHandler;
      asyncManager.UnloadWaitLimit = 1;
    }

    private void ClusterLoadedHandler(Cluster cluster)
    {

      if (BinaryClusterSearch(clusters, 0, insertIndex, cluster.HashCode) == null)
      {
        loadedClusters.Push(cluster);
      }
      
    }

    private void ClusterUnloadedHandler(Cluster cluster)
    {
      if (GetCluster(cluster.Coordinates.X, cluster.Coordinates.Y) != null)
      {
        return;
      }
    }

    private void ClusterNotLoadedHandler(Coordinates coords)
    {
      loadedClusters.Push(TerrainGenerator.CreateCluster(coords));
    }

    static Random rand = new Random();

    public Cluster CreateCluster(int clusterX, int clusterY)
    {
      Cluster cluster = new Cluster(ClusterType.Evergreen, clusterX, clusterY);

      TileType currentType = 0;

      for (int y = 0; y < Constants.ClusterHeight; ++y)
      {
        for (int x = 0; x < Constants.ClusterWidth; ++x)
        {
          Vector2 tilePos = new Vector2(
            x - Constants.ClusterWidth / 2,
            y - Constants.ClusterHeight / 2
          );

          Tile t = cluster.GetTileAt(x, y);

          if (x % 5 == 0)
          {
            currentType = (TileType)(rand.Next() % 2 + 1);
          }
          if ((x == 0 || x == Constants.ClusterWidth - 1) ||
             (y == 0 || y == Constants.ClusterHeight - 1))
          {
            t.Type = TileType.Grass;
          }
          else
          {
            t.Type = TileType.Water;
          }
          //t.Type = currentType;
          cluster.SetTileAt(x, y, t);
        }
      }

      return cluster;
    }

    public Cluster CreateCluster(Coordinates coordinates)
    {
      return CreateCluster(coordinates.X, coordinates.Y);
    }

    public void Update(GameTime gameTime)
    {
      CheckClusterLoading();

      while(loadedClusters.Count > 0)
      {
        Cluster c = loadedClusters.Pop();
        if (GetCluster(c.Coordinates.X, c.Coordinates.Y) == null)
        {
          AddCluster(c);
        }
      }
    }

    private void TrimClusters()
    {
      int firstNull = -1;
      clusters = QuickSortClusters(clusters, clusters.Length / 2);
      for (int i = 0; i < clusters.Length; ++i)
      {
        insertIndex = clusters[i] == null ? insertIndex : i;
        if (firstNull == -1 && clusters[i] == null)
        {
          firstNull = i;
        }
      }

      Array.Resize<Cluster>(ref clusters, firstNull + 1);
      insertIndex = clusters.Length - 1;
    }

    // TEST METHOD
    private bool TestClusterRange(Cluster cluster)
    {
      Vector2 camCluster = WorldManager.TransformScreenToCluster(Camera2D.Position);

      if (Convert.ToInt32(Math.Abs(cluster.Coordinates.X - camCluster.X)) > 2 ||
          Convert.ToInt32(Math.Abs(cluster.Coordinates.Y - camCluster.Y)) > 2)
      {
        return false;
      }

      return true;
    }

    private void CheckClusterLoading()
    {
      int removedCount = 0;
      for (int i = 0; i < clusters.Length; ++i)
      {
        if (clusters[i] != null && TestClusterRange(clusters[i]) == false)
        {
          DevConsole.WriteLine(string.Format("Putting {0} on the remove queue.", clusters[i].ToString()));
          asyncManager.UnloadCluster(clusters[i]);
          clusters[i] = null;
          removedCount++;
          isSorted = false;
        }
      }

      if (removedCount > 0)
      {
        TrimClusters();
      }

      Vector2 camCluster = WorldManager.TransformScreenToCluster(Camera2D.Position);

      Rectangle rect = new Rectangle(
        Convert.ToInt32(camCluster.X - 2),
        Convert.ToInt32(camCluster.Y - 2),
        4,
        4
      );

      for (int y = rect.Top; y <= rect.Bottom; ++y)
      {
        for (int x = rect.Left; x <= rect.Right; ++x)
        {
          if (GetCluster(x, y) != null) continue;
          
          asyncManager.LoadCluster(new Coordinates(x, y));
        }
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (Cluster cluster in clusters)
      {
        if (cluster == null)
        {
          continue;
        }

        if (IsInView(cluster))
        {
          cluster.Draw(spriteBatch);
        }
      }
      
    }

    /// <summary>
    /// Determines if a given cluster is in view.
    /// </summary>
    /// <param name="cluster">The cluster to investigate.</param>
    private bool IsInView(Cluster cluster)
    {
      return IsInView(cluster.Coordinates);
    }

    private bool IsInView(Coordinates clusterCoordinates)
    {
      Vector2 topLeft = new Vector2(Camera2D.Bounds.Left, Camera2D.Bounds.Top);
      Vector2 bottomRight = new Vector2(Camera2D.Bounds.Right, Camera2D.Bounds.Bottom);

      topLeft = WorldManager.TransformScreenToCluster(topLeft);
      bottomRight = WorldManager.TransformScreenToCluster(bottomRight);

      // Offsets are in cluster coordinates.
      if (clusterCoordinates.X + 0.5f >= topLeft.X &&
          clusterCoordinates.X - 0.5f <= bottomRight.X &&
          clusterCoordinates.Y - 0.5f <= bottomRight.Y &&
          clusterCoordinates.Y + 0.5f >= topLeft.Y)
      {
        return true;
      }

      return false;
    }

    /// <summary>
    /// Adds a cluster to the Worlds cluster list and sorts the list.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void AddCluster(Cluster cluster)
    {
      if (insertIndex == clusters.Length)
      {
        GrowClusterBuffer(ref clusters, 10);
      }

      cluster.SetHashCode();
      clusters[insertIndex++] = cluster;
      isSorted = false;
    }

    public Cluster GetCluster(int x, int y)
    {
      if (isSorted == false)
      {
        clusters = QuickSortClusters(clusters, clusters.Length / 2);
        isSorted = true;
      }
      
      // Search the array between 0 and the last inserted cluster.
      return BinaryClusterSearch(clusters, 0, insertIndex, Cluster.GetHashFromXY(x, y));
    }

    public static void GrowClusterBuffer(ref Cluster[] clusters, int amount)
    {
      Array.Resize<Cluster>(ref clusters, clusters.Length + amount);
    }

    /// <summary>
    /// Sorts an array of cluster(ideally the buffer of a world) recursively
    /// by their hash values through the Quicksort algorithm.
    /// </summary>
    /// <param name="array">Array to sort.</param>
    /// <param name="pivot">Array index to begin lookup.</param>
    private static Cluster[] QuickSortClusters(Cluster[] array, int pivot)
    {
      /* If the input array size is two or less, the sorting is done.
       * the one exception is when an unsorted array with the size of two is
       * given as an input parameter.
       * TODO(Peter): Handle this edge-case.*/
      if (array.Length <= 2)
      {
        return array;
      }

      /* Create two arrays of max value since the worst case scenario
       * could allow for all numbers to be either greater or less than the pivot. */
      Cluster[] less = new Cluster[array.Length];
      Cluster[] more = new Cluster[array.Length];

      // Index for less array
      int lessIndex = 0;
      // Index for more array
      int moreIndex = 0;

      for (int i = 0; i < array.Length; ++i)
      {
        // Skip the pivot as it will be appended to the less array.
        if (i == pivot)
        {
          continue;
        }

        /* Null clusters are considered as 'more' than any cluster
         * that actually exists as we want them in the end of the array. */
        if (array[i] == null)
        {
          more[moreIndex++] = array[i];
        }
        // Like above, if the pivot is null, all clusters are less than the pivot.
        else if (array[pivot] == null || array[i].HashCode < array[pivot].HashCode)
        {
          less[lessIndex++] = array[i];
        }
        else if (array[i].HashCode > array[pivot].HashCode)
        {
          more[moreIndex++] = array[i];
        }
      }

      // Append the pivot to the less list.
      less[lessIndex++] = array[pivot];

      // Resize arrays to remove the extra allocated space.
      Array.Resize(ref less, lessIndex);
      Array.Resize(ref more, moreIndex);

      // Sort the less segment recursively.
      QuickSortClusters(less, lessIndex / 2);

      // Sort the more segment recursively.
      QuickSortClusters(more, moreIndex / 2);

      // Re-assign with the two now sorted segments.
      int arrIndex = 0;
      for (int i = 0; i < less.Length; ++i, arrIndex++)
      {
        array[arrIndex] = less[i];
      }
      for (int i = 0; i < more.Length; ++i, arrIndex++)
      {
        array[arrIndex] = more[i];
      }

      return array;
    }

    /// <summary>
    /// Finds a cluster in the cluster buffer through binary search.
    /// </summary>
    /// <param name="clusters">The list of clusters to search.</param>
    /// <param name="start">Index to start search from.</param>
    /// <param name="length">Distance to search.</param>
    /// <param name="hashkey">The cluster hash key to look for.</param>
    /// <returns>The cluster if present.</returns>
    private static Cluster BinaryClusterSearch(Cluster[] clusters,
      int start, int length, long hashkey)
    {
      int center = start + length / 2;

      // Not found condition.
      if (clusters[center] == null || clusters[center].HashCode != hashkey && length == 1)
      {
        return null;
      }

      if (clusters[center].HashCode == hashkey)
      {
        return clusters[center];
      }
      else if (clusters[center].HashCode > hashkey)
      {
        return BinaryClusterSearch(clusters, start, length / 2, hashkey);
      }
      else
      {
        return BinaryClusterSearch(
          clusters,
          center,
          (length + start) - center,
          hashkey
        );
      }
    }
  }
}
