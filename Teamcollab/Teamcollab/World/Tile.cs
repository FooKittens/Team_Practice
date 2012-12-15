using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
    #region Constants
    static readonly Point size = new Point(32, 32);
    #endregion

    #region Members
    public TileType type;
    #endregion

    public Tile(TileType type)
    {
      this.type = type;
    }

    /// <summary>
    /// Returns the cluster-based bounds of this tile,
    /// as opposed to world bounds
    /// </summary>
    /// <param name="x">X index</param>
    /// <param name="y">Y index</param>
    /// <returns>Tell me if you see this anywhere</returns>
    public static Rectangle TileBounds(int x, int y)
    {
      Rectangle result = new Rectangle(
        x * size.X,
        y * size.Y,
        size.X,
        size.Y
      );
      return result;
    }

    public Texture2D GetTexture()
    {
      if (type == TileType.Undefined)
        return null;
      return null; // Library.textures[type.ToString()];
    }
  }
}