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
    public partial class WhistMain : Form
    {
        public WhistMain()
        {
            InitializeComponent();
        }

        private void WhitsMain_Load(object sender, EventArgs e)
        {
            this.Width = (int)(Screen.PrimaryScreen.WorkingArea.Size.Width / 4.33225);
            this.Height = (int)(Screen.PrimaryScreen.WorkingArea.Size.Height / 1.761);

            Label title = new Label();
            title.Text = "Please choose one of the options below";
            title.Font = new Font("Ariel", 16);
            title.Size = new Size(400, 100);
            title.Location = new Point(Width / 2 -(int)( title.Size.Width / 1.95), Height / 9);
            Controls.Add(title);

            Button host = new Button();
            Button play = new Button();

            host.Text = "Host";
            play.Text = "Play";
            host.Size = new Size(125, 125);
            play.Size = new Size(125, 125);
            host.Font = new Font("Ariel", 18);
            play.Font = new Font("Ariel", 18);
            host.Location = new Point(Width / 2 - (int)(host.Size.Width / 1.9), (int)(Height / 3.5));
            play.Location = new Point(Width / 2 - (int)(play.Size.Width / 1.9), (int)(Height / 1.75));
            play.Click += Click;
            host.Click += Click;
            Controls.Add(play);
            Controls.Add(host);

        }
        public void Click(object sender,EventArgs args)
        {
            Button but = (Button)sender;

            if (but.Text == "Play")
            {
                Whist hostForm = new Whist();
                hostForm.StartPosition = FormStartPosition.Manual;
                hostForm.Location = new Point(this.Location.X, 0);
                this.Hide();
                Controls.Clear();
                hostForm.Show();
            }
            else
            {
                WhistServer hostForm = new WhistServer();
                hostForm.StartPosition = FormStartPosition.Manual;
                hostForm.Location = new Point(this.Location.X, 0);
                this.Hide();
                Controls.Clear();
                hostForm.Show();
            }
            
        }
    }
}
