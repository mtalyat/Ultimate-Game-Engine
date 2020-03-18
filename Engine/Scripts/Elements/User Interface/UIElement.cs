using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateEngine.UI
{
    [Serializable]
    class UIElement : GameObject
    {
        public override Point ScreenPosition
        {
            get
            {
                if(Transform.Parent.GameObject == Camera.MainCamera)
                    return Transform.LocalPosition.Floor();
                return Transform.Position.Floor();
            }
        }

        public UIElement(string name = "UI Element", Image img = null) : base(name, img)
        {

        }

        public override void OnStart()
        {

        }

        public override void OnUpdate()
        {
            
        }
    }
}
