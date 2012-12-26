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
      camCluster = WorldManager.TransformInvIsometric(camCluster);

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
        if (clusters[i] != null && IsInView(clusters[i]) == false)
        {
          //DevConsole.WriteLine(string.Format("Putting {0} on the remove queue.", clusters[i].ToString()));
          //asyncManager.UnloadCluster(clusters[i]);
          //clusters[i] = null;
          //removedCount++;
          //isSorted = false;
        }
      }

      if (removedCount > 0)
      {
        TrimClusters();
      }

      Vector2 camPos = Camera2D.Position;
      camPos = WorldManager.TransformScreenToCluster(camPos);
      camPos = WorldManager.TransformInvIsometric(camPos);

      int camX = Convert.ToInt32(camPos.X);
      int camY = Convert.ToInt32(camPos.Y);

      for (int y = camY - 1; y <= camY + 1; y++)
      {
        for (int x = camX - 1; x <= camX + 1; x++)
        {
          if (GetCluster(x, y) == null)
          {
            asyncManager.LoadCluster(new Coordinates(x, y));
          }
        }
      }

      //if (GetCluster(camX, camY) == null)
      //{
        //asyncManager.LoadCluster(new Coordinates(camX, camY));
      //}
      //asyncManager.LoadCluster(new Coordinates(camX - 1, camY));
      //asyncManager.LoadCluster(new Coordinates(camX + 1, camY));
      //asyncManager.LoadCluster(new Coordinates(camX, camY - 1));
      //asyncManager.LoadCluster(new Coordinates(camX, camY + 1));
    }

    public void Draw(IsoBatch spriteBatch)
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
    /// <param name="clusterCoordinates">The coordinates of the cluster.</param>
    private bool IsInView(Coordinates clusterCoordinates)
    {
      // Creates cluster edges with clockwise winding.
      Vector2[] vertices = new[] {
        new Vector2(-0.5f, -0.5f), // Top Left
        new Vector2(0.5f, -0.5f), // Top Right
        new Vector2(0.5f, 0.5f), // Bottom Right
        new Vector2(-0.5f, 0.5f), // Bottom Left
      };

      // TODO(Peter): Create Enum ?
      const int TopLeft = 0;
      const int TopRight = 1;
      const int BottomRight = 2;
      const int BottomLeft = 3;

      // Transform all vertices to fit the cluster.
      for (int i = 0; i < 4; ++i)
      {
        vertices[i] += clusterCoordinates;
        vertices[i] = WorldManager.TransformIsometric(vertices[i]);
        vertices[i] = Vector2.Transform(
          vertices[i],
          Matrix.CreateScale(Constants.ClusterWidth, Constants.ClusterWidth, 1f) *
          Matrix.CreateScale(Constants.TileWidth, Constants.TileHeight, 1f)
        );
      }


      // Creates an Axis-Aligned BoundingBox that fits the whole cluster.
      Rectangle cAABB = new Rectangle(
        Convert.ToInt32(vertices[BottomLeft].X),
        Convert.ToInt32(vertices[TopLeft].Y),
        Convert.ToInt32(vertices[TopRight].X - vertices[BottomLeft].X),
        Convert.ToInt32(vertices[BottomRight].Y - vertices[TopLeft].Y)
      );

      // Simplicity itself.
      if (cAABB.Intersects(Camera2D.Bounds))
      {
        return true;
      }

      return false;
    }

    /// <summary>
    /// Determines if a given cluster is in view.
    /// </summary>
    /// <param name="cluster">The cluster to investigate.</param>
    private bool IsInView(Cluster cluster)
    {
      return IsInView(cluster.Coordinates);
    }

    /// <summary>
    /// Adds a cluster to the Worlds cluster list.
    /// </summary>
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

    /// <summary>
    /// Returns the cluster at the specified coordinates, if it exists.
    /// </summary>
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
      if (length == 0 || (clusters[center].HashCode != hashkey && length == 1))
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
