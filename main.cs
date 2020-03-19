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
		}, 1);

		Player player = new Player();
		player.Animator.SetTransparency(true);

		MessageBox test = new MessageBox("This is a test message. Testing, 1, 2, 3, can anybody hear me? Shed the irony, it's really gonna feel me. Something something.. just like me.. something something.");

		ground = (Ground)scene.Instantiate(ground, -ground.Bounds.CenterX, 0);
		player = (Player)scene.Instantiate(player, 0, 10);
		test = (MessageBox)scene.Instantiate(test, player.Camera.Transform, new Point(1, 1));

		//Start the Scene
		scene.Run();

		Thread.Sleep(2000);
		test.Show();

		//Ensure that the program does not end any time soon
		Thread.Sleep(int.MaxValue);
	}
}