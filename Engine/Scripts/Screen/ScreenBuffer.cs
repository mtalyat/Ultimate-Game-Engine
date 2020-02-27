using System;
using System.Text;

namespace UltimateEngine{
	static class ScreenBuffer{
		private static char[][] data;

		public static string Title
		{
			get
			{
				return Console.Title;
			}
			set
			{
				Console.Title = value;
			}
		}
		public static Size Size { get; set; } = new Size(0, 0);
		public static bool Active { get; set; } = false;

		private static string clearString = "";

		static ScreenBuffer(){
			Initialize(new Size(0, 0));
		}

		public static void Initialize(Size s, string title = "Ultimate Game Engine"){
			Title = title;

			Console.Clear();//get rid of the warnings and info

			Size = s;

			data = new char[s.Height][];
			for(int i = 0; i < s.Height; i++){
				data[i] = new char[s.Width];
				for(int j = 0; j < s.Width; j++){
					data[i][j] = ' ';
				}
			}

			if(s.Width > 0 && s.Height > 0)
			{
				SetBufferToSize();
				SetClearString();
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
					//update the display for the current frame
					output += new string(data[i]) + "\n";

					//reset the display for the next frame
					data[i] = new string(' ', Size.Width).ToCharArray();
				}
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
			//the index used to access the array
			int top = Size.Height - 1 - y;

			//the string used to draw
			string str = o.ToString();

			//if not in view
			if(!IsInView(x, y, str.Length)){
				return;//do not copy
			}

			//copy from the string to the data array
			Array.Copy(str.ToCharArray(), 0, data[top], Math.Max(0, x), Math.Min(str.Length, data[top].Length - 1 - x));
		}

		public static void Draw(char[][] array, Size s, Point p){
			Draw(array, s, (int)p.X, (int)p.Y);
		}

		//draws a char array onto the screen
		public static void Draw(char[][] array, Size s, int x, int y){
			//adjust to whatever the offset is
			if(!IsInView(x, y, s.Width, s.Height)){
				return;//do not copy
			}

			for(int i = 0; i < s.Height; i++){
				if(i + y < 0 || i + y >= Size.Height) continue;

				int fromIndex = Math.Max(0, x * -1);
				int toIndex = Math.Max(0, x);

				Array.Copy(array[s.Height - 1 - i], fromIndex, data[y + i], toIndex, Math.Min(s.Width - fromIndex, Size.Width - 1 - toIndex));
			}
		}

		public static void SetColors(ConsoleColor fg, ConsoleColor bg)
		{
			//always change the foreground, it only shows when the screen is printed anyways
			Console.ForegroundColor = fg;

			//only change background when the color changes
			if(Console.BackgroundColor != bg)
			{
				Console.BackgroundColor = bg;
				//clearing shows changes but glitches screen when used in succession
				Clear();
			}
		}

		public static void Clear()
		{
			
			Console.SetCursorPosition(0, 0);
			Console.Write(clearString);
		}

		private static void SetClearString()
		{
			StringBuilder sb = new StringBuilder();
			string line = new string(' ', Size.Width + 1);
			for (int i = 0; i < Size.Height + 1; i++)
				sb.AppendLine(line);

			clearString = sb.ToString();
		}

		private static void SetBufferToSize()
		{
			Console.SetWindowSize(Size.Width + 1, Size.Height + 1);
			Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
		}

		//returns true if any part of a string will be in view when drawn
		private static bool IsInView(int x, int y, int xlength, int ylength = 1){
			return x + xlength >= 0 && x < Size.Width && y + ylength >= 0 && y < Size.Height;
		}
	}
}