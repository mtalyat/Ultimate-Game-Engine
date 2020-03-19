using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateEngine.Basics
{
    [Serializable]
    public class Box : GameObject
    {
        public Animator Animator { get; set; }
        public Collider Collider { get; set; }
        public PhysicsBody Body { get; set; }

        public Box(string name = "Box", Image img = null) : base(name, img)
        {
            AddComponent(new Animator());
            AddComponent(new Collider());
            AddComponent(new PhysicsBody());

            Animator = GetComponent<Animator>();
            Collider = GetComponent<Collider>();
            Body = GetComponent<PhysicsBody>();

            Body.Elasticity = 0;//no bounce
        }
    }
}
