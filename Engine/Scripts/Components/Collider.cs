using System;
using System.Collections.Generic;

namespace UltimateEngine {
	[Serializable]
	public class Collider : Component {
		const double Spacing = 0.3;

		public bool IsTrigger { get; set; } = false;

		protected PhysicsBody body { get; private set; }
		public Point Velocity => (body == null) ? Point.Zero : body.Velocity;
		protected Point Accleration => (body == null) ? Point.Zero : body.Acceleration;

		public double CoefficientOfFriction { get; set; } = 0.1;

		public Rect BoundsOverride { get; set; } = new Rect(new Point(0, 0), new Size(0, 0));

		public Collider Ignore { get; set; }

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
				return BoundsOverride + GameObject.Transform.Position;
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

		public bool WillCollideWith(Collider c)
		{
			Point translation = Translation();
			Rect other = c.GetBounds() + c.Translation();

			return GetBounds().Crosses(other, translation);
		}

		//checks if two GameObjects are colliding, returns true if it will hit something
		public bool CheckCollision(Collider c){
			if (WillCollideWith(c))
			{
				if(c != Ignore)
				{
					CollideWith(c, GetBounds().FindSide(c.GetBounds()));
				}
				return true;//returning true because it did hit something
			}
			return false;//hit nothing
		}

		//checks if two objects are intersecting, and fixes it if it needs to
		public void CheckFix(Collider c)
		{
			Rect bounds1 = GetBounds();
			Rect bounds2 = c.GetBounds();

			if (bounds1.Intersects(bounds2))
			{
				if(c != Ignore)
				{
					FixWith(c, bounds1, bounds2);
				}
			}
		}

		//runs after collisions have been detected
		private void CollideWith(Collider c, Direction side){
			Direction oppositeSide = AdvMath.OppositeDirection(side);

			GameObject.PreCollision(c, side);
			c.GameObject.PreCollision(this, oppositeSide);

			if (c == Ignore) return;

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
					FixWith(c, one, two);

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
				//only do the calculations for the side that the collision is on
				if (side == Direction.Down || side == Direction.Up)
				{// Y axis
					body.Velocity = new Point(body.Velocity.X, (2 * systemVelocity.Y) - (averageElasticity * body.Velocity.Y));
					c.body.Velocity = new Point(c.body.Velocity.X, (2 * systemVelocity.Y) - (averageElasticity * c.body.Velocity.Y));
				} else
				{//X axis
					body.Velocity = new Point((2 * systemVelocity.X) - (averageElasticity * body.Velocity.X), body.Velocity.Y);
					c.body.Velocity = new Point((2 * systemVelocity.X) - (averageElasticity * c.body.Velocity.X), c.body.Velocity.Y);
				}

				GameObject.OnCollision(c.GameObject, side);//activate collison for this
				c.GameObject.OnCollision(GameObject, oppositeSide);//activate collison for other object
			}
		}

		//fixes a collision
		//basically, Kinematic collisions only
		private void FixWith(Collider c, Rect one, Rect two)
		{
			if (Ignore == c) return;

			Direction side = one.FindSide(two);

			//get the normal force for the friction
			//math in notebook, derived from Frictional Force equation and F = ma where a = g
			Point normalForce = Velocity * CoefficientOfFriction;

			//adjust the position so it isn't directly on stuff
			switch (side)
			{
				case Direction.Left:
					GameObject.Transform.Position = new Point(two.Right - BoundsOverride.X + Spacing, GameObject.Transform.Y);
					body.Velocity = new Point(0, Velocity.Y - normalForce.Y);
					break;
				case Direction.Right:
					GameObject.Transform.Position = new Point(two.Left - one.Width + BoundsOverride.X - Spacing, GameObject.Transform.Y);
					body.Velocity = new Point(0, Velocity.Y - normalForce.Y);
					break;
				case Direction.Down:
					GameObject.Transform.Position = new Point(GameObject.Transform.X, two.Top - BoundsOverride.Y + Spacing);
					body.Velocity = new Point(Velocity.X - normalForce.X, 0);
					break;
				case Direction.Up:
					GameObject.Transform.Position = new Point(GameObject.Transform.X, two.Bottom - one.Height + BoundsOverride.Y - Spacing);
					body.Velocity = new Point(Velocity.X - normalForce.X, 0);
					break;
			}

			//do not call Collisions again because they should have already been called in Check
		}
	}
}