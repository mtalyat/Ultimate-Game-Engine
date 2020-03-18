using System;

namespace UltimateEngine{
	//a 2D point in space
	[Serializable]
	public struct Point {

        #region Readonlys

        /// <summary>
        /// A Point representing (0, 0).
        /// </summary>
        public static readonly Point Zero = new Point(0, 0);
		/// <summary>
		/// A Point facing the Right (1, 0).
		/// </summary>
		public static readonly Point Right = new Point(1, 0);
		/// <summary>
		/// A Point facing the Left (-1, 0).
		/// </summary>
		public static readonly Point Left = new Point(-1, 0);
		/// <summary>
		/// A Point facing Up (0, 1).
		/// </summary>
		public static readonly Point Up = new Point(0, 1);
		/// <summary>
		/// A Point facing Down (0, -1).
		/// </summary>
		public static readonly Point Down = new Point(0, -1);

        #endregion

		/// <summary>
		/// The X coordinate.
		/// </summary>
        public double X { get; set; }
		/// <summary>
		/// The Y coordinate.
		/// </summary>
		public double Y { get; set; }

        #region Constructors

        public Point(double x, double y){
			X = x;
			Y = y;
		}

		/// <summary>
		/// Converts a Size to a Point.
		/// </summary>
		/// <param name="s"></param>
		public Point(Size s){
			X = s.Width;
			Y = s.Height;
		}

        #endregion

        #region Methods

        #region Math

		/// <summary>
		/// Rounds the Point to the nearest amount of Digits.
		/// </summary>
		/// <param name="digits">The degree the number will be rounded to.</param>
		/// <returns>A new Point with rounded values.</returns>
        public Point Round(int digits = 0)
		{
			return new Point(AdvMath.Round(X, digits), AdvMath.Round(Y, digits));
		}

		/// <summary>
		/// Rounds the Point values up to the next whole number.
		/// </summary>
		/// <returns>A new Point with the values rounded up.</returns>
		public Point Ceiling()
		{
			return new Point(Math.Ceiling(X), Math.Ceiling(Y));
		}

		/// <summary>
		/// Rounds the Point values down to the next whole number.
		/// </summary>
		/// <returns>A new Point with the values rounded down.</returns>
		public Point Floor()
		{
			return new Point(Math.Floor(X), Math.Floor(Y));
		}

		/// <summary>
		/// Gets the absolite value of the Point values.
		/// </summary>
		/// <returns>A new Point with the absolute values.</returns>
		public Point Abs()
		{
			return new Point(Math.Abs(X), Math.Abs(Y));
		}

		/// <summary>
		/// Gets the distance from the origin (0, 0).
		/// </summary>
		public double Magnitude()
		{
			return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
		}

		/// <summary>
		/// Gets the dot product of two Points.
		/// </summary>
		public static double DotProduct(Point one, Point two)
		{
			return (one.X * two.X) + (one.Y * two.Y);
		}

		/// <summary>
		/// Gets the distance between two Points.
		/// </summary>
		public static double Distance(Point one, Point two)
		{
			return Math.Sqrt(Math.Pow(one.X - two.X, 2) + Math.Pow(one.Y + two.Y, 2));
		}

		/// <summary>
		/// Gets the distance from this Point, to another Point.
		/// </summary>
		public double DistanceFrom(Point other)
		{
			return Distance(this, other);
		}

		/// <summary>
		/// Finds the angle from one Point to another Point.
		/// </summary>
		/// <param name="one">The Point that the angle is measured from.</param>
		/// <param name="two">The Point that the angle is measured to.</param>
		/// <returns>The angle from Point one to Point two, in radians.</returns>
		public static double Angle(Point one, Point two)
		{
			return Math.Acos(AdvMath.DegreesToRadians(DotProduct(one, two) / (one.Magnitude() * two.Magnitude())));
		}

		#endregion

		/// <summary>
		/// Checks if the Point is within a radius of another Point.
		/// </summary>
		/// <param name="center">The origin of the other Point.</param>
		/// <param name="radius">The radius of the other Point.</param>
		/// <returns>True if this Point is within the radius of the other Point.</returns>
		public bool IsInRange(Point center, double radius)
		{
			double result = Math.Sqrt(Math.Pow(X - center.X, 2) + Math.Pow(Y - center.Y, 2));
			return result < radius;
		}

		/// <summary>
		/// Checks if the Point is within a radii of another Point.
		/// </summary>
		/// <param name="center">The origin of the other Point.</param>
		/// <param name="radii">The radii of the other Point.</param>
		/// <returns>True if this Point is within the radii of the other Point.</returns>
		public bool IsInRange(Point center, Point radii)
		{
			return Math.Pow(X - center.X, 2) / Math.Pow(radii.X, 2) +
				Math.Pow(Y - center.Y, 2) / Math.Pow(radii.Y, 2) <= 1;
		}

		#endregion

        #region Operators

        public static Point operator+(Point one, Point two){
			return new Point(one.X + two.X, one.Y + two.Y);
		}

		/// <summary>
		/// Adds a double to both the X and Y values.
		/// </summary>
		public static Point operator +(Point one, double two)
		{
			return new Point(one.X + two, one.Y + two);
		}

		public static Point operator-(Point one, Point two){
			return new Point(one.X - two.X, one.Y - two.Y);
		}

		/// <summary>
		/// Subtracts a double to both the X and Y values.
		/// </summary>
		public static Point operator -(Point one, double two)
		{
			return new Point(one.X - two, one.Y - two);
		}

		public static Point operator*(Point one, Point two){
			return new Point(one.X * two.X, one.Y * two.Y);
		}

		/// <summary>
		/// Multiplies the X and Y values by a double.
		/// </summary>
		public static Point operator*(Point one, double two){
			return new Point(one.X * two, one.Y * two);
		}

		/// <summary>
		/// Multiplies the X and Y values by a double.
		/// </summary>
		public static Point operator*(double one, Point two)
		{
			return new Point(one * two.X, one * two.Y);
		}

		public static Point operator/(Point one, Point two){
			return new Point(one.X / two.X, one.Y / two.Y);
		}

		/// <summary>
		/// Divides the X and Y values by a double.
		/// </summary>
		public static Point operator/(Point one, double two)
		{
			return new Point(one.X / two, one.Y / two);
		}

		/// <summary>
		/// Divides the X and Y values by a double.
		/// </summary>
		public static Point operator /(double one, Point two)
		{
			return new Point(one / two.X, one / two.Y);
		}

		/// <summary>
		/// Compares a Point's magnitude to another Point's magnitude.
		/// </summary>
		public static bool operator >(Point one, Point two)
		{
			return one.Magnitude() > two.Magnitude();
		}

		/// <summary>
		/// Compares a Point's magnitude to another Point's magnitude.
		/// </summary>
		public static bool operator <(Point one, Point two)
		{
			return one.Magnitude() < two.Magnitude();
		}

		public static bool operator==(Point one, Point two){
			return one.Equals(two);
		}

		public static bool operator!=(Point one, Point two){
			return !one.Equals(two);
		}

		public static Point operator %(Point one, Point two)
		{
			return new Point(one.X % two.X, one.Y % two.Y);
		}

		public static Point operator %(Point one, double two)
		{
			return new Point(one.X % two, one.Y % two);
		}

		#endregion

		/// <summary>
		/// Checks if both values are numbers, and not infinite.
		/// </summary>
		/// <returns>True if both X and Y are a number and are not infinite.</returns>
		public bool IsValid()
		{
			return !(double.IsNaN(X + Y) || double.IsInfinity(X + Y));
		}

		/// <summary>
		/// Gets the direction of a Point representing velocity.
		/// </summary>
		/// <returns>A new Point with X and Y simplified to either 1, 0 or -1.</returns>
		public static Point GetDirection(Point velocity)
		{
			double x = 0;
			double y = 0;

			if(velocity.X > 0)
			{
				x = 1;
			} else if (velocity.X < 0)
			{
				x = -1;
			}

			if(velocity.Y > 0)
			{
				y = 1;
			} else if (velocity.Y < 0)
			{
				y = -1;
			}

			return new Point(x, y);
		}

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