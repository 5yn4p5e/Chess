using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;

namespace Chess
{
    [Serializable]

    class Cell
    {
        protected int x = 0, y = 0, hor = 0, ver = 0;
        protected bool empty = true;
        protected Color color;
        public const int side = 75;
        protected Piece pi;

        public Cell(byte r, byte g, byte b)
        {
            this.color = Color.FromArgb(r, g, b);
            pi = null;
            InstallSubscriptions();
        }

        public Cell(byte r, byte g, byte b, Piece piece)
        {
            this.color = Color.FromArgb(r, g, b);
            this.pi = piece;
            empty = false;
            InstallSubscriptions();
        }

        public void InstallSubscriptions()
        {
            Board.UserClickEvent += CheckMoves;
            Board.GetPieceEvent += GetPiece;
            Board.AddPieceEvent += AddPiece;
            Board.EndMoveEvent += EndMove;
            Pawn.PassantEvent += CheckPassant;
            King.CastlingEvent += CheckCastle;
        }

        public void DeleteSubscriptions()
        {
            Board.UserClickEvent -= CheckMoves;
            Board.GetPieceEvent -= GetPiece;
            Board.AddPieceEvent -= AddPiece;
            Board.EndMoveEvent -= EndMove;
            Pawn.PassantEvent -= CheckPassant;
            King.CastlingEvent -= CheckCastle;
        }

        public void Draw(Graphics g)
        {
            Brush br = new SolidBrush(color);
            g.FillRectangle(br, x, y, side, side);
            br.Dispose();
            if (!empty)
                pi.Draw(g, hor, ver, x, y);
        }

        public void DrawR(Graphics g, Brush br)
        {
            g.FillRectangle(br, x, y, side, side);
            if (!empty)
                pi.Draw(g, hor, ver, x, y);
        }

        public void Highlight(Graphics g, Piece p)
        {
            Color c = Color.FromArgb(255, 127, 39);
            Brush br = new SolidBrush(c);
            g.FillRectangle(br, x, y, side, side);
            p.Draw(g, hor, ver, x, y);
        }

        public void DrawE(Graphics g, Brush br)
        {
            g.FillEllipse(br, x + 27, y + 27, 21, 21);
            if (!empty)
                pi.Draw(g, hor, ver, x, y);
        }

        public void DrawT(Graphics g, Brush br)
        {
            Point p1 = new Point(x + 15, y);
            Point p2 = new Point(x + 60, y);
            Point p3 = new Point(x + side, y + 15);
            Point p4 = new Point(x + side, y + 60);
            Point p5 = new Point(x + 60, y + side);
            Point p6 = new Point(x + 15, y + side);
            Point p7 = new Point(x, y + 60);
            Point p8 = new Point(x, y + 15);
            Point[] p = new Point[8]
                { p1, p2, p3, p4, p5, p6, p7, p8 };
            Brush c = new SolidBrush(color);
            g.FillRectangle(br, x, y, side, side);
            g.FillPolygon(c, p);
            c.Dispose();
            if (!empty)
                pi.Draw(g, hor, ver, x, y);
        }

        public void Erase()
        {

        }

        void CheckMoves(object sender, MovesEventArgs ev)
        {
            if (hor == ev.hor && ver == ev.ver && !empty && pi.Army == ev.army)
            {
                pi.Move(ev.matr, 8, 8);
                ev.res = true;
            }
        }

        void GetPiece(object sender, MovesEventArgs ev)
        {
            if (!ev.extra)
            {
                if (hor == ev.horPiece && ver == ev.verPiece && ev.matr[ev.hor, ev.ver] >= 2)
                {
                    ev.pi = pi;
                    pi = null;
                    empty = true;
                    ev.res = true;
                }
            }
            else
            {
                if (hor == ev.horPieceExtra && ver == ev.verPieceExtra)
                {
                    ev.piExtra = pi;
                    pi = null;
                    empty = true;
                    ev.res = true;
                }
            }
        }

        void AddPiece(object sender, MovesEventArgs ev)
        {
            if (!ev.extra)
            {
                if (hor == ev.hor && ver == ev.ver && ev.matr[ev.hor, ev.ver] >= 2)
                {
                    pi = ev.pi;
                    empty = false;
                    ev.res = true;
                }
            }
            else
            {
                if (hor == ev.horExtra && ver == ev.verExtra && ev.matr[ev.horExtra, ev.verExtra] >= 2)
                {
                    pi = ev.piExtra;
                    empty = false;
                    ev.res = true;
                }
            }
        }

        void EndMove(object sender, MovesEventArgs ev)
        {
            if (hor == ev.hor && ver == ev.ver && ev.matr[hor, ver] >= 2)
            {
                pi.Moves++;
                ev.res = true;
            }
        }

        void CheckCastle(object sender, SpecMoveEventArgs ev)
        {
            if (pi is Rook && hor == ev.hor && ver == ev.ver && pi.Army == ev.army && pi.Moves == 0)
                ev.res = true;
        }

        void CheckPassant(object sender, SpecMoveEventArgs ev)
        {
            if (pi is Pawn && hor == ev.hor && ver == ev.ver && pi.Army == ev.army)
                ev.res = true;
        }

        public void SetXY(int x, int y)
        {
            if (x >= 0)
                this.x = x;
            if (y >= 0)
                this.y = y;
        }

        public void SetHorVer(int x, int y)
        {
            if (x >= 0)
                this.hor = x;
            if (y >= 0)
                this.ver = y;
        }

        public Piece Pi
        {
            get { return pi; }
            set
            {
                if (value == null)
                    empty = true;
                else empty = false;
                pi = value;
            }
        }

        public int GetX()
        {
            return x;
        }
        public int GetY()
        {
            return y;
        }
        public int GetHor()
        {
            return hor;
        }
        public int GetVer()
        {
            return ver;
        }

    }
}
