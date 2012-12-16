using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Teamcollab.Engine.Helpers;

namespace Teamcollab.Engine.World
{
  sealed class WorldManager
  {
    #region Properties

    #endregion

    #region Members

    #endregion

    public WorldManager()
    {

    }

    public void Update(GameTime gameTime)
    {

    }

    public void Draw(SpriteBatch spriteBatch)
    {

    }

    /// <summary>
    /// Gets the tile at the given coordinate in the cluster
    /// </summary>
    /// <param name="cluster">Cluster to search</param>
    /// <param name="coord">Coordinate to use</param>
    /// <returns>Tell me if you see this</returns>
    private Tile GetTileAt(Cluster cluster, Coordinates coord)
    {
      return cluster[coord.X * coord.Y + coord.X];
    }
  }
}
