using System.Collections.Generic;
using System.Xml.Serialization;

namespace TGDBHashTool.Models.SimpleHashes
{
    [XmlRoot(ElementName = "games")]
    public class SimpleHashes
    {
        [XmlElement(ElementName = "game")]
        public List<SimpleHash> Hashes = new List<SimpleHash>();
    }
}
