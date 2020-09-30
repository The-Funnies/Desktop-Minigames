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
    public partial class Whist : Form
    {
        private TextBox name;
        private TextBox ip;
        private Button submit;
        public Whist()
        {
            this.Width = (int)(Screen.PrimaryScreen.WorkingArea.Size.Width / 4.33225);
            this.Height = (int)(Screen.PrimaryScreen.WorkingArea.Size.Height / 1.761);

            Label label = new Label();
            label.Text = "Please enter your name and the host's IP to enter the game.";
            label.Font = new Font("Ariel", 20);
            label.Location = new Point(20, 50);
            label.Size = new Size(400, 100);
            Controls.Add(label);

            label = new Label();
            label.Text = "Name:";
            label.Font = new Font("Ariel", 16);
            label.Location = new Point(50, 170);
            label.Size = new Size(400, 30);
            Controls.Add(label);


            name = new TextBox();
            name.Text = "Enter your name here";
            name.Font = new Font("Ariel", 14);
            name.Size = new Size(300, 200);
            name.Location = new Point(55, 200);
            name.Click += Nameclick;
            Controls.Add(name);

            label = new Label();
            label.Text = "IP:";
            label.Font = new Font("Ariel", 16);
            label.Location = new Point(50, 270);
            label.Size = new Size(400, 30);
            Controls.Add(label);

            ip = new TextBox();
            ip.Text = "Enter IP here";
            ip.Font = new Font("Ariel", 14);
            ip.Size = new Size(300, 200);
            ip.Location = new Point(55, 300);
            ip.Click += IP;
            Controls.Add(ip);

            submit = new Button();
            submit.Text = "Submit";
            submit.Location = new Point(150, 375);
            submit.Size = new Size(100, 50);
            submit.Font = new Font("Ariel", 14);
            submit.Click += Submit;
            Controls.Add(submit);
        }
        public void Nameclick(object sender, EventArgs args)
        {
            if (name.Text == "Enter your name here")
            {
                name.Text = "";
            }
        }

        public void IP(object sender, EventArgs args)
        {
            if (ip.Text == "Enter IP here")
            {
                ip.Text = "";
            }
        }

        public void Submit(object sender, EventArgs args)
        {
            //if (name.Text != "" && name.Text != "Enter your name here")
            {
                try
                {
                    WhistClient hostForm = new WhistClient(name.Text, ip.Text);
                    hostForm.StartPosition = FormStartPosition.Manual;
                    hostForm.Location = new Point(this.Location.X, 0);
                    this.Hide();
                    Controls.Clear();
                    hostForm.Show();


                }
                catch
                {
                    MessageBox.Show("Please enter a valid IP");
                }
            }
            //else
            //{
            //    MessageBox.Show("Please enter a valid name");
            //}
        }
        private void Whist_Load(object sender, EventArgs e)
        {

        }
    }
}
