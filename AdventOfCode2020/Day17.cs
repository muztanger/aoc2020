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
            public PosN pos;

            public override bool Equals(object obj)
            {
                return Equals(obj as Cube);
            }

            public bool Equals(Cube other)
            {
                return other != null &&
                       EqualityComparer<PosN>.Default.Equals(pos, other.pos);
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
        private static HashSet<Cube> Parse(IEnumerable<string> input, int n = 3)
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
                        var pos = new List<int>() { x, y };
                        for (int i = 2; i < n; i++)
                        {
                            pos.Add(0);
                        }
                        result.Add(new Cube() { active = true, pos = new PosN(pos) });
                    }
                    x++;
                }
                y++;
            }
            return result;
        }

        string example1 = @".#.
..#
###";

        [Test]
        public void Part1_Example1()
        {
            var cubes = Parse(Common.GetLines(example1));
            IEnumerable<PosN> neighbourhood = GenerateNeighbourhood();

            for (int i = 0; i < 6; i++) cubes = Iterate(cubes, neighbourhood);

            Assert.AreEqual(112, cubes.Where(x => x.active).Count());
        }

        private static IEnumerable<PosN> GenerateNeighbourhood(int n = 3)
        {
            var result = CubeN(n);
            var origo = new PosN(Enumerable.Range(0, n).Select(x => 0));
            Assert.IsTrue(result.Remove(origo));
            return result;
        }

        private static HashSet<PosN> CubeN(int n = 3)
        {
            var neighbourhood = new HashSet<PosN>();
            if (n == 1)
            {
                for (int i = -1; i <= 1; i++)
                {
                    neighbourhood.Add(new PosN(new int[] { i }));
                }
            }
            else
            {
                for (int i = -1; i <= 1; i++)
                {
                    foreach (var v in CubeN(n - 1))
                    {
                        var w = new List<int>(v.values);
                        w.Add(i);
                        neighbourhood.Add(new PosN(w));
                    }
                }
            }
            return neighbourhood;
        }

        private static HashSet<Cube> Iterate(HashSet<Cube> cubes, IEnumerable<PosN> neighbours)
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
        public void Part1()
        {
            var cubes = Parse(Common.DayInput(nameof(Day17)));
            IEnumerable<PosN> neighbourhood = GenerateNeighbourhood();

            for (int i = 0; i < 6; i++) cubes = Iterate(cubes, neighbourhood);

            Assert.AreEqual(382, cubes.Where(x => x.active).Count());

        }

        [Test]
        public void Part2_Example1()
        {
            int n = 4;
            var cubes = Parse(Common.GetLines(example1), n);
            IEnumerable<PosN> neighbourhood = GenerateNeighbourhood(n);

            for (int i = 0; i < 6; i++) cubes = Iterate(cubes, neighbourhood);

            Assert.AreEqual(848, cubes.Where(x => x.active).Count());
        }

        [Test]
        public void Part2()
        {
            int n = 4;
            var cubes = Parse(Common.DayInput(nameof(Day17)), n);
            IEnumerable<PosN> neighbourhood = GenerateNeighbourhood(n);

            for (int i = 0; i < 6; i++) cubes = Iterate(cubes, neighbourhood);

            Assert.AreEqual(2552, cubes.Where(x => x.active).Count());
        }

    }
}
