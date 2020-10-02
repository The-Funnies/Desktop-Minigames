using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class UserControl1 : UserControl
    {
        private PictureBox[] slots = new PictureBox[4];
        private PictureBox[] checks = new PictureBox[4];
        private Image[] checksImg = new Image[4];
        public UserControl1()
        {
            InitializeComponent();
        }
        public UserControl1(int s1, int s2, int s3, int s4, int[] bullseyes)
        {
            {
                slots[0] = new PictureBox();
                slots[0].Location = new Point(10, 0);
                Image imgg = Image.FromFile($"..\\..\\bPNG\\{s1}.png");
                imgg = Resize(imgg, 50, 50);
                slots[0].Size = imgg.Size;
                slots[0].Image = imgg;
                Controls.Add(slots[0]);

                slots[1] = new PictureBox();
                slots[1].Location = new Point(10 + 50 * 1, 0);
                Image img1 = Image.FromFile($"..\\..\\bPNG\\{s2}.png");
                img1 = Resize(img1, 50, 50);
                slots[1].Size = img1.Size;
                slots[1].Image = img1;
                Controls.Add(slots[1]);

                slots[2] = new PictureBox();
                slots[2].Location = new Point(10 + 50 * 2, 0);
                Image img2 = Image.FromFile($"..\\..\\bPNG\\{s3}.png");
                img2 = Resize(img2, 50, 50);
                slots[2].Size = img2.Size;
                slots[2].Image = img2;
                Controls.Add(slots[2]);

                slots[3] = new PictureBox();
                slots[3].Location = new Point(10 + 150, 0);
                Image img3 = Image.FromFile($"..\\..\\bPNG\\{s4}.png");
                img3 = Resize(img3, 50, 50);
                slots[3].Size = img3.Size;
                slots[3].Image = img3;
                Controls.Add(slots[3]);
            }

            for (int i = 0; i < 4; i++)
            {
                checks[i] = new PictureBox();
                Image imgc = Image.FromFile($"..\\..\\bPNG\\check{bullseyes[i]}.png");
                imgc = Resize(imgc, 20, 20);
                checks[i].Size = imgc.Size;
                checks[i].Image = imgc;
            }

            checks[0].Location = new Point(230, 10);
            Controls.Add(checks[0]);

            checks[1].Location = new Point(250, 10);
            Controls.Add(checks[1]);

            checks[2].Location = new Point(230, 30);
            Controls.Add(checks[2]);

            checks[3].Location = new Point(250, 30);
            Controls.Add(checks[3]);
        }
        private void UserControl1_Load(object sender, EventArgs e)
        {

        }
        public Image Resize(Image image, int w, int h)
        {
            Bitmap bmp = new Bitmap(w, h);
            Graphics grp = Graphics.FromImage(bmp);
            grp.DrawImage(image, 0, 0, w, h);
            grp.Dispose();
            return bmp;
        }
    }
}
