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

        List<Car> cars = new List<Car>();

        public FormRaceGame()
        {
            InitializeComponent();

            Car car1 = new Car(30, 30, 0, 0, Keys.Left, Keys.Right, Keys.Up, Keys.Down, new Bitmap(Path.Combine(Environment.CurrentDirectory, "pijl_2.png")));

            cars.Add(car1);

            this.SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.DoubleBuffer, true);

            this.ResizeEnd += new EventHandler(Form_CreateBackBuffer);
            this.Load += new EventHandler(Form_CreateBackBuffer);
            this.Paint += new PaintEventHandler(Form_Paint);
        }

        //Drawing
        void Form_Paint(object sender, PaintEventArgs e)
        {
            if(Backbuffer != null)
            {
                e.Graphics.DrawImageUnscaled(Backbuffer, Point.Empty);
            }
        }

        void Form_CreateBackBuffer(object sender, EventArgs e)
        {
            if (Backbuffer != null)
                Backbuffer.Dispose();

            Backbuffer = new Bitmap(ClientSize.Width, ClientSize.Height);
        }

        void Draw(Graphics g)
        {
            foreach (Car car in cars)
                g.DrawImage(car.getImage(), car.getPosition());
        }
    }
}
