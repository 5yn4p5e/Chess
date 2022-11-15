using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;

namespace Chess
{
    [Serializable]

    public abstract class Piece
    {
        protected int x, y, hor, ver, moves = 0;
        protected const int side = 71;
        protected int army;
        protected Color color;
        protected Bitmap pic;

        public void Draw(Graphics g, int hor, int ver, int x, int y)
        {
            this.hor = hor;
            this.ver = ver;
            this.x = x + 2;
            this.y = y + 2;
            g.DrawImage(pic, this.x, this.y, side, side);
        }

        public void Erase(Graphics g)
        {
            if ((hor + ver) % 2 == 0)
                color = Color.FromArgb(72, 157, 60);
            else
                color = Color.FromArgb(255, 253, 157);
            Brush br = new SolidBrush(color);
            g.FillRectangle(br, x, y, side, side);
            br.Dispose();
        }

        public int Army { get { return army; } }

        public int Moves { get { return moves; } set { moves = value; } }

        abstract public void Move(int [,] matr, int a, int b);
    }
}
