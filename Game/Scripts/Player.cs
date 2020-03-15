using System;
using UltimateEngine;

namespace Game {
	public class Player : GameObject {
		public static Player Active;

		public Animator Animator { get; set; }
		public Collider Collider { get; set; }
		public PhysicsBody Body { get; set; }

		public Camera Camera { get; private set; }

		public double Speed { get; set; } = 5;
		public double JumpPower { get; set; } = 10;

		int jumpLevel = 0;
		int maxJumps = 1;

		public Player(string name = "Player", Image img = null) : base(name, img){
			Tag = "Player";

			AddComponent(new Animator());
			AddComponent(new Collider());
			AddComponent(new PhysicsBody());

			Animator = GetComponent<Animator>();
			Collider = GetComponent<Collider>();
			Body = GetComponent<PhysicsBody>();

			Camera = new Camera();
			InstantiateChild(Camera, (Camera.Bounds.Center * -1) + new Point(3, 3));//kind of centers the player

			Active = this;
		}

		public override void OnStart()
		{
			Camera.MainCamera = Camera;
		}

		public override void OnUpdate()
		{
			if (Input.IsKeyPressed("D"))
			{//move right
				Body.Velocity = new Point(Speed, Body.Velocity.Y);

				Animator.Set("Running");

				if (Animator.FlippedH)
				{
					Animator.FlipHorizontal();
				}
			}
			else if (Input.IsKeyPressed("A"))
			{//move left
				Body.Velocity = new Point(Speed * -1, Body.Velocity.Y);

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
					Body.Velocity = new Point(0, JumpPower);
				}
			}
			else
			{//standing
				if (jumpLevel == 0)
				{
					Animator.Set("Idle");
					Body.Velocity = new Point(0, Body.Velocity.Y);
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

		public override void OnCollision(GameObject other, Direction side){
			if(other.Tag == "Ground" && side == Direction.Down){//top collision
				//reset the jump
				jumpLevel = 0;
			}

			//Debug.Log($"Player collided with {other} on side {side}.");
		}
	}
}