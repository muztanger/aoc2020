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

            public override string ToString()
            {
                return $"Number({value})";
            }
        }

        public abstract class Operator: Expression
        {
            public Expression e1;
            public Expression e2;

            public Operator()
            {
            }

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
            public Addition()
            {
            }

            public Addition(Expression e1) : base(e1) { }

            public Addition(Expression e1, Expression e2) : base(e1, e2) {}

            public override long Eval()
            {
                return e1.Eval() + e2.Eval();
            }

            public override string ToString()
            {
                return $"Addition({e1},{e2})";
            }
        }

        public class Multiplication: Operator
        {
            public Multiplication()
            {
            }

            public Multiplication(Expression e1) : base(e1) { }
            public Multiplication(Expression e1, Expression e2) : base(e1, e2) { }

            public override long Eval()
            {
                return e1.Eval() * e2.Eval();
            }
            public override string ToString()
            {
                return $"Multiplication({e1},{e2})";
            }
        }

        public class Group: Expression
        {
            public Expression group;

            public Group()
            {
            }

            public Group(Expression broup)
            {
                this.group = broup;
            }

            public override long Eval()
            {
                return group.Eval();
            }

            public override string ToString()
            {
                return $"Group({group})";
            }
        }

        private static IEnumerable<MatchCollection> Tokenize(IEnumerable<string> input)
        {
            return input.Select(line => Regex.Matches(line, @"[\d]+|[()+*]"));
        }

        private static long CalcPart1(IEnumerable<MatchCollection> parsed)
        {
            long sum = 0;
            foreach (MatchCollection tokens in parsed)
            {
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
                        stack = groups.Pop();
                        if (stack.Count > 0)
                        {
                            var peek = stack.Peek();
                            switch (peek)
                            {
                                case Addition add:
                                    add.e2 = new Group(last);
                                    break;
                                case Multiplication mult:
                                    mult.e2 = new Group(last);
                                    break;
                                default:
                                    stack.Push(new Group(last));
                                    break;
                            }
                        }
                        else
                        {
                            stack.Push(new Group(last));
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                sum += stack.Pop().Eval();
            }

            return sum;
        }

        [TestCase("1 + 2 * 3 + 4 * 5 + 6", 71)]
        [TestCase("1 + (2 * 3) + (4 * (5 + 6))", 51)]
        [TestCase("2 * 3 + (4 * 5)", 26)]
        [TestCase("5 + (8 * 3 + 9 + 3 * 4 * 3)", 437)]
        [TestCase("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 12240)]
        [TestCase("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 13632)]
        public void Part1_Example1(string input, long expected)
        {
            var parsed = Tokenize(Common.GetLines(input));
            var result = CalcPart1(parsed);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Part1()
        {
            var parsed = Tokenize(Common.DayInput(nameof(Day18)));
            long sum = CalcPart1(parsed);
            Assert.AreNotEqual(486, sum);
            Assert.AreEqual(31142189909908, sum);
        }

        [TestCase("1 + 2 * 3 + 4 * 5 + 6", 231)]
        [TestCase("1 + (2 * 3) + (4 * (5 + 6))", 51)]
        [TestCase("2 * 3 + (4 * 5)", 46)]
        [TestCase("5 + (8 * 3 + 9 + 3 * 4 * 3)", 1445)]
        [TestCase("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 669060)]
        [TestCase("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 23340)]
        public void Part2_Example1(string input, long expected)
        {
            var parsed = Tokenize(Common.GetLines(input));
            var groupStack = new Stack<List<Expression>>();
            var elems = new List<Expression>();
            foreach (MatchCollection tokens in parsed)
            {
                foreach (Match m in tokens)
                {
                    if (Regex.IsMatch(m.Value, @"\d+"))
                    {
                        elems.Add(new Number(long.Parse(m.Value)));
                    }
                    else if (m.Value.Equals("+"))
                    {
                        elems.Add(new Addition());
                    }
                    else if (m.Value.Equals("*"))
                    {
                        elems.Add(new Multiplication());
                    }
                    else if (m.Value.Equals("("))
                    {
                        groupStack.Push(elems);
                        elems = new List<Expression>();
                    }
                    else if (m.Value.Equals(")"))
                    {
                        var group = new Group(ParseGroup(elems));
                        elems = groupStack.Pop();
                        elems.Add(group);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }

            elems = ParseAdd(elems);
            var top = ParseMult(elems);

            Assert.AreEqual(expected, top.Eval());
        }

        private Expression ParseMult(List<Expression> elems)
        {
            var stack = new Stack<Expression>();
            foreach (var elem in elems)
            {
                switch (elem)
                {
                    case Number num:
                        {
                            if (stack.Count > 0 && stack.Peek() is Multiplication last)
                            {
                                stack.Pop();
                                last.e2 = num;
                                stack.Push(last);
                            }
                            else
                            {
                                stack.Push(num);
                            }
                            break;
                        }
                    case Multiplication mult:
                        {
                            if (stack.Count > 0)
                            {
                                mult.e1 = stack.Pop();
                            }
                            stack.Push(mult);
                            break;
                        }
                    case Addition add:
                        {
                            if (stack.Count > 0 && stack.Peek() is Multiplication last)
                            {
                                stack.Pop();
                                last.e2 = add;
                                stack.Push(last);
                            }
                            else
                            {
                                stack.Push(add);
                            }
                            break;
                        }
                    case Group group:
                        {
                            if (stack.Count > 0 && stack.Peek() is Multiplication last)
                            {
                                stack.Pop();
                                last.e2 = group;
                                stack.Push(last);
                            }
                            else
                            {
                                stack.Push(group);
                            }
                            break;
                        }
                    default:
                        throw new NotImplementedException();
                }
            }
            Assert.AreEqual(1, stack.Count);
            return stack.Pop();
        }

        private static List<Expression> ParseAdd(List<Expression> elems)
        {
            var nextElems = new List<Expression>();
            var stack = new Stack<Expression>();
            foreach (var elem in elems)
            {
                switch (elem)
                {
                    case Number num:
                        {
                            if (stack.Count > 0 && stack.Peek() is Addition lastAdd)
                            {
                                stack.Pop();
                                if (stack.Count > 0 && stack.Peek() is Addition anotherAdd)
                                {
                                    var save = anotherAdd.e2;
                                    anotherAdd.e2 = lastAdd;
                                    lastAdd.e1 = save;
                                }
                                lastAdd.e2 = num;
                                stack.Push(lastAdd);
                            }
                            else
                            {
                                stack.Push(num);
                            }
                            break;
                        }
                    case Addition add:
                        {
                            if (stack.Count > 0)
                            {
                                add.e1 = stack.Pop();
                            }
                            stack.Push(add);
                            break;
                        }
                    case Multiplication mult:
                        {
                            if (stack.TryPeek(out var peek))
                            {
                                if (peek is Number || peek is Group || peek is Addition)
                                {
                                    stack.Pop();
                                    nextElems.Add(peek);
                                }
                            }
                            nextElems.Add(mult);
                            break;
                        }
                    case Group group:
                        {
                            if (stack.Count > 0 && stack.Peek() is Addition lastAdd)
                            {
                                stack.Pop();
                                lastAdd.e2 = group;
                                stack.Push(lastAdd);
                            }
                            else
                            {
                                stack.Push(group);
                            }
                            break;
                        }
                    default:
                        throw new NotImplementedException();
                }
            }
            if (stack.Count > 0) nextElems.Add(stack.Pop()); // need more pop?
            Assert.AreEqual(0, stack.Count);
            return nextElems;
        }

        private Expression ParseGroup(List<Expression> elems)
        {
            elems = ParseAdd(elems);
            var top = ParseMult(elems);
            return top;
        }

        [Test]
        public void Part2()
        {
            var parsed = Tokenize(Common.DayInput(nameof(Day18)));
            long sum = 0;
            foreach (MatchCollection tokens in parsed)
            {
                var groupStack = new Stack<List<Expression>>();
                var elems = new List<Expression>();
                foreach (Match m in tokens)
                {
                    if (Regex.IsMatch(m.Value, @"\d+"))
                    {
                        elems.Add(new Number(long.Parse(m.Value)));
                    }
                    else if (m.Value.Equals("+"))
                    {
                        elems.Add(new Addition());
                    }
                    else if (m.Value.Equals("*"))
                    {
                        elems.Add(new Multiplication());
                    }
                    else if (m.Value.Equals("("))
                    {
                        groupStack.Push(elems);
                        elems = new List<Expression>();
                    }
                    else if (m.Value.Equals(")"))
                    {
                        var group = new Group(ParseGroup(elems));
                        elems = groupStack.Pop();
                        elems.Add(group);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                elems = ParseAdd(elems);
                var top = ParseMult(elems);
                var result = top.Eval();
                sum += result;
            }


            Assert.AreNotEqual(187, sum); // too low
            Assert.AreEqual(323912478287549, sum);
        }

    }
}
