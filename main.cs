using System;
using System.Threading;
using UltimateEngine;
using UltimateEngine.UI;
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
		scene.FRAMERATE_LIMIT = true;

		Ground ground = new Ground("M   .:X###", 300, ".'*", 4, new Image[]{
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
			}),
			new Image(new string[]
			{
				"|", "|"
			}),
			new Image(new string[]
			{
				"|", "|"
			}),
			new Image(new string[]
			{
				"|", "|"
			}),
			new Image(new string[]
			{
				"|", "|"
			}),
			new Image(new string[]
			{
				"|", "|"
			}),
			new Image(new string[]
			{
				"|", "|"
			}),
			new Image(new string[]
			{
				"|"
			}),
			new Image(new string[]
			{
				"|"
			}),
			new Image(new string[]
			{
				"|"
			}),
			new Image(new string[]
			{
				"|"
			}),
			new Image(new string[]
			{
				"|"
			}),
			new Image(new string[]
			{
				"|"
			}),
			new Image(new string[]
			{
				"|"
			}),
			new Image(new string[]
			{
				"|"
			}),
			new Image(new string[]
			{
				"|"
			})
		}, 5);

		Player player = new Player();
		player.Animator.SetTransparency(true);

		//ground = (Ground)scene.Instantiate(ground, -ground.Bounds.CenterX, 0);
		//player = (Player)scene.Instantiate(player, 0, 10);

		//Start the Scene
		scene.Run();

		//Ensure that the program does not end any time soon
		Thread.Sleep(int.MaxValue);
	}
}