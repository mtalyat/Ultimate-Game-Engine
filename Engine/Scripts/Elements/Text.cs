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

        public Text(object r, string s = "")
        {
            Reference = r;
            String = s;
        }

        public override void OnUpdate()
        {
            if (string.IsNullOrEmpty(String))
            {
                Image = new Image(new string[] { Reference.ToString() });
            } else
            {
                Image = new Image(new string[] { string.Format(String, Reference) });
            }
        }
    }
}
