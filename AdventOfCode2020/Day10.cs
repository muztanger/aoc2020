using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class Day10
    {
        private static List<int> Parse(IEnumerable<string> input)
        {
            return input.Select(x => int.Parse(x)).ToList();
        }

        private static int DiffCount(IOrderedEnumerable<int> jolts)
        {
            var counts = new DefaultDictionary<int, int>();
            var last = 0;
            foreach (var x in jolts)
            {
                int diff = x - last;
                counts[diff]++;
                last = x;
            }
            counts[3]++;
            var result = counts[1] * counts[3];
            return result;
        }

        [Test]
        public void Part1_Example1()
        {
            var jolts = Parse(Common.GetLines(example1)).OrderBy(x => x);
            var result = DiffCount(jolts);
            Assert.AreEqual(7 * 5, result);
        }

        [Test]
        public void Part1_Example2()
        {
            var jolts = Parse(Common.GetLines(example2)).OrderBy(x => x);
            var result = DiffCount(jolts);
            Assert.AreEqual(22 * 10, result);
        }

        [Test]
        public void Part1()
        {
            var jolts = Parse(Common.DayInput(nameof(Day10))).OrderBy(x => x);
            int result = DiffCount(jolts);
            Assert.AreEqual(2812, result);
        }

        public long ChoiceCount(IEnumerable<int> choices, int x = 0, Dictionary<long, long> mem = null)
        {
            if (mem == null) mem = new Dictionary<long, long>();
            else if (mem.ContainsKey(x)) return mem[x];
            if (choices.Count() <= 1) return 1;
            long sum = 0;
            foreach (var choice in choices.Where(y => y > x && y <= x + 3).OrderByDescending(z => z))
            {
                sum += ChoiceCount(choices.Where(x => x > choice), choice, mem);
            }
            mem[x] = sum;
            return sum;
        }

        [Test]
        public void Part2_Example1()
        {
            var jolts = Parse(Common.GetLines(example1)).OrderBy(x => x);
            long sum = ChoiceCount(jolts);
            Assert.AreEqual(8, sum);
        }

        [Test]
        public void Part2_Example2()
        {
            var jolts = Parse(Common.GetLines(example2)).OrderBy(x => x);
            long sum = ChoiceCount(jolts);
            Assert.AreEqual(19208, sum);
        }

        [Test]
        public void Part2()
        {
            var jolts = Parse(Common.DayInput(nameof(Day10))).OrderBy(x => x);
            long sum = ChoiceCount(jolts);
            Assert.AreNotEqual(67108864, sum);
            Assert.AreEqual(386869246296064L, sum);
        }

        string example1 = @"16
10
15
5
1
11
7
19
6
12
4";

        string example2 = @"28
33
18
42
31
14
46
20
48
47
24
23
49
45
19
38
39
11
1
32
25
35
8
17
7
9
4
2
34
10
3";


    }
}
