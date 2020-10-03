using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop_Minigames
{
    public delegate void Delegate();
    public partial class ChatClient : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private TextBox text;
        private Thread waitformessages;
        private Thread language;
        private List<Label> messages = new List<Label>();
        public ChatClient()
        {
            this.BackgroundImage = Properties.Resources.the_funnies_club_chat;

            Width = (int)(Screen.PrimaryScreen.WorkingArea.Size.Width / 4.8);
            Height = (int)(Screen.PrimaryScreen.WorkingArea.Size.Height / 1.87);

            text = new TextBox();
            text.Font = new Font("Ariel", 28);
            text.Size = new Size((int)(Width / 1.12), (int)(Height / 5.56));
            text.Location = new Point((int)(Width / 26.666667), (int)(Height / 1.21));
            Controls.Add(text);






            client = new TcpClient("localhost", 8888);
            stream = client.GetStream();

            byte[] data = Encoding.ASCII.GetBytes(Environment.UserName);
            stream.Write(data, 0, data.Length);

            waitformessages = new Thread(WaitForMessages);
            waitformessages.Start();

            this.FormClosed += CloseWindow;
            this.Shown += StartThread;
        }
        void WaitForMessages()
        {
            while (true)
            {
                byte[] data = new byte[257];
                stream.Read(data, 0, data.Length);

                NewMessage(Encoding.ASCII.GetString(data));
            }
        }
        void CloseWindow(object sender, EventArgs args)
        {

            stream.Close();
            client.Close();
        }
        void StartThread(object sender, EventArgs args)
        {
            language = new Thread(Language);
           // language.Start();


        }
        void NewMessage(string mes)
        {
            Label label = new Label();
            label.Text = mes;
            label.Size = new Size((int)(Width / 2.3), Height / 8);
            label.Font = new Font("Ariel", 14);
            label.Location = new Point((int)(Width / 50), (int)(Height / 1.5));
            label.BackColor = Color.Gray; 
            label.Tag = false;

            this.Invoke(new Delegate(() =>
            {
                foreach (Label label1 in messages)
                {
                    label1.Location = new Point(label1.Location.X, (bool)messages[messages.Count-1].Tag? label1.Location.Y - (int)(label1.Size.Height * 0.8): label1.Location.Y - (int)(label1.Size.Height * 1.1));
                    if (((bool)(messages[messages.Count - 1].Tag) && label1.Location.Y - (int)(label1.Size.Height * 0.8) < 70)|| (!(bool)(messages[messages.Count - 1].Tag) && (label1.Location.Y - (int)(label1.Size.Height * 1.1) < 70)))
                    {
                        Controls.Remove(label1);
                    }
                }
                Controls.Add(label);
            }));
            
            messages.Add(label);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (text.Text != "")
                {
                    Label label = new Label();
                    label.Text = text.Text;
                    label.Size = new Size((int)(Width / 2.3), Height / 8);
                    label.Font = new Font("Ariel", 14);
                    label.Location = new Point((int)(Width / 1.95), (int)(Height / 1.48));
                    label.BackColor = Color.Aquamarine;
                    label.Tag = true;
                    
                    Controls.Add(label);
                    foreach (Label label1 in messages)
                    {
                        label1.Location = new Point(label1.Location.X, (bool)messages[messages.Count - 1].Tag ? label1.Location.Y - (int)(label1.Size.Height * 1.1) : label1.Location.Y - (int)(label1.Size.Height * 0.8));
                        if (((bool)(messages[messages.Count - 1].Tag) && label1.Location.Y - (int)(label1.Size.Height * 1.1) < 70) || (!(bool)(messages[messages.Count - 1].Tag) && (label1.Location.Y - (int)(label1.Size.Height * 0.8) < 70)))
                        {
                            Controls.Remove(label1);
                        }
                    }
                    messages.Add(label);


                    byte[] data = Encoding.ASCII.GetBytes(text.Text);
                    stream.Write(data, 0, data.Length);

                    text.Text = null;
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
                    this.Invoke(new Delegate(() =>
                    {
                        text.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
                    }));
                }
                else
                {
                    this.Invoke(new Delegate(() =>
                    {
                        text.RightToLeft = System.Windows.Forms.RightToLeft.No;
                    }));
                }
                Thread.Sleep(100);
            }


        }
    }
}
