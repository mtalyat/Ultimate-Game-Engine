using System;
using System.IO;
using System.Threading;

using UltimateEngine;
using Game;

class MainClass {
	static MainClass(){
		Debug.Reset();
		Console.CursorVisible = false;
	}

  public static void Main (string[] args) {
		Scene scene = new Scene(new Size(100, 20));
		scene.DebugMode = true;

		GameObject ground = new GameObject("Ground", new Image(new string('^', 100)));
		ground.Tag = "Ground";
		ground.AddComponent(new Collider());

		GameObject box = new GameObject("Box", new Image(new string[]{
			" ___ ",
			"|   |",
			"|   |",
			"|___|"
		}));
		box.Tag = "Ground";
		box.AddComponent(new Collider());

		GameObject platform = new GameObject("Platform", new Image(new string('"', 20)));
		platform.Tag = "Ground";
		platform.AddComponent(new Collider());

		
		GameObject go = new Player();

		Animator a = new Animator(70);
		a.Add(Animation.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "Game/Animations/Running.anim")));
		a.Add(Animation.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "Game/Animations/Idle.anim")));
		a.Add(Animation.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "Game/Animations/Jumping.anim")));

		a.Set("Idle");

		RigidBody r = new RigidBody();
		r.Acceleration = new Point(0, -0.05);//gravity
		
		go.AddComponent(new Collider());
		go.AddComponent(a);
		go.AddComponent(r);

		scene.Instantiate(go, new Point(50, 1));

		Collectable col = new Collectable("Col", new Image("C"));
		scene.Instantiate(col, new Point(64, 2));


		scene.Instantiate(ground, new Point(0, 0));
		scene.Instantiate(platform, new Point(60, 6));
		scene.Instantiate(box, new Point(11, 1));

		scene.Run();

		//Ensure that the program does not end any time soon
		Thread.Sleep(int.MaxValue);
  }
}