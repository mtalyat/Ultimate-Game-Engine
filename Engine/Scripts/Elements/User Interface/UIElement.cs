using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateEngine.UI
{
    [Serializable]
    public class UIElement : GameObject
    {
        public override Point ScreenPosition => Transform.LocalPosition;

        public UIElement(string name = "UI Element", Image img = null) : base(name, img)
        {

        }
    }
}
