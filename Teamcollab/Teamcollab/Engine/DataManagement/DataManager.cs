using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Teamcollab.Engine.WorldManagement;
using System.IO;
using System.Data;

namespace Teamcollab.Engine.DataManagement
{
  static class DataManager
  {
    #region Properties

    #endregion

    #region Members

    #endregion

    public static void SaveWorld(World world)
    {
      string path = string.Format(
        @"Saves\{0}.xml", world.Name
      );

      if (Directory.Exists(path))
      {
        // TODO(Peter): Handle file exists.
      }
      else
      {
        if (!Directory.Exists("Saves"))
        {
          Directory.CreateDirectory("Saves");
        }
      }

      FileStream stream = new FileStream(path, FileMode.Create);

      XmlWriterSettings ws = new XmlWriterSettings();
      ws.Indent = true;

      XmlWriter writer = XmlWriter.Create(stream);
      
      //// Version string
      writer.WriteStartDocument();

      writer.WriteStartElement("World");

      // World Name.
      writer.WriteStartElement("Name");
      writer.WriteValue(world.Name);
      writer.WriteEndElement();

      // Time of creation
      writer.WriteStartElement("CreationTimeTicks");
      writer.WriteValue(world.CreationTimeTicks);
      writer.WriteEndElement();

      // Last time played in ticks.
      writer.WriteStartElement("LastPlayedTicks");
      writer.WriteValue(world.LastPlayedTimeTicks);
      writer.WriteEndElement();

      // Seed used in randomization
      writer.WriteStartElement("Seed");
      writer.WriteValue(world.Seed);
      writer.WriteEndElement();

      // Current in-game time.
      writer.WriteStartElement("CurrentGameTimeTicks");
      writer.WriteValue(world.CurrentTimeTicks);
      writer.WriteEndElement();

      // Write actual world data.
      WriteWorldData(writer, world);

      // End the XML-Document
      writer.WriteEndDocument();

      // Should not be needed...
      writer.Flush();

      // Don't forget to dispose..!
      writer.Close();
      stream.Close();
    }


    public static void SaveCluster(World world, Cluster cluster)
    {
      FileStream stream = File.Open(
        string.Format("Saves\\{0}.xml", world.Name),
        FileMode.Open, FileAccess.Read
      );

      //XmlWriter writer = XmlWriter.Create(stream);
      XmlTextReader reader = new XmlTextReader(stream);

      reader.ReadToFollowing("Data");
      reader.ReadToDescendant("Clusters");
      reader.ReadToDescendant("Cluster");

      

      bool found = false;
      while (!found || reader.IsStartElement("Cluster"))
      {
        reader.MoveToAttribute("Y");
        int y = reader.ReadContentAsInt();
        reader.MoveToAttribute("X");
        int x = reader.ReadContentAsInt();
        if (y == cluster.Coordinates.Y && x == cluster.Coordinates.X)
        {
          found = true;
          break;
        }
        
        if (!reader.ReadToNextSibling("Cluster"))
        {
          //writer.WriteNode(reader, false);
          //WriteClusterData(writer, cluster);
          break;
        }
      }

      if (found)
      {
        //writer.Close();
        stream.Close();
        throw new DataException("Cluster Element exists in world file.");
      }

      //writer.Close();
      stream.Close();
    }

    /// <summary>
    /// Writes data as a part of an open XmlWriter stream.
    /// </summary>
    private static void WriteWorldData(XmlWriter writer, World world)
    {
      writer.WriteStartElement("Data");

      // Write every cluster currently in memory.
      writer.WriteStartElement("Clusters");
      foreach (Cluster cluster in world.Clusters)
      {
        if (cluster != null)
        {
          WriteClusterData(writer, cluster);
        }
      }
      writer.WriteEndElement();

      // TODO(Peter): Write Player Data and Entity Data.

      writer.WriteEndElement();
    }

    private static void WriteClusterData(XmlWriter writer, Cluster cluster)
    {
      writer.WriteStartElement("Cluster");
      writer.WriteAttributeString("X", cluster.Coordinates.X.ToString());
      writer.WriteAttributeString("Y", cluster.Coordinates.Y.ToString());
      writer.WriteStartElement("Data");

      byte[] clusterData = cluster.GetData();

      // Writes ClusterData with base64 encoding.
      writer.WriteBase64(clusterData, 0, clusterData.Length);
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
  }
}
