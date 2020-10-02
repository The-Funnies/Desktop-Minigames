using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class ConnectToServer : Form
    {
        private MultiplayerGame gameForm;

        public ConnectToServer(MultiplayerGame form)
        {
            InitializeComponent();
            this.gameForm = form;
        }

        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            try
            {
                TcpClient client = new TcpClient(ipBox.Text, ServerGUI.PORT);
                NetworkStream stream = client.GetStream();
                gameForm.client = client;
                gameForm.stream = stream;
                this.Hide();
                gameForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error");
            }
        }

        private void ConnectToServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
