using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop_Minigames
{
    public delegate void RightToLeft();
    public partial class ChatClient : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private TextBox text;
        Thread language;
        public ChatClient()
        {
            this.BackgroundImage = Properties.Resources.the_funnies_club_chat;

            Width = (int)(Screen.PrimaryScreen.WorkingArea.Size.Width / 4.8);
            Height = (int)(Screen.PrimaryScreen.WorkingArea.Size.Height / 1.87);

            text = new TextBox();
            text.Font = new Font("Ariel", 28);
            text.Size = new Size((int)(Width/1.12), (int)(Height/5.56));
            text.Location = new Point((int)(Width/26.666667), (int)(Height/1.21));
            Controls.Add(text);

          




            client = new TcpClient("localhost", 8888);
            stream = client.GetStream();

            byte[] data = System.Text.Encoding.ASCII.GetBytes(Environment.UserName);
            stream.Write(data, 0, data.Length);

           




            stream.Close();
            client.Close();
            this.Shown += StartThread;
        }

        void StartThread(object sender,EventArgs args)
        {
            language = new Thread(Language);
            language.Start();

           
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (text.Text != "")
                {
                    byte[] data = System.Text.Encoding.ASCII.GetBytes(text.Text);
                    stream.Write(data, 0, data.Length);
                }
            }
            return false;
        }

        void Language()
        {
            
            while (true)
            {
                if (InputLanguage.CurrentInputLanguage.Culture.Name == "he -IL")
                {
                    this.Invoke(new RightToLeft(() =>
                    {
                        text.RightToLeft=System.Windows.Forms.RightToLeft.Yes;
                    }));
                }
                else
                {
                    this.Invoke(new RightToLeft(() =>
                    {
                        text.RightToLeft = System.Windows.Forms.RightToLeft.No;
                    }));
                }
                Thread.Sleep(100);
            }

            
        }
    }
}
