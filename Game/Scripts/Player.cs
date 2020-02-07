using System;
using UltimateEngine;

namespace Game {
	public class Player : GameObject {
		Animator animator;
		Collider collider;
		RigidBody body;

		int jumpLevel = 0;
		int maxJumps = 1;

		public Player(string name = "Player", Image img = null) : base(name, img){
			Tag = "Player";
		}

		public override void OnStart(){
			animator = GetComponent<Animator>();
			collider = GetComponent<Collider>();
			body = GetComponent<RigidBody>();
		}

		public override void OnUpdate(){
			if(Input.IsKeyPressed("D")){//move right
				Transform.Position += Point.Right;

				animator.Set("Running");

				if(animator.FlippedH){
					animator.FlipHorizontal();
				}
			} else if (Input.IsKeyPressed("A")){//move left
				Transform.Position += Point.Left;

				animator.Set("Running");

				if(!animator.FlippedH){
					animator.FlipHorizontal();
				}
			} else if(Input.IsKeyDown("W")){//jump
				animator.Set("Jumping");

				if(jumpLevel++ < maxJumps){
					body.Velocity = new Point(0, 1);
					//body.Acceleration = new Point(0, -0.05);
				}
			} else {//standing
				if(jumpLevel == 0){
					animator.Set("Idle");
				}
			}
		}

		public override void OnCollision(GameObject other, int side){
			if(other.Tag == "Ground" && side == 1){//top collision
				//reset the jump
				jumpLevel = 0;
				body.Velocity = new Point(0, body.Velocity.Y);
			}
		}
	}
}