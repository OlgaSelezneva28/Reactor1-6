using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example1_DZ
{
    class MyPoint
    {
        public double x, y;

        public MyPoint()
        {
        }

        public MyPoint(float X, float Y)
        {
            x = (double)X;
            y = (double)Y;
        }

        public MyPoint(int X, int Y)
        {
            x = (double)X;
            y = (double)Y;
        }

        public MyPoint(Double X, Double Y)
        {
            x = (double)X;
            y = (double)Y;
        }

        public MyPoint(String X, String Y)
        {
            x = Convert.ToDouble(X);
            y = Convert.ToDouble(Y);
        }

        public string X()
        {
            String sx = Convert.ToString(x);
            return sx; 
        }

        public string Y()
        {
            String sy = Convert.ToString(y);
            return sy;
        }
    }
}
