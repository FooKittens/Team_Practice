using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Midgard.Engine.DataManagement;
using Midgard.Engine.Helpers;
using Midgard.Engine.WorldGeneration;
using Midgard.GUI;

namespace Midgard.Engine.WorldManagement
{
  public class World
  {
    public Cluster[] Clusters { get { return clusters; } }
    public int Seed { get; set; }
    public string Name { get; set; }
    public long CreationTimeTicks { get; set; }
    public long LastPlayedTimeTicks { get; set; }
    public long CurrentTimeTicks { get; set; }

    private Cluster[] clusters;
    private bool isSorted;
    private int insertIndex;
    private bool initialized;

    List<Cluster> loadedClusters;

    AsyncClusterManager asyncManager;

    // Used for loading.
    public World() { }

    public World(int seed, string name = "test")
    {
      Name = name;
      Seed = seed;
      CreationTimeTicks = DateTime.UtcNow.Ticks;
      LastPlayedTimeTicks = DateTime.UtcNow.Ticks;
      Initialize();
      DataManager.SaveWorld(this);
    }

    public void Initialize()
    {
      loadedClusters = new List<Cluster>();
      clusters = new Cluster[1];
      isSorted = true;
      insertIndex = 0;

      TerrainGenerator.Initialize(Seed);
      asyncManager = new AsyncClusterManager(this);
      //asyncManager.ClusterUnloaded += ClusterUnloadedHandler;
      asyncManager.ClusterLoaded += ClusterLoadedHandler;
      //asyncManager.ClusterNotLoaded += ClusterNotLoadedHandler;
      asyncManager.UnloadWaitLimit = 1;

      initialized = true;
    }

    private void ClusterLoadedHandler(Cluster cluster)
    {
      if (BinaryClusterSearch(clusters, 0, insertIndex, cluster.HashCode) == null)
      {
        loadedClusters.Add(cluster);
      }
    }

    private void ClusterUnloadedHandler(Cluster cluster)
    {
      if (GetCluster(cluster.Coordinates.X, cluster.Coordinates.Y) != null)
      {
        DevConsole.WriteLine("Unload failed for: {0} ?", cluster.ToString());
      }
    }

    private void ClusterNotLoadedHandler(Coordinates coords)
    {
      loadedClusters.Add(TerrainGenerator.CreateCluster(coords));
    }

    public void Update(GameTime gameTime)
    {
      // TODO(Peter): Handle better?
      if (!initialized)
      {
        throw new NotSupportedException(
          "World must be initialized prior to updating!"
        );
      }

      CheckClusterLoading();

      while(loadedClusters.Count > 0)
      {
        Cluster c = loadedClusters[0];
        loadedClusters.RemoveAt(0);
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
        if (clusters[i] == null)
        {
          firstNull = i;
          break;
        }
      }

      Array.Resize<Cluster>(ref clusters, firstNull + 1);
      insertIndex = clusters.Length - 1;
    }

    private void CheckClusterLoading()
    {
      int removedCount = 0;
      for (int i = 0; i < clusters.Length; ++i)
      {
        if (clusters[i] != null && IsInView(clusters[i]) == false)
        {
          DevConsole.WriteLine(string.Format(
            "Putting {0} on the remove queue.", clusters[i].ToString())
          );
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

      Vector2 camPos = Camera2D.Position;
      camPos = WorldManager.TransformScreenToCluster(camPos);
      camPos = WorldManager.TransformInvIsometric(camPos);

      int camX = Convert.ToInt32(camPos.X);
      int camY = Convert.ToInt32(camPos.Y);

      for (int y = camY - 1; y <= camY + 1; y++)
      {
        for (int x = camX - 1; x <= camX + 1; x++)
        {
          if (GetCluster(x, y) == null && IsLoaded(x, y) == false &&
            IsInView(new Coordinates(x, y)))
          {
            asyncManager.LoadCluster(new Coordinates(x, y));
          }
        }
      }
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

    private bool IsLoaded(int x, int y)
    {
      return loadedClusters.Exists(delegate(Cluster cluster)
      {
        return cluster.Coordinates.X == x && cluster.Coordinates.Y == y;
      });
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
          Matrix.CreateScale(Constants.ClusterWidth,
            Constants.ClusterWidth, 1f) *
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
      return BinaryClusterSearch(clusters, 0,
        insertIndex, Cluster.GetHashFromXY(x, y)
      );
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
      if (clusters[center] == null ||
         (clusters[center].HashCode != hashkey && length == 1))
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
