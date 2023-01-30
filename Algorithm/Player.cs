using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    class Player
    {
        public int PosX { get; private set; }
        public int PosY { get; private set; }
        public Player(int posX, int posY, int destX, int destY, Board board)
        {
            mBoard = board;
            PosX = posX;
            PosY = posY;
        }

        public void Update(int deltaTick)
        {
            mSumTick += deltaTick;

            if (mSumTick >= MOVE_TICK)
            {
                mSumTick = 0;

                int randValue = mRand.Next(0, 4);

                switch(randValue)
                {
                    case 0:
                        if (mBoard.Tile[PosY - 1, PosX] == Board.ETileType.Empty)
                        {
                            PosY = PosY - 1;
                        }
                        break;
                    case 1:
                        if (mBoard.Tile[PosY + 1, PosX] == Board.ETileType.Empty)
                        {
                            PosY = PosY + 1;
                        }
                        break;
                    case 2:
                        if (mBoard.Tile[PosY, PosX - 1] == Board.ETileType.Empty)
                        {
                            PosX = PosX - 1;
                        }
                        break;
                    case 3:
                        if (mBoard.Tile[PosY, PosX + 1] == Board.ETileType.Empty)
                        {
                            PosX = PosX + 1;
                        }
                        break;
                    default:
                        Debug.Fail("잘못된 좌표에 대한 값");
                        break;
                }
            }
        }

        private const int MOVE_TICK = 100;
        private Random mRand = new Random();
        private int mSumTick = 0;
        private Board mBoard;
    }
}
