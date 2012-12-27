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

      Created,
      Seed,
      LastPlayed,
      Name,
      CurrentTime,
      // Version // TODO(Peter): Implement
      End,
    }


    #region Properties

    #endregion

    #region Members

    #endregion

    public static void SaveWorld(World world)
    {
      string path = string.Format(
        @"Saves\{0}\", world.Name
      );

      string worldPath = path + "world.dat";

      if (Directory.Exists(path))
      {
        if (File.Exists(worldPath))
        {
          // Saves an existing world by updating.
          SaveExisting(world);
        }
      }
      else
      {
        Directory.CreateDirectory(path);
        File.Create(worldPath);
      }

      using (FileStream stream = File.Open(worldPath, FileMode.CreateNew))
      {
        // Save the name.
        SaveName(stream, world);

        // Save the last played date.
        SaveLong(stream, world.LastPlayedTimeTicks, SaveTag.LastPlayed);

        // Save the Creation time.
        SaveLong(stream, world.CreationTimeTicks, SaveTag.Created);

        // Save the in-game time.
        SaveLong(stream, world.CurrentTimeTicks, SaveTag.CurrentTime);

        // Save the seed.
        stream.WriteByte((byte)SaveTag.Seed);
        byte[] seedBytes = BitConverter.GetBytes(world.Seed);
        stream.Write(BitConverter.GetBytes(seedBytes.Length), 0, sizeof(int));
        stream.Write(seedBytes, 0, seedBytes.Length);

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
        while (tag != SaveTag.End)
        {
          // Read first byte to find what the next tag is.
          tag = (SaveTag)stream.ReadByte();
          byte[] lengthBytes = new byte[sizeof(int)];
          stream.Read(lengthBytes, 0, sizeof(int));
          int length = BitConverter.ToInt32(lengthBytes, 0);
          byte[] data = new byte[length];
          stream.Read(data, 0, length);

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
              char[] chars = new char[(length / sizeof(char))];
              for (int i = 0; i < chars.Length; ++i)
              {
                chars[i] = BitConverter.ToChar(data, i * 2);
              }
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
