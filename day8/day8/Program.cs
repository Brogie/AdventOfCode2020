using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace day8
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("../../../input.txt");
            string[] input = sr.ReadToEnd().Split("\n", StringSplitOptions.RemoveEmptyEntries);
            sr.Close();

            string[] exampleInput = new string[] {
                "nop +0",
                "acc +1",
                "jmp +4",
                "acc +3",
                "jmp -3",
                "acc -99",
                "acc +1",
                "jmp -4",
                "acc +6"
            };

            Console.WriteLine(Part1(exampleInput));
            Console.WriteLine(Part1(input));
            Console.WriteLine(Part2(exampleInput));
            Console.WriteLine(Part2(input));
        }

        private static int Part1(string[] input)
        {
            Tuple<string, int>[] instructions = InterpretInstructions(input);

            int stackPointer = 0;
            int acc = 0;

            List<int> executedInstructions = new List<int>();

            while (stackPointer < input.Length)
            {
                if(executedInstructions.Contains(stackPointer)) { return acc; }

                executedInstructions.Add(stackPointer);

                switch (instructions[stackPointer].Item1)
                {
                    case "acc":
                        acc += instructions[stackPointer].Item2;
                        stackPointer++;
                        break;
                    case "jmp":
                        stackPointer += instructions[stackPointer].Item2;
                        break;
                    case "nop":
                        stackPointer++;
                        break;
                    default:
                        break;
                }
            }


            return -1;
        }

        private static int Part2(string[] input)
        {
            Tuple<string, int>[] instructions = InterpretInstructions(input);

            for (int i = 0; i < instructions.Length; i++)
            {
                int stackPointer = 0;
                int acc = 0;
                bool infinateLoopDetected = false;

                if (instructions[i].Item1 == "acc") { continue; }

                Tuple<string, int> corruptedInst = new Tuple<string, int>(instructions[i].Item1 == "nop" ? "jmp" : "nop", instructions[i].Item2);
                instructions[i] = corruptedInst;

                List<int> executedInstructions = new List<int>();

                while (stackPointer >= 0 && stackPointer < input.Length && !infinateLoopDetected)
                {
                    if (executedInstructions.Contains(stackPointer)) { infinateLoopDetected = true; }
                    executedInstructions.Add(stackPointer);

                    switch (instructions[stackPointer].Item1)
                    {
                        case "acc":
                            acc += instructions[stackPointer].Item2;
                            stackPointer++;
                            break;
                        case "jmp":
                            stackPointer += instructions[stackPointer].Item2;
                            break;
                        case "nop":
                            stackPointer++;
                            break;
                        default:
                            break;
                    }
                }

                if(stackPointer == instructions.Length) { return acc; }

                corruptedInst = new Tuple<string, int>(instructions[i].Item1 == "nop" ? "jmp" : "nop", instructions[i].Item2);
                instructions[i] = corruptedInst;
            }


            return -1;
        }

        private static Tuple<string, int>[] InterpretInstructions(string[] input)
        {
            Regex rx = new Regex("([a-z]{3}) ([+|-])(\\d*)");
            Tuple<string, int>[] instructions = new Tuple<string, int>[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                var match = rx.Match(input[i]);

                int number = 0;

                if (match.Groups[2].Value == "+")
                {
                    number = int.Parse(match.Groups[3].Value);
                } 
                else
                {
                    number -= int.Parse(match.Groups[3].Value);
                }

                instructions[i] = new Tuple<string, int>(match.Groups[1].Value, number);
            }

            return instructions;
        }
    }
}
