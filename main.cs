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
		Scene scene = new Scene(100, 20);

		Player p = new Player();

		Animator animator = p.Animator;

		animator.Add(Animation.FromFile("C:\\Users\\Me\\source\\repos\\Ultimate-Game-Engine\\Game\\Animations\\Running.anim"));
		animator.Add(Animation.FromFile("C:\\Users\\Me\\source\\repos\\Ultimate-Game-Engine\\Game\\Animations\\Jumping.anim"));
		animator.Add(Animation.FromFile("C:\\Users\\Me\\source\\repos\\Ultimate-Game-Engine\\Game\\Animations\\Idle.anim"));

		GameObject ground = new GameObject("Ground", new Image(new string[] { new string('^', 100) }));
		ground.AddComponent(new Collider());
		ground.Tag = "Ground";

		Camera cam = new Camera();

		Text t = new Text(p.Transform);

		scene.Instantiate(p, new Point(20, 10));

		scene.Instantiate(ground, new Point(0, 0));

		scene.Instantiate(t, new Point(0, 5), p.Transform);

		scene.Instantiate(cam);

		scene.Run();

		//Ensure that the program does not end any time soon
		Thread.Sleep(int.MaxValue);
	}
}