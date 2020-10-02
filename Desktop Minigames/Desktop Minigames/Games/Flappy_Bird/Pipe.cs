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
        private Label lowerPipe;
        private Label higherPipe;
        public Label LowerPipe { get => lowerPipe; set => lowerPipe = value; }
        public Label HigherPipe { get => higherPipe; set => higherPipe = value; }
    }
}
