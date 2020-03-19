using System;
using System.Threading;
using UltimateEngine;
using UltimateEngine.UI;
using UltimateEngine.Basics;
using UltimateEngine.Basics.Items;

class MainClass {
	public static void Main (string[] args) {
		//----------Scene set up
		Scene scene = new Scene("Scene 1");
		scene.DEBUG_MODE = false;
		scene.SLOW_MODE = false;
		scene.FRAMERATE_LIMIT = true;

		Ground ground = new Ground("M   .:X#############", 100, ".'*", 4, new Image[]{
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

		Box box = new Box(img: new Image(new string[]
		{
			"[][][][][][]",
			"|\\||/||\\||/|",
			"|/||\\||/||\\|",
			"[][][][][][]"
		}));
		box.Body.Mass = 20;
		box.Collider.CoefficientOfFriction = 0.1;

		Sign sign = new Sign("This is a sign.", new Image(new string[]
		{
			" _______ ",
			"| XXxXx |",
			"|_xXxXX_|",
			"   | |   ",
			"   |_|   "
		}));
		sign.Transform.Z = -0.5;

		Coin coin = new Coin(img: new Image("(c)"));

		MovingPlatform plat = new MovingPlatform('^', 10, new Point(0, 20), 2, 100);

		Text scoreDisplay = new Text(coin, "Score: {0}");

		scene.Instantiate(ground, -20, 0);
		scene.Instantiate(plat, 70, 19);
		scene.Instantiate(coin, 20, 21);
		scene.Instantiate(coin, 25, 21);
		scene.Instantiate(coin, 30, 21);

		scene.Instantiate(ground, 80, 20);
		scene.Instantiate(box, 100, 41);
		scene.Instantiate(coin, 175, 46);

		scene.Instantiate(ground, 180, 30);
		scene.Instantiate(sign, 200, 50);

		scene.Instantiate(player, 0, 15);
		scene.Instantiate(scoreDisplay, 0, ScreenBuffer.Size.Height - 1);

		//Start the Scene
		scene.Run();

		//Ensure that the program does not end any time soon
		Thread.Sleep(int.MaxValue);
	}
}