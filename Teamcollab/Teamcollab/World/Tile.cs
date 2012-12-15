using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Teamcollab.World
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
  /// One tiny square of the world
  /// </summary>
  public struct Tile
  {
    TileType type;

    public Tile(TileType type)
    {
      this.type = type;
    }
  }
}