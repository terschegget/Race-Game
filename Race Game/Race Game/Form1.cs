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
        List<Bitmap> track = new List<Bitmap>();
        private object b;

        //Initalizatie van alle stukken van de baan
        Bitmap fullTrack = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/fullTrack.png"));
        /*
        Bitmap trackPitsEnter = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/pitstopBocht1Rechts.png"));
        Bitmap trackPitsExit = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/pitstopBocht1Links2.png"));
        Bitmap trackPitstop = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/pitstop.png"));
        Bitmap trackFinish = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/start.png"));
        Bitmap trackPitsCornerRight = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/pitstopBocht2Rechts.png"));
        Bitmap trackPitsCornerLeft = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/pitstopBocht2Links.png"));
        Bitmap trackStraight1 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/baanRechtBreed.png"));
        Bitmap trackStraight2 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/baanRechtBreed.png"));
        Bitmap trackStraight3 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/baanRechtBreed.png"));
        Bitmap trackCorner1 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/bocht2v4.png"));
        Bitmap trackCorner2 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/bocht1v3.png"));
        Bitmap trackCorner3 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/bocht2v1.png"));
        Bitmap trackCorner4 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/bocht2v3.png"));
        Bitmap trackCorner5 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/bocht1v1.png"));
        Bitmap trackCorner6 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/bocht1v2.png"));
        Bitmap trackCorner7 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/bocht1v4.png"));
        Bitmap trackCorner8 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/bocht1v3.png"));
        Bitmap trackCorner9 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/bocht1v4.png"));
        Bitmap trackCorner10 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/bocht1v2.png"));
        Bitmap trackStraightUp1 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/baanRechtHoog.png"));
        Bitmap trackStraightUp2 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/baanRechtHoog.png"));
        Bitmap trackStraightUp3 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/baanRechtHoog.png"));
        Bitmap trackStraightUp4 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/baanRechtHoog.png"));
        Bitmap trackStraightUp5 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/baanRechtHoog.png"));
        Bitmap trackStraightUp6 = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/baanRechtHoog.png"));
        */
        public FormRaceGame()
        {
            InitializeComponent();

            Car car1 = new Car(ClientSize.Width / 2, ClientSize.Height - 190, 135, 0, 1000, Keys.Left, Keys.Right, Keys.Up, Keys.Down, "bluecar2.png");
            Car car2 = new Car(ClientSize.Width / 2, ClientSize.Height - 170, 135, 0, 1000, Keys.A, Keys.D, Keys.W, Keys.S, "redcar2.png");

            cars.Add(car1);
            cars.Add(car2);

            //add all track parts to the list
            /*
            track.Add(trackPitsEnter);
            track.Add(trackPitsExit);
            track.Add(trackPitstop);
            track.Add(trackFinish);
            track.Add(trackPitsCornerRight);
            track.Add(trackPitsCornerLeft);
            track.Add(trackStraight1);
            track.Add(trackStraight2);
            track.Add(trackStraight3);
            track.Add(trackCorner1);
            track.Add(trackCorner2);
            track.Add(trackCorner3);
            track.Add(trackCorner4);
            track.Add(trackCorner5);
            track.Add(trackCorner6);
            track.Add(trackCorner7);
            track.Add(trackCorner8);
            track.Add(trackCorner9);
            track.Add(trackCorner10);
            track.Add(trackStraightUp1);
            track.Add(trackStraightUp2);
            track.Add(trackStraightUp3);
            track.Add(trackStraightUp4);
            track.Add(trackStraightUp5);
            track.Add(trackStraightUp6);
            */

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

        void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (Car car in cars)
                car.handleKeyUpEvent(e);
        }

        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            foreach (Car car in cars)
                car.handleKeyDownEvent(e);
        }

        void Form1_PaintCar(object sender, PaintEventArgs e)
        {
            if (Backbuffer != null)
            {
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
            //Het grafisch tekenen van elk stuk van de baan
            e.Graphics.DrawImage(fullTrack, 0, 0);
            /*
            e.Graphics.DrawImage(trackCorner2, ClientSize.Width / 2 + 320, ClientSize.Height - 224);
            e.Graphics.DrawImage(trackStraight2, ClientSize.Width / 2 + 192, ClientSize.Height - 224);
            e.Graphics.DrawImage(trackPitsEnter, ClientSize.Width / 2 + 64, ClientSize.Height - 224);
            e.Graphics.DrawImage(trackPitsCornerRight, ClientSize.Width / 2 + 64, ClientSize.Height - 352);
            e.Graphics.DrawImage(trackPitsCornerLeft, ClientSize.Width / 2 - 192, ClientSize.Height - 352);
            e.Graphics.DrawImage(trackPitstop, ClientSize.Width / 2 - 64, ClientSize.Height - 352);
            e.Graphics.DrawImage(trackFinish, ClientSize.Width / 2 - 64, ClientSize.Height - 224);
            e.Graphics.DrawImage(trackPitsExit, ClientSize.Width / 2 - 192, ClientSize.Height - 224);
            e.Graphics.DrawImage(trackStraight1, ClientSize.Width / 2 - 320, ClientSize.Height - 224);
            e.Graphics.DrawImage(trackCorner1, ClientSize.Width / 2 - 448, ClientSize.Height - 224);
            e.Graphics.DrawImage(trackStraightUp1, ClientSize.Width / 2 - 448, ClientSize.Height - 352);
            e.Graphics.DrawImage(trackCorner3, ClientSize.Width / 2 - 448, ClientSize.Height - 480);
            e.Graphics.DrawImage(trackStraight3, ClientSize.Width / 2 - 320, ClientSize.Height - 480);
            e.Graphics.DrawImage(trackCorner4, ClientSize.Width / 2 - 192, ClientSize.Height - 480);
            e.Graphics.DrawImage(trackStraightUp2, ClientSize.Width / 2 - 192, ClientSize.Height - 608);
            e.Graphics.DrawImage(trackCorner5, ClientSize.Width / 2 - 192, ClientSize.Height - 736);
            e.Graphics.DrawImage(trackCorner6, ClientSize.Width / 2 - 64, ClientSize.Height - 736);
            e.Graphics.DrawImage(trackStraightUp3, ClientSize.Width / 2 - 64, ClientSize.Height - 608);
            e.Graphics.DrawImage(trackCorner7, ClientSize.Width / 2 - 64, ClientSize.Height - 480);
            e.Graphics.DrawImage(trackCorner8, ClientSize.Width / 2 + 64, ClientSize.Height - 480);
            e.Graphics.DrawImage(trackStraightUp4, ClientSize.Width / 2 + 64, ClientSize.Height - 608);
            e.Graphics.DrawImage(trackCorner5, ClientSize.Width / 2 + 64, ClientSize.Height - 736);
            e.Graphics.DrawImage(trackCorner6, ClientSize.Width / 2 + 192, ClientSize.Height - 736);
            e.Graphics.DrawImage(trackStraightUp5, ClientSize.Width / 2 + 192, ClientSize.Height - 608);
            e.Graphics.DrawImage(trackStraightUp6, ClientSize.Width / 2 + 192, ClientSize.Height - 480);
            e.Graphics.DrawImage(trackCorner9, ClientSize.Width / 2 + 192, ClientSize.Height - 352);
            e.Graphics.DrawImage(trackCorner10, ClientSize.Width / 2 + 320, ClientSize.Height - 352);
            */
        }

        void Form1_CreateBackBuffer(object sender, EventArgs e)
        {
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

                if (car.onMap(fullTrack, car.getPosition().X, car.getPosition().Y))
                {
                    car.speed = .6f;
                }

                Console.Write(car.getPosition());
                car.calculateNewPosition();
            }
                       
            Invalidate();
        }
    }
}
