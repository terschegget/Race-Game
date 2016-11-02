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
        public Point deltaObjectPosition;
        private Vector2D rotationPoint;

        /*illustratie van de assen en de hoeken
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

        public CollisionBox(Size sizeBox ,int width, int height, double rotation, Point position, Vector2D rotationPoint) 
        {
            this.objectPosition = position;
            this.objectImageWidth = width;
            this.objectImageHeight = height;
            this.rotation = rotation;
            this.objectWidth = sizeBox.Width;
            this.objectHeight = sizeBox.Height;
            this.rotationPoint = rotationPoint;

            pointUpRight.X += objectPosition.X + objectWidth + (objectImageWidth - objectWidth) / 2;
            pointUpRight.Y += objectPosition.Y + (objectImageHeight - objectHeight) / 2;
            pointUpLeft.X += objectPosition.X + (objectImageWidth - objectWidth) / 2;
            pointUpLeft.Y += objectPosition.Y + (objectImageHeight - objectHeight) / 2;
            pointDownLeft.X += objectPosition.X + (objectImageWidth - objectWidth) / 2;
            pointDownLeft.Y += objectPosition.Y + objectHeight + (objectImageHeight - objectHeight) / 2;
            pointDownRight.X += objectPosition.X + objectWidth + (objectImageWidth - objectWidth) / 2;
            pointDownRight.Y += objectPosition.Y + objectHeight + (objectImageHeight - objectHeight) / 2;
        }

        public void calculateBox()
        {
            //het berekenen van de hoekken met de rotatities
            calculateNewPosition(pointUpRight, deltaRotation);
            calculateNewPosition(pointUpLeft, deltaRotation);
            calculateNewPosition(pointDownLeft, deltaRotation);
            calculateNewPosition(pointDownRight, deltaRotation);
        }

        private void calculateNewPosition(Vector2D position, double angle)
        {
            double tempX = position.X - (objectPosition.X + rotationPoint.X);
            double tempY = position.Y - (objectPosition.Y + rotationPoint.Y);

            double deltaX = tempX * Math.Cos(angle) - tempY * Math.Sin(angle);
            double deltaY = tempX * Math.Sin(angle) + tempY * Math.Cos(angle);

            position.X = deltaX + objectPosition.X + (deltaObjectPosition.X + rotationPoint.X);
            position.Y = deltaY + objectPosition.Y + (deltaObjectPosition.Y + rotationPoint.Y);
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
            deltaObjectPosition.X = newPost.X - objectPosition.X;
            deltaObjectPosition.Y = newPost.Y - objectPosition.Y;
            objectPosition = newPost;
        }

        public void addRoation(double newRotation)
        {
            double old = rotation;
            rotation = newRotation;
            deltaRotation = rotation - old;
        }

    }
}
