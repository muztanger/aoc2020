using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class Day05
    {
        private static List<string> Parse(IEnumerable<string> input)
        {
            return new List<string>(input);
        }

        [Test]
        public void Part1_Example1()
        {
            // F Front
            // B BAck
            // L Left
            // R Right
            // 128 rows
            // seat ID: row * 8 + column
            string input = @"BFFFBBFRRR
FFFBBBFRRR
BBFFBBFRLL";
            var parsed = Parse(Common.GetLines(input));
            foreach (var line in parsed)
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
                    
                    Console.WriteLine($"{c}: ul={ul} lr={lr} widht={width} height={height}");
                }
                Console.WriteLine($"{ul} {lr}");
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
            var parsed = Parse(Common.DayInput(nameof(Day05)));
            var maxSeat = -1;
            foreach (var line in parsed)
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

                    Console.WriteLine($"{c}: ul={ul} lr={lr} widht={width} height={height}");
                }
                Console.WriteLine($"{ul} {lr}");
                var seat = lr.y * 8 + lr.x;
                maxSeat = Math.Max(maxSeat, seat);
            }
            Assert.AreEqual(0, maxSeat);
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
            var parsed = Parse(Common.DayInput(nameof(Day05)));
            Assert.AreEqual(0, 1);
        }

    }
}
