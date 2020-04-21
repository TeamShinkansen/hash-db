using System.Collections.Generic;
using System.Xml.Serialization;

namespace DataTool.Models.Dat
{
    [XmlRoot(ElementName = "datafile")]
    public class DatFile
    {
        [XmlElement(ElementName = "header")]
        public DatHeader Header = new DatHeader();

        [XmlElement(ElementName = "game")]
        public List<DatGame> Games = new List<DatGame>();
    }
}
