using System;

namespace UltimateEngine {
	//this class is the base class for components
	//components are elements that can be added to GameObjects
	public abstract class Component{
		public abstract GameObject GameObject { get; set; }

		public Component(){

		}

		//called on the GameObject's start, or when the GameObject is instantiated
		public abstract void Start();

		//called on the GameObject's update
		public abstract void Update();
	}
}