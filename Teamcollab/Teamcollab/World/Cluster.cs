using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Teamcollab.World
{
  /// <summary>
  /// Handles a collection of Tiles
  /// </summary>
  class Cluster
  {
    Tile[,] tiles;

    public Cluster()
    {
      tiles = new Tile[128, 128];

      for (int y = 0; y < tiles.GetLength(1); y++)
      {
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
          tiles[x, y] = new Tile(TileType.Grass);
        }
      }
    }

    /// <summary>
    /// Draws the cluster
    /// </summary>
    /// <param name="spriteBatch">Spritebatch used for drawing</param>
    public void Draw(SpriteBatch spriteBatch, GUI.Camera2D camera)
    {
      for (int y = 0; y < tiles.GetLength(1); y++)
      {
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
          Tile tile = tiles[x, y];
          switch (tile.type)
          {
            case TileType.Undefined:
              throw new NotImplementedException();
            case TileType.Grass:
            case TileType.Water:
              Rectangle bounds = Tile.TileBounds(x, y);
              if (camera.Bounds.Intersects(bounds))
              {
                spriteBatch.Draw(tile.GetTexture(), bounds, Color.White);
              }
              break;
            default:
              throw new NotImplementedException();
          }
          tiles[x, y] = new Tile(TileType.Grass);
        }
      }
    }
  }
}
