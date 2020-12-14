using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    public class Day14
    {
        private static long ParseProgram(IEnumerable<string> input)
        {
            var mem = new Dictionary<long, long>();
            string mask = null;
            foreach (var line in input)
            {
                Console.WriteLine(line);
                if (line.StartsWith("mask"))
                {
                    mask = line.Split()[2];
                    Assert.AreEqual(36, mask.Length);
                }
                else if (line.StartsWith("mem"))
                {
                    MatchCollection matches = Regex.Matches(line, @"(\d+)");
                    long index = long.Parse(matches[0].Value);
                    long number = long.Parse(matches[1].Value);
                    long mask2 = 1L << 35;
                    Console.WriteLine(Convert.ToString((long)number, 2).PadLeft(36, '0'));
                    Console.WriteLine(mask);
                    for (int i = 0; mask != null && i < mask.Length; i++)
                    {
                        if (mask[i] == '1')
                        {
                            Console.WriteLine('1');
                            Console.WriteLine("   " + Convert.ToString((long)mask2, 2).PadLeft(36, '0'));
                            number = mask2 | number;
                            Console.WriteLine("   " + Convert.ToString((long)number, 2).PadLeft(36, '0'));
                        }
                        else if (mask[i] == '0')
                        {
                            Console.WriteLine('0');
                            Console.WriteLine("   " + Convert.ToString((long)mask2, 2).PadLeft(36, '0'));
                            number = ~mask2 & number;
                            Console.WriteLine("   " + Convert.ToString((long)number, 2).PadLeft(36, '0'));
                        }
                        mask2 >>= 1;
                    }
                    Console.WriteLine(Convert.ToString((long) number, 2).PadLeft(36, '0'));
                    mem[index] = number;
                }
            }
            return mem.Values.Sum();
        }

        [Test]
        public void Part1_Example1()
        {
            string input = @"mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
mem[8] = 11
mem[7] = 101
mem[8] = 0";
            var parsed = ParseProgram(Common.GetLines(input));
            Assert.AreEqual(0, 1);
        }

        [Test]
        public void Part1_Example2()
        {
            string input = @"";
            var parsed = ParseProgram(Common.GetLines(input));
            Assert.AreEqual(0, 1);
        }

        [Test]
        public void Part1()
        {
            var parsed = ParseProgram(Common.DayInput(nameof(Day14)));
            Assert.AreEqual(17934269678453, parsed);
        }

        [Test]
        public void Part2_Example1()
        {
            string input = @"";
            var parsed = ParseProgram(Common.GetLines(input));
            Assert.AreEqual(0, 1);
        }

        [Test]
        public void Part2_Example2()
        {
            string input = @"";
            var parsed = ParseProgram(Common.GetLines(input));
            Assert.AreEqual(0, 1);
        }

        [Test]
        public void Part2()
        {
            var parsed = ParseProgram(Common.DayInput(nameof(Day14)));
            Assert.AreEqual(0, 1);
        }

    }
}
