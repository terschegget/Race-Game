using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Race_Game
{
    class CollisionBox
    {
        //info over het object
        private int objectImageWidth;
        private int objectImageHeight;
        private int objectWidth;
        private int objectHeight;
        private double rotation;
        private double deltaRotation;
        public Point objectPosition;

        /*ilustratie van de assen en de hoeken
         *              ^
         *       axis 2 |
         *              |
         *          B-------A
         *          |       |
         * <--axis 1|       |------>
         *          |       |
         *          C-------D
         *              |
         *              |
         *              v
         */
        Vector2D pointUpRight = new Vector2D(0, 0);
        Vector2D pointUpLeft = new Vector2D(0, 0);
        Vector2D pointDownLeft = new Vector2D(0, 0);
        Vector2D pointDownRight = new Vector2D(0, 0);

        public CollisionBox(Size sizeBox ,int width, int height, double rotation, Point position) 
        {
            this.objectPosition = position;
            this.objectImageWidth = width;
            this.objectImageHeight = height;
            this.rotation = rotation;
            this.objectWidth = sizeBox.Width;
            this.objectHeight = sizeBox.Height;

            pointUpRight.X += objectPosition.X + objectImageWidth / 2;
            pointUpRight.Y += objectPosition.Y + (objectImageHeight - objectHeight) / 2;
            pointUpLeft.X += objectPosition.X - objectWidth + objectImageWidth / 2;
            pointUpLeft.Y += objectPosition.Y + (objectImageHeight - objectHeight) / 2;
            pointDownLeft.X += objectPosition.X - objectWidth + objectImageWidth / 2;
            pointDownLeft.Y += objectPosition.Y + objectHeight + (objectImageHeight - objectHeight) / 2;
            pointDownRight.X += objectPosition.X + objectImageWidth / 2;
            pointDownRight.Y += objectPosition.Y + objectHeight + (objectImageHeight - objectHeight) / 2;
        }

        public void calculateBox()
        {

            double r = Math.Sqrt(objectHeight/2 * objectHeight/2 + objectWidth/2 * objectWidth/2);

            //het berekenen van de hoekken met de rotatities
            /*
            pointUpRight.X = Math.Sin(rotation) * d;
            pointUpRight.Y = Math.Cos(rotation) * d;
            pointUpLeft.X = -Math.Sin(rotation) * d;
            pointUpLeft.Y = Math.Cos(rotation) * d;
            pointDownLeft.X = -Math.Sin(rotation) * d;
            pointDownLeft.Y = -Math.Cos(rotation) * d;
            pointDownRight.X = Math.Sin(rotation) * d;
            pointDownRight.Y = -Math.Cos(rotation) * d;
            */
            calculateNewPosition(pointUpRight, rotation);
            calculateNewPosition(pointUpLeft, rotation);
            calculateNewPosition(pointDownLeft, rotation);
            calculateNewPosition(pointDownRight, rotation);
            
            //het zetten van de posities van de x en y coordinaten van de hoeken door 
            /*
            pointUpRight.X += objectPosition.X + objectImageWidth / 2;
            pointUpRight.Y += objectPosition.Y + (objectImageHeight - objectHeight) / 2;
            pointUpLeft.X += objectPosition.X - objectWidth + objectImageWidth / 2;
            pointUpLeft.Y += objectPosition.Y + (objectImageHeight - objectHeight) / 2;
            pointDownLeft.X += objectPosition.X - objectWidth + objectImageWidth / 2;
            pointDownLeft.Y += objectPosition.Y + objectHeight + (objectImageHeight - objectHeight) / 2;
            pointDownRight.X += objectPosition.X + objectImageWidth / 2;
            pointDownRight.Y += objectPosition.Y + objectHeight + (objectImageHeight - objectHeight) / 2;
            */
        }

        private void calculateNewPosition(Vector2D position, double angle)
        {
            position.X = position.X * Math.Cos(angle) + position.Y * Math.Sin(angle);
            position.Y = position.X * Math.Sin(angle) - position.Y * Math.Cos(angle);
        }

        public Vector2D getUR()
        {
            return pointUpRight;
        }

        public Vector2D getUL()
        {
            return pointUpLeft;
        }

        public Vector2D getDL()
        {
            return pointDownLeft;
        }

        public Vector2D getDR()
        {
            return pointDownRight;
        }

        public void addPosition(Point newPost)
        {
            objectPosition = newPost;
        }

        public void addRoation(double newRotation)
        {
            double old = rotation;
            rotation = newRotation;
            deltaRotation = rotation = old;
        }

    }
}
