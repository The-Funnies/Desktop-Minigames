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

namespace Desktop_Minigames
{
    public delegate void myDelegate(Form form);

    public partial class Ohno : Form
    {
        private Label label = new Label();

        public Ohno()
        {
            this.Text = "FUCK YOU";
            this.Size = new Size(250, 250);
            this.BackColor = Color.FromName("Black");
            this.FormClosing += Ohno_FormClosing;
            this.Shown += Ohno_Shown;
            label.Font = new Font("Arial", 42);
            label.Size = new Size(200, 200);
            label.Location = new Point(25, 25);
            label.Text = "FUCK\nYOU";
            label.ForeColor = Color.FromName("White");
            Controls.Add(label);
        }

        private void Ohno_Shown(object sender, EventArgs e)
        {
            Thread changeColor = new Thread(ChangeColor);
            Thread moveWindow = new Thread(MoveWindow);
            changeColor.Start();
            moveWindow.Start();
        }

        public void MoveWindow()
        {
            try
            {
                while (true)
                {
                    this.Invoke(new myDelegate(Move), this);
                    Thread.Sleep(5);
                }
            }
            catch
            {

            }
        }

        public void ChangeColor()
        {
            while (true)
            {
                this.BackColor = Color.FromName("White");
                label.ForeColor = Color.FromName("Black");
                Thread.Sleep(200);
                this.BackColor = Color.FromName("Black");
                label.ForeColor = Color.FromName("White");
                Thread.Sleep(200);
            }
        }

        private void Ohno_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = 0; i < 4; i++)
            {
                Ohno connectForm = new Ohno
                {
                    StartPosition = FormStartPosition.Manual,
                    Location = this.Location
                };
                connectForm.Show();
            }
        }

        public void Move(Form form)
        {
            int changeX = Minigames.random.Next(0, 2) == 0 ? -5 : 5;
            int changeY = Minigames.random.Next(0, 2) == 0 ? -5 : 5;
            form.Location = new Point(form.Location.X + changeX, form.Location.Y + changeY);
        }
    }
}
