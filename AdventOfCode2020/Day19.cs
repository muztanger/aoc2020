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
            Assert.AreEqual(180, count);
        }

        [Test]
        public void Part2_Example1()
        {
            string input = @"42: 9 14 | 10 1
9: 14 27 | 1 26
10: 23 14 | 28 1
1: ""a""
11: 42 31
5: 1 14 | 15 1
19: 14 1 | 14 14
12: 24 14 | 19 1
16: 15 1 | 14 14
31: 14 17 | 1 13
6: 14 14 | 1 14
2: 1 24 | 14 4
0: 8 11
13: 14 3 | 1 12
15: 1 | 14
17: 14 2 | 1 7
23: 25 1 | 22 14
28: 16 1
4: 1 1
20: 14 14 | 1 15
3: 5 14 | 16 1
27: 1 6 | 14 18
14: ""b""
21: 14 1 | 1 14
25: 1 1 | 1 14
22: 14 14
8: 42
26: 14 22 | 1 20
18: 15 15
7: 14 5 | 1 21
24: 14 1

abbbbbabbbaaaababbaabbbbabababbbabbbbbbabaaaa
bbabbbbaabaabba
babbbbaabbbbbabbbbbbaabaaabaaa
aaabbbbbbaaaabaababaabababbabaaabbababababaaa
bbbbbbbaaaabbbbaaabbabaaa
bbbababbbbaaaaaaaabbababaaababaabab
ababaaaaaabaaab
ababaaaaabbbaba
baabbaaaabbaaaababbaababb
abbbbabbbbaaaababbbbbbaaaababb
aaaaabbaabaaaaababaa
aaaabbaaaabbaaa
aaaabbaabbaaaaaaabbbabbbaaabbaabaaa
babaaabbbaaabaababbaabababaaab
aabbbbbaabbbaaaaaabbbbbababaaaaabbaaabba";
            var rules = GetRules(Common.GetLines(input));

            rules.Remove(8);
            rules.Remove(11);
            //0: 8 11
            //8: 42 | 42 8
            //11: 42 31 | 42 11 31

            //0: (42)+ (42){x} (31){x}

            var regex42 = GetRegex(rules[42], rules);
            //regex42 = $"^({regex42})+";
            var regex31 = GetRegex(rules[31], rules);
            //regex31 = $"({regex31})+$";
            
            var count = 0;
            bool check = false;
            foreach (var line in Common.GetLines(input))
            {
                if (check)
                {
                    if (Regex.IsMatch(line, $"^({regex42})+({regex31})+$"))
                    {
                        var last31 = Regex.Match(line, $"({regex31})+$");
                        var x = Regex.Matches(line[0..last31.Index], regex42).Count;
                        var y = Regex.Matches(line[last31.Index..], regex31).Count;
                        if (x > y)
                        {
                            count++;
                        }
                    }
                }
                if (!line.Any()) check = true;
            }
            Assert.AreEqual(12, count);
        }

        [Test]
        public void Part2()
        {
            var rules = GetRules(Common.DayInput(nameof(Day19)));

            rules.Remove(8);
            rules.Remove(11);
            //0: 8 11
            //8: 42 | 42 8
            //11: 42 31 | 42 11 31

            //0: (42)+ (42){x} (31){x}

            var regex42 = GetRegex(rules[42], rules);
            //regex42 = $"^({regex42})+";
            var regex31 = GetRegex(rules[31], rules);
            //regex31 = $"({regex31})+$";

            var count = 0;
            bool check = false;
            foreach (var line in Common.DayInput(nameof(Day19)))
            {
                if (check)
                {
                    if (Regex.IsMatch(line, $"^({regex42})+({regex31})+$"))
                    {
                        var last31 = Regex.Match(line, $"({regex31})+$");
                        var x = Regex.Matches(line[0..last31.Index], regex42).Count;
                        var y = Regex.Matches(line[last31.Index..], regex31).Count;
                        if (x > y)
                        {
                            count++;
                        }
                    }
                }
                if (!line.Any()) check = true;
            }
            Assert.AreNotEqual(351, count); //too high
            Assert.AreEqual(323, count);
        }

    }
}
