using Teamcollab.Engine.Helpers;
using System;

namespace Teamcollab.Engine.WorldManagement
{

  public enum ClusterType : byte
  {
    Undefined = 0,

    Evergreen,
  }

  [Serializable]
  public struct ClusterData
  {
    public ClusterType Type;
    public Tile[] Tiles;
    public Coordinates Coordinates;
    public bool Active;
  }

  /// <summary>
  /// Handles a collection of Tiles
  /// </summary>
  [Serializable]
  public class Cluster
  {
    #region Properties
    public long HashCode { get; private set; }
    public ClusterType Type { get; private set; }
    public bool Active { get; private set; }
    public bool Loaded { get; private set; }
    
    #endregion

    #region Members
    private Tile[] tiles;
    public Coordinates Coordinates;
    #endregion

    public Cluster(ClusterType type, Coordinates coordinates)
    {
      Type = type;
      tiles = new Tile[Constants.ClusterWidth * Constants.ClusterHeight];
      Active = false;
      Coordinates = coordinates;
      HashCode = GetHashCode();
    }

    public void SetTileAt(int x, int y, Tile newTile)
    {
      tiles[y * Constants.ClusterWidth + x] = newTile;
    }

    public void SetTileAt(Coordinates coord, Tile newTile)
    {
      SetTileAt(coord.X, coord.Y, newTile);
    }

    /// <summary>
    /// Retrieves the tile at the input coordinates relative
    /// to the cluster.
    /// </summary>
    public Tile GetTileAt(int x, int y)
    {
      return tiles[y * Constants.ClusterWidth + x];
    }

    /// <summary>
    /// Retrieves the tile at the input coordinates relative
    /// to the cluster.
    /// </summary>
    private Tile GetTileAt(Coordinates coord)
    {
      return GetTileAt(coord.X, coord.Y);
    }

    public override string ToString()
    {
      return string.Format("({0}, {1})", Coordinates.X, Coordinates.Y);
    }

    /// <summary>
    /// Tells the Cluster to generate its hashcode.
    /// </summary>
    public void SetHashCode()
    {
      HashCode = GetHashFromXY(Coordinates.X, Coordinates.Y);
    }

    /// <summary>
    /// Retrieve a hashcode using the same algorithm as
    /// the cluster uses to generate its hashcode from 
    /// two coordinates.
    /// </summary>
    public static long GetHashFromXY(int x, int y)
    {
      long hash = ((long)x << 32) + y;
      return hash;
    }
  }
}