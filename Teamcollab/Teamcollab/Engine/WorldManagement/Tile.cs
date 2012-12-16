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
  }

  /// <summary>
  /// One tiny square of the world.
  /// </summary>
  [Serializable]
  public struct Tile
  {
    #region Members
    public TileType Type;
    public Vector2 Position;
    #endregion

    public Tile(TileType type, Vector2 position)
    {
      Type = type;
      Position = position;
    }
  }
}