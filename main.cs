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
		scene.SLOW_MODE = false;

		Ground ground = new Ground("M   .:X###", 100, ".'*", 4, new Image[]{
			new Image(new string[]
			{
				" _ ",
				"{O}",
				"c| ",
				" | "
			}),
			new Image(new string[]
			{
				" __ ",
				"() )",
				"( ()",
				" || ",
				" || "
			})
		}, 4);

		Player player = new Player();

		scene.Instantiate(ground, -ground.Bounds.CenterX, 0);
		scene.Instantiate(player, 0, 10);

		//Start the Scene
		scene.Run();

		//Ensure that the program does not end any time soon
		Thread.Sleep(int.MaxValue);
	}
}