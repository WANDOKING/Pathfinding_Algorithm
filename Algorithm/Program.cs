using System;

namespace Algorithm
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Board board = new Board(25);
            
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
                lastTick = currentTick;
                #endregion

                // 입력
                // 로직
                // 렌더링
                Console.SetCursorPosition(0, 0);
                board.Render();
            }
        }
    }
}
