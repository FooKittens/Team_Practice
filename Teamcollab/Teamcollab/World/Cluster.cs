using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
  }
}
