using System;
using UltimateEngine;

namespace Game {
	public class Player : GameObject {
		public Animator Animator { get; set; }
		public Collider Collider { get; set; }
		public RigidBody Body { get; set; }

		public Camera Camera { get; private set; }

		int jumpLevel = 0;
		int maxJumps = 1;

		public Player(string name = "Player", Image img = null) : base(name, img){
			Tag = "Player";

			AddComponent(new Animator());
			AddComponent(new Collider());
			AddComponent(new RigidBody());

			Animator = GetComponent<Animator>();
			Collider = GetComponent<Collider>();
			Body = GetComponent<RigidBody>();

			Camera = new Camera();
			InstantiateChild(Camera, new Point(-48, -7));//kind of centers the player
		}

		public override void OnStart()
		{
			Body.Acceleration = new Point(0, -0.1);

			Camera.MainCamera = Camera;
		}

		public override void OnUpdate()
		{
			if (Input.IsKeyPressed("D"))
			{//move right
				Transform.Position += Point.Right;

				Animator.Set("Running");

				if (Animator.FlippedH)
				{
					Animator.FlipHorizontal();
				}
			}
			else if (Input.IsKeyPressed("A"))
			{//move left
				Transform.Position += Point.Left;

				Animator.Set("Running");

				if (!Animator.FlippedH)
				{
					Animator.FlipHorizontal();
				}
			}
			else if (Input.IsKeyDown("W"))
			{//jump
				Animator.Set("Jumping");

				if (jumpLevel++ < maxJumps)
				{
					Body.Velocity = new Point(0, 1.5);
				}
			}
			else
			{//standing
				if (jumpLevel == 0)
				{
					Animator.Set("Idle");
				}
			}

			if(Input.IsKeyDown("UpArrow"))
			{
				Transform.BringToFront();
			} else if(Input.IsKeyDown("DownArrow"))
			{
				Transform.SendToBack();
			}
		}

		public override void OnCollision(GameObject other, int side){
			if(other.Tag == "Ground" && side == 1){//top collision
				//reset the jump
				jumpLevel = 0;
				if(Body.Velocity.Y < 0)
				{
					Body.Velocity = new Point(Body.Velocity.X, 0);
				}
			}
			if(other.Name == "Ground")
			{
				ScreenBuffer.SetColors(ConsoleColor.White, ConsoleColor.Black);
			} else if (other.Name == "Platform 1")
			{
				ScreenBuffer.SetColors(ConsoleColor.Yellow, ConsoleColor.Red);
			} else if (other.Name == "Platform 2")
			{
				ScreenBuffer.SetColors(ConsoleColor.DarkBlue, ConsoleColor.Blue);
			} else if (other.Name == "Platform 3")
			{
				ScreenBuffer.SetColors(ConsoleColor.Black, ConsoleColor.DarkYellow);
			}
		}
	}
}