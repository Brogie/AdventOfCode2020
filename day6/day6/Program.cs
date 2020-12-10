using System;
using System.Collections.Generic;
using System.IO;

namespace day6
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("../../../input.txt");
            string[] groups = sr.ReadToEnd().Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine(Part1(groups));
            Console.WriteLine(Part2(groups));
        }

        private static int Part1(string[] groups)
        {
            List<int> groupAnswers = new List<int>();
            foreach (var group in groups)
            {
                List<char> answers = new List<char>();

                foreach (var character in group)
                {
                    if (character == '\n')
                    {
                        continue;
                    }

                    if (!answers.Contains(character))
                    {
                        answers.Add(character);
                    }
                }

                groupAnswers.Add(answers.Count);
            }

            int output = groupAnswers[0];

            for (int i = 1; i < groupAnswers.Count; i++)
            {
                output += groupAnswers[i];
            }

            return output;
        }

        private static int Part2(string[] groups)
        {
            List<int> groupAnswers = new List<int>();
            foreach (var group in groups)
            {
                List<char> validAnswers = new List<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

                foreach (var person in group.Split('\n'))
                {
                    List<char> invalidAnswers = new List<char>();
                    foreach (var answer in validAnswers)
                    {
                        if (!person.Contains(answer))
                        {
                            invalidAnswers.Add(answer);
                        }
                    }
                    validAnswers.RemoveAll(invalidAnswers.Contains);
                }

                groupAnswers.Add(validAnswers.Count);
            }

            int output = groupAnswers[0];

            for (int i = 1; i < groupAnswers.Count; i++)
            {
                output += groupAnswers[i];
            }

            return output;
        }
    }
}
