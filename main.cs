using System;
using System.Threading;
using UltimateEngine;
using UltimateEngine.UI;
using UltimateEngine.Basics;

class MainClass {
	public static void Main (string[] args) {
		//----------Scene set up
		Scene scene = new Scene("Scene 1");
		scene.DEBUG_MODE = true;
		scene.SLOW_MODE = false;
		scene.FRAMERATE_LIMIT = true;

		Ground ground = new Ground("M   .:X###", 20, ".'*", 4, new Image[]{
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
		}, 1);

		Player player = new Player();
		player.Animator.SetTransparency(true);

		ground = (Ground)scene.Instantiate(ground, -ground.Bounds.CenterX, 0);
		player = (Player)scene.Instantiate(player, 0, 10);
		test = (MovingPlatform)scene.Instantiate(test);

		//Start the Scene
		scene.Run();

		//Ensure that the program does not end any time soon
		Thread.Sleep(int.MaxValue);
	}
}