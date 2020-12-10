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

        [Test]
        public void Part1_Example1()
        {
            string input = @"16
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
            var jolts = Parse(Common.GetLines(input)).OrderBy(x => x);
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
            Assert.AreEqual(0, 3);
        }

        [Test]
        public void Part1_Example2()
        {
            string input = @"28
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
            var jolts = Parse(Common.GetLines(input)).OrderBy(x => x);
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
            Assert.AreEqual(0, counts[1] * counts[3]);
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
            var parsed = Parse(Common.DayInput(nameof(Day10)));
            Assert.AreEqual(0, 1);
        }

    }
}
