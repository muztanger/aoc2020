using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class Day23
    {
        private static Node Parse(IEnumerable<string> input, int fill = 0)
        {
            var list = new List<Node>();
            Node.Count = 0;
            foreach (var x in input.First().Select(x => int.Parse("" + x)))
            {
                list.Add(new Node() { value = x });
                Node.Count++;
            }

            if (fill > 0)
            {
                while (list.Count < fill)
                {
                    list.Add(new Node() { value = list.Count + 1 });
                    Node.Count++;
                }
            }
            for (int i = 0; i < list.Count - 1; i++)
            {
                list[i].next = list[i + 1];
            }
            list.Last().next = list.First();
            return list.First();
        }

        internal class Node
        {
            internal static int Count; // pretty scary ;-)
            internal int value;
            internal Node next;
        }

        class CrabCups
        {
            private readonly Node start;
            private Node current;
            private int move = 0;
            private readonly Dictionary<int, Node> dict;
            internal CrabCups(Node cups)
            {
                this.start = cups;
                this.current = cups;
                this.dict = new Dictionary<int, Node>();
                var node = start;
                for (int i = 0; i < Node.Count; i++)
                {
                    dict[node.value] = node;
                    node = node.next;
                }
            }

            internal void Move()
            {
                move++;

                int destinationValue = Decrease(current.value);

                // Find allowed destination value
                Node currentStart = current;
                bool loop = true;
                while (loop)
                {
                    loop = false;
                    var node = currentStart;
                    for (int i = 0; i < 3; i++)
                    {
                        node = node.next;
                        if (destinationValue == node.value)
                        {
                            loop = true;
                            destinationValue = Decrease(destinationValue);
                            break;
                        }
                    }
                }

                // find destination
                Node destinationNode = dict[destinationValue];
                //{
                //    var node = current;
                //    while (node.value != destinationValue) node = node.next;
                //    destinationNode = node;
                //}

                // pick up three cups
                Node threeCups = current.next;

                // print stuff
                //Console.WriteLine($"-- move {move} --");
                //PrintCups();
                //{
                //    var line = new StringBuilder("pick up: ");
                //    var node = threeCups;
                //    for (int i = 0; i < 3; i++)
                //    {
                //        if (i != 0) line.Append(' ');
                //        line.Append(node.value);
                //        node = node.next;
                //    }
                //    Console.WriteLine(line.ToString());
                //}
                //Console.WriteLine($"destination: {destinationValue}");

                // switch next on current to after the three cups (part of pick up)
                {
                    var node = threeCups;
                    for (int i = 0; i < 3; i++) node = node.next;
                    current.next = node;
                }

                // place three cups directly after the destination cup
                {
                    var save = destinationNode.next;
                    destinationNode.next = threeCups;
                    var node = threeCups;
                    for (int i = 0; i < 2; i++) node = node.next;
                    node.next = save;
                }

                current = current.next;
            }

            //private void PrintCups()
            //{
            //    var line = new StringBuilder("cups:");
            //    Node node = start;
            //    for (int i = 0; i < Node.Count; i++)
            //    {
            //        line.Append(' ');
            //        if (node == current) line.Append('(');
            //        line.Append(node.value);
            //        if (node == current) line.Append(')');
            //        node = node.next;
            //    }
            //    Console.WriteLine(line.ToString());
            //}

            internal string Result()
            {
                Node node = dict[1];
                node = node.next;
                var result = new StringBuilder();
                for (int i = 0; i < Node.Count - 1; i++)
                {
                    result.Append(node.value);
                    node = node.next;
                }
                return result.ToString();
            }

            internal long Result2()
            {
                Node node = dict[1];
                return node.next.value * (long)node.next.next.value;
            }

            private int Decrease(int destination)
            {
                int result = destination - 2; // -1 and shift to zero-based
                if (result < 0) result += Node.Count;
                result++; // shift back to 1 based
                return result;
            }
        }

        string example1 = @"389125467";

        [Test]
        public void Part1_Example1()
        {
            var parsed = Parse(Common.GetLines(example1));
            var game = new CrabCups(parsed);
            for (int i = 0; i < 100; i++) game.Move();

            Assert.AreEqual("67384529", game.Result());
        }

        [Test]
        public void Part1()
        {
            var parsed = Parse(Common.DayInput(nameof(Day23)));
            var game = new CrabCups(parsed);
            for (int i = 0; i < 100; i++) game.Move();

            Assert.AreEqual("52937846", game.Result());
        }

        [Test]
        public void Part2_Example1()
        {
            var parsed = Parse(Common.GetLines(example1), 1000000);
            Assert.AreEqual(1000000, Node.Count);

            var game = new CrabCups(parsed);
            for (int i = 0; i < 10000000; i++) game.Move();

            Assert.AreEqual(149245887792, game.Result2());
        }

        [Test]
        public void Part2()
        {
            var parsed = Parse(Common.DayInput(nameof(Day23)), 1000000);
            Assert.AreEqual(1000000, Node.Count);

            var game = new CrabCups(parsed);
            for (int i = 0; i < 10000000; i++) game.Move();

            Assert.AreEqual(8456532414, game.Result2());
        }

    }
}
