using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateEngine.Basics.Items
{
    [Serializable]
    public class Coin : Collectable
    {
        public static int Score { get; set; } = 0;

        public Coin(string name = "Coin", Image img = null) : base(name, img)
        {

        }

        public override void OnTrigger(GameObject go, Direction side)
        {
            Score++;

            base.OnTrigger(go, side);
        }

        public override string ToString()
        {
            return Score.ToString();
        }
    }
}
