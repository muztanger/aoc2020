using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class Day09
    {
        private static List<long> Parse(IEnumerable<string> input)
        {
            return new List<long>(input.Select(line => long.Parse(line)));
        }

        [Test]
        public void Part1_Example1()
        {
            string input = @"35
20
15
25
47
40
62
55
65
95
102
117
150
182
127
219
299
277
309
576";
            var parsed = Parse(Common.GetLines(input));
            int preamble = 5;
            long result = long.MinValue;
            for (int index = preamble; index < parsed.Count; index++)
            {
                Console.Write("{parsed[index]}: ");
                var sums = new List<long>();
                for (int i = index - preamble; i < index - 1; i++)
                {
                    for (int j = i + 1; j < index; j++)
                    {
                        //Console.WriteLine($"{i} {j}");
                        sums.Add(parsed[i] + parsed[j]);
                    }
                }
                if (sums.Contains(parsed[index]))
                {
                    Console.WriteLine("Valid");
                }
                else
                {
                    Console.WriteLine("Invalid");
                    result = parsed[index];
                    break;
                }
            }
            Assert.AreEqual(127, result);
        }

        [Test]
        public void Part1_Example2()
        {
            string input = @"";
            var parsed = Parse(Common.GetLines(input));
            Assert.AreEqual(0, 1);
        }

        [Test]
        public void Part1()
        {
            var parsed = Parse(Common.DayInput(nameof(Day09)));
            int preamble = 25;
            long result = long.MinValue;
            for (int index = preamble; index < parsed.Count; index++)
            {
                Console.Write("{parsed[index]}: ");
                var sums = new List<long>();
                for (int i = index - preamble; i < index - 1; i++)
                {
                    for (int j = i + 1; j < index; j++)
                    {
                        //Console.WriteLine($"{i} {j}");
                        sums.Add(parsed[i] + parsed[j]);
                    }
                }
                if (sums.Contains(parsed[index]))
                {
                    Console.WriteLine("Valid");
                }
                else
                {
                    Console.WriteLine("Invalid");
                    result = parsed[index];
                    break;
                }
            }
            Assert.AreEqual(552655238, result);
        }

        [Test]
        public void Part2_Example1()
        {
            const long y = 127;
            string input = @"35
20
15
25
47
40
62
55
65
95
102
117
150
182
127
219
299
277
309
576";
            var parsed = Parse(Common.GetLines(input));
            long min = long.MaxValue;
            long max = long.MinValue;
            bool found = false;
            for (int n = 2; n < parsed.Count - 1 && !found; n++)
            {
                long sum = 0;
                for (int i = 0; i < n; i++)
                {
                    sum += parsed[i];
                }

                if (sum == y)
                {
                    Console.WriteLine("i=0 n={n}");
                    found = true;
                    break;
                }

                {
                    int i = 0;
                    for (int j = n; j < parsed.Count; j++, i++)
                    {
                        sum -= parsed[i];
                        sum += parsed[j];
                        if (sum == y)
                        {
                            Console.WriteLine($"i={i} j={j} n={n}");
                            min = parsed[i + 1];
                            max = parsed[j - 1];
                            found = true;
                            break;
                        }
                    }
                }
            }
            Console.WriteLine($"min={min} max={max}");
            long result = min + max;
            Assert.AreEqual(62, result);
        }

        [Test]
        public void Part2_Example2()
        {
            string input = @"";
            var parsed = Parse(Common.GetLines(input));
            Assert.AreEqual(0, 1);
        }

        [Test]
        public void Part2()
        {
            const long y = 552655238;
            var parsed = Parse(Common.DayInput(nameof(Day09)));
            long min = long.MaxValue;
            long max = long.MinValue;
            bool found = false;
            for (int n = 2; n < parsed.Count - 1 && !found; n++)
            {
                long sum = 0;
                for (int i = 0; i < n; i++)
                {
                    sum += parsed[i];
                }

                if (sum == y)
                {
                    Console.WriteLine("i=0 n={n}");
                    found = true;
                    break;
                }

                {
                    int i = 0;
                    for (int j = n; j < parsed.Count; j++, i++)
                    {
                        sum -= parsed[i];
                        sum += parsed[j];
                        if (sum == y)
                        {
                            Console.WriteLine($"i+1={i+1} j-1={j-1} n={n}");
                            min = parsed[i + 1];
                            max = parsed[j - 1];
                            found = true;
                            break;
                        }
                    }
                }
            }
            Console.WriteLine($"min={min} max={max}");
            long result = min + max;
            Assert.AreNotEqual(75253258, result);
            Assert.AreNotEqual(66770727, result);
            Assert.AreEqual(0, result);
        }

    }
}
