using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class Day11
    {
        public class Plane
        {
            private Dictionary<Pos, bool> mSeats;
            private readonly Pos mColRange;
            private readonly Pos mRowRange;
            private readonly IEnumerable<Pos> mDirections = new[] { (0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1) }.Select(t => new Pos(t.Item1, t.Item2));

            public Plane(Dictionary<Pos, bool> seats, Pos colRange, Pos rowRange)
            {
                mSeats = seats;
                mColRange = colRange;
                mRowRange = rowRange;
            }

            public static Plane Parse(IEnumerable<string> input)
            {
                var seats = new Dictionary<Pos, bool>();
                int row = 0;
                int col = 0;
                foreach (var line in input)
                {
                    col = 0;
                    foreach (char c in line)
                    {
                        // . floor
                        // L empty seet
                        // # occupied seat
                        if (c == 'L')
                        {
                            seats[new Pos(col, row)] = false;
                        }
                        col++;
                    }
                    row++;
                }
                return new Plane(seats, new Pos(0, col), new Pos(0, row));
            }

            public void Print()
            {
                for (int y = mRowRange.x; y <= mRowRange.y; y++)
                {
                    var line = new StringBuilder();
                    for (int x = mColRange.x; x <= mColRange.y; x++)
                    {
                        var p = new Pos(x, y);
                        if (mSeats.ContainsKey(p))
                        {
                            if (mSeats[p])
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

            public bool Iterate()
            {
                var result = new Dictionary<Pos, bool>();
                bool isChanged = false;
                foreach (var key in mSeats.Keys)
                {
                    int count = mDirections.Where(d => mSeats.ContainsKey(key + d) && mSeats[key + d]).Count();
                    if (!mSeats[key] && count == 0)
                    {
                        result[key] = true;
                        isChanged = true;
                    }
                    else if (mSeats[key] && count >= 4)
                    {
                        result[key] = false;
                        isChanged = true;
                    }
                    else
                    {
                        result[key] = mSeats[key];
                    }
                }
                mSeats = result;
                return isChanged;
            }

            public bool Iterate2()
            {
                var result = new Dictionary<Pos, bool>();
                bool isChanged = false;
                foreach (var key in mSeats.Keys)
                {
                    int count = 0;
                    foreach (var d in mDirections)
                    {
                        var p = key;
                        while (mColRange.BetweenXY(p.x) && mRowRange.BetweenXY(p.y))
                        {
                            p += d;
                            if (mSeats.ContainsKey(p))
                            {
                                if (mSeats[p]) count++;
                                break;
                            }

                        }
                    }

                    if (!mSeats[key] && count == 0)
                    {
                        result[key] = true;
                        isChanged = true;
                    }
                    else if (mSeats[key] && count >= 5)
                    {
                        result[key] = false;
                        isChanged = true;
                    }
                    else
                    {
                        result[key] = mSeats[key];
                    }
                }

                mSeats = result;
                return isChanged;
            }

            public int Count() => mSeats.Values.Where(x => x == true).Count();
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
            var plane = Plane.Parse(Common.GetLines(example1));
            plane.Print();
            while (plane.Iterate())
            {
                plane.Print();
            }
            Assert.AreEqual(37, plane.Count());
        }

        [Test]
        public void Part1()
        {
            var plane = Plane.Parse(Common.DayInput(nameof(Day11)));
            while (plane.Iterate());
            Assert.AreEqual(2438, plane.Count());
        }

        [Test]
        public void Part2_Example1()
        {
            var plane = Plane.Parse(Common.GetLines(example1));
            plane.Print();
            while (plane.Iterate2())
            {
                plane.Print();
            }
            Assert.AreEqual(26, plane.Count());
        }


        [Test]
        public void Part2()
        {
            var plane = Plane.Parse(Common.DayInput(nameof(Day11)));
            while (plane.Iterate2());
            Assert.AreEqual(2174, plane.Count());
        }

    }
}
