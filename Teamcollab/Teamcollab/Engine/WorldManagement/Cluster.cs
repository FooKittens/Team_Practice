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
  class Cluster
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
      return Coordinates.ToString();
    }
  }
}