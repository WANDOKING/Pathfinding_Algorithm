using System;
using System.Reflection;

namespace Algorithm
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int BOARD_SIZE = 25;

            Console.CursorVisible = false;

            Board board = new Board(BOARD_SIZE, BOARD_SIZE - 2, BOARD_SIZE - 2);

            Player player = new Player(1, 1, board);
            board.Player = player;
            
            const int WAIT_TICK = 1000 / 30;

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

                // 입력
                // 로직
                player.Update(deltaTick);

                // 렌더링
                Console.SetCursorPosition(0, 0);
                board.Render();
            }
        }
    }
}
