using Teamcollab.Engine.Helpers;
using System;

namespace Teamcollab.Engine.WorldManagement
{

  public enum ClusterType : byte
  {
    Undefined = 0,

    Evergreen,
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
    public bool Active;

    public Cluster(ClusterType type, Coordinates coordinates)
    {
      Type = type;
      Tiles = new Tile[Constants.ClusterWidth * Constants.ClusterHeight];
      Active = false;
      Coordinates = coordinates;
    }

    public override string ToString()
    {
      return string.Format("({0}, {1})", Coordinates.X, Coordinates.Y);
    }

    public override int GetHashCode()
    {
      return GetHashFromXY(Coordinates.X, Coordinates.Y);
    }

    public static int GetHashFromXY(int x, int y)
    {
      long hash = ((long)x << 32) + y;
      return (int)(hash >> 32);
    }
  }
}