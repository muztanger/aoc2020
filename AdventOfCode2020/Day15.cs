using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class Day15
    {
        public class MemoryGame
        {
            private int[] init;
            private int spoken;
            public int Spoken { get => spoken; }
            int turn = 0;
            DefaultValueDictionary<int, List<int>> mem;
            public MemoryGame(string[] init)
            {
                this.init = init.Select(x => int.Parse(x)).ToArray();
                this.mem = new DefaultValueDictionary<int, List<int>>(() => new List<int>());
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
                    if (mem[spoken].Count < 2)
                    {
                        spoken = 0;
                        mem[spoken].Add(turn);
                    }
                    else
                    {
                        spoken = Math.Abs(mem[spoken].Last() - mem[spoken][mem[spoken].Count - 2]);
                        mem[spoken].Add(turn);
                    }
                }

                Console.WriteLine($"{turn}: {spoken}");

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
        public void Part1_Example2(string input, int expected)
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
            Assert.AreEqual(0, game.Spoken);
        }

        [Test]
        public void Part2_Example1()
        {
            string input = @"";
            //var parsed = Parse(Common.GetLines(input));
            Assert.AreEqual(0, 1);
        }

        [Test]
        public void Part2_Example2()
        {
            string input = @"";
            //var parsed = Parse(Common.GetLines(input));
            Assert.AreEqual(0, 1);
        }

        [Test]
        public void Part2()
        {
            //var parsed = Parse(Common.DayInput(nameof(Day15)));
            Assert.AreEqual(0, 1);
        }

    }
}
