using System;
using System.Collections.Generic;
using System.IO;

namespace day7
{
    class Program
    {
        struct bag
        {
            public string colour;
            public Dictionary<string, int> contains;
        }

        static List<bag> bags = new List<bag>();
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("../../../sample.txt");
            string[] input = sr.ReadToEnd().Split("\r\n");
            sr.Close();

            Console.WriteLine(Part1(input));
        }

        private static bool Part1(string[] bagDescription)
        {
            foreach (var bag in bagDescription)
            {
                var bagPair = bag.Split(" bags contain ");

                Dictionary<string, int> contents = new Dictionary<string, int>();

                foreach (var content in bagPair[1].Split(", "))
                {

                }

                bags.Add(new bag { colour = bagPair[0] });
                
            }

            throw new NotImplementedException();
        }
    }
}
