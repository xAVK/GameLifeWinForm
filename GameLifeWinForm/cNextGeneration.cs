using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLifeWinForm
{
    class cNextGeneration
    {
        private cCurrentGeneration NewGeneration = new cCurrentGeneration();
        cCurrentGeneration CurrentGeneration = new cCurrentGeneration();
        public cNextGeneration(cCurrentGeneration Current)
        {
            CurrentGeneration = Current;
            NewGeneration.Clear();
        }
        public void Next()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    int CountLife = CheckNeighbor(i, j);
                    if (CurrentGeneration.Get(i, j).IsAlive && (CountLife > 3 || CountLife < 2))
                    {
                        NewGeneration.Set(i, j, false);
                    }
                    else if (!CurrentGeneration.Get(i, j).IsAlive && CountLife == 3)
                    {
                        NewGeneration.Set(i, j, true);
                    }
                    else
                    {
                        NewGeneration.Set(i, j, CurrentGeneration.Get(i, j).IsAlive);
                    }
                }
            }
        }
        public cCurrentGeneration Get(int Time)
        {            
            System.Threading.Thread.Sleep(Time);
            return NewGeneration;            
        }
        public bool CompareGeneration()
        {
            int Count = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (NewGeneration.Get(i, j).IsAlive != CurrentGeneration.Get(i, j).IsAlive)
                    {
                        Count++;
                    }
                }
            }
            return Count == 0;
        }
        private int CheckNeighbor(int X, int Y)
        {
            int CountLife = 0;
            Compare(X, Y + 1, CountLife,  out CountLife);
            Compare(X, Y - 1, CountLife,  out CountLife);
            Compare(X + 1, Y, CountLife,  out CountLife);
            Compare(X + 1, Y - 1, CountLife, out CountLife);
            Compare(X + 1, Y + 1, CountLife, out CountLife);
            Compare(X - 1, Y, CountLife, out CountLife);
            Compare(X - 1, Y - 1, CountLife, out CountLife);
            Compare(X - 1, Y + 1, CountLife, out CountLife);

            return CountLife;
        }
        private void Compare(int X, int Y, int CountLife, out int Life)
        {
            if (CurrentGeneration.Get(X, Y) != null && CurrentGeneration.Get(X, Y).IsAlive)
            {
                CountLife++;
            }
            Life = CountLife;
        }
    }
}
