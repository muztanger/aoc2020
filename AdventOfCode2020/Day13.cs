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
        }


        [Test]
        public void Part1_Example1()
        {
            string input = @"939
7,13,x,x,59,x,31,19";
            var result = Schedule.Earliest(Common.GetLines(input));
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
            string input = @"";
            //var parsed = Parse(Common.GetLines(input));
            Assert.AreEqual(0, 1);
        }

        [Test]
        public void Part2_Example2()
        {
            string input = @"";
            //var parsed = Parse(Common.GetLines(input));
            Assert.AreEqual(0, 1);
        }

        [Test]
        public void Part2()
        {
            //var parsed = Parse(Common.DayInput(nameof(Day13)));
            Assert.AreEqual(0, 1);
        }

    }
}
