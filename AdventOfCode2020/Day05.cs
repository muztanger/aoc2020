using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class Square
    {
        public Pos ColumnRange = new Pos(0, 7);
        public Pos RowRange = new Pos(0, 127);
        public int Width => ColumnRange.Dist() + 1;
        public int Height => RowRange.Dist() + 1;
        public void SplitF()
        {
            var height = Height / 2;
            if (height > 1)
            {
                RowRange.y -= height;
            }
        }

        public void SplitB()
        {
            var height = Height / 2;
            if (height > 1)
            {
                RowRange.x += height;
            }
            else
            {
                RowRange.x = RowRange.y;
            }
        }
        public void SplitL()
        {
            var width = Width / 2;
            if (width > 1)
            {
                ColumnRange.y -= width;
            }
        }

        public void SplitR()
        {
            var width = Width / 2;
            if (width > 1)
            {
                ColumnRange.x += width;
            }
            else
            {
                ColumnRange.x = ColumnRange.y;
            }
        }

    }

    public class Seat
    {
        public int Row;
        public int Column;
        public int Id =>  Row * 8 + Column;
        public override string ToString()
        {
            return $"row {Row}, column {Column}, seat ID {Id}";
        }

        public static Seat Parse(string line)
        {
            var square = new Square();
            foreach (char c in line)
            {
                switch (c)
                {
                    case 'F':
                        square.SplitF();
                        break;
                    case 'B':
                        square.SplitB();
                        break;
                    case 'L':
                        square.SplitL();
                        break;
                    case 'R':
                        square.SplitR();
                        break;
                    default:
                        Console.WriteLine("Unhandled");
                        break;
                }

            }
            var result = new Seat
            {
                Column = square.ColumnRange.x,
                Row = square.RowRange.x
            };
            return result;
        }
    }

    public class Day05
    {
        [Test]
        public void Part1_Example1()
        {
            string input = @"BFFFBBFRRR
FFFBBBFRRR
BBFFBBFRLL";
            foreach (var line in Common.GetLines(input))
            {
                var seat = Seat.Parse(line);
                Console.WriteLine(seat);
            }
        }

        [Test]
        public void Part1()
        {
            var maxSeat = -1;
            foreach (var line in Common.DayInput(nameof(Day05)))
            {
                var seatClass = Seat.Parse(line);
                maxSeat = Math.Max(maxSeat, seatClass.Id);
            }

            Assert.AreEqual(989, maxSeat);
        }

        [Test]
        public void Part2()
        {
            var max = int.MinValue;
            var min = int.MaxValue;
            var sum = 0;
            foreach (var line in Common.DayInput(nameof(Day05)))
            {
                var seat = Seat.Parse(line);
                max = Math.Max(max, seat.Id);
                min = Math.Min(min, seat.Id);
                sum += seat.Id;
            }
            var result = ConSum(max) - ConSum(min - 1) - sum;

            static int ConSum(int n) => n * (n + 1) / 2;

            Assert.AreEqual(548, result);
        }

    }
}
