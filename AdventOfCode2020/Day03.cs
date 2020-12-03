using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class Day03
    {
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
            var trees = new List<List<bool>>();
            int M = 0;
            foreach (var line in Common.GetLines(input))
            {
                var row = line.Select(x => x == '#').ToList();
                trees.Add(row);
                M = row.Count;
            }
            Pos p = new Pos(0, 0);
            Pos d = new Pos(3, 1);
            int N = trees.Count;
            int count = 0;
            while (p.y < N)
            {
                if (trees[p.y][p.x]) count++;
                p += d;
                if (p.x >= N) p.x = p.x % N;
            }
            Assert.AreEqual(0, count);
        }

        [Test]
        public void Part1_Example2()
        {
        }

        [Test]
        public void Part1()
        {
            var trees = new List<List<bool>>();
            int M = 0;
            foreach (var line in Common.DayInput(nameof(Day03)))
            {
                var row = line.Select(x => x == '#').ToList();
                trees.Add(row);
                M = row.Count;
            }
            Pos p = new Pos(0, 0);
            Pos d = new Pos(3, 1);
            int N = trees.Count;
            int count = 0;
            while (p.y < N)
            {
                if (trees[p.y][p.x]) count++;
                p += d;
                if (p.x >= M) p.x = p.x % M;
            }
            Assert.AreEqual(0, count);
        }

       
        public int Sum(Pos d)
        {
            var trees = new List<List<bool>>();
            int M = 0;
            foreach (var line in Common.DayInput(nameof(Day03)))
            {
                var row = line.Select(x => x == '#').ToList();
                trees.Add(row);
                M = row.Count;
            }
            Pos p = new Pos(0, 0);
            //Pos d = new Pos(3, 1);
            int N = trees.Count;
            int count = 0;
            while (p.y < N)
            {
                if (trees[p.y][p.x]) count++;
                p += d;
                if (p.x >= M) p.x = p.x % M;
            }
            return count;
        }

        [Test]
        public void Part2_Example2()
        {
        }

        [Test]
        public void Part2()
        {
            long prod = 1;
            foreach (var d in new List<Pos>() { new Pos(1,1), new Pos(3, 1), new Pos(5, 1), new Pos(7, 1), new Pos(1, 2)})
            {
                prod *= (long) Sum(d);
            }
            Assert.AreEqual(0, prod);
        }

    }
}
