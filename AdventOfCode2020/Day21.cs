using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    public class Day21
    {
        class Food
        {
            internal HashSet<string> ingridients;
            internal HashSet<string> allergens;
        }

        private static int Parse(IEnumerable<string> input)
        {
            var foods = new List<Food>();
            var allIngridients = new HashSet<string>();
            var allAllergens = new HashSet<string>();
            var ingredientToAllergen = new DefaultValueDictionary<string, HashSet<string>>(() => new HashSet<string>());
            var allergensToIngredients = new DefaultValueDictionary<string, HashSet<string>>(() => new HashSet<string>());

            foreach (var line in input)
            {
                var sp = line.Split("(contains");
                var ingredients = sp[0].Trim().Split().ToList();
                var allergens = sp[1][0..^1].Split(",").Select(x => x.Trim()).ToList();

                foods.Add(new Food()
                {
                    ingridients = new HashSet<string>(ingredients),
                    allergens = new HashSet<string>(allergens)
                });

                ingredients.ForEach(x => allIngridients.Add(x));
                allergens.ForEach(x => allAllergens.Add(x));

                foreach (var ingredient in ingredients)
                {
                    foreach (var allergen in allergens)
                    {
                        ingredientToAllergen[ingredient].Add(allergen);
                        allergensToIngredients[allergen].Add(ingredient);
                    }
                }
            }

            // 1. each allergen is found in exactly one ingredient
            // 2. each ingredient contains zero or one allergen
            // 3. Allergens aren't always marked
            // 4. when they're listed the ingredient that contains each listed allergen will be somewhere in the corresponding ingredients list
            // 5. even if an allergen isn't listed, the ingredient that contains that allergen could still be present

            Console.WriteLine("allergensToIngredients before:");
            foreach (var kv in allergensToIngredients)
            {
                Console.WriteLine($"{kv.Key}: {string.Join(",", kv.Value)}");
            }

            foreach (var allergen in allAllergens)
            {
                HashSet<string> candidates = new HashSet<string>(allergensToIngredients[allergen]);
                foreach (var food in foods.Where(x => x.allergens.Contains(allergen)))
                {
                    candidates.IntersectWith(food.ingridients);
                }
                allergensToIngredients[allergen] = candidates;
            }

            Console.WriteLine("after:");
            foreach (var kv in allergensToIngredients)
            {
                Console.WriteLine($"{kv.Key}: {string.Join(",", kv.Value)}");
            }

            var canContainAllergen = new HashSet<string>();
            foreach (var kv in allergensToIngredients)
            {
                canContainAllergen.UnionWith(kv.Value);
            }

            var notAllergens = new HashSet<string>(allIngridients);
            notAllergens.ExceptWith(canContainAllergen);
            
            var result = 0;
            // Counting the number of times any of these ingredients appear in any ingredients list
            foreach (var food in foods)
            {
                result += food.ingridients.Where(x => notAllergens.Contains(x)).Count();
            }
            return result;
        }

        [Test]
        public void Part1_Example1()
        {
            string input = @"mxmxvkd kfcds sqjhc nhms (contains dairy, fish)
trh fvjkl sbzzf mxmxvkd (contains dairy)
sqjhc fvjkl (contains soy)
sqjhc mxmxvkd sbzzf (contains fish)";
            var parsed = Parse(Common.GetLines(input));
            Assert.AreEqual(5, parsed);
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
            var parsed = Parse(Common.DayInput(nameof(Day21)));
            Assert.AreEqual(0, 1);
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
            var parsed = Parse(Common.DayInput(nameof(Day21)));
            Assert.AreEqual(0, 1);
        }

    }
}
