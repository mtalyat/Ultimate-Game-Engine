using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;

namespace UltimateEngine{
	public class Scene{
		private static Scene current;
		public static Scene Current
		{
			get
			{
				return current;
			}
			set
			{
				SetCurrentScene(value);
			}
		}

		public string Name { get; set; } = "";

		private Transform origin = new Transform();
		private Size screenSize;
		
		private Stopwatch watch = new Stopwatch();
		public int DeltaTime { get; private set; } = 0;

		Thread updateThread;
		Thread collisionsThread;

		ConsoleColor backgroundColor;
		ConsoleColor foregroundColor;

		const int PredictionFrames = 1;

		public bool Active { get; private set; } = false;

		List<GameObject> InScene = new List<GameObject>();

		//for debugging
		public bool DebugMode { get; set; } = false;

		//FPS stuff
		public int GoalFPS { get; set; } = 30;
		public int ActualFPS { get; private set; } = -1;
		private int framesPassed = 0;
		private System.Timers.Timer FPSTimer;

		//gravity stuff
		public double Gravity { get; set; } = 9.8;
		public double AdjustedGravity => Gravity / GoalFPS;

        #region Constructors

        public Scene(string name = "Untitled Scene")
		{
			Name = name;
			SetScreenSize(1000, 1000);//ensure largest size possibe

			if (current == null) SetCurrentScene(this);
		}

		public Scene(int width, int height, string name = "Untitled Scene")
		{
			Name = name;
			SetScreenSize(width, height);

			if (current == null) SetCurrentScene(this);
		}

		public Scene(Size s, string name = "Untitled Scene")
		{
			Name = name;
			SetScreenSize(s.Width, s.Height);

			if (current == null) SetCurrentScene(this);
		}

        #endregion

        #region Running

        //starts the Scene
        public void Run(){
			Input.Start();

			Active = true;

			FPSTimer = new System.Timers.Timer();
			FPSTimer.Elapsed += FPSTimer_Elapsed;
			FPSTimer.Interval = 1000;//1 second

			updateThread = new Thread(new ThreadStart(() => {
				while(Active){
					watch.Start();

					//update all objects, then update the screen to reflect changes
					UpdateAll();

					//draw Debug stuff on top of Scene
					if(DebugMode){
						ScreenBuffer.Draw("dT: " + DeltaTime, 0, 0);
						ScreenBuffer.Draw("GoalFPS: " + GoalFPS, 0, 1);
						ScreenBuffer.Draw("ActualFPS: " + ActualFPS, 0, 2);
					}
					
					ScreenBuffer.Print();

					//A frame has passed, so update that for the FPS
					framesPassed++;

					watch.Stop();
					DeltaTime = watch.Elapsed.Milliseconds;
					Thread.Sleep(Math.Max(0, (1000 / (GoalFPS * 2)) - DeltaTime));//try to make up for when it takes longer to draw objects
					watch.Reset();
				}
			}));

			//putting collisions in its own thread so it does not slow down update when there are many objects in the Scene
			collisionsThread = new Thread(new ThreadStart(() => {
				while (Active){
					for(int i = 0; i < InScene.Count; i++){
						Collider one = InScene[i].GetComponent<Collider>();

						if(one == null) continue;
						if(!one.IsMoving()) continue;

						for(int j = 0; j < InScene.Count; j++){
							Collider two = InScene[j].GetComponent<Collider>();

							if(two == null) continue;
							if((two.IsMoving() && j < i) || j == i) continue;//don't check self or any that has been checked

							//check if the collision will occur in the next frame
							one.CheckCollision(two, PredictionFrames);
						}
					}
				}
			}));

			StartAll();

			updateThread.Name = "Update";
			updateThread.Start();

			collisionsThread.Name = "Collisions";
			collisionsThread.Start();

			FPSTimer.Start();
		}

		private void FPSTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			ActualFPS = framesPassed;
			framesPassed = 0;
		}

		public void Stop(){
			Active = false;

			if(FPSTimer != null)
			{
				FPSTimer.Stop();
				FPSTimer = null;
			}
			if(updateThread != null)
			{
				while (updateThread.ThreadState != System.Threading.ThreadState.Stopped)
				{
					Thread.Sleep(1);
				}
				updateThread = null;
			}
			if(collisionsThread != null)
			{
				while (collisionsThread.ThreadState != System.Threading.ThreadState.Stopped)
				{
					Thread.Sleep(1);
				}
				collisionsThread = null;
			}
		}

        #endregion

        #region Display

        public void SetColors(ConsoleColor fg, ConsoleColor bg)
		{
			foregroundColor = fg;
			backgroundColor = bg;

			ScreenBuffer.SetColors(fg, bg);
		}

		private void SetScreenSize(int width, int height)
		{
			screenSize = new Size(Math.Min(width, Console.LargestWindowWidth - 1),
				Math.Min(height, Console.LargestWindowHeight - 1));
		}

        #endregion

        #region GameObject Management

        //spawns a new object in the scene
        public GameObject Instantiate(GameObject g){
			return Instantiate(g, g.Transform.Position);
		}

		//spawns a new object in the scene with a new position
		public GameObject Instantiate(GameObject g, Point position){
			return Instantiate(g, position, origin);
		}

		public GameObject Instantiate(GameObject g, Transform parent)
		{
			return Instantiate(g, new Point(0, 0), parent);
		}

		public GameObject Instantiate(GameObject g, Transform parent, Point position)
		{
			return Instantiate(g, position, parent);
		}

		//spawns a new object in the scene with a new position and parent
		public GameObject Instantiate(GameObject g, Point position, Transform parent){
			if (g == null) return null;

			g.Transform.SetParent(parent);
			g.Transform.LocalPosition = position;

			InScene.Add(g);

			if(Active)
				Start(g.Transform);

			return g;
		}

		//removes a GameObject from the scene
		public void Delete(GameObject g){
			g.Transform.RemoveFromParent();

			int index = InScene.FindIndex(o => o.Name == g.Name);

			if(index >= 0){
				InScene.RemoveAt(index);
			}
		}

		#endregion

		#region Updating and Starting

		//starts all of the GameObjects already in the Scene before the program is ran
		private void StartAll(){
			foreach(Transform t in origin.GetAllChildren()){
				Start(t);
			}
		}

		//starts a single GameObject
		private void Start(Transform t){
			if(t == null) return;

			GameObject go = t.GameObject;

			//update the GameObject
			if(go != null){
				go.Start(this);

				foreach(Transform child in t.Children){
					Start(child);
				}
			}
		}

		//updates all of the GameObjects and their children
		//also draws them
		private void UpdateAll(){
			if (Camera.MainCamera == null) return;

			Point offset = Camera.MainCamera.ScreenPosition;

			for(int i = 0; i < origin.Children.Count; i++){
				Update(origin.Children[i], offset);
			}
		}

		//updates one object
		private void Update(Transform t, Point offset){
			if(t == null) return;

			GameObject go = t.GameObject;
			//update the GameObject
			if(go != null){
				go.Update();
				ScreenBuffer.Draw(go.Image.RawData, go.Image.Size, go.ScreenPosition - offset);

				foreach(Transform child in t.Children){
					Update(child, offset);
				}
			}
		}

        #endregion

        #region Statics

		private static void SetCurrentScene(Scene scene)
		{
			if (current == scene) return;

			if(current != null) current.Stop();

			current = scene;

			ScreenBuffer.Initialize(current.screenSize, current.Name);
			ScreenBuffer.SetColors(current.foregroundColor, current.backgroundColor);
		}

		public static void SwitchScenes(Scene newScene)
		{
			if (current == newScene) return;

			if(current != null)
			{
				current.Stop();
			}

			current = newScene;

			ScreenBuffer.Initialize(current.screenSize, current.Name);
			ScreenBuffer.SetColors(current.foregroundColor, current.backgroundColor);

			current.Run();
		}

        #endregion
    }
}