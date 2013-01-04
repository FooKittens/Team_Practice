using Microsoft.Xna.Framework;
using Midgard.Engine.Helpers;
using Midgard.Engine.WorldManagement;
using Midgard.GameObjects;
using System;

namespace Midgard.Engine.WorldGeneration
{
  class TerrainGenerator
  {
    #region Properties

    #endregion

    const float GenPersistance2D = 0.75f;
    const int GenOctaves = 5;

    #region Members
    static PerlinGenerator landGen;
    static int seed;
    #endregion

    static TerrainGenerator()
    {

    }

    public static void Initialize(int seed)
    {
      TerrainGenerator.seed = seed;
      landGen = new PerlinGenerator(seed, GenOctaves, GenPersistance2D);
    }

    public static Cluster CreateCluster(int clusterX, int clusterY)
    {
      Cluster cluster = new Cluster(ClusterType.Evergreen, clusterX, clusterY);

      Vector2 vClusterPos = new Vector2(clusterX, clusterY);

      EntityManager em = EntityManager.GetInstance();

      for (int y = 0; y < Constants.ClusterHeight; ++y)
      {
        for (int x = 0; x < Constants.ClusterWidth; ++x)
        {
          Vector2 tileWorldPos = new Vector2(
            x - Constants.ClusterWidth / 2,
            y - Constants.ClusterHeight / 2
          );
          tileWorldPos = WorldManager.TransformByCluster(
            tileWorldPos, vClusterPos
          );

          Vector2 noisePos = new Vector2(x, y);
          noisePos = WorldManager.TransformByCluster(noisePos, vClusterPos);

          float noise = GenerateTileNoise(noisePos.X, noisePos.Y);
          TileType type = TileFromNoise(noise);
          Tile t = Tile.Create(type);

          t.Coordinates = new Point2D((int)tileWorldPos.X,
            (int)tileWorldPos.Y)
          ;
          cluster.SetTileAt(x, y, t);
        }
      }

      return cluster;
    }

    public static Cluster CreateCluster(Point2D coords)
    {
      return CreateCluster(coords.X, coords.Y);
    }

    private static TileType TileFromNoise(float noise)
    {
      TileType type;
      if (noise >= -1.4f)
      {
        type = TileType.Grass;
      }
      else
      {
        type = TileType.Water;
      }

      return type;
    }

    private static float GenerateTileNoise(float worldX, float worldY)
    {
      // Div by 32f is a resolution factor for testing.
      return landGen.Perlin2D(worldX / 32f, worldY / 32f);
    }      
  }
}
