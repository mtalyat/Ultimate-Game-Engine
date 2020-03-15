using System;
using System.Collections.Generic;

namespace UltimateEngine {
	public class Collider : Component {
		const double Spacing = 0.5;

		public bool IsTrigger { get; set; } = false;

		protected PhysicsBody body { get; private set; }
		public Point Velocity => (body == null) ? Point.Zero : body.Velocity;
		protected Point Accleration => (body == null) ? Point.Zero : body.Acceleration;

		public double CoefficientOfFriction { get; set; } = 0.1;

		public Rect BoundsOverride { get; set; } = new Rect(new Point(0, 0), new Size(0, 0));

		public Collider(){

		}

		public Collider(Rect bounds)
		{
			BoundsOverride = bounds;
		}

		public override void Start(){
			body = GameObject.GetComponent<PhysicsBody>();
		}

		public override void Update(){}

		//gets the difference between positions
		public Point GetMovement(){
			if(body == null) return Point.Zero;

			return body.Velocity + body.Acceleration;
		}

		//gets the bounds. Gets the override if it is valid
		public Rect GetBounds()
		{
			if(BoundsOverride.Area() > 0)
			{
				return BoundsOverride;
			}
			return GameObject.Bounds;
		}

		//returns true if the Collider is moving
		public bool IsMoving(){
			if(body == null) return false;

			return body.Velocity != Point.Zero;
		}

		//returns true if the collider is kinematic or has no body
		public bool IsKinematic()
		{
			if (body == null) return true;

			return body.IsKinematic;
		}

		protected Point Translation(int frames = 1)
		{
			return (Velocity * frames + Accleration * AdvMath.Factorial(frames)) / Scene.ScaledFPS;
		}

		public bool WillCollideWith(Collider c, int frames = 1)
		{
			Point translation = Translation(frames);
			Rect other = c.GetBounds() + c.Translation(frames);

			return GetBounds().Crosses(other, translation);
		}

		//checks if two GameObjects are colliding
		public void CheckCollision(Collider c, int frames = 1){
			if (WillCollideWith(c, frames))
			{
				CollideWith(c, GetBounds().FindSide(c.GetBounds()), frames);
			}
		}

		//runs after collisions have been detected
		private void CollideWith(Collider c, Direction side, int frames){
			Direction oppositeSide = AdvMath.OppositeDirection(side);

			Rect one = GetBounds();
			Rect two = c.GetBounds();

			//if one of the colliders is a trigger
			if (IsTrigger || c.IsTrigger)
			{
				if (IsTrigger) GameObject.OnTrigger(c.GameObject, side);//activate collison for this
				if (c.IsTrigger) c.GameObject.OnTrigger(GameObject, oppositeSide);//activate collison for other object, but opposite side
			}
			else
			{//both are normal
			 //use momentum and physics to determine what happens when they both collide:

				//HOWEVER, if one of them Is Kinematic, or has no PhysicsBody, there is no physics collision
				if (c.IsKinematic())
				{
					//get the normal force for the friction
					//math in notebook, derived from Frictional Force equation and F = ma where a = g
					Point normalForce = Velocity * CoefficientOfFriction;

					//adjust the position so it isn't directly on stuff
					switch (side)
					{
						case Direction.Left:
							GameObject.Transform.Position = new Point(two.Right + Spacing, one.Y);
							body.Velocity = new Point(0, Velocity.Y - normalForce.Y);
							break;
						case Direction.Right:
							GameObject.Transform.Position = new Point(two.Left - one.Width - Spacing, one.Y);
							body.Velocity = new Point(0, Velocity.Y - normalForce.Y);
							break;
						case Direction.Down:
							GameObject.Transform.Position = new Point(one.X, two.Top + Spacing);
							body.Velocity = new Point(Velocity.X - normalForce.X, 0);
							break;
						case Direction.Up:
							GameObject.Transform.Position = new Point(one.X, two.Bottom - one.Height - Spacing);
							body.Velocity = new Point(Velocity.X - normalForce.X, 0);
							break;
					}

					//Debug.Log("After fix: " + GameObject.Transform);

					GameObject.OnCollision(c.GameObject, side);//activate collison for this
					c.GameObject.OnCollision(GameObject, oppositeSide);//activate collison for other object

					//ok, done with collisions now
					return;
				}

				//get the average elasticity to use for the collisions
				double averageElasticity = (body.Elasticity + c.body.Elasticity) / 2;

				//get the system velocity
				Point systemVelocity = (body.Momentum + c.body.Momentum) / (body.Mass + c.body.Mass);

				//get the new velocities based on that, and the average elasticity
				body.Velocity = (2 * systemVelocity) - (averageElasticity * body.Velocity);
				c.body.Velocity = (2 * systemVelocity) - (averageElasticity * c.body.Velocity);

				GameObject.OnCollision(c.GameObject, side);//activate collison for this
				c.GameObject.OnCollision(GameObject, oppositeSide);//activate collison for other object
			}
		}
	}
}