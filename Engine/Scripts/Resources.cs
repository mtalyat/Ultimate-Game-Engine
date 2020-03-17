using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateEngine
{
    public static class Resources
    {
        static string path = "";

        static Resources()
        {
            path = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName, "Resources");
        }

        public static Image GetImage(string name)
        {
            return Image.FromFile(Path.Combine(path, "Images", name + ".txt"));
        }

        public static Animation GetAnimation(string name)
        {
            return Animation.FromFile(Path.Combine(path, "Animations", name + ".anim"));
        }
    }
}
