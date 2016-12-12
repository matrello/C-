using System.Collections.Generic;
using System.Linq;

namespace Maze
{
    public class Maze
    {
        // FM.2016.07.20 - the maze exercise. https://github.com/matrello/MazeExercise

        public int Width { get { return Blocks.Max(p => p.X) + 1; } }
        public int Height { get { return Blocks.Max(p => p.Y) + 1; } }

        public List<Block> Blocks { get; private set; }

        public Maze(List<Block> blocks)
        {
            this.Blocks = blocks == null ? new List<Block>() : blocks;
        }

        public void ClearTrace()
        {
            Blocks.ForEach(p => { p.Traced = false; p.Origin = null; });
        }

        public Block Block(int x, int y)
        {
            return Blocks.FirstOrDefault(p => p.X == x && p.Y == y);
        }

        public Block StartBlock()
        {
            return Blocks.FirstOrDefault(p => p.Type == BlockType.Start);
        }

        public int Count()
        {
            return Blocks.Count();
        }

        public int Count(BlockType type)
        {
            return Blocks.Count(p => p.Type == type);
        }
    }
}
