using System.Collections.Generic;
using System.Linq;

namespace Maze
{
    // FM.2016.07.20 - the maze exercise. https://github.com/matrello/MazeExercise

    public enum BlockType
    {
        Wall, Empty, Start, Finish
    }

    public class Block
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public BlockType Type { get; set; }

        public int Distance { get; set; }   // FM.2016.07.16 - stack count about when the block was defined as empty, used to determine the finish

        public bool Traced { get; set; }    // FM.2016.07.16 - used to trace the solution and the user movements
        public Block Origin { get; set; }   // FM.2016.07.16 - points to the previous block of the chain

        public Block(int x, int y, BlockType type)
        {
            X = x;
            Y = y;
            Type = type;
        }
    }
}
