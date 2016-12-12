using System.Collections.Generic;
using System.Linq;

namespace Maze.Services
{
    public static class Solver
    {
        public static void Solve(Maze maze)
        {
            maze.ClearTrace();

            var stack = new Stack<Block>();

            var current = maze.Blocks.First(p => p.Type == BlockType.Start);
            stack.Push(current);

            // FM.2016.07.16 - DFS approach similar to Builder.BuildMaze()
            while (stack.Count > 0)
            {
                var near = Explorer.Neighbors(maze.Blocks, current).Where(p => (p.Type == BlockType.Empty || p.Type == BlockType.Finish) && p.Origin == null).ToList();

                if (near.Count > 0)
                {
                    if (near.Count > 1)
                        stack.Push(current);

                    var next = near.First();
                    next.Origin = current;
                    current = next;

                    // TODO: FM.2016.07.16 - could be optimized by exiting if the reached block is the finish
                }
                else
                {
                    current = stack.Pop();
                }
            }

            // FM.2016.07.16 - trace the solution from the finish going backward
            current = maze.Blocks.First(p => p.Type == BlockType.Finish);
            while (current != null)
            {
                current.Traced = true;
                current = current.Origin;
            }
        }

    }
}
