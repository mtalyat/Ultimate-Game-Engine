using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateEngine.UI;

namespace UltimateEngine.Basics.Items
{
    [Serializable]
    public class Sign : Interactable
    {
        MessageBox mBox;

        public Sign(string message, Image img = null, string name = "Sign") : base(name, img)
        {
            mBox = new MessageBox(message);
        }

        public override void OnWake()
        {
            base.OnWake();

            mBox = (MessageBox)InstantiateChild(mBox, new Point(1, 1));
        }

        public override void OnInteraction(Player player)
        {
            mBox.Show();
        }
    }
}
