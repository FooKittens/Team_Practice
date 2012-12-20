using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;

namespace Teamcollab.Engine.DataManagement
{
  class SQLiteDatabase
  {
    #region Properties
    public bool Connected { get { return connection.State == ConnectionState.Open; } }
    public string[] CommandHistory { get { return commandHistory.ToArray(); } }

    protected SQLiteConnection Connection { get { return connection; } }
    #endregion

    // TEMP CONSTS
    const int MaxCommandHistory = 20;

    #region Members
    SQLiteConnection connection;
    readonly string cnnString;
    Queue<string> commandHistory;
    #endregion

    public SQLiteDatabase(string filepath)
    {
      cnnString = filepath;
      connection = new SQLiteConnection(
        string.Format("Data Source={0}",
        filepath)
      );
    }

    /// <summary>
    /// Returns a DataTable from the database as specified by the query.
    /// </summary>
    /// <param name="query">An SQL query for selecting.</param>
    public virtual DataTable Select(string query)
    {
      AddToCommandHistory(query);

      DataTable table = new DataTable();
      connection.Open();
      using (SQLiteCommand cmd = new SQLiteCommand(connection))
      {
        cmd.CommandText = query;
        SQLiteDataReader reader = cmd.ExecuteReader();
        table.Load(reader);
        reader.Close();
      }
      connection.Close();

      return table;
    }

    /// <summary>
    /// Inserts into the database using the paramter query.
    /// Example: "INSERT INTO |table|(|colname1|, |colname2|) VALUES(val1, val2)".
    /// </summary>
    /// <param name="sql">SQL Language Query.</param>
    public virtual void Insert(string sql)
    {
      connection.Open();
      using (SQLiteCommand cmd = new SQLiteCommand(connection))
      {
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
      }
      connection.Close();
    }

    public virtual int RunNonQuery(string sql)
    {
      int rowsUpdated = 0;

      connection.Open();
      using (SQLiteCommand cmd = new SQLiteCommand(connection))
      {
        cmd.CommandText = sql;
        rowsUpdated = cmd.ExecuteNonQuery();
      }
      connection.Close();
      return rowsUpdated;
    }

    /// <summary>
    /// The |MaxCommandHistory| amount of commands will be saved by
    /// this method. If MaxCommandHistory is -1 the queue will never dequeue.
    /// </summary>
    /// <param name="query">The query to be saved.</param>
    protected virtual void AddToCommandHistory(string query)
    {
      commandHistory.Enqueue(query);
      if (commandHistory.Count > MaxCommandHistory && MaxCommandHistory != -1)
      {
        commandHistory.Dequeue();
      }
    }
  }
}
