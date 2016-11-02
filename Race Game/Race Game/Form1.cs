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

            Car car1 = new Car(500, 200, 0, new Vector2D(32, 32), 0, 100000000, Keys.Left, Keys.Right, Keys.Up, Keys.Down, "blueCar.png");
            Car car2 = new Car(64, 512, 0, new Vector2D(32, 32), 0, 1000, Keys.A, Keys.D, Keys.W, Keys.S, "redCar.png");

            cars.Add(car1);
            cars.Add(car2);

            this.SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.DoubleBuffer, true);

            this.ResizeEnd += new EventHandler(Form1_CreateBackBuffer);
            this.Load += new EventHandler(Form1_CreateBackBuffer);
            this.Paint += new PaintEventHandler(Form1_Paint);

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

        void Form1_Paint(object sender, PaintEventArgs e) {
            if (Backbuffer != null) {
                e.Graphics.DrawImageUnscaled(Backbuffer, Point.Empty);
            }

            Draw(e.Graphics);
        }

        void Form1_CreateBackBuffer(object sender, EventArgs e) {
            if (Backbuffer != null)
                Backbuffer.Dispose();

            Backbuffer = new Bitmap(ClientSize.Width, ClientSize.Height);
        }
        
        void Draw(Graphics g) {
            Brush aBrush = (Brush)Brushes.YellowGreen;

            foreach (Car car in cars)
            {
                g.TranslateTransform(car.getPosition().X + (int)car.rotationPoint.X, car.getPosition().Y + (int)car.rotationPoint.Y);
                g.RotateTransform(car.getRotation() * (float)(180.0 / Math.PI) + 90);
                g.TranslateTransform(-car.getPosition().X - 31.5f, -car.getPosition().Y - 31.5f);
                
                g.DrawImage(car.getImage(), car.getPosition());

                g.ResetTransform();
            }


            g.FillRectangle(aBrush, (int)cars[0].getCollider().getUL().X, (int)cars[0].getCollider().getUL().Y, 4, 4);
            g.FillRectangle(aBrush, (int)cars[0].getCollider().getUR().X, (int)cars[0].getCollider().getUR().Y, 4, 4);
            g.FillRectangle(aBrush, (int)cars[0].getCollider().getDL().X, (int)cars[0].getCollider().getDL().Y, 4, 4);
            g.FillRectangle(aBrush, (int)cars[0].getCollider().getDR().X, (int)cars[0].getCollider().getDR().Y, 4, 4);

            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 13);
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            g.DrawString("Colision x: " + cars[0].getCollider().objectPosition.X + " y: " + cars[0].getCollider().objectPosition.Y, drawFont, drawBrush, 10, 10);
            g.DrawString("Colision top left x: " + cars[0].getCollider().getUL().X + " y: " + cars[0].getCollider().getUL().Y, drawFont, drawBrush, 10, 50);
            g.DrawString("Colision top right x: " + cars[0].getCollider().getUR().X + " y: " + cars[0].getCollider().getUR().Y, drawFont, drawBrush, 10, 75);
            g.DrawString("Colision bottom left x: " + cars[0].getCollider().getDL().X + " y: " + cars[0].getCollider().getDL().Y, drawFont, drawBrush, 10, 100);
            g.DrawString("Colision bottom right x: " + cars[0].getCollider().getDR().X + " y: " + cars[0].getCollider().getDR().Y, drawFont, drawBrush, 10, 125);

            g.DrawString("Car x: " + cars[0].getPosition().X + " y: " + cars[0].getPosition().Y, drawFont, drawBrush, 10, 25);
            
        }
        //collision funcies
        private bool checkCollision(CollisionBox obj1, CollisionBox obj2)
        {
            Vector2D axis1 = new Vector2D(0, 0);
            Vector2D axis2 = new Vector2D(0, 0);
            Vector2D axis3 = new Vector2D(0, 0);
            Vector2D axis4 = new Vector2D(0, 0);

            //het bereken van de assen voor de collision berekeninen
            axis1.X = obj1.getUR().X - obj1.getUL().X;
            axis1.Y = obj1.getUR().Y - obj1.getUL().Y;
            axis2.X = obj1.getUR().X - obj1.getDR().X;
            axis2.Y = obj1.getUR().Y - obj1.getDR().Y;
            axis3.X = obj2.getUL().X - obj2.getDL().X;
            axis3.Y = obj2.getUL().Y - obj2.getDL().Y;
            axis4.X = obj2.getUL().X - obj2.getUR().X;
            axis4.Y = obj2.getUL().Y - obj2.getUR().Y;

            if(matrixProject(axis1, obj1, obj2))
            {
                if (matrixProject(axis2, obj1, obj2))
                {
                    if (matrixProject(axis3, obj1, obj2))
                    {
                        if (matrixProject(axis4, obj1, obj2))
                        {
                            return true;
                        }
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            else
                return false;
        }

        private bool matrixProject(Vector2D axis, CollisionBox obj1, CollisionBox obj2)
        {
            //object 1
            List<Vector2D> vectorPointsObj1 = calculateVectorPoints(axis, obj1);
            List<Double> scalarObj1 = calculateVectorScalar(axis, vectorPointsObj1);
            double obj1Min = scalarObj1.Min();
            double obj1Max = scalarObj1.Max();

            //object 2
            List<Vector2D> vectorPointsObj2 = calculateVectorPoints(axis, obj2);
            List<Double> scalarObj2 = calculateVectorScalar(axis, vectorPointsObj2);
            double obj2Min = scalarObj2.Min();
            double obj2Max = scalarObj2.Max();

            if (obj1Max < obj2Min)
            {
                return false;
            }
            else if (obj1Min > obj2Max)
            {
                return false;
            }
            else
                return true;
        }

        private List<Vector2D> calculateVectorPoints(Vector2D axis, CollisionBox obj)
        {
            double calculationNumber;
            List<Vector2D> pointsObj = new List<Vector2D>();
            calculationNumber = (obj.getUR().X * axis.X + obj.getUR().Y) / (axis.X * axis.X + axis.Y + axis.Y);
            pointsObj.Add(new Vector2D(calculationNumber * axis.X, calculationNumber * axis.Y));

            calculationNumber = (obj.getUL().X * axis.X + obj.getUL().Y) / (axis.X * axis.X + axis.Y + axis.Y);
            pointsObj.Add(new Vector2D(calculationNumber * axis.X, calculationNumber * axis.Y));

            calculationNumber = (obj.getDR().X * axis.X + obj.getDR().Y) / (axis.X * axis.X + axis.Y + axis.Y);
            pointsObj.Add(new Vector2D(calculationNumber * axis.X, calculationNumber * axis.Y));

            calculationNumber = (obj.getDL().X * axis.X + obj.getDL().Y) / (axis.X * axis.X + axis.Y + axis.Y);
            pointsObj.Add(new Vector2D(calculationNumber * axis.X, calculationNumber * axis.Y));

            return pointsObj;
        }

        private List<Double> calculateVectorScalar(Vector2D axis, List<Vector2D> points)
        {
            List<double> scalarObj = new List<double>();

            for(int i = 0; i < points.Count; i++)
            {
                scalarObj.Add(points[i].X * axis.X + points[i].Y * axis.Y);
            }

            return scalarObj;
        }

        private void timerGameTicks_Tick(object sender, EventArgs e) {
            
            if (checkCollision(cars[0].getCollider(), cars[1].getCollider()))
            {
                Console.WriteLine("colliding");
                
            }
            else
            {
                Console.WriteLine("not colliding");
            }
            
            foreach (Car car in cars)
                car.calculateNewPosition();

            Invalidate();
        }
    }
}
