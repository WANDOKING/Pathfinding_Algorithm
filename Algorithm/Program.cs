using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Algorithm
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            const int BOARD_SIZE = 25;
            const int WAIT_TICK = 1000 / 30;

            Board board = new Board(BOARD_SIZE, BOARD_SIZE - 2, BOARD_SIZE - 2);
            Player player = new Player(1, 1, board);
            board.Player = player;

            int lastTick = 0;
            while (true)
            {
                #region 프레임 관리
                int currentTick = System.Environment.TickCount;
                if (currentTick - lastTick < WAIT_TICK)
                {
                    continue;
                }
                int deltaTick = currentTick - lastTick;
                lastTick = currentTick;
                #endregion

                // 로직
                player.Update(deltaTick);

                // 렌더링
                Console.SetCursorPosition(0, 0);
                board.Render();
            }
        }
    }
}
