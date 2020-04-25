using DataTool.Models.Data;
using DataTool.Models.SimpleHashes;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace DataTool
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var exePath = Assembly.GetEntryAssembly().Location;

            if (Directory.Exists(Path.Combine(Path.GetDirectoryName(exePath), "DatFiles")))
            {
                XmlPath = Path.Combine(Path.GetDirectoryName(exePath), "DatFiles");
            }

            var xmlPathIndex = Array.IndexOf(args, "--xml-path");
            var csIndex = Array.IndexOf(args, "--cs");
            var outXmlIndex = Array.IndexOf(args, "--out-xml");
            var outXmlGzIndex = Array.IndexOf(args, "--out-xml-gz");
            var csOptionsIndex = Array.IndexOf(args, "--cs-opts");
            bool showUi = true;


            if (xmlPathIndex != -1)
            {
                XmlPath = args[xmlPathIndex + 1];
            }

            if (Directory.Exists(XmlPath))
            {
                ProgressForm.Start(host =>
                {
                    host.SetTitle("Reading Dat Files");
                    Collection = new List<DataGroup>();
                    var files = Directory.GetFiles(XmlPath, "*.xml", SearchOption.TopDirectoryOnly);

                    long count = 0;
                    foreach (var filename in files)
                    {
                        host.SetStatus($"Processing: {new FileInfo(filename).Name}");
                        Collection.Add(DataGroup.FromFile(filename));
                        host.SetProgress(++count, files.Length);
                    }

                    return ProgressForm.Result.Success;
                });
            }
            else
            {
                Collection = new List<DataGroup>();
            }

            if (csIndex != -1 && csOptionsIndex != -1)
            {
                ProgressForm.Start(host =>
                {
                    host.SetTitle("Writing .cs File");

                    var csOptions = args[csOptionsIndex + 1].Split(',');
                    var cs = Data.GenerateCsFile(csOptions[0], csOptions[1], csOptions[2], Collection);
                    File.WriteAllText(args[csIndex + 1], cs);

                    showUi = false;

                    return ProgressForm.Result.Success;
                });
            }

            if (outXmlIndex != -1 || outXmlGzIndex != -1)
            {
                ProgressForm.Start(host =>
                {
                    host.SetTitle("Writing XML file");

                    var simple = new RomFiles();
                    foreach (var entry in Data.GetHashDictionary(Collection).OrderBy(e => e.Key).Where(e => e.Value.TgdbId.Count > 0))
                    {
                        simple.Hashes.Add(entry.Value);
                    }

                    using (var xmlStream = new MemoryStream())
                    {
                        Xml.Serialize<RomFiles>(xmlStream, simple);
                        xmlStream.Seek(0, SeekOrigin.Begin);

                        if (outXmlIndex != -1)
                        {
                            using (var file = File.Create(args[outXmlIndex + 1]))
                            {
                                xmlStream.CopyTo(file);
                                xmlStream.Seek(0, SeekOrigin.Begin);
                            }
                        }

                        if (outXmlGzIndex != -1)
                        {
                            using (var file = File.Create(args[outXmlGzIndex + 1]))
                            using (var gzipStream = new GZipStream(file, CompressionLevel.Optimal))
                            {
                                xmlStream.CopyTo(gzipStream);
                            }
                        }
                    }

                    showUi = false;

                    return ProgressForm.Result.Success;
                });
            }

            if (showUi)
            {
                Application.Run(new MainForm());

                Directory.CreateDirectory(XmlPath);

                ProgressForm.Start(host =>
                {
                    host.SetTitle("Writing Dat Files");
                    Collection = Collection.OrderBy(e => e.Name).ToList();
                    long count = 0;
                    foreach (var group in Collection)
                    {
                        host.SetStatus($"Processing: {group.Name}");

                        group.Games = group.Games.OrderBy(e => e.Name).ToList();
                        foreach (var game in group.Games)
                        {
                            game.Roms = game.Roms.OrderBy(e => e.Name).ToList();
                        }
                        group.SaveXml(xmlPath: XmlPath);

                        host.SetProgress(++count, Collection.Count);
                    }

                    return ProgressForm.Result.Success;
                });
            }

            return 0;
        }
    }
}
