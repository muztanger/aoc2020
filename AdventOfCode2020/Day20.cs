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
            public int[] Sides = new int[4];
            public int[] InvertedSides = new int[4];
            public bool[,] Values = new bool[10, 10];

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
                        if (Values[i, 0])
                        {
                            x |= 0x1;
                        }
                    }
                    Sides[(int) Side.Top] = x;        
                }

                // inverted top
                {
                    int x = 0;
                    for (int i = 9; i >= 0; i--)
                    {
                        x <<= 1;
                        if (Values[i, 0])
                        {
                            x |= 0x1;
                        }
                    }
                    InvertedSides[(int)Side.Top] = x;
                }

                // right
                {
                    int x = 0;
                    for (int i = 0; i < 10; i++)
                    {
                        x <<= 1;
                        if (Values[9, i])
                        {
                            x |= 0x1;
                        }
                    }
                    Sides[(int)Side.Right] = x;
                }

                // inverted right
                {
                    int x = 0;
                    for (int i = 9; i >= 0; i--)
                    {
                        x <<= 1;
                        if (Values[9, i])
                        {
                            x |= 0x1;
                        }
                    }
                    InvertedSides[(int)Side.Right] = x;
                }

                // bottom
                {
                    int x = 0;
                    for (int i = 0; i < 10; i++)
                    {
                        x <<= 1;
                        if (Values[i, 9])
                        {
                            x |= 0x1;
                        }
                    }
                    Sides[(int)Side.Bottom] = x;
                }

                // inverted bottom
                {
                    int x = 0;
                    for (int i = 9; i >= 0; i--)
                    {
                        x <<= 1;
                        if (Values[i, 9])
                        {
                            x |= 0x1;
                        }
                    }
                    InvertedSides[(int)Side.Bottom] = x;
                }

                // left
                {
                    int x = 0;
                    for (int i = 0; i < 10; i++)
                    {
                        x <<= 1;
                        if (Values[0, i])
                        {
                            x |= 0x1;
                        }
                    }
                    Sides[(int)Side.Left] = x;
                }

                // inverted left
                {
                    int x = 0;
                    for (int i = 9; i >= 0; i--)
                    {
                        x <<= 1;
                        if (Values[0, i])
                        {
                            x |= 0x1;
                        }
                    }
                    InvertedSides[(int)Side.Left] = x;
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
                        result.Append(Values[i, j] ? '#' : '.');
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
                        tile.Values[i, j] = line[i] == '#' ? true : false;
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


            var nCount = new DefaultDictionary<int, int>();
            foreach (var t1 in parsed)
            {
                foreach (var t2 in parsed)
                {
                    if (t1.Key == t2.Key) continue;
                    foreach (var k in t1.Value.Sides)
                    {
                        if (t2.Value.Sides.Contains(k) || t2.Value.InvertedSides.Contains(k))
                        {
                            nCount[t1.Key]++;
                        }
                    }
                    foreach (var k in t1.Value.InvertedSides)
                    {
                        if (t2.Value.Sides.Contains(k) || t2.Value.InvertedSides.Contains(k))
                        {
                            nCount[t1.Key]++;
                        }
                    }
                }
            }
            var min = Convert.ToInt64(nCount.Inner.Select(x => x.Value).Min());
            long result = 1L;
            foreach (var kv in nCount.Inner)
            {
                if (kv.Value == min)
                {
                    result *= kv.Key;
                }
            }
            Assert.AreEqual(20899048083289, result);
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
            var nCount = new DefaultDictionary<int, int>();
            foreach (var t1 in parsed)
            {
                foreach (var t2 in parsed)
                {
                    if (t1.Key == t2.Key) continue;
                    foreach (var k in t1.Value.Sides)
                    {
                        if (t2.Value.Sides.Contains(k) || t2.Value.InvertedSides.Contains(k))
                        {
                            nCount[t1.Key]++;
                        }
                    }
                    foreach (var k in t1.Value.InvertedSides)
                    {
                        if (t2.Value.Sides.Contains(k) || t2.Value.InvertedSides.Contains(k))
                        {
                            nCount[t1.Key]++;
                        }
                    }
                }
            }
            var min = Convert.ToInt64(nCount.Inner.Select(x => x.Value).Min());
            long result = 1L;
            foreach (var kv in nCount.Inner)
            {
                if (kv.Value == min)
                {
                    result *= kv.Key;
                }
            }
            Assert.AreEqual(20899048083289, result);
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
