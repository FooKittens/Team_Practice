using Teamcollab.Engine.Helpers;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Teamcollab.Resources;
using System.Runtime.InteropServices;
using System.IO.Compression;
using Teamcollab.Engine.DataManagement;

namespace Teamcollab.Engine.WorldManagement
{

  public enum ClusterType : byte
  {
    Undefined = 0,

    Evergreen,
  }

  [Serializable]
  public struct ClusterData
  {
    public ClusterType Type;
    public Tile[] Tiles;
    public Coordinates Coordinates;
  }

  /// <summary>
  /// Handles a collection of Tiles
  /// </summary>
  [Serializable]
  public class Cluster
  {
    #region Properties
    public long HashCode { get; private set; }
    public ClusterType Type { get; private set; }
    public bool Active { get; private set; }
    public bool Loaded { get; private set; }
    
    #endregion

    #region Members
    private Tile[] tiles;
    public Coordinates Coordinates;
    #endregion

    static ResourceCollection<Texture2D> tileTextures;

    // Static Constructor for loading tiletextures initially.
    static Cluster()
    {
      tileTextures = ResourceManager.TileTextureBank;
    }

    public Cluster(ClusterType type, int x, int y)
    {
      Type = type;
      tiles = new Tile[Constants.ClusterWidth * Constants.ClusterHeight];
      Active = false;
      Coordinates = new Coordinates(x, y);
      HashCode = GetHashCode();
    }

    public Cluster(ClusterType type, Coordinates coordinates)
      :this(type, coordinates.X, coordinates.Y) { }

    public Cluster(ClusterData data)
    {
      Type = data.Type;
      tiles = data.Tiles;
      Coordinates = data.Coordinates;

      SetHashCode();
      Active = false;
    }

    public void Unload()
    {
      Loaded = false;
      tiles = null;
      squareRes = null;
    }

    public void Load(ClusterData data)
    {
      if (HashCode != GetHashFromXY(data.Coordinates.X, data.Coordinates.Y))
      {
        throw new Exception("Data does not match the cluster");
      }
      squareRes = ResourceManager.TileTextureBank.Query("Grass");
      tiles = data.Tiles;

      SetHashCode();
    }

    // TODO REMOVE
    Resource<Texture2D> squareRes;
    Resource<Texture2D> grassRes;
    public void Draw(SpriteBatch spriteBatch)
    {
      // TODO REMOVE
      if (squareRes == null)
      {
        squareRes = tileTextures.Query("Square");
        grassRes = tileTextures.Query("Grass");
      }

      for (int y = 0; y < Constants.ClusterHeight; ++y)
      {
        for (int x = 0; x < Constants.ClusterWidth; ++x)
        {
          Tile tile = GetTileAt(x, y);

          Vector2 tilePos = new Vector2(
            x - Constants.ClusterWidth / 2,
            y - Constants.ClusterHeight / 2
          );

          Vector2 drawPos = WorldManager.TransformByCluster(tilePos, Coordinates);
          drawPos = WorldManager.GetTileScreenPosition(drawPos);

          Resource<Texture2D> texture = tile.Type == TileType.Water ? grassRes : squareRes;

          spriteBatch.Draw(texture, drawPos, null, Color.White, 0f, new Vector2(16, 16), 1f, SpriteEffects.None, 0f);
        }
      }
    }

    /// <summary>
    /// Retrieves the bounding rectangle for a cluster in pixels.
    /// </summary>
    /// <param name="cluster">Cluster to get bounds from.</param>
    public static Rectangle GetClusterBounds(Cluster cluster)
    {
      // Creates cluster edges with clockwise winding.
      Vector2[] vertices = new[] {
        new Vector2(-0.5f, -0.5f), // Top Left
        new Vector2(0.5f, -0.5f), // Top Right
        new Vector2(0.5f, 0.5f), // Bottom Right
        new Vector2(-0.5f, 0.5f) // Bottom Left
      };

      // Matrix for translating into tilespace and then scaling to screen.
      // Matrix mat = ClusterTileTransform * TileScreenTransform;

      /* Translate all vertices to the correct cluster and transform
       * into pixel coordinates. */
      for (int i = 0; i < vertices.Length; ++i)
      {
        vertices[i] += cluster.Coordinates;
        vertices[i] = WorldManager.GetClusterScreenCenter(vertices[i]);

        //vertices[i] = Vector2.Transform(vertices[i], mat);
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

    public void SetTileAt(int x, int y, Tile newTile)
    {
      tiles[y * Constants.ClusterWidth + x] = newTile;
    }

    public void SetTileAt(Coordinates coord, Tile newTile)
    {
      SetTileAt(coord.X, coord.Y, newTile);
    }

    /// <summary>
    /// Retrieves the tile at the input coordinates relative
    /// to the cluster.
    /// </summary>
    public Tile GetTileAt(int x, int y)
    {
      return tiles[y * Constants.ClusterWidth + x];
    }

    /// <summary>
    /// Retrieves the tile at the input coordinates relative
    /// to the cluster.
    /// </summary>
    private Tile GetTileAt(Coordinates coord)
    {
      return GetTileAt(coord.X, coord.Y);
    }

    public override string ToString()
    {
      return string.Format("({0}, {1})", Coordinates.X, Coordinates.Y);
    }

    /// <summary>
    /// Tells the Cluster to generate its hashcode.
    /// </summary>
    public void SetHashCode()
    {
      HashCode = GetHashFromXY(Coordinates.X, Coordinates.Y);
    }

    /// <summary>
    /// Retrieve a hashcode using the same algorithm as
    /// the cluster uses to generate its hashcode from 
    /// two coordinates.
    /// </summary>
    public static long GetHashFromXY(int x, int y)
    {
      long hash = ((long)x << 32) + y;
      return hash;
    }

    /// <summary>
    /// Gets the cluster as a byte array.
    /// </summary>
    public byte[] GetData()
    {
      const int clusterLength = Constants.ClusterWidth * Constants.ClusterHeight;

      // 1 for cluster type, 8 for 2 ints in coordinates.
      const int dataSize = 1 + clusterLength + 8;

      byte[] data = new byte[dataSize];
      int dataIndex = 0;
      data[dataIndex++] = (byte)Type;

      for (int i = 0; i < clusterLength; ++i)
      {
        data[dataIndex++] = (byte)tiles[i].Type;
      }

      byte[] coordBytes = new byte[8];
      for (int i = 0; i < 4; ++i)
      {
        coordBytes[i] = BitConverter.GetBytes(Coordinates.X)[i];
      }
      for (int i = 4; i < 8; ++i)
      {
        coordBytes[i] = BitConverter.GetBytes(Coordinates.Y)[i - 4];
      }

      for (int i = 0; i < coordBytes.Length; ++i)
      {
        data[dataIndex++] = coordBytes[i];
      }

      return CompressionHelper.Compress(data);
    }

    
  }
}