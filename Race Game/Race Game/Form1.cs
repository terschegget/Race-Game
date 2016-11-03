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
        Bitmap speedRedArrow = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/redArrow.png"));
        Bitmap speedBlueArrow = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/blueArrow.png"));
        Bitmap tankBlueArrow = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/blueArrowS.png"));
        Bitmap tankRedArrow = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/redArrowS.png"));
        public FormRaceGame()
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(this.Form1_FormClosed);

            Car car1 = new Car(2, ClientSize.Width / 2, ClientSize.Height - 190, 135, 0, 1000, Keys.Left, Keys.Right, Keys.Up, Keys.Down, "bluecar2.png");
            Car car2 = new Car(1, ClientSize.Width / 2, ClientSize.Height - 170, 135, 0, 1000, Keys.A, Keys.D, Keys.W, Keys.S, "redcar2.png");

            cars.Add(car1);
            cars.Add(car2);

            this.SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.DoubleBuffer, true);

            this.ResizeEnd += new EventHandler(Form1_CreateBackBuffer);
            this.Load += new EventHandler(Form1_CreateBackBuffer);

            this.Paint += new PaintEventHandler(Form1_PaintTrack);
            this.Paint += new PaintEventHandler(Form1_PaintCar);
			this.Paint += new PaintEventHandler(Form1_PaintUI);

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
            int car1 = Convert.ToInt16(cars[0].getSpeed());
            int car2 = Convert.ToInt16(cars[1].getSpeed());

            e.Graphics.DrawImage(new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/meterroodgoed.png")), 1, ClientSize.Height - 135, 192, 128);
            e.Graphics.DrawImage(rotationArrowImg(speedRedArrow, (float)getSpeedRotation(cars[1]), 235),50, ClientSize.Height - 85, 32, 32);
            e.Graphics.DrawImage(new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/blauwegoede.png")), 810, ClientSize.Height-135, 192, 128);
            e.Graphics.DrawImage(rotationArrowImg(speedBlueArrow, (float)getSpeedRotation(cars[0]),235), 920, ClientSize.Height - 85, 32, 32);

            e.Graphics.DrawImage(rotationArrowImg(tankBlueArrow, (float)getTankRotation(cars[0]),160), 845, ClientSize.Height - 80, 32, 32);
            e.Graphics.DrawImage(rotationArrowImg(tankRedArrow, -(float)getTankRotation(cars[1]),-160), 130, ClientSize.Height - 80, 32, 32);

            Font drawfont = new Font("Arial", 14);
            SolidBrush brush = new SolidBrush(Color.Black);
            e.Graphics.DrawString("Speler 1", drawfont, brush, 350, ClientSize.Height - 92);
            e.Graphics.DrawString("Speler 2", drawfont, brush, 600, ClientSize.Height - 92);
            e.Graphics.DrawString("Aantal Rondes : "+ cars[1].getLaps(), drawfont, brush, 350, ClientSize.Height - 72);
            e.Graphics.DrawString("Aantal Rondes : " + cars[0].getLaps(), drawfont, brush, 600, ClientSize.Height - 72);
            e.Graphics.DrawString("Aantal Pitstops : " + cars[1].countPitstop, drawfont, brush, 350, ClientSize.Height - 52);
            e.Graphics.DrawString("Aantal Pitstops : " + cars[0].countPitstop, drawfont, brush, 600, ClientSize.Height - 52);

            // countPitstop

        }
        double getSpeedRotation(Car car)
        {
            //0 - 90
            double rotationSpeed = 90 * car.getSpeed();
            if (rotationSpeed < 0)
            {
                rotationSpeed = 45;
            }
            return rotationSpeed;
        }

        double getTankRotation(Car car)
        {
            //0 - 1000
            double rotationTank = 0.14 * car.getTank() + 35;
            return rotationTank;
        }
           double getWinner(Car car, int carIndex)
        {
            int winner = 0;
            int laps = car.getLaps();
            if(laps == 1)
            {
                winner = carIndex;
            }
            return winner;
        }


        public Bitmap rotationArrowImg(Bitmap b, float angle, int startAngel)
        {

            
            Bitmap returnBitmap = new Bitmap(b.Width, b.Height);
            using (Graphics g = Graphics.FromImage(returnBitmap))
            {
               
                g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
                g.RotateTransform(startAngel);
                g.RotateTransform(angle);
                g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
                g.DrawImage(b, new Point(0, 0));
            }
            return returnBitmap;
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
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void timerGameTicks_Tick(object sender, EventArgs e) {

            foreach (Car car in cars)
            {              
                car.calculateNewPosition();
                //Console.WriteLine(car.tank);
                try
                {
                    car.checkpointsHit(fullTrack, car.getPosition().X + car.getBitmap().Width / 2, car.getPosition().Y + car.getBitmap().Height / 2);
                    if (car.notOnMap(fullTrack, car.getPosition().X + car.getBitmap().Width / 2, car.getPosition().Y + car.getBitmap().Height / 2))
                    {
                        if(car.speed >= 0.6f)
                        {
                            car.speed = 0.6f;                        
                        }  
                    }
                    if (car.inPitstop(fullTrack, car.getPosition().X + car.getBitmap().Width / 2, car.getPosition().Y + car.getBitmap().Height / 2))
                    {
                        if (car.tank < 800)
                        {
                            car.speed = 0.6f;
                            car.tank = 1000f;
                            car.countPitstop++;
                        }
                    }
                }
               
                catch (System.ArgumentOutOfRangeException)
                {
                    car.bounce();
                }
            }

            Invalidate();
        }
    }
	public partial class formStart : Form
    {
        public formStart()
        {
            this.FormClosed += new FormClosedEventHandler(this.Form2_FormClosed);
            this.Width = 1024;
            this.Height = 768;
            Image myimage = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/startscherm.png"));
            this.BackgroundImage = myimage;

            Button btnStart = new Button();
            btnStart.Location = new Point(200, 600);
            btnStart.Text = "Start Game";
            btnStart.Width = 125;
            btnStart.Height = 75;
            btnStart.Click += new EventHandler(btnStart_Click);
            this.Controls.Add(btnStart);

            Button btnControls = new Button();
            btnControls.Location = new Point(470, 600);
            btnControls.Text = "Controls";
            btnControls.Width = 125;
            btnControls.Height = 75;
            btnControls.Click += new EventHandler(btnControls_Click);
            this.Controls.Add(btnControls);

            Button btnQuit = new Button();
            btnQuit.Location = new Point(740, 600);
            btnQuit.Text = "Quit Game";
            btnQuit.Width = 125;
            btnQuit.Height = 75;
            btnQuit.Click += new EventHandler(btnQuit_Click);
            this.Controls.Add(btnQuit);
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            (new FormRaceGame()).Show();this.Hide();
        }
        private void btnControls_Click(object sender, EventArgs e)
        {
            (new formControls()).Show(); this.Hide();
        }
        private void btnQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
    public partial class formControls : Form
    {
        public formControls()
        {
            this.FormClosed += new FormClosedEventHandler(this.Form3_FormClosed);
            this.Width = 1024;
            this.Height = 768;
            Image myimage = new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/controls.png"));
            this.BackgroundImage = myimage;

            Button btnStart2 = new Button();
            btnStart2.Location = new Point(312, 600);
            btnStart2.Text = "Start Game";
            btnStart2.Width = 125;
            btnStart2.Height = 75;
            btnStart2.Click += new EventHandler(btnStart2_Click);
            this.Controls.Add(btnStart2);

            Button btnControls2 = new Button();
            btnControls2.Location = new Point(612, 600);
            btnControls2.Text = "Quit Game";
            btnControls2.Width = 125;
            btnControls2.Height = 75;
            btnControls2.Click += new EventHandler(btnControls2_Click);
            this.Controls.Add(btnControls2);
        }
        private void btnStart2_Click(object sender, EventArgs e)
        {
            (new FormRaceGame()).Show(); this.Hide();
        }
        private void btnControls2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
