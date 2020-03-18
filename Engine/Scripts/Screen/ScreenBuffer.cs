using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace UltimateEngine{
	static class ScreenBuffer{
		//for fullscreen stuff
		[DllImport("kernel32.dll", ExactSpelling = true)]
		private static extern IntPtr GetConsoleWindow();
		private static IntPtr ThisConsole = GetConsoleWindow();
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		private const int HIDE = 0;
		private const int MAXIMIZE = 3;
		private const int MINIMIZE = 6;
		private const int RESTORE = 9;

		public static bool ClearAfterPrint { get; set; } = true;

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
			Console.CursorVisible = false;
			Initialize(new Size(100, 20));

			//skip for now
			//PlayIntro();
		}

		public static void Initialize(Size s, string title = "Ultimate Game Engine", bool fullScreen = false){
			if (fullScreen)
			{
				InitializeAtFullScreen(title);
				return;
			}

			Title = title;

			Console.Clear();//get rid of the warnings and info

			//fix the size so it is only even numbers
			Size = s - (s % 2);

			data = new char[Size.Height][];
			for(int i = 0; i < Size.Height; i++){
				data[i] = new char[Size.Width];
				for(int j = 0; j < Size.Width; j++){
					data[i][j] = ' ';
				}
			}

			if(Size.Width > 0 && Size.Height > 0)
			{
				SetBufferToSize();
				SetClearString();
			}
		}

		public static void InitializeAtFullScreen(string title = "Ultimate Game Engine")
		{
			Title = title;

			Console.Clear();//get rid of the warnings and info

			Size = new Size(Console.LargestWindowWidth - 1, Console.LargestWindowHeight - 1);
			Size -= Size % 2;

			data = new char[Size.Height][];
			for (int i = 0; i < Size.Height; i++)
			{
				data[i] = new char[Size.Width];
				for (int j = 0; j < Size.Width; j++)
				{
					data[i][j] = ' ';
				}
			}

			if (Size.Width > 0 && Size.Height > 0)
			{
				SetBufferToSize();
				SetClearString();

				//keep the CursorVisibility
				bool show = Console.CursorVisible;
				ShowWindow(ThisConsole, MAXIMIZE);
				Console.CursorVisible = show;
			}
		}

		//prints and resets the data array
		public static void Print(){
			string output = "";

			for (int i = Size.Height - 1; i >= 0; i--)
			{
				//update the display for the current frame
				output += new string(data[i]) + "\n";

				//reset the display for the next frame
				if (ClearAfterPrint) data[i] = new string(' ', Size.Width).ToCharArray();
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
		public static void Draw(char[][] array, Size s, int x, int y)
		{
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

		public static void TransparentDraw(char[][] array, Size s, Point p)
		{
			TransparentDraw(array, s, (int)p.X, (int)p.Y);
		}

		//draws a char array, but with transparency
		//MUCH SLOWER
		public static void TransparentDraw(char[][] array, Size s, int x, int y)
		{
			if (!IsInView(x, y, s.Width, s.Height))
			{
				return;//do not copy
			}

			//copy each letter character by character
			for (int i = 0; i < s.Height; i++)
			{
				if (i + y < 0 || i + y >= Size.Height) continue;

				int fromIndex = Math.Max(0, x * -1);
				int toIndex = Math.Max(0, x);

				for (int j = 0; j < Math.Min(s.Width - fromIndex, Size.Width - 1 - toIndex); j++)
				{
					char c = array[s.Height - 1 - i][fromIndex + j];
					if (!char.IsWhiteSpace(c))
						data[y + i][toIndex + j] = c;
				}
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

		//Clears the screen, without using Console.Clear()
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

		//Sets the WindowSize and BufferSize to the values of Size, + 1
		private static void SetBufferToSize()
		{
			Console.SetWindowSize(Math.Min(Size.Width + 1, Console.LargestWindowWidth),
				Math.Min(Size.Height + 1, Console.LargestWindowHeight));
			Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
		}

		//returns true if any part of a string will be in view when drawn
		private static bool IsInView(int x, int y, int xlength, int ylength = 1){
			return x + xlength >= 0 && x < Size.Width && y + ylength >= 0 && y < Size.Height;
		}

		private static void PlayIntro()
		{
			ClearAfterPrint = false;
			Random random = new Random();

			string[] lines = File.ReadAllLines(Path.Combine(new string[] { Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName, "Engine", "Scripts", "Screen", "Intro.txt" }));

			for(int y = 0; y < lines.Length; y++)
			{
				for(int x = 0; x < lines[y].Length; x++)
				{
					Draw(lines[y][x], x, y);
					Print();

					System.Threading.Thread.Sleep(1);
				}
			}

			System.Threading.Thread.Sleep(2000);

			ClearAfterPrint = true;
			Print();
		}
	}
}