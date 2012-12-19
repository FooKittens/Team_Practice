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

    World currentWorld;
    ResourceCollection<Texture2D> tileTextures;
    #endregion

    #region Static Methods
    public static Vector2 GetTileScreenPosition(Vector2 tilePosition)
    {
      return Vector2.Transform(
        tilePosition,
        TilePositionTransform * TileScreenTransform
      );
    }

    public static Vector2 TransformByCluster(Vector2 tilePosition,
      Vector2 clusterPosition)
    {
      Vector2 cTranslate = Vector2.Transform(
        clusterPosition,
        ClusterTileTransform
      );

      Vector2 res =  Vector2.Transform(
        tilePosition + cTranslate,
        TilePositionTransform *
        TileScreenTransform
      );

      return res;
    }

    public static Vector2 GetClusterScreenCenter(Vector2 clusterCoordinates)
    {
      return Vector2.Transform(
        clusterCoordinates,
        ClusterTileTransform * TileScreenTransform
      );
    }

    #endregion

    // DEBUG TEST CONSTANTS
    const int WorldWidth = 74;
    const int WorldHeight = 74;

    static WorldManager()
    {
      CreateMatrices();
    }

    private WorldManager(Game game)
    {
      tileTextures = new ResourceCollection<Texture2D>();
      tileTextures.Add("Grass", game.Content.Load<Texture2D>("square"));

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
      World world = new World();

      for (int y = top; y <= bottom; ++y)
      {
        for (int x = left; x <= right; ++x)
        {
          AddCluster(world, new Coordinates(x, y));
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
     
    /// <summary>
    /// Initializes the worldmanager instance.
    /// </summary>
    private void Initialize(World world)
    {
      currentWorld = world;
      grassText = tileTextures.Query("Grass");
    }

    public void Update(GameTime gameTime)
    {

    }

    Resource<Texture2D> grassText;
    public void Draw(SpriteBatch spriteBatch)
    {
      for (int y = -WorldHeight / 2; y <= WorldHeight / 2; ++y)
      {
        for (int x = -WorldWidth / 2; x <= WorldWidth / 2; ++x)
        {
          Cluster cluster = currentWorld.GetCluster(x, y);

          // Test with cluster bounds
          if (GetClusterBounds(cluster).Intersects(Camera2D.Bounds) == false)
          {
            continue;
          }

          for (int tileY = 0; tileY < Constants.ClusterHeight; ++tileY)
          {
            for (int tileX = 0; tileX < Constants.ClusterWidth; ++tileX)
            {
              Vector2 origin = new Vector2(16, 16);

              Vector2 tPos = GetTileAt(cluster, tileX, tileY).Position;

              Vector2 transformed = TransformByCluster(tPos, cluster.Coordinates);

              spriteBatch.Draw(grassText, transformed, null, Color.White, 0f, origin, 1, SpriteEffects.None, 0f);
            }
          }
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
    /// Retrieves the bounding rectangle for a cluster in pixels.
    /// </summary>
    /// <param name="cluster">Cluster to get bounds from.</param>
    private static Rectangle GetClusterBounds(Cluster cluster)
    {
      // Creates cluster edges with clockwise winding.
      Vector2[] vertices = new[] {
        new Vector2(-0.5f, -0.5f), // Top Left
        new Vector2(0.5f, -0.5f), // Top Right
        new Vector2(0.5f, 0.5f), // Bottom Right
        new Vector2(-0.5f, 0.5f) // Bottom Left
      };

      // Matrix for translating into tilespace and then scaling to screen.
      Matrix mat = ClusterTileTransform * TileScreenTransform;

      /* Translate all vertices to the correct cluster and transform
       * into pixel coordinates. */
      for (int i = 0; i < vertices.Length; ++i)
      {
        vertices[i] += cluster.Coordinates;
        vertices[i] = Vector2.Transform(vertices[i], mat);
      }

      // Bounding Rectangle.
      Rectangle rect = new Rectangle(
        (int)vertices[0].X,
        (int)vertices[0].Y,
        (int)(vertices[1].X - vertices[0].X),
        (int)(vertices[2].Y - vertices[1].Y)        
      );

      return rect;
    }

    /// <summary>
    /// Insert a cluster at the given coordinates in cluster space.
    /// </summary>
    /// <param name="clusterCoordinates"></param>
    private static void AddCluster(World world, Coordinates clusterCoordinates)
    {
      Cluster cluster = new Cluster(ClusterType.Evergreen, clusterCoordinates);

      Matrix clusterOffset = Matrix.CreateTranslation(new Vector3(-0.5f, -0.5f, 0));

      Matrix tileTranslate = 
        /*TilePositionTransform **/
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

      world.AddCluster(cluster);
    }

    /// <summary>
    /// Gets the tile at the given coordinates in the cluster
    /// </summary>
    /// <param name="cluster">Cluster to search</param>
    /// <param name="coord">Coordinate to use</param>
    /// <returns>Tell me if you see this</returns>
    private static Tile GetTileAt(Cluster cluster, Coordinates coord)
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
    private static Tile GetTileAt(Cluster cluster, int x, int y)
    {
      return cluster.Tiles[y * Constants.ClusterWidth + x];
    }
  }
}
