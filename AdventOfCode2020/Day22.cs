using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class Day22
    {
        class Game
        {
            Player p1;
            Player p2;


        }

        class Player
        {
            internal string Name;
            Queue<int> deck = new Queue<int>();
            HashSet<int> deckHashes = new HashSet<int>();
            internal int DeckSize => deck.Count();

            bool PlayedDeck()
            {
                int hash = deck.GetSequenceHashCode();
                if (deckHashes.Contains(hash)) return true;
                return false;
            }

            internal long Score()
            {
                if (deck.Count == 0) return 0;

                long result = 0;
                long m = deck.Count;
                foreach (var card in deck)
                {
                    result += m * card;
                    m--;
                }
                return result;
            }

            internal int Play()
            {
                return deck.Dequeue();
            }

            internal void Take(int card)
            {
                deck.Enqueue(card);
            }

        }

        private static List<Player> Parse(IEnumerable<string> input)
        {
            var result = new List<Player>();
            Player player = null;
            foreach (var line in input)
            {
                if (!line.Any()) continue;

                if (line.StartsWith("Player"))
                {
                    if (player != null) result.Add(player);
                    player = new Player()
                    {
                        Name = line[..^1]
                    };
                }
                else
                {
                    player.Take(int.Parse(line));
                }
            }
            result.Add(player);
            return result;
        }

        [Test]
        public void Part1_Example1()
        {
            string input = @"Player 1:
9
2
6
3
1

Player 2:
5
8
4
7
10";
            var players = Parse(Common.GetLines(input));
            while (players[0].DeckSize > 0 && players[1].DeckSize > 0)
            {
                var c1 = players[0].Play();
                var c2 = players[1].Play();
                if (c1 > c2)
                {
                    players[0].Take(c1);
                    players[0].Take(c2);
                }
                else
                {
                    players[1].Take(c2);
                    players[1].Take(c1);
                }
            }

            long score = players[1].Score();
            score = Math.Max(score, players[0].Score());

            Assert.AreEqual(306, score);
        }

        [Test]
        public void Part1()
        {
            var players = Parse(Common.DayInput(nameof(Day22)));
            while (players[0].DeckSize > 0 && players[1].DeckSize > 0)
            {
                var c1 = players[0].Play();
                var c2 = players[1].Play();
                if (c1 > c2)
                {
                    players[0].Take(c1);
                    players[0].Take(c2);
                }
                else
                {
                    players[1].Take(c2);
                    players[1].Take(c1);
                }
            }

            long score = players[1].Score();
            score = Math.Max(score, players[0].Score());
            Assert.AreEqual(32815, score);
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
            var parsed = Parse(Common.DayInput(nameof(Day22)));
            Assert.AreEqual(0, 1);
        }

    }
}
