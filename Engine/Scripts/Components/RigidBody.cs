using System;

namespace UltimateEngine {
	//this component controls the velocity and acceleration for a GameObject
	public class RigidBody : Component {
		public override GameObject GameObject { get; set; }
		public Point Velocity { get; set; } = new Point(0, 0);
		public Point Acceleration { get; set; } = new Point(0, 0);

		public RigidBody(){

		}

		public override void Start(){

		}

		public override void Update(){
			//update the velocity
			Velocity += Acceleration;

			//update the position
			GameObject.Transform.Move(Velocity);
		}

		//checks if the GameObject is moving at all
		public bool IsMoving(){
			return Velocity.X != 0 || Velocity.Y != 0;
		}

		//stops all movement
		public void Stop(){
			Acceleration = Point.Zero;
			Velocity = Point.Zero;
		}
	}
}