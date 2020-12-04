using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class Day04
    {
        class Passport
        {
            private Dictionary<string, int> rules = new Dictionary<string, int>()
            {
                {"byr", 1 },
                {"iyr", 1 },
                {"eyr", 1 },
                {"hgt", 1 },
                {"hcl", 1 },
                {"ecl", 1 },
                {"pid", 1 },
                {"cid", 1 },
            };
            /*
    byr (Birth Year)
    iyr (Issue Year)
    eyr (Expiration Year)
    hgt (Height)
    hcl (Hair Color)
    ecl (Eye Color)
    pid (Passport ID)
    cid (Country ID)
*/

            public Dictionary<string, string> Fields = new Dictionary<string, string>();
            public bool Valid()
            {
                int missingCount = 0;
                foreach (var kv in rules)
                {
                    if (!Fields.ContainsKey(kv.Key)) missingCount++;
                }
                if (missingCount == 1)
                {
                    if (!Fields.ContainsKey("cid")) return true; // not passport
                }
                return missingCount == 0;
            }
        }
        private static List<Passport> Parse(IEnumerable<string> input)
        {
            var result = new List<Passport>();
            var current = new Passport();
            foreach (var line in input)
            {
                if (!line.Any()) 
                {
                    result.Add(current);
                    current = new Passport();
                }
                else
                {
                    var split = line.Split();
                    foreach (var field in split)
                    {
                        var fs = field.Split(":");
                        current.Fields[fs[0]] = fs[1];
                    }
                }
            }
            result.Add(current);

            return result;
        }

        [Test]
        public void Part1_Example1()
        {
            string input = @"ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm

iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929

hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm

hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in";
            var parsed = Parse(Common.GetLines(input));
            foreach (var passport in parsed)
            {
                var valid = passport.Valid();
                Console.WriteLine(valid);
            }
            Assert.AreEqual(0, 1);
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
            var parsed = Parse(Common.DayInput(nameof(Day04)));
            var count = 0;
            foreach (var passport in parsed)
            {
                var valid = passport.Valid();
                if (valid) count++;
            }
            Assert.AreEqual(0, count);
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
            var parsed = Parse(Common.DayInput(nameof(Day04)));
            Assert.AreEqual(0, 1);
        }

    }
}
