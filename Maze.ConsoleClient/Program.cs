using Maze;
using Maze.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Maze.ConsoleClient
{
    class Program
    {
        // FM.2016.07.20 - the maze exercise. https://github.com/matrello/MazeExercise

        private static Maze maze;

        private const int size = 20;

        static void Main(string[] args)
        {
            maze = Builder.BuildMaze(size, size);

            ShowMaze(true);

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Enter (1) to get a block type given its coordinates");
                Console.WriteLine("Enter (2) to move into the maze");
                Console.WriteLine("Enter (3) to solve the maze");
                Console.WriteLine("Enter (4) to save the maze");
                Console.WriteLine("Enter (5) to show the maze");
                Console.WriteLine("Press Ctrl+c to exit");

                int choice = 0;
                int.TryParse(Console.ReadLine(), out choice);

                Console.WriteLine();

                switch (choice)
                {
                    case 1:
                        GetBlockType();
                        break;

                    case 2:
                        MoveIntoMaze();
                        break;

                    case 3:
                        SolveMaze();
                        break;

                    case 4:
                        SaveMaze();
                        break;

                    case 5:
                        ShowMaze(true);
                        break;
                }

            }
        }

        private static void GetBlockType()
        {
            Console.WriteLine("Enter coordinates (ex. 1, 1):");
            var coordinates = Console.ReadLine();

            int x, y;

            if (int.TryParse(coordinates.Split(',')[0], out x) && int.TryParse(coordinates.Split(',')[1], out y))
            {
                var block = maze.Block(x, y);

                if (block != null)
                    Console.WriteLine("Block type at ({0}, {1}) is {2}.", x, y, block.Type);
                else
                    Console.WriteLine("There is no block at ({0}, {1}).", x, y);
            }
            else
                Console.WriteLine("Coordinates format is not valid.");

            return;
        }

        private static void MoveIntoMaze()
        {
            var explorer = new Explorer(maze);

            while (true)
            {
                Console.WriteLine("You are at ({0}, {1}). Your direction is {2}. You can move {3}.",
                    explorer.Current.X, explorer.Current.Y, explorer.Direction, string.Join(", ", explorer.GetDirections()));

                Console.WriteLine();
                Console.WriteLine("Enter (1) to turn left");
                Console.WriteLine("Enter (2) to turn right");
                Console.WriteLine("Enter (3) to look ahead");
                Console.WriteLine("Enter (4) to move forward");
                Console.WriteLine("Enter (5) to draw your path");
                Console.WriteLine("Enter (6) to go back to main menu");
                Console.WriteLine("Press Ctrl+c to exit");

                int choice = 0;
                int.TryParse(Console.ReadLine(), out choice);

                Console.WriteLine();

                switch (choice)
                {
                    case 1:
                        explorer.TurnLeft();
                        break;

                    case 2:
                        explorer.TurnRight();
                        break;

                    case 3:
                        Console.Write("Ahead of you there is ");

                        switch (explorer.LookAhead())
                        {
                            case BlockType.Empty:
                                Console.WriteLine("an empty block.");
                                break;

                            case BlockType.Wall:
                                Console.WriteLine("a wall.");
                                break;

                            case BlockType.Start:
                                Console.WriteLine("the start.");
                                break;

                            case BlockType.Finish:
                                Console.WriteLine("the finish.");
                                break;
                        }
                        break;

                    case 4:
                        if (!explorer.MoveForward())
                            Console.WriteLine("You cannot move in that direction.");
                        break;

                    case 5:
                        ShowMaze();
                        break;

                    case 6:
                        return;
                }
            }
        }

        private static void SolveMaze()
        {
            Solver.Solve(maze);
            ShowMaze();

            return;
        }

        private static void SaveMaze()
        {
            Console.WriteLine("Enter the full path (ie. c:\\temp\\maze.txt), or null to return to the main menu:");
            var path = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(path))
                return;

            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Console.WriteLine("The specified path does not exist.");
                return;
            }

            try
            {
                Serializer.PersistToFile(maze, path);
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Error: {0}\nDetails to follow: {1}\nStacktrace: {2}", e.Message, e.InnerException.Message, e.StackTrace));
            }
        }

        private static void ShowMaze(bool stats = false)
        {
            Console.Write(Serializer.PersistToString(maze));
            Console.WriteLine();

            if (stats)
                Console.WriteLine("Total blocks: {0}\nNumber of walls: {1}\nNumber of empty spaces: {2}.", maze.Count(), maze.Count(BlockType.Wall), maze.Count(BlockType.Empty));
        }
    }
}
