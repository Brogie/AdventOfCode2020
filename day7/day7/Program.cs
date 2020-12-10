using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace day7
{
    class Program
    {
        struct Bag
        {
            public string colour;
            public Dictionary<string, int> contains;
        }

        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("../../../input.txt");
            string[] input = sr.ReadToEnd().Split("\n", StringSplitOptions.RemoveEmptyEntries);
            sr.Close();

            Console.WriteLine(Part1(input));
            Console.WriteLine(Part2(input));
        }

        private static int Part2(string[] input)
        {
            Dictionary<string, Bag> bags = ProcessBagData(input);

            return ContainedBags(bags["shiny gold"], bags);
        }

        private static int ContainedBags(Bag bag, Dictionary<string, Bag> bags)
        {
            int count = 0;
            foreach (var containedBag in bag.contains)
            {
                count += containedBag.Value;
                count += ContainedBags(bags[containedBag.Key], bags) * containedBag.Value;
            }

            return count;
        }

        private static int Part1(string[] bagDescriptions)
        {
            Dictionary<string, Bag> bags = ProcessBagData(bagDescriptions);
            int canContainCount = 0;

            foreach (var bag in bags)
            {
                if (CanContainShiny(bag.Value, bags)) { canContainCount++; }
            }

            return canContainCount;
        }

        private static bool CanContainShiny(Bag bag, Dictionary<string, Bag> bags)
        {
            foreach (var contained in bag.contains)
            {
                if(contained.Key == "shiny gold") { return true; }
                if(CanContainShiny(bags[contained.Key], bags)) {  return true; }
            }

            return false;
        }

        private static Dictionary<string, Bag> ProcessBagData(string[] bagDescription)
        {
            Dictionary<string, Bag> bags = new Dictionary<string, Bag>();
            foreach (var bag in bagDescription)
            {
                Regex rx = new Regex("(\\d) ([a-z|\\s]*) bag");
                var bagPair = bag.Split(" bags contain ");

                Dictionary<string, int> contents = new Dictionary<string, int>();

                foreach (Match match in rx.Matches(bagPair[1]))
                {
                    contents.Add(match.Groups[2].Value, int.Parse(match.Groups[1].Value));
                }


                bags.Add(bagPair[0], new Bag { colour = bagPair[0], contains = contents });

            }

            return bags;
        }
    }
}
