using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;

namespace UltimateEngine{
	static class Input {
		public static string PauseKeyName { get; set; } = "Escape";
		public static string ExitKeyName { get; set; } = "Enter";
		private static string lookingForKey = "";
		private static Action onFound;

		public static bool Active { get; set; } = false;
		public static List<ScreenKey> Keys { get; private set; } = new List<ScreenKey>();

		private static Thread keyInputThread;
		private static Thread keyEventManagerThread;

		private static Stopwatch watch = new Stopwatch();

		//the amount of frames it waits after the initial key press
		private const int KEYDOWNFRESH = 16;
		//the amount of frames it waits after the first key press
		private const int KEYDOWNREFRESH = 3;

		public static void Start(){
			if (Active) return;

			Active = true;

			//set up the thread that gets input from the user
			keyInputThread = new Thread(new ThreadStart(() => {

				while (Active)
				{
					//wait until a key is pressed
					while (!Console.KeyAvailable)
					{
						Thread.Sleep(1);
					}

					//key has been pressed...
					ConsoleKeyInfo info = Console.ReadKey(true);
					string name = info.Key.ToString();
					ScreenKey key = new ScreenKey(name, info.KeyChar);

					AddKey(key);
				}
			}));
			
			//set up the thread that managers events
			keyEventManagerThread = new Thread(new ThreadStart(() => {
				while(Active){
					watch.Start();
					for(int i = Keys.Count - 1; i >= 0; i--){
						if(--Keys[i].KeyDownState <= 0){
							RemoveKey(Keys[i]);
						}
					}
					watch.Stop();
					Thread.Sleep(Math.Max(0, 30 - watch.Elapsed.Milliseconds));
					watch.Reset();
				}
			}));

			//start both of the threads
			keyInputThread.Name = "KeyInput";
			keyInputThread.Start();

			keyEventManagerThread.Name = "KeyEventManager";
			keyEventManagerThread.Start();
		}

		public static void Stop(){
			Active = false;
			keyInputThread = null;
			keyEventManagerThread = null;
		}

		//adds a key to Keys, returns true if the key has not been pressed recently
		private static bool AddKey(ScreenKey key){
			int index = Keys.FindIndex(k => k.Name == key.Name);

			if(index >= 0 && index < Keys.Count){//the key has been pressed before
				Keys[index].KeyDownState = KEYDOWNREFRESH;
				return false;
			} else {//'new' key
				//check for if looking for specific key
				if(lookingForKey != "")
				{
					if(lookingForKey == key.Name)
					{
						//found the key, run the action and turn input back to normal
						lookingForKey = "";

						onFound();
					}
				} else
				{   //not looking for any key so just do normal input
					key.KeyDownState = KEYDOWNFRESH;
					Keys.Add(key);

					//check to pause
					if (Scene.Current != null)
					{
						if (key.Name == PauseKeyName)
						{
							//toggle the pause
							if (Scene.Current.PAUSED)
								Resume();
							else
								Pause();
						}
						else if (key.Name == ExitKeyName && Scene.Current.PAUSED)
						{
							Resume();
							Scene.Current.Stop();
							Environment.Exit(0);
						}
					}
				}

				return true;
			}
		}

		//removes a key
		private static void RemoveKey(ScreenKey key){
			Keys.Remove(key);
		}

		#region Key Events

		//returns true if a key was just pressed
		public static bool IsKeyDown(string name){
			int index = Keys.FindIndex(k => k.Name == name);

			if(index >= 0){
				if(Keys[index].KeyDownState >= KEYDOWNFRESH - 1){
					return true;
				}
			}//else
			return false;
		}

		//returns true when a key is held
		public static bool IsKeyPressed(string name){
			return Keys.FindIndex(k => k.Name == name) >= 0;
		}

		//returns true when a key is let go
		public static bool IsKeyUp(string name){
			int index = Keys.FindIndex(k => k.Name == name);

			if(index >= 0){
				if(Keys[index].KeyDownState <= 1){
					return true;
				}
			}//else
			return false;
		}

		public static void WaitForKey(string name, Action onInput)
		{
			lookingForKey = name;
			onFound = onInput;
		}

        #endregion

        private static void Pause()
		{
			ScreenBuffer.ClearAfterPrint = false;
			Thread.Sleep(10);

			Scene.Current.PAUSED = true;
			Thread.Sleep(10);

			int width = 26 + Math.Max(PauseKeyName.Length, ExitKeyName.Length);

			//display pause and options
			ScreenBuffer.Draw(Words.StringArrayToJaggedCharArray(Words.Fill(new string[]
			{
				"                                       ",
				" PAUSED                                ",
				"                                       ",
			   $" PRESS '{PauseKeyName}' TO RESUME.     ",
			    "                                       ",
			   $" PRESS '{ExitKeyName}' TO END PROGRAM. ",
			    "                                       "
			}, ' ', width)), new Size(width, 7), 0, ScreenBuffer.Size.Height - 7);

			ScreenBuffer.Print();
		}

		private static void Resume()
		{
			ScreenBuffer.ClearAfterPrint = true;
			Scene.Current.PAUSED = false;
		}
	}
}