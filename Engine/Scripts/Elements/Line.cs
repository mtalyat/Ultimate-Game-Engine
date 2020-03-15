using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateEngine
{
    public class Line
    {
        public Point Start { get; set; }
        public Point End { get; set; }

        public double Slope => (End.Y - Start.Y) / (End.X - Start.X);

        public Line(double x1, double y1, double x2, double y2) : this(new Point(x1, y1), new Point(x2, y2))
        {

        }

        public Line(Point s, Point e)
        {
            Start = s;
            End = e;
        }

        public int FindSide(Point p)
        {
            double dx = p.X - GetX(p.Y);

            if (dx > 0) return 1;
            else if (dx < 0) return -1;
            else return 0;
        }

        //derived from the Point Slope formula
        public double GetY(double x)
        {
            return Slope * (x - Start.X) + Start.Y;
        }

        //derived from the Point Slope formula
        public double GetX(double y)
        {
            return (y - Start.Y) / Slope + Start.X;
        }
    }
}
