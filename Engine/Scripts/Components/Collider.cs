using System;
using System.Collections.Generic;

namespace UltimateEngine {
	public class Collider : Component {
		public bool IsTrigger { get; set; } = false;

		protected PhysicsBody body { get; private set; }

		public Rect BoundsOverride { get; set; } = new Rect(new Point(0, 0), new Size(0, 0));

		const double CollisionBuffer = 0.0;

		public Collider(){

		}

		public Collider(Rect bounds)
		{
			BoundsOverride = bounds;
		}

		public override void Start(){
			body = GameObject.GetComponent<PhysicsBody>();
		}

		public override void Update(){
		}

		//get the Factorial of a number
		private int Factorial(int number){
			if(number == 1){
				return 1;
			}

			return number * Factorial(number - 1);
		}

		//gets the difference between positions
		public Point GetMovement(int frames){
			if(body == null) return Point.Zero;

			return body.Velocity * (double)frames + body.Acceleration * (double)Factorial(frames);
		}

		//returns true if the Collider is moving
		public bool IsMoving(){
			if(body == null) return false;

			return body.Velocity != Point.Zero;
		}

		//checks if two GameObjects are colliding
		public void CheckCollision(Collider c, int frames){
			int intersect = WillCollide(c, frames);

			//if the objects are colliding, run collide events
			if(intersect >= 0){
				CollideWith(c, intersect);
			}
		}

		//determines if two Colliders will collide with each other in x frames
		private int WillCollide(Collider c, int frames){
			return (GameObject.Bounds + GetMovement(frames)).Intersects(c.GameObject.Bounds + c.GetMovement(frames));
		}

		//runs after collisions have been detected
		private void CollideWith(Collider c, int side){
			//if one of the colliders is a trigger
			if(IsTrigger || c.IsTrigger){
				GameObject.OnTrigger(c.GameObject, side);//activate collison for this
				c.GameObject.OnTrigger(GameObject, side);//activate collison for other object
			} else {//both are normal
				Rect one = GameObject.Bounds;
				Rect two = c.GameObject.Bounds;

				switch(side){
					case 0: //right
						GameObject.Transform.Position = new Point(two.Right + CollisionBuffer, one.Y);
						break;
					case 1: //top
						GameObject.Transform.Position = new Point(one.X, two.Top + CollisionBuffer);
						break;
					case 2: //left
						GameObject.Transform.Position = new Point(two.X - one.Width - CollisionBuffer, one.Y);
						break;
					case 3: //bottom
						GameObject.Transform.Position = new Point(one.X, two.Y - one.Height - CollisionBuffer);
						break;
				}

				if(body != null){
					if(side == 0 || side == 2){
						body.Velocity = new Point(0, body.Velocity.Y);
					} else {
						body.Velocity = new Point(body.Velocity.X, 0);
					}
				}

				GameObject.OnCollision(c.GameObject, side);//activate collison for this
				c.GameObject.OnCollision(GameObject, side);//activate collison for other object
			}
		}
	}
}