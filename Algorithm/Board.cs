using System;
using System.Diagnostics;

namespace Algorithm
{
    class Board
    {
        const char CIRCLE = '\u25cf';
        public ETileType[,] Tile;
        public int Size;

        public enum ETileType
        {
            Empty,
            Wall
        }

        public Board(int size)
        {
            Debug.Assert(size % 2 == 1); // size는 홀수여야 함

            Tile = new ETileType[size, size];
            Size = size;

            GenerateByBinaryTree();
        }

        public void Render()
        {
            ConsoleColor prevColor = Console.ForegroundColor;

            for (int i = 0; i < Size; ++i)
            {
                for (int j = 0; j < Size; ++j)
                {
                    Console.ForegroundColor = GetTileColor(Tile[i, j]);
                    Console.Write(CIRCLE);
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = prevColor;
        }

        private void GenerateByBinaryTree()
        {
            for (int i = 0; i < Size; ++i)
            {
                for (int j = 0; j < Size; ++j)
                {
                    if (i % 2 == 0 || j % 2 == 0)
                    {
                        Tile[i, j] = ETileType.Wall;
                    }
                    else
                    {
                        Tile[i, j] = ETileType.Empty;
                    }
                }
            }

            Random rand = new Random();

            for (int i = 0; i < Size; ++i)
            {
                for (int j = 0; j < Size; ++j)
                {
                    if (i % 2 == 0 || j % 2 == 0)
                    {
                        continue;
                    }

                    if (i == Size - 2 && j == Size - 2)
                    {
                        continue;
                    }

                    if (i == Size - 2)
                    {
                        Tile[i, j + 1] = ETileType.Empty;
                    }
                    else if (j == Size - 2)
                    {
                        Tile[i + 1, j] = ETileType.Empty;
                    }
                    if (rand.Next(0, 2) == 0)
                    {
                        Tile[i, j + 1] = ETileType.Empty;
                    }
                    else
                    {
                        Tile[i + 1, j] = ETileType.Empty;
                    }
                }
            }
        }

        private ConsoleColor GetTileColor(ETileType type)
        {
            switch (type)
            {
                case ETileType.Empty:
                    return ConsoleColor.Green;
                case ETileType.Wall:
                    return ConsoleColor.Red;
                default:
                    Debug.Assert(false);
                    return ConsoleColor.Black;
            }
        }
    }
}