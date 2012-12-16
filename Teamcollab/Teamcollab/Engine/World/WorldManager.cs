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
    static readonly Matrix ScreenClusterTransform;

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
        1f / Constants.TileWidth,
        1f / Constants.TileHeight,
        1
      ) * Matrix.CreateTranslation(new Vector3(-0.5f, -0.5f, 0));


      ScreenClusterTransform =
        /*Matrix.CreateTranslation(new Vector3(-0.5f, -0.5f, 0)) **/
        Matrix.CreateScale(1f / Constants.ClusterWidth, 1f / Constants.ClusterHeight, 1) * 
        Matrix.CreateScale(1f / Constants.TileWidth, 1f / Constants.TileWidth, 1);

      
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
      if (IsInView(clusters[0]) == false)
        return;

      for (int y = YClusterStart; y < YClusterEnd; ++y)
      {
        for (int x = XClusterStart; x < XClusterEnd; ++x)
        {
          if (HasMouse(GetTileAt(clusters[0], x, y))) continue;

          Vector2 v = new Vector2(x, y);
          v = Vector2.Transform(v, GetTileSpaceMatrix(clusters[0]));
          v = Vector2.Transform(v, TileScreenTransform);
          //v = Vector2.Transform(v, View);

          spriteBatch.Draw(tileTextures.Query("Grass"), v, Color.White);

        }
      }
    }

    private bool HasMouse(Tile t)
    {
      Vector2 mPos = InputManager.MousePosition();
      
      //mPos.X -= 32 * Math.Sign(mPos.X);
      mPos = Camera2D.TranslatePositionByCamera(mPos);
      //mPos -= new Vector2(16, 16);
      //mPos = Vector2.Transform(mPos, Camera2D.Transform);
      mPos = Vector2.Transform(mPos, ScreenTileTransfrom);
      //mPos /= 2

      mPos.X = Convert.ToInt32(mPos.X);
      mPos.Y = Convert.ToInt32(mPos.Y);

      if (mPos.X == t.Coordinates.X && mPos.Y == t.Coordinates.Y)
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

      Matrix trans = TileClusterTransform * ScreenTileTransfrom;

      topLeft = Vector2.Transform(topLeft, ScreenClusterTransform);
      bottomRight = Vector2.Transform(bottomRight, ScreenClusterTransform);

      //topLeft = Vector2.Transform(topLeft, ScreenTileTransfrom);
      //bottomRight = Vector2.Transform(bottomRight, ScreenTileTransfrom);
      //topLeft = Vector2.Transform(topLeft, TileClusterTransform);
      //bottomRight = Vector2.Transform(bottomRight, TileClusterTransform);

      Vector2 v = Vector2.Transform(cluster.Coordinates, ClusterTileTransform);
      v = cluster.Coordinates;
      
      if (v.X >= topLeft.X &&
          v.X <= bottomRight.X &&
          v.Y <= bottomRight.Y &&
          v.Y >= topLeft.Y)
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
      for (int y = YClusterStart; y < YClusterEnd; ++y)
      {
        for (int x = XClusterStart; x < XClusterEnd; ++x)
        {
          Tile t = GetTileAt(cluster, x, y);
          t.Coordinates = new Coordinates(x, y);
          t.Type = TileType.Grass;
          cluster.Tiles[(y + 4) * Constants.ClusterWidth + (x + 4)] = t;
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
