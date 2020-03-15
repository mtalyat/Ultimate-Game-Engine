using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateEngine
{
    public static class AdvMath
    {
        public static double Round(double number, int digits = 2)
        {
            return Math.Round(number * Math.Pow(10.0, digits)) / Math.Pow(10.0, digits);
        }

        public static double[] QuadraticEquation(double a, double b, double c)
        {
            return new double[] { QEq(a, b, c, 1), QEq(a, b, c, -1) };
        }

        private static double QEq(double a, double b, double c, int f)
        {
            return ((b * -1) + Math.Sqrt(Math.Pow(b, 2) - 4 * a * c) * f) / (2 * a);
        }

        public static double FinalPositionEquation(double a, double vi, double xi, double t)
        {
            return (0.5 * a * Math.Pow(t, 2)) + (vi * t) + xi;
        }

        public static int Factorial(int number)
        {
            if (number <= 1) return 1;

            return Factorial(number - 1) * number;
        }

        public static double Clamp(double number, double min, double max)
        {
            return Math.Max(min, Math.Min(max, number));
        }

        public static Direction OppositeDirection(Direction dir)
        {
            if (dir == Direction.None) return dir;

            return (Direction)(((int)dir + 2) % 4);
        }
    }
}
