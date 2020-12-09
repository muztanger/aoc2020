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
            Assert.AreEqual(127, result);
        }

        [Test]
        public void Part2_Example1()
        {
            string input = @"";
            var parsed = Parse(Common.GetLines(input));
            Assert.AreEqual(0, 1);
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
            var parsed = Parse(Common.DayInput(nameof(Day09)));
            Assert.AreEqual(0, 1);
        }

    }
}
