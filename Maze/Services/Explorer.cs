using System.Collections.Generic;
using System.Linq;

namespace Maze.Services
{
    public enum Direction
    {
        North, East, South, West
    }

    public class Explorer
    {
        public Block Current { get; set; }
        public Direction Direction { get; set; }

        private Maze maze;

        public Explorer(Maze maze)
        {
            this.maze = maze;
            maze.ClearTrace();

            Direction = Direction.North;
            Current = maze.StartBlock();
        }

        public BlockType LookAhead()
        {
            switch (Direction)
            {
                case Direction.North:
                    return maze.Block(Current.X, Current.Y - 1).Type;

                case Direction.East:
                    return maze.Block(Current.X + 1, Current.Y).Type;

                case Direction.South:
                    return maze.Block(Current.X, Current.Y + 1).Type;

                default:
                    return maze.Block(Current.X - 1, Current.Y).Type;
            }
        }

        public bool MoveForward()
        {
            if (GetDirections().Contains(Direction))
            {
                var previous = Current;

                switch(Direction)
                {
                    case Direction.North:
                        Current = maze.Block(previous.X, previous.Y - 1);
                        break;

                    case Direction.East:
                        Current = maze.Block(previous.X + 1, previous.Y);
                        break;

                    case Direction.South:
                        Current = maze.Block(previous.X, previous.Y + 1);
                        break;

                    case Direction.West:
                        Current = maze.Block(previous.X - 1, previous.Y);
                        break;
                }

                Current.Origin = previous;
                Current.Traced = true;

                return true;
            }

            return false;
        }

        public void TurnLeft()
        {
            Direction = (Direction) (Direction == 0 ? 3 : (int)Direction - 1);
        }

        public void TurnRight()
        {
            Direction = (Direction) ((int)Direction == 3 ? 0 : (int)Direction + 1);
        }

        public List<Direction> GetDirections()
        {
            var directions = new List<Direction>();

            foreach(var block in Neighbors(maze.Blocks, Current).Where(p => p.Type == BlockType.Empty))
            {
                Direction direction;

                if (block.X != Current.X)
                {
                    direction = (block.X > Current.X) ? Direction.East: Direction.West;
                }
                else
                {
                    direction = block.Y > Current.Y ? Direction.South: Direction.North;
                }

                directions.Add(direction);
            }

            return directions;
        }

        public static IEnumerable<Block> Neighbors(List<Block> blocks, Block from)
        {
            return blocks.Where(p =>
                (p.X == from.X + 1 && p.Y == from.Y) ||
                (p.X == from.X - 1 && p.Y == from.Y) ||
                (p.X == from.X && p.Y == from.Y + 1) ||
                (p.X == from.X && p.Y == from.Y - 1)
            );
        }
    }
}
