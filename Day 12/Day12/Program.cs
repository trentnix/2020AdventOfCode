using System;
using System.Collections.Generic;
using System.IO;

namespace Day12
{
    class Program
    {
        public class Instruction
        {
            public char command 
            {
                get;
            }

            public int magnitude
            {
                get;
            }

            public Instruction(string instruction)
            {
                if (instruction.Length == 0)
                {
                    command = ' ';
                    magnitude = -1;

                    return;
                }

                command = instruction[0];
                magnitude = int.Parse(instruction.Substring(1));
            }

            public Instruction(char thisCommand, int thisMagnitude)
            {
                command = thisCommand;
                magnitude = thisMagnitude;
            }
        }

        public class Location
        {
            public long x = 0;
            public long y = 0;
            public char facing = ' ';
        }

        public static string Directions = "NESW";

        static void Main(string[] args)
        {
            //file in disk
            var FileUrl = @"D:\Source\Advent of Code\Day 12\day12.txt";

            //file lines
            string[] lines = File.ReadAllLines(FileUrl);

            string[] step1 = (string[])lines.Clone();
            Step1(step1);
            string[] step2 = (string[])lines.Clone();
            Step2(step2);
        }

        static void Step1(string[] lines)
        {
            List<Instruction> instructions = new List<Instruction>();
            foreach (string instruction in lines)
            {
                Instruction thisInstruction = new Instruction(instruction);
                instructions.Add(thisInstruction);
            }

            Location currentLocation = new Location() { x = 0, y = 0, facing = 'E' };
            foreach (Instruction instruction in instructions)
            {
                currentLocation = ChangePosition(instruction, currentLocation);
            }

            Console.WriteLine("Step 1 - x:{0} + y:{1} = {2}", Math.Abs(currentLocation.x), Math.Abs(currentLocation.y), Math.Abs(currentLocation.x) + Math.Abs(currentLocation.y));
        }

        private static Location ChangePosition(Instruction instruction, Location currentLocation)
        {
            Location updatedLocation = null;
            switch (instruction.command)
            {
                case 'F':
                case 'L':
                case 'R':
                    updatedLocation = ChangeDirection(instruction, currentLocation);
                    break;
                case 'N':
                case 'S':
                case 'E':
                case 'W':
                    updatedLocation = MovePosition(instruction, currentLocation);
                    break;
            }
            
            if (updatedLocation == null)
            {
                return currentLocation;
            }

            return updatedLocation;
        }

        private static Location MovePosition(Instruction instruction, Location currentLocation)
        {
            switch (instruction.command)
            {
                case 'N':
                    currentLocation.y = currentLocation.y + instruction.magnitude;
                    break;
                case 'S':
                    currentLocation.y = currentLocation.y - instruction.magnitude;
                    break;
                case 'E':
                    currentLocation.x = currentLocation.x + instruction.magnitude;
                    break;
                case 'W':
                    currentLocation.x = currentLocation.x - instruction.magnitude;
                    break;
            }

            return currentLocation;
        }

        private static Location ChangeDirection(Instruction instruction, Location currentLocation)
        {
            int currentDirectionIndex = Directions.IndexOf(currentLocation.facing);
            int numTurns = (instruction.magnitude / 90 % Directions.Length);
            
            switch (instruction.command)
            {
                case 'F':
                    Instruction forward = new Instruction(currentLocation.facing, instruction.magnitude);
                    return MovePosition(forward, currentLocation);
                case 'L':
                    currentLocation.facing = Directions[mod((currentDirectionIndex - numTurns), 4)];
                    break;
                case 'R':
                    currentLocation.facing = Directions[mod((currentDirectionIndex + numTurns), 4)];
                    break;
            }

            return currentLocation;
        }

        private static int mod(int x, int m)
        {
            return (x%m + m) % m;
        }

        static void Step2(string[] lines)
        {
            Location waypointLocation = new Location()  { x = 10, y = 1};
            Location shipLocation = new Location() { x = 0, y = 0};

            List<Instruction> instructions = new List<Instruction>();
            foreach (string instruction in lines)
            {
                Instruction thisInstruction = new Instruction(instruction);
                instructions.Add(thisInstruction);
            }

            foreach (Instruction instruction in instructions)
            {
                switch (instruction.command)
                {
                    case 'N':
                    case 'E':
                    case 'S':
                    case 'W':
                        waypointLocation = MoveWaypoint(instruction, waypointLocation);
                        break;
                    case 'R':
                    case 'L':
                        waypointLocation = RotateWaypoint(instruction, waypointLocation);
                        break;
                    case 'F':
                        shipLocation = MoveShip(instruction, waypointLocation, shipLocation);
                        break;
                }
            }

            Console.WriteLine("Step 2 - x:{0} + y:{1} = {2}", Math.Abs(shipLocation.x), Math.Abs(shipLocation.y), Math.Abs(shipLocation.x) + Math.Abs(shipLocation.y));
        }
        private static Location MoveWaypoint(Instruction instruction, Location currentLocation)
        {
            switch (instruction.command)
            {
                case 'N':
                    currentLocation.y = currentLocation.y + instruction.magnitude;
                    break;
                case 'S':
                    currentLocation.y = currentLocation.y - instruction.magnitude;
                    break;
                case 'E':
                    currentLocation.x = currentLocation.x + instruction.magnitude;
                    break;
                case 'W':
                    currentLocation.x = currentLocation.x - instruction.magnitude;
                    break;
            }

            return currentLocation;
        }

        private static Location RotateWaypoint(Instruction instruction, Location currentLocation)
        {
            int numTurns = (instruction.magnitude / 90 % Directions.Length);

            for (int i = 0; i < numTurns; i++)
            {
                int xMultiplier = 1;
                int yMultiplier = 1;

                long swap = currentLocation.x;

                switch (instruction.command)
                {
                    case 'L':
                        xMultiplier = -1;                  

                        break;
                    case 'R':
                        yMultiplier = -1;

                        break;
                }

                currentLocation.x = currentLocation.y * xMultiplier;
                currentLocation.y = swap * yMultiplier;
            }

            return currentLocation;
        }

        private static Location MoveShip(Instruction instruction, Location waypointLocation, Location currentLocation)
        {
            int multiplier = instruction.magnitude;

            currentLocation.x = currentLocation.x + (waypointLocation.x * multiplier);
            currentLocation.y = currentLocation.y + (waypointLocation.y * multiplier);

            return currentLocation;
        }
    }
}

/*
 * 
 * --- Day 12: Rain Risk ---
Your ferry made decent progress toward the island, but the storm came in faster than anyone expected. The 
ferry needs to take evasive actions!

Unfortunately, the ship's navigation computer seems to be malfunctioning; rather than giving a route directly 
to safety, it produced extremely circuitous instructions. When the captain uses the PA system to ask if anyone 
can help, you quickly volunteer.

The navigation instructions (your puzzle input) consists of a sequence of single-character actions paired 
with integer input values. After staring at them for a few minutes, you work out what they probably mean:

Action N means to move north by the given value.
Action S means to move south by the given value.
Action E means to move east by the given value.
Action W means to move west by the given value.
Action L means to turn left the given number of degrees.
Action R means to turn right the given number of degrees.
Action F means to move forward by the given value in the direction the ship is currently facing.
The ship starts by facing east. Only the L and R actions change the direction the ship is facing. (That is, 
if the ship is facing east and the next instruction is N10, the ship would move north 10 units, but would 
still move east if the following action were F.)

For example:

F10
N3
F7
R90
F11
These instructions would be handled as follows:

F10 would move the ship 10 units east (because the ship starts by facing east) to east 10, north 0.
N3 would move the ship 3 units north to east 10, north 3.
F7 would move the ship another 7 units east (because the ship is still facing east) to east 17, north 3.
R90 would cause the ship to turn right by 90 degrees and face south; it remains at east 17, north 3.
F11 would move the ship 11 units south to east 17, south 8.
At the end of these instructions, the ship's Manhattan distance (sum of the absolute values of its east/west 
position and its north/south position) from its starting position is 17 + 8 = 25.

Figure out where the navigation instructions lead. What is the Manhattan distance between that location and 
the ship's starting position?

Your puzzle answer was 1032.

--- Part Two ---
Before you can give the destination to the captain, you realize that the actual action meanings were printed 
on the back of the instructions the whole time.

Almost all of the actions indicate how to move a waypoint which is relative to the ship's position:

Action N means to move the waypoint north by the given value.
Action S means to move the waypoint south by the given value.
Action E means to move the waypoint east by the given value.
Action W means to move the waypoint west by the given value.
Action L means to rotate the waypoint around the ship left (counter-clockwise) the given number of degrees.
Action R means to rotate the waypoint around the ship right (clockwise) the given number of degrees.
Action F means to move forward to the waypoint a number of times equal to the given value.
The waypoint starts 10 units east and 1 unit north relative to the ship. The waypoint is relative to the ship; 
that is, if the ship moves, the waypoint moves with it.

For example, using the same instructions as above:

F10 moves the ship to the waypoint 10 times (a total of 100 units east and 10 units north), leaving the ship 
at east 100, north 10. The waypoint stays 10 units east and 1 unit north of the ship.
N3 moves the waypoint 3 units north to 10 units east and 4 units north of the ship. The ship remains at east 
100, north 10.
F7 moves the ship to the waypoint 7 times (a total of 70 units east and 28 units north), leaving the ship at 
east 170, north 38. The waypoint stays 10 units east and 4 units north of the ship.
R90 rotates the waypoint around the ship clockwise 90 degrees, moving it to 4 units east and 10 units south 
of the ship. The ship remains at east 170, north 38.
F11 moves the ship to the waypoint 11 times (a total of 44 units east and 110 units south), leaving the ship 
at east 214, south 72. The waypoint stays 4 units east and 10 units south of the ship.
After these operations, the ship's Manhattan distance from its starting position is 214 + 72 = 286.

Figure out where the navigation instructions actually lead. What is the Manhattan distance between that 
location and the ship's starting position?

Your puzzle answer was 156735.
 *
 */ 