using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateEngine.UI
{
    //A GameObject that always displays an object
    [Serializable]
    public class Text : UIElement
    {
        public object Reference { get; set; }
        public string FormatString { get; set; }

        public override Image Image { get => new Image(GetVisual()); }

        public Text(object reference, string formatString = "")
        {
            Reference = reference;
            FormatString = formatString;
        }

        private string GetVisual()
        {
            if (string.IsNullOrEmpty(FormatString))
            {
                return Reference.ToString();
            } else
            {
                return string.Format(FormatString, Reference);
            }
        }
    }
}
