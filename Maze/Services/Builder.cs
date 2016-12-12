using System;
using System.Collections.Generic;
using System.Linq;

namespace Maze.Services
{
    public static class Builder
    {
        public static int MaxHeight { get { return 80; } }
        public static int MaxWidth { get { return 80; } }

        public static Maze BuildMaze(int width, int height)
        {
            List<Block> blocks = new List<Block>();

            if (!(width > 0 && height <= MaxWidth))
                throw new ArgumentException("Parameter out of bounds", "width");

            if (!(height > 0 && height <= MaxHeight))
                throw new ArgumentException("Parameter out of bounds", "height");

            Random rnd = new Random();
            var stack = new Stack<Block>();

            // FM.2016.07.16 - initialization: all blocks are defined as wall
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    blocks.Add(new Block(x, y, BlockType.Wall));

            // FM.2016.07.16 - start point is random
            int startX = rnd.Next(0, width);
            int startY = rnd.Next(0, height);

            var current = blocks.First(p => p.X == startX && p.Y == startY);
            current.Type = BlockType.Start;
            stack.Push(current);

            // FM.2016.07.16 - DFS approach as described at:
            //                 https://channel9.msdn.com/coding4fun/blog/Getting-lost-and-found-with-the-C-Maze-Generator-and-Solver
            while (stack.Count > 0)
            {
                var near = Explorer.Neighbors(blocks, current).Where(p => p.Type == BlockType.Wall && Explorer.Neighbors(blocks, p).Where(x => x.Type == BlockType.Wall).Count() == 3).ToList();

                if (near.Count > 0)
                {
                    stack.Push(current);
                    current = near.ElementAt(rnd.Next(0, near.Count));  // FM.2016.07.16 - this gives the casuality to the maze 
                    current.Type = BlockType.Empty;
                    current.Distance = stack.Count();                   // FM.2016.07.16 - about the concept of distance and finish: http://xnafan.net/2012/03/maze-creation-in-c/
                }
                else
                {
                    current = stack.Pop();
                }
            }

            // FM.2016.07.16 - the finish block is the farest one
            blocks.OrderByDescending(x => x.Distance).First().Type = BlockType.Finish;

            return new Maze(blocks);
        }
    }
}
