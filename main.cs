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

		//----------Player
		Player p = new Player();

		Animator animator = p.Animator;
		animator.Add(Animation.FromFile("C:\\Users\\Me\\source\\repos\\Ultimate-Game-Engine\\Game\\Animations\\Running.anim"));
		animator.Add(Animation.FromFile("C:\\Users\\Me\\source\\repos\\Ultimate-Game-Engine\\Game\\Animations\\Jumping.anim"));
		animator.Add(Animation.FromFile("C:\\Users\\Me\\source\\repos\\Ultimate-Game-Engine\\Game\\Animations\\Idle.anim"));

		//----------Ground
		GameObject ground = new GameObject("Ground", new Image(new string[] { new string('^', 300) }));
		ground.AddComponent(new Collider());
		ground.Tag = "Ground";

		//----------Immovable box
		Box immovableBox = new Box("Immovable Box", new Image(new string[]{
			"IMMOVABLE",
			"---BOX---",
			"---------"
		}));
		immovableBox.Body.IsKinematic = true;

		//----------Heavy box
		Box heavyBox = new Box("Heavy Box", new Image(new string[]
		{
			"HEAVY",
			"-BOX-",
			"-----"
		}));
		heavyBox.Body.Mass = 25;
		heavyBox.Collider.CoefficientOfFriction = 0.9;

		//----------Light box
		Box lightBox = new Box("Light Box", new Image(new string[]
		{
			"LIGHT",
			"-BOX-"
		}));
		lightBox.Body.Mass = 2;
		lightBox.Collider.CoefficientOfFriction = 0.2;

		//----------Collectable
		Collectable test = new Collectable("Test", new Image(new string[]
		{
			"Test"
		}));

		//----------Wall
		GameObject wall = new GameObject("Wall", new Image(new string[]
		{
			"WALLWALLWALL",
			"WALLWALLWALL",
			"WALLWALLWALL",
			"WALLWALLWALL",
			"WALLWALLWALL",
			"WALLWALLWALL"
		}));
		wall.Layer = 2;

		//add all the GameObjects to the Scene
		scene.Instantiate(p, 0, 1.5);
		scene.Instantiate(ground, -50, 0);
		scene.Instantiate(immovableBox, 20, 3);
		scene.Instantiate(heavyBox, 60, 1.5);
		scene.Instantiate(lightBox, 100, 1.5);
		scene.Instantiate(test, -20, 3);
		scene.Instantiate(wall, 140, 1);

		//Start the Scene
		scene.Run();

		//Ensure that the program does not end any time soon
		Thread.Sleep(int.MaxValue);
	}
}