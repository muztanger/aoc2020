using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class Day02
    {
        [Test]
        public void Part1_Example1()
        {
            string input = @"1-3 a: abcde
1-3 b: cdefg
2-9 c: ccccccccc";
            var count = 0;
            foreach (var line in Common.GetLines(input))
            {
                var split = line.Split();
                var range = split[0].Split("-");
                var low = int.Parse(range[0]);
                var high = int.Parse(range[1]);
                var c = split[1].First();
                var password = split[2];
                var check = password.Count(x => x.Equals(c));
                if (check >= low && check <= high)
                {
                    count++;
                }
            }
            Assert.AreEqual(2, count);
        }

        [Test]
        public void Part1_Example2()
        {
        }

        [Test]
        public void Part1()
        {
            var count = 0;
            foreach (var line in Common.DayInput(nameof(Day02)))
            {
                var split = line.Split();
                var range = split[0].Split("-");
                var low = int.Parse(range[0]);
                var high = int.Parse(range[1]);
                var c = split[1].First();
                var password = split[2];
                var check = password.Count(x => x.Equals(c));
                if (check >= low && check <= high)
                {
                    count++;
                }
            }
            Assert.AreEqual(474, count);
        }

        [Test]
        public void Part2_Example1()
        {
        }

        [Test]
        public void Part2_Example2()
        {
        }

        [Test]
        public void Part2()
        {
            var count = 0;
            foreach (var line in Common.DayInput(nameof(Day02)))
            {
                var split = line.Split();
                var range = split[0].Split("-");
                var low = int.Parse(range[0]);
                var high = int.Parse(range[1]);
                var c = split[1].First();
                var password = split[2];
                var check = password[low - 1].Equals(c) ? 1 : 0;
                if (password[high -1].Equals(c))
                {
                    check++;
                }
                if (check == 1) count++;
            }
            Assert.AreEqual(745, count);
        }

    }
}
