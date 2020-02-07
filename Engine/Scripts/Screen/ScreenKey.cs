using System;

namespace UltimateEngine{
	class ScreenKey{
		//the key itself
		public string Name { get; private set; } = "";
		public char Char { get; private set; } = '\n';
		//the state that regulats key up/down
		public int KeyDownState { get; set; } = 0;

		public ScreenKey(string name, char c){
			Name = name;
			Char = c;
		}

		public override string ToString(){
			return Name;
		}
	}
}