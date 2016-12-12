using System;
using System.IO;
using System.Text;

namespace Maze.Services
{
    public static class Serializer
    {
        public static void PersistToFile(Maze maze, string path)
        {
            try
            {
                File.WriteAllText(path, PersistToString(maze));
            }
            catch(Exception e)
            {
                throw new InvalidOperationException(string.Format("Cannot persist maze to path '{0}'.", path), e);
            }
        }

        public static string PersistToString(Maze maze)
        {
            var sr = new StringBuilder();

            for (int y = 0; y < maze.Height; y++)
            {
                string row = "";

                for (int x = 0; x < maze.Width; x++)
                {
                    var block = maze.Block(x, y);

                    switch (block.Type)
                    {
                        case BlockType.Empty:
                            row += block.Traced ? "." : " ";
                            break;

                        case BlockType.Wall:
                            row += "X";
                            break;

                        case BlockType.Start:
                            row += "S";
                            break;

                        case BlockType.Finish:
                            row += "F";
                            break;
                    }
                }

                sr.AppendLine(row);
            }

            return sr.ToString();
        }
    }
}
