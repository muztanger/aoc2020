using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020
{
    public class Line
    {
        private readonly Pos mP1;
        private readonly Pos mP2;
        private bool mIsVertical;
        private int mDx;
        private int mDy;
        double mSlope = 0;

        public Line(Pos p1, Pos p2)
        {
            mP1 = p1;
            mP2 = p2;
            mIsVertical = p1.x == p2.x;
            mDx = p2.x - p1.x;
            mDy = p2.y - p1.y;
            if (!mIsVertical)
            {
                mSlope = mDy / (double)mDx;
            }
        }

        public bool OnLine(Pos pos)
        {
            return mDx * (pos.y - mP1.y) - mDy * (pos.x - mP1.x) == 0;
        }
    }
}
