using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class Day12
    {
        public enum Direction {  East = 0, South, West, North, Left, Right, Forward }
        public class FerryAction
        {
            public Direction Direction { get; private set; }
            public int Count { get; private set; }

            public static FerryAction Parse(string line)
            {
                var result = new FerryAction();
                result.Direction = line[0] switch
                {
                    'N' => Direction.North,
                    'S' => Direction.South,
                    'E' => Direction.East,
                    'W' => Direction.West,
                    'L' => Direction.Left,
                    'R' => Direction.Right,
                    'F' => Direction.Forward,
                    _ => throw new NotImplementedException(line)
                };
                result.Count = int.Parse(line[1..]);
                if (result.Direction == Direction.Left || result.Direction == Direction.Right)
                {
                    Assert.IsTrue(result.Count % 90 == 0);
                }
                return result;
            }

            public override string ToString()
            {
                return $"{Direction} {Count}";
            }
        }

        public class Ferry
        {
            readonly List<Pos> rotations = new List<Pos>() { new Pos(1, 0), new Pos(0, -1), new Pos(-1, 0), new Pos(0, 1) };

            Pos start = new Pos(0, 0);
            Pos position = new Pos(0, 0);
            Direction rotation = Direction.East;

            public void Action(FerryAction action)
            {
                switch (action.Direction)
                {
                    case Direction.East:
                    case Direction.South:
                    case Direction.West:
                    case Direction.North:
                        {
                            position += action.Count * rotations[(int)action.Direction];
                        }
                        break;
                    case Direction.Left:
                        {
                            int x = action.Count / 90;
                            int y = ((int)rotation - x) % rotations.Count;
                            rotation = (Direction)(y < 0 ? y + rotations.Count : y);
                        }
                        break;
                    case Direction.Right:
                        {
                            int x = action.Count / 90;
                            int y = ((int)rotation + x) % rotations.Count;
                            rotation = (Direction)(y < 0 ? y + rotations.Count : y);
                        }
                        break;
                    case Direction.Forward:
                        {
                            position += action.Count * rotations[(int)rotation];
                        }
                        break;
                }
            }

            internal int Manhattan()
            {
                return start.Manhattan(position);
            }

            public override string ToString()
            {
                return $"{rotation} {position}";
            }

        }
        public class Ferry2
        {
            readonly List<Pos> rotations = new List<Pos>() { new Pos(1, 0), new Pos(0, -1), new Pos(-1, 0), new Pos(0, 1) };

            Pos meridian = new Pos(0, 0);
            Pos position = new Pos(0, 0);
            Pos waypoint = new Pos(10, 1);

            public void Action(FerryAction action)
            {
                switch (action.Direction)
                {
                    case Direction.East:
                    case Direction.South:
                    case Direction.West:
                    case Direction.North:
                        {
                            waypoint += action.Count * rotations[(int)action.Direction];
                        }
                        break;
                    case Direction.Left:
                        {
                            double phi = Math.Atan2(waypoint.y, waypoint.x);
                            double dist = meridian.Dist(waypoint);
                            phi += action.Count * Math.PI / 180.0;
                            int x = Convert.ToInt32(dist * Math.Cos(phi));
                            int y = Convert.ToInt32(dist * Math.Sin(phi));
                            waypoint = new Pos(x, y);
                        }
                        break;
                    case Direction.Right:
                        {
                            double phi = Math.Atan2(waypoint.y, waypoint.x);
                            double r = meridian.Dist(waypoint);
                            phi -= action.Count * Math.PI / 180.0;
                            int x = Convert.ToInt32(r * Math.Cos(phi));
                            int y = Convert.ToInt32(r * Math.Sin(phi));
                            waypoint = new Pos(x, y);
                        }
                        break;
                    case Direction.Forward:
                        {
                            position += action.Count * waypoint;
                        }
                        break;
                }
            }

            internal int Manhattan()
            {
                return meridian.Manhattan(position);
            }

            public override string ToString()
            {
                return $"pos={position} waypoint={waypoint}";
            }
        }

        private static List<FerryAction> Parse(IEnumerable<string> input)
        {
            return input.Select(x => FerryAction.Parse(x)).ToList();
        }

        string example1 = @"F10
N3
F7
R90
F11";

        [Test]
        public void Part1_Example1()
        {
            var parsed = Parse(Common.GetLines(example1));
            var ferry = new Ferry();
            foreach (var action in parsed)
            {
                ferry.Action(action);
                Console.WriteLine(ferry);
            }
            Assert.AreEqual(25, ferry.Manhattan());
        }

        [Test]
        public void Part1()
        {
            var parsed = Parse(Common.DayInput(nameof(Day12)));
            var ferry = new Ferry();
            foreach (var action in parsed)
            {
                ferry.Action(action);
                Console.WriteLine(ferry);
            }
            Assert.AreEqual(1007, ferry.Manhattan());
        }

        [Test]
        public void Part2_Example1()
        {
            var parsed = Parse(Common.GetLines(example1));
            var ferry = new Ferry2();
            foreach (var action in parsed)
            {
                ferry.Action(action);
                Console.WriteLine(ferry);
            }
            Assert.AreEqual(286, ferry.Manhattan());
        }

        [Test]
        public void Part2()
        {
            var parsed = Parse(Common.DayInput(nameof(Day12)));
            var ferry = new Ferry2();
            foreach (var action in parsed)
            {
                ferry.Action(action);
                Console.WriteLine(ferry);
            }
            Assert.AreNotEqual(13380, ferry.Manhattan());
            Assert.AreEqual(41212, ferry.Manhattan());
        }

    }
}
