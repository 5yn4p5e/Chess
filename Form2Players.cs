using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Timers;

namespace Chess
{
    public partial class Form2Players : Form
    {
        Board gameBoard;
        Graphics g;
        int leftX = Board.x + Board.margin;
        int rightX = Board.x + Board.margin + 600;
        int upY = Board.y + Board.margin;
        int downY = Board.y + Board.margin + 600;
        bool endGame = false;
        BinaryFormatter formatter = new BinaryFormatter();
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
        //        return cp;
        //    }
        //}
        public Form2Players()
        {
            StandardConstructor();
            gameBoard = new Board();
        }

        public Form2Players(Board obj)
        {
            StandardConstructor();
            gameBoard = obj;
            string str = null;
            gameBoard.NotationToForm(ref str);
            notationTextBox.Text = str;
            gameBoard.RecoverySubscription();
        }

        void StandardConstructor()
        {
            InitializeComponent();
            g = this.CreateGraphics();
            //Board.ChangeStatusEvent += ChangeStat;
            Board.ChangeNotationEvent += ChangeNotation;
        }

        private void Form2Players_Load(object sender, EventArgs e)
        {

        }

        private void btnCross_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void Serialize(bool mode)
        {
            string str = null;
            CreateFileName(ref str);
            if (!mode)
            {
                FileStream stream = new FileStream(str, FileMode.Create);
                formatter.Serialize(stream, gameBoard);
                stream.Close();
            }
            else
            {
                str += ".txt";
                using (StreamWriter stream = new StreamWriter(str, false))
                {
                    stream.WriteLine(notationTextBox.Text);
                }
            }
        }

        void CreateFileName(ref string str)
        {
            DateTime time = DateTime.Now;
            str += time.Year;
            str += "_" + time.Month;
            str += "_" + time.Day;
            str += "_" + time.Hour;
            str += "_" + time.Minute;
            str += "_" + time.Second;
            str += ".chsv";
        }

        private void SaveAndGoToMenu(object sender, EventArgs e)
        {
            if (!endGame)
            {
                Serialize(false);
            }
            else
            {
                Serialize(true);
            }
            Board.ChangeNotationEvent -= ChangeNotation;
            Dispose();
            FormMainMenu mainMenu = new FormMainMenu();
            mainMenu.Show();
        }

        void RecoverySubscription()
        {

        }

        private void Form2Players_Paint(object sender, PaintEventArgs e)
        {
            gameBoard.Draw(e.Graphics);
        }

        private void Form2Players_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.X >= leftX && e.X <= rightX && e.Y >= upY && e.Y < downY && !endGame)
                gameBoard.Play(e.X, e.Y, g);
        }

        //private void ChangeStat(object sender, StatusEventArgs ev)
        //{
        //    if (ev.mode == 0)
        //    {
        //        if (ev.army == 0)
        //            labelStatus.Text = "ИДЁТ ИГРА. ХОД ЧЁРНЫХ.";
        //        else
        //            this.labelStatus.Text = "ИДЁТ ИГРА. ХОД БЕЛЫХ.";
        //    }
        //    else if (ev.mode == 1)
        //    {
        //        if (ev.army == 0)
        //            labelStatus.Text = "ИДЁТ ИГРА. ШАХ. ХОД ЧЁРНЫХ.";
        //        else
        //            labelStatus.Text = "ИДЁТ ИГРА. ШАХ. ХОД БЕЛЫХ.";
        //    }
        //    else if (ev.mode == 2)
        //    {
        //        labelStatus.Text = "ИГРА ОКОНЧЕНА. ПАТ.";
        //    }
        //    else if (ev.mode == 3)
        //    {
        //        labelStatus.Text = "ИГРА ОКОНЧЕНА. НИЧЬЯ.";
        //    }
        //    else if (ev.mode == 4)
        //    {
        //        if (ev.army == 0)
        //            labelStatus.Text = "МАТ. ПОБЕДА БЕЛЫХ.";
        //        else
        //            labelStatus.Text = "МАТ. ПОБЕДА ЧЁРНЫХ.";
        //    }
        //    if (ev.mode > 1)
        //        endGame = true;
        //}

        private void ChangeNotation(object sender, NotationEventArgs ev)
        {
            notationTextBox.Text += ev.str;
        }

        private void ExitApp(object sender, FormClosingEventArgs e)
        {
            Serialize(false);
            Application.Exit();
        }

        private void TickWhite(object sender, EventArgs e)
        {
        }

        private void TickBlack(object sender, EventArgs e)
        {

        }
    }
}
