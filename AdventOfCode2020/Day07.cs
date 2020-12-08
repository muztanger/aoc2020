using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    public class Day07
    {
        public class Bag : IEquatable<Bag>
        {
            readonly string color;
            Dictionary<Bag, int> subBags = new Dictionary<Bag, int>();

            public string Color => color;

            public Bag(string color)
            {
                this.color = color;
            }
            public static Bag Parse(string line)
            {
                var split = Regex.Split(line, @"contain");
                var result = new Bag(split[0].Substring(0, split[0].Length - " bags".Length).Trim());
                if (!split[1].Contains("no other bags"))
                {
                    var bags = split[1].Split(",");
                    foreach (var bagStr in bags)
                    {
                        var match = Regex.Match(bagStr, @"(\d+) (.*)bags?");
                        if (!match.Success)
                        {
                            Console.WriteLine($"bagStr={bagStr}");
                        }
                        var count = int.Parse(match.Groups[1].Value);
                        var bag = new Bag(match.Groups[2].Value.Trim());
                        result.subBags.Add(bag, count);
                    }
                }

                return result;
            }

            public bool Contains(string otherColor, HashSet<Bag> bags)
            {
                if (color.Equals(otherColor)) return true;
                if (subBags.ContainsKey(new Bag(otherColor))) return true;
                foreach (var kv in subBags)
                {
                    if (bags.TryGetValue(kv.Key, out var bag))
                    {
                        if (bag.Contains(otherColor, bags))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            public int Count(HashSet<Bag> bags)
            {
                int count = 1;
                foreach (var bag in subBags)
                {
                    if (bags.TryGetValue(bag.Key, out var subBag))
                    {
                        count += bag.Value * subBag.Count(bags);
                    }
                }
                return count;
            }

            public override string ToString()
            {
                var result = new StringBuilder();
                result.Append($"{color}");
                if (subBags.Count > 0)
                {
                    result.Append("(");
                    bool isFirst = true;
                    foreach (var kv in subBags)
                    {
                        if (!isFirst) result.Append(",");
                        isFirst = false;
                        result.Append(kv.Key);
                        result.Append($": {kv.Value}");
                    }
                    result.Append($")");
                }
                return result.ToString();
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Bag);
            }

            public bool Equals(Bag other)
            {
                return other != null &&
                       color == other.color;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(color);
            }

            public static bool operator ==(Bag left, Bag right)
            {
                return EqualityComparer<Bag>.Default.Equals(left, right);
            }

            public static bool operator !=(Bag left, Bag right)
            {
                return !(left == right);
            }
        }

        private static HashSet<Bag> Parse(IEnumerable<string> input)
        {
            var result = new HashSet<Bag>();
            foreach (var line in input)
            {
                result.Add(Bag.Parse(line));
            }
            return result;
        }

        [Test]
        public void Part1_Example1()
        {
            string input = @"light red bags contain 1 bright white bag, 2 muted yellow bags.
dark orange bags contain 3 bright white bags, 4 muted yellow bags.
bright white bags contain 1 shiny gold bag.
muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.
shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.
dark olive bags contain 3 faded blue bags, 4 dotted black bags.
vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.
faded blue bags contain no other bags.
dotted black bags contain no other bags.";
            var bags = Parse(Common.GetLines(input));

            var count = 0;
            var nonShinyBags = bags.Where(x => !x.Color.Equals("shiny gold")).ToHashSet();
            foreach (var bag in nonShinyBags)
            {
                if (bag.Contains("shiny gold", nonShinyBags))
                {
                    count++;
                }
                Console.WriteLine(bag);
            }
            Assert.AreEqual(4, count);
        }

        [Test]
        public void Part1()
        {
            var bags = Parse(Common.DayInput(nameof(Day07)));

            var count = 0;
            var nonShinyBags = bags.Where(x => !x.Color.Equals("shiny gold")).ToHashSet();
            foreach (var bag in nonShinyBags)
            {
                if (bag.Contains("shiny gold", nonShinyBags))
                {
                    count++;
                }
                Console.WriteLine(bag);
            }
            Assert.AreEqual(4, count);
        }

        [Test]
        public void Part2_Example1()
        {
            string input = @"light red bags contain 1 bright white bag, 2 muted yellow bags.
dark orange bags contain 3 bright white bags, 4 muted yellow bags.
bright white bags contain 1 shiny gold bag.
muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.
shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.
dark olive bags contain 3 faded blue bags, 4 dotted black bags.
vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.
faded blue bags contain no other bags.
dotted black bags contain no other bags.";
            var bags = Parse(Common.GetLines(input));
            int count = -1;
            if (bags.TryGetValue(new Bag("shiny gold"), out var shinyGold))
            {
                count = shinyGold.Count(bags);
            }
            
            Assert.AreEqual(32, count - 1);
        }

        [Test]
        public void Part2_Example2()
        {
            string input = @"shiny gold bags contain 2 dark red bags.
dark red bags contain 2 dark orange bags.
dark orange bags contain 2 dark yellow bags.
dark yellow bags contain 2 dark green bags.
dark green bags contain 2 dark blue bags.
dark blue bags contain 2 dark violet bags.
dark violet bags contain no other bags.";
            var bags = Parse(Common.GetLines(input));
            int count = -1;
            if (bags.TryGetValue(new Bag("shiny gold"), out var shinyGold))
            {
                count = shinyGold.Count(bags);
            }

            Assert.AreEqual(126, count - 1);
        }

        [Test]
        public void Part2()
        {
            var bags = Parse(Common.DayInput(nameof(Day07)));
            int count = -1;
            if (bags.TryGetValue(new Bag("shiny gold"), out var shinyGold))
            {
                count = shinyGold.Count(bags);
            }

            Assert.AreEqual(126, count - 1);
        }

    }
}
