using System;
using System.Diagnostics;
using System.Threading;

namespace Algorithm
{
    class Board
    {
        public ETileType[,] Tile { get; set; }
        public int Size { get; private set; }

        public int DestX { get; private set; }
        public int DestY { get; private set; }

        public Player Player { get; set; }

        public enum ETileType
        {
            Empty,
            Wall,
            EmptyVisited
        }

        public Board(int size, int destX, int destY)
        {
            Debug.Assert(size % 2 == 1); // size는 홀수여야 함

            Tile = new ETileType[size, size];
            Size = size;
            DestX = destX;
            DestY = destY;

            GenerateBySideWinder();
            //GenerateEmptySpace();
        }

        public void Render()
        {
            ConsoleColor prevColor = Console.ForegroundColor;

            for (int i = 0; i < Size; ++i)
            {
                for (int j = 0; j < Size; ++j)
                {
                    if (Player != null && j == Player.PosX && i == Player.PosY)
                    {
                        Console.ForegroundColor = PLAYER_COLOR;
                    }
                    else if (j == DestY && i == DestX)
                    {
                        Console.ForegroundColor = DESTINATION_COLOR;
                    }
                    else
                    {
                        Console.ForegroundColor = GetTileColor(Tile[i, j]);
                    }

                    Console.Write(CIRCLE);
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = prevColor;
        }

        private ConsoleColor GetTileColor(ETileType type)
        {
            switch (type)
            {
                case ETileType.Empty:
                    return ConsoleColor.Green;
                case ETileType.Wall:
                    return ConsoleColor.Red;
                case ETileType.EmptyVisited:
                    return ConsoleColor.DarkMagenta;
                default:
                    Debug.Assert(false);
                    return ConsoleColor.Black;
            }
        }

        // Binary Tree 알고리듬 기반 미로 생성
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

                    // 최외곽을 뚫고 나가지 않도록
                    if (i == Size - 2 && j == Size - 2)
                    {
                        continue;
                    }

                    if (i == Size - 2)
                    {
                        Tile[i, j + 1] = ETileType.Empty;
                        continue;
                    }
                    
                    if (j == Size - 2)
                    {
                        Tile[i + 1, j] = ETileType.Empty;
                        continue;
                    }

                    // 2분의 1 확률로 아래나 오른쪽을 뚫는다
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

        // SideWinder 알고리듬 기반 미로 생성
        private void GenerateBySideWinder()
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
                int count = 1; // 지금까지 몇 번의 정점을 거쳐갔는지
                for (int j = 0; j < Size; ++j)
                {
                    if (i % 2 == 0 || j % 2 == 0)
                    {
                        continue;
                    }

                    // 최외곽을 뚫고 나가지 않도록
                    if (i == Size - 2 && j == Size - 2)
                    {
                        continue;
                    }

                    if (i == Size - 2)
                    {
                        Tile[i, j + 1] = ETileType.Empty;
                        continue;
                    }

                    if (j == Size - 2)
                    {
                        Tile[i + 1, j] = ETileType.Empty;
                        continue;
                    }

                    //// 디버깅용
                    //{
                    //    Thread.Sleep(500);
                    //    Console.SetCursorPosition(0, 0);
                    //    Render();
                    //}

                    // 2분의 1 확률로 오른쪽으로 가거나, 지금까지 거쳐온 정점 중 한 곳의 아래를 뚫음
                    if (rand.Next(0, 2) == 0)
                    {
                        Tile[i, j + 1] = ETileType.Empty;
                        count++;
                    }
                    else
                    {
                        int randomIndex = rand.Next(0, count);
                        Tile[i + 1, j - randomIndex  * 2] = ETileType.Empty;
                        count = 1;
                    }
                }
            }
        }

        private void GenerateEmptySpace()
        {
            for (int i = 0; i < Size; ++i)
            {
                for (int j = 0; j < Size; ++j)
                {
                    if (i == 0 || j == 0 || i == Size - 1 || j == Size - 1)
                    {
                        Tile[i, j] = ETileType.Wall;
                    }
                    else
                    {
                        Tile[i, j] = ETileType.Empty;
                    }
                }
            }
        }

        private const char CIRCLE = '\u25cf';
        private const ConsoleColor PLAYER_COLOR = ConsoleColor.Blue;
        private const ConsoleColor DESTINATION_COLOR = ConsoleColor.DarkYellow;
    }
}