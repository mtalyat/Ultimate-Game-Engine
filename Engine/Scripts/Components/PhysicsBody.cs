using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateEngine
{
    public class PhysicsBody : Component
    {
		public override GameObject GameObject { get; set; }
		public Point Velocity { get; set; } = new Point(0, 0);
		public Point Acceleration { get; private set; } = new Point(0, 0);

		public PhysicsBody()
		{

		}

		public override void Start()
		{

		}

		public override void Update()
		{
		}

		//checks if the GameObject is moving at all
		public bool IsMoving()
		{
			return Velocity.X != 0 || Velocity.Y != 0;
		}

		//stops all movement
		public void Stop()
		{
			Acceleration = Point.Zero;
			Velocity = Point.Zero;
		}
	}
}
