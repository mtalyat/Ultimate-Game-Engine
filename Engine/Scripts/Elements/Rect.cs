using System;

namespace UltimateEngine{
	public class Rect {
		public Point Position { get; set; } = new Point(0, 0);
		public double X => Position.X;
		public double Y => Position.Y;

		public Size Size { get; set; } = new Size(0, 0);
		public int Width => Size.Width;
		public int Height => Size.Height;

		public double Top => Position.Y + Size.Height;
		public double Right => Position.X + Size.Width;
		public double Bottom => Position.Y;
		public double Left => Position.X;

		public Point BottomRight => new Point(Right, Bottom);
		public Point BottomLeft => new Point(Left, Bottom);
		public Point TopLeft => new Point(Left, Top);
		public Point TopRight => new Point(Right, Top);

		public Point Center
		{
			get
			{
				return new Point(CenterX, CenterY);
			}
			set
			{
				Position = new Point(value.X - Width / 2, value.Y - Height / 2);
			}
		}
		public double CenterX => Position.X + (double)Width / 2;
		public double CenterY => Position.Y + (double)Height / 2;

		public Rect(Point p, Size s){
			Position = p;
			Size = s;
		}

		public Rect(double x, double y, int w, int h){
			Position = new Point(x, y);
			Size = new Size(w, h);
		}

		//finds the side another rect is on compared to this rect
		public Direction FindSide(Rect other)
		{
			double w = (Width + other.Width) / 2;
			double h = (Height + other.Height) / 2;

			double dx = CenterX - other.CenterX;
			double dy = CenterY - other.CenterY;

			double wy = w * dy;
			double hx = h * dx;

			if (wy > hx)
			{
				if (wy > hx * -1)
				{//top
					return Direction.Down;
				}
				else
				{//left
					return Direction.Right;
				}
			}
			else
			{
				if (wy > hx * -1)
				{//right
					return Direction.Left;
				}
				else
				{//bottom
					return Direction.Up;
				}
			}

		}

		//checks for an intersection
		public bool Intersects(Rect other){
			return Left <= other.Right &&
				Right >= other.Left &&
				Top >= other.Bottom &&
				Bottom <= other.Top;
		}

		//checks if a Point is inside of the Rect
		public bool Contains(Point p)
		{
			return p.X >= Left && p.X <= Right && p.Y >= Bottom && p.Y <= Top;
		}

		//checks for an intersection when this is moving
		public Direction Crosses(Rect other, Point translation)
		{
			//get the before and after translation
			Rect before = this;
			Rect after = this + translation;

			Point center = before.Center + ((after.Center - before.Center) / 2);
			Point closest = other.Closest(center);

			Rect outerBounds = Rect.Combine(before, after);

			//check if the closest point is even in the area
			if(outerBounds.Contains(closest))
			{
				//then check if it is in the path
				int[] sides = new int[4]
				{
					new Line(before.BottomLeft, after.BottomLeft).FindSide(closest),
					new Line(before.TopLeft, after.TopLeft).FindSide(closest),
					new Line(before.TopRight, after.TopRight).FindSide(closest),
					new Line(before.BottomRight, after.BottomRight).FindSide(closest)
				};

				//check again if it is inside
				if(Math.Abs(sides[0] + sides[1] + sides[2] + sides[3]) < 4)
				{
					//for now
					return FindSide(other);
				}
			}

			return Direction.None;
		}

		//gets the closest Point on the paremeter to a Point
		public Point Closest(Point point)
		{
			return new Point(AdvMath.Clamp(point.X, Left, Right), AdvMath.Clamp(point.Y, Bottom, Top));
		}

		public double Paremeter()
		{
			return (Width * 2) + (Height * 2);
		}

		public double Area()
		{
			return (Width * Height);
		}

		public double Diagonal()
		{
			return Math.Sqrt(Math.Pow(Width, 2) + Math.Pow(Height, 2));
		}

		public Rect RoundPosition(int digits = 0)
		{
			return new Rect(Position.Round(digits), Size);
		}

		public static Rect Combine(Rect one, Rect two)
		{
			Point position = new Point(Math.Min(one.X, two.X), Math.Min(one.Y, two.Y));
			return new Rect(position,
				new Size(new Point(Math.Max(one.Right, two.Right), Math.Max(one.Top, two.Top)) - position));
		}

        #region Operators

        public static Rect operator +(Rect one, Point two){
			return new Rect(one.Position + two, one.Size);
		}

		public static Rect operator -(Rect one, Point two){
			return new Rect(one.Position - two, one.Size);
		}

		public static Rect operator +(Rect one, Size two)
		{
			return new Rect(one.Position, one.Size + two);
		}

		public static Rect operator -(Rect one, Size two)
		{
			return new Rect(one.Position, one.Size - two);
		}

        #endregion

        public override string ToString(){
			return $"({X}, {Y}, {Width}, {Height})";
		}
	}
}