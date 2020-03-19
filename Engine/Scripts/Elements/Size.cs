using System;

namespace UltimateEngine{
	[Serializable]
	public struct Size {
		public int Width { get; set; }
		public int Height { get; set; }

		public Size(int w, int h){
			Width	= w;
			Height = h;
		}

		public Size(Point p){
			Width = (int)p.X;
			Height = (int)p.Y;
		}

		#region Operators

		public static Size operator+(Size one, Size two)
		{
			return new Size(Math.Abs(one.Width + two.Width), Math.Abs(one.Height + two.Height));
		}

		public static Size operator +(Size one, int two)
		{
			return new Size(Math.Abs(one.Width + two), Math.Abs(one.Height + two));
		}

		public static Size operator-(Size one, Size two)
		{
			return new Size(Math.Abs(one.Width - two.Width), Math.Abs(one.Height - two.Height));
		}

		public static Size operator*(Size one, Size two)
		{
			return new Size(one.Width * two.Width, one.Height * two.Height);
		}

		public static Size operator *(Size one, int two)
		{
			return new Size(one.Width * two, one.Height * two);
		}

		public static Size operator/(Size one, Size two)
		{
			return new Size(one.Width / two.Width, one.Height / two.Height);
		}

		public static Size operator %(Size one, Size two)
		{
			return new Size(one.Width % two.Width, one.Height % two.Height);
		}

		public static Size operator %(Size one, int two)
		{
			return new Size(one.Width % two, one.Height % two);
		}

		public static bool operator==(Size one, Size two){
			return one.Equals(two);
		}

		public static bool operator!=(Size one, Size two){
			return !one.Equals(two);
		}

		#endregion

		#region Overrides

		public override bool Equals(object o){
			if(o is Size s){
				if(s.Width == Width && s.Height == Height){
					return true;
				}
			}

			return false;
		}

		public override string ToString(){
			return $"({Width}, {Height})";
		}

		public override int GetHashCode(){
			return Width ^ Height;
		}

		#endregion
	}
}