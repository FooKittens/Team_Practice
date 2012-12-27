using System;
using System.Data;
using System.Data.SQLite;
using Teamcollab.Engine.Helpers;
using Teamcollab.Engine.WorldManagement;

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
    #endregion

    public ClusterDatabase(string filepath)
      :base(filepath)
    {
    }

    //public virtual Cluster Find(long hashcode)
    //{
    //  DataTable table = base.Select(
    //    string.Format("SELECT * FROM clusters WHERE hashcode={0}", hashcode)
    //  );

    //  if (table.Rows.Count == 0)
    //  {
    //    return null;
    //  }
    //  else if (table.Rows.Count > 1)
    //  {
    //    throw new DataException(
    //      "Multiple clusters found with the same hashcode!"
    //    );
    //  }

    //  System.Diagnostics.Debug.WriteLine("Found row:");
    //  System.Diagnostics.Debug.WriteLine(table.Rows[0]["hashcode"].ToString());
    //  System.Diagnostics.Debug.WriteLine(table.Rows[0]["data"].ToString());

    //  ClusterData data;
    //  GetClusterDataFromRow(table.Rows[0], out data);
    //  return new Cluster(data);
    //}

    //public virtual Cluster Find(int x, int y)
    //{
    //  return Find(Cluster.GetHashFromXY(x, y));
    //}

    public void InsertCluster(Cluster data)
    {
      long hashcode = Cluster.GetHashFromXY(
        data.Coordinates.X,
        data.Coordinates.Y
      );

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

      byte[] bytes = data.GetData();

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

    private void GetClusterDataFromRow(DataRow row, out ClusterData data)
    {
      byte[] bytes = (byte[])row["data"];
      bytes = CompressionHelper.Decompress(bytes);

      const int clusterLength = Constants.ClusterWidth * Constants.ClusterHeight;

      data = new ClusterData();
      data.Tiles = new Tile[clusterLength];
      
      data.Type = (ClusterType)bytes[0];

      for (int i = 1; i < clusterLength + 1; ++i)
      {
        data.Tiles[i - 1] = new Tile((TileType)bytes[i]);
      }

      Coordinates coords = new Coordinates();
      coords.X = BitConverter.ToInt32(bytes, bytes.Length - 8);
      coords.Y = BitConverter.ToInt32(bytes, bytes.Length - 4);
      data.Coordinates = coords;
    }
  }
}
