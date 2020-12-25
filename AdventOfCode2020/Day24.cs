using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    public class Day24
    {
        internal class Tile : IEquatable<Tile>
        {
            internal Pos pos;
            internal bool isWhite = true;

            public override bool Equals(object obj)
            {
                return Equals(obj as Tile);
            }

            public bool Equals(Tile other)
            {
                return other != null &&
                       EqualityComparer<Pos>.Default.Equals(pos, other.pos);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(pos);
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

        private static HashSet<Tile> Parse(IEnumerable<string> input)
        {
            var directions = new Pos[2][];
            directions[0] = new Pos[] { new Pos(1, 0), new Pos(0, 1), new Pos(-1, 1), new Pos(-1, 0), new Pos(-1, -1), new Pos(0, -1) }; // even lines
            directions[1] = new Pos[] { new Pos(1, 0), new Pos(1, 1), new Pos(0, 1), new Pos(-1, 0), new Pos(0, -1), new Pos(1, -1) }; // odd lines

            var str2index = new Dictionary<string, int>()
            {
                { "e", 0 },
                { "se", 1 },
                { "sw", 2 },
                { "w", 3 },
                { "nw", 4 },
                { "ne", 5 },
            };
            
            var tiles = new HashSet<Tile>();
            foreach (var line in input)
            {
                var pos = new Pos(0, 0);
                foreach (Match m in Regex.Matches(line, @"[ns]?[ew]"))
                {
                    var index = str2index[m.Value];
                    var step = directions[Math.Abs(pos.y) % 2][index];
                    pos += step;
                }
                var newTile = new Tile() { pos = pos , isWhite = false};
                if (tiles.TryGetValue(newTile, out var foundTile))
                {
                    foundTile.isWhite = !foundTile.isWhite;
                }
                else
                {
                    tiles.Add(newTile);
                }
            }
            
            return tiles;
        }

     string example = @"sesenwnenenewseeswwswswwnenewsewsw
neeenesenwnwwswnenewnwwsewnenwseswesw
seswneswswsenwwnwse
nwnwneseeswswnenewneswwnewseswneseene
swweswneswnenwsewnwneneseenw
eesenwseswswnenwswnwnwsewwnwsene
sewnenenenesenwsewnenwwwse
wenwwweseeeweswwwnwwe
wsweesenenewnwwnwsenewsenwwsesesenwne
neeswseenwwswnwswswnw
nenwswwsewswnenenewsenwsenwnesesenew
enewnwewneswsewnwswenweswnenwsenwsw
sweneswneswneneenwnewenewwneswswnese
swwesenesewenwneswnwwneseswwne
enesenwswwswneneswsenwnewswseenwsese
wnwnesenesenenwwnenwsewesewsesesew
nenewswnwewswnenesenwnesewesw
eneswnwswnwsenenwnwnwwseeswneewsenese
neswnwewnwnwseenwseesewsenwsweewe
wseweeenwnesenwwwswnew";

        [Test]
        public void Part1_Example1()
        {
            var parsed = Parse(Common.GetLines(example));
            Assert.AreEqual(10, parsed.Where(x => x.isWhite == false).Count());
        }

        [Test]
        public void Part1()
        {
            var parsed = Parse(Common.DayInput(nameof(Day24)));
            Assert.AreEqual(266, parsed.Where(x => x.isWhite == false).Count());
        }

        internal class GameOfTiles
        {
            internal int DayNum => day;
            int day = 0;
            HashSet<Tile> tiles;

            readonly Pos[][] directions = new Pos[2][];

            internal GameOfTiles(HashSet<Tile> tiles)
            {
                this.tiles = tiles;

                directions[0] = new Pos[] { new Pos(1, 0), new Pos(0, 1), new Pos(-1, 1), new Pos(-1, 0), new Pos(-1, -1), new Pos(0, -1) }; // even lines
                directions[1] = new Pos[] { new Pos(1, 0), new Pos(1, 1), new Pos(0, 1), new Pos(-1, 0), new Pos(0, -1), new Pos(1, -1) }; // odd lines
            }

            internal int Day()
            {
                day++;

                var nextDay = new HashSet<Tile>();

                var newTiles = new HashSet<Tile>();
                foreach (var tile in tiles.Where(x => x.isWhite == false))
                {
                    foreach (var dir in directions[Math.Abs(tile.pos.y) % 2])
                    {
                        var neighbour = new Tile() { pos = tile.pos + dir };
                        if (!tiles.Contains(neighbour)) newTiles.Add(neighbour);
                    }
                }
                tiles.UnionWith(newTiles);

                foreach (var tile in tiles)
                {
                    int count = 0;
                    foreach (var dir in directions[Math.Abs(tile.pos.y) % 2])
                    {
                        var neighbour = new Tile() { pos = tile.pos + dir };
                        if (tiles.TryGetValue(neighbour, out var n))
                        {
                            if (!n.isWhite) count++;
                        }
                    }

                    if (!tile.isWhite)
                    {
                        if (count > 0 && count < 3)
                        {
                            nextDay.Add(new Tile() { pos = tile.pos, isWhite = false});
                        }
                    }
                    else if (count == 2)
                    {
                        nextDay.Add(new Tile() { pos = tile.pos, isWhite = false });
                    }
                }

                tiles = nextDay;

                return BlackCount();
            }

            internal int BlackCount()
            {
                return tiles.Where(x => x.isWhite == false).Count();
            }

            internal void Print()
            {
                var max = new Pos(tiles.First().pos);
                var min = new Pos(tiles.First().pos);
                foreach (var pos in tiles.Select(x => x.pos))
                {
                    if (pos.x > max.x) max.x = pos.x;
                    if (pos.y > max.y) max.y = pos.y;
                    if (pos.x < min.x) min.x = pos.x;
                    if (pos.y < min.y) min.y = pos.y;
                }
                for (int j = min.y; j < max.y; j++)
                {
                    var line = new StringBuilder();
                    if (Math.Abs(j) % 2 == 0) line.Append(' ');
                    for (int i = min.x; i < max.x; i++)
                    {
                        var tile = new Tile() { pos = new Pos(i, j) };
                        if (tiles.TryGetValue(tile, out Tile found) && !found.isWhite)
                        {
                            line.Append('#');
                        }
                        else
                        {
                            line.Append('.');
                        }
                        line.Append(' ');
                    }
                    Console.WriteLine(line.ToString());
                }
            }
        }

        [Test]
        public void Part2_Example1()
        {
            var parsed = Parse(Common.GetLines(example));
            var game = new GameOfTiles(parsed);
            int count = 0;
            for (int i = 0; i < 100; i++)
            {
                count = game.Day();
                Console.WriteLine($"Day {i+1}: {count}");
            }
            Assert.AreEqual(2208, count);
        }

        [Test]
        public void Part2()
        {
            var parsed = Parse(Common.DayInput(nameof(Day24)));
            var game = new GameOfTiles(parsed);
            int count = 0;
            for (int i = 0; i < 100; i++)
            {
                count = game.Day();
                Console.WriteLine($"Day {i + 1}: {count}");
            }
            game.Print();
            Assert.AreEqual(3627, count);
        }

    }
}
