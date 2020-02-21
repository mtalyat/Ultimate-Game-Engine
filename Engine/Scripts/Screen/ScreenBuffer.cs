using System;
using System.Threading;

namespace UltimateEngine{
	static class ScreenBuffer{
		private static char[][] data;

		public static Size Size { get; set; } = new Size(0, 0);
		public static Point Offset { get; set; } = new Point(0, 0);
		public static bool Active { get; set; } = false;

		static ScreenBuffer(){
			Initialize(new Size(0, 0));
		}

		public static void Initialize(Size s){
			Console.Clear();//get rid of the warnings and info

			Size = s;

			data = new char[s.Height][];
			for(int i = 0; i < s.Height; i++){
				data[i] = new char[s.Width];
				for(int j = 0; j < s.Width; j++){
					data[i][j] = ' ';
				}
			}
		}

		//prints and resets the data array
		public static void Print(){
			string output = "";

			//need a Camera to display stuff
			if(Camera.MainCamera != null)
			{
				for (int i = Size.Height - 1; i >= 0; i--)
				{
					output += new string(data[i]) + "\n";
					data[i] = new string(' ', Size.Width).ToCharArray();
				}
				Offset = Camera.MainCamera.Position;
			} else
			{
				output = "No Main Camera found!";
			}

			Console.SetCursorPosition(0, 0);
			Console.Write(output);
		}

		//resizes the buffer
		public static void Resize(Size newSize){
			Array.Resize(ref data, newSize.Height);
			for(int i = 0; i < newSize.Width; i++){
				Array.Resize(ref data[i], newSize.Width);
			}

			Size = newSize;
		}

		public static void Draw(object o, Point position){
			Draw(o, (int)position.X, (int)position.Y);
		}

		//draws an object as a string at a given position
		public static void Draw(object o, int x, int y){
			int xx = x - (int)Offset.X;
			int yy = y - (int)Offset.Y;

			//the index used to access the array
			int top = Size.Height - 1 - yy;

			//the string used to draw
			string str = o.ToString();

			//if not in view
			if(!IsInView(xx, yy, str.Length)){
				return;//do not copy
			}

			//copy from the string to the data array
			Array.Copy(str.ToCharArray(), 0, data[top], Math.Max(0, xx), Math.Min(str.Length, data[top].Length - 1 - xx));
		}

		public static void Draw(char[][] array, Size s, Point p){
			Draw(array, s, (int)p.X, (int)p.Y);
		}

		//draws a char array onto the screen
		public static void Draw(char[][] array, Size s, int x, int y){
			//adjust to whatever the offset is
			int xx = x - (int)Offset.X;
			int yy = y - (int)Offset.Y;

			if(!IsInView(xx, yy, s.Width, s.Height)){
				return;//do not copy
			}

			for(int i = 0; i < s.Height; i++){
				if(i + yy < 0 || i + yy >= Size.Height) continue;

				int fromIndex = Math.Max(0, xx * -1);
				int toIndex = Math.Max(0, xx);

				Array.Copy(array[s.Height - 1 - i], fromIndex, data[yy + i], toIndex, Math.Min(s.Width - fromIndex, Size.Width - 1 - toIndex));
			}
		}

		//returns true if any part of a string will be in view when drawn
		private static bool IsInView(int x, int y, int xlength, int ylength = 1){
			return x + xlength >= 0 && x < Size.Width && y + ylength >= 0 && y < Size.Height;
		}
	}
}