using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Chess
{
    [Serializable]

    class Pawn : Piece
    {
        int[,] posMoves = new int[4, 2]
            { {0, -1}, {0, 1}, {0, -2}, {0, 2} };
        int[,] posEats = new int[4, 2]
        { {-1, -1}, {-1, 1}, {1, -1}, {1, 1} };

        public static event SpecMover PassantEvent;

        public Pawn(int army)
        {
            this.army = army;
            pic = new Bitmap($@"..\..\assets\pieces\{army}\pawn.png");
        }

        public override void Move(int[,] matr, int a, int b)
        {
            int c, d, e, f;
            c = hor + posMoves[army, 0];
            d = ver + posMoves[army, 1];
            if (c >= 0 && c < a && d >= 0 && d < b && matr[c, d] < 0)
            {
                matr[c, d] = 2;
                if (CheckProm(d))
                    matr[c, d] = 5;
                if (moves == 0)
                {
                    c += posMoves[army, 0];
                    d += posMoves[army, 1];
                    if (c >= 0 && c < a && d >= 0 && d < b && matr[c, d] < 0)
                        matr[c, d] = 2;
                }
            }
            for (int i = army; i < 4; i += 2)
            {
                e = hor + posEats[i, 0];
                f = ver + posEats[i, 1];
                if (e >= 0 && e < a && f >= 0 && f < b && matr[e, f] == 1 - army)
                {
                    matr[e, f] = 2;
                    if (CheckProm(f))
                        matr[e, f] = 5;
                }
            }
            c = hor + 1;
            e = hor - 1;
            if (((army == 0 && ver == 3) || (army == 1 && ver == 4)) && ((c >= 0 && c < a && matr[c, ver] == 1 - army) || (e >= 0 && e < b && matr[e, ver] == 1 - army)))
            {
                SpecMoveEventArgs ev = new SpecMoveEventArgs
                {
                    hor = -1,
                    ver = ver,
                    army = 1 - army,
                };
                if (c >= 0 && c < b && matr[c, ver] == 1 - army)
                {
                    ev.hor = c;
                    PassantEvent(this, ev);
                    if (ev.res)
                        matr[c, ver + posEats[army, 1]] = 4;
                }
                ev.res = false;
                if (e >= 0 && e < b && matr[e, ver] == 1 - army)
                {
                    ev.hor = e;
                    PassantEvent(this, ev);
                    if (ev.res)
                        matr[e, ver + posEats[army, 1]] = 4;
                }
            }
        }

        bool CheckProm(int ver)
        {
            if ((ver == 0 && army == 0) || (ver == 7 && army == 1))
                return true;
            return false;
        }
    }
}
