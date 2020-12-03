using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class Day02
    {
        private static int Count(IEnumerable<string> lines, Func<string, int, int, char, bool> check)
        {
            var count = 0;
            foreach (var line in lines)
            {
                var split = line.Split();
                var range = split[0].Split("-");
                var low = int.Parse(range[0]);
                var high = int.Parse(range[1]);
                var c = split[1].First();
                var password = split[2];
                if (check(password, low, high, c))
                {
                    count++;
                }
            }
            return count;
        }

        private static readonly Func<string, int, int, char, bool> Check1 = (password, low, high, c) =>
        {
            var check = password.Count(x => x.Equals(c));
            return check >= low && check <= high;
        };

        private static readonly Func<string, int, int, char, bool> Check2 = (password, low, high, c) =>
        {
            return (password[low - 1].Equals(c) ^ password[high - 1].Equals(c));
        };

        [Test]
        public void Part1_Example1()
        {
            string input = @"1-3 a: abcde
1-3 b: cdefg
2-9 c: ccccccccc";
            int count = Count(Common.GetLines(input), Check1);
            Assert.AreEqual(2, count);
        }

        [Test]
        public void Part1()
        {
            Assert.AreEqual(474, Count(Common.DayInput(nameof(Day02)), Check1));
        }

        [Test]
        public void Part2()
        {
            Assert.AreEqual(745, Count(Common.DayInput(nameof(Day02)), Check2));
        }

    }
}
