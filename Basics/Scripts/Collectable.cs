using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateEngine.Basics
{
    [Serializable]
    public class Collectable : GameObject
    {
        public Collectable(string name = "Collectable", Image img = null) : base(name, img)
        {
            Collider c = new Collider();
            c.IsTrigger = true;
            AddComponent(c);
        }

        public virtual void OnCollect(GameObject go) { }

        public override void OnTrigger(GameObject go, Direction side)
        {
            if(go.Tag == "Player")
            {
                OnCollect(go);
                Destroy(this);
            }
        }
    }
}
