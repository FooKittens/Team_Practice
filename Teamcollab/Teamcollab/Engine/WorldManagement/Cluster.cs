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
    public ClusterType Type;
    public Tile[] Tiles;
    public Coordinates Coordinates;
    public long HashCode;
    public bool Active;

    public Cluster(ClusterType type, Coordinates coordinates)
    {
      Type = type;
      Tiles = new Tile[Constants.ClusterWidth * Constants.ClusterHeight];
      Active = false;
      Coordinates = coordinates;
      HashCode = GetHashCode();
    }

    public override string ToString()
    {
      return string.Format("({0}, {1})", Coordinates.X, Coordinates.Y);
    }

    public new long GetHashCode()
    {
      return GetHashFromXY(Coordinates.X, Coordinates.Y);
    }

    public static long GetHashFromXY(int x, int y)
    {
      long hash = ((long)x << 32) + y;
      return hash;
    }
  }
}