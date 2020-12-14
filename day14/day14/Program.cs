using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Text.RegularExpressions;

namespace day14
{
    class InstructionSet
    {
        private int[] mask0s;
        private int[] mask1s;
        private int[] maskXs;

        public Dictionary<long, long> memory;

        public InstructionSet(string input, bool part1 = true)
        {
            Regex rx = new Regex("^mem\\[(\\d*)\\] = (\\d*)");
            string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            CreateMask(lines[0]);
            memory = new Dictionary<long, long>();

            if (part1)
            {
                Part1Process(rx, lines);
            }
            else
            {
                Part2Process(rx, lines);
            }
        }

        private void Part1Process(Regex rx, string[] lines)
        {
            for (int i = 1; i < lines.Length; i++)
            {
                var match = rx.Match(lines[i]);
                long memoryLocation = long.Parse(match.Groups[1].Value);
                long data = ApplyMask(long.Parse(match.Groups[2].Value));

                WriteToAddress(memoryLocation, data);
            }
        }

        private void WriteToAddress(long memoryLocation, long data)
        {
            if (memory.ContainsKey(memoryLocation))
            {
                memory[memoryLocation] = data;
            }
            else
            {
                memory.Add(memoryLocation, data);
            }
        }

        private void Part2Process(Regex rx, string[] lines)
        {
            for (int i = 1; i < lines.Length; i++)
            {
                var match = rx.Match(lines[i]);
                SetAllMemoryLocations(long.Parse(match.Groups[1].Value), long.Parse(match.Groups[2].Value));
            }
        }

        private void SetAllMemoryLocations(long memoryLocation, long data)
        {
            //If the bitmask bit is 1, the corresponding memory address bit is overwritten with 1.
            for (int i = 0; i < mask1s.Length; i++) { memoryLocation |= 1L << mask1s[i]; }

            //If the bitmask bit is X, the corresponding memory address bit is floating.
            int totalMemoryLocations = (int)Math.Pow(2, maskXs.Length);

            for (int i = 0; i < totalMemoryLocations; i++)
            {
                for (int j = 0; j < maskXs.Length; j++)
                {
                    if(1 == ((i >> j) & 1))
                    {
                        memoryLocation |= 1L << maskXs[j];
                    } 
                    else if (0 == ((i >> j) & 1))
                    {
                        memoryLocation &= ~(1L << maskXs[j]);
                    }
                }

                WriteToAddress(memoryLocation, data);
            }
        }

        private void CreateMask(string maskString)
        {
            List<int> ones = new List<int>();
            List<int> zeros = new List<int>();
            List<int> Xs = new List<int>();

            for (int i = 0; i < maskString.Length; i++)
            {
                if(maskString[i] == '0') { zeros.Add(maskString.Length - i - 1); } 
                else if (maskString[i] == '1') { ones.Add(maskString.Length - i - 1); }
                else if (maskString[i] == 'X') { Xs.Add(maskString.Length - i - 1); }
            }

            mask0s = zeros.ToArray();
            mask1s = ones.ToArray();
            maskXs = Xs.ToArray();
        }

        private long ApplyMask(long data)
        {
            for (int i = 0; i < mask1s.Length; i++) { data |= 1L << mask1s[i]; }
            for (int i = 0; i < mask0s.Length; i++) { data &= ~(1L << mask0s[i]); }

            return data;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("../../../input.txt");
            string[] input = sr.ReadToEnd().Split("mask = ", StringSplitOptions.RemoveEmptyEntries);
            sr.Close();

            Console.WriteLine("[Part 1]");
            Stopwatch watch = new Stopwatch();
            watch.Start();
            BigInteger part1 = Process(input, true);
            watch.Stop();
            Console.WriteLine("Part 1: " + part1);
            Console.WriteLine(((double)watch.ElapsedTicks / Stopwatch.Frequency * 1000) + " ms\n");

            Console.WriteLine("[Part 2]");
            watch = new Stopwatch();
            watch.Start();
            BigInteger part2 = Process(input, false);
            watch.Stop();
            Console.WriteLine("Part 2: " + part2);
            Console.WriteLine(((double)watch.ElapsedTicks / Stopwatch.Frequency * 1000) + " ms\n");
        }

        private static BigInteger Process(string[] input, bool part1 = true)
        {
            Dictionary<long, long> memory = new Dictionary<long, long>();
            BigInteger output = 0;

            foreach (var program in input)
            {
                InstructionSet instruction = new InstructionSet(program, part1);

                foreach (var memoryLocation in instruction.memory.Keys)
                {
                    if (memory.ContainsKey(memoryLocation))
                    {
                        memory[memoryLocation] = instruction.memory[memoryLocation];
                    }
                    else
                    {
                        memory.Add(memoryLocation, instruction.memory[memoryLocation]);
                    }
                }
            }

            foreach (var memoryLocation in memory.Keys)
            {
                output += memory[memoryLocation];
            }


            return output;
        }
    }
}
