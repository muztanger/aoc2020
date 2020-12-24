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
            internal bool isWhiteUp = true;

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

        private static int Parse(IEnumerable<string> input)
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
                var newTile = new Tile() { pos = pos , isWhiteUp = false};
                if (tiles.TryGetValue(newTile, out var foundTile))
                {
                    foundTile.isWhiteUp = !foundTile.isWhiteUp;
                }
                else
                {
                    tiles.Add(newTile);
                }
            }
            
            return tiles.Where(x => x.isWhiteUp == false).Count();
        }

        [Test]
        public void Part1_Example1()
        {
            string input = @"sesenwnenenewseeswwswswwnenewsewsw
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
            var parsed = Parse(Common.GetLines(input));
            Assert.AreEqual(10, parsed);
        }

        [Test]
        public void Part1()
        {
            var parsed = Parse(Common.DayInput(nameof(Day24)));
            Assert.AreEqual(266, parsed);
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
            var parsed = Parse(Common.DayInput(nameof(Day24)));
            Assert.AreEqual(0, 1);
        }

    }
}
