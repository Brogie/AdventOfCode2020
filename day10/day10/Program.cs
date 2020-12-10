using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;

namespace day10
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] example = new string[] { "16", "10", "15", "5", "1", "11", "7", "19", "6", "12", "4" };
            string[] example2 = new string[] { "28", "33", "18", "42", "31", "14", "46", "20", "48", "47", "24", "23", "49", "45", "19", "38", "39", "11", "1", "32", "25", "35", "8", "17", "7", "9", "4", "2", "34", "10", "3" };
            StreamReader sr = new StreamReader("../../../input.txt");
            string[] input = sr.ReadToEnd().Split("\n", StringSplitOptions.RemoveEmptyEntries);
            sr.Close();


            Part1(example);


            Console.WriteLine("[Preprocess data]");

            Stopwatch watch = new Stopwatch();
            watch.Start();
            IntifyAndProcess(input);
            watch.Stop();
            Console.WriteLine(((double)watch.ElapsedTicks / Stopwatch.Frequency * 1000) + " ms\n");

            Console.WriteLine("[Big input]");

            watch = new Stopwatch();
            watch.Start();
            int part1 = Part1(input);
            watch.Stop();
            Console.WriteLine("Part 1: {0}", part1);
            Console.WriteLine(((double)watch.ElapsedTicks / Stopwatch.Frequency * 1000) + " ms\n");


            watch = new Stopwatch();
            watch.Start();
            BigInteger part2 = 0;
            for (int i = 0; i < 1000; i++)
            {
                part2 = Part2(input);
            }
            watch.Stop();
            Console.WriteLine("Part 2: {0} (Average of 1000)", part2);
            Console.WriteLine(((double)watch.ElapsedTicks / Stopwatch.Frequency * 1000) / 1000 + " ms\n");
        }

        private static BigInteger Part2(string[] input)
        {
            int[] adapters = IntifyAndProcess(input);
            int sequenceStartPointer = 0;

            BigInteger posibilities = 1;

            for (int i = 1; i < adapters.Length; i++)
            {
                if(adapters[i] - adapters[i-1] == 3)
                {
                    switch (i - sequenceStartPointer)
                    {
                        case 1:
                        case 2:
                            posibilities *= 1;
                            break;
                        case 3:
                            posibilities *= 2;
                            break;
                        case 4:
                            posibilities *= 4;
                            break;
                        case 5:
                            posibilities *= 7;
                            break;
                        default:
                            break;
                    }

                    sequenceStartPointer += i - sequenceStartPointer;
                }
            }

            return posibilities;
        }

        private static void Part2Attempt1(string[] input)
        {
            int[] adapters = IntifyAndProcess(input);
            List<int[]> subSequnces = new List<int[]>();
            int startCut = 0;


            for (int i = 1; i < adapters.Length; i++)
            {
                if (adapters[i] - adapters[i - 1] == 3)
                {
                    subSequnces.Add(adapters.Skip(startCut).Take(i - startCut).ToArray());
                    startCut += i - startCut;

                }
            }

            throw new NotImplementedException();
        }

        private static int[] IntifyAndProcess(string[] input)
        {
            int[] output = new int[input.Length + 2];
            int max = int.MinValue;

            for (int i = 0; i < input.Length; i++)
            {
                output[i] = int.Parse(input[i]);

                if(output[i] > max) { max = output[i]; }
            }

            output[output.Length-1] = max + 3;

            Array.Sort(output);

            return output;
        }

        private static int Part1(string[] input)
        {
            int[] adapters = Intify(input);
            Array.Sort(adapters);
            int ones = 0, threes = 0, currentJolts = 0;

            for (int i = 0; i < adapters.Length; i++)
            {
                if(currentJolts - adapters[i] == -1)
                {
                    ones++;
                } 
                else if (currentJolts - adapters[i] == -3)
                {
                    threes++;
                }

                currentJolts = adapters[i];
            }
            threes++;
            return ones * threes;
        }

        private static int[] Intify(string[] input)
        {
            int[] output = new int[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                output[i] = int.Parse(input[i]);
            }

            return output;
        }
    }
}
