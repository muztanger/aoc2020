using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class Day11
    {
        private static Dictionary<Pos, bool> Parse(IEnumerable<string> input)
        {
            var result = new Dictionary<Pos, bool>();
            int row = 0;
            foreach (var line in input)
            {
                var nums = new List<int>();
                int col = 0;
                foreach (char c in line)
                {
                    // . floor
                    // L empty seet
                    // # occupied seat
                    if (c == 'L')
                    {
                        result[new Pos(col, row)] = false;
                    }
                    col++;
                }
                row++;
            }
            return result;
        }

        string example1 = @"L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL";

        [Test]
        public void Part1_Example1()
        {

            var seats = Parse(Common.GetLines(example1));
            Print(seats);
            var last = seats;
            var equal = false;
            while (!equal)
            {
                seats = Iterate(seats);
                //Print(seats);
                equal = true;
                foreach (var key in seats.Keys)
                {
                    if (last[key] != seats[key])
                    {
                        equal = false;
                        break;
                    }
                }
                if (equal) break;
                last = seats;
            }
            Print(seats);

            Assert.AreEqual(37, seats.Values.Where(x => x == true).Count());
        }

        private static void Print(Dictionary<Pos, bool> seats)
        {
            int X = seats.Select(kv => kv.Key.x).Max();
            int Y = seats.Select(kv => kv.Key.y).Max();
            for (int y = 0; y <= Y; y++)
            {
                var line = new StringBuilder();
                for (int x = 0; x <= X; x++)
                {
                    var p = new Pos(x, y);
                    if (seats.ContainsKey(p))
                    {
                        if (seats[p])
                        {
                            line.Append('#');
                        }
                        else
                        {
                            line.Append('L');
                        }
                    }
                    else
                    {
                        line.Append('.');
                    }
                }
                Console.WriteLine(line.ToString());
            }
            Console.WriteLine();
        }

        private static Dictionary<Pos, bool> Iterate(Dictionary<Pos, bool> seats)
        {
            var result = new Dictionary<Pos, bool>();
            var directions = new[] { (0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1) }.Select(t => new Pos(t.Item1, t.Item2));
            foreach (var key in seats.Keys)
            {
                int count = directions.Where(d => seats.ContainsKey(key + d) && seats[key + d]).Count();
                if (!seats[key] && count == 0)
                {
                    result[key] = true;
                }
                else if (seats[key] && count >= 4)
                {
                    result[key] = false;
                }
                else
                {
                    result[key] = seats[key];
                }
            }

            return result;
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
            var seats = Parse(Common.DayInput(nameof(Day11)));
            Print(seats);
            var last = seats;
            var equal = false;
            while (!equal)
            {
                seats = Iterate(seats);
                //Print(seats);
                equal = true;
                foreach (var key in seats.Keys)
                {
                    if (last[key] != seats[key])
                    {
                        equal = false;
                        break;
                    }
                }
                if (equal) break;
                last = seats;
            }
            Print(seats);

            Assert.AreEqual(2438, seats.Values.Where(x => x == true).Count());
        }

        [Test]
        public void Part2_Example1()
        {
            var seats = Parse(Common.GetLines(example1));
            Print(seats);
            var last = seats;
            while (true)
            {
                seats = Iterate2(seats);
                Print(seats);

                if (AreEqual(seats, last)) break;
                last = seats;
            }
            Print(seats);

            Assert.AreEqual(26, seats.Values.Where(x => x == true).Count());
        }

        private static bool AreEqual(Dictionary<Pos, bool> seats, Dictionary<Pos, bool> last)
        {
            foreach (var key in seats.Keys)
            {
                if (last[key] != seats[key])
                {
                    return false;
                }
            }
            return true;
        }

        private static Dictionary<Pos, bool> Iterate2(Dictionary<Pos, bool> seats)
        {
            int X = seats.Select(kv => kv.Key.x).Max();
            int Y = seats.Select(kv => kv.Key.y).Max();

            var result = new Dictionary<Pos, bool>();
            var directions = new[] { (0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1) }.Select(t => new Pos(t.Item1, t.Item2));
            foreach (var key in seats.Keys)
            {
                int count = 0;
                foreach (var d in directions)
                {
                    var p = key;
                    while (p.x >= 0 && p.y >= 0 && p.x <= X && p.y <= Y)
                    {
                        p += d;
                        if (seats.ContainsKey(p))
                        {
                            if (seats[p]) count++;
                            break;
                        }

                    }

                }
                if (!seats[key] && count == 0)
                {
                    result[key] = true;
                }
                else if (seats[key] && count >= 5)
                {
                    result[key] = false;
                }
                else
                {
                    result[key] = seats[key];
                }
            }

            return result;
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
            var seats = Parse(Common.DayInput(nameof(Day11)));
            Print(seats);
            var last = seats;
            while (true)
            {
                seats = Iterate2(seats);
                Print(seats);

                if (AreEqual(seats, last)) break;
                last = seats;
            }
            Print(seats);

            Assert.AreEqual(26, seats.Values.Where(x => x == true).Count());

        }

    }
}
