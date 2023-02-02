using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

    class Player
    {
        public int PosX { get; private set; }
        public int PosY { get; private set; }

        public Player(int posX, int posY, Board board)
        {
            mBoard = board;
            PosX = posX;
            PosY = posY;

            getRouteBFS();
            //getRouteRightHandRule();
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

            Queue<Position> q = new Queue<Position>();
            q.Enqueue(new Position(PosX, PosY));
            visited[PosY, PosX].X = -1;
            visited[PosY, PosX].Y = -1;

            while (q.Count > 0)
            {
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
                }

                if (mBoard.Tile[y, x + 1] == Board.ETileType.Empty && visited[y, x + 1].X == 0 && visited[y, x + 1].Y == 0)
                {
                    Position next = new Position(x + 1, y);
                    visited[y, x + 1] = q.Peek();
                    q.Enqueue(next);
                }

                if (mBoard.Tile[y - 1, x] == Board.ETileType.Empty && visited[y - 1, x].X == 0 && visited[y - 1, x].Y == 0)
                {
                    Position next = new Position(x, y - 1);
                    visited[y - 1, x] = q.Peek();
                    q.Enqueue(next);
                }

                if (mBoard.Tile[y + 1, x] == Board.ETileType.Empty && visited[y + 1, x].X == 0 && visited[y + 1, x].Y == 0)
                {
                    Position next = new Position(x, y + 1);
                    visited[y + 1, x] = q.Peek();
                    q.Enqueue(next);
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
