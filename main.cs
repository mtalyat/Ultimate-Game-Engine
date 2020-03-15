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

		Player p = new Player();

		p.Body.Mass = 10;
		p.Collider.CoefficientOfFriction = 0.2;

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
			"IMMOVABLE",
			"-OBJECT--",
			"---------"
		}));
		box.AddComponent(new PhysicsBody());
		box.GetComponent<PhysicsBody>().IsKinematic = true;
		box.AddComponent(new Collider());
		box.Tag = "Ground";

		GameObject box2 = new GameObject("Box 2", new Image(new string[]
		{
			"MOVABLE",
			"--BOX--",
			"-------"
		}));
		PhysicsBody pb = new PhysicsBody();
		pb.Mass = 5;
		box2.AddComponent(pb);
		box2.AddComponent(new Collider());
		box2.Tag = "Ground";

		scene.Instantiate(p, new Point(0, 1.5));
		scene.Instantiate(ground, new Point(-50, 0));
		scene.Instantiate(box, new Point(-20, 3));
		scene.Instantiate(box2, new Point(20, 3));

		scene.Run();

		//Ensure that the program does not end any time soon
		Thread.Sleep(int.MaxValue);
	}
}