using System;

namespace UltimateEngine{
	//a 2D point in space
	public struct Point {
		public static readonly Point Zero = new Point(0, 0);
		public static readonly Point Right = new Point(1, 0);
		public static readonly Point Left = new Point(-1, 0);
		public static readonly Point Up = new Point(0, 1);
		public static readonly Point Down = new Point(0, -1);

		public double X { get; set; }
		public double Y { get; set; }

		public Point(double x, double y){
			X = x;
			Y = y;
		}

		public Point(Size s){
			X = s.Width;
			Y = s.Height;
		}

		#region Operators

		public static Point operator+(Point one, Point two){
			return new Point(one.X + two.X, one.Y + two.Y);
		}

		public static Point operator-(Point one, Point two){
			return new Point(one.X - two.X, one.Y - two.Y);
		}

		public static Point operator*(Point one, Point two){
			return new Point(one.X * two.X, one.Y * two.Y);
		}

		public static Point operator*(Point one, double two){
			return new Point(one.X * two, one.Y * two);
		}

		public static Point operator/(Point one, Point two){
			return new Point(one.X / two.X, one.Y / two.Y);
		}

		public static bool operator==(Point one, Point two){
			return one.Equals(two);
		}

		public static bool operator!=(Point one, Point two){
			return !one.Equals(two);
		}

		#endregion

		#region Overrides

		public override bool Equals(object o){
			if(o is Point p){
				if(p.X == X && p.Y == Y){
					return true;
				}
			}

			return false;
		}

		public override int GetHashCode(){
			return base.GetHashCode();
		}

		public override string ToString(){
			return $"({X}, {Y})";
		}

		#endregion
	}
}