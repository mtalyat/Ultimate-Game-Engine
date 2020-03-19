using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateEngine.UI;

namespace UltimateEngine.Basics
{
    [Serializable]
    public class Interactable : GameObject
    {
        string _keyName = "E";
        public string InteractKeyName
        {
            get
            {
                return _keyName;
            }
            set
            {
                _keyName = value;
                displayText.Image = new Image($"'{value}'");
            }
        }
        public Point Range { get; set; } = new Point(4, 4);
        bool inRange = false;

        GameObject displayText;

        public Interactable(string name = "Interactable GameObject", Image img = null) : base(name, img)
        {
            if(img != null)
            {
                Range = new Point(img.Size);
            }

            displayText = InstantiateChild(new GameObject("Display Text", new Image($"'{InteractKeyName}'")),
                new Point(Bounds.CenterX - 1, Bounds.Top + 1));
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
