using System.Xml.Serialization;
using System.IO;
using System;

namespace Teamcollab.DataSerialization
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
    /// <typeparam name="T"></typeparam>
    /// <param name="data">The object to serialize.</param>
    /// <param name="filePath"></param>
    /// <param name="fileMode"></param>
    public static void SerializeXml<T>(T data, string filePath, FileMode fileMode)
    {
      // Creates a global namespace to serialize to.
      XmlSerializerNamespaces nm = new XmlSerializerNamespaces();
      nm.Add("", "");

      XmlSerializer serializer = new XmlSerializer(typeof(T));
      using (FileStream stream = new FileStream(filePath, fileMode))
      {
        serializer.Serialize(stream, data, nm);
      }
    }

    /// <summary>
    /// Deserializes the file at the filepath and returns an object
    /// of type T if possible. Otherwise an exception will be thrown.
    /// </summary>
    /// <param name="filePath">The filepath to look for.</param>
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
