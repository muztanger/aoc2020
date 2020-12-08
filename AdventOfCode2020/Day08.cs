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
            public enum Operation { Acc, Jmp, Nop };
            public class Instruction
            {
                public Operation Op { get; set; }
                public int Value { get; set; }
                public static Instruction Parse(string line)
                {
                    var split = line.Split();
                    var instructionStr = split[0];
                    var oper = instructionStr switch
                    {
                        "nop" => Operation.Nop,
                        "acc" => Operation.Acc,
                        "jmp" => Operation.Jmp,
                        _ => throw new NotImplementedException(instructionStr),
                    };
                    var x = int.Parse(split[1]);
                    return new Instruction()
                    {
                        Op = oper,
                        Value = x
                    };
                }

                public override string ToString()
                {
                    return $"{Op} {Value}";
                }
            }

            private int mAccumulator;
            private int mIndex;
            private List<Instruction> mProgram;


            public GameConsole(List<string> program)
            {
                mProgram = program.Select(str => Instruction.Parse(str)).ToList();
            }

            public void Reset()
            {
                mIndex = 0;
                mAccumulator = 0;
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
                    var instruction = mProgram[mIndex];
                    switch (instruction.Op)
                    {
                        case Operation.Nop: Nop(instruction.Value); break;
                        case Operation.Acc: Acc(instruction.Value); break;
                        case Operation.Jmp: Jmp(instruction.Value); break;
                        default: throw new NotImplementedException();
                    }
                }
                return mAccumulator;
            }

            public void Flip(int index)
            {
                mProgram[index].Op = mProgram[index].Op switch
                {
                    Operation.Jmp => Operation.Nop,
                    Operation.Nop => Operation.Jmp,
                    _ => throw new Exception("invalid instruction"),
                };
            }

            public int Exec2()
            {
                Reset();

                var mem = new Dictionary<int, int>();
                while (mIndex != mProgram.Count)
                {
                    mem.TryGetValue(mIndex, out var count);
                    mem[mIndex] = ++count;
                    if (count >= 100) throw new TimeoutException();

                    var instruction = mProgram[mIndex];
                    switch (instruction.Op)
                    {
                        case Operation.Nop: Nop(instruction.Value); break;
                        case Operation.Acc: Acc(instruction.Value); break;
                        case Operation.Jmp: Jmp(instruction.Value); break;
                        default: throw new NotImplementedException();
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
        public void Part1()
        {
            var parsed = Parse(Common.DayInput(nameof(Day08)));
            var game = new GameConsole(parsed);
            Assert.AreEqual(1584, game.Exec());
        }

        [Test]
        public void Part2_Example1()
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
            var indexes = new List<int>();
            for (int i = 0; i < parsed.Count; i++)
            {
                if (parsed[i].StartsWith("jmp") || parsed[i].StartsWith("nop"))
                {
                    indexes.Add(i);
                }
            }
            var game = new GameConsole(parsed);
            var result = int.MinValue;
            foreach (var index in indexes)
            {
                game.Flip(index);
                try
                {
                    result = game.Exec2();
                    Console.WriteLine($"index: {index} result={result}");
                }
                catch (TimeoutException)
                {
                    Console.WriteLine($"index: {index} timeout");
                }
                game.Flip(index);
            }

            Assert.AreEqual(8, result);
        }


        [Test]
        public void Part2()
        {
            var parsed = Parse(Common.DayInput(nameof(Day08)));
            var indexes = new List<int>();
            for (int i = 0; i < parsed.Count; i++)
            {
                if (parsed[i].StartsWith("jmp") || parsed[i].StartsWith("nop"))
                {
                    indexes.Add(i);
                }
            }
            var game = new GameConsole(parsed);
            var result = int.MinValue;
            int count = 0;
            foreach (var index in indexes)
            {
                game.Flip(index);
                try
                {
                    result = game.Exec2();
                    Console.WriteLine($"index: {index} result={result}");
                    count++;
                }
                catch (TimeoutException)
                {
                    Console.WriteLine($"index={index} timeout");
                }
                game.Flip(index);
            }
            Assert.IsTrue(count == 1);
            Assert.AreEqual(920, result);
        }

    }
}
