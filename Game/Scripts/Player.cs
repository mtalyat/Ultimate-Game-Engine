using System;
using UltimateEngine;

namespace Game {
	public class Player : GameObject {
		public Animator Animator { get; set; }
		public Collider Collider { get; set; }
		public RigidBody Body { get; set; }

		Camera camera;

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

			camera = new Camera();
			Scene.Current.Instantiate(camera, new Point(-48, -7), Transform);
		}

		public override void OnStart()
		{
			Body.Acceleration = new Point(0, -0.1);
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

			//Camera toggle
			if (Input.IsKeyUp("C"))
			{
				Camera.NextCamera();
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
		}
	}
}