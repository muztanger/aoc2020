using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class Day22
    {
        static int globalGameCount = 1;
        class Game
        {
            readonly Player player1;
            readonly Player player2;
            int round = 0;
            int game;
            int winner = 0;
            internal int Winner => winner;
            internal long Score()
            {
                return winner switch
                {
                    1 => player1.Score(),
                    2 => player2.Score(),
                    _ => Math.Max(player1.Score(), player2.Score()),
                };
            }
            
            internal Game(int game, Player player1, Player player2)
            {
                this.game = globalGameCount++;
                this.player1 = player1;
                this.player2 = player2;
            }

            public Game(int game, IEnumerable<int> deck1, IEnumerable<int> deck2)
            {
                this.game = globalGameCount++;
                this.player1 = new Player("Player 1");
                foreach (var x in deck1)
                {
                    this.player1.Add(x);
                }
                this.player2 = new Player("Player2");
                foreach (var x in deck2)
                {
                    this.player2.Add(x);
                }
            }

            internal bool Round()
            {
                if (winner != 0) return false;
                round++;
                if (round == 1)
                {
                    Console.WriteLine($"=== Game {game} ===");
                    Console.WriteLine();
                }

                if (!player1.LogDeck() && (!player2.LogDeck()))
                {
                    winner = 1;
                    Console.WriteLine($"The winner of game {game} is player {winner}!");
                    return false;
                }
                //if )
                //{
                //    winner = 1;
                //    Console.WriteLine($"The winner of game {game} is player {winner}!");
                //    return false;
                //}

                Console.WriteLine($"-- Round {round} (Game {game}) --");
                Console.WriteLine($"Player 1's deck: " + player1.DeckToString());
                Console.WriteLine($"Player 2's deck: " + player2.DeckToString());
                var card1 = player1.Draw();
                Console.WriteLine($"Player 1 plays: {card1}");
                var card2 = player2.Draw();
                Console.WriteLine($"Player 2 plays: {card2}");

                if (player1.DeckSize == 0)
                {
                    player2.Take(card2, card1);
                    winner = 2;
                }
                else if (player2.DeckSize == 0)
                {
                    player1.Take(card1, card2);
                    winner = 1;
                }
                if (winner != 0)
                {
                    Console.WriteLine($"Player {winner} wins round {round} of game {game}!");
                    Console.WriteLine($"The winner of game {game} is player {winner}!");
                    return false;
                }

                int roundWinner;
                if (card1 <= player1.DeckSize && card2 <= player2.DeckSize)
                {
                    // play a sub game!
                    Console.WriteLine("Playing a sub-game to determine the winner...");
                    Console.WriteLine();
                    var subGame = new Game(game + 1, player1.CopySubDeck(card1), player2.CopySubDeck(card2));
                    while (subGame.Round());
                    roundWinner = subGame.Winner;
                    Console.WriteLine();
                    Console.WriteLine($"...anyway, back to game {game}.");
                }
                else
                {
                    // at least one player must not have enough cards left in their deck to recurse; 
                    // the winner of the round is the player with the higher-value card.
                    if (card1 > card2)
                    {
                        roundWinner = 1;
                    }
                    else
                    {
                        roundWinner = 2;
                    }
                }
                Console.WriteLine($"Player {roundWinner} wins round {round} of game {game}!");
                Console.WriteLine();
                if (roundWinner == 1)
                {
                    player1.Take(card1, card2);
                }
                else if (roundWinner == 2)
                {
                    player2.Take(card2, card1);
                }

                return winner == 0;
            }
        }

        class Player
        {
            internal string Name { get; }
            Queue<int> deck = new Queue<int>();
            HashSet<int> deckHashes = new HashSet<int>();

            public Player(string name)
            {
                Name = name;
            }

            public Player(Player player1)
            {
                Name = player1.Name;
                deck = new Queue<int>(player1.deck);
            }

            internal IEnumerable<int> CopySubDeck(int n)
            {
                return deck.Take(n);
            }

            internal int DeckSize => deck.Count();

            internal bool LogDeck()
            {
                int hash = deck.GetSequenceHashCodeFromString();
                return deckHashes.Add(hash);
            }

            internal long Score()
            {
                if (deck.Count == 0) return 0;

                long result = 0;
                long m = deck.Count;
                foreach (var card in deck)
                {
                    Console.WriteLine($"{m} * {card}");
                    result += m * card;
                    m--;
                }
                return result;
            }

            internal int Draw()
            {
                return deck.Dequeue();
            }

            internal void Take(int card1, int card2)
            {
                deck.Enqueue(card1);
                deck.Enqueue(card2);
            }

            internal void Add(int card)
            {
                deck.Enqueue(card);
            }

            internal string DeckToString()
            {
                return string.Join(", ", deck);
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
                    player = new Player(name: line[..^1]);
                }
                else
                {
                    player.Add(int.Parse(line));
                }
            }
            result.Add(player);
            return result;
        }

        string example1 = @"Player 1:
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
        [Test]
        public void Part1_Example1()
        {
            var players = Parse(Common.GetLines(example1));
            while (players[0].DeckSize > 0 && players[1].DeckSize > 0)
            {
                var c1 = players[0].Draw();
                var c2 = players[1].Draw();
                if (c1 > c2)
                {
                    players[0].Take(c1, c2);
                }
                else
                {
                    players[1].Take(c2, c1);
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
                var c1 = players[0].Draw();
                var c2 = players[1].Draw();
                if (c1 > c2)
                {
                    players[0].Take(c1, c2);
                }
                else
                {
                    players[1].Take(c2, c1);
                }
            }

            long score = players[1].Score();
            score = Math.Max(score, players[0].Score());
            Assert.AreEqual(32815, score);
        }

        [Test]
        public void Part2_Example1()
        {
            var parsed = Parse(Common.GetLines(example1));
            var game = new Game(1, parsed[0], parsed[1]);
            while (game.Round());
            Assert.AreEqual(291, game.Score());
        }

        [Test]
        public void Part2()
        {
            var parsed = Parse(Common.DayInput(nameof(Day22)));
            var game = new Game(1, parsed[0], parsed[1]);
            while (game.Round());
            Assert.AreNotEqual(33901, game.Score()); // your answer is too high
            Assert.AreNotEqual(32460, game.Score());
            Assert.AreNotEqual(31467, game.Score()); // your answer is too high

            Assert.AreEqual(291, game.Score());
        }

    }
}
