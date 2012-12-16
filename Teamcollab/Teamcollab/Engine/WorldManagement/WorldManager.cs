using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Teamcollab.Engine.Helpers;
using Teamcollab.Resources;
using Microsoft.Xna.Framework.Input;
using Teamcollab.GUI;

namespace Teamcollab.Engine.WorldManagement
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
    static readonly Matrix ClusterTileTransform;
    static readonly Matrix TileScreenTransform;
    static readonly Matrix TileClusterTransform;
    
    static readonly Matrix ScreenTileTransform;
    static readonly Matrix ScreenClusterTransform;

    static readonly Matrix TilePositionTransform;

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

      ScreenTileTransform = Matrix.CreateScale(
        1f / Constants.TileWidth,
        1f / Constants.TileHeight,
        1
      );

      TilePositionTransform =
      Matrix.CreateTranslation(new Vector3(0.5f, 0.5f, 0));

      ScreenClusterTransform = 
        ScreenTileTransform * 
        TilePositionTransform *
        TileClusterTransform;

    
      
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
      tileTextures.Add("Grass", game.Content.Load<Texture2D>("square"));

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
      if (IsInView(clusters[0]) == false)
        return;

      for (int y = 0; y < Constants.ClusterHeight; ++y)
      {
        for (int x = 0; x < Constants.ClusterWidth; ++x)
        {
          if (HasMouse(GetTileAt(clusters[0], x, y))) continue;

          //Vector2 v = new Vector2(x, y);
          //v = Vector2.Transform(v, GetTileSpaceMatrix(clusters[0]));
          //v = Vector2.Transform(v, TileScreenTransform);
          //v = Vector2.Transform(v, View);
          Vector2 origin = new Vector2(16, 16);

          spriteBatch.Draw(tileTextures.Query("Grass"), GetTileAt(clusters[0], x, y).Position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
        }
      }
    }

    private bool HasMouse(Tile t)
    {
      Vector2 mPos = InputManager.MousePosition();
      
      //mPos.X -= 32 * Math.Sign(mPos.X);
      mPos = Camera2D.TranslatePositionByCamera(mPos);
      mPos /= 32;
      //mPos = Vector2.Transform(mPos, ScreenTileTransfrom);

      mPos.X = Convert.ToInt32(mPos.X - 0.5f);
      mPos.Y = Convert.ToInt32(mPos.Y - 0.5f);


      if (mPos.X == t.Position.X && mPos.Y == t.Position.Y)
      {
        return true;
      }
      
      return false;

    }

    private Matrix GetTileSpaceMatrix(Cluster cluster)
    {
      return Matrix.CreateTranslation(Vector3.Transform(new Vector3(cluster.Coordinates, 0), ClusterTileTransform));
    }

    private bool IsInView(Cluster cluster)
    {
      Vector2 topLeft = new Vector2(Camera2D.Bounds.Left, Camera2D.Bounds.Top);
      Vector2 bottomRight = new Vector2(Camera2D.Bounds.Right, Camera2D.Bounds.Bottom);

      topLeft = Vector2.Transform(topLeft, ScreenClusterTransform);
      bottomRight = Vector2.Transform(bottomRight, ScreenClusterTransform);

      if (cluster.Coordinates.X + 0.5f >= topLeft.X &&
          cluster.Coordinates.X - 0.5f <= bottomRight.X &&
          cluster.Coordinates.Y - 0.5f <= bottomRight.Y &&
          cluster.Coordinates.Y + 0.5f >= topLeft.Y)
      {
        return true;
      }

      return false;
    }


    /// <summary>
    /// Insert a cluster at the given coordinates in cluster space.
    /// </summary>
    /// <param name="clusterCoordinates"></param>
    private void AddCluster(Coordinates clusterCoordinates)
    {
      Cluster cluster = new Cluster(ClusterType.Evergreen, clusterCoordinates);

      Matrix clusterOffset = Matrix.CreateTranslation(new Vector3(-0.5f, -0.5f, 0));

      Matrix tileTranslate = TileClusterTransform * clusterOffset * ClusterTileTransform * TileScreenTransform;

      for (int y = 0; y < Constants.ClusterHeight; ++y)
      {
        for (int x = 0; x < Constants.ClusterWidth; ++x)
        {
          Vector2 tilePos = Vector2.Transform(new Vector2(x, y), tileTranslate);          

          Tile t = GetTileAt(cluster, x, y);
          t.Position = tilePos;
          t.Type = TileType.Grass;
          cluster.Tiles[y * Constants.ClusterWidth + x] = t;
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
      return cluster.Tiles[y * Constants.ClusterWidth + x];
    }
  }
}
