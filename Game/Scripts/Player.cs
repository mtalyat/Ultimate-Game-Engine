using System;
using UltimateEngine;

namespace Game {
	public class Player : GameObject {
		public Animator Animator { get; set; }
		public Collider Collider { get; set; }
		public RigidBody Body { get; set; }

		int jumpLevel = 0;
		int maxJumps = 1;

		public Player(string name = "Player", Image img = null) : base(name, img){
			Tag = "Player";

			Animator a = new Animator();
			Collider c = new Collider();
			RigidBody r = new RigidBody();

			AddComponent(a);
			AddComponent(c);
			AddComponent(r);

			Animator = a;
			Collider = c;
			Body = r;
		}

		public override void OnStart(){
			Body.Acceleration = new Point(0, -0.05);
		}

		public override void OnUpdate(){
			if(Input.IsKeyPressed("D")){//move right
				Transform.Position += Point.Right;

				Animator.Set("Running");

				if(Animator.FlippedH){
					Animator.FlipHorizontal();
				}
			} else if (Input.IsKeyPressed("A")){//move left
				Transform.Position += Point.Left;

				Animator.Set("Running");

				if(!Animator.FlippedH){
					Animator.FlipHorizontal();
				}
			} else if(Input.IsKeyDown("W")){//jump
				Animator.Set("Jumping");

				if(jumpLevel++ < maxJumps){
					Body.Velocity = new Point(0, 1);
					//body.Acceleration = new Point(0, -0.05);
				}
			} else {//standing
				if(jumpLevel == 0){
					Animator.Set("Idle");
				}
			}
		}

		public override void OnCollision(GameObject other, int side){
			if(other.Tag == "Ground" && side == 1){//top collision
				//reset the jump
				jumpLevel = 0;
				Body.Velocity = new Point(0, Body.Velocity.Y);
			}
		}
	}
}