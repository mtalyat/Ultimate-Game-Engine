using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateEngine.Basics
{
    [Serializable]
    public class Interactable : GameObject
    {
        public String InteractKeyName { get; set; } = "E";
        public Point Range { get; set; } = new Point(4, 4);
        bool inRange = false;

        Text displayText;

        public Interactable(string name = "Interactable GameObject", Image img = null) : base(name, img)
        {
            if(img != null)
            {
                Range = new Point(img.Size);
            }

            displayText = (Text)InstantiateChild(new Text(InteractKeyName, "'{0}'"), new Point(Bounds.CenterX - 1, Bounds.Top + 1));
            displayText.Visible = false;
        }

        public virtual void OnInteraction(Player player) { }

        public override void OnUpdate()
        {
            if(Player.Active.Bounds.Center.IsInRange(Bounds.Center, Range))
            {
                if (!inRange)
                {
                    //enter
                    inRange = true;
                    displayText.Visible = true;
                }
            } else
            {
                if (inRange)
                {
                    //exit
                    displayText.Visible = false;
                    inRange = false;
                }
            }

            if (Input.IsKeyDown(InteractKeyName))
            {
                if (inRange)
                {
                    OnInteraction(Player.Active);
                }
            }
        }
    }
}
