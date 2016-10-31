using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Race_Game
{
    class Colision
    {
        public void makeCarBounds(int width, int height, double rotation)
        {
            double angleRotation = rotation * (180.0 / Math.PI);

            Point pointA;
            pointA.X = (int)Math.Sin(angleRotation) * 30.9;
        }
    }
}
