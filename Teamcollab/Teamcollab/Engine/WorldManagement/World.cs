using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Teamcollab.Engine.WorldManagement
{
  [Serializable]
  public class World
  {
    public List<Cluster> Clusters;

    public World()
    {
      Clusters = new List<Cluster>();
    }

    public Cluster GetCluster(int x, int y)
    {
      return BinaryClusterSearch(Clusters.ToArray(), Cluster.GetHashFromXY(x, y));
    }

    public void SortClusters()
    {
      Clusters.Sort(delegate(Cluster c1, Cluster c2)
        {
          int c1Hash = c1.GetHashCode();
          int c2Hash = c2.GetHashCode();

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
    }

    private Cluster BinaryClusterSearch(Cluster[] array, int hashkey)
    {
      int center = array.Length / 2;

      int centerHash = array[center].GetHashCode();

      if (centerHash == hashkey)
      {
        return array[array.Length / 2];
      }
      else if (centerHash > hashkey)
      {
        Cluster[] leftArray = new Cluster[center];
        Array.Copy(array, 0, leftArray, 0, center);
        return BinaryClusterSearch(leftArray, hashkey);
      }
      else
      {
        Cluster[] rightArray = new Cluster[center];
        Array.Copy(array, center, rightArray, 0, center - 1);
        return BinaryClusterSearch(rightArray, hashkey);
      }
    }


    [XmlElement("Clusters")]
    public Cluster[] _Clusters
    {
      get
      {
        return Clusters.ToArray();
      }
      set
      {
        Clusters = new List<Cluster>(value);
      }
    }
  }
}
