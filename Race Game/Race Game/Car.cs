using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Race_Game
{
    class Car
    {
        private Point position;
        private float rotation;
        private double speed;
        private Keys leftKey, rightKey, upKey, downKey;
        private Image image;

        public Car(int positionX, int positionY, float rotation, double speed, Keys leftKey, Keys rightKey, Keys upKey, Keys downKey, Image image)
        {
            position.X = positionX;
            position.Y = positionY;
            this.rotation = rotation;
            this.leftKey = leftKey;
            this.rightKey = rightKey;
            this.upKey = upKey;
            this.downKey = downKey;
            this.image = image;
        }

        public Car()
        {
        }
    }
}
