using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Checker
    {
        int army;
        int[,] posMoves = new int[16, 2]
        { { 1, 0 }, { 1, 1 }, { 0, 1 }, { -1, 1 }, { -1, 0 }, { -1, -1 }, { 0, -1 }, { 1, -1 },
          { -2, -1 }, { -1, -2 }, { 1, -2 }, { 2, -1 }, { 2, 1 }, { 1, 2 }, { -1, 2 }, { -2, 1 }
        };

        int[,] posPawnMoves = new int[4, 2]
        { {-1, -1}, {-1, 1}, {1, -1}, {1, 1} };

        public Checker(int army)
        {
            this.army = army;
        }

        public void Check(int[,] matr, int a, int b, int hor, int ver, int mode)
        {
            int c, d;
            switch (mode)
            {
                case 0:
                    for (int i = 8; i < 16; i++)
                    {
                        c = hor + posMoves[i, 0];
                        d = ver + posMoves[i, 1];
                        if (c >= 0 && c < a && d >= 0 && d < b && matr[c, d] == 1 - army)
                            matr[c, d] = 2;
                    }
                    break;
                case 1:
                    for (int i = 0; i < 8; i += 2)
                    {
                        c = hor + posMoves[i, 0];
                        d = ver + posMoves[i, 1];
                        while (c >= 0 && c < a && d >= 0 && d < b && matr[c, d] < 0)
                        {
                            c += posMoves[i, 0];
                            d += posMoves[i, 1];
                        }
                        if (c >= 0 && c < a && d >= 0 && d < b && matr[c, d] == 1 - army)
                            matr[c, d] = 2;
                    }
                    break;
                case 2:
                    for (int i = 1; i < 8; i += 2)
                    {
                        c = hor + posMoves[i, 0];
                        d = ver + posMoves[i, 1];
                        while (c >= 0 && c < a && d >= 0 && d < b && matr[c, d] < 0)
                        {
                            c += posMoves[i, 0];
                            d += posMoves[i, 1];
                        }
                        if (c >= 0 && c < a && d >= 0 && d < b && matr[c, d] == 1 - army)
                            matr[c, d] = 2;
                    }
                    break;
                case 3:
                    for (int i = army; i < 4; i += 2)
                    {
                        c = hor + posPawnMoves[i, 0];
                        d = ver + posPawnMoves[i, 1];
                        if (c >= 0 && c < a && d >= 0 && d < b && matr[c, d] == 1 - army)
                            matr[c, d] = 2;
                    }
                    break;
            }
        }
    }
}
