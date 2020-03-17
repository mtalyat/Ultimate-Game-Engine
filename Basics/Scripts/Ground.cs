using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateEngine.Basics
{
    [Serializable]
    public class Ground : GameObject
    {
        protected Collider collider;

        public Ground(char display, int length, string name = "Ground") : base(name, new Image(new string(display, length > 0 ? length : 1)))
        {
            collider = new Collider();
            AddComponent(collider);
        }
    }
}
