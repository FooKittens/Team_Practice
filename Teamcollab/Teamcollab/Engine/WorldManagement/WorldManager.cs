﻿using System;
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
  public sealed class WorldManager
  {
    #region Properties
    
    #endregion

    #region Matrices


    /// <summary>
    /// Transforms cluster coordinates into tile coordinates.
    /// Example: Cluster(-1, 0) becomes (-1 * ClusterWidth, 0 * ClusterHeight).
    /// </summary>
    private static Matrix ClusterTileTransform { get; set; }

    /// <summary>
    /// Transforms TileCoordinates to ScreenCoordinates.
    /// TileCoordinates should be cluster relative.
    /// Example: Tile(-10, 0) becomes (-10 * TileWidth, 0 * TileHeight).
    /// </summary>
    private static Matrix TileScreenTransform { get; set; }

    /// <summary>
    /// Transforms TileCoordinates into ClusterCoordinates.
    /// TileCoordinates should be world relative.
    /// Example: Tile(-512, 0) becomes (-512 / ClusterWidth, 0 / ClusterHeight)
    /// </summary>
    private static Matrix TileClusterTransform { get; set; }

    /// <summary>
    /// Transforms ScreenCoordinates into TileSpace.
    /// ScreenCoordinates should be transformed by a view matrix(camera).
    /// Example: Screen(-100, 0) becomes (-100 / TileWidth, 0)
    /// </summary>
    private static Matrix ScreenTileTransform { get; set; }

    /// <summary>
    /// Transforms ScreenCordinates into ClusterSpace.
    /// ScreenCoordinates should be transformed by a view matrix(camera).
    /// Example: Screen(-100, 0) becomes((-100 / TileWidth) / ClusterWidth, 0). 
    /// </summary>
    private static Matrix ScreenClusterTransform { get; set; }

    /// <summary>
    /// Offsets a TilePosition in TileCoordinates by half a tile.
    /// Example: Tile(0, 0) becomes (0.5, 0.5)
    /// </summary>
    private static Matrix TilePositionTransform { get; set; }
    
    
    public static Matrix WorldPixelTransform { get; set;}

    public static Matrix PixelWorldTransform { get; set; }
    #endregion

    #region Members
    // Singleton instance
    private static WorldManager singleton;

    List<Cluster> clusters;
    ResourceCollection<Texture2D> tileTextures;
    #endregion

    #region Static Methods
    public static Vector2 GetTileScreenPosition(Vector2 tilePosition)
    {
      return Vector2.Transform(tilePosition, TileScreenTransform);
    }

    public static Vector2 TransformByCluster(Vector2 tilePosition, Vector2 clusterPosition)
    {
      return Vector2.Transform(
        tilePosition,
        Matrix.CreateTranslation(new Vector3(clusterPosition, 0)) *
        ClusterTileTransform
      );
    }

    public static Vector2 GetTileCenterWorld(Vector2 tilePosition, Vector2 clusterPosition)
    {
      return GetTileScreenPosition(tilePosition) +
        TransformByCluster(tilePosition, clusterPosition);
    }

    #endregion

    static WorldManager()
    {
      CreateMatrices();
    }

    private WorldManager(Game game)
    {
      tileTextures = new ResourceCollection<Texture2D>();
      tileTextures.Add("Grass", game.Content.Load<Texture2D>("square"));

      Initialize();
    }

    /// <summary>
    /// Returns the single WorldManager instance that exists.
    /// </summary>
    /// <param name="game">Necessary for instancing the singleton.</param>
    /// <returns>The singleton instance.</returns>
    public static WorldManager GetInstance(Game game)
    {
      if (singleton == null)
      {
        singleton = new WorldManager(game);
      }

      return singleton;
    }

    /// <summary>
    /// Initializes the worldmanager instance.
    /// </summary>
    private void Initialize()
    {
      clusters = new List<Cluster>();
      AddCluster(new Coordinates(0, 0));
    }

    public void Update(GameTime gameTime)
    {

    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (IsInView(clusters[0]) == false)
        return;

      for (int y = 0; y < Constants.ClusterHeight; ++y)
      {
        for (int x = 0; x < Constants.ClusterWidth; ++x)
        {
          Vector2 origin = new Vector2(16, 16);

          Vector2 tPos = GetTileAt(clusters[0], x, y).Position;

          spriteBatch.Draw(tileTextures.Query("Grass"), Vector2.Transform(tPos, WorldPixelTransform), null, Color.White, 0f, origin, 1, SpriteEffects.None, 0f);
        }
      }
    }

    /// <summary>
    /// Creates a set of default matrices for the world manager.
    /// </summary>
    private static void CreateMatrices()
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

      PixelWorldTransform = Matrix.CreateScale(1f / Constants.WorldPixelRatio);
      WorldPixelTransform = Matrix.CreateScale(Constants.WorldPixelRatio);
    }

    /// <summary>
    /// Determines if a given cluster is in view.
    /// </summary>
    /// <param name="cluster">The cluster to investigate.</param>
    private bool IsInView(Cluster cluster)
    {
      Vector2 topLeft = new Vector2(Camera2D.Bounds.Left, Camera2D.Bounds.Top);
      Vector2 bottomRight = new Vector2(Camera2D.Bounds.Right, Camera2D.Bounds.Bottom);

      topLeft = Vector2.Transform(topLeft, ScreenClusterTransform);
      bottomRight = Vector2.Transform(bottomRight, ScreenClusterTransform);

      // Offsets are in cluster coordinates.
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

      Matrix tileTranslate = 
        TilePositionTransform *
        TileClusterTransform *
        clusterOffset *
        ClusterTileTransform
        ;

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
      return cluster.Tiles[y * Constants.ClusterWidth + x];
    }
  }
}
