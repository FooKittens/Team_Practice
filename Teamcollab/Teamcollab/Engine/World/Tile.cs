using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Teamcollab.Engine.World
{
  /// <summary>
  /// Holds all tile types
  /// </summary>
  public enum TileType : byte
  {
    Undefined = 0,

    Grass,
    Water,
  }

  /// <summary>
  /// One tiny square of the world.
  /// </summary>
  public struct Tile
  {
    #region Members
    public TileType Type;
    public Point Coordinates;
    #endregion

    public Tile(TileType type, Point coordinates)
    {
      Type = type;
      Coordinates = coordinates;
    }

  }
}