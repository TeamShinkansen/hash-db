using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;
using TGDBHashTool.Models.Data;
using TGDBHashTool.Models.SimpleHashes;

namespace TGDBHashTool
{
    static class Program
    {
        public static string XmlPath { get; set; } = "../../../DatFiles";
        public static List<DataGroup> Collection;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            var xmlPathIndex = Array.IndexOf(args, "--xml-path");
            var csIndex = Array.IndexOf(args, "--cs");
            var ceXmlIndex = Array.IndexOf(args, "--out-xml");
            var ceXmlGzIndex = Array.IndexOf(args, "--out-xml-gz");
            var csOptionsIndex = Array.IndexOf(args, "--cs-opts");
            bool showUi = true;
            

            if (xmlPathIndex != -1)
            {
                XmlPath = args[xmlPathIndex + 1];
            }

            if (Directory.Exists(XmlPath))
            {
                Collection = new List<DataGroup>();
                foreach (var filename in Directory.GetFiles(XmlPath, "*.xml", SearchOption.TopDirectoryOnly))
                {
                    Collection.Add(DataGroup.FromFile(filename));
                }
            }
            else
            {
                Collection = new List<DataGroup>();
            }
            
            if (csIndex != -1 && csOptionsIndex != -1)
            {
                var csOptions = args[csOptionsIndex + 1].Split(',');
                var cs = Data.GenerateCsFile(csOptions[0], csOptions[1], csOptions[2], Collection);
                File.WriteAllText(args[csIndex + 1], cs);

                showUi = false;
            }

            if (ceXmlIndex != -1 || ceXmlGzIndex != -1)
            {
                var simple = new SimpleHashes();
                foreach (var entry in Data.GetHashDictionary(Collection).OrderBy(e => e.Key).Where(e => e.Value.Count > 0))
                {
                    simple.Hashes.Add(new SimpleHash()
                    {
                        Crc32 = entry.Key,
                        TgdbId = entry.Value
                    });
                }

                using (var xmlStream = new MemoryStream())
                {
                    Xml.Serialize<SimpleHashes>(xmlStream, simple);
                    xmlStream.Seek(0, SeekOrigin.Begin);

                    if (ceXmlIndex != -1)
                    {
                        using (var file = File.Create(args[ceXmlIndex + 1]))
                        {
                            xmlStream.CopyTo(file);
                            xmlStream.Seek(0, SeekOrigin.Begin);
                        }
                    }

                    if (ceXmlGzIndex != -1)
                    {
                        using (var file = File.Create(args[ceXmlGzIndex + 1]))
                        using (var gzipStream = new GZipStream(file, CompressionLevel.Optimal))
                        {
                            xmlStream.CopyTo(gzipStream);
                        }
                    }   
                }

                showUi = false;
            }
            
            if (showUi)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());

                Directory.CreateDirectory(XmlPath);
                Collection = Collection.OrderBy(e => e.Name).ToList();
                foreach (var group in Collection)
                {
                    group.Games = group.Games.OrderBy(e => e.Name).ToList();
                    foreach (var game in group.Games)
                    {
                        game.Roms = game.Roms.OrderBy(e => e.Name).ToList();
                    }
                    group.SaveXml(xmlPath: XmlPath);
                }
            }

            return 0;
        }
    }
}
