using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class Day23
    {
        private static List<int> Parse(IEnumerable<string> input)
        {
            return new List<int>(input.First().Select(x => int.Parse("" + x)));
        }

        class CrabCups
        {
            private readonly List<int> cups;
            private int currentIndex = 0;
            private int move = 0;

            internal CrabCups(List<int> cups)
            {
                this.cups = cups;
            }

            internal void Move()
            {
                move++;

                int destination = cups[currentIndex] - 1;
                if (destination <= 0) destination += cups.Count;

                // Find allowed destination
                bool loop = true;
                while (loop)
                {
                    loop = false;
                    for (int i = 0; i < 3; i++)
                    {
                        int check = (currentIndex + 1 + i) % cups.Count;
                        if (destination == cups[check])
                        {
                            loop = true;
                            destination--;
                            if (destination < 0) destination += cups.Count;
                            break;
                        }
                    }
                }

                // find destination index
                int destinationIndex = cups.IndexOf(destination);

                Console.WriteLine($"-- move {move} --");
                {
                    var line = new StringBuilder("cups:");
                    for (int i = 0; i < cups.Count; i++)
                    {
                        line.Append(' ');
                        if (i == currentIndex) line.Append('(');
                        line.Append(cups[i]);
                        if (i == currentIndex) line.Append(')');
                    }
                    Console.WriteLine(line.ToString());
                }
                {
                    var line = new StringBuilder("pick up: ");
                    int j = (currentIndex + 1) % cups.Count;
                    for (int i = 0; i < 3; i++)
                    {
                        if (i != 0) line.Append(' ');
                        line.Append(cups[j]);
                        j = (j + 1) % cups.Count;
                    }
                    Console.WriteLine(line.ToString());
                }
                Console.WriteLine($"destination: {destination}");

                // Do some swapping
                int swapIndex = (currentIndex + 1) % cups.Count;
                for (int i = 0; i < 3; i++)
                {
                    cups.Swap(swapIndex, destinationIndex);
                    //destinationIndex = (destinationIndex + 1) % cups.Count;
                    swapIndex = (swapIndex + 1) % cups.Count;
                }
                currentIndex = (currentIndex + 1) % cups.Count;
            }
        }

        [Test]
        public void Part1_Example1()
        {
            string input = @"389125467";
            var parsed = Parse(Common.GetLines(input));
            var game = new CrabCups(parsed);
            for (int i = 0; i < 100; i++) game.Move();
            Assert.AreEqual(0, 1);
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
            var parsed = Parse(Common.DayInput(nameof(Day23)));
            Assert.AreEqual(0, 1);
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
            var parsed = Parse(Common.DayInput(nameof(Day23)));
            Assert.AreEqual(0, 1);
        }

    }
}
