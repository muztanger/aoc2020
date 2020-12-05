using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
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
            var ul = new Pos(0, 0);
            var lr = new Pos(7, 127);
            var height = 128;
            var width = 8;
            foreach (char c in line)
            {
                switch (c)
                {
                    case 'F':
                        height /= 2;
                        if (height == 1)
                        {
                            var min = Math.Min(ul.y, lr.y);
                            ul.y = min;
                            lr.y = min;
                        }
                        else
                            lr.y -= height;
                        break;
                    case 'B':
                        height /= 2;
                        if (height == 1)
                        {
                            var max = Math.Max(ul.y, lr.y);
                            ul.y = max;
                            lr.y = max;
                        }
                        else
                            ul.y += height;
                        break;
                    case 'L':
                        width /= 2;
                        if (width == 1)
                        {
                            var min = Math.Min(ul.x, lr.x);
                            ul.x = min;
                            lr.x = min;
                        }
                        else
                            lr.x -= width;
                        break;
                    case 'R':
                        width /= 2;
                        if (width == 1)
                        {
                            var max = Math.Max(ul.x, lr.x);
                            ul.x = max;
                            lr.x = max;
                        }
                        else
                            ul.x += width;
                        break;
                    default:
                        Console.WriteLine("Unhandled");
                        break;
                }

            }
            var result = new Seat
            {
                Column = lr.x,
                Row = lr.y
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
