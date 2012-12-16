using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Teamcollab.Engine.Helpers;
using Teamcollab.Resources;
using Microsoft.Xna.Framework.Input;

namespace Teamcollab.Engine.World
{
  sealed class WorldManager
  {
    #region Properties

    #endregion

    #region Members
    List<Cluster> clusters;
    ResourceCollection<Texture2D> tileTextures;
    #endregion

    #region Constants
    const int XClusterStart = -Constants.ClusterWidth / 2;
    const int YClusterStart = -Constants.ClusterHeight / 2;
    const int XClusterEnd = Constants.ClusterWidth / 2;
    const int YClusterEnd = Constants.ClusterHeight / 2;

    static readonly Matrix ClusterTileTransform;
    static readonly Matrix TileScreenTransform;
    static readonly Matrix TileClusterTransform;
    static readonly Matrix ScreenTileTransfrom;

    static Matrix View;
    #endregion

    static WorldManager()
    {
      ClusterTileTransform = Matrix.CreateScale(
        Constants.ClusterWidth,
        Constants.ClusterHeight,
        1
      );

      TileScreenTransform = Matrix.CreateScale(
        Constants.TileWidth,
        Constants.TileHeight,
        1
      );

      TileClusterTransform = Matrix.CreateScale(
        1f / Constants.ClusterWidth,
        1f / Constants.ClusterHeight,
        1
      );

      ScreenTileTransfrom = Matrix.CreateScale(
        1f / Constants.ClusterWidth,
        1f / Constants.ClusterHeight,
        1
      );

      View = Matrix.CreateOrthographicOffCenter(
        -Settings.ScreenWidth / 2,
        Settings.ScreenWidth / 2,
        Settings.ScreenHeight / 2,
        -Settings.ScreenHeight / 2,
        0,
        1
      );

      View = Matrix.CreateTranslation(
        new Vector3(
          Settings.ScreenWidth / 2,
          Settings.ScreenHeight / 2,
          0
          )
        );

    }

    public WorldManager(Game game)
    {
      tileTextures = new ResourceCollection<Texture2D>();
      tileTextures.Add("Grass", game.Content.Load<Texture2D>("grass32x32"));

      Initialize();
    }

    public void Initialize()
    {
      clusters = new List<Cluster>();
      AddCluster(new Coordinates(0, 0));
    }

    public void Update(GameTime gameTime)
    {
      // TODO(Zerkish): Remove useless input.

      if (InputManager.KeyDown(Keys.Left))
      {
        View *= Matrix.CreateTranslation(new Vector3(-1, 0, 0));
      }

      if (InputManager.KeyDown(Keys.Right))
      {
        View *= Matrix.CreateTranslation(new Vector3(1, 0, 0));
      }

      if (InputManager.KeyDown(Keys.Up))
      {
        View *= Matrix.CreateTranslation(new Vector3(0, -1, 0));
      }

      if (InputManager.KeyDown(Keys.Down))
      {
        View *= Matrix.CreateTranslation(new Vector3(0, 1, 0));
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      for (int y = YClusterStart; y < YClusterEnd; ++y)
      {
        for (int x = XClusterStart; x < XClusterEnd; ++x)
        {
          Vector2 v = new Vector2(x, y);
          v = Vector2.Transform(v, GetTileSpaceMatrix(clusters[0]));
          v = Vector2.Transform(v, TileScreenTransform);
          v = Vector2.Transform(v, View);

          spriteBatch.Draw(tileTextures.Query("Grass"), v, Color.White);

        }
      }
    }

    private Matrix GetTileSpaceMatrix(Cluster cluster)
    {
      return Matrix.CreateTranslation(Vector3.Transform(new Vector3(cluster.Coordinates, 0), ClusterTileTransform));
    }


    /// <summary>
    /// Insert a cluster at the given coordinates in cluster space.
    /// </summary>
    /// <param name="clusterCoordinates"></param>
    private void AddCluster(Coordinates clusterCoordinates)
    {
      Cluster cluster = new Cluster(ClusterType.Evergreen, clusterCoordinates);
      for (int y = YClusterStart; y < YClusterEnd; ++y)
      {
        for (int x = XClusterStart; x < XClusterEnd; ++x)
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
      int xOffset = (x + Constants.ClusterWidth / 2);
      int yOffset = (y + Constants.ClusterHeight / 2);
      return cluster.Tiles[yOffset * Constants.ClusterWidth + xOffset];
    }
  }
}
