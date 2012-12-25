using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Teamcollab.Engine.WorldManagement;
using Teamcollab.Engine.Helpers;
using Microsoft.Xna.Framework;

namespace Teamcollab.Engine.WorldGeneration
{
  class TerrainGenerator
  {
    #region Properties

    #endregion

    const float GenPersistance2D = 0.75f;
    const int GenOctaves = 5;

    #region Members
    static  PerlinGenerator gen;
    #endregion

    static TerrainGenerator()
    {
      gen = new PerlinGenerator();
    }

    public static Cluster CreateCluster(int clusterX, int clusterY)
    {
      Cluster cluster = new Cluster(ClusterType.Evergreen, clusterX, clusterY);

      Vector2 vClusterPos = new Vector2(clusterX, clusterY);

      for (int y = 0; y < Constants.ClusterHeight; ++y)
      {
        for (int x = 0; x < Constants.ClusterWidth; ++x)
        {
          Vector2 tileWorldPos = new Vector2(
            x - Constants.ClusterWidth / 2,
            y - Constants.ClusterHeight / 2
          );
          tileWorldPos = WorldManager.TransformByCluster(tileWorldPos, vClusterPos);

          Vector2 noisePos = new Vector2(x, y);
          noisePos = WorldManager.TransformByCluster(noisePos, vClusterPos);

          float noise = GenerateTileNoise(noisePos.X, noisePos.Y);
          TileType type = TileFromNoise(noise);
          Tile t = Tile.Create(type);
          
          t.Coordinates = new Coordinates((int)tileWorldPos.X, (int)tileWorldPos.Y);
          cluster.SetTileAt(x, y, t);
        }
      }

      return cluster;
    }

    public static Cluster CreateCluster(Coordinates coords)
    {
      return CreateCluster(coords.X, coords.Y);
    }

    private static TileType TileFromNoise(float noise)
    {
      TileType type;
      if (noise >= -0.55f)
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
      // Div by 128f is a resolution factor for testing.
      return gen.Perlin2D(worldX / 128f, worldY / 128f, GenPersistance2D, GenOctaves);
    }
  }
}
