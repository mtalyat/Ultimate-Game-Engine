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

		GameObject ground = new GameObject("Ground", new Image(new string('M', 200)));
		ground.AddComponent(new Collider());

		Player player = new Player("Player", new Image(new string[]{
			" (_) ",
			" -|- ",
			" / \\ ",
		}));

		InteractableGameObject test = new InteractableGameObject(img: new Image(new string[]
		{
			"TEST",
			"TEST",
			"TEST",
			"TEST"
		}));

		scene.Instantiate(ground, -100, 0);
		Player.Active = (Player)scene.Instantiate(player, 0, 1.5);
		scene.Instantiate(test, 30, 1);

		//Start the Scene
		scene.Run();

		//Ensure that the program does not end any time soon
		Thread.Sleep(int.MaxValue);
	}
}