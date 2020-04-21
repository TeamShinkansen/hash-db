using CsvHelper.Configuration;

namespace TGDBHashTool.Models
{
    public class HashCsv
    {
        public string Sha1;
        public int? TgdbId;
        public int SystemId;
        public string Name;
        public sealed class Map : ClassMap<HashCsv>
        {
            public Map()
            {
                Map(m => m.Sha1).Index(0);
                Map(m => m.TgdbId).Index(1);
                Map(m => m.SystemId).Index(2);
                Map(m => m.Name).Index(3);
            }
        }
    }

}
