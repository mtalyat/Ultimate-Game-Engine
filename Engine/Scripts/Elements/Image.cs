 using System;

namespace UltimateEngine{
	[Serializable]
	public class Image {
		public Size Size { get; private set; }
        public char[][] RawData { get; private set; }

        const string horizontalFlips = "/\\<>{}[]()bdpqszSZ";
		const string verticalFlips = "/\\.'bpv^dq";

        #region Constructors

        public Image(){
			Initialize(new Size(0, 0));
		}

		public Image(Size s){
			Initialize(s);
		}

		public Image(Image i){
			Size = i.Size;
			RawData = new char[Size.Height][];
			Array.Copy(i.RawData, 0, RawData, 0, Size.Height);
		}

		public Image(string str)
		{
			Size = new Size(str.Length, 1);
			RawData = new char[1][] { str.ToCharArray() };
		}

		public Image(string[] strs){
			SetData(strs);
		}

        #endregion

        //sets data to a string[]
        private void SetData(string[] strs){
			RawData = new char[strs.Length][];

			int maxWidth = 0;
			//find the widest string
			for(int i = 0; i < strs.Length; i++){
				if(strs[i].Length > maxWidth){
					maxWidth = strs[i].Length;
				}
			}

			//set each part of data to the string
			for(int i = 0; i < strs.Length; i++){
				RawData[i] = Words.Fill(strs[i], ' ', maxWidth).ToCharArray();
			}

			//set the size
			if(strs.Length > 0){
				if(strs[0].Length > 0){
					Size = new Size(strs[0].Length, strs.Length);
					return;
				}
			}//else
			Size = new Size(0, 0);
		}

		//sets up data for a new Image
		private void Initialize(Size s){
			//set the data[][] to the correct Height and Width, and fill with spaces
			RawData = new char[s.Height][];
			for(int i = 0; i < s.Height; i++){
				RawData[i] = new char[s.Width];
				for(int j = 0; j < s.Width; j++){
					RawData[i][j] = ' ';
				}
			}
		}

		//flips the Image from left to right
		public void FlipHorizontal(){
			for(int i = 0; i < Size.Height; i++){
				for(int j = 0; j < Size.Width / 2; j++){
					char temp = RawData[i][Size.Width - 1 - j];

					RawData[i][Size.Width - 1 - j] = Swap(RawData[i][j], horizontalFlips);
					RawData[i][j] = Swap(temp, horizontalFlips);
				}
			}
		}

		//flips the Image from up to down
		public void FlipVertical(){
			char[] temp = new char[Size.Width];
			for(int i = 0; i < Size.Height / 2; i++){
				int k = Size.Height - 1 - i;
				Array.Copy(RawData[k], temp, Size.Width);

				Array.Copy(RawData[i], RawData[k], Size.Width);
				Array.Copy(temp, RawData[i], Size.Width);

				//then go through the array and flip the characters
				for(int j = 0; j < Size.Width; j++){
					RawData[i][j] = Swap(RawData[i][j], verticalFlips);
					RawData[k][j] = Swap(RawData[k][j], verticalFlips);
				}
			}
		}

		//'swaps' a char based on a string with combos
		private char Swap(char c, string list){
			for(int i = 0; i < list.Length; i++){
				if(list[i] == c){
					//return the next char or the previous depending on the position in the current combo
					if(i % 2 == 0){
						return list[i + 1];
					} else {
						return list[i - 1];
					}
				}
			}
			//char was not in the list
			return c;
		}

		public override string ToString(){
			string output = "";

			foreach(char[] array in RawData){
				output += new string(array) + "\n";
			}

			return output;
		}
	}
}