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
                var busses = new SortedList<long, long>();
                int index = 0;
                foreach (var bus in lines[1].Split(","))
                {
                    if (Regex.IsMatch(bus, @"\d+"))
                    {
                        busses.Add(long.Parse(bus), index);
                    }
                    index++;
                }
                var first = busses.First();
                long timestamp = first.Key - first.Value;
                long step = first.Key;
                busses.Remove(first.Key);

                var last = new Dictionary<long, long>();
                var stepChange = long.MinValue;
                var stepChangeKey = first.Key;
                for (;;)
                {
                    var isFound = true;
                    foreach (var bust in busses)
                    {
                        if ((timestamp + bust.Value) % bust.Key != 0)
                        {
                            isFound = false;
                            break;
                        }
                        else
                        {
                            if (last.ContainsKey(bust.Key))
                            {
                                stepChange = timestamp - last[bust.Key];
                                stepChangeKey = bust.Key;
                            }
                            else
                            {
                                last[bust.Key] = timestamp;
                            }
                        }
                    }
                    if (isFound) break;
                    if (stepChange > 0)
                    {
                        last.Clear();
                        step = stepChange;
                        stepChange = long.MinValue;
                        busses.Remove(stepChangeKey);
                    }
                    timestamp += step;
                }
                return timestamp;
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
            var result = Schedule.Contest(Common.DayInput(nameof(Day13)));
            Assert.AreEqual(690123192779524L, result);
        }

    }
}
