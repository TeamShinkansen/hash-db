using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TGDBHashTool.Models.Dat;

namespace TGDBHashTool.Models.Data
{
    [XmlRoot(ElementName = "datafile")]
    public class DataGroup
    {
        [XmlIgnore]
        public string Name 
        {
            get {
                var output = Header.Name?.Trim() ?? "";
                if (Header.Version != null && Header.Version.Trim().Length > 0)
                {
                    output += $" ({Header.Version.Trim()})";
                }
                return output;
            }
        }

        [XmlElement(ElementName = "header")]
        public DatHeader Header = new DatHeader();

        [XmlElement(ElementName = "game")]
        public List<DataGame> Games = new List<DataGame>();

        [XmlIgnore]
        private string XmlPath;

        public static DataGroup FromFile(string path)
        {
            using (var file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var deserialized = Xml.Deserialize<DataGroup>(file);
                deserialized.XmlPath = path;
                return deserialized;
            }
        }

        public DataGroup SaveXml(string filename = null, string xmlPath = null)
        {
            var output = filename ?? XmlPath;
            if (output == null && xmlPath == null)
            {
                throw new FileNotFoundException("No filename or path was specified.");
            }

            if (output == null && xmlPath != null)
            {
                var potentialName = $"{this.Name}";

                foreach (var invalid in Path.GetInvalidFileNameChars())
                {
                    potentialName = potentialName.Replace(invalid, '_');
                }

                var count = 0;
                
                while (true)
                {
                    var fullName = Path.Combine(xmlPath, $"{potentialName}{(count > 0 ? $" ({count})" : "")}.xml");
                    if (!File.Exists(fullName))
                    {
                        XmlPath = output = fullName;
                        break;
                    }
                    count++;
                }
            }

            Directory.CreateDirectory(Path.GetDirectoryName(output));

            using (var file = File.Create(output))
            {
                Xml.Serialize<DataGroup>(file, this);
            }

            return this;
        }

        public DataGroup Delete()
        {
            if (XmlPath != null && File.Exists(XmlPath))
            {
                File.Delete(XmlPath);
            }

            return this;
        }
    }
}
