using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Teamcollab.Engine.WorldManagement;
using System.IO;
using System.Data;

namespace Teamcollab.Engine.DataManagement
{
  static class DataManager
  {
    enum SaveTag : byte
    {
      Undefined = 0,

      // Version // TODO(Peter): Implement

      #region world.dat
      Created,
      Seed,
      LastPlayed,
      Name,
      CurrentTime,
      #endregion

      End,

      #region cluster.dat
      ClusterType,
      ClusterX,
      ClusterY,
      TileData,

           
      #endregion
    }

    const string clusterString = "{0}_{1}.dat";
    const string savePath = "Saves\\{0}\\";
    const string worldDatPath = "world.dat";
    const string clustersPath = "clusters\\";

    #region Properties

    #endregion

    #region Members

    #endregion

    public static void SaveWorld(World world)
    {
      string path = string.Format(
        savePath, world.Name
      );

      string worldPath = path + worldDatPath;
      bool saveExists = false;

      if (Directory.Exists(path))
      {
        if (File.Exists(worldPath))
        {
          saveExists = true;
        }
      }
      else
      {
        if (!Directory.Exists("Saves"))
        {
          Directory.CreateDirectory("Saves");
        }
        // Create sub folders.
        Directory.CreateDirectory(path);
        Directory.CreateDirectory(path + clustersPath);
      }

      using (FileStream stream = File.Open(worldPath, FileMode.OpenOrCreate))
      {
        // Theres some data we wont need to change if the file exists.
        if (!saveExists)
        {
          // Save the name.
          SaveName(stream, world);

          // Save the Creation time.
          SaveLong(stream, world.CreationTimeTicks, SaveTag.Created);

          // Save the seed.
          stream.WriteByte((byte)SaveTag.Seed);
          byte[] seedBytes = BitConverter.GetBytes(world.Seed);
          stream.Write(BitConverter.GetBytes(seedBytes.Length), 0, sizeof(int));
          stream.Write(seedBytes, 0, seedBytes.Length);
        }

        // Save the last played date.
        SaveLong(stream, world.LastPlayedTimeTicks, SaveTag.LastPlayed);

        // Save the in-game time.
        SaveLong(stream, world.CurrentTimeTicks, SaveTag.CurrentTime);

        // Write the end tag 
        stream.WriteByte((byte)SaveTag.End);
      }


    }

    private static void SaveExisting(World world)
    {

    }

    /// <summary>
    /// Saves the name of a world at the current position of the stream.
    /// </summary>
    /// <param name="stream">An open filestream.</param>
    private static void SaveName(FileStream stream, World world)
    {
      stream.WriteByte((byte)SaveTag.Name);
      char[] nameChars = world.Name.ToCharArray();
      byte[] nameBytes = new byte[nameChars.Length * sizeof(char)];

      // Index for the actual nameByteArray.
      int bIndex = 0;

      /* Go through the chars in nameChars and convert to bytes,
       * then append them to nameBytes. */
      for (int i = 0; i < nameChars.Length; ++i)
      {
        byte[] bytes = BitConverter.GetBytes(nameChars[i]);
        for (int k = 0; k < bytes.Length; ++k)
        {
          nameBytes[bIndex++] = bytes[k];
        }
      }

      // Write the length of the bytes stored for the name.
      stream.Write(BitConverter.GetBytes(nameBytes.Length), 0, sizeof(int));

      // Write the name in bytes.
      stream.Write(nameBytes, 0, nameBytes.Length);
    }
    
    /// <summary>
    /// Saves a long to the input stream. 
    /// Method will also save the tag first, then an int with the length
    /// of the comming data and then finally the long itself.
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="data"></param>
    /// <param name="tag"></param>
    private static void SaveLong(FileStream stream, long data, SaveTag tag)
    {
      stream.WriteByte((byte)tag);

      byte[] lpbytes = BitConverter.GetBytes(data);

      // Write how many bytes are comming.
      stream.Write(BitConverter.GetBytes(lpbytes.Length), 0, sizeof(int));

      // Write the long data.
      stream.Write(lpbytes, 0, lpbytes.Length);

    }

    public static World LoadWorld(string name)
    {
      string savePath = string.Format("Saves\\{0}\\world.dat", name);

      World world = new World();

      using (FileStream stream = File.Open(savePath, FileMode.Open))
      {
        SaveTag tag = 0;
        byte[] data = null;
        int length = 0;

        while (tag != SaveTag.End)
        {
          ReadNextDataBlock(stream, ref tag, ref data, ref length);

          switch (tag)
          {
            case SaveTag.Created:
              world.CreationTimeTicks = BitConverter.ToInt64(data, 0);
              break;
            case SaveTag.Seed:
              world.Seed = BitConverter.ToInt32(data, 0);
              break;
            case SaveTag.LastPlayed:
              world.LastPlayedTimeTicks = BitConverter.ToInt64(data, 0);
              break;
            case SaveTag.Name:
              // An array that fits the unicode chars(size 2 bytes usually).
              char[] chars = new char[(length / sizeof(char))];
              
              // Iterate through the array and assign the chars.
              for (int i = 0; i < chars.Length; ++i)
              {
                chars[i] = BitConverter.ToChar(data, i * sizeof(char));
              }

              // Convert the char array to a string.
              world.Name = new string(chars, 0, chars.Length);
              break;
            case SaveTag.CurrentTime:
              world.CurrentTimeTicks = BitConverter.ToInt64(data, 0);
              break;
            case SaveTag.End:
              // File ends here..
              // TODO(Peter): Throw on missing initializers.
              break;
            default:
              // All Tags should be initialized when saved.
              throw new InvalidDataException("world.dat contains invalid SaveTag!");
          }
        }
      }

      world.Initialize();

      return world;
    }

    public static void SaveCluster(World world, Cluster cluster)
    {
      string path = string.Format("Saves\\{0}\\clusters\\", world.Name);
      string cPath = string.Format(
        clusterString, cluster.Coordinates.X, cluster.Coordinates.Y
      );

      if (File.Exists(path + cPath))
      {
        throw new DataException(
          string.Format(
          "Cluster ({0}, {1}) already exists!",
            cluster.Coordinates.X.ToString(),
            cluster.Coordinates.Y.ToString()
          )
        );
      }

      byte[] clusterData;

      using (MemoryStream memStream = new MemoryStream())
      {
        // Save the cluster type.
        memStream.WriteByte((byte)SaveTag.ClusterType);
        memStream.Write(BitConverter.GetBytes(sizeof(byte)), 0, sizeof(int));
        memStream.WriteByte((byte)cluster.Type);

        // Save Cluster X-Coordinate
        memStream.WriteByte((byte)SaveTag.ClusterX);
        memStream.Write(BitConverter.GetBytes(sizeof(int)), 0, sizeof(int));
        memStream.Write(BitConverter.GetBytes(cluster.Coordinates.x), 0, sizeof(int));

        // Save Cluster Y-Coordinate
        memStream.WriteByte((byte)SaveTag.ClusterY);
        memStream.Write(BitConverter.GetBytes(sizeof(int)), 0, sizeof(int));
        memStream.Write(BitConverter.GetBytes(cluster.Coordinates.Y), 0, sizeof(int));

        // Save TileData
        int tilesLength = cluster.Tiles.Length;
        memStream.WriteByte((byte)SaveTag.TileData);
        memStream.Write(BitConverter.GetBytes(tilesLength), 0, sizeof(int));
        for (int i = 0; i < tilesLength; ++i)
        {
          memStream.WriteByte((byte)cluster.Tiles[i].Type);
        }

        // Set the end tag.
        memStream.WriteByte((byte)SaveTag.End);

        // Retrieve the compressed version of the data from memstream.
        clusterData = CompressionHelper.Compress(memStream.GetBuffer());
      }

      using (FileStream stream = File.Create(path + cPath))
      {
        stream.Write(clusterData, 0, clusterData.Length);
      }
    }

    public static Cluster LoadCluster(World world, int x, int y)
    {
      string filePath = string.Format(savePath, world.Name);
      filePath += clustersPath;
      filePath += string.Format(clusterString, x, y);
      
      // return null if the cluster has not been saved yet.
      if (!File.Exists(filePath))
      {
        return null;
      }

      Cluster cluster = new Cluster();

      byte[] cData;
      using (FileStream fStream = File.Open(filePath, FileMode.Open, FileAccess.Read))
      {
        cData = new byte[fStream.Length];
        fStream.Read(cData, 0, cData.Length);
      }

      // Decompress compressed data.
      cData = CompressionHelper.Decompress(cData);

      using (MemoryStream stream = new MemoryStream(cData))
      {
        SaveTag tag = 0;
        byte[] data = null;
        int length = 0;

        while (tag != SaveTag.End)
        {
          // Reads the next data block and saves tag, data and length of data.
          ReadNextDataBlock(stream, ref tag, ref data, ref length);

          switch (tag)
          {
            case SaveTag.ClusterType:
              cluster.Type = (ClusterType)data[0];
              break;
            case SaveTag.ClusterX:
              cluster.Coordinates.X = BitConverter.ToInt32(data, 0);
              break;
            case SaveTag.ClusterY:
              cluster.Coordinates.Y = BitConverter.ToInt32(data, 0);
              break;
            case SaveTag.TileData:
              cluster.Tiles = new Tile[length];

              // Set up all tiles.
              for (int i = 0; i < length; ++i)
              {
                cluster.Tiles[i] = Tile.Create((TileType)data[i]);
              }
              break;
            case SaveTag.End:
              // TODO(Peter): Do Something about end of file.
              break;
            default:
              throw new DataException(
                string.Format("Unrecognized SaveTag found in ({0}, {1})", x, y)
              );
          }
        }
      }

      cluster.SetTileCoords();
      cluster.SetHashCode();
      return cluster;
    }

    /// <summary>
    /// Reads a datablock from a stream using the in-house format.
    /// </summary>
    /// <param name="stream">Open stream to read from.</param>
    /// <param name="tag"></param>
    /// <param name="data"></param>
    /// <param name="length">Length in bytes of the data will be stored here.</param>
    private static void ReadNextDataBlock(Stream stream, ref SaveTag tag,
      ref byte[] data, ref int length)
    {
      // Read first byte to find what the next tag is.
      tag = (SaveTag)stream.ReadByte();
      byte[] lengthBytes = new byte[sizeof(int)];
      stream.Read(lengthBytes, 0, sizeof(int));
      length = BitConverter.ToInt32(lengthBytes, 0);
      data = new byte[length];
      stream.Read(data, 0, length);
    }

    /// <summary>
    /// Writes data as a part of an open XmlWriter stream.
    /// </summary>
    private static void WriteWorldData(XmlWriter writer, World world)
    {

    }

    private static void WriteClusterData(XmlWriter writer, Cluster cluster)
    {
  
    }
  }
}
