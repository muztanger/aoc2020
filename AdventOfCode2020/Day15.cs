using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class Day15
    {
        public class LastTwo
        {
            bool flip = true;
            long count = 0;
            long[] values = new long[2];
            
            public void Add(long x)
            {
                values[flip ? 0 : 1] = x;
                flip = !flip;
                count++;
            }

            public long Dist() => Math.Abs(values[0] - values[1]);

            public bool IsTwo() => count >= 2;
        }
        public class MemoryGame
        {
            private long[] init;
            private long spoken;
            public long Spoken { get => spoken; }
            long turn = 0;
            DefaultValueDictionary<long, LastTwo> mem;
            public MemoryGame(string[] init)
            {
                this.init = init.Select(x => long.Parse(x)).ToArray();
                this.mem = new DefaultValueDictionary<long, LastTwo>(() => new LastTwo());
            }

            public void Turn()
            {
                turn++;
                if (turn <= init.Length)
                {
                    spoken = init[turn - 1];
                    mem[spoken].Add(turn); 
                }
                else
                {
                    if (!mem[spoken].IsTwo())
                    {
                        spoken = 0;
                        mem[spoken].Add(turn);
                    }
                    else
                    {
                        spoken = mem[spoken].Dist();
                        mem[spoken].Add(turn);
                    }
                }
            }

            public static MemoryGame Parse(IEnumerable<string> input)
            {
                return new MemoryGame(input.First().Split(","));
            }
        }

        [Test]
        public void Part1_Example1()
        {
            string input = @"0,3,6";
            var game = MemoryGame.Parse(Common.GetLines(input));
            for (int i = 0; i < 2020; i++)
            {
                game.Turn();
            }
            Assert.AreEqual(436, game.Spoken);
        }

        [TestCase("1,3,2", 1)]
        [TestCase("2,1,3", 10)]
        [TestCase("1,2,3", 27)]
        [TestCase("2,3,1", 78)]
        [TestCase("3,2,1", 438)]
        [TestCase("3,1,2", 1836)]
        public void Part1_Example2(string input, long expected)
        {
            var game = MemoryGame.Parse(Common.GetLines(input));
            for (int i = 0; i < 2020; i++)
            {
                game.Turn();
            }
            Assert.AreEqual(expected, game.Spoken);
        }

        [Test]
        public void Part1()
        {
            var game = MemoryGame.Parse(Common.DayInput(nameof(Day15)));
            for (int i = 0; i < 2020; i++)
            {
                game.Turn();
            }
            Assert.AreEqual(447L, game.Spoken);
        }

        [TestCase("0,3,6", 175594)]
        [TestCase("1,3,2", 2578)]
        [TestCase("2,1,3", 3544142)]
        [TestCase("1,2,3", 261214)]
        [TestCase("2,3,1", 6895259)]
        [TestCase("3,2,1", 18)]
        [TestCase("3,1,2", 362)]
        public void Part2_Example1(string input, long expected)
        {
            var game = MemoryGame.Parse(Common.GetLines(input));
            for (int i = 0; i < 30000000; i++)
            {
                game.Turn();
            }
            Assert.AreEqual(expected, game.Spoken);
        }

        [Test]
        public void Part2()
        {
            var game = MemoryGame.Parse(Common.DayInput(nameof(Day15)));
            for (int i = 0; i < 30000000; i++)
            {
                game.Turn();
            }
            Assert.AreEqual(11721679L, game.Spoken);
        }

    }
}
