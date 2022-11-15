using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Chess
{
    [Serializable]

    class Bishop : Piece
    {
        int[,] posMoves = new int[4, 2]
        { { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } };

        public Bishop(int army)
        {
            this.army = army;
            pic = new Bitmap($@"..\..\assets\pieces\{army}\bishop.png");
        }

        public override void Move(int[,] matr, int a, int b)
        {
            for (int i = 0; i < 4; i++)
            {
                int c = hor + posMoves[i, 0], d = ver + posMoves[i, 1];
                while (c >= 0 && c < a && d >= 0 && d < b && matr[c, d] < 0)
                {
                    matr[c, d] = 2;
                    c += posMoves[i, 0];
                    d += posMoves[i, 1];
                }
                if (c >= 0 && c < a && d >= 0 && d < b && matr[c, d] == 1 - army)
                    matr[c, d] = 2;
            }
        }
    }
}