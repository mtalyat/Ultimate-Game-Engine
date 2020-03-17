using System;
using System.Threading;
using UltimateEngine;
using UltimateEngine.Basics;

class MainClass {
	static MainClass(){
		Debug.Reset();
		Console.CursorVisible = false;
	}

	public static void Main (string[] args) {
		//----------Scene set up
		Scene scene = new Scene(100, 20, "Scene 1");
		scene.DEBUG_MODE = true;

		Ground ground = new Ground('M', 200);

		Player player = new Player();

		scene.Instantiate(ground, -100, 0);
		scene.Instantiate(player, 0, 1.5);

		//Start the Scene
		scene.Run();

		//Ensure that the program does not end any time soon
		Thread.Sleep(int.MaxValue);
	}
}