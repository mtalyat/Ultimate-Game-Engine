using System;
using System.Timers;
using System.Collections.Generic;

namespace UltimateEngine{
	[Serializable]
	public class Animator : Component{
		List<Animation> animations = new List<Animation>();

		[NonSerialized]
		private Timer updateTimer;
		public int Speed => (int)updateTimer.Interval;

		public int Index { get; private set; } = 0;
		public Animation Current { get; private set; }

		public bool FlippedH { get; private set; } = false;
		public bool FlippedV { get; private set; } = false;

		public Animator(int speed = 100){
			updateTimer = new Timer(speed);
			updateTimer.Elapsed += OnElapsedEvent;
			updateTimer.Start();

			Set(0);
		}

		//called by the updateTimer to change Images
		private void OnElapsedEvent(object source, ElapsedEventArgs e){
			Next();
		}

		public override void Start(){
			
		}

		//updates the Image of the GameObject
		public override void Update(){
			if(Current != null)
				GameObject.Image = Current.Current;
		}

		//advances to the Next Image of the Current Animation
		public void Next(){
			if(Current != null)
				Current.Next();
		}

		//adds an Animation
		public void Add(Animation anim){
			animations.Add(anim);

			//set Current if first animation in the Animator
			if(animations.Count == 1)
			{
				Set(0);
			}
		}

		//gets an animation by Index
		public Animation Get(int index){
			if(index < 0 || index >= animations.Count) return null;

			return animations[index];
		}

		//gets an animation by name
		public Animation Get(string name){
			return Get(animations.FindIndex(a => a.Name == name));
		}

		//removes an Animation by name
		public void Remove(string name){
			animations.Remove(Get(name));
		}

		//sets the current animation by name
		public void Set(string name){
			Set(animations.FindIndex(a => a.Name == name));
		}

		//sets the current animation by index
		public void Set(int index){
			if(index == Index || index < 0 || index >= animations.Count) return;//if already playing or out of range, stop

			Index = index;
			Current = animations[index];
		}

		//flips the animations left and right
		public void FlipHorizontal(){
			for(int i = 0; i < animations.Count; i++){
				animations[i].FlipHorizontal();
			}
			FlippedH = !FlippedH;
		}

		//flips the animations up and down
		public void FlipVertical(){
			for(int i = 0; i < animations.Count; i++){
				animations[i].FlipVertical();
			}
			FlippedV = !FlippedV;
		}
	}
}