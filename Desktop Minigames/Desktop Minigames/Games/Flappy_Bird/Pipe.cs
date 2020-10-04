using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace flappy_bird
{
    public class Pipe
    {
        private PictureBox lowerPipe;
        private PictureBox higherPipe;
        public PictureBox LowerPipe { get => lowerPipe; set => lowerPipe = value; }
        public PictureBox HigherPipe { get => higherPipe; set => higherPipe = value; }
    }
}
