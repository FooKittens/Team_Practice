using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Teamcollab.Engine.WorldManagement
{
  [Serializable]
  struct World
  {
    public List<Cluster> Clusters;
    




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
