using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace day9
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("../../../input.txt");
            string[] input = sr.ReadToEnd().Split("\n", StringSplitOptions.RemoveEmptyEntries);
            sr.Close();
            Console.WriteLine(Part1(input));
            Console.WriteLine(Part2(input, Part1(input)));
        }

        private static int Part1(string[] input)
        {
            List<int> buffer = new List<int>();

            foreach (var item in input)
            {
                int checkNumber = int.Parse(item);
                if (buffer.Count == 25 && !IsValid(checkNumber, buffer))
                {
                    return checkNumber;
                }

                buffer.Add(checkNumber);

                if(buffer.Count > 25)
                {
                    buffer.RemoveAt(0);
                }
            }

            return -1;
        }

        private static int Part2(string[] input, int invalidNumber)
        {
            BigInteger[] intInput = intify(input);

            for (int i = 0; i < intInput.Length; i++)
            {
                List<BigInteger> checkNums = new List<BigInteger>();
                for (int j = i; j < intInput.Length; j++)
                {
                    checkNums.Add(intInput[j]);
                    BigInteger sum = SumOf(checkNums);

                    if (sum == invalidNumber)
                    {
                        return SmallestAndLargest(checkNums);
                    } 
                    else if (sum > invalidNumber)
                    {
                        break;
                    }
                }
            }

            return -1;
        }

        private static int SmallestAndLargest(List<BigInteger> numbers)
        {
            int min = int.MaxValue;
            int max = int.MinValue;

            foreach (var num in numbers)
            {
                if (num < min) { min = (int)num; }
                if (num > max) { max = (int)num; }
            }

            return min + max;
        }

        private static BigInteger SumOf(List<BigInteger> checkNums)
        {
            BigInteger output = 0;

            foreach (var number in checkNums)
            {
                output += number;
            }

            return output;
        }

        private static BigInteger[] intify(string[] input)
        {
            BigInteger[] output = new BigInteger[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                output[i] = BigInteger.Parse(input[i]);
            }

            return output;
            
        }

        private static bool IsValid(int checkNumber, List<int> buffer)
        {
            for (int i = 0; i < buffer.Count; i++)
            {
                for (int j = i + 1; j < buffer.Count; j++)
                {
                    if(buffer[i] + buffer[j] == checkNumber) { return true; }
                }
            }
            return false;
        }
    }
}
