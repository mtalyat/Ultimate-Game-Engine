using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateEngine.UI
{
    class UIElement : GameObject
    {
        public UIElement(string name = "UI Element", Image img = null) : base(name, img)
        {

        }

        public override void OnStart()
        {
            Transform.SetParent(Camera.MainCamera.Transform);
        }
    }
}
