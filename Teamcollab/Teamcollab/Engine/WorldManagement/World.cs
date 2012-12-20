using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Teamcollab.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Teamcollab.GUI;

namespace Teamcollab.Engine.WorldManagement
{
  [Serializable]
  public class World
  {
    private Cluster[] clusters;
    private bool isSorted;
    private int insertIndex;

    public World(int startSize = 10)
    {
      clusters = new Cluster[startSize];
      isSorted = true;
      insertIndex = 0;
    }

    public void Update(GameTime gameTime)
    {

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
      Vector2 topLeft = new Vector2(Camera2D.Bounds.Left, Camera2D.Bounds.Top);
      Vector2 bottomRight = new Vector2(Camera2D.Bounds.Right, Camera2D.Bounds.Bottom);

      topLeft = WorldManager.TransformScreenToCluster(topLeft);
      bottomRight = WorldManager.TransformScreenToCluster(bottomRight);
      
      // Offsets are in cluster coordinates.
      if (cluster.Coordinates.X + 0.5f >= topLeft.X &&
          cluster.Coordinates.X - 0.5f <= bottomRight.X &&
          cluster.Coordinates.Y - 0.5f <= bottomRight.Y &&
          cluster.Coordinates.Y + 0.5f >= topLeft.Y)
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
        QuickSortClusters(ref clusters, clusters.Length / 2);
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
    private static void QuickSortClusters(ref Cluster[] array, int pivot)
    {
      /* If the input array size is two or less, the sorting is done.
       * the one exception is when an unsorted array with the size of two is
       * given as an input parameter.
       * TODO(Peter): Handle this edge-case.*/
      if (array.Length <= 2)
      {
        return;
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
      QuickSortClusters(ref less, lessIndex / 2);

      // Sort the more segment recursively.
      QuickSortClusters(ref more, moreIndex / 2);

      // Re-assign with the two now sorted segments.
      for (int i = 0; i < array.Length; ++i)
      {
        array[i] = i < less.Length ? less[i] : more[i - less.Length];        
      }
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
