using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateEngine
{
    [Serializable]
    public class PhysicsBody : Component
    {
        public const double Gravity = -9.81;

        //weight
        private double _mass = 1;
        public double Mass
        {
            get
            {
                return _mass;
            }
            set
            {
                SetMass(value);
            }
        }
        private double _gravityScale = 1;
        public double GravityScale
        {
            get
            {
                return _gravityScale;
            }
            set
            {
                SetGravityScale(value);
            }
        }
        public Point CenterOfMass { get; set; } = new Point(0.5, 0.5);//not used yet
        public double Weight => Gravity * _gravityScale * _mass;

        //drag and anti-forces
        public double Drag { get; set; } = 0.5;//not used yet

        //collisions
        public double Elasticity { get; set; } = 1.0;
        public Point Momentum => Velocity * Mass;
        private bool _isKinematic = false;
        public bool IsKinematic
        {
            get
            {
                return _isKinematic;
            }
            set
            {
                SetKinematic(value);
            }
        }

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
            if(!IsKinematic)
                Acceleration = new Point(0, Gravity * GravityScale);
        }

        public override void Update()
        {
            //if (GravityScale != 0) AddForce(new Point(0, Gravity * GravityScale) / Scene.GoalFPS);

            //update velocity and GameObject position
            Velocity += (Acceleration / Scene.ScaledFPS).Round(2);

            Position += (Velocity / Scene.ScaledFPS).Round(2);
        }

        //increases the Velocity based on a force, and the mass of the body
        public void AddForce(Point force)
        {
            Velocity += (force / _mass).Round(2);
        }

        //increases the Acceleration based on a force, and the mass of the body
        public void AddConstantForce(Point force)
        {
            Acceleration += (force / _mass).Round(2);
        }

        public void RemoveConstantForce(Point force)
        {
            AddConstantForce(force * -1);
        }

        #region Setters

        //adjusts the Acceleration based on the new scale of Gravity
        private void SetGravityScale(double value)
        {
            if (value == _gravityScale) return;//no need to do anything if it stays the same

            //remove the force from the acceleration
            Acceleration -= new Point(0, Gravity * _gravityScale);

            _gravityScale = value;

            //add it back, but with the new scale
            Acceleration += new Point(0, Gravity * _gravityScale);
        }

        //sets the mass
        private void SetMass(double value)
        {
            if (value == _mass) return;

            _mass = value;
        }

        private void SetKinematic(bool k)
        {
            if (_isKinematic == k) return;

            _isKinematic = k;

            if (_isKinematic)//non-moving
            {
                GravityScale = 0;
                Acceleration = Point.Zero;
                Velocity = Point.Zero;
            } else//moving
            {
                GravityScale = 1;
            }
        }

        #endregion
    }
}
