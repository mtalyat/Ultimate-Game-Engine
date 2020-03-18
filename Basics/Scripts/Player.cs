using System;
using UltimateEngine;

namespace UltimateEngine.Basics {
	[Serializable]
	public class Player : GameObject {
		public static Player Active;

		public Animator Animator { get; set; }
		public Collider Collider { get; set; }
		public PhysicsBody Body { get; set; }

		public Camera Camera { get; private set; }

		public double Speed { get; set; } = 5;
		public double JumpPower { get; set; } = 14;

		int jumpLevel = 0;
		int maxJumps = 1;

		public Player(string name = "Player") : base(name, null){
			Tag = "Player";

			AddComponent(new Animator());
			AddComponent(new Collider());
			AddComponent(new PhysicsBody());

			Animator = GetComponent<Animator>();
			Collider = GetComponent<Collider>();
			Body = GetComponent<PhysicsBody>();

			Body.Mass = 10;
			Body.Elasticity = 0;
			Collider.CoefficientOfFriction = 0.2;

			Animator.Add(Resources.GetAnimation("Idle"));
			Animator.Add(Resources.GetAnimation("Running"));
			Animator.Add(Resources.GetAnimation("Jumping"));

			Camera = new Camera("Player Camera");
		}

		public override void OnWake()
		{
			Camera = (Camera)InstantiateChild(Camera, (Camera.Bounds.Center * -1) + new Point(3, 3));//kind of centers the player

			Camera.MainCamera = Camera;

			Active = this;
		}

		public override void OnStart()
		{
			Animator.Play("Idle");
		}

		public override void OnUpdate()
		{
			if (Input.IsKeyPressed("D"))
			{//move right
				Body.Velocity = new Point(Speed, Body.Velocity.Y);

				if(jumpLevel == 0)//if on ground
					Animator.Play("Running");

				if (Animator.FlippedH)
				{
					Animator.FlipHorizontal();
				}
			}
			else if (Input.IsKeyPressed("A"))
			{//move left
				Body.Velocity = new Point(Speed * -1, Body.Velocity.Y);

				if(jumpLevel == 0)//if on ground
					Animator.Play("Running");

				if (!Animator.FlippedH)
				{
					Animator.FlipHorizontal();
				}
			}
			else if (Input.IsKeyDown("W"))
			{//jump
				Animator.Play("Jumping");

				if (jumpLevel++ < maxJumps)
				{
					Transform.Y += 0.5;
					Body.Velocity = new Point(Body.Velocity.X, JumpPower);
				}
			}
			else
			{//standing
				if (jumpLevel == 0)
				{
					Animator.Play("Idle");
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
			if(side == Direction.Down){//top collision
				//reset the jump
				jumpLevel = 0;
			}
		}
	}
}