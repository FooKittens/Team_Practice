using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Teamcollab.Engine.Helpers;

namespace Teamcollab.Engine.WorldManagement
{
  [Serializable]
  public class World
  {
    private List<Cluster> clusters;
    private bool isSorted;

    public World()
    {
      clusters = new List<Cluster>();
      isSorted = true;
    }

    public delegate void ObjectOutOfBoundsHandler(Cluster cluster);

    public event ObjectOutOfBoundsHandler ObjectOutOfBounds;

    /// <summary>
    /// Adds a cluster to the Worlds cluster list and sorts the list.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void AddCluster(Cluster cluster)
    {
      cluster.HashCode = cluster.GetHashCode();
      clusters.Add(cluster);
      isSorted = false;
    }

    public Cluster GetCluster(int x, int y)
    {
      if (isSorted == false)
      {
        SortClusters();
      }

      return BinaryClusterSearch(clusters, 0, clusters.Count, Cluster.GetHashFromXY(x, y));
    }

    public void SortClusters()
    {
      clusters.Sort(delegate(Cluster c1, Cluster c2)
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


    // TODO(Peter): Needs heavy optimization on the array division.
    private Cluster BinaryClusterSearch(List<Cluster> clusters, int start, int length, long hashkey)
    {
      int center = start + length / 2;

      long centerHash = clusters[center].HashCode;

      if (centerHash == hashkey)
      {
        return clusters[center];
      }
      else if (centerHash > hashkey)
      {
        //Cluster[] leftArray = new Cluster[center];
        //Array.Copy(array, min, leftArray, 0, center);
        return BinaryClusterSearch(clusters, start, length / 2, hashkey);
      }
      else
      {
        //Cluster[] rightArray = new Cluster[array.Length - center];
        //Array.Copy(array, center, rightArray, 0, array.Length - center);
        return BinaryClusterSearch(clusters, center, (length + start) - center, hashkey);
      }
    }

    

    [XmlElement("Clusters")]
    public Cluster[] _Clusters
    {
      get
      {
        return clusters.ToArray();
      }
      set
      {
        clusters = new List<Cluster>(value);
      }
    }
  }
}
