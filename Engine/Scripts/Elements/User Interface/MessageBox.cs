using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateEngine.UI
{
    [Serializable]
    public class MessageBox : UIElement
    {
        public static char Corner { get; set; } = '+';
        public static char Vertical { get; set; } = '|';
        public static char Horizontal { get; set; } = '-';

        public string ContinueKeyName { get; set; } = "E";
        public string Message { get; private set; }
        public int Width { get; set; }

        public MessageBox(string message, int width = 50)
        {
            SetMessage(message, width);
        }

        //sets a Message, and adjusts the Image
        public void SetMessage(string m, int width = -1)
        {
            Message = m;
            Width = Math.Max(27 + ContinueKeyName.Length, width);

            string[] lines = Words.WordWrap(Message, Width - 4);

            string[] final = new string[lines.Length + 4];

            final[0] = Corner + new string(Horizontal, Width - 2) + Corner;

            for(int i = 0; i < lines.Length; i++)
            {
                final[i + 1] = Vertical + " " + lines[i] + " " + Vertical;
            }

            final[final.Length - 3] = Vertical + new string(' ', width - 2) + Vertical;
            final[final.Length - 2] = $"{Vertical} Press '{ContinueKeyName}' to continue..." + new string(' ', Width - 27 - ContinueKeyName.Length) + " " + Vertical;
            final[final.Length - 1] = final[0];

            Image = new Image(final);
        }

        public void Show()
        {
            Visible = true;

            Input.WaitForKey(ContinueKeyName, new Action(() =>
            {
                Hide();
            }));
        }

        public void Hide()
        {
            Visible = false;
        }

        public override void OnWake()
        {
            Hide();
        }
    }
}
