using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop_Minigames
{
    public partial class LocalOrOnline : Form
    {
        public LocalOrOnline()
        {
            InitializeComponent();

            this.FormClosed += (sender, e) => { Environment.Exit(Environment.ExitCode); };

            Width = 4*Screen.PrimaryScreen.Bounds.Width / 17;
            Height = 2 * Screen.PrimaryScreen.Bounds.Height / 5;
            this.MaximumSize = new Size(this.Width, this.Height);
        }


        private void Multiplayer_Click(object sender, EventArgs e)
        {
            UltimateTicTacToe choiceWindow = new UltimateTicTacToe();
            choiceWindow.StartPosition = FormStartPosition.Manual;
            choiceWindow.Location = this.Location;
            this.Hide();
            choiceWindow.Show();
        }

        private void Single_Click(object sender, EventArgs e)
        {
            Client gameForm = new Client();
            gameForm.StartPosition = FormStartPosition.Manual;
            gameForm.Location = this.Location;
            this.Hide();
            gameForm.Show();
        }

        private void LocalOrOnline_Load(object sender, EventArgs e)
        {

        }
    }
}
