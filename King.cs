using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Chess
{

    public class SpecMoveEventArgs : EventArgs
    {
        public int hor, ver, army;
        public bool res = false;

        public SpecMoveEventArgs() : base() { }
    }

    public delegate void SpecMover(object sender, SpecMoveEventArgs ev);

    [Serializable]

    class King : Piece
    {
        int[,] posMoves = new int[8, 2]
        { { 1, 0 }, { 1, 1 }, { 0, 1 }, { -1, 1 }, { -1, 0 }, { -1, -1 }, { 0, -1 }, { 1, -1 } };
        int[] posCastling = new int[2]
        { -2, 2 };

        public static event SpecMover CastlingEvent;

        public King(int army)
        {
            this.army = army;
            pic = new Bitmap($@"..\..\assets\pieces\{army}\king.png");
        }

        public override void Move(int[,] matr, int a, int b)
        {
            int c, d;
            for (int i = 0; i < 8; i++)
            {
                c = hor + posMoves[i, 0];
                d = ver + posMoves[i, 1];
                if (c >= 0 && c < a && d >= 0 && d < b && (matr[c, d] < 0 || matr[c, d] == 1 - army))
                    matr[c, d] = 2;
            }
            if (moves == 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    c = hor + posCastling[i];
                    d = ver;
                    if (c >= 0 && c < a && d >= 0 && d < b && matr[c, d] < 0)
                    {
                        int e = c + posMoves[i * 4, 0];
                        int f = ver;
                        if (matr[e, f] == 2 && (i != 0 || matr[c - 1, d] < 0) && CastlingEvent != null)
                        {
                            SpecMoveEventArgs ev = new SpecMoveEventArgs
                            {
                                hor = i * 7,
                                ver = ver,
                                army = army,
                            };
                            CastlingEvent(this, ev);
                            if (ev.res)
                                matr[c, d] = 3;
                        }
                    }
                }
            }
        }
    }
}
