using System;
using System.IO;

namespace UltimateEngine {
	[Serializable]
	public class Animation {
		bool transparent = false;

		//the extension used for all animations
		const string extension = ".anim";

		public string Name { get; set; }
		Image[] images;

		public bool FlippedH { get; private set; } = false;
		public bool FlippedV { get; private set; } = false;

		public int Index { get; private set; } = 0;
		public Image Current { get; private set; }

		public Animation(string name, Image[] imgs){
			Name = name;
			images = imgs;

			SetIndex(0);
		}

		private void SetIndex(int index){
			Index = index;
			if(index >= 0 && index < images.Length)
				Current = images[index];
		}

		//moves the Animation to the next Image
		public void Next(){
			SetIndex((Index + 1) % images.Length);
		}

		//restarts the animation
		public void Reset(){
			SetIndex(0);
		}

		//flips all of the images horizontally
		public void FlipHorizontal(){
			for(int i = 0; i < images.Length; i++){
				images[i].FlipHorizontal();
			}
			FlippedH = !FlippedH;
		}

		//flips all of the images vertically
		public void FlipVertical(){
			for(int i = 0; i < images.Length; i++){
				images[i].FlipVertical();
			}
			FlippedV = !FlippedV;
		}

		public void SetTransparency(bool trans)
		{
			foreach(Image i in images)
			{
				i.SetTransparency(trans);
			}
		}

		//parses an animation from a file
		public static Animation FromFile(string path){
			//ensure the file is an animation file
			if(Path.GetExtension(path) != extension) return null;

			string[] lines = File.ReadAllLines(path);

			int height;

			//check to make sure
			if(lines.Length <= 1){//there is something in the file
				return null;
			} else if (!int.TryParse(lines[0], out height)) {//there is an Image height on the first line
				return null;
			} else if (height <= 0) {//the height is valid
				return null;
			}

			Image[] images = new Image[lines.Length / height];

			//get each Image from the file
			for(int i = 1, j = 0; i < lines.Length - (lines.Length % height); i += height, j++){
				string[] array = new string[height];

				Array.Copy(lines, i, array, 0, height);

				images[j] = new Image(array);
			}

			return new Animation(Path.GetFileNameWithoutExtension(path), images);
		}
	}
}