using DataTool.Models.Dat;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DataTool.Models.Data
{
    [XmlRoot(ElementName = "game")]
public class DataGame : DatGame
{
  [XmlElement(ElementName = "tgdb")]
  public List<int> TgdbId = new List<int>();
}
}
