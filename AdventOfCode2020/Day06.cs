using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class Day06
    {

        private static List<HashSet<char>> Parse(IEnumerable<string> input)
        {
            var result = new List<HashSet<char>>();
            var group = new HashSet<char>();
            foreach (var line in input)
            {
                if (!line.Any())
                {
                    result.Add(group);
                    group = new HashSet<char>();
                }
                else
                {
                    foreach (char c in line)
                    {
                        group.Add(c);
                    }
                }
            }
            result.Add(group);
            return result;
        }

        private static List<HashSet<char>> Parse2(IEnumerable<string> input)
        {
            var result = new List<HashSet<char>>();
            var group = new HashSet<char>();

            bool isFirst = true;
            foreach (var line in input)
            {
                if (!line.Any())
                {
                    result.Add(group);
                    group = new HashSet<char>();
                    isFirst = true;
                }
                else
                {
                    if (isFirst)
                    {
                        foreach (char c in line)
                        {
                            group.Add(c);
                        }
                        isFirst = false;
                    }
                    else
                    {
                        var next = new HashSet<char>();
                        foreach (char c in line)
                        {
                            next.Add(c);
                        }
                        group = group.Intersect(next).ToHashSet();
                    }
                }
            }
            result.Add(group);
            return result;
        }

        [Test]
        public void Part1_Example1()
        {
            string input = @"abc

a
b
c

ab
ac

a
a
a
a

b";
            var parsed = Parse(Common.GetLines(input));
            var sum = 0;
            foreach (var group in parsed)
            {
                sum += group.Count;
            }
            Assert.AreEqual(11, sum);
        }

        [Test]
        public void Part1()
        {
            var parsed = Parse(Common.DayInput(nameof(Day06)));
            var sum = 0;
            foreach (var group in parsed)
            {
                sum += group.Count;
            }
            Assert.AreEqual(6885, sum);
        }

        [Test]
        public void Part2_Example1()
        {
            string input = @"abc

a
b
c

ab
ac

a
a
a
a

b";
            var parsed = Parse2(Common.GetLines(input));
            int sum = 0;
            foreach (var group in parsed)
            {
                sum += group.Count;
            }
            Assert.AreEqual(6, sum);
        }

        [Test]
        public void Part2()
        {
            var parsed = Parse2(Common.DayInput(nameof(Day06)));
            int sum = 0;
            foreach (var group in parsed)
            {
                sum += group.Count;
            }
            Assert.AreEqual(3550, sum);
        }

    }
}
