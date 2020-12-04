using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    public class Day04
    {
        class Passport
        {
            static Func<string, bool> Byr = (str) =>
            {
                if (str.Length != 4) return false;
                foreach (char c in str)
                {
                    if (!Char.IsDigit(c)) return false;
                }
                var num = int.Parse(str);
                if (num < 1920 || num > 2002) return false;
                return true;
            };

            static Func<string, bool> Iyr = (str) =>
            {
                if (str.Length != 4) return false;
                foreach (char c in str)
                {
                    if (!Char.IsDigit(c)) return false;
                }
                var num = int.Parse(str);
                if (num < 2010 || num > 2020) return false;
                return true;
            };

            static Func<string, bool> Eyr = (str) =>
            {
                if (str.Length != 4) return false;
                foreach (char c in str)
                {
                    if (!Char.IsDigit(c)) return false;
                }
                var num = int.Parse(str);
                if (num < 2020 || num > 2030) return false;
                return true;
            };

            static Func<string, bool> Hgt = (str) =>
            {
                var match = Regex.Match(str, @"^(\d+)(cm|in)$");
                if (!match.Success) return false;
                var num = int.Parse(match.Groups[1].Value);
                if (match.Groups[2].Value.Equals("cm"))
                {
                    if (num < 150 || num > 193) return false;
                }
                else
                {
                    if (num < 59 || num > 76) return false;

                }
                return true;
            };

            static Func<string, bool> Hcl = (str) =>
            {
                var match = Regex.Match(str, @"^#[0-9a-f]{6}$");
                if (!match.Success) return false;
                return true;
            };

            static Func<string, bool> Ecl = (str) =>
            {
                var match = Regex.Match(str, @"^amb|blu|brn|gry|grn|hzl|oth$");
                if (!match.Success) return false;
                return true;
            };

            static Func<string, bool> Pid = (str) =>
            {
                var match = Regex.Match(str, @"^\d{9}$");
                if (!match.Success) return false;
                return true;
            };

            static Func<string, bool> Cid = (str) =>
            {
                return true;
            };

            private Dictionary<string, Func<string, bool>> rules = new Dictionary<string, Func<string, bool>>()
            {
                {"byr", Byr },
                {"iyr", Iyr },
                {"eyr", Eyr },
                {"hgt", Hgt },
                {"hcl", Hcl },
                {"ecl", Ecl },
                {"pid", Pid },
                {"cid", Cid },
            };

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

            public bool Valid2()
            {
                int missingCount = 0;
                foreach (var kv in rules)
                {
                    if (!Fields.ContainsKey(kv.Key))
                    {
                        missingCount++;
                    }
                    else
                    {
                        if (!rules[kv.Key](Fields[kv.Key])) return false;
                    }
                }
                if (missingCount == 1)
                {
                    if (!Fields.ContainsKey("cid")) return true; // not passport
                }
                return missingCount == 0;
            }

            public override string ToString()
            {
                return string.Join(", ", Fields.Select(kv => $"{kv.Key}: {kv.Value}"));
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
            string input = @"eyr:1972 cid:100
hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926

iyr:2019
hcl:#602927 eyr:1967 hgt:170cm
ecl:grn pid:012533040 byr:1946

hcl:dab227 iyr:2012
ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277

hgt:59cm ecl:zzz
eyr:2038 hcl:74454a iyr:2023
pid:3556412378 byr:2007";
            var parsed = Parse(Common.GetLines(input));
            foreach (var passport in parsed)
            {
                var valid = passport.Valid2();
                Assert.IsFalse(valid, passport.ToString());
            }
            Assert.AreEqual(0, 1);
        }

        [Test]
        public void Part2_Example2()
        {
            string input = @"pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980
hcl:#623a2f

eyr:2029 ecl:blu cid:129 byr:1989
iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm

hcl:#888785
hgt:164cm byr:2001 iyr:2015 cid:88
pid:545766238 ecl:hzl
eyr:2022

iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719";
            var parsed = Parse(Common.GetLines(input));
            foreach (var passport in parsed)
            {
                var valid = passport.Valid2();
                Assert.IsTrue(valid, passport.ToString());
            }
            Assert.AreEqual(0, 1);
        }

        [Test]
        public void Part2()
        {
            var parsed = Parse(Common.DayInput(nameof(Day04)));
            var count = 0;
            foreach (var passport in parsed)
            {
                var valid = passport.Valid2();
                if (valid) count++;
            }
            Assert.AreEqual(0, count);
        }

    }
}
