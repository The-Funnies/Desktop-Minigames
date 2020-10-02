using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pong
{
    public partial class HostOrConnect : Form
    {
        public string name;
        private string namePath;
        public HostOrConnect()
        {
            InitializeComponent();

            string chessDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\PongGame";
            string nameFilePath = chessDirectoryPath + "\\name.txt";
            this.namePath = nameFilePath;

            if (!Directory.Exists(chessDirectoryPath))
            {
                DirectoryInfo directory = Directory.CreateDirectory(chessDirectoryPath);
                directory.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }
            else if (File.Exists(nameFilePath))
            {
                using (StreamReader sr = new StreamReader(nameFilePath, true))
                {
                    this.username.Text = sr.ReadToEnd();
                }
            }
        }

        private bool IsUsernameValid()
        {
            if (this.username.Text == "")
            {
                MessageBox.Show("You need to enter your username", "Error");
                this.username.Focus();
                return false;
            }
            else if (this.username.Text.Length > 10)
            {
                MessageBox.Show("Your username cannot be longer than 10 characters", "Error");
                this.username.Focus();
                return false;
            }
            this.name = username.Text;
            return true;
        }

        private void CreateFile(string name)
        {
            using (StreamWriter sw = new StreamWriter(namePath, false))
            {
                sw.Write(name);
            }
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            if (!IsUsernameValid()) return;
            CreateFile(username.Text);
            ConnectToServer connectWindow = new ConnectToServer(new PongGame(this.name));
            connectWindow.StartPosition = FormStartPosition.Manual;
            connectWindow.Location = this.Location;
            this.Hide();
            connectWindow.Show();
        }

        private void Host_Click(object sender, EventArgs e)
        {
            if (!IsUsernameValid()) return;
            CreateFile(username.Text);
            ServerGUI serverWindow = new ServerGUI(this.name);
            serverWindow.StartPosition = FormStartPosition.Manual;
            serverWindow.Location = this.Location;
            this.Hide();

            Thread serverThread = new Thread(() =>
            {
                Application.Run(serverWindow);
            });

            serverThread.Start();
        }

        private void HostOrConnect_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
