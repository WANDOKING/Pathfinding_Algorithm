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

            int direction = (int)EDirection.Up;

            // 계산을 위한 코드
            int[] mFrontX = new int[] { 0, -1, 0, 1 };
            int[] mFrontY = new int[] { -1, 0, 1, 0 };
            int[] mRightX = new int[] { 1, 0, -1, 0 };
            int[] mRightY = new int[] { 0, -1, 0, 1 };

            mPositions.Add(new Position(PosX, PosY));

            while (PosX != board.DestX || PosY != board.DestY)
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
