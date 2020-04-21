using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DataTool
{
    public static class Xml
    {
        public static void Serialize<T>(Stream output, T data)
        {
            
            XmlSerializer mySerializer = new XmlSerializer(typeof(T));

            using (var myWriter = XmlWriter.Create(output, new XmlWriterSettings()
            {
                CloseOutput = false,
                Indent = true,
                IndentChars = "\t",
                Encoding = new UTF8Encoding(false)
            }))
            {
                mySerializer.Serialize(myWriter, data);
                myWriter.Close();
            }
        }
        public static T Deserialize<T>(Stream input)
        {
            var mySerializer = new XmlSerializer(typeof(T));
            var mySettings = new XmlReaderSettings()
            {
                DtdProcessing = DtdProcessing.Ignore
            };

            var myReader = XmlReader.Create(input, mySettings);
            return (T)mySerializer.Deserialize(myReader);
        }
    }
}
