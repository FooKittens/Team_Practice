using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Teamcollab.Engine.Helpers;

namespace Teamcollab.Engine.WorldManagement
{
  /// <summary>
  /// Holds all tile types
  /// </summary>
  public enum TileType : byte
  {
    Undefined = 0,

    Grass,
    Water,
    Mountain,
  }

  /// <summary>
  /// One tiny square of the world.
  /// </summary>
  [Serializable]
  public class Tile
  {
    #region Members
    public TileType Type;
    public Coordinates Coordinates;
    #endregion

    public Tile(TileType type)
    {
      Type = type;
    }

    public static Tile Create(TileType type)
    {
      Tile created = new Tile(type);

      // Configure the tile depending on the type here.
      switch (type)
      {
        case TileType.Undefined:
          throw new ArgumentException("Undefined tiletype requested.");
        case TileType.Grass:
          
          break;
        case TileType.Water:
          
          break;
        case TileType.Mountain:

          break;
        default:
          throw new ArgumentException("Default in Tile.Create() triggered.");
      }

      return created;
    }

   

  }
}