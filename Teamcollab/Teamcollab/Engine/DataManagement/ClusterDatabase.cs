using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Teamcollab.Engine.WorldManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data.SQLite;
using System.Runtime.InteropServices;

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

    #region Members
    BinaryFormatter serializer;
    #endregion

    public ClusterDatabase(string filepath)
      :base(filepath)
    {
    }

    public virtual Cluster Find(long hashcode)
    {
      DataTable table = base.Select(
        string.Format("SELECT data FROM clusters WHERE hashcode={0}", hashcode)
      );

      if (table.Rows.Count == 0)
      {
        return null;
      }
      else if (table.Rows.Count > 1)
      {
        throw new DataException("Multiple clusters found with the same hashcode!");
      }

      ClusterData data;
      GetClusterDataFromRow(table.Rows[0], out data);
      return new Cluster(data);
    }

    public virtual Cluster Find(int x, int y)
    {
      return Find(Cluster.GetHashFromXY(x, y));
    }

    public void InsertCluster(ClusterData data)
    {
      // Blocking loop for thread safety.
      while (Connected) ;

      long hashcode = Cluster.GetHashFromXY(data.Coordinates.X, data.Coordinates.Y);

      string sql = string.Format(
        "INSERT INTO clusters(hashcode, data) VALUES({0}, @data);",
        hashcode
      );

      AddToCommandHistory(sql);

      base.Connection.Open();
      using (SQLiteCommand cmd = new SQLiteCommand(base.Connection))
      {
        cmd.CommandText = sql;
        cmd.Parameters.Add(
          "@data", DbType.Binary, Marshal.SizeOf(data)
        );

        cmd.ExecuteNonQuery();
      }
      base.Connection.Close();
    }

    public void InsertCluster(Cluster cluster)
    {
      InsertCluster(cluster.GetData());
    }

    private void GetClusterDataFromRow(DataRow row, out ClusterData data)
    {
      byte[] bytes = (byte[])row["data"];
      using (MemoryStream memStream = new MemoryStream(bytes))
      {
        memStream.Seek(0, SeekOrigin.Begin);
        data = (ClusterData)serializer.Deserialize(memStream);
        memStream.Close();
      }
    }
  }
}
