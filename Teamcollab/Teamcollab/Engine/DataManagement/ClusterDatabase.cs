﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Teamcollab.Engine.WorldManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data.SQLite;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Teamcollab.Engine.Helpers;

namespace Teamcollab.Engine.DataManagement
{
  /// <summary>
  /// ClusterDatabase specializes SQLiteDatabase to work with clusters
  /// in a much easier way than writing manual queries.
  /// </summary>
  class ClusterDatabase : SQLiteDatabase
  {
    #region Properties

    #endregion

    volatile object locker = new object();

    #region Members
    BinaryFormatter serializer;
    #endregion

    public ClusterDatabase(string filepath)
      :base(filepath)
    {
      serializer = new BinaryFormatter();
    }

    public virtual Cluster Find(long hashcode)
    {
      DataTable table = base.Select(
        string.Format("SELECT * FROM clusters WHERE hashcode={0}", hashcode)
      );

      if (table.Rows.Count == 0)
      {
        return null;
      }
      else if (table.Rows.Count > 1)
      {
        throw new DataException("Multiple clusters found with the same hashcode!");
      }

      System.Diagnostics.Debug.WriteLine("Found row:");
      System.Diagnostics.Debug.WriteLine(table.Rows[0]["hashcode"].ToString());
      System.Diagnostics.Debug.WriteLine(table.Rows[0]["data"].ToString());

      ClusterData data;
      GetClusterDataFromRow(table.Rows[0], out data);
      return new Cluster(data);
    }

    public virtual Cluster Find(int x, int y)
    {
      return Find(Cluster.GetHashFromXY(x, y));
    }

    public void InsertCluster(Cluster data)
    {
      long hashcode = Cluster.GetHashFromXY(data.Coordinates.X, data.Coordinates.Y);

      if (base.Select(string.Format(
        "SELECT hashcode FROM clusters WHERE hashcode={0};", hashcode
        )
      ).Rows.Count != 0)
      {
        throw new DataException(string.Format(
            "Cluster with id --> {0} <-- already exists in database.",
            hashcode
          )
        );
      }
      
      string sql = string.Format(
        "INSERT INTO clusters(hashcode, data) VALUES({0}, @data);",
        hashcode
      );

      AddToCommandHistory(sql);

      byte[] bytes = data.GetClusterBytes();

      //using (MemoryStream memStream = new MemoryStream())
      //{
      //  serializer.Serialize(memStream, data);
      //  memStream.Close();
      //  bytes = memStream.GetBuffer();
        
      //}

      base.Connection.Open();
      using (SQLiteCommand cmd = new SQLiteCommand(base.Connection))
      {
        cmd.CommandText = sql;
        cmd.Parameters.Add(
          "@data", DbType.Binary, bytes.Length
        ).Value = bytes;

        cmd.ExecuteNonQuery();
      }
      base.Connection.Close();
      
    }

    //public void InsertCluster(Cluster cluster)
    //{
    //  InsertCluster(cluster);
    //}

    private void GetClusterDataFromRow(DataRow row, out ClusterData data)
    {
      byte[] bytes = (byte[])row["data"];
      //using (MemoryStream memStream = new MemoryStream(bytes))
      //{
      //  memStream.Seek(0, SeekOrigin.Begin);
      //  data = (ClusterData)serializer.Deserialize(memStream);
      //  memStream.Close();
      //}

      const int clusterLength = Constants.ClusterWidth * Constants.ClusterHeight;

      data = new ClusterData();
      data.Tiles = new Tile[clusterLength];
      
      // Get huffman encoded tile data.
      using (MemoryStream stream = new MemoryStream(bytes))
      {
        data.Type = (ClusterType)stream.ReadByte();
        int tileIndex = 0;
        while (tileIndex < clusterLength)
        {
          //stream.Seek(streamIndex, SeekOrigin.End);
          TileType type = (TileType)stream.ReadByte();
          
          byte[] intCounter = new byte[4];

          for (int i = 0; i < 4; ++i)
            intCounter[i] = (byte)stream.ReadByte();
                    
          int tileCount = BitConverter.ToInt32(intCounter, 0);
          
          for (int i = 0; i < tileCount; ++i)
          {
            data.Tiles[tileIndex++] = new Tile(type);
          }
        }
      }

      //for (int i = 1; i < clusterLength + 1; ++i)
      //{
      //  data.Tiles[i - 1] = new Tile((TileType)bytes[i]);
      //}

      Coordinates coords = new Coordinates();
      coords.X = BitConverter.ToInt32(bytes, bytes.Length - 8);
      coords.Y = BitConverter.ToInt32(bytes, bytes.Length - 4);
      data.Coordinates = coords;
    }
  }
}
