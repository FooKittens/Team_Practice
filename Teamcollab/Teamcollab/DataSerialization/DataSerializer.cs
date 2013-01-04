using System.IO;
using System.Xml.Serialization;

namespace Midgard.DataSerialization
{
  public static class DataSerializer
  {
    #region Properties

    #endregion

    #region Members

    #endregion

    /// <summary>
    /// Serializes an object of type T to the file path.
    /// Filepath should include file endings.
    /// </summary>
    /// <typeparam name="T">Any serializable object</typeparam>
    /// <param name="data">The object to serialize.</param>
    /// <param name="path">Relative path to write to</param>
    /// <param name="fileMode">How to open the file</param>
    public static void SerializeXml<T>(T data, string path, FileMode fileMode)
    {
      // Creates a global namespace to serialize to.
      XmlSerializerNamespaces nm = new XmlSerializerNamespaces();
      nm.Add("", "");

      XmlSerializer serializer = new XmlSerializer(typeof(T));
      using (FileStream stream = new FileStream(path, fileMode))
      {
        serializer.Serialize(stream, data, nm);
      }
    }

    /// <summary>
    /// Deserializes the file at the filepath and returns an object
    /// of type T if possible. Otherwise an exception will be thrown.
    /// </summary>
    /// <param name="filePath">Relative path to read from</param>
    /// <returns></returns>
    public static T DeSerializeXml<T>(string filePath)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(T));
      T result;
      using (FileStream stream = new FileStream(filePath, FileMode.Open))
      {
        result = (T)serializer.Deserialize(stream);
      }

      return result;
    }
  }
}
