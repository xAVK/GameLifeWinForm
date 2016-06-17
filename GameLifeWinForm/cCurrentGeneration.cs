using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLifeWinForm
{
    [Serializable]
    public class cCurrentGeneration
    {
        private cCell[,] Generation = new cCell[10, 10];
        public cCurrentGeneration()
        {

        }
        public void Set(cCurrentGeneration Array)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Generation[j, i] = Array.Get(j, i);
                }
            }
        }
        public void Set(int X, int Y, bool IsAlive)
        {
            Generation[X, Y].IsAlive = IsAlive;
        }
        public cCell Get(int X, int Y)
        {
            if ((X >= 10 || X < 0) || (Y >= 10 || Y < 0))
            {
                return null;
            }
            return Generation[X, Y];
        }
        public void Clear()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    bool State = false;
                    Generation[j, i] = new cCell(State);
                }
            }
        }
        public void RND()
        {
            Random rnd = new Random();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {                    
                    int iState = rnd.Next() % 2;
                    bool bState = (iState == 1) ? true : false;
                    Generation[j, i] = new cCell(bState);
                    System.Threading.Thread.Sleep(1);
                }
            }
        }
    }
}
