using System;
using System.Threading;
using UltimateEngine;
using Game;

class MainClass {
	static MainClass(){
		Debug.Reset();
		Console.CursorVisible = false;
	}

	public static void Main (string[] args) {
		Scene scene = new Scene(100, 20, "Scene 1");
		scene.DebugMode = true;

		Camera cam = new Camera();

		Player p = new Player();

		p.Body.Mass = 10;
		p.Collider.CoefficientOfFriction = 0.5;

		p.Speed = 5;
		p.JumpPower = 12;

		Animator animator = p.Animator;

		animator.Add(Animation.FromFile("C:\\Users\\Me\\source\\repos\\Ultimate-Game-Engine\\Game\\Animations\\Running.anim"));
		animator.Add(Animation.FromFile("C:\\Users\\Me\\source\\repos\\Ultimate-Game-Engine\\Game\\Animations\\Jumping.anim"));
		animator.Add(Animation.FromFile("C:\\Users\\Me\\source\\repos\\Ultimate-Game-Engine\\Game\\Animations\\Idle.anim"));

		GameObject ground = new GameObject("Ground", new Image(new string[] { new string('^', 200) }));
		ground.AddComponent(new Collider());
		ground.Tag = "Ground";

		GameObject box = new GameObject("Box", new Image(new string[]
		{
			"XXX", "XXX", "XXX", "XXX", "XXX", "XXX", "XXX", "XXX", "XXX", "XXX", "XXX", "XXX"
		}));
		//box.AddComponent(new PhysicsBody());
		//box.GetComponent<PhysicsBody>().IsKinematic = true;
		box.AddComponent(new Collider());
		box.Tag = "Ground";

		scene.Instantiate(p, new Point(2, 2));
		scene.Instantiate(ground, new Point(-50, 0));
		scene.Instantiate(box, new Point(20, 3));

		scene.Instantiate(cam);

		scene.Run();

		//Ensure that the program does not end any time soon
		Thread.Sleep(int.MaxValue);
	}
}