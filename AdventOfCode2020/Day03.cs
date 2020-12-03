using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class Day03
    {

        private static List<List<bool>> Parse(IEnumerable<string> input)
        {
            return input.Select(line => line.Select(x => x == '#').ToList()).ToList();
        }

        public static int Sum(Pos d, List<List<bool>> trees)
        {
            int cols = trees[0].Count;
            int rows = trees.Count;

            Pos p = new Pos(0, 0);
            int count = 0;
            while (p.y < rows)
            {
                if (trees[p.y][p.x]) count++;
                p += d;
                p.x %= cols;
            }
            return count;
        }

        [Test]
        public void Part1_Example1()
        {
            string input = @"..##.......
#...#...#..
.#....#..#.
..#.#...#.#
.#...##..#.
..#.##.....
.#.#.#....#
.#........#
#.##...#...
#...##....#
.#..#...#.#";
            Assert.AreEqual(7, Sum(new Pos(3, 1), Parse(Common.GetLines(input))));
        }

        [Test]
        public void Part1()
        {
            List<List<bool>> trees = Parse(Common.DayInput(nameof(Day03)));
            Assert.AreEqual(178, Sum(new Pos(3, 1), trees));
        }

        [Test]
        public void Part2()
        {
            List<List<bool>> trees = Parse(Common.DayInput(nameof(Day03)));
            var ps = new[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) };
            long prod = 1;
            foreach (var d in ps.Select( z => new Pos(z)))
            {
                prod *= Sum(d, trees);
            }
            Assert.AreEqual(3492520200L, prod);
        }

    }
}
