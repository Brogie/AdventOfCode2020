using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace day5
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("../../../input.txt");
            string[] seatLocations = sr.ReadToEnd().Split("\n", StringSplitOptions.RemoveEmptyEntries);
            int part1, part2;

            Console.WriteLine("[Attempt 1]");
            Stopwatch watch = new Stopwatch();
            watch.Start();
            part1 = Part1(seatLocations);
            part2 = Part2(seatLocations);
            watch.Stop();
            Console.WriteLine("Part 1: " + part1);
            Console.WriteLine("Part 2: " + part2);
            Console.WriteLine(((double)watch.ElapsedTicks / Stopwatch.Frequency * 1000) + " ms\n");

            Console.WriteLine("[Attempt 2]");
            watch = new Stopwatch();
            watch.Start();
            //Attempt2(seatLocations, out part1, out part2);
            watch.Stop();
            Console.WriteLine("Part 1: " + part1);
            Console.WriteLine("Part 2: " + part2);
            Console.WriteLine(((double)watch.ElapsedTicks / Stopwatch.Frequency * 1000) + " ms\n");

            //reset to be sure
            part1 = 0;
            part2 = 0;

            Console.WriteLine("[Attempt 3]");
            watch = new Stopwatch();
            watch.Start();
            Attempt3(seatLocations, out part1, out part2);
            watch.Stop();
            Console.WriteLine("Part 1: " + part1);
            Console.WriteLine("Part 2: " + part2);
            Console.WriteLine( ((double)watch.ElapsedTicks / Stopwatch.Frequency * 1000) + " ms\n");

            part1 = 0;
            part2 = 0;

            Console.WriteLine("[Attempt 3: avg over 1000]");
            watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < 1000; i++)
            {
                Attempt3(seatLocations, out part1, out part2);
            }
            watch.Stop();
            Console.WriteLine("Part 1: " + part1);
            Console.WriteLine("Part 2: " + part2);
            Console.WriteLine(((double)watch.ElapsedTicks / Stopwatch.Frequency) + " ms\n");

            //for (int i = 0; i < 10000; i++)
            //{
            //    Attempt3(seatLocations, out part1, out part2);
            //}
        }

        static int Part1(string[] seatLocations)
            {
                int highestID = 0;
                foreach (var seat in seatLocations)
                {
                    int row = SearchXorY(seat.Substring(0, 7), 0, 127, 'F', 'B');
                    int column = SearchXorY(seat.Substring(7, 3), 0, 7, 'L', 'R');
                    int id = (row * 8) + column;

                    if (id > highestID)
                    {
                        highestID = id;
                    }
                }
                return highestID;
            }

            static int Part2(string[] seatLocations)
            {
                Dictionary<int, string> seatIds = new Dictionary<int, string>();
                List<int> justIDs = new List<int>();
                foreach (var seat in seatLocations)
                {
                    int row = SearchXorY(seat.Substring(0, 7), 0, 127, 'F', 'B');
                    int column = SearchXorY(seat.Substring(7, 3), 0, 7, 'L', 'R');
                    int id = (row * 8) + column;

                    justIDs.Add(id);
                    seatIds.Add(id, seat);
                }

                return FindMissingId(justIDs);
            }

            static void Attempt2(string[] seatLocations, out int part1, out int part2)
            {
                int highId = 0;
                List<int> ids = new List<int>();
                foreach (var seat in seatLocations)
                {
                    StringBuilder rowString = new StringBuilder(seat);
                    rowString.Replace("F", "0");
                    rowString.Replace("B", "1");
                    rowString.Replace("L", "0");
                    rowString.Replace("R", "1");

                    int id = Convert.ToInt32(rowString.ToString(), 2);

                    ids.Add(id);
                    if (id > highId) { highId = id; }
                }

                part1 = highId;
                part2 = FindMissingId(ids);
            }

            // Cheating at this point by looking at other solutions trying to make it fast
    unsafe static void Attempt3(string[] seatLocations, out int part1, out int part2)
    {
        ushort checksum = 0;
        ushort minId = ushort.MaxValue;
        ushort maxId = ushort.MinValue;
        foreach (var seat in seatLocations)
        {
            ushort id = 0;

            fixed (char* pString = seat)
            {
                char* pChar = pString;
                for (int i = 0; i < 10; i++)
                {
                    if (*pChar == 'B' | *pChar == 'R')
                    {
                        id |= (ushort)(1 << 9 - i);
                    }
                    pChar++;
                }
            }

            checksum ^= id;
            if (id > maxId) { maxId = id; }
            if (id < minId) { minId = id; }
        }

        for (ushort i = ushort.MinValue; i < minId; i++)
        {
            checksum ^= i;
        }

        for (ushort i = (ushort)(maxId + 1); i < 1024; i++)
        {
            checksum ^= i;
        }

        part1 = maxId;
        part2 = checksum;
    }

            static int SearchXorY(string seat, int lowerBound, int upperBound, char lowerCommand, char upperCommand)
            {
                for (int i = 0; i < seat.Length; i++)
                {
                    int midpoint = (upperBound - lowerBound) / 2;
                    if (seat[i] == lowerCommand)
                    {
                        upperBound -= midpoint;
                    }
                    else if (seat[i] == upperCommand)
                    {
                        lowerBound += midpoint;
                    }
                }

                return seat[0] == lowerCommand ? lowerBound : upperBound;
            }

            static int FindMissingId(List<int> ids)
            {
                ids.Sort();
                for (int i = 0; i < ids.Count; i++)
                {
                    if (ids[i + 1] != ids[i] + 1) { return ids[i] + 1; }
                }
                return -1;
            }

        }
    }
