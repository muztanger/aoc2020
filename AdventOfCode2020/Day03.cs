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
            var trees = new List<List<bool>>();
            foreach (var line in input)
            {
                var row = line.Select(x => x == '#').ToList();
                trees.Add(row);
            }
            return trees;
        }

        public static int Sum(Pos d, List<List<bool>> trees)
        {
            int M = trees[0].Count;
            Pos p = new Pos(0, 0);
            int N = trees.Count;
            int count = 0;
            while (p.y < N)
            {
                if (trees[p.y][p.x]) count++;
                p += d;
                p.x %= M;
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
            long prod = 1;
            foreach (var d in new List<Pos>() { new Pos(1,1), new Pos(3, 1), new Pos(5, 1), new Pos(7, 1), new Pos(1, 2)})
            {
                prod *= (long) Sum(d, trees);
            }
            Assert.AreEqual(3492520200L, prod);
        }

    }
}
