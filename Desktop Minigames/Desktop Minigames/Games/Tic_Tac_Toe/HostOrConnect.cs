using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class HostOrConnect : Form
    {
        private MultiplayerGame gameForm;

        public HostOrConnect()
        {
            InitializeComponent();
            gameForm = new MultiplayerGame();
            gameForm.StartPosition = FormStartPosition.Manual;
            gameForm.Location = this.Location;

        }

        private void Connect_Click(object sender, EventArgs e)
        {
            ConnectToServer connectWindow = new ConnectToServer(gameForm);
            connectWindow.StartPosition = FormStartPosition.Manual;
            connectWindow.Location = this.Location;
            this.Hide();
            connectWindow.Show();
        }

        private void Host_Click(object sender, EventArgs e)
        {
            ServerGUI serverWindow = new ServerGUI();
            serverWindow.StartPosition = FormStartPosition.Manual;
            serverWindow.Location = this.Location;
            this.Hide();

            Thread serverThread = new Thread(() =>
            {
                Application.Run(serverWindow);
            });

            serverThread.Start();
            gameForm.SetLocalhost();
            gameForm.Show();
        }

        private void HostOrConnect_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
