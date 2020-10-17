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
        PictureBox frame = new PictureBox();
        private NetworkStream stream;
        private TextBox text;
        private static bool changed= false;
        private Thread waitformessages;
        private List<Label> messages = new List<Label>();
        public ChatClient()
        {
            Width = (int)(Screen.PrimaryScreen.WorkingArea.Size.Width / 4.8);
            Height = (int)(Screen.PrimaryScreen.WorkingArea.Size.Height / 1.87);
            this.MaximumSize = new Size(this.Width, this.Height);
            Minigames.SetBackground(this);
            this.Text = this.GetType().Name;
            text = new TextBox();
            text.Font = new Font(FontFamily.GenericMonospace, 28);
            text.Size = new Size((int)(Width / 1.12), (int)(Height / 5.56));
            text.Location = new Point((int)(Width / 26.666667), (int)(Height / 1.21));
            Controls.Add(text);
            Image img = Properties.Resources.the_funnies_club_chat;
            frame.BackColor = Color.Transparent;
            frame.Dock = DockStyle.Fill;
            frame.Image = img;
            Controls.Add(frame);
            this.ControlAdded += (sender, e) =>
            {
                if (changed)
                {
                    Controls.Remove(frame);
                    changed = false;
                    Controls.Add(frame);
                    
                }
            };
            this.FormClosed += (object sender, FormClosedEventArgs e) =>
            {
                Controls.Clear();
                client.Close();
                stream.Close();
                waitformessages.Abort();
            };

            client = new TcpClient("213.57.202.58", 8888);
            stream = client.GetStream();

            byte[] data = Encoding.UTF8.GetBytes(Environment.UserName);

            stream.Write(data, 0, data.Length);


            waitformessages = new Thread(WaitForMessages);
            waitformessages.Start();

            this.FormClosed += CloseWindow;
        }
        void WaitForMessages()
        {
            
            while (true)
            {
                byte[] data = new byte[257];
                try
                {
                    stream.Read(data, 0, data.Length);
                }
                catch
                {

                }
                string mes = Encoding.UTF8.GetString(data);
                if (IsNewClient(mes))
                {

                    Label label = new Label();
                    label.Text = mes.Substring(0, BackSlash0(mes)) + " is inda chat boys";
                    label.Size = new Size((int)(Width / 2.5), Height / 40);
                    label.Font = new Font("Ariel", 8);
                    label.Location = new Point((int)(Width / 3.5), (int)(Height / 1.33));
                    label.Tag = 2;
                    label.BackColor = Color.Transparent;
                    Thread thread =new  Thread(()=>
                    {
                        this.Invoke(new Delegate(() =>
                        {

                            foreach (Label label1 in messages)
                            {
                                label1.Location = new Point(label1.Location.X, label1.Location.Y - (int)(label.Size.Height * 1.3));
                                if (((int)(messages[messages.Count - 1].Tag) == 0 && label1.Location.Y - (int)(label.Size.Height * 0.6) < 70) || ((int)(messages[messages.Count - 1].Tag) != 0 && (label1.Location.Y - (int)(label.Size.Height * 1.1) < 70)))
                                {
                                    Controls.Remove(label1);
                                }
                            }
                            Controls.Add(label);
                            label.BringToFront();
                            changed = true;

                        })); 
                        messages.Add(label);
                    });
                    thread.Start();
                    
                }
                else
                {
                    NewMessage(mes);
                }

            }
        }
        int BackSlash0(string mes)
        {
            for (int i = 0; i < mes.Length; i++)
            {
                if (mes[i] == '\0')
                {
                    return i;
                }
            }
            return mes.Length;
        }
        int BackSlashn(string mes)
        {
            for (int i = 0; i < mes.Length; i++)
            {
                if (mes[i] == '\n')
                {
                    return i;
                }
            }
            return mes.Length;
        }
        bool IsNewClient(string mes)
        {
            for (int i = 0; i < mes.Length; i++)
            {
                if (mes[i] == '\n')
                {
                    return false;
                }
            }
            return true;
        }

        void CloseWindow(object sender, EventArgs args)
        {
            stream.Close();
            client.Close();
        }
        void NewMessage(string mes)
        {
            Label label = new Label();
            label.Text = mes;
            label.ForeColor = Color.White;
            //label.Size = new Size((int)(Width / 2.3), Height / 8);
            label.Font = new Font(FontFamily.GenericMonospace, 12);
            int labelwidth = (int)(Width / 2.3);
            int rows = 1;
            int index = BackSlashn(mes) + 1;
            int length = mes.Substring(0, BackSlash0(mes)).Length - index + 1;
            rows += (int)Math.Ceiling((length * label.Font.Size) / (2 * label.Width));
            label.Size = new Size(labelwidth, rows * label.Font.Height);
            label.Location = new Point((int)(Width / 50), (int)(Height / 1.28) - label.Size.Height);
            label.BackColor = Color.Gray;
            label.Tag = 1;

            this.Invoke(new Delegate(() =>
            {
                foreach (Label label1 in messages)
                {
                    label1.Location = new Point(label1.Location.X, label1.Location.Y - (int)(label.Size.Height * 1.1));
                    //label1.Location = new Point(label1.Location.X, (int)(label1.Tag) == 2 ? label1.Location.Y - (int)(label.Size.Height* 1.03) : (int)messages[messages.Count-1].Tag==0? label1.Location.Y - (int)(label.Size.Height * 0.6): label1.Location.Y - (int)(label.Size.Height * 1.1));
                    if (((int)(messages[messages.Count - 1].Tag) == 0 && label1.Location.Y - (int)(label.Size.Height * 0.6) < 70) || ((int)(messages[messages.Count - 1].Tag) != 0 && (label1.Location.Y - (int)(label.Size.Height * 1.1) < 70)))
                    {
                        Controls.Remove(label1);
                    }
                }
                Controls.Add(label);
                changed = true;
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
                    label.Font = new Font(FontFamily.GenericMonospace, 12);
                    int labelwidth = (int)(Width / 2.3);
                    int length = text.Text.Substring(0, BackSlash0(text.Text)).Length + 1;
                    int rows = (int)Math.Ceiling((length * label.Font.Size) / (2 * label.Width));

                    label.Size = new Size(labelwidth, rows * label.Font.Height);
                    //label.Size = new Size(labelwidth, (int)(text.Text.Length * Font.Height * Font.Height * 1d / labelwidth + Font.Height * 1.03d));
                    label.Location = new Point((int)(Width / 1.95), (int)(Height / 1.28) - label.Size.Height);
                    label.BackColor = Color.Aquamarine;
                    label.Tag = 0;

                    Controls.Add(label);
                    changed = true;
                    foreach (Label label1 in messages)
                    {
                        label1.Location = new Point(label1.Location.X, label1.Location.Y - (int)(label.Size.Height * 1.1));
                        //  label1.Location = new Point(label1.Location.X, (int)(label1.Tag) == 2 ? label1.Location.Y - (int)(label.Size.Height*1.03 ) : (int)messages[messages.Count - 1].Tag==0 ? label1.Location.Y - (int)(label.Size.Height * 1.1) : label1.Location.Y - (int)(label.Size.Height * 0.6));
                        if (((int)(messages[messages.Count - 1].Tag) == 0 && label1.Location.Y - (int)(label.Size.Height * 1.1) < 70) || ((int)(messages[messages.Count - 1].Tag) != 0 && (label1.Location.Y - (int)(label.Size.Height * 0.6) < 70)))
                        {
                            Controls.Remove(label1);
                        }
                    }
                    messages.Add(label);


                    byte[] data = Encoding.UTF8.GetBytes(text.Text);
                    stream.Write(data, 0, data.Length);

                    text.Text = null;
                }
            }
            return false;
        }
     
    }
}
