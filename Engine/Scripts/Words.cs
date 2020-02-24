using System;
using System.Linq;
using System.Collections.Generic;

namespace UltimateEngine {
	//this whole class just manages strings and such that may be useful
	public static class Words {

		#region Conversions

		//converts string[] to char[][]
		public static char[][] StringArrayToJaggedCharArray(string[] strs){
			char[][] output = new char[strs.Length][];

			for(int i = 0; i < strs.Length; i++){
				output[i] = strs[i].ToCharArray();
			}

			return output;
		}

		//converts char[][] to string[]
		public static string[] JaggedCharArrayToStringArray(char[][] chars){
			string[] output = new string[chars.Length];

			for(int i = 0; i < chars.Length; i++){
				output[i] = new string(chars[i]);
			}

			return output;
		}

		//converts string to string[] by splitting the string by new lines
		public static string[] StringToStringArray(string str){
			return str.Split('\n');
		}

		//converts string[] to string
		public static string StringArrayToString(string[] strs){
			string output = "";

			for(int i = 0; i < strs.Length; i++){
				output += strs[i] + "\n";
			}

			//remove the last \n, or just return an empty string otherwise
			return output.Length > 0 ? output.Remove(output.Length - 1) : "";
		}

		#endregion

		#region Helpful Functions

		//breaks a sentence/paragraph up by words, given a width
		public static string[] WordWrap(string sentence, int width){
			List<string> output = new List<string>();

			string[] words = sentence.Split(' ');

			string current = "";

			for(int i = 0; i < words.Length; i++){
				string w = words[i];

				if(w.Length > width){
					for(int j = 0; j < w.Length; j++){
						current += w[j];

						if(current.Length == width){
							output.Add(current);
							current = "";
						}
					}

					current += ' ';
				} else if (current.Length + w.Length > width){
					output.Add(current);
					current = w + ' ';
				} else {//just add the word
					current += w + ' ';
				}
			}

			if(!String.IsNullOrWhiteSpace(current)){
				output.Add(current);
			}

			return Fill(output.ToArray(), ' ', width);
		}

		//fills a string with a given char
		public static string Fill(string str, char c, int width){
			if (str.Length > width){
				return str.Substring(0, width);
			} else if (str.Length < width){
				return str + new string(c, Math.Max(0, width - str.Length));
			} else {//Length == width
				return str;
			}
		}

		//fills each string in an array with a given char
		public static string[] Fill(string[] strs, char c, int width){
			string[] output = new string[strs.Length];

			for(int i = 0; i < strs.Length; i++){
				output[i] = Fill(strs[i], c, width);
			}

			return output;
		}

		#endregion
	}
}