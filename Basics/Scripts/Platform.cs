using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateEngine.Basics
{
    [Serializable]
    public class Platform : GameObject
    {
        protected Collider collider;
        public Platform(char display, int length, string name = "Platform") : base(name, new Image(new string(display, length)))
        {
            collider = new Collider();
            AddComponent(collider);
        }

        public override void PreCollision(Collider c, Direction side)
        {
            if(c.GameObject.Tag == "Player" && side != Direction.Up)//if player hits the bottom of this GameObject
            {
                c.Ignore = GetComponent<Collider>();
            }
        }
    }
}
