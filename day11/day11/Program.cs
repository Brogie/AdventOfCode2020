using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace day11
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("../../../input.txt");
            string[] input = sr.ReadToEnd().Split("\n", StringSplitOptions.RemoveEmptyEntries);
            sr.Close();

            string[] example = new string[] {
                "L.LL.LL.LL",
                "LLLLLLL.LL",
                "L.L.L..L..",
                "LLLL.LL.LL",
                "L.LL.LL.LL",
                "L.LLLLL.LL",
                "..L.L.....",
                "LLLLLLLLLL",
                "L.LLLLLL.L",
                "L.LLLLL.LL",
            };

            Part1(example, 500, false);
            Part2(example, 500, false);

            Console.WriteLine("PART 1: Cold");
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Part1(example, 500, false);
            watch.Stop();
            Console.WriteLine(((double)watch.ElapsedTicks / Stopwatch.Frequency * 1000) + " ms\n");

            Console.WriteLine("PART 2: Cold");
            watch = new Stopwatch();
            watch.Start();
            Part2(example, 500, false);
            watch.Stop();
            Console.WriteLine(((double)watch.ElapsedTicks / Stopwatch.Frequency * 1000) + " ms\n");



            Console.WriteLine("PART 1: Avg 1000");
            watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < 1000; i++)
            {
                Part1(example, 500, false);
            }
            watch.Stop();
            Console.WriteLine(((double)watch.ElapsedTicks / Stopwatch.Frequency * 1000) /1000 + " ms\n");

            Console.WriteLine("PART 2: Avg 1000");
            watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < 1000; i++)
            {
                Part2(example, 500, false);
            }
            watch.Stop();
            Console.WriteLine(((double)watch.ElapsedTicks / Stopwatch.Frequency * 1000) / 1000 + " ms\n");

            Console.WriteLine("[EXAMPLE] PART 1");
            Console.WriteLine("Occupied Seats: " + Part1(example, 500));
            Console.WriteLine("\n[EXAMPLE] PART 2");
            Console.WriteLine("Occupied Seats: " + Part2(example, 500));
            Console.WriteLine("\n[INPUT] PART 1");
            Console.WriteLine("Occupied Seats: " + Part1(input, 10));
            Console.WriteLine("\n[INPUT] PART 2");
            Console.WriteLine("Occupied Seats: " + Part2(input, 0));
        }

        private static int Part1(string[] input, int pause, bool draw = true)
        {
            Tuple<int, int>[] seats = GetSeatLocationsArray(input);
            bool[,] peopleLastFrame = new bool[input.Length + 2, input[0].Length + 2];
            int top = Console.CursorTop;
            int left = Console.CursorLeft;

            bool changeHappened;
            int occupiedSeats;
            int frame = 0;
            do
            {
                if (draw)
                {
                    frame++;
                    if (frame % 2 == 0)
                    {
                        DrawFrame(seats, peopleLastFrame, top, left, pause);
                        Console.WriteLine("Frame: {0}", frame);
                    }
                }
                
                bool[,] people = new bool[input.Length + 2, input[0].Length + 2];
                occupiedSeats = 0;
                changeHappened = false;

                foreach (var seat in seats)
                {
                    bool isOccupied = UpdateSeat(seat, peopleLastFrame);

                    if (isOccupied) { occupiedSeats++; }
                    if (peopleLastFrame[seat.Item1, seat.Item2] != isOccupied) { changeHappened = true; }
                    people[seat.Item1, seat.Item2] = isOccupied;
                }

                peopleLastFrame = people;
            } while (changeHappened);

            return occupiedSeats;
        }
        private static int Part2(string[] input, int pause, bool draw = true)
        {
            Tuple<int, int>[] seats = GetSeatLocationsArray(input);
            bool[,] peopleLastFrame = new bool[input.Length + 2, input[0].Length + 2];

            int top = Console.CursorTop;
            int left = Console.CursorLeft;
            
            int occupiedSeats;
            int frame = 0;
            bool changeHappened;
            Dictionary<string, string> seatLookup = GenSeatLookup(seats);
            do
            {
                if (draw)
                {
                    frame++;
                    if (frame % 2 == 0)
                    {
                        DrawFrame(seats, peopleLastFrame, top, left, pause);
                        Console.WriteLine("Frame: {0}", frame);
                    }
                }

                bool[,] people = new bool[input.Length + 2, input[0].Length + 2];
                occupiedSeats = 0;
                changeHappened = false;

                foreach (var seat in seats)
                {
                    bool isOccupied = UpdateSeatPart2(seat, seatLookup, peopleLastFrame);

                    if (isOccupied) { occupiedSeats++; }
                    if (peopleLastFrame[seat.Item1, seat.Item2] != isOccupied) { changeHappened = true; }
                    people[seat.Item1, seat.Item2] = isOccupied;
                }

                peopleLastFrame = people;
            } while (changeHappened);

            return occupiedSeats;
        }

        private static void DrawFrame(Tuple<int, int>[] seats, bool[,] peopleLastFrame, int top, int left, int pause)
        {
            char[,] drawing = new char[peopleLastFrame.GetLength(0), peopleLastFrame.GetLength(1)];

            for (int i = 0; i < peopleLastFrame.GetLength(0); i++)
            {
                for (int j = 0; j < peopleLastFrame.GetLength(1); j++)
                {
                    drawing[i, j] = '█';
                }
            }

            foreach (var seat in seats)
            {
                drawing[seat.Item1, seat.Item2] = ' ';
            }

            for (int i = 0; i < peopleLastFrame.GetLength(0); i++)
            {
                for (int j = 0; j < peopleLastFrame.GetLength(1); j++)
                {
                    if(peopleLastFrame[i,j])
                    {
                        drawing[i, j] = '■';
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < peopleLastFrame.GetLength(0); i++)
            {
                for (int j = 0; j < peopleLastFrame.GetLength(1); j++)
                {
                    sb.Append(drawing[i, j]);
                }
                sb.Append('\n');
            }

            Console.CursorLeft = left;
            Console.CursorTop = top;
            Thread.Sleep(pause);
            Console.WriteLine(sb.ToString());
        }

        private static bool UpdateSeatPart2(Tuple<int, int> seat, Dictionary<string,string> seats, bool[,] peopleLastFrame)
        {
            int inSight = 0;

            //up
            for (int i = seat.Item1 - 1; i > 0; i--)
            {
                if(seats.ContainsKey(i.ToString() + ',' + seat.Item2.ToString())) {
                    if(peopleLastFrame[i, seat.Item2])
                    {
                        inSight++;
                    }

                    break;
                }
            }

            //down
            for (int i = seat.Item1 + 1; i < peopleLastFrame.GetLength(0); i++)
            {
                if (seats.ContainsKey(i.ToString() + ',' + seat.Item2.ToString()))
                {
                    if (peopleLastFrame[i, seat.Item2])
                    {
                        inSight++;
                    }

                    break;
                }
            }

            //left
            for (int j = seat.Item2 - 1; j > 0; j--)
            {
                if (seats.ContainsKey(seat.Item1.ToString() + ',' + j.ToString()))
                {
                    if (peopleLastFrame[seat.Item1, j])
                    {
                        inSight++;
                    }

                    break;
                }
            }

            //right
            for (int j = seat.Item2 + 1; j < peopleLastFrame.GetLength(1); j++)
            {
                if (seats.ContainsKey(seat.Item1.ToString() + ',' + j.ToString()))
                {
                    if (peopleLastFrame[seat.Item1, j])
                    {
                        inSight++;
                    }

                    break;
                }
            }

            //up left
            int j2 = seat.Item2;
            for (int i = seat.Item1 - 1; i > 0; i--)
            {
                j2--;
                if (seats.ContainsKey(i.ToString() + ',' + j2.ToString()))
                {
                    if (peopleLastFrame[i, j2])
                    {
                        inSight++;
                    }

                    break;
                }

                if (j2 <= 0) { break; }
            }

            //down left
            j2 = seat.Item2;
            for (int i = seat.Item1 + 1; i < peopleLastFrame.GetLength(0); i++)
            {
                j2--;
                if (seats.ContainsKey(i.ToString() + ',' + j2.ToString()))
                {
                    if (peopleLastFrame[i, j2])
                    {
                        inSight++;
                    }

                    break;
                }
                if (j2 <= 0) { break; }
            }

            //up right
            j2 = seat.Item2;
            for (int i = seat.Item1 - 1; i > 0; i--)
            {
                j2++;
                if (seats.ContainsKey(i.ToString() + ',' + j2.ToString()))
                {
                    if (peopleLastFrame[i, j2])
                    {
                        inSight++;
                    }

                    break;
                }
                if (j2 >= peopleLastFrame.GetLength(1)) { break; }
            }

            //down right
            j2 = seat.Item2;
            for (int i = seat.Item1 + 1; i < peopleLastFrame.GetLength(0); i++)
            {
                j2++;
                if (seats.ContainsKey(i.ToString() + ',' + j2.ToString()))
                {
                    if (peopleLastFrame[i, j2])
                    {
                        inSight++;
                    }

                    break;
                }
                
                if (j2 >= peopleLastFrame.GetLength(1)) { break; }
            }

            //If a seat is empty(L) and there are no occupied seats adjacent to it, the seat becomes occupied.
            if (!peopleLastFrame[seat.Item1, seat.Item2] && inSight == 0) { return true; }
            //If a seat is occupied(#) and five or more seats adjacent to it are also occupied, the seat becomes empty.
            if (peopleLastFrame[seat.Item1, seat.Item2] && inSight >= 5) { return false; }
            //Otherwise, the seat's state does not change.
            return peopleLastFrame[seat.Item1, seat.Item2];
        }

        private static bool UpdateSeat(Tuple<int, int> seat, bool[,] peopleLastFrame)
        {
            int count = 0;

            if (peopleLastFrame[seat.Item1 - 1, seat.Item2 - 1]) { count++; }
            if (peopleLastFrame[seat.Item1 - 1, seat.Item2]) { count++; }
            if (peopleLastFrame[seat.Item1 - 1, seat.Item2 + 1]) { count++; }

            if (peopleLastFrame[seat.Item1, seat.Item2 - 1]) { count++; }
            if (peopleLastFrame[seat.Item1, seat.Item2 + 1]) { count++; }

            if (peopleLastFrame[seat.Item1 + 1, seat.Item2 - 1]) { count++; }
            if (peopleLastFrame[seat.Item1 + 1, seat.Item2]) { count++; }
            if (peopleLastFrame[seat.Item1 + 1, seat.Item2 + 1]) { count++; }


            //If a seat is empty(L) and there are no occupied seats adjacent to it, the seat becomes occupied.
            if (!peopleLastFrame[seat.Item1, seat.Item2] && count == 0) { return true; }
            //If a seat is occupied(#) and four or more seats adjacent to it are also occupied, the seat becomes empty.
            if (peopleLastFrame[seat.Item1, seat.Item2] && count >= 4) { return false; }
            //Otherwise, the seat's state does not change.
            return peopleLastFrame[seat.Item1, seat.Item2];
        }

        private static Dictionary<string, string> GenSeatLookup(Tuple<int, int>[] seats)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();
            foreach (var seat in seats)
            {
                output.Add(seat.Item1.ToString() + ',' + seat.Item2.ToString(), null);
            }
            return output;
        }

        private static Tuple<int,int>[] GetSeatLocationsArray(string[] input)
        {
            List<Tuple<int, int>> seatLocations = new List<Tuple<int, int>>();

            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input[i].Length; j++)
                {
                    if (input[i][j] == 'L') { seatLocations.Add(new Tuple<int, int>(i + 1, j + 1)); }
                }
            }

            return seatLocations.ToArray();
        }
    }
}
