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
                int xMultiplier = 0;
                int yMultiplier = 0;

                long swap = currentLocation.x;

                switch (instruction.command)
                {
                    case 'L':
                        xMultiplier = -1;
                        yMultiplier = 1;                        

                        break;
                    case 'R':
                        xMultiplier = 1;
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
