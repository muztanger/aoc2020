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
            var upperLeft = new Pos(0, 0);
            var lowerRight = new Pos(7, 127);
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
                            var min = Math.Min(upperLeft.y, lowerRight.y);
                            upperLeft.y = min;
                            lowerRight.y = min;
                        }
                        else
                            lowerRight.y -= height;
                        break;
                    case 'B':
                        height /= 2;
                        if (height == 1)
                        {
                            var max = Math.Max(upperLeft.y, lowerRight.y);
                            upperLeft.y = max;
                            lowerRight.y = max;
                        }
                        else
                            upperLeft.y += height;
                        break;
                    case 'L':
                        width /= 2;
                        if (width == 1)
                        {
                            var min = Math.Min(upperLeft.x, lowerRight.x);
                            upperLeft.x = min;
                            lowerRight.x = min;
                        }
                        else
                            lowerRight.x -= width;
                        break;
                    case 'R':
                        width /= 2;
                        if (width == 1)
                        {
                            var max = Math.Max(upperLeft.x, lowerRight.x);
                            upperLeft.x = max;
                            lowerRight.x = max;
                        }
                        else
                            upperLeft.x += width;
                        break;
                    default:
                        Console.WriteLine("Unhandled");
                        break;
                }

            }
            var result = new Seat
            {
                Column = lowerRight.x,
                Row = lowerRight.y
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
