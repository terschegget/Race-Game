using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Race_Game
{
    public partial class FormRaceGame : Form
    {   
        Bitmap Backbuffer;

        public FormRaceGame()
        {
            InitializeComponent();

            Car car1 = new Car(30, 30, 0, 0, Keys.Left, Keys.Right, Keys.Up, Keys.Down, new Bitmap(Path.Combine(Environment.CurrentDirectory, "pijl.png")));

            this.SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.DoubleBuffer, true);

        }
    }
}
