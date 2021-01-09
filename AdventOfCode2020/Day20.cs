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

        public class Tile : IEquatable<Tile>
        {

            private static List<Side[]> Transforms = new List<Side[]>()
            {
                // unit
                {new Side[4]{Side.Top, Side.Right, Side.Bottom, Side.Left }},

                // rotations
                {new Side[4]{Side.Right, Side.Bottom, Side.Left, Side.Top }},
                {new Side[4]{Side.Bottom, Side.Left, Side.Top, Side.Right }},
                {new Side[4]{Side.Left, Side.Top, Side.Right, Side.Bottom }},

                // flips
                {new Side[4]{Side.Bottom, Side.Right, Side.Top, Side.Left }},
                {new Side[4]{Side.Top, Side.Left, Side.Bottom, Side.Right }},
            };

            public int[] Sides = new int[4];
            public int[] InvertedSides = new int[4];
            public bool[,] Values = new bool[10, 10];

            private DefaultDictionary<Side, int> neighbourCount = new DefaultDictionary<Side, int>();
            private DefaultDictionary<Side, int> neighboursInvertedCount = new DefaultDictionary<Side, int>();
            public DefaultValueDictionary<Side, Tile> neighbourTiles = new DefaultValueDictionary<Side, Tile>(() => null);
            private readonly int identifier;

            public int Identifier => identifier;

            public IEnumerator<KeyValuePair<Side, Tile>> Neighbours { get => neighbourTiles.GetEnumerator(); }

            public Tile(int identifier)
            {
                this.identifier = identifier;
            }

            public void Init()
            {
                // top
                {
                    int x = 0;
                    for (int i = 0; i < Values.GetLength(0); i++)
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
                    for (int i = Values.GetLength(0) - 1; i >= 0; i--)
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
                    for (int i = 0; i < Values.GetLength(0); i++)
                    {
                        x <<= 1;
                        if (Values[Values.GetLength(0) - 1, i])
                        {
                            x |= 0x1;
                        }
                    }
                    Sides[(int)Side.Right] = x;
                }

                // inverted right
                {
                    int x = 0;
                    for (int i = Values.GetLength(0) - 1; i >= 0; i--)
                    {
                        x <<= 1;
                        if (Values[Values.GetLength(0) - 1, i])
                        {
                            x |= 0x1;
                        }
                    }
                    InvertedSides[(int)Side.Right] = x;
                }

                // bottom
                {
                    int x = 0;
                    for (int i = 0; i < Values.GetLength(0); i++)
                    {
                        x <<= 1;
                        if (Values[i, Values.GetLength(0) - 1])
                        {
                            x |= 0x1;
                        }
                    }
                    Sides[(int)Side.Bottom] = x;
                }

                // inverted bottom
                {
                    int x = 0;
                    for (int i = Values.GetLength(0) - 1; i >= 0; i--)
                    {
                        x <<= 1;
                        if (Values[i, Values.GetLength(0) - 1])
                        {
                            x |= 0x1;
                        }
                    }
                    InvertedSides[(int)Side.Bottom] = x;
                }

                // left
                {
                    int x = 0;
                    for (int i = 0; i < Values.GetLength(0); i++)
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
                    for (int i = Values.GetLength(0) - 1; i >= 0; i--)
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

            public void VerticalFlip()
            {
                bool[,] result = new bool[Values.GetLength(0), Values.GetLength(1)];
                for (int j = 0; j < Values.GetLength(1); j++)
                {
                    for (int i = 0; i < Values.GetLength(0); i++)
                    {
                        int x = i;
                        int y = Values.GetLength(1) - 1 - j;
                        result[x, y] = Values[i, j];
                    }
                }
                Values = result;

                var tmp = neighbourTiles[Side.Top];
                neighbourTiles[Side.Top] = neighbourTiles[Side.Bottom];
                neighbourTiles[Side.Bottom] = tmp;
                
                Init();
            }

            public void HorizontalFlip()
            {
                bool[,] result = new bool[Values.GetLength(0), Values.GetLength(1)];
                for (int j = 0; j < Values.GetLength(1); j++)
                {
                    for (int i = 0; i < Values.GetLength(0); i++)
                    {
                        int x = Values.GetLength(0) - 1 - i;
                        int y = j;
                        result[x, y] = Values[i, j];
                    }
                }
                Values = result;

                var tmp = neighbourTiles[Side.Right];
                neighbourTiles[Side.Right] = neighbourTiles[Side.Left];
                neighbourTiles[Side.Left] = tmp;

                Init();
            }

            public void Rotate90()
            {
                Assert.AreEqual(Values.GetLength(0), Values.GetLength(1));
                bool[,] result = new bool[Values.GetLength(0), Values.GetLength(1)];
                for (int j = 0; j < Values.GetLength(1); j++)
                {
                    for (int i = 0; i < Values.GetLength(0); i++)
                    {
                        int x = Values.GetLength(0) - 1 - j;
                        int y = i;
                        result[x, y] = Values[i, j];
                    }
                }

                Values = result;

                var tmp = neighbourTiles[Side.Right];
                neighbourTiles[Side.Right] = neighbourTiles[Side.Top];
                neighbourTiles[Side.Top] = neighbourTiles[Side.Left];
                neighbourTiles[Side.Left] = neighbourTiles[Side.Bottom];
                neighbourTiles[Side.Bottom] = tmp;

                Init();
            }

            public override string ToString()
            {
                var result = new StringBuilder();
                result.Append(identifier);
                result.Append('\n');
                for (int j = 0; j < Values.GetLength(1); j++)
                {
                    if (j != 0) result.Append("\n");
                    for (int i = 0; i < Values.GetLength(0); i++)
                    {
                        result.Append(Values[i, j] ? '#' : '.');
                    }
                }
                return result.ToString();
            }

            internal bool Neighbour(Tile other)
            {
                var result = false;
                foreach (var k in other.Sides.Concat(other.InvertedSides))
                {
                    for (int i = 0; i < Sides.Length; i++)
                    {
                        if (Sides[i] == k)
                        {
                            neighbourTiles[(Side)i] = other;
                            neighbourCount[(Side)i]++;
                            result = true;
                        }
                        if (InvertedSides[i] == k)
                        {
                            neighbourTiles[(Side)i] = other;
                            neighboursInvertedCount[(Side)i]++;
                            result = true;
                        }
                    }
                }
                return result;
            }

            public void PrintNeighbours()
            {
                foreach (var kv in neighbourTiles)
                {
                    Console.WriteLine($"{kv.Key}: {string.Join(",", kv.Value.Identifier)}");
                }
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Tile);
            }

            public bool Equals(Tile other)
            {
                return other != null &&
                       identifier == other.identifier;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(identifier);
            }

            public static bool operator ==(Tile left, Tile right)
            {
                return EqualityComparer<Tile>.Default.Equals(left, right);
            }

            public static bool operator !=(Tile left, Tile right)
            {
                return !(left == right);
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
                    tile = new Tile(key ?? throw new NullReferenceException(nameof(key)));
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

        private static Dictionary<int, int> CountNeighbours(KeyValuePair<int, Tile>[] parsed)
        {
            var nCount = new DefaultDictionary<int, int>();
            for (int i = 0; i < parsed.Length - 1; i++)
            {
                var t1 = parsed[i];
                for (int j = i + 1; j < parsed.Length; j++)
                {
                    var t2 = parsed[j];
                    if (t1.Value.Neighbour(t2.Value) && t2.Value.Neighbour(t1.Value))
                    {
                        nCount[t1.Key]++;
                        nCount[t2.Key]++;
                    }
                }
            }
            return nCount.Inner;
        }

        string example = @"Tile 2311:
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

        [Test]
        public void Part1_Example1()
        {
            var tiles = Parse(Common.GetLines(example)).ToArray();
            Dictionary<int, int> nCount = CountNeighbours(tiles);
            var min = Convert.ToInt64(nCount.Select(x => x.Value).Min());
            long result = 1L;
            foreach (var kv in nCount)
            {
                if (kv.Value == min)
                {
                    result *= kv.Key;
                }
            }
            Assert.AreEqual(20899048083289, result);
        }

        [Test]
        public void Part1()
        {
            var tiles = Parse(Common.DayInput(nameof(Day20))).ToArray();
            Dictionary<int, int> nCount = CountNeighbours(tiles);
            var min = Convert.ToInt64(nCount.Select(x => x.Value).Min());
            long result = 1L;
            foreach (var kv in nCount)
            {
                if (kv.Value == min)
                {
                    result *= kv.Key;
                }
            }
            Assert.AreEqual(68781323018729, result);
        }

        [Test]
        public void Part2_Example1()
        {
            var tiles = Parse(Common.GetLines(example));
            Dictionary<int, int> nCount = CountNeighbours(tiles.ToArray());

            var firstCorner = tiles[nCount.Where(k => k.Value == nCount.Select(x => x.Value).Min()).First().Key];

            var tilePos = new Dictionary<Pos, Tile>();
            {
                var pos = new Pos(0, 0);
                tilePos[new Pos(pos)] = firstCorner;
                findNeighbours(firstCorner, ref tilePos, pos);
            }

            var min = new Pos(int.MaxValue, int.MaxValue);
            var max = new Pos(int.MinValue, int.MinValue);
            foreach (var pos in tilePos.Keys)
            {
                min.x = Math.Min(pos.x, min.x);
                min.y = Math.Min(pos.y, min.y);
                max.x = Math.Max(pos.x, max.x);
                max.y = Math.Max(pos.y, max.y);
            }

            char[,] sea;
            {
                var diff = max - min;
                sea = new char[(diff.x + 1) * (firstCorner.Values.GetLength(0) + 1), (diff.y + 1) * (firstCorner.Values.GetLength(1) + 1)];
                for (int j = 0; j < sea.GetLength(0); j++)
                    for (int i = 0; i < sea.GetLength(1); i++)
                        sea[i, j] = ' ';
            }

            for (int j = min.y; j <= max.y; j++)
            {
                for (int i = min.x; i <= max.x; i++)
                {
                    var pos = new Pos(i, j);
                    var diff = pos - min;
                    if (tilePos.TryGetValue(pos, out Tile tile))
                    {
                        var tileX = diff.x * (tile.Values.GetLength(0) + 1);
                        var tileY = diff.y * (tile.Values.GetLength(1) + 1);
                        for (int y = 0; y < tile.Values.GetLength(1); y++)
                        {
                            for (int x = 0; x < tile.Values.GetLength(0); x++)
                            {
                                sea[tileX + x, tileY + y] = tile.Values[x, y] ? '#' : '.';
                            }
                        }
                        //Console.WriteLine(tile);
                    }
                }
            }

            var seaText = new StringBuilder();
            for (int j = 0; j < sea.GetLength(1); j++)
            {
                if (j != 0)
                {
                    seaText.Append("\n");
                }
                for (int i = 0; i < sea.GetLength(0); i++)
                {
                    seaText.Append(sea[i, j]);
                }
            }
            Console.WriteLine(seaText);
        }


        Dictionary<Side, Pos> sideDiff = new Dictionary<Side, Pos>()
        { 
            { Side.Top, new Pos(0, -1) },
            { Side.Right, new Pos(1, 0) },
            { Side.Bottom, new Pos(0, 1) },
            { Side.Left, new Pos(-1, 0) },
        };
        private void findNeighbours(Tile tile, ref Dictionary<Pos, Tile> tilePos, Pos pos)
        {
            foreach (var sideTile in tile.neighbourTiles)
            {
                var neighbourPos = pos + sideDiff[sideTile.Key];
                var neighbour = sideTile.Value;
                if (neighbour != null && !tilePos.ContainsValue(neighbour))
                {
                    tilePos[neighbourPos] = neighbour;
                    findNeighbours(neighbour, ref tilePos, neighbourPos);
                }
            }
        }

        [Test]
        public void Part2()
        {
            var parsed = Parse(Common.DayInput(nameof(Day20)));
            Assert.AreEqual(0, 1);
        }

    }
}
