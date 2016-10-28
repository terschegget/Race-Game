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

        public FormRaceGame()
        {
            InitializeComponent();

            Car car1 = new Car(16, 16, 0, 0, 1000, Keys.Left, Keys.Right, Keys.Up, Keys.Down, "blueCar.png");
            Car car2 = new Car(16, 16, 0, 0, 1000, Keys.A, Keys.D, Keys.W, Keys.S, "redCar.png");

            cars.Add(car1);
            cars.Add(car2);

            this.SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.DoubleBuffer, true);
            

            this.ResizeEnd += new EventHandler(Form1_CreateBackBuffer);
            this.Load += new EventHandler(Form1_CreateBackBuffer);

            this.Paint += new PaintEventHandler(Form1_PaintUI);
            this.Paint += new PaintEventHandler(Form1_PaintCar2);
            this.Paint += new PaintEventHandler(Form1_PaintCar1);

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

        void Form1_PaintCar1(object sender, PaintEventArgs e) {
            if (Backbuffer != null) {
                e.Graphics.DrawImageUnscaled(Backbuffer, Point.Empty);
            }
            DrawCar1(e.Graphics);
        }

        void Form1_PaintCar2(object sender, PaintEventArgs e)
        {
            if (Backbuffer != null)
            {
                e.Graphics.DrawImageUnscaled(Backbuffer, Point.Empty);
            }
            DrawCar2(e.Graphics);
        }

        void Form1_PaintUI(object sender, PaintEventArgs e)
        {
            SolidBrush blueBrush = new SolidBrush(Color.Blue);
            Rectangle rect = new Rectangle(0, ClientSize.Height - 96, ClientSize.Width, 96);
            e.Graphics.FillRectangle(blueBrush, rect);
        }

        void Form1_CreateBackBuffer(object sender, EventArgs e) {
            if (Backbuffer != null)
                Backbuffer.Dispose();

            Backbuffer = new Bitmap(ClientSize.Width, ClientSize.Height);
        }
        
        void DrawCar1(Graphics g)
        {
            g.TranslateTransform(cars[0].getPosition().X + 16, cars[0].getPosition().Y + 16);
            g.RotateTransform(cars[0].getRotation() * (float)(180.0 / Math.PI) + 90);
            g.TranslateTransform(-cars[0].getPosition().X - 16, -cars[0].getPosition().Y - 16);

            g.DrawImage(cars[0].getImage(), cars[0].getPosition());
        }

        void DrawCar2(Graphics g)
        {
            g.TranslateTransform(cars[1].getPosition().X + 16, cars[1].getPosition().Y + 16);
            g.RotateTransform(cars[1].getRotation() * (float)(180.0 / Math.PI) + 90);
            g.TranslateTransform(-cars[1].getPosition().X - 16, -cars[1].getPosition().Y - 16);

            g.DrawImage(cars[1].getImage(), cars[1].getPosition());
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
