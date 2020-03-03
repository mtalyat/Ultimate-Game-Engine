using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateEngine
{
    public enum ForceMode
    {
        Force,
        Impulse
    }

    public class PhysicsBody : Component
    {
        //weight
        public double Mass { get; set; } = 1;
        public double GravityScale { get; set; } = 1;
        public Point CenterOfMass { get; set; } = new Point(0.5, 0.5);//not used yet

        public double Drag { get; set; } = 0;

        //positioning
        public Point Position
        {
            get
            {
                return GameObject.Transform.Position;
            }
            private set
            {
                GameObject.Transform.Position = value;
            }
        }
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
            //account for gravity
            AddForce(Point.Down * GameObject.Scene.AdjustedGravity);

            //update velocity and GameObject position
            Velocity += Acceleration;

            Position += Velocity;
        }

        //increases the velocity based on a force, and the mass of the body
        public void AddForce(Point force)
        {
            Velocity += force * Mass;
        }
    }
}
