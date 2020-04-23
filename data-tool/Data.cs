using DataTool.Models.Data;
using DataTool.Models.SimpleHashes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataTool
{
    public class Data
    {
        public static Dictionary<string, RomData> GetHashDictionary(List<DataGroup> data)
        {
            var output = new Dictionary<string, RomData>();
            foreach (var group in data)
            {
                foreach (var game in group.Games)
                {
                    foreach (var rom in game.Roms)
                    {
                        RomData romInfo;
                        if (output.ContainsKey(rom.Crc32))
                        {
                            romInfo = output[rom.Crc32];
                        }
                        else
                        {
                            output[rom.Crc32] = romInfo = new RomData();
                            romInfo.Name = rom.Name;
                            romInfo.Crc32 = rom.Crc32;
                        }

                        foreach (var id in game.TgdbId)
                        {
                            if (!romInfo.TgdbId.Contains(id))
                            {
                                romInfo.TgdbId.Add(id);
                            }
                        }
                    }
                }
            }

            return output;
        }

        public static string GenerateCsFile(string namespaceName, string className, string variableName, List<DataGroup> data)
        {
            var output = Data.GetHashDictionary(data);
            var outputLines = new List<string>();

            var entries = new List<string>();
            var keys = output.Keys.ToList();
            keys.Sort();
            foreach (var key in keys)
            {
                var entry = output[key];
                if (entry.TgdbId.Count > 0)
                {
                    entries.Add($"[0x{key}] = new int[] {{ {String.Join(", ", entry.TgdbId.ToArray())} }}");
                }
            }

            outputLines.AddRange(new string[]
            {
                "using System;",
                "using System.Collections.Generic;",
                "",
                $"namespace {namespaceName}",
                "{",
                $"   class {className}",
                "   {",
                $"       public static readonly IReadOnlyDictionary<UInt32, int[]> {variableName} = new Dictionary<UInt32, int[]>()",
                "       {",
                $"           {string.Join(",\r\n           ", entries.ToArray())}",
                "       };",
                "   }",
                "}"
            });

            return string.Join("\r\n", outputLines.ToArray());
        }
    }
}
