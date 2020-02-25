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

        #region Constructors

        public Point(double x, double y){
			X = x;
			Y = y;
		}

		public Point(Size s){
			X = s.Width;
			Y = s.Height;
		}

        #endregion

        #region Methods

		public Point Round()
		{
			return new Point(Math.Round(X), Math.Round(Y));
		}

		public Point Ceiling()
		{
			return new Point(Math.Ceiling(X), Math.Ceiling(Y));
		}

		public Point Floor()
		{
			return new Point(Math.Floor(X), Math.Floor(Y));
		}

		//Gets the magnitude of the Point
		public double Magnitude()
		{
			return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
		}

		//Gets the result of a Dot Product between two Points
		public static double DotProduct(Point one, Point two)
		{
			return (one.X * two.X) + (one.Y * two.Y);
		}

		//Finds the distance between two Points
		public static double Distance(Point one, Point two)
		{
			return Math.Sqrt(Math.Pow(one.X - two.X, 2) + Math.Pow(one.Y + two.Y, 2));
		}

		//Finds the angle in radians from the first Point to the second Point
		public static double Angle(Point one, Point two)
		{
			return Math.Acos(DegreesToRadians(DotProduct(one, two) / (one.Magnitude() * two.Magnitude())));
		}

		private static double RadiansToDegrees(double rad)
		{
			return rad * 180 / Math.PI;
		}

		private static double DegreesToRadians(double deg)
		{
			return deg * Math.PI / 180;
		}

        #endregion

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