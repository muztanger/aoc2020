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
        public enum Side { Up, Right, Down, Left}

        public class Tile : IEquatable<Tile>
        {

            private static List<Side[]> Transforms = new List<Side[]>()
            {
                // unit
                {new Side[4]{Side.Up, Side.Right, Side.Down, Side.Left }},

                // rotations
                {new Side[4]{Side.Right, Side.Down, Side.Left, Side.Up }},
                {new Side[4]{Side.Down, Side.Left, Side.Up, Side.Right }},
                {new Side[4]{Side.Left, Side.Up, Side.Right, Side.Down }},

                // flips
                {new Side[4]{Side.Down, Side.Right, Side.Up, Side.Left }},
                {new Side[4]{Side.Up, Side.Left, Side.Down, Side.Right }},
            };

            public int[] Sides = new int[4];
            public int[] InvertedSides = new int[4];
            public bool[,] Values = new bool[10, 10];

            private DefaultDictionary<Side, int> neighbourCount = new DefaultDictionary<Side, int>();
            private DefaultDictionary<Side, int> neighboursInvertedCount = new DefaultDictionary<Side, int>();
            public DefaultValueDictionary<Side, Tile> Neighbours = new DefaultValueDictionary<Side, Tile>(() => null);
            private readonly int identifier;

            public int Identifier => identifier;

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
                    Sides[(int) Side.Up] = x;        
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
                    InvertedSides[(int)Side.Up] = x;
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
                    Sides[(int)Side.Down] = x;
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
                    InvertedSides[(int)Side.Down] = x;
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

                var tmp = Neighbours[Side.Up];
                Neighbours[Side.Up] = Neighbours[Side.Down];
                Neighbours[Side.Down] = tmp;
                
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

                var tmp = Neighbours[Side.Right];
                Neighbours[Side.Right] = Neighbours[Side.Left];
                Neighbours[Side.Left] = tmp;

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

                var tmp = Neighbours[Side.Right];
                Neighbours[Side.Right] = Neighbours[Side.Up];
                Neighbours[Side.Up] = Neighbours[Side.Left];
                Neighbours[Side.Left] = Neighbours[Side.Down];
                Neighbours[Side.Down] = tmp;

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
                            Neighbours[(Side)i] = other;
                            neighbourCount[(Side)i]++;
                            result = true;
                        }
                        if (InvertedSides[i] == k)
                        {
                            Neighbours[(Side)i] = other;
                            neighboursInvertedCount[(Side)i]++;
                            result = true;
                        }
                    }
                }
                return result;
            }

            public void PrintNeighbours()
            {
                foreach (var kv in Neighbours)
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

            internal bool IsUp(Tile up)
            {
                return up.Sides[(int)Side.Down] == Sides[(int)Side.Up];
            }

            internal bool IsLeft(Tile left)
            {
                return left.Sides[(int) Side.Right] == Sides[(int) Side.Left];
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

            var cornerIds = nCount.Where(k => k.Value == nCount.Select(x => x.Value).Min()).Select(kv => kv.Key);
            Tile firstCorner = null;
            foreach (var cornerId in cornerIds)
            {
                var tile = tiles[cornerId];
                var neighbours = tile.Neighbours.Inner;
                if (neighbours.ContainsKey(Side.Right) && neighbours.ContainsKey(Side.Down))
                {
                    firstCorner = tile;
                    break;
                }
            }
            Assert.NotNull(firstCorner);

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
                sea = new char[(diff.x + 1) * (firstCorner.Values.GetLength(0) - 2), (diff.y + 1) * (firstCorner.Values.GetLength(1) - 2)];
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
                        var tileX = diff.x * (tile.Values.GetLength(0) - 2);
                        var tileY = diff.y * (tile.Values.GetLength(1) - 2);
                        for (int y = 1; y < tile.Values.GetLength(1) - 1; y++)
                        {
                            for (int x = 1; x < tile.Values.GetLength(0) - 1; x++)
                            {
                                sea[tileX + x - 1, tileY + y - 1] = tile.Values[x, y] ? '#' : '.';
                            }
                        }
                    }
                }
            }

            var monster = new char[,] {
                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#', ' '},
                {'#', ' ', ' ', ' ', ' ', '#', '#', ' ', ' ', ' ', ' ', '#', '#', ' ', ' ', ' ', ' ', '#', '#', '#'},
                {' ', '#', ' ', ' ', '#', ' ', ' ', '#', ' ', ' ', '#', ' ', ' ', '#', ' ', ' ', '#', ' ', ' ', ' '},
            };

            var monsters = new List<Pos>();
            
            bool TryRotations()
            {
                for (int i = 0; i < 4; i++)
                {
                    monsters = FindMonsters(sea, monster);
                    if (monsters.Count > 0)
                        return true;
                    sea = Rotate90(sea);
                }
                return false;
            }

            if (!TryRotations())
            {
                sea = HorisontalFlip(sea);
            }
            if (monsters.Count == 0 && !TryRotations())
            {
                sea = HorisontalFlip(sea); // flip back
                sea = VerticalFlip(sea);
                TryRotations();
            }

            Console.WriteLine($"monsterCount={monsters.Count}");

            // print the monsters
            foreach (var pos in monsters)
            {
                for (int j = 0; j < monster.GetLength(0); j++)
                {
                    for (int i = 0; i < monster.GetLength(1); i++)
                    {
                        if (monster[j, i] == '#')
                        {
                            sea[pos.x + i, pos.y + j] = 'O';
                        }
                    }
                }
            }

            PrintSea(sea);

            // count remaining #
            var result = 0;
            foreach (var c in sea)
            {
                if (c == '#')
                    result++;
            }

            Assert.AreEqual(273, result);
        }

        private static void PrintSea(char[,] sea)
        {
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

        private static char[,] Rotate90(char[,] sea)
        {
            Assert.AreEqual(sea.GetLength(0), sea.GetLength(1));

            var result = new char[sea.GetLength(0), sea.GetLength(1)];
            for (int j = 0; j < sea.GetLength(1); j++)
            {
                for (int i = 0; i < sea.GetLength(0); i++)
                {
                    int x = sea.GetLength(0) - 1 - j;
                    int y = i;
                    result[y, x] = sea[j, i];
                }
            }
            return result;
        }

        private static char[,] HorisontalFlip(char[,] sea)
        {
            var result = new char[sea.GetLength(0), sea.GetLength(1)];
            for (int j = 0; j < sea.GetLength(1); j++)
            {
                for (int i = 0; i < sea.GetLength(0); i++)
                {
                    int x = sea.GetLength(0) - 1 - i;
                    int y = j;
                    result[y, x] = sea[j, i];
                }
            }
            return result;
        }

        private static char[,] VerticalFlip(char[,] sea)
        {
            var result = new char[sea.GetLength(0), sea.GetLength(1)];
            for (int j = 0; j < sea.GetLength(1); j++)
            {
                for (int i = 0; i < sea.GetLength(0); i++)
                {
                    int x = i;
                    int y = sea.GetLength(1) - 1 - j;
                    result[y, x] = sea[j, i];
                }
            }
            return result;
        }

        private static List<Pos> FindMonsters(char[,] sea, char[,] monster)
        {
            var monsters = new List<Pos>();
            for (var x = 0; x < sea.GetLength(0) - monster.GetLength(1); x++)
            {
                for (var y = 0; y < sea.GetLength(1) - monster.GetLength(0); y++)
                {
                    if (CheckMonster(sea, monster, x, y))
                    {
                        var pos = new Pos(x, y);
                        monsters.Add(pos);
                        Console.WriteLine($"Monster at pos={pos}");
                    }
                }
            }
            return monsters;
        }

        private static bool CheckMonster(char[,] sea, char[,] monster, int x, int y)
        {
            var isMonster = true;
            for (int j = 0; j < monster.GetLength(0) && isMonster; j++)
            {
                for (int i = 0; i < monster.GetLength(1) && isMonster; i++)
                {
                    if (monster[j, i] == '#')
                    {
                        isMonster = sea[x + i, y + j] == '#';
                    }
                }
            }

            return isMonster;
        }

        Dictionary<Side, Pos> sideDiff = new Dictionary<Side, Pos>()
        { 
            { Side.Up, new Pos(0, -1) },
            { Side.Right, new Pos(1, 0) },
            { Side.Down, new Pos(0, 1) },
            { Side.Left, new Pos(-1, 0) },
        };
        private void findNeighbours(Tile tile, ref Dictionary<Pos, Tile> tilePos, Pos pos)
        {
            tilePos.TryGetValue(pos + sideDiff[Side.Up], out var up);
            tilePos.TryGetValue(pos + sideDiff[Side.Left], out var left);

            while (!IsMatch())
            {
                if (TryRotations())
                    break;
                tile.HorizontalFlip();
                if (TryRotations())
                    break;
                tile.HorizontalFlip(); // flip back
                tile.VerticalFlip();
                if (TryRotations())
                    break;
            }

            bool IsMatch()
            {
                bool match = left == null || tile.IsLeft(left);
                match &= up == null || tile.IsUp(up);
                return match;
            }

            bool TryRotations()
            {
                for (int i = 0; i < 3; i++)
                {
                    tile.Rotate90();
                    if (IsMatch())
                    {
                        return true;
                    }
                }
                tile.Rotate90();
                return IsMatch();
            }

            if (tile.Neighbours.Inner.TryGetValue(Side.Right, out Tile right) && right != null)
            {
                var rightPos = pos + sideDiff[Side.Right];
                tilePos[rightPos] = right;
                findNeighbours(right, ref tilePos, rightPos);
            }
            if (tile.Neighbours.Inner.TryGetValue(Side.Down, out Tile down) && down != null)
            {
                var downPos = pos + sideDiff[Side.Down];
                tilePos[downPos] = down;
                findNeighbours(down, ref tilePos, downPos);
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
