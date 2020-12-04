using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    public class Generate
    {
        static readonly HttpClient client = new HttpClient();

        public static async Task<string> GetDayInput(int day)
        {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://adventofcode.com/2020/day/{day}/input");
                var cookieFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Cookie.dat");
                if (!File.Exists(cookieFile))
                {
                    Console.WriteLine($"Create a cookie file and call it: {cookieFile}");
                }
                var lines = File.ReadAllLines(cookieFile);
                request.Headers.Add("Cookie", lines[0]);
                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
        }

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
                    File.WriteAllText(file, GetDayInput(day).Result);
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
                        "Part2_Example2",
                        "Part2"
                    };

                    using var fs = File.Create(file);
                    using var writer = new StreamWriter(fs);

                    foreach (var line in usings)
                    {
                        writer.WriteLine($"using {line};");
                    }

                    writer.WriteLine();
                    writer.WriteLine("namespace AdventOfCode2020");
                    writer.WriteLine("{");
                    int pad = 1;
                    WriteLine($"public class {dayStr}");
                    WriteLine("{");
                    pad++;

                    WriteLine("private static List<int> Parse(IEnumerable<string> input)");
                    WriteLine("{");
                    pad++;
                    WriteLine("return new List<int>();");
                    pad--;
                    WriteLine("}");
                    writer.WriteLine();

                    foreach (var test in tests)
                    {
                        WriteLine("[Test]");
                        WriteLine($"public void {test}()");
                        WriteLine("{");
                        pad++;
                        if (test.Contains("Example"))
                        {
                            WriteLine("string input = @\"\";");
                            WriteLine("var parsed = Parse(Common.GetLines(input));");
                        }
                        else
                        {
                            WriteLine($"var parsed = Parse(Common.DayInput(nameof({dayStr})));");
                        }
                        WriteLine("Assert.AreEqual(0, 1);");
                        pad--;
                        WriteLine("}");
                        writer.WriteLine();
                    }
                    pad--;
                    WriteLine("}");
                    pad--;
                    WriteLine("}");

                    string Padding() => new String(' ', 4 * pad);
                    void WriteLine(string str) => writer.WriteLine($"{Padding()}{str}");
                }
            }
        }
    }
}