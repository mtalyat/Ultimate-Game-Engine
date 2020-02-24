using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace UltimateEngine{
	public static class Debug{
		static string path = "";
		static Queue<string> toPrint = new Queue<string>();

		public static bool Active { get; set; } = false;

		static Debug(){
			path = Path.Combine(new string[] { Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName, "Engine", "Log" });
		}

		public static void Reset(){
			//clear log with each new session
			File.WriteAllText(path, $"LOG FOR {DateTime.Now}. Reminder: The log will reset each time the program is ran.\n");

			Active = true;
			Thread debugThread = new Thread(new ThreadStart(() =>
			{
				while (Active)
				{
					while(toPrint.Count > 0)
					{
						File.AppendAllText(path, toPrint.Dequeue());
					}
					Thread.Sleep(0);
				}
			}));

			debugThread.Name = "Debug";
			debugThread.Start();
		}

		public static void Log(object o){
			toPrint.Enqueue(o.ToString() + '\n');
		}

		public static void LogError(object o)
		{
			Log("[ERROR] " + o.ToString());
		}
	}
}