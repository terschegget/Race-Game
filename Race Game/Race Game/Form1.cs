using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Race_Game
{
    public partial class FormRaceGame : Form
    {   
        Bitmap Backbuffer;
        List<Car> cars = new List<Car>();
        private object b;

        Bitmap trackFinish = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/start.png"));
        Bitmap trackStraight = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/baanrechtgoeie.png"));
        Bitmap trackStraight2 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/baanrechtgoeie.png"));

        
        public FormRaceGame()
        {
            InitializeComponent();

            Car car1 = new Car(32, 32, 0, 0, 1000, Keys.Left, Keys.Right, Keys.Up, Keys.Down, "bluecar2.png");
            Car car2 = new Car(32, 32, 0, 0, 1000, Keys.A, Keys.D, Keys.W, Keys.S, "redcar2.png");

            cars.Add(car1);
            cars.Add(car2);
            
            this.SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.DoubleBuffer, true);
            

            this.ResizeEnd += new EventHandler(Form1_CreateBackBuffer);
            this.Load += new EventHandler(Form1_CreateBackBuffer);

            this.Paint += new PaintEventHandler(Form1_PaintUI);
            this.Paint += new PaintEventHandler(Form1_PaintTrack);
            this.Paint += new PaintEventHandler(Form1_PaintCar);

            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.KeyUp += new KeyEventHandler(Form1_KeyUp);
        }

        void Form1_KeyUp(object sender, KeyEventArgs e) {
            foreach (Car car in cars)
                car.handleKeyUpEvent(e);
        }

        void Form1_KeyDown(object sender, KeyEventArgs e) {
            foreach (Car car in cars)
                car.handleKeyDownEvent(e);
        }

        void Form1_PaintCar(object sender, PaintEventArgs e) {
            if (Backbuffer != null) {
                e.Graphics.DrawImageUnscaled(Backbuffer, Point.Empty);
            }
            DrawCar(e.Graphics);
        }

        void Form1_PaintUI(object sender, PaintEventArgs e)
        {
            SolidBrush blueBrush = new SolidBrush(Color.Blue);
            Rectangle rect = new Rectangle(0, ClientSize.Height - 96, ClientSize.Width, 96);
            e.Graphics.FillRectangle(blueBrush, rect);
        }

        void Form1_PaintTrack(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(trackFinish, ClientSize.Width/2 - 64, ClientSize.Height - 352);
            e.Graphics.DrawImage(trackStraight, ClientSize.Width / 2 - 188, ClientSize.Height - 352);
            e.Graphics.DrawImage(trackStraight2, ClientSize.Width / 2 - 256, ClientSize.Height - 352);
        }
        
        void Form1_CreateBackBuffer(object sender, EventArgs e) {
            if (Backbuffer != null)
                Backbuffer.Dispose();

            Backbuffer = new Bitmap(ClientSize.Width, ClientSize.Height);
        }

        void DrawCar(Graphics g)
        {
            foreach (Car car in cars)
            {
                g.TranslateTransform(car.getPosition().X + 16, car.getPosition().Y + 16);
                g.RotateTransform(car.getRotation() * (float)(180.0 / Math.PI) + 90);
                g.TranslateTransform(-1 * (car.getPosition().X + 16), -1 * (car.getPosition().Y + 16));

                g.DrawImage(car.getImage(), car.getPosition());
                g.ResetTransform();
            }
        }
        
        private void timerGameTicks_Tick(object sender, EventArgs e) {
            foreach (Car car in cars)
            {
                car.calculateNewPosition();
            }

            Invalidate();
        }
    }
}
