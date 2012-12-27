using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Teamcollab.Engine.Helpers;
using Teamcollab.Resources;
using Teamcollab.Engine.WorldGeneration;
using Teamcollab.Engine.DataManagement;
using System.IO;

namespace Teamcollab.Engine.WorldManagement
{
  public sealed class WorldManager
  {
    #region Properties
    public World CurrentWorld { get { return currentWorld; } }
    #endregion
    
    #region Matrices

    /// <summary>
    /// Transforms cluster coordinates into tile coordinates.
    /// Example: Cluster(-1, 0) becomes (-1 * ClusterWidth, 0 * ClusterHeight).
    /// </summary>
    private static Matrix clusterTileTransform;

    /// <summary>
    /// Transforms TileCoordinates to ScreenCoordinates.
    /// TileCoordinates should be cluster relative.
    /// Example: Tile(-10, 0) becomes (-10 * TileWidth, 0 * TileHeight).
    /// </summary>
    private static Matrix tileScreenTransform;

    /// <summary>
    /// Transforms TileCoordinates into ClusterCoordinates.
    /// TileCoordinates should be world relative.
    /// Example: Tile(-512, 0) becomes (-512 / ClusterWidth, 0 / ClusterHeight)
    /// </summary>
    private static Matrix tileClusterTransform;

    /// <summary>
    /// Transforms ScreenCoordinates into TileSpace.
    /// ScreenCoordinates should be transformed by a view matrix(camera).
    /// Example: Screen(-100, 0) becomes (-100 / TileWidth, 0)
    /// </summary>
    private static Matrix screenTileTransform;

    /// <summary>
    /// Transforms ScreenCordinates into ClusterSpace.
    /// ScreenCoordinates should be transformed by a view matrix(camera).
    /// Example: Screen(-100, 0) becomes((-100 / TileWidth) / ClusterWidth, 0). 
    /// </summary>
    private static Matrix screenClusterTransform;

    /// <summary>
    /// Offsets a TilePosition in TileCoordinates by half a tile.
    /// Example: Tile(0, 0) becomes (0.5, 0.5)
    /// </summary>
    private static Matrix tilePositionTransform;

    private static Matrix isometricTransform;

    private static Matrix invIsometricTransform;

    public static Matrix WorldPixelTransform { get; set;}

    public static Matrix PixelWorldTransform { get; set; }
    #endregion

    #region Members
    // Singleton instance
    private static WorldManager singleton;

    World currentWorld;
    #endregion

    #region Static Methods
    //TODO(Martin): Remove as many matrices as possible!
    public static Vector2 TransformByCluster(Vector2 tilePosition,
      Vector2 clusterPosition)
    {
      Vector2 cTranslate = Vector2.Transform(
        clusterPosition,
        clusterTileTransform
      );

      Vector2 res =  Vector2.Transform(
        tilePosition + cTranslate,
        tilePositionTransform
      );

      return res;
    }

    public static Vector2 GetClusterScreenCenter(Vector2 clusterCoordinates)
    {
      return Vector2.Transform(
        clusterCoordinates,
        clusterTileTransform * tileScreenTransform
      );
    }

    public static Vector2 TransformScreenToCluster(Vector2 screenCoordinates)
    {
      return Vector2.Transform(
        screenCoordinates,
        screenClusterTransform
      );
    }

    public static Vector2 TransformIsometric(Vector2 screenCoordinates)
    {
      return Vector2.Transform(screenCoordinates, isometricTransform);
    }

    public static Vector2 TransformInvIsometric(Vector2 isoCoords)
    {
      return Vector2.Transform(isoCoords, invIsometricTransform);
    }

    public static Vector2 TransformWorldToScreen(Vector2 worldCoords)
    {
      return Vector2.Transform(worldCoords, tileScreenTransform);
    }

    public static Vector2 TransformScreenToWorld(Vector2 screenCoordinates)
    {
      return Vector2.Transform(
        screenCoordinates,
        screenTileTransform
      );
    }

    #endregion

    // DEBUG TEST CONSTANTS
    const int WorldWidth = 10;
    const int WorldHeight = 10;

    static WorldManager()
    {
      CreateMatrices();
      TerrainGenerator.Initialize(1337);
    }

    private WorldManager(Game game)
    {
      try
      {
        DataManager.LoadWorld("test");
      }
      catch (FileNotFoundException)
      {
        // Om nom nom
      }
      Initialize(WorldManager.CreateWorld(WorldWidth, WorldHeight));
    }

    /// <summary>
    /// Creates a world with a set of initial clusters as defined
    /// by the parameters.
    /// </summary>
    /// <param name="left">Max left cluster coordinate.</param>
    /// <param name="right">Max right cluster coordinate.</param>
    /// <param name="top">Max top cluster coordinate.</param>
    /// <param name="bottom">Max bottom cluster coordinate.</param>
    /// <returns></returns>
    public static World CreateWorld(int left, int right, int top, int bottom)
    {
      World world = new World((right - left) * (bottom - top));

      for (int y = top; y <= bottom; ++y)
      {
        for (int x = left; x <= right; ++x)
        {
          //world.AddCluster(world.CreateCluster(x, y));
          //AddCluster(world, new Coordinates(x, y));
        }
      }

      return world;
    }

    /// <summary>
    /// Creates a world with the specified width and height.
    /// World will be centered on 0,0.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static World CreateWorld(int width, int height)
    {
      return CreateWorld(-width / 2, width / 2, -height / 2, height / 2);
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


    public static void Save()
    {
      DataManager.SaveWorld(GetInstance(null).CurrentWorld);
    }

    /// <summary>
    /// Initializes the worldmanager instance.
    /// </summary>
    private void Initialize(World world)
    {
      currentWorld = world;
    }

    public void Update(GameTime gameTime)
    {
      currentWorld.Update(gameTime);
    }

    public void Draw(IsoBatch spriteBatch)
    {

      currentWorld.Draw(spriteBatch);
      
    }

    /// <summary>
    /// Creates a set of default matrices for the world manager.
    /// </summary>
    private static void CreateMatrices()
    {
      clusterTileTransform = Matrix.CreateScale(
        Constants.ClusterWidth,
        Constants.ClusterHeight,
        1
      );

      tileScreenTransform = Matrix.CreateScale(
        Constants.TileWidth,
        Constants.TileHeight,
        1
      );

      tileClusterTransform = Matrix.CreateScale(
        1f / Constants.ClusterWidth,
        1f / Constants.ClusterHeight,
        1
      );

      screenTileTransform = Matrix.CreateScale(
        1f / Constants.TileWidth,
        1f / Constants.TileHeight,
        1
      );

      tilePositionTransform =
      Matrix.CreateTranslation(new Vector3(0.5f, 0.5f, 0));

      screenClusterTransform =
        screenTileTransform *
        tileClusterTransform;

      float sqTwo = (float)Math.Sqrt(2);

      isometricTransform =
        Matrix.CreateRotationZ(MathHelper.PiOver4) *
        Matrix.CreateScale(sqTwo / 2.0f, sqTwo / 2.0f, 1);

      invIsometricTransform =
        Matrix.CreateRotationZ(-MathHelper.PiOver4) *
        Matrix.CreateScale(sqTwo, sqTwo, 1f);

      PixelWorldTransform = Matrix.CreateScale(1f / Constants.WorldPixelRatio);
      WorldPixelTransform = Matrix.CreateScale(Constants.WorldPixelRatio);
    }

    /// <summary>
    /// Insert a cluster at the given coordinates in cluster space.
    /// </summary>
    /// <param name="clusterCoordinates"></param>
    private static void AddCluster(World world, Coordinates clusterCoordinates)
    {
      Cluster cluster = new Cluster(ClusterType.Evergreen, clusterCoordinates);

      Matrix clusterOffset = Matrix.CreateTranslation(
        new Vector3(-0.5f, -0.5f, 0)
      );

      Matrix tileTranslate = 
        tileClusterTransform *
        clusterOffset *
        clusterTileTransform
        ;

      for (int y = 0; y < Constants.ClusterHeight; ++y)
      {
        for (int x = 0; x < Constants.ClusterWidth; ++x)
        {
          Vector2 tilePos = Vector2.Transform(
            new Vector2(x, y), tileTranslate
          );

          Tile t = cluster.GetTileAt(x, y);
          t.Type = TileType.Grass;
          cluster.SetTileAt(x, y, t);
        }
      }

      world.AddCluster(cluster);
    }
  }
}
