using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateEngine
{
    //A GameObject that always displays an object
    public class Text : GameObject
    {
        public object Reference { get; set; }
        public string String { get; set; }

        public override Image Image { get => new Image(GetVisual()); }

        public Text(object r, string s = "")
        {
            Reference = r;
            String = s;
        }

        private string GetVisual()
        {
            if (string.IsNullOrEmpty(String))
            {
                return Reference.ToString();
            } else
            {
                return string.Format(String, Reference);
            }
        }
    }
}
