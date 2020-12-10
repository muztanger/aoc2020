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

            public void Acc(int x)
            {
                mAccumulator += x;
                Jmp(1);
            }

            public void Jmp(int x) => mIndex += x;

            public void Nop() => Jmp(1);

            public int Exec()
            {
                var mem = new HashSet<int>();
                while (!mem.Contains(mIndex))
                {
                    mem.Add(mIndex);
                    var instruction = mProgram[mIndex];
                    switch (instruction.Op)
                    {
                        case Operation.Nop: Nop(); break;
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

                var mem = new DefaultDictionary<int, int>();
                while (mIndex != mProgram.Count)
                {
                    mem[mIndex]++;
                    if (mem[mIndex] >= 10) throw new TimeoutException();

                    var instruction = mProgram[mIndex];
                    switch (instruction.Op)
                    {
                        case Operation.Nop: Nop(); break;
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
                    break;
                }
                catch (TimeoutException)
                {
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
            foreach (var index in indexes)
            {
                game.Flip(index);
                try
                {
                    result = game.Exec2();
                    break;
                }
                catch (TimeoutException)
                {
                    // continue
                }
                game.Flip(index);
            }
            Assert.AreEqual(920, result);
        }

    }
}
