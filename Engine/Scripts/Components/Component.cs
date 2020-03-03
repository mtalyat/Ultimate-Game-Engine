using System;

namespace UltimateEngine {
	//this class is the base class for components
	//components are elements that can be added to GameObjects
	public abstract class Component{
		//GameObject is assigned when added to the GameObject
		public GameObject GameObject { get; set; }
		public string Name
		{
			get
			{
				return GameObject.Name;
			}
		}

		public Component(){

		}

		//called on the GameObject's start, or when the GameObject is instantiated
		public abstract void Start();

		//called on the GameObject's update
		public abstract void Update();

		public override string ToString()
		{
			return Name;
		}
	}
}