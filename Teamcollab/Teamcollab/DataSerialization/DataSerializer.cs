using System.Xml.Serialization;
using System.IO;
using System;

namespace Teamcollab.DataSerialization
{
  sealed static class DataSerializer
  {
    #region Properties

    #endregion

    #region Members

    #endregion

    public DataSerializer()
    {

    }

    public static void SerializeXml<T>(T data, string filePath, FileMode fileMode)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(T));

      using (FileStream stream = new FileStream(filePath, fileMode))
      {
        serializer.Serialize(stream, data);
      }
    }

    public static T DeSerializeXml<T>(Type type, string filePath)
    {
      T result;

      XmlSerializer serializer = new XmlSerializer(typeof(T));
      using (FileStream stream = new FileStream(filePath, FileMode.Open))
      {
        result = (T)serializer.Deserialize(stream);
      }

      return result;
    }
  }
}
