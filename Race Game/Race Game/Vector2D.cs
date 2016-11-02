using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Race_Game
{
    class Vector2D
    {
        public double X;
        public double Y;

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point getAsPoint()
        {
            return new Point((int)Math.Round(X), (int)Math.Round(Y));
        }
    }
}
