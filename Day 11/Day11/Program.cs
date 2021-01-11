using System;
using System.IO;
using System.Text;

namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            //file in disk
            var FileUrl = @"D:\Source\Advent of Code\Day 11\day11.txt";

            //file lines
            string[] lines = File.ReadAllLines(FileUrl);

            string[] step1 = (string[])lines.Clone();
            string[] step2 = (string[])lines.Clone();

            Step1(step1);
            Step2(step2);
        }

        static void Step1(string[] rows)
        {
            bool seatChanged = true;
            long numChanges = 0;
            
            string[] rowsChange = (string[])rows.Clone();
            while (seatChanged)
            {
                seatChanged = false;

                for (int i = 0; i < rows.Length; i++)
                {
                    string alteredRow = String.Empty;
                    if (i == 0)
                    {
                        alteredRow = CheckSeatsAdjacent(rows[i], String.Empty, rows[i + 1]);
                    }
                    else if ( i == rows.Length - 1)
                    {
                        alteredRow = CheckSeatsAdjacent(rows[i], rows[i - 1], String.Empty);
                    } 
                    else
                    {
                        alteredRow = CheckSeatsAdjacent(rows[i], rows[i - 1], rows[i + 1]);
                    }

                    if (String.Compare(alteredRow, rows[i]) != 0)
                    {
                        seatChanged = true;
                    }

                    rowsChange[i] = alteredRow;
                }

                rows = (string[])rowsChange.Clone();

                numChanges++;
            }

            int occupiedSeats = 0;
            foreach (string row in rows)
            {
                foreach (char seat in row)
                {
                    if (seat == '#')
                    {
                        occupiedSeats++;
                    }
                }
            }

            Console.WriteLine("Step 1 - Number of Occupied Seats: {0}", occupiedSeats);
        }

        static string CheckSeatsAdjacent(string row, string previousRow, string nextRow)
        {
            int min = 0; 
            int max = row.Length - 1;
            StringBuilder updatedRow = new StringBuilder(row);

            for (int i = 0; i < row.Length; i++)
            {
                if (row[i] == '.')
                {
                    continue;
                }

                int[] adjacents;
                if (i == min)
                {
                    adjacents = new int[] { i, i + 1 };
                }
                else if (i == max)
                {
                    adjacents = new int[] { i-1, i };
                }
                else
                {
                    adjacents = new int[] { i - 1, i, i + 1 };
                }

                int numOccupied = 0;
                if (previousRow.Length > 0)
                {
                    foreach(int column in adjacents)
                    {
                        if (previousRow[column] == '#')
                        {
                            numOccupied++;
                        }
                    }
                }

                // this row
                foreach (int column in adjacents)
                {
                    if (column == i)
                        continue;

                    if (row[column] == '#')
                    {
                        numOccupied++;
                    }
                }

                if (nextRow.Length > 0)
                {
                    foreach (int column in adjacents)
                    {
                        if (nextRow[column] == '#')
                        {
                            numOccupied++;
                        }
                    }
                }

                // rule 1
                if (numOccupied == 0)
                {
                    updatedRow[i] = '#';
                }

                // rule 2
                if (numOccupied >= 4)
                {
                    updatedRow[i] = 'L';
                }
            }

            return updatedRow.ToString();
        }


        static void Step2(string[] rows)
        {
            bool seatChanged = true;
            long numChanges = 0;

            string[] rowsChange = (string[])rows.Clone();
            while (seatChanged)
            {
                seatChanged = false;

                for (int i = 0; i < rows.Length; i++)
                {
                    string alteredRow = CheckSeatsVisible(i, rows);

                    if (String.Compare(alteredRow, rows[i]) != 0)
                    {
                        seatChanged = true;
                    } 

                    rowsChange[i] = alteredRow;
                }

                rows = (string[])rowsChange.Clone();

                numChanges++;
            }

            int occupiedSeats = 0;
            foreach (string row in rows)
            {
                foreach (char seat in row)
                {
                    if (seat == '#')
                    {
                        occupiedSeats++;
                    }
                }
            }

            Console.WriteLine("Step 2 - Number of Occupied Seats: {0}", occupiedSeats);
        }

        static string CheckSeatsVisible(int thisRow, string[] rows)
        {
            int visibilityCount = -1;
            int adjacentCount = -1;

            StringBuilder updatedRow = new StringBuilder(rows[thisRow]);
            for (int i = 0; i < rows[thisRow].Length; i++)
            {
                if (rows[thisRow][i] == '.')
                {
                    adjacentCount = -1;
                    visibilityCount = -1;
                }
                else
                {
                    CountVisible(thisRow, i, rows, out adjacentCount, out visibilityCount);
                }

                // rule 1
                if (visibilityCount == 0)
                {
                    updatedRow[i] = '#';
                }

                // rule 2 (different for step 2)
                if (visibilityCount >= 5)
                {
                     updatedRow[i] = 'L';
                }
            }

            return updatedRow.ToString();
        }

        static void CountVisible(int seatRow, int seatColumn, string[] rows, out int adjacentCount, out int visibilityCount)
        {
            int minColumns = 0; 
            int minRows = 0;
            int maxColumns = rows[seatRow].Length; 
            int maxRows = rows.Length;
            
            bool up = false;
            bool down = false;
            bool left = false;
            bool right = false;
            bool upLeft = false;
            bool downRight = false;
            bool upRight = false;
            bool downLeft = false;

            visibilityCount = 0;
            adjacentCount = 0;
            for (int i = minColumns + 1; i < maxColumns; i++)
            {
                if (seatColumn - i >= 0)
                {
                    // left
                    if (!left)
                    {
                        if (IsOccupied(seatColumn - i, seatRow, rows))
                        {
                            left = true;
                            visibilityCount++;
                        }
                        else if (IsEmpty(seatColumn - i, seatRow, rows))
                        {
                            left = true;
                        }
                    }
                }

                if (seatColumn + i < maxColumns)
                {
                    // right
                    if (!right)
                    {
                        if (IsOccupied(seatColumn + i, seatRow, rows))
                        {
                            right = true;
                            visibilityCount++;
                        }
                        else if (IsEmpty(seatColumn + i, seatRow, rows))
                        {
                            right = true;
                        }
                    }
                }

                if (seatRow - i >= 0)
                {
                    // up
                    if (!up)
                    {
                        if (IsOccupied(seatColumn, seatRow - i, rows))
                        {
                            up = true;
                            visibilityCount++;
                        }
                        else if (IsEmpty(seatColumn, seatRow - i, rows))
                        {
                            up = true;
                        }
                    }
                }

                if (seatRow + i < maxRows)
                {
                    // down
                    if (!down)
                    {
                        if (IsOccupied(seatColumn, seatRow + i, rows))
                        {
                            down = true;
                            visibilityCount++;
                        }
                        else if (IsEmpty(seatColumn, seatRow + i, rows))
                        {
                            down = true;
                        }
                    }
                }

                if (seatColumn - i >= 0 && seatRow - 1 >= 0)
                {
                    // up/left
                    if (!upLeft)
                    {
                        if (IsOccupied(seatColumn - i, seatRow - i, rows))
                        {
                            upLeft = true;
                            visibilityCount++;
                        }
                        else if (IsEmpty(seatColumn - i, seatRow - i, rows))
                        {
                            upLeft = true;
                        }
                    }
                }

                if ((seatColumn + i < maxColumns) && (seatRow + i < maxRows))
                {
                    // down/right
                    if (!downRight)
                    {
                        if (IsOccupied(seatColumn + i, seatRow + i, rows))
                        {
                            downRight = true;
                            visibilityCount++;
                        }
                        else if (IsEmpty(seatColumn + i, seatRow + i, rows))
                        {
                            downRight = true;
                        }
                    }
                }

                if ((seatColumn + i < maxColumns) && (seatRow - i >= minRows))
                {
                    // up/right
                    if (!upRight)
                    {
                        if (IsOccupied(seatColumn + i, seatRow - i, rows))
                        {
                            upRight = true;
                            visibilityCount++;
                        }
                        else if (IsEmpty(seatColumn + i, seatRow - i, rows))
                        {
                            upRight = true;
                        }
                    }
                }

                if ((seatColumn - i >= minColumns) && (seatRow + i < maxRows))
                {
                    // down/left
                    if (!downLeft)
                    {
                        if (IsOccupied(seatColumn - i, seatRow + i, rows))
                        {
                            downLeft = true;
                            visibilityCount++;
                        }
                        else if (IsEmpty(seatColumn - i, seatRow + i, rows))
                        {
                            downLeft = true;
                        }
                    }
                }

                if (i == minColumns + 1)
                {
                    adjacentCount = visibilityCount;
                }
            }
        }

        static bool IsOccupied(int column, int row, string[] rows)
        {
            if (column < 0 || row < 0)
                return false;

            if (column > rows[0].Length)
                return false;

            if (row >= rows.Length)
                return false;

            if (rows[row][column] == '#')
            {
                return true;
            }

            return false;
        }

        static bool IsEmpty(int column, int row, string[] rows)
        {
            if (column < 0 || row < 0)
                return false;

            if (column > rows[0].Length)
                return false;

            if (row >= rows.Length)
                return false;

            if (rows[row][column] == 'L')
            {
                return true;
            }

            return false;
        }

        static void PrintGrid(string[] rows)
        {
            foreach (string row in rows)
            {
                Console.WriteLine("{0}", row);
            }

            Console.ReadKey();
            Console.WriteLine();
        }
    }
}
