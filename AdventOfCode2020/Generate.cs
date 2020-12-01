using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020
{
    public class Generate
    {
        [Test]
        public void GenerateDay()
        {
            int day = DateTime.Now.Day;
            string dayStr = $"Day{day:D2}";
            var baseDir  = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..");
            {
                var file = Path.Combine(baseDir, "Input", $"{dayStr}.input");
                if (!File.Exists(file))
                {
                    using var fs = File.Create(file);
                }
            }
            {
                var file = Path.Combine(baseDir, $"{dayStr}.cs");
                if (!File.Exists(file))
                {
                    var usings = new List<string>()
                    {
                        "NUnit.Framework",
                        "System",
                        "System.Collections.Generic",
                        "System.Linq",
                        "System.Text",
                    };

                    var tests = new List<string>()
                    {
                        "Part1_Example1",
                        "Part1_Example2",
                        "Part1",
                        "Part2_Example1",
                        "Part2_Example2", "" +
                        "Part2"
                    };

                    using (var fs = File.Create(file))
                    using (var writer = new StreamWriter(fs))
                    {
                        
                        foreach (var line in usings)
                        {
                            writer.WriteLine($"using {line};");
                        }
                        writer.WriteLine();
                        writer.WriteLine("namespace AdventOfCode2020");
                        writer.WriteLine("{");
                        int pad = 1;
                        Pad(writer, pad, $"public class {dayStr}");
                        Pad(writer, pad, "{");
                        pad++;

                        foreach (var test in tests)
                        {
                            Pad(writer, pad, "[Test]");
                            Pad(writer, pad, $"public void {test}()");
                            Pad(writer, pad, "{");
                            Pad(writer, pad, "}");
                            writer.WriteLine();
                        }
                        pad--;
                        Pad(writer, pad, "}");
                        pad--;
                        Pad(writer, pad, "}");

                        string Padding(int p) => new String(' ', 4 * p);
                        void Pad(StreamWriter w, int p, string str) => w.WriteLine($"{Padding(p)}{str}");
                    }
                }
            }
        }
    }
}