using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Teamcollab.Resources;

namespace Teamcollab.Engine.World
{

  public enum ClusterType : byte
  {
    Undefined = 0,

    Evergreen,
  }

  /// <summary>
  /// Handles a collection of Tiles
  /// </summary>
  struct Cluster
  {
    public ClusterType Type;
    public Tile[,] Tiles;
    public Point Coordinates;
    public bool Active;

    public Cluster(ClusterType type, Point coordinates)
    {
      Type = type;
      Tiles = new Tile[Constants.ClusterWidth, Constants.ClusterHeight];
      Active = false;
      Coordinates = coordinates;
    }
  }
}
