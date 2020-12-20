using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    public class Day20
    {
        public enum Side { Top, Right, Bottom, Left}

        public class Tile
        {

            private static List<Side[]> Transforms = new List<Side[]>()
            {
                // unit
                {new Side[4]{Side.Top, Side.Right, Side.Bottom, Side.Left }},

                // rotations
                {new Side[4]{Side.Top, Side.Right, Side.Bottom, Side.Left }},
                {new Side[4]{Side.Right, Side.Bottom, Side.Left, Side.Top }},
                {new Side[4]{Side.Bottom, Side.Left, Side.Top, Side.Right }},
                {new Side[4]{Side.Left, Side.Top, Side.Right, Side.Bottom }},

                // flips
                {new Side[4]{Side.Bottom, Side.Right, Side.Top, Side.Left }},
                {new Side[4]{Side.Top, Side.Left, Side.Bottom, Side.Right }},
            };

            private bool initialized = false;
            public int[] sides = new int[4];
            public bool[,] values = new bool[10, 10];

            public void Init()
            {
                if (initialized) return;
                initialized = true;

                // top
                {
                    int x = 0;
                    for (int i = 0; i < 10; i++)
                    {
                        x <<= 1;
                        if (values[i, 0])
                        {
                            x |= 0x1;
                        }
                    }
                    sides[(int) Side.Top] = x;        
                }

                // right
                {
                    int x = 0;
                    for (int i = 0; i < 10; i++)
                    {
                        x <<= 1;
                        if (values[9, i])
                        {
                            x |= 0x1;
                        }
                    }
                    sides[(int)Side.Right] = x;
                }

                // bottom
                {
                    int x = 0;
                    for (int i = 0; i < 10; i++)
                    {
                        x <<= 1;
                        if (values[i, 9])
                        {
                            x |= 0x1;
                        }
                    }
                    sides[(int)Side.Bottom] = x;
                }

                // left
                {
                    int x = 0;
                    for (int i = 0; i < 10; i++)
                    {
                        x <<= 1;
                        if (values[0, i])
                        {
                            x |= 0x1;
                        }
                    }
                    sides[(int)Side.Left] = x;
                }
            }

            public override string ToString()
            {
                var result = new StringBuilder();
                for (int j = 0; j < 10; j++)
                {
                    if (j != 0) result.Append("\n");
                    for (int i = 0; i < 10; i++)
                    {
                        result.Append(values[i, j] ? '#' : '.');
                    }
                }
                return result.ToString();
            }
        }

        private static Dictionary<int, Tile> Parse(IEnumerable<string> input)
        {
            var result = new Dictionary<int, Tile>();
            Tile tile = null;
            int? key = null;
            int j = 0;
            foreach (var line in input)
            {
                if (line.StartsWith("Tile"))
                {
                    if (key != null)
                    {
                        int v = key ?? default(int);
                        tile.Init();
                        result[v] = tile;
                        j = 0;
                    }
                    key = int.Parse(Regex.Match(line, @"\d+").Value);
                    tile = new Tile();
                }
                else if (line.Any())
                {
                    Assert.AreEqual(10, line.Length);
                    for (int i = 0; i < 10; i++)
                    {
                        tile.values[i, j] = line[i] == '#' ? true : false;
                    }
                    j++;
                }
            }
            if (key != null)
            {
                int v = key ?? default(int);
                tile.Init();
                result[v] = tile;
            }
            return result;
        }

        [Test]
        public void Part1_Example1()
        {
            string input = @"Tile 2311:
..##.#..#.
##..#.....
#...##..#.
####.#...#
##.##.###.
##...#.###
.#.#.#..##
..#....#..
###...#.#.
..###..###

Tile 1951:
#.##...##.
#.####...#
.....#..##
#...######
.##.#....#
.###.#####
###.##.##.
.###....#.
..#.#..#.#
#...##.#..

Tile 1171:
####...##.
#..##.#..#
##.#..#.#.
.###.####.
..###.####
.##....##.
.#...####.
#.##.####.
####..#...
.....##...

Tile 1427:
###.##.#..
.#..#.##..
.#.##.#..#
#.#.#.##.#
....#...##
...##..##.
...#.#####
.#.####.#.
..#..###.#
..##.#..#.

Tile 1489:
##.#.#....
..##...#..
.##..##...
..#...#...
#####...#.
#..#.#.#.#
...#.#.#..
##.#...##.
..##.##.##
###.##.#..

Tile 2473:
#....####.
#..#.##...
#.##..#...
######.#.#
.#...#.#.#
.#########
.###.#..#.
########.#
##...##.#.
..###.#.#.

Tile 2971:
..#.#....#
#...###...
#.#.###...
##.##..#..
.#####..##
.#..####.#
#..#.#..#.
..####.###
..#.#.###.
...#.#.#.#

Tile 2729:
...#.#.#.#
####.#....
..#.#.....
....#..#.#
.##..##.#.
.#.####...
####.#.#..
##.####...
##..#.##..
#.##...##.

Tile 3079:
#.#.#####.
.#..######
..#.......
######....
####.#..#.
.#...#.##.
#.#####.##
..#.###...
..#.......
..#.###...";
            var parsed = Parse(Common.GetLines(input));

            foreach (var tile in parsed)
            {
                Console.WriteLine(tile.Key);
                Console.WriteLine(string.Join(",", tile.Value.sides.Select((x, i) => $"{(Side) i}: {x}")));
                Console.WriteLine(tile.Value);
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
            var parsed = Parse(Common.DayInput(nameof(Day20)));
            Assert.AreEqual(0, 1);
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
            var parsed = Parse(Common.DayInput(nameof(Day20)));
            Assert.AreEqual(0, 1);
        }

    }
}
