using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    public class Day13
    {
        public sealed class ReverseComparer<T> : IComparer<T>
        {
            private readonly IComparer<T> inner;
            public ReverseComparer() : this(null) { }
            public ReverseComparer(IComparer<T> inner)
            {
                this.inner = inner ?? Comparer<T>.Default;
            }
            int IComparer<T>.Compare(T x, T y) { return inner.Compare(y, x); }
        }

        class Schedule
        {
            public static int Earliest(IEnumerable<string> input)
            {
                var lines = input.ToList();
                var timestamp = int.Parse(lines[0]);
                var earliest = timestamp;
                var busses = lines[1].Split(",").Where(x => Regex.IsMatch(x, @"\d+")).Select(x => int.Parse(x));
                while (true)
                {
                    foreach (var bus in busses)
                    {
                        if (timestamp % bus == 0)
                        {
                            return bus * (timestamp - earliest);
                        }
                    }
                    timestamp++;
                }
            }

            public static long Contest(IEnumerable<string> input)
            {
                var lines = input.ToList();
                var busses = new List<(long, long)>();
                int offset = 0;
                foreach (var bus in lines[1].Split(","))
                {
                    if (Regex.IsMatch(bus, @"\d+"))
                    {
                        busses.Add((long.Parse(bus), offset));
                    }
                    offset++;
                }
                var bussTimes = new List<long>();
                foreach (var kv in busses)
                {
                    var x = kv.Item1 - kv.Item2;
                    while (x < 0) x += kv.Item1;
                    bussTimes.Add(x);
                }

                long step = 1024 * 100;
                long last = 0L;
                long limit = step;
                for (;;)
                {
                    // jump longer and longer when realizing perodicity!
                    Console.WriteLine(limit);
                    var table = new DefaultDictionary<long, long>();
                    //var table = new int[step];
                    for (int i = 0; i < bussTimes.Count; i++)
                    {
                        while (bussTimes[i] < limit)
                        {
                            //Console.WriteLine(bussTimes[i] - last);
                            //table[bussTimes[i] - last]++;
                            table[bussTimes[i]]++;
                            //if (table[bussTimes[i] - last] == bussTimes.Count) return bussTimes[i];
                            if (table[bussTimes[i]] == bussTimes.Count) return bussTimes[i];
                            bussTimes[i] += busses[i].Item1;
                        }
                    }
                    last = limit;
                    limit += step;
                }
                throw new Exception("not found");
            }
        }


        string example1 = @"939
7,13,x,x,59,x,31,19";

        [Test]
        public void Part1_Example1()
        {
            var result = Schedule.Earliest(Common.GetLines(example1));
            Assert.AreEqual(295, result);
        }

        [Test]
        public void Part1()
        {
            var result = Schedule.Earliest(Common.DayInput(nameof(Day13)));
            Assert.AreEqual(333, result);
        }

        [Test]
        public void Part2_Example1()
        {
            var result = Schedule.Contest(Common.GetLines(example1));
            Assert.AreEqual(1068781L, result);
        }

        [TestCase("17,x,13,19", 3417L)]
        [TestCase("67,7,59,61", 754018L)]
        [TestCase("67,x,7,59,61", 779210L)]
        [TestCase("67,7,x,59,61", 1261476L)]
        [TestCase("1789,37,47,1889", 1202161486L)]
        public void Part2_Example2(string input, long expected)
        {
            var result = Schedule.Contest(Common.GetLines("\n" + input));
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Part2()
        {
            //var parsed = Parse(Common.DayInput(nameof(Day13)));
            var result = Schedule.Contest(Common.DayInput(nameof(Day13)));
            Assert.AreEqual(1068781L, result);
        }

    }
}
