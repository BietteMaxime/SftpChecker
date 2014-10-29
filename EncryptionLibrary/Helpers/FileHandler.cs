using System.IO;
using System.Xml.Serialization;

namespace EncryptionLibrary.Helpers
{
    static public class FileHandler
    {
        static public T LoadFile<T>(string filename)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var fileStream = new FileStream(filename, FileMode.Open))
            {
                return (T)xmlSerializer.Deserialize(fileStream);
            }
        }

        static public void SaveObject<T>(T objectToSerialize, string filename)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var streamWriter = new StreamWriter(filename))
            {
                xmlSerializer.Serialize(streamWriter, objectToSerialize);
            }
        }
    }
}
