using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateEngine.Basics
{
    [Serializable]
    public class Ground : GameObject
    {
        protected Collider collider;

        public Ground(string pattern, int length, string accents, int accentDepth, Image[] plants, int multiplyPlants, string name = "Ground") : base(name, null)
        {
            collider = new Collider();

            AddComponent(collider);

            Image = CreateImage(pattern, length, accents, accentDepth, plants, multiplyPlants);
        }

        public override void OnWake()
        {
            Transform.Z = -1;
        }

        private Image CreateImage(string pattern, int length, string accents, int accentDepth, Image[] plants, int multiplyPlants)
        {
            Random random = new Random();

            //get the height of the image
            int plantHeight = 0;

            foreach(Image img in plants)
            {
                if(img.Size.Height > plantHeight)
                {
                    plantHeight = img.Size.Height;
                }
            }

            int height = pattern.Length + plantHeight;

            //create the array to manage
            string[] data = new string[height];

            //create the display
            for(int i = 0; i < height; i++)
            {
                if(i < plantHeight)
                {
                    data[i] = new string(' ', length);
                } else
                {
                    data[i] = new string(pattern[i - plantHeight], length);
                }
            }

            //add the accents, below the first layer of the ground
            int accentAmount = length;

            for(int i = 0; i < accentAmount; i++)
            {
                int y = random.Next(plantHeight + 1, plantHeight + accentDepth);
                int x = random.Next(0, length);
                StringBuilder sb = new StringBuilder(data[y]);
                sb.Remove(x, 1);
                sb.Insert(x, accents[random.Next(0, accents.Length)].ToString());

                data[y] = sb.ToString();
            }

            //create the image
            Image final = new Image(data);

            //draw the plants
            for(int i = 0; i < Math.Max(1, multiplyPlants); i++)
            {
                foreach (Image image in plants)
                {
                    final.DrawImage(image, random.Next(0, length - image.Size.Width), plantHeight - image.Size.Height);
                }
            }

            //adjust the collider to match
            collider.BoundsOverride = new Rect(0, 0, length, height - plantHeight);

            return final;
        }
    }
}
