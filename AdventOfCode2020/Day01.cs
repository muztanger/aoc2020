using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class Day01
    {
        [Test]
        public void Part1()
        {
            Assert.AreEqual(877971, Part1Sol1());
            Assert.AreEqual(877971, Part1Sol2());
        }

        private static int Part1Sol1()
        {
            var numbers = Common.DayInput(nameof(Day01)).Select(x => int.Parse(x)).ToList();
            for (int i = 0; i < numbers.Count - 1; i++)
                for (int j = i + 1; j < numbers.Count; j++)
                    if (numbers[i] + numbers[j] == 2020) return numbers[i] * numbers[j];
            return -1;
        }

        private static int Part1Sol2()
        {
            var numbers = Common.DayInput(nameof(Day01)).Select(x => int.Parse(x)).ToList();
            return (from l1 in numbers
                    from l2 in numbers
                    where l1 < l2 && l1 + l2 == 2020
                    select l1 * l2).First();
        }


        [Test]
        public void Part2()
        {
            var numbers = Common.DayInput(nameof(Day01)).Select(x => int.Parse(x)).ToList();
            for (int i = 0; i < numbers.Count - 2; i++)
            for (int j = i + 1; j < numbers.Count - 1; j++)
            for (int k = j + 1; k < numbers.Count; k++)
                if (numbers[i] + numbers[j] + numbers[k] == 2020) Console.WriteLine(numbers[i] * numbers[j] * numbers[k]);
        }
    }
}
