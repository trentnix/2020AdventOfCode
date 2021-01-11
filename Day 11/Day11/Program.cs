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

            Console.WriteLine("Step 1 - Number of Occupied Seats: {0}", CountOccupiedSeats(rows));
        }

        static int CountOccupiedSeats(string[] rows)
        {
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

            return occupiedSeats;
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

            Console.WriteLine("Step 2 - Number of Occupied Seats: {0}", CountOccupiedSeats(rows));
        }

        static string CheckSeatsVisible(int thisRow, string[] rows)
        {
            int visibilityCount = 0;

            StringBuilder updatedRow = new StringBuilder(rows[thisRow]);
            for (int i = 0; i < rows[thisRow].Length; i++)
            {
                if (rows[thisRow][i] == '.')
                {
                    visibilityCount = -1;
                }
                else
                {
                    visibilityCount = CountVisible(thisRow, i, rows);
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

        static int CountVisible(int seatRow, int seatColumn, string[] rows)
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

            int visibilityCount = 0;
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
            }

            return visibilityCount;
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

/*
 * --- Day 11: Seating System ---
Your plane lands with plenty of time to spare. The final leg of your journey is a ferry that goes directly 
to the tropical island where you can finally start your vacation. As you reach the waiting area to board the 
ferry, you realize you're so early, nobody else has even arrived yet!

By modeling the process people use to choose (or abandon) their seat in the waiting area, you're pretty sure 
you can predict the best place to sit. You make a quick map of the seat layout (your puzzle input).

The seat layout fits neatly on a grid. Each position is either floor (.), an empty seat (L), or an occupied 
seat (#). For example, the initial seat layout might look like this:

L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL
Now, you just need to model the people who will be arriving shortly. Fortunately, people are entirely 
predictable and always follow a simple set of rules. All decisions are based on the number of occupied seats 
adjacent to a given seat (one of the eight positions immediately up, down, left, right, or diagonal from the 
seat). The following rules are applied to every seat simultaneously:

If a seat is empty (L) and there are no occupied seats adjacent to it, the seat becomes occupied.
If a seat is occupied (#) and four or more seats adjacent to it are also occupied, the seat becomes empty.
Otherwise, the seat's state does not change.
Floor (.) never changes; seats don't move, and nobody sits on the floor.

After one round of these rules, every seat in the example layout becomes occupied:

#.##.##.##
#######.##
#.#.#..#..
####.##.##
#.##.##.##
#.#####.##
..#.#.....
##########
#.######.#
#.#####.##
After a second round, the seats with four or more occupied adjacent seats become empty again:

#.LL.L#.##
#LLLLLL.L#
L.L.L..L..
#LLL.LL.L#
#.LL.LL.LL
#.LLLL#.##
..L.L.....
#LLLLLLLL#
#.LLLLLL.L
#.#LLLL.##
This process continues for three more rounds:

#.##.L#.##
#L###LL.L#
L.#.#..#..
#L##.##.L#
#.##.LL.LL
#.###L#.##
..#.#.....
#L######L#
#.LL###L.L
#.#L###.##
#.#L.L#.##
#LLL#LL.L#
L.L.L..#..
#LLL.##.L#
#.LL.LL.LL
#.LL#L#.##
..L.L.....
#L#LLLL#L#
#.LLLLLL.L
#.#L#L#.##
#.#L.L#.##
#LLL#LL.L#
L.#.L..#..
#L##.##.L#
#.#L.LL.LL
#.#L#L#.##
..L.L.....
#L#L##L#L#
#.LLLLLL.L
#.#L#L#.##
At this point, something interesting happens: the chaos stabilizes and further applications of these rules 
cause no seats to change state! Once people stop moving around, you count 37 occupied seats.

Simulate your seating area by applying the seating rules repeatedly until no seats change state. How many 
seats end up occupied?

Your puzzle answer was 2289.

--- Part Two ---
As soon as people start to arrive, you realize your mistake. People don't just care about adjacent seats - 
they care about the first seat they can see in each of those eight directions!

Now, instead of considering just the eight immediately adjacent seats, consider the first seat in each of 
those eight directions. For example, the empty seat below would see eight occupied seats:

.......#.
...#.....
.#.......
.........
..#L....#
....#....
.........
#........
...#.....
The leftmost empty seat below would only see one empty seat, but cannot see any of the occupied ones:

.............
.L.L.#.#.#.#.
.............
The empty seat below would see no occupied seats:

.##.##.
#.#.#.#
##...##
...L...
##...##
#.#.#.#
.##.##.
Also, people seem to be more tolerant than you expected: it now takes five or more visible occupied seats 
for an occupied seat to become empty (rather than four or more from the previous rules). The other rules 
still apply: empty seats that see no occupied seats become occupied, seats matching no rule don't change, 
and floor never changes.

Given the same starting layout as above, these new rules cause the seating area to shift around as follows:

L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL
#.##.##.##
#######.##
#.#.#..#..
####.##.##
#.##.##.##
#.#####.##
..#.#.....
##########
#.######.#
#.#####.##
#.LL.LL.L#
#LLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLL#
#.LLLLLL.L
#.LLLLL.L#
#.L#.##.L#
#L#####.LL
L.#.#..#..
##L#.##.##
#.##.#L.##
#.#####.#L
..#.#.....
LLL####LL#
#.L#####.L
#.L####.L#
#.L#.L#.L#
#LLLLLL.LL
L.L.L..#..
##LL.LL.L#
L.LL.LL.L#
#.LLLLL.LL
..L.L.....
LLLLLLLLL#
#.LLLLL#.L
#.L#LL#.L#
#.L#.L#.L#
#LLLLLL.LL
L.L.L..#..
##L#.#L.L#
L.L#.#L.L#
#.L####.LL
..#.#.....
LLL###LLL#
#.LLLLL#.L
#.L#LL#.L#
#.L#.L#.L#
#LLLLLL.LL
L.L.L..#..
##L#.#L.L#
L.L#.LL.L#
#.LLLL#.LL
..#.L.....
LLL###LLL#
#.LLLLL#.L
#.L#LL#.L#
Again, at this point, people stop shifting around and the seating area reaches equilibrium. Once this 
occurs, you count 26 occupied seats.

Given the new visibility method and the rule change for occupied seats becoming empty, once equilibrium 
is reached, how many seats end up occupied?

Your puzzle answer was 2059.
 * 
 */