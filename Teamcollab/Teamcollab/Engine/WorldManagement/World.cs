using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Teamcollab.Engine.Helpers;

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

      cluster.HashCode = cluster.GetHashCode();
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

      return BinaryClusterSearch(clusters, 0, insertIndex, Cluster.GetHashFromXY(x, y));
    }

    // TODO(Peter): Monitor performance of this method.
    public static void GrowClusterBuffer(ref Cluster[] clusters, int amount)
    {
      Array.Resize<Cluster>(ref clusters, clusters.Length + amount);
    }

    public void SortClusters()
    {
      Array.Sort<Cluster>(clusters, delegate(Cluster c1, Cluster c2)
        {
          long c1Hash = c1.GetHashCode();
          long c2Hash = c2.GetHashCode();

          if (c1Hash == c2Hash)
          {
            return 0;
          }
          else if (c1Hash < c2Hash)
          {
            return -1;
          }

          return 1;
        }
      );

      isSorted = true;
    }

    /// <summary>
    /// Sorts an array of cluster(ideally the buffer of a world) 
    /// by their hash values through the Quicksort algorithm.
    /// </summary>
    /// <param name="array">Array to sort.</param>
    /// <param name="pivot">Array index to begin lookup.</param>
    /// <param name="left">Every index left of this will be ignored.</param>
    /// <param name="right">Every index right of this will be ignored.</param>
    private static void QuickSortClusters(ref Cluster[] array, int pivot)
    {
      //Array.Resize(ref array, Array.IndexOf(array, null));

      if (array.Length <= 2)
      {
        return;
      }

      /* Create two arrays of max value since the worst case scenario
       * could allow for all numbers to be either greater or less than the pivot. */
      Cluster[] less = new Cluster[array.Length];
      Cluster[] more = new Cluster[array.Length];

      // Index for less array
      int li = 0;
      // Index for more array
      int mi = 0;

      for (int i = 0; i < array.Length; ++i)
      {

        if (i == pivot)
        {
          continue;
        }

        if (array[i] == null)
        {
          more[mi++] = array[i];
        }
        else if (array[pivot] == null || array[i].HashCode < array[pivot].HashCode)
        {
          less[li++] = array[i];
        }
        else if (array[i].HashCode > array[pivot].HashCode)
        {
          more[mi++] = array[i];
        }
      }

      less[li++] = array[pivot];

      Array.Resize(ref less, li);
      Array.Resize(ref more, mi);

      // Sort left.
      QuickSortClusters(ref less, li / 2);

      // Sort right.
      QuickSortClusters(ref more, mi / 2);

      // Re-Assign Array
      int arrIndex = 0;
      for (int i = 0; i < less.Length; ++i, ++arrIndex)
      {
        array[arrIndex] = less[i];
      }
      for (int i = 0; i < more.Length; ++i, ++arrIndex)
      {
        array[arrIndex] = more[i];
      }
      
    }


    // TODO(Peter): Handle attempts to find non existing clusters.
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
