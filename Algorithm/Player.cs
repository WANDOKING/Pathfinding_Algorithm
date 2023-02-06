using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Algorithm
{
    struct Position
    {
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X;
        public int Y;
    }

    struct PqNode : IComparable<PqNode>
    {
        public int X;
        public int Y;
        public int Fn;
        public int Gn;
        public int Hn;

        public PqNode(int x, int y, int gn, int hn)
        {
            this.X = x;
            this.Y = y;
            this.Fn = gn + hn;
            this.Gn = gn;
            this.Hn = hn;
        }

        public int CompareTo(PqNode obj)
        {
            if (this.Fn == obj.Fn)
            {
                return 0;
            }
            else if (this.Fn < obj.Gn)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }

    class Player
    {
        public int PosX { get; private set; }
        public int PosY { get; private set; }

        public Player(int posX, int posY, Board board)
        {
            mBoard = board;
            PosX = posX;
            PosY = posY;

            //getRouteBFS();
            //getRouteRightHandRule();
            getRouteAStar();
        }

        public void Update(int deltaTick)
        {
            if (mLastIndex >= mPositions.Count)
            {
                return;
            }

            mSumTick += deltaTick;

            if (mSumTick >= MOVE_TICK)
            {
                mSumTick = 0;

                PosX = mPositions[mLastIndex].X;
                PosY = mPositions[mLastIndex].Y;
                mLastIndex++;
            }
        }

        private void getRouteRightHandRule()
        {
            int direction = (int)EDirection.Up;

            // 계산을 위한 코드
            int[] mFrontX = new int[] { 0, -1, 0, 1 };
            int[] mFrontY = new int[] { -1, 0, 1, 0 };
            int[] mRightX = new int[] { 1, 0, -1, 0 };
            int[] mRightY = new int[] { 0, -1, 0, 1 };

            mPositions.Add(new Position(PosX, PosY));

            while (PosX != mBoard.DestX || PosY != mBoard.DestY)
            {
                // 1. 현재 바라보는 방향을 기준으로 오른쪽으로 갈 수 있는지 확인
                if (mBoard.Tile[PosY + mRightY[direction], PosX + mRightX[direction]] == Board.ETileType.Empty)
                {
                    // 1-a. 오른쪽 방향으로 90도 회전
                    direction = (direction - 1 + 4) % 4;

                    // 2-b. 앞으로 한 보 전진
                    PosX += mFrontX[direction];
                    PosY += mFrontY[direction];

                    mPositions.Add(new Position(PosX, PosY));
                }
                // 2. 현재 바라보는 방향을 기준으로 전진할 수 있는지 확인
                else if (mBoard.Tile[PosY + mFrontY[direction], PosX + mFrontX[direction]] == Board.ETileType.Empty)
                {
                    // 앞으로 한 보 전진
                    PosX += mFrontX[direction];
                    PosY += mFrontY[direction];

                    mPositions.Add(new Position(PosX, PosY));
                }
                // 3. 모두 불가능
                else
                {
                    // 왼쪽 방향으로 90도 회전
                    direction = (direction + 1) % 4;
                }
            }
        }

        private void getRouteBFS()
        {
            Position[,] visited = new Position[mBoard.Size, mBoard.Size];

            // init
            Queue<Position> q = new Queue<Position>();
            q.Enqueue(new Position(PosX, PosY));
            visited[PosY, PosX].X = -1;
            visited[PosY, PosX].Y = -1;
            mBoard.Tile[PosY, PosX] = Board.ETileType.EmptyVisited;

            // BFS
            while (q.Count > 0)
            {
                Thread.Sleep(20);
                Console.SetCursorPosition(0, 0);
                mBoard.Render();

                int x = q.Peek().X;
                int y = q.Peek().Y;

                if (x == mBoard.DestX && y == mBoard.DestY)
                {
                    break;
                }

                if (mBoard.Tile[y, x - 1] == Board.ETileType.Empty && visited[y, x - 1].X == 0 && visited[y, x - 1].Y == 0)
                {
                    Position next = new Position(x - 1, y);
                    visited[y, x - 1] = q.Peek();
                    q.Enqueue(next);
                    mBoard.Tile[y, x - 1] = Board.ETileType.EmptyVisited;
                }

                if (mBoard.Tile[y, x + 1] == Board.ETileType.Empty && visited[y, x + 1].X == 0 && visited[y, x + 1].Y == 0)
                {
                    Position next = new Position(x + 1, y);
                    visited[y, x + 1] = q.Peek();
                    q.Enqueue(next);
                    mBoard.Tile[y, x + 1] = Board.ETileType.EmptyVisited;
                }

                if (mBoard.Tile[y - 1, x] == Board.ETileType.Empty && visited[y - 1, x].X == 0 && visited[y - 1, x].Y == 0)
                {
                    Position next = new Position(x, y - 1);
                    visited[y - 1, x] = q.Peek();
                    q.Enqueue(next);
                    mBoard.Tile[y - 1, x] = Board.ETileType.EmptyVisited;
                }

                if (mBoard.Tile[y + 1, x] == Board.ETileType.Empty && visited[y + 1, x].X == 0 && visited[y + 1, x].Y == 0)
                {
                    Position next = new Position(x, y + 1);
                    visited[y + 1, x] = q.Peek();
                    q.Enqueue(next);
                    mBoard.Tile[y + 1, x] = Board.ETileType.EmptyVisited;
                }

                q.Dequeue();
            }

            int routeX = mBoard.DestX;
            int routeY = mBoard.DestY;
            while ((routeX == -1 && routeY == -1) == false)
            {
                mPositions.Add(new Position(routeX, routeY));
                int tempX = routeX;
                int tempY = routeY;

                routeX = visited[tempY, tempX].X;
                routeY = visited[tempY, tempX].Y;
            }

            mPositions.Reverse();
        }

        private void getRouteAStar()
        {
            int[,] fns = new int[mBoard.Size, mBoard.Size];
            bool[,] visited = new bool[mBoard.Size, mBoard.Size];
            Position[,] parents = new Position[mBoard.Size, mBoard.Size];
            PriorityQueue<PqNode> open = new PriorityQueue<PqNode>();

            for (int i = 0; i < mBoard.Size; ++i)
            {
                for (int j = 0; j < mBoard.Size; ++j)
                {
                    fns[i, j] = Int32.MaxValue;
                    visited[i, j] = false;
                }
            }

            fns[1, 1] = 0 + Math.Abs(1 - mBoard.DestX) + Math.Abs(1 - mBoard.DestY);
            visited[1, 1] = true;
            open.Add(new PqNode(1, 1, 0, Math.Abs(1 - mBoard.DestX) + Math.Abs(1 - mBoard.DestY)));

            while (open.Count > 0)
            {
                Thread.Sleep(20);
                Console.SetCursorPosition(0, 0);
                mBoard.Render();

                PqNode visit = open.Pop();
                visited[visit.Y, visit.X] = true;
                mBoard.Tile[visit.Y, visit.X] = Board.ETileType.EmptyVisited;

                if (visit.X == mBoard.DestX && visit.Y == mBoard.DestY)
                {
                    break;
                }

                if (mBoard.Tile[visit.Y - 1, visit.X] == Board.ETileType.Empty && visited[visit.Y - 1, visit.X] == false)
                {
                    int prevFn = fns[visit.Y - 1, visit.X];
                    int newGn = visit.Gn + 1;
                    int newHn = Math.Abs(visit.Y - 1 - mBoard.DestY) + Math.Abs(visit.X - mBoard.DestX);
                    int newFn = newGn + newHn;

                    if (newFn < prevFn)
                    {
                        fns[visit.Y - 1, visit.X] = newFn;
                        parents[visit.Y - 1, visit.X].X = visit.X;
                        parents[visit.Y - 1, visit.X].Y = visit.Y;
                        open.Add(new PqNode(visit.X, visit.Y - 1, newGn, newHn));
                    }
                }

                if (mBoard.Tile[visit.Y + 1, visit.X] == Board.ETileType.Empty && visited[visit.Y + 1, visit.X] == false)
                {
                    int prevFn = fns[visit.Y + 1, visit.X];
                    int newGn = visit.Gn + 1;
                    int newHn = Math.Abs(visit.Y + 1 - mBoard.DestY) + Math.Abs(visit.X - mBoard.DestX);
                    int newFn = newGn + newHn;

                    if (newFn < prevFn)
                    {
                        fns[visit.Y + 1, visit.X] = newFn;
                        parents[visit.Y + 1, visit.X].X = visit.X;
                        parents[visit.Y + 1, visit.X].Y = visit.Y;
                        open.Add(new PqNode(visit.X, visit.Y + 1, newGn, newHn));
                    }
                }

                if (mBoard.Tile[visit.Y, visit.X - 1] == Board.ETileType.Empty && visited[visit.Y, visit.X - 1] == false)
                {
                    int prevFn = fns[visit.Y, visit.X - 1];
                    int newGn = visit.Gn + 1;
                    int newHn = Math.Abs(visit.Y - mBoard.DestY) + Math.Abs(visit.X - 1 - mBoard.DestX);
                    int newFn = newGn + newHn;

                    if (newFn < prevFn)
                    {
                        fns[visit.Y, visit.X - 1] = newFn;
                        parents[visit.Y, visit.X - 1].X = visit.X;
                        parents[visit.Y, visit.X - 1].Y = visit.Y;
                        open.Add(new PqNode(visit.X - 1, visit.Y, newGn, newHn));
                    }
                }

                if (mBoard.Tile[visit.Y, visit.X + 1] == Board.ETileType.Empty && visited[visit.Y, visit.X + 1] == false)
                {
                    int prevFn = fns[visit.Y, visit.X + 1];
                    int newGn = visit.Gn + 1;
                    int newHn = Math.Abs(visit.Y - mBoard.DestY) + Math.Abs(visit.X + 1 - mBoard.DestX);
                    int newFn = newGn + newHn;

                    if (newFn < prevFn)
                    {
                        fns[visit.Y, visit.X + 1] = newFn;
                        parents[visit.Y, visit.X + 1].X = visit.X;
                        parents[visit.Y, visit.X + 1].Y = visit.Y;
                        open.Add(new PqNode(visit.X + 1, visit.Y, newGn, newHn));
                    }
                }
            }

            int routeX = mBoard.DestX;
            int routeY = mBoard.DestY;
            while ((routeX == 0 && routeY == 0) == false)
            {
                mPositions.Add(new Position(routeX, routeY));
                int tempX = routeX;
                int tempY = routeY;

                routeX = parents[tempY, tempX].X;
                routeY = parents[tempY, tempX].Y;
            }

            mPositions.Reverse();
        }

        private enum EDirection
        {
            Up = 0,
            Left = 1,
            Down = 2,
            Right = 3
        }
        
        private Board mBoard;

        private const int MOVE_TICK = 50;
        private int mSumTick = 0;

        private List<Position> mPositions = new List<Position>(); // 이동 경로
        private int mLastIndex = 0;
    }
}
