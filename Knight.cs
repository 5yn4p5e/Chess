using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Chess
{
    [Serializable]

    class Knight : Piece
    {
        int[,] posMoves = new int[8, 2]
        { {-2, -1}, {-1, -2}, {1, -2}, {2, -1}, {2, 1}, {1, 2}, {-1, 2}, {-2, 1} };

        public override void Move(int[,] matr, int a, int b)
        {
            for (int i = 0; i < 8; i++)
            {
                int c = hor + posMoves[i, 0], d = ver + posMoves[i, 1];
                if (c >= 0 && c < a && d >= 0 && d < b && (matr[c, d] < 0 || matr[c, d] == 1 - army))
                    matr[c, d] = 2;
            }
        }

        public Knight(int army)
        {
            this.army = army;
            pic = new Bitmap($@"..\..\assets\pieces\{army}\knight.png");
        }
    }
}