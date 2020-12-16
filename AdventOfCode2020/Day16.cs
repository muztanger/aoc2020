using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class Day16
    {
        public class RuleRange
        {
            int min;
            int max;
            public RuleRange(int min, int max)
            {
                this.min = min;
                this.max = max;
            }
            public bool InRange(int x) => x >= min && x <= max;
        }

        public class FieldRule
        {
            private readonly RuleRange r1;
            private readonly RuleRange r2;

            public FieldRule(RuleRange r1, RuleRange r2)
            {
                this.r1 = r1;
                this.r2 = r2;
            }

            public bool IsValid(int x) => r1.InRange(x) || r2.InRange(x);
        }
        enum State { Init, YourTicket, Nearby };
        private static int Parse(IEnumerable<string> input)
        {
            var rules = new Dictionary<string, FieldRule>();
            var state = State.Init;
            var invalidFields = new List<int>();
            foreach (var line in input)
            {
                if (line.StartsWith("your ticket:"))
                {
                    state = State.YourTicket;
                }
                else if (line.StartsWith("nearby tickets:"))
                {
                    state = State.Nearby;
                }
                else if (line.Length == 0)
                {
                    // do nothing
                }
                else
                {
                    if (state == State.Init)
                    {
                        // departure location: 45-609 or 616-954
                        var ruleLine = line.Split(":");
                        var name = ruleLine[0];
                        ruleLine = ruleLine[1].Trim().Split();
                        var r1 = ruleLine[0].Split("-");
                        var range1 = new RuleRange(int.Parse(r1[0]), int.Parse(r1[1]));
                        var r2 = ruleLine[2].Split("-");
                        var range2 = new RuleRange(int.Parse(r2[0]), int.Parse(r2[1]));
                        var fieldRule = new FieldRule(range1, range2);
                        rules[name] = fieldRule;
                    }
                    else if (state == State.Nearby)
                    {
                        var values = line.Split(",").Select(x => int.Parse(x)).ToList();
                        foreach (var value in values)
                        {
                            var isFoundValid = false;
                            foreach (var rule in rules.Values)
                            {
                                if (rule.IsValid(value))
                                {
                                    isFoundValid = true;
                                    break;
                                }
                            }
                            if (!isFoundValid)
                            {
                                invalidFields.Add(value);
                            }
                        }
                    }
                }
            }
            return invalidFields.Sum();
        }

        [Test]
        public void Part1_Example1()
        {
            string input = @"class: 1-3 or 5-7
row: 6-11 or 33-44
seat: 13-40 or 45-50

your ticket:
7,1,14

nearby tickets:
7,3,47
40,4,50
55,2,20
38,6,12";
            var parsed = Parse(Common.GetLines(input));
            Assert.AreEqual(71, parsed);
        }

        [Test]
        public void Part1_Example2()
        {
            string input = @"";
            var parsed = Parse(Common.GetLines(input));
            Assert.AreEqual(0, 1);
        }

        [Test]
        public void Part1()
        {
            var parsed = Parse(Common.DayInput(nameof(Day16)));
            Assert.AreEqual(0, parsed);
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
            var parsed = Parse(Common.DayInput(nameof(Day16)));
            Assert.AreEqual(0, 1);
        }

    }
}
