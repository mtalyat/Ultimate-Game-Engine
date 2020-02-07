using System;

namespace UltimateEngine{
	public class Rect {
		public Point Position { get; set; }
		public double X => Position.X;
		public double Y => Position.Y;

		public Size Size { get; set; }
		public int Width => Size.Width;
		public int Height => Size.Height;

		public double Top => Position.Y + Size.Height;
		public double Right => Position.X + Size.Width;
		public double CenterX => Position.X + Size.Width / 2;
		public double CenterY => Position.Y + Size.Height / 2;

		public Rect(Point p, Size s){
			Position = p;
			Size = s;
		}

		public Rect(int x, int y, int w, int h){
			Position = new Point(x, y);
			Size = new Size(w, h);
		}

		//checks for an intersection, returns 0 for right, 1 = up, 2 = left, 3 = down
		//returns -1 for none
		public int Intersects(Rect other){
			double w = (Width + other.Width) / 2;
			double h = (Height + other.Height) / 2;

			double dx = CenterX - other.CenterX;
			double dy = CenterY - other.CenterY;

			if(Math.Abs(dx) <= w && Math.Abs(dy) <= h){//collision detected
				double wy = w * dy;
				double hx = h * dx;

				if(wy > hx){
					if(wy > hx * -1){//top
						return 1;
					} else {//left
						return 2;
					}
				} else {
					if(wy > hx * -1){//right
						return 0;
					} else {//bottom
						return 3;
					}
				}
			} else {//no collision
				return -1;
			}
		}

		public static Rect operator +(Rect one, Point two){
			return new Rect(one.Position + two, one.Size);
		}

		public static Rect operator -(Rect one, Point two){
			return new Rect(one.Position - two, one.Size);
		}

		public override string ToString(){
			return $"({X}, {Y}, {Width}, {Height})";
		}
	}
}