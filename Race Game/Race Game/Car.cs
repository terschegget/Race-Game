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
        public Point position;
        public Vector2D rotationPoint;
        private float rotation;
        public double speed;
        private Keys leftKey, rightKey, upKey, downKey;
        private bool leftPressed = false, rightPressed = false, upPressed = false, downPressed = false;
        private String image;
        private float tank;
        public bool isColliding = false;

        private CollisionBox boxCollider;

        public Car(int positionX, int positionY, float rotation, Vector2D rotationPoint, double speed, float tank, Keys leftKey, Keys rightKey, Keys upKey, Keys downKey, String image)
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
            boxCollider = new CollisionBox(new Size(56, 26) ,getImage().Width, getImage().Height, 0, position, rotationPoint);
            this.rotationPoint = rotationPoint;

        }

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

        public Point getPosition()
        {
            return position;
        }

        public Image getImage()
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

        public CollisionBox getCollider()
        {
            return boxCollider;
        }

        private void accelerate()
        {
            speed = speed + .1;

            if (speed >= 5.0)
                speed = 5.0;
            emtyTank();
        }

        public void bounce()
        {
            speed = -speed;
        }

        private void brake()
        {
            speed = speed - .1;

            if (speed <= -2.0)
                speed = -2.0;
            emtyTank();
        }

        public void coast()
        {
            if (speed >= .02)
                speed -= .05;
            else if (speed <= -.02)
                speed += 0.05;
            else
                speed = 0;
        }

        private void emtyTank()
        {
            if(speed < 0)
                tank += (float)Math.Sin(0.1f * Math.PI * speed);
            if(speed > 0)
                tank -= (float)Math.Sin(0.1f * Math.PI * speed);
        }

        private void rotateRight()
        {
            if (speed != 0)
            {
                this.rotation += .07f;
            }
        }

        private void rotateLeft()
        {
            if (speed != 0)
            {
                this.rotation -= .07f;
            }
        }

        private void changeSpeed()
        {
            if (!isColliding)
            {
                if (upPressed && tank > 0)
                    accelerate();
                else if (downPressed && tank > 0)
                    brake();
                else
                    coast();
            }
            if (leftPressed)
                rotateLeft();
            else if (rightPressed)
                rotateRight();
        }

        public bool circlesColliding(int x1, int y1, int radius1, int x2, int y2, int radius2)
        {
            //compare the distance to combined radii
            int dx = x2 - x1;
            int dy = y2 - y1;
            int radii = radius1 + radius2;
            if ((dx * dx) + (dy * dy) < radii * radii)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        // Calculates the new position for the car
        public void calculateNewPosition()
        {
            changeSpeed();

            boxCollider.addPosition(position);
            boxCollider.addRoation(rotation);
            boxCollider.calculateBox();
            
            position.X += (int)Math.Round(speed * Math.Cos(rotation)); //pure magic here!
            position.Y += (int)Math.Round(speed * Math.Sin(rotation)); //more magic here

        }
    }
}
