using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class Day08
    {
        public class GameConsole
        {
            private int mAccumulator;
            private int mIndex;
            private List<string> mProgram;

            public GameConsole(List<string> program)
            {
                this.mProgram = program;
            }

            /**
             *     acc increases or decreases a single global value called the accumulator 
             *     by the value given in the argument. For example, acc +7 would 
             *     increase the accumulator by 7. The accumulator starts at 0. After an acc 
             *     instruction, the instruction immediately below it is executed next.
             */
            public void Acc(int x)
            {
                mAccumulator += x;
                Jmp(1);
            }

            /**
             *  jmp jumps to a new instruction relative to itself. The next instruction to execute is 
             *  found using the argument as an offset from the jmp instruction; for example, 
             *  jmp +2 would skip the next instruction, jmp +1 would continue to the instruction 
             *  immediately below it, and jmp -20 would cause the instruction 20 lines above 
             *  to be executed next.
             */
            public void Jmp(int x)
            {
                mIndex += x;
            }

            /**
             *  nop stands for No OPeration - it does nothing. The instruction immediately 
             *  below it is executed next.
             */
            public void Nop(int x)
            {
                Jmp(1);
            }

            public int Exec()
            {
                var mem = new HashSet<int>();
                while (!mem.Contains(mIndex))
                {
                    mem.Add(mIndex);
                    var split = mProgram[mIndex].Split();
                    var instruction = split[0];
                    var x = int.Parse(split[1]);
                    Console.WriteLine($"mProgram[mIndex]={mProgram[mIndex]} instruction={instruction} x={x}");
                    switch (instruction)
                    {
                        case "nop": Nop(x); break;
                        case "acc": Acc(x); break;
                        case "jmp": Jmp(x); break;
                        default: throw new NotImplementedException(instruction);
                    }
                }
                return mAccumulator;
            }
        }

        private static List<string> Parse(IEnumerable<string> input)
        {
            return input.ToList();
        }

        [Test]
        public void Part1_Example1()
        {
            
            string input = @"nop +0
acc +1
jmp +4
acc +3
jmp -3
acc -99
acc +1
jmp -4
acc +6";
            var parsed = Parse(Common.GetLines(input));
            var game = new GameConsole(parsed);

            Assert.AreEqual(5, game.Exec());
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
            var parsed = Parse(Common.DayInput(nameof(Day08)));
            var game = new GameConsole(parsed);
            Assert.AreEqual(0, game.Exec());
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
            var parsed = Parse(Common.DayInput(nameof(Day08)));
            Assert.AreEqual(0, 1);
        }

    }
}
