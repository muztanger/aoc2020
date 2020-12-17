using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class Day17
    {
        public class Cube : IEquatable<Cube>
        {
            public bool active;
            public Pos3 pos;

            public override bool Equals(object obj)
            {
                return Equals(obj as Cube);
            }

            public bool Equals(Cube other)
            {
                return other != null &&
                       EqualityComparer<Pos3>.Default.Equals(pos, other.pos);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(pos);
            }

            public static bool operator ==(Cube left, Cube right)
            {
                return EqualityComparer<Cube>.Default.Equals(left, right);
            }

            public static bool operator !=(Cube left, Cube right)
            {
                return !(left == right);
            }


        }
        private static HashSet<Cube> Parse(IEnumerable<string> input)
        {
            var result = new HashSet<Cube>();
            int y = 0;
            foreach (var line in input)
            {
                int x = 0;
                foreach (char c in line)
                {
                    if (c == '#')
                    {
                        result.Add(new Cube() { active = true, pos = new Pos3(x, y, 0) });
                    }
                    x++;
                }
                y++;
            }
            return result;
        }

        [Test]
        public void Part1_Example1()
        {
            string input = @".#.
..#
###";
            var cubes = Parse(Common.GetLines(input));
            List<Pos3> neighbourhood = GenerateNeighbourhood();

            for (int i = 0; i < 6; i++) cubes = Iterate(cubes, neighbourhood);

            Assert.AreEqual(112, cubes.Where(x => x.active).Count());
        }

        private static List<Pos3> GenerateNeighbourhood()
        {
            var neighbourhood = new List<Pos3>();
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                    for (int z = -1; z <= 1; z++)
                        if (x != 0 || y != 0 || z != 0)
                            neighbourhood.Add(new Pos3(x, y, z));
            return neighbourhood;
        }

        private static HashSet<Cube> Iterate(HashSet<Cube> cubes, List<Pos3> neighbours)
        {
            var cubesWithNeighbours = new HashSet<Cube>(cubes);
            foreach (var cube in cubes)
            {
                foreach (var d in neighbours)
                {
                    var n = new Cube() { pos = cube.pos + d };
                    if (!cubesWithNeighbours.Contains(n))
                    {
                        cubesWithNeighbours.Add(n);
                    }
                }
            }
            Console.WriteLine($"cubesWithNeighbours count: {cubesWithNeighbours.Where(c => c.active).Count()}");

            var cubesWithNeighbours2 = new HashSet<Cube>(cubesWithNeighbours);
            foreach (var cube in cubesWithNeighbours)
            {
                foreach (var d in neighbours)
                {
                    var n = new Cube() { active = false, pos = cube.pos + d };
                    if (!cubesWithNeighbours2.Contains(n))
                    {
                        cubesWithNeighbours2.Add(n);
                    }
                }
            }
            Console.WriteLine($"cubesWithNeighbours2 count: {cubesWithNeighbours2.Where(c => c.active).Count()}");

            var nextCubes = new HashSet<Cube>();
            foreach (var cube in cubesWithNeighbours2)
            {
                int count = 0;
                foreach (var d in neighbours)
                {
                    var n = new Cube() { pos = cube.pos + d };
                    if (cubesWithNeighbours2.TryGetValue(n, out var actual) && actual.active)
                    {
                        count++;
                    }
                }
                if (count > 0) Console.WriteLine(count);
                if (cube.active && (count == 2 || count == 3))
                {
                    nextCubes.Add(new Cube() { active = true, pos = cube.pos });
                }
                if (!cube.active && count == 3)
                {
                    nextCubes.Add(new Cube() { active = true, pos = cube.pos });
                }
            }

            return nextCubes;
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
            var cubes = Parse(Common.DayInput(nameof(Day17)));
            List<Pos3> neighbourhood = GenerateNeighbourhood();

            for (int i = 0; i < 6; i++) cubes = Iterate(cubes, neighbourhood);

            Assert.AreEqual(112, cubes.Where(x => x.active).Count());

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
            var parsed = Parse(Common.DayInput(nameof(Day17)));
            Assert.AreEqual(0, 1);
        }

    }
}
