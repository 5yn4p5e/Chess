using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Chess
{
    public partial class FormMainMenu : Form
    {
        public FormMainMenu()
        {
            InitializeComponent();
        }

        private void FormMainMenu_Load(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn2players_Click(object sender, EventArgs e)
        {
            Hide();
            Form2Players twoPlayers = new Form2Players();
            twoPlayers.Show();
        }

        private void ExitApp(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btnLoadGame_Click(object sender, EventArgs e)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Board brd;
            string filePath;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                openFileDialog.Filter = "Chess saves (*.chsv)|*.chsv";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                    {
                        brd = (Board)formatter.Deserialize(fileStream);
                        fileStream.Close();
                        Hide();
                        Form2Players twoPlayers = new Form2Players(brd);
                        twoPlayers.Show();
                    }
                }
            }
        }
    }
}
