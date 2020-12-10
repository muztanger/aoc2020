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
        [Test]
        public void Part1_Example1()
        {
            var jolts = Parse(Common.GetLines(example1)).OrderBy(x => x);
            var counts = new Dictionary<int, int>();
            var last = 0;
            foreach (var x in jolts)
            {
                int diff = x - last;
                counts.TryGetValue(diff, out var y);
                y++;
                counts[diff] = y;
                last = x;
            }
            counts.TryGetValue(3, out var z);
            z++;
            counts[3] = z;

            Assert.AreEqual(7, counts[1]);
            Assert.AreEqual(5, counts[3]);
        }

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
        [Test]
        public void Part1_Example2()
        {
            var jolts = Parse(Common.GetLines(example2)).OrderBy(x => x);
            var counts = new Dictionary<int, int>();
            var last = 0;
            foreach (var x in jolts)
            {
                int diff = x - last;
                counts.TryGetValue(diff, out var y);
                y++;
                counts[diff] = y;
                last = x;
            }
            counts.TryGetValue(3, out var z);
            z++;
            counts[3] = z;

            Assert.AreEqual(22, counts[1]);
            Assert.AreEqual(10, counts[3]);
        }

        [Test]
        public void Part1()
        {
            var jolts = Parse(Common.DayInput(nameof(Day10))).OrderBy(x => x);
            var counts = new Dictionary<int, int>();
            var last = 0;
            foreach (var x in jolts)
            {
                int diff = x - last;
                counts.TryGetValue(diff, out var y);
                y++;
                counts[diff] = y;
                last = x;
            }
            counts.TryGetValue(3, out var z);
            z++;
            counts[3] = z;

            foreach (var kv in counts)
            {
                Console.WriteLine($"{kv.Key}: {kv.Value}");
            }
            Assert.AreEqual(2812, counts[1] * counts[3]);
        }

        public long ChoiceCount(int x, IEnumerable<int> choices, Dictionary<long, long> mem = null)
        {
            if (mem == null) mem = new Dictionary<long, long>();
            else if (mem.ContainsKey(x)) return mem[x];
            if (choices.Count() <= 1) return 1;
            long sum = 0;
            foreach (var choice in choices.Where(y => y > x && y <= x + 3).OrderByDescending(z => z))
            {
                sum += ChoiceCount(choice, choices.Where(x => x > choice), mem);
            }
            mem[x] = sum;
            return sum;
        }

        [Test]
        public void Part2_Example1()
        {
            var jolts = Parse(Common.GetLines(example1)).OrderBy(x => x);
            var choiceCount = new Dictionary<int, int>();
            long sum = 0;
            foreach (var choice in jolts.Where(y => y >= 1 && y <= 3))
            {
                sum += ChoiceCount(choice, jolts.Where(x => x > choice));
            }

            Assert.AreEqual(8, sum);
        }

        [Test]
        public void Part2_Example2()
        {
            var jolts = Parse(Common.GetLines(example2)).OrderBy(x => x);
            var choiceCount = new Dictionary<int, int>();
            long sum = 0;
            foreach (var choice in jolts.Where(y => y >= 1 && y <= 3))
            {
                sum += ChoiceCount(choice, jolts.Where(x => x > choice));
            }

            Assert.AreEqual(19208, sum);
        }

        [Test]
        public void Part2()
        {
            var jolts = Parse(Common.DayInput(nameof(Day10))).OrderBy(x => x);
            var choiceCount = new Dictionary<int, int>();
            long sum = 0;
            foreach (var choice in jolts.Where(y => y >= 1 && y <= 3))
            {
                sum += ChoiceCount(choice, jolts.Where(x => x > choice));
            }
            Assert.AreNotEqual(67108864, sum);
            Assert.AreEqual(386869246296064L, sum);
        }

    }
}
