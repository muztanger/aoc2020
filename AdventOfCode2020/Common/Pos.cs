﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace AdventOfCode2020
{
    public class Pos : IEquatable<Pos>
    {
        public int x;
        public int y;
        public Pos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Pos((int, int) z)
        {
            x = z.Item1;
            y = z.Item2;
        }

        public Pos(Pos other)
        {
            this.x = other.x;
            this.y = other.y;
        }

        public int Dist()
        {
            return Math.Abs(y - x);
        }

        public static Pos operator *(Pos p1, int n)
        {
            return new Pos(p1.x * n, p1.y * n);
        }

        public static Pos operator +(Pos p1, Pos p2)
        {
            return new Pos(p1.x + p2.x, p1.y + p2.y);
        }
        public static Pos operator -(Pos p) => new Pos(-p.x, -p.y);
        public static Pos operator -(Pos p1, Pos p2) => p1 + (-p2);

        public override string ToString()
        {
            return $"({x}, {y})";
        }

        internal int manhattan(Pos inter)
        {
            return Math.Abs(x - inter.x) + Math.Abs(y - inter.y);
        }

        bool IEquatable<Pos>.Equals(Pos other)
        {
            return this.x == other.x && this.y == other.y;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Pos);
        }

        public bool Equals([AllowNull] Pos other)
        {
            return other != null &&
                   x == other.x &&
                   y == other.y;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() * 7919 + y.GetHashCode();
        }

        public bool BetweenXY(int z)
        {
            return z >= x && z <= y;
        }

        internal bool Between(Pos p1, Pos p2)
        {
            if (p1.x == p2.x && p2.x == this.x)
            {
                return (p1.y < y && y < p2.y) || (p2.y < y && y < p1.y);
            }
            if (!(new Line(this, p1).OnLine(p2))) return false;

            if (p1.x < this.x) return p2.x > this.x;
            if (p1.x > this.x) return p2.x < this.x;

            return false;
        }

        internal double Dist(Pos p1)
        {
            var delta = p1 - this;
            return Math.Sqrt(delta.x * (double)delta.x + delta.y * (double)delta.y);
        }
    }
}
