using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Race_Game
{
    class Car
    {
        private Point position;
        public float rotation;
        public double speed;
        private Keys leftKey, rightKey, upKey, downKey;
        public bool leftPressed = false, rightPressed = false, upPressed = false, downPressed = false;
        private String image;
        public float tank;
        public Boolean accelerating = false;
        public int nrOfCheckpoints = 0;
        public int nrOfLaps = 0;

        //Constructor
        public Car(int positionX, int positionY, float rotation, double speed, float tank, Keys leftKey, Keys rightKey, Keys upKey, Keys downKey, String image)
        {
            position.X = positionX;
            position.Y = positionY;
            this.speed = speed;
            this.tank = tank;
            this.rotation = rotation;
            this.leftKey = leftKey;
            this.rightKey = rightKey;
            this.upKey = upKey;
            this.downKey = downKey;
            this.image = image;
        }
        //Key handlers
        public void handleKeyDownEvent(KeyEventArgs keys)
        {
            if (leftKey == keys.KeyCode)
                leftPressed = true;
            if (rightKey == keys.KeyCode)
                rightPressed = true;
            if (upKey == keys.KeyCode)
                upPressed = true;
            if (downKey == keys.KeyCode)
                downPressed = true;
        }

        public void handleKeyUpEvent(KeyEventArgs keys)
        {
            if (leftKey == keys.KeyCode)
                leftPressed = false;
            if (rightKey == keys.KeyCode)
                rightPressed = false;
            if (upKey == keys.KeyCode)
                upPressed = false;
            if (downKey == keys.KeyCode)
                downPressed = false;
        }
        //get functies
        public Point getPosition()
        {
            return position;
        }

        public Image getImage()
        {
            return new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/" + image));
        }

        public Bitmap getBitmap()
        {
            return new Bitmap(Path.Combine(Environment.CurrentDirectory, "resources/sprites/" + image));
        }

        public float getRotation()
        {
            return rotation;
        }

        public double getSpeed()
        {
            return speed;
        }
        //laat de auto voor uitrijden
        private void accelerate()
        {
            speed = speed + .1;

            if (speed >= 3.0)
                speed = 3.0;
            emtyTank();
        }
        // remt en laat de auto achteruit rijden
        private void brake()
        {
            speed = speed - .1;

            if (speed <= -1.0)
                speed = -1.0;
            emtyTank();
        }
        //laat de auto doorvrijden na dat het omhoog word los gelaten
        private void coast()
        {
            if (speed >= .02)
                speed -= .05;
            else if (speed <= -.02)
                speed += 0.05;
            else
                speed = 0;
        }

        public void bounce()
        {
            speed = -speed *2;
        }
        //berekend de in houd van de tank
        private void emtyTank()
        {
            if(speed < 0)
                tank += (float)Math.Sin(0.1f * Math.PI * speed);
            if(speed > 0)
                tank -= (float)Math.Sin(0.1f * Math.PI * speed);
        }
        //laat de auto aneren kant op kan rijden en draien
        public void rotateRight()
        {
            if (speed != 0)
                this.rotation += .07f;
        }

        public void rotateLeft()
        {
            if (speed != 0)
                this.rotation -= .07f;
        }
        //veranderd de snelheid
        private void changeSpeed()
        {
            if (upPressed && tank > 0)
                accelerate();
            else if (downPressed && tank > 0)
                brake();
    
            else
                coast();

            if (leftPressed)
                rotateLeft();
            else if (rightPressed)
                rotateRight();
        }

        public Boolean notOnMap(Bitmap track, int x, int y)
        {
            Color color = track.GetPixel(x, y);
                
            //Als auto op grijs, wit, zwart of alle checkpoints rijdt
            if ((color.R == 64 && color.G == 64 && color.B == 64) || (color.R == 255 && color.G == 255 && color.B == 255) || (color.R == 0 && color.G == 0 && color.B == 0) || (color.R == 149 && color.G == 20 && color.B == 255) || (color.R == 25 && color.G == 167 && color.B == 255) || (color.R == 255 && color.G == 240 && color.B == 40))
            {
                return false;
            }
            else return true;   
        }

        public Boolean inPitstop(Bitmap track, int x, int y)
        {
            Color color = track.GetPixel(x, y);

            //Als auto op het donkere pitstop hutje rijdt
            if (color.R == 95 && color.G == 57 && color.B == 32)
            {
                return true;
            }
            else return false;
        }

        //Houdt bij hoeveel checkpoints je geraakt hebt en hoeveel rondes gereden
        public void checkpointsHit(Bitmap track, int x, int y)
        {
            Color color = track.GetPixel(x, y);

            if (color.R == 149 && color.G == 20 && color.B == 255 && nrOfCheckpoints == 0)
            {
                nrOfCheckpoints = 1;
                Console.WriteLine(nrOfCheckpoints);
            }

            if (color.R == 165 && color.G == 198 && color.B == 255 && nrOfCheckpoints == 1)
            {
                nrOfCheckpoints = 2;
                Console.WriteLine(nrOfCheckpoints);
            }

            if (color.R == 25 && color.G == 167 && color.B == 255 && nrOfCheckpoints == 2)
            {
                nrOfCheckpoints = 3;
                Console.WriteLine(nrOfCheckpoints);
            }

            if (color.R == 255 && color.G == 240 && color.B == 40 && nrOfCheckpoints == 3)
            {
                nrOfCheckpoints = 4;
                Console.WriteLine(nrOfCheckpoints);
            }

            if ((color.R == 255 && color.G == 255 && color.B == 255 && nrOfCheckpoints == 4) || (color.R == 95 && color.G == 57 && color.B == 32 && nrOfCheckpoints == 4))
            {
                nrOfLaps += 1;
                nrOfCheckpoints = 0;
                Console.WriteLine(nrOfCheckpoints);
                Console.WriteLine(nrOfLaps);
            }
        }

        // Berekend de de nieuwe positie van de auto
        public void calculateNewPosition()
        {
            changeSpeed();

            position.X += (int)Math.Round(speed * Math.Cos(rotation));
            position.Y += (int)Math.Round(speed * Math.Sin(rotation)); 
        }
    }
}
