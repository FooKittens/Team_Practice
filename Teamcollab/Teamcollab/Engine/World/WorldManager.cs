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
    List<Cluster> clusters;
    #endregion

    public WorldManager()
    {
      Initialize();
    }

    public void Initialize()
    {
      clusters = new List<Cluster>();
      AddCluster(new Coordinates());

      Vector2 test = Vector2.Transform(new Vector2(3, 3), GetTileSpaceMatrix(clusters[0]));
    }

    public void Update(GameTime gameTime)
    {

    }

    public void Draw(SpriteBatch spriteBatch)
    {

    }

    private Matrix GetTileSpaceMatrix(Cluster cluster)
    {
      return Matrix.CreateWorld(new Vector3(cluster.Coordinates, 0), -Vector3.UnitZ, Vector3.UnitY);
    }


    /// <summary>
    /// Insert a cluster at the given coordinates.
    /// </summary>
    /// <param name="clusterCoordinates"></param>
    private void AddCluster(Coordinates clusterCoordinates)
    {
      Cluster cluster = new Cluster(ClusterType.Evergreen, clusterCoordinates);
      for (int y = 0; y < Constants.ClusterHeight; ++y)
      {
        for (int x = 0; x < Constants.ClusterWidth; ++x)
        {
          Tile t = GetTileAt(cluster, x, y);
          t.Coordinates.X = x << Constants.TileMod;
          t.Coordinates.Y = y << Constants.TileMod;
          t.Type = TileType.Grass;
        }
      }

      clusters.Add(cluster);
    }

    /// <summary>
    /// Gets the tile at the given coordinates in the cluster
    /// </summary>
    /// <param name="cluster">Cluster to search</param>
    /// <param name="coord">Coordinate to use</param>
    /// <returns>Tell me if you see this</returns>
    private Tile GetTileAt(Cluster cluster, Coordinates coord)
    {
      return GetTileAt(cluster, coord.X, coord.Y);
    }

    /// <summary>
    /// Gets the tile at the given coordinates in the cluster.
    /// </summary>
    /// <param name="cluster">The cluster to retrieve from.</param>
    /// <param name="x">X-coordinate in tilecoordinates.</param>
    /// <param name="y">Y-coordinate in tilecoordinates.</param>
    /// <returns></returns>
    private Tile GetTileAt(Cluster cluster, int x, int y)
    {
      return cluster.Tiles[y * Constants.ClusterWidth + x];
    }

  }
}
