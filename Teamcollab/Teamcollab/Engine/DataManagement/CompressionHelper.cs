using System.IO;
using System.IO.Compression;

namespace Teamcollab.Engine.DataManagement
{
  static class CompressionHelper
  {
    static object compLocker = new object();

    public static byte[] Compress(byte[] data)
    {
      using (MemoryStream stream = new MemoryStream())
      using (GZipStream zipStream = new GZipStream(stream,
        CompressionMode.Compress, false))
      {
        zipStream.Write(data, 0, data.Length);
        zipStream.Close();
        return stream.ToArray();
      }
    }

    public static byte[] Decompress(byte[] compressedData)
    {
      using (GZipStream zipStream = new GZipStream(
        new MemoryStream(compressedData), CompressionMode.Decompress))
      {
        using (MemoryStream memStream = new MemoryStream())
        {
          const int bSize = 4096;

          byte[] buffer = new byte[bSize];
          int bytesRead = 0;

          do
          {
            if (zipStream.CanRead)
            {
              bytesRead = zipStream.Read(buffer, 0, bSize);
            }
            if (bytesRead > 0)
            {
              memStream.Write(buffer, 0, bytesRead);
            }

          } while (bytesRead > 0);

          return memStream.ToArray();
        }
      }
    }
  }
}
