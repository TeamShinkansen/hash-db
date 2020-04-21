using DataTool.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataTool
{
    public class Data
    {
        public static Dictionary<string, List<int>> GetHashDictionary(List<DataGroup> data)
        {
            var output = new Dictionary<string, List<int>>();
            foreach (var group in data)
            {
                foreach (var game in group.Games)
                {
                    foreach (var rom in game.Roms)
                    {
                        List<int> idList;
                        if (output.ContainsKey(rom.Crc32))
                        {
                            idList = output[rom.Crc32];
                        }
                        else
                        {
                            output[rom.Crc32] = idList = new List<int>();
                        }

                        foreach (var id in game.TgdbId)
                        {
                            if (!idList.Contains(id))
                            {
                                idList.Add(id);
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
                if (entry.Count > 0)
                {
                    entries.Add($"[0x{key}] = new int[] {{ {String.Join(", ", entry.ToArray())} }}");
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
