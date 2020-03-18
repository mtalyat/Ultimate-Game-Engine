using System;
using System.Timers;
using System.Collections.Generic;

namespace UltimateEngine{
	[Serializable]
	public class Animator : Component{
		bool transparent = false;

		List<Animation> animations = new List<Animation>();

		[NonSerialized]
		private Timer updateTimer;
		int _speed = 100;
		public int Speed
		{
			get
			{
				return _speed;
			}
			set
			{
				_speed = value;
				if (updateTimer != null) updateTimer.Interval = value;
			}
		}

		public int Index { get; private set; } = -1;
		private int nextIndex = -1;
		public Animation Current { get; private set; }

		public bool FlippedH { get; private set; } = false;
		public bool FlippedV { get; private set; } = false;

		public Animator(){
			
		}

		private void StartTimer()
		{
			updateTimer = new Timer(_speed);
			updateTimer.Elapsed += OnElapsedEvent;
			updateTimer.Start();
		}

		//called by the updateTimer to change Images
		private void OnElapsedEvent(object source, ElapsedEventArgs e){
			Next();
		}

		public override void Wake()
		{
			if (updateTimer == null)
			{
				StartTimer();
				Play(0);
			}
		}

		public override void Start(){
			
		}

		//updates the Image of the GameObject
		public override void Update(){
			if (Current != null)
				GameObject.Image = Current.Current;
		}

		//advances to the Next Image of the Current Animation
		public void Next(){
			if(Current != null)
			{
				Current.Next();
				if(nextIndex >= 0 && Current.Index == 0)
				{
					Play(nextIndex);
					nextIndex = -1;
				}
			}
		}

		//adds an Animation
		public void Add(Animation anim){
			anim.SetTransparency(transparent);

			animations.Add(anim);

			//set Current if first animation in the Animator
			if(animations.Count == 1)
			{
				Play(0);
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

		//plays the current animation by name
		public void Play(string name){
			Play(animations.FindIndex(a => a.Name == name));
		}

		//plays the current animation by index
		public void Play(int index){
			if(index == Index || index < 0 || index >= animations.Count) return;//if already playing or out of range, stop

			Index = index;
			Current = animations[index];
			//might remove this:
			Current.Reset();
		}

		//plays an animation, runs it once, and then switches to another animation
		public void PlayOnce(string name, string after = "")
		{
			PlayOnce(animations.FindIndex(a => a.Name == name),
				animations.FindIndex(a => a.Name == after));
		}

		//plays an animaion, runs it once, and then switches to another animation
		public void PlayOnce(int index, int after)
		{
			int next = after < 0 ? Index : after;

			Play(index);
			nextIndex = next;
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

		public void SetTransparency(bool trans)
		{
			foreach(Animation anim in animations)
			{
				anim.SetTransparency(trans);
			}
		}
	}
}