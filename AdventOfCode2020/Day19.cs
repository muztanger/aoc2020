using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    public class Day19
    {
        private static Dictionary<int, string> GetRules(IEnumerable<string> input)
        {
            var dict = new Dictionary<int, string>();
            foreach (var line in input)
            {
                if (!line.Any()) break;
                var sp = line.Split(":");
                dict[int.Parse(sp[0])] = sp[1].Replace(@"""", "").Trim();
            }
            return dict;
        }

        [Test]
        public void Part1_Example1()
        {
            string input = @"0: 1 2
1: ""a""
2: 1 3 | 3 1
3: ""b""";
            var rules = GetRules(Common.GetLines(input));
            var expr = rules[0];
            var regex = GetRegex(expr, rules);
            Console.WriteLine(  regex);
            Assert.AreEqual(0, 1);
        }

        private string GetRegex(string expr, Dictionary<int, string> rules)
        {
            var result = new StringBuilder();
            foreach (var token in expr.Split())
            {
                if (Regex.IsMatch(token, @"^[^\d]+$")) //char or pipe
                {
                    result.Append(token);
                }
                else
                {
                    var sub = rules[int.Parse(token)];
                    result.Append($"({GetRegex(sub, rules)})");
                }
            }
            return result.ToString();
        }

        [Test]
        public void Part1_Example2()
        {
            string input = @"0: 4 1 5
1: 2 3 | 3 2
2: 4 4 | 5 5
3: 4 5 | 5 4
4: ""a""
5: ""b""

ababbb
bababa
abbbab
aaabbb
aaaabbb";
            var rules = GetRules(Common.GetLines(input));
            var expr = rules[0];
            var regex = GetRegex(expr, rules);
            regex = "^" + regex + "$";
            var count = 0;
            bool check = false;
            foreach (var line in Common.GetLines(input))
            {
                if (check)
                {
                    if (Regex.IsMatch(line, regex))
                    {
                        count++;
                    }
                }
                if (!line.Any()) check = true;
            }
            Console.WriteLine(regex);
            Assert.AreEqual(2, count);
        }

        [Test]
        public void Part1()
        {
            var parsed = GetRules(Common.DayInput(nameof(Day19)));
            var rules = GetRules(Common.DayInput(nameof(Day19)));
            var expr = rules[0];
            var regex = GetRegex(expr, rules);
            regex = "^" + regex + "$";
            var count = 0;
            bool check = false;
            foreach (var line in Common.DayInput(nameof(Day19)))
            {
                if (check)
                {
                    if (Regex.IsMatch(line, regex))
                    {
                        count++;
                    }
                }
                if (!line.Any()) check = true;
            }
            Console.WriteLine(regex);
            Assert.AreEqual(2, count);
        }

        [Test]
        public void Part2_Example1()
        {
            string input = @"";
            var parsed = GetRules(Common.GetLines(input));
            Assert.AreEqual(0, 1);
        }

        [Test]
        public void Part2_Example2()
        {
            string input = @"";
            var parsed = GetRules(Common.GetLines(input));
            Assert.AreEqual(0, 1);
        }

        [Test]
        public void Part2()
        {
            var parsed = GetRules(Common.DayInput(nameof(Day19)));
            Assert.AreEqual(0, 1);
        }

    }
}
