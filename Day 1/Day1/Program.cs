using System;
using System.IO;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            Step1();
            Step2();
        }

        static void Step1()
        {
            //file in disk
            var FileUrl = @"D:\Source\Advent of Code\Day 1\day1.txt";

            //file lines
            string[] lines = File.ReadAllLines(FileUrl);
            int[] numbers = Array.ConvertAll(lines, s => int.Parse(s));

            bool resolution = false;

            int numItems = lines.Length;

            int num1 = 0; int num2 = 0;
            for (int n = 0; n < numItems; n++)
            {
                num1 = numbers[n];
                for (int y = n + 1; y < numItems; y++)
                {
                    if (2020 == (num1 + numbers[y]))
                    {
                        num2 = numbers[y];
                        resolution = true;
                        break;
                    }
                }

                if (resolution)
                    break;
            }

            Console.WriteLine("Step 1 - {0} * {1} = {2}", num1, num2, num1 * num2);
        }

        static void Step2()
        {
            //file in disk
            var FileUrl = @"D:\Source\Advent of Code\Day 1\day1.txt";

            //file lines
            string[] lines = File.ReadAllLines(FileUrl);
            int[] numbers = Array.ConvertAll(lines, s => int.Parse(s));

            bool resolution = false;

            int numItems = lines.Length;

            int num1 = 0; int num2 = 0; int num3 = 0;
            for (int x = 0; x < numItems; x++)
            {
                num1 = numbers[x];
                for (int y = x + 1; y < numItems; y++)
                {
                    num2 = numbers[y];

                    for (int z = y + 1; z < numItems; z++)
                    { 
                        if (2020 == (num1 + num2 + numbers[z]))
                        {
                            num3 = numbers[z];
                            resolution = true;
                            break;
                        }
                    }

                    if (resolution)
                        break;
                }

                if (resolution)
                    break;
            }

            Console.WriteLine("Step 2 - {0} * {1} * {2} = {3}", num1, num2, num3, num1 * num2 * num3);
        }
    }
}

/*
 * 
 * --- Day 1: Report Repair ---
After saving Christmas five years in a row, you've decided to take a vacation at a nice resort on a tropical 
island. Surely, Christmas will go on without you.

The tropical island has its own currency and is entirely cash-only. The gold coins used there have a little 
picture of a starfish; the locals just call them stars. None of the currency exchanges seem to have heard of them, 
but somehow, you'll need to find fifty of these coins by the time you arrive so you can pay the deposit on your 
room.

To save your vacation, you need to get all fifty stars by December 25th.

Collect stars by solving puzzles. Two puzzles will be made available on each day in the Advent calendar; the second 
puzzle is unlocked when you complete the first. Each puzzle grants one star. Good luck!

Before you leave, the Elves in accounting just need you to fix your expense report (your puzzle input); apparently, 
something isn't quite adding up.

Specifically, they need you to find the two entries that sum to 2020 and then multiply those two numbers together.

For example, suppose your expense report contained the following:

1721
979
366
299
675
1456
In this list, the two entries that sum to 2020 are 1721 and 299. Multiplying them together produces 
1721 * 299 = 514579, so the correct answer is 514579.

Of course, your expense report is much larger. Find the two entries that sum to 2020; what do you get if you 
multiply them together?

Your puzzle answer was 1014624.

--- Part Two ---
The Elves in accounting are thankful for your help; one of them even offers you a starfish coin they had left over 
from a past vacation. They offer you a second one if you can find three numbers in your expense report that meet the 
same criteria.

Using the above example again, the three entries that sum to 2020 are 979, 366, and 675. Multiplying them together 
produces the answer, 241861950.

In your expense report, what is the product of the three entries that sum to 2020?

Your puzzle answer was 80072256.
 * 
 */