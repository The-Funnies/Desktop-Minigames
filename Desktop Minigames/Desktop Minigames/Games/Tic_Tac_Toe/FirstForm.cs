using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class FirstForm : Form
    {
        public FirstForm()
        {
            InitializeComponent();
        }

        private void FirstForm_Load(object sender, EventArgs e)
        {

        }

        private void Multiplayer_Click(object sender, EventArgs e)
        {
            HostOrConnect choiceWindow = new HostOrConnect();
            choiceWindow.StartPosition = FormStartPosition.Manual;
            choiceWindow.Location = this.Location;
            this.Hide();
            choiceWindow.Show();
        }

        private void Single_Click(object sender, EventArgs e)
        {
            SingleplayerGame gameForm = new SingleplayerGame();
            gameForm.StartPosition = FormStartPosition.Manual;
            gameForm.Location = this.Location;
            this.Hide();
            gameForm.Show();
        }
    }
}
