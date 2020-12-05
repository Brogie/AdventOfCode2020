using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace day5
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("../../../input.txt");
            string[] seatLocations = sr.ReadToEnd().Split("\n");
            Console.WriteLine(Part1(seatLocations));
            Console.WriteLine(Part2(seatLocations));
            Attempt2(seatLocations);
        }

        static int Part1(string[] seatLocations)
        {
            int highestID = 0;
            foreach (var seat in seatLocations)
            {
                if (seat == string.Empty) { continue; }
                int row = SearchXorY(seat.Substring(0, 7), 0, 127, 'F', 'B');
                int column = SearchXorY(seat.Substring(7, 3), 0, 7, 'L', 'R');
                int id = (row * 8) + column;

                if(id > highestID)
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
                if (seat == string.Empty) { continue; }
                int row = SearchXorY(seat.Substring(0, 7), 0, 127, 'F', 'B');
                int column = SearchXorY(seat.Substring(7, 3), 0, 7, 'L', 'R');
                int id = (row * 8) + column;

                justIDs.Add(id);
                seatIds.Add(id, seat);
            }

            return FindMissingId(justIDs);
        }

        static void Attempt2(string[] seatLocations)
        {
            int highId = 0;
            List<int> ids = new List<int>();
            foreach (var seat in seatLocations)
            {
                if (seat == string.Empty) { continue; }
                StringBuilder rowString = new StringBuilder(seat.Substring(0, 7));
                StringBuilder columnString = new StringBuilder(seat.Substring(7, 3));
                rowString.Replace("F", "0");
                rowString.Replace("B", "1");
                columnString.Replace("L", "0");
                columnString.Replace("R", "1");

                int row = Convert.ToInt32(rowString.ToString(), 2);
                int column = Convert.ToInt32(columnString.ToString(), 2);
                int id = (row * 8) + column;

                ids.Add(id);
                if (id > highId) { highId = id; }
            }

            Console.WriteLine("Part 1: " + highId);
            Console.WriteLine("Part 2: " + FindMissingId(ids));
        }

        private static int SearchXorY(string seat, int lowerBound, int upperBound, char lowerCommand, char upperCommand)
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

        private static int FindMissingId(List<int> ids)
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
