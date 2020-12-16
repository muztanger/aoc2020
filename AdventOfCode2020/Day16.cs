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
        enum State { Init, Your, Nearby };
        private static int Parse(IEnumerable<string> input)
        {
            var rules = new Dictionary<string, FieldRule>();
            var state = State.Init;
            var invalidFields = new List<int>();
            foreach (var line in input)
            {
                if (line.StartsWith("your ticket:"))
                {
                    state = State.Your;
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

        private static long Parse2(IEnumerable<string> input)
        {
            var rules = new Dictionary<string, FieldRule>();
            var state = State.Init;
            var invalidFields = new List<int>();
            var myTicket = new List<int>();
            var validTickets = new List<List<int>>();
            foreach (var line in input)
            {
                if (line.StartsWith("your ticket:"))
                {
                    state = State.Your;
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
                    else if (state == State.Your)
                    {
                        myTicket = line.Split(",").Select(x => int.Parse(x)).ToList();
                    }
                    else if (state == State.Nearby)
                    {
                        var values = line.Split(",").Select(x => int.Parse(x)).ToList();
                        int validCount = 0;
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
                            else
                            {
                                validCount++;
                            }
                        }
                        if (validCount == values.Count)
                        {
                            validTickets.Add(values);
                        }
                    }
                }
            }
            var validRules = new List<List<string>>();
            for (int i = 0; i < myTicket.Count; i++)
            {
                validRules.Add(rules.Keys.ToList());
            }

            // Remove all invalid
            foreach (var ticket in validTickets)
            {
                for (int i = 0; i < validRules.Count; i++)
                {
                    var remove = new HashSet<string>();
                    foreach (var ruleKV in rules)
                    {
                        if (!ruleKV.Value.IsValid(ticket[i]))
                        {
                            remove.Add(ruleKV.Key);
                        }
                    }
                    foreach (var r in remove)
                    {
                        if (validRules[i].Contains(r))
                        {
                            validRules[i].Remove(r);
                        }
                    }
                }
            }

            var finalRules = new string[validRules.Count];

            // Exclude
            bool loop = true;
            while (loop)
            {
                loop = false;
                for (int i = 0; i < validRules.Count; i++)
                {
                    if (finalRules[i] == null)
                    {
                        if (validRules[i].Count == 1)
                        {
                            for (int j = 0; j < validRules.Count; j++)
                            {
                                if (i != j)
                                {
                                    validRules[j].Remove(validRules[i].First());
                                    loop = true;
                                }
                            }
                            finalRules[i] = validRules[i].First();
                        }
                    }
                }
            }
            var prod = 1L;
            for (int i = 0; i < finalRules.Length; i++)
            {
                if (finalRules[i].StartsWith("departure"))
                {
                    prod *= myTicket[i];
                }
            }

            return prod;
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
            string input = @"class: 0-1 or 4-19
row: 0-5 or 8-19
seat: 0-13 or 16-19

your ticket:
11,12,13

nearby tickets:
3,9,18
15,1,5
5,14,9";
            var parsed = Parse2(Common.GetLines(input));
            //Assert.AreEqual(12*11*13, parsed);
            Assert.AreEqual(1, parsed);
        }

        [Test]
        public void Part2()
        {
            var parsed = Parse2(Common.DayInput(nameof(Day16)));
            Assert.AreEqual(0, parsed);
        }

    }
}
