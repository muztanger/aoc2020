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

        private static long FindInvalid(List<long> parsed, int preamble)
        {
            for (int index = preamble; index < parsed.Count; index++)
            {
                var sums = new List<long>();
                for (int i = index - preamble; i < index - 1; i++)
                {
                    for (int j = i + 1; j < index; j++)
                    {
                        sums.Add(parsed[i] + parsed[j]);
                    }
                }
                if (!sums.Contains(parsed[index]))
                {
                    return parsed[index];
                }
            }
            throw new Exception("Not found");
        }

       readonly string example1 = @"35
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
        [Test]
        public void Part1_Example1()
        {
            var parsed = Parse(Common.GetLines(example1));
            long result = FindInvalid(parsed, 5);
            Assert.AreEqual(127, result);
        }

        [Test]
        public void Part1()
        {
            var parsed = Parse(Common.DayInput(nameof(Day09)));
            long result = FindInvalid(parsed, 25);
            Assert.AreEqual(552655238, result);
        }

        [Test]
        public void Part2_Example1()
        {
            var parsed = Parse(Common.GetLines(example1));
            var result = BreakEncryption(127, parsed);
            Assert.AreEqual(62, result);
        }

        [Test]
        public void Part2()
        {
            var parsed = Parse(Common.DayInput(nameof(Day09)));
            long result = BreakEncryption(552655238L, parsed);
            Assert.AreEqual(70672245, result);
        }

        private static long BreakEncryption(long y, List<long> parsed)
        {
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
                    for (int k = 0; k < n; k++)
                    {
                        min = Math.Min(min, parsed[k]);
                        max = Math.Max(max, parsed[k]);
                    }
                    return min + max;
                }

                if (!found)
                {
                    int i = 0;
                    for (int j = n; j < parsed.Count; j++, i++)
                    {
                        sum -= parsed[i];
                        sum += parsed[j];
                        if (sum == y)
                        {
                            for (int k = i + 1; k <= j; k++)
                            {
                                min = Math.Min(min, parsed[k]);
                                max = Math.Max(max, parsed[k]);
                            }
                            return min + max;
                        }
                    }
                }
            }
            throw new Exception("Not found");
        }
        
    }
}
