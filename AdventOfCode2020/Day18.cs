using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    public class Day18
    {
        public abstract class Expression
        {
            public abstract long Eval();
        }

        public class Number: Expression
        {
            private long value;

            public Number(long value)
            {
                this.value = value;
            }

            public Number(string value)
            {
                this.value = long.Parse(value);
            }

            public override long Eval()
            {
                return value;
            }
        }

        public abstract class Operator: Expression
        {
            public Expression e1;
            public Expression e2;

            public Operator(Expression e1)
            {
                this.e1 = e1;
            }

            public Operator(Expression e1, Expression e2)
            {
                this.e1 = e1;
                this.e2 = e2;
            }
        }

        public class Addition: Operator
        {
            public Addition(Expression e1) : base(e1) { }

            public Addition(Expression e1, Expression e2) : base(e1, e2) {}

            public override long Eval()
            {
                return e1.Eval() + e2.Eval();
            }
        }

        public class Multiplication: Operator
        {
            public Multiplication(Expression e1) : base(e1) { }
            public Multiplication(Expression e1, Expression e2) : base(e1, e2) { }

            public override long Eval()
            {
                return e1.Eval() * e2.Eval();
            }
        }

        public class Group: Expression
        {
            public Expression group;

            public override long Eval()
            {
                return group.Eval();
            }
        }

        private static IEnumerable<MatchCollection> Tokenize(IEnumerable<string> input)
        {
            return input.Select(line => Regex.Matches(line, @"[\d()+*]+"));
        }

        [TestCase("1 + 2 * 3 + 4 * 5 + 6", 71)]
        [TestCase("1 + (2 * 3) + (4 * (5 + 6))", 51)]
        [TestCase("2 * 3 + (4 * 5)", 26)]
        [TestCase("5 + (8 * 3 + 9 + 3 * 4 * 3", 437)]
        [TestCase("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 12240)]
        [TestCase("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 13632)]
        public void Part1_Example1(string input, long expected)
        {
            var parsed = Tokenize(Common.GetLines(input));
            MatchCollection tokens = parsed.First();
            var stack = new Stack<Expression>();
            var groups = new Stack<Stack<Expression>>();
            foreach (Match m in tokens)
            {
                if (Regex.IsMatch(m.Value, @"\d+"))
                {
                    if (stack.Count > 0)
                    {
                        var last = stack.Peek();
                        switch (last)
                        {
                            case Addition add:
                                add.e2 = new Number(m.Value);
                                break;
                            case Multiplication mult:
                                mult.e2 = new Number(m.Value);
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }
                    else
                    {
                        stack.Push(new Number(m.Value));
                    }
                }
                else if (m.Value.Equals("+"))
                {
                    var last = stack.Pop();
                    stack.Push(new Addition(last));
                }
                else if (m.Value.Equals("*"))
                {
                    var last = stack.Pop();
                    stack.Push(new Multiplication(last));
                }
                else if (m.Value.Equals("("))
                {
                    groups.Push(stack);
                    stack = new Stack<Expression>();
                }
                else if (m.Value.Equals(")"))
                {
                    var last = stack.Pop();
                    var group = groups.Pop();

                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            var result = stack.Pop();
            Assert.AreEqual(expected, result.Eval());
        }

        [Test]
        public void Part1_Example2()
        {
            string input = @"";
            var parsed = Tokenize(Common.GetLines(input));
            Assert.AreEqual(0, 1);
        }

        [Test]
        public void Part1()
        {
            var parsed = Tokenize(Common.DayInput(nameof(Day18)));
            Assert.AreEqual(0, 1);
        }

        [Test]
        public void Part2_Example1()
        {
            string input = @"";
            var parsed = Tokenize(Common.GetLines(input));
            Assert.AreEqual(0, 1);
        }

        [Test]
        public void Part2_Example2()
        {
            string input = @"";
            var parsed = Tokenize(Common.GetLines(input));
            Assert.AreEqual(0, 1);
        }

        [Test]
        public void Part2()
        {
            var parsed = Tokenize(Common.DayInput(nameof(Day18)));
            Assert.AreEqual(0, 1);
        }

    }
}
