using System;
using System.IO;

namespace UltimateEngine{
	public static class Debug{
		private static object monitor = new object();

		static string path = "";

		static Debug(){
			path = Path.Combine(new string[] { Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName, "Engine", "Log" });
		}

		public static void Reset(){
			lock (monitor)
			{
				//clear log with each new session
				File.WriteAllText(path, $"LOG FOR {DateTime.Now}. Reminder: The log will reset each time the program is ran.\n");
			}
		}

		public static void Log(object o){
			lock (monitor)
			{
				File.AppendAllText(path, o.ToString() + '\n');
			}
		}

		public static void LogError(object o)
		{
			Log("[ERROR] " + o.ToString());
		}
	}
}