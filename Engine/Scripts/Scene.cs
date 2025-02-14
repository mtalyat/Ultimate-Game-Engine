using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace UltimateEngine
{
	[Serializable]
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
		public bool FullScreen { get; private set; } = false;

		public int DeltaTime { get; private set; } = 0;

		[NonSerialized]
		Thread updateThread;
		[NonSerialized]
		Thread drawingThread;

		ConsoleColor backgroundColor = ConsoleColor.Black;
		ConsoleColor foregroundColor = ConsoleColor.White;

		public bool Active { get; private set; } = false;

		List<GameObject> InScene = new List<GameObject>();

		//for debugging
		public bool DEBUG_MODE { get; set; } = false;
		public bool SLOW_MODE { get; set; } = false;
		public bool FRAMERATE_LIMIT { get; set; } = true;
		public bool PAUSED { get; set; } = false;

		//FPS stuff
		public static int GoalFPS { get; set; } = 60;
		public static int ScaledFPS => GoalFPS / 5;
		public int ActualFPS { get; private set; } = -1;
		private int framesPassed = 0;
		[NonSerialized]
		private System.Timers.Timer FPSTimer;

        #region Constructors

        public Scene(string name = "Untitled Scene")
		{
			Name = name;
			FullScreen = true;
			SetScreenSize(1000, 1000);//ensure largest size possible

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

			bool goDraw = false;

			updateThread = new Thread(new ThreadStart(() => {
				Stopwatch watch = new Stopwatch();
				while(Active){
					watch.Start();

					//update all objects, then update the screen to reflect changes
					UpdateAll();

					//check for collisions
					CollideAll();
					FixAll();

					while (goDraw) Thread.Sleep(0);

					//finally draw them all
					goDraw = true;

					//A frame has passed, so update that for the FPS
					framesPassed++;

					watch.Stop();
					DeltaTime = watch.Elapsed.Milliseconds;
					if (FRAMERATE_LIMIT)
					{
						Thread.Sleep(Math.Max(0, (int)((double)GoalFPS / 4.5) - DeltaTime * 2));
					}

					if (SLOW_MODE) Thread.Sleep(1000);

					while (PAUSED) Thread.Sleep(1);

					watch.Reset();
				}
			}));

			drawingThread = new Thread(new ThreadStart(() =>
			{
				while (Active)
				{
					while (!goDraw)
					{
						Thread.Sleep(0);
					}

					goDraw = false;

					DrawAll();

					//draw Debug stuff on top of Scene
					if (DEBUG_MODE)
					{
						ScreenBuffer.Draw("dT: " + DeltaTime, 0, 0);
						ScreenBuffer.Draw("GoalFPS: " + GoalFPS, 0, 1);
						ScreenBuffer.Draw("ActualFPS: " + ActualFPS, 0, 2);
						if (Basics.Player.Active != null) ScreenBuffer.Draw("P. Pos: " + Basics.Player.Active.Transform.Position, 0, 3);
					}

					ScreenBuffer.Print();

					while (PAUSED) Thread.Sleep(1);
				}
			}));

			WakeAll();
			StartAll();

			updateThread.Name = "Update";
			updateThread.Start();

			drawingThread.Name = "Drawing";
			drawingThread.Start();

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

        #region Instantiating/Deleting

        //spawns a new object in the scene
        public GameObject Instantiate(GameObject g){
			return Instantiate(g, g.Transform.Position);
		}

		//spawns a new GameObject in the Scene with a new position
		public GameObject Instantiate(GameObject g, Point position){
			return Instantiate(g, position, origin);
		}

		//spawns a new GameObject in the Scene with a new X and Y position
		public GameObject Instantiate(GameObject g, double x, double y)
		{
			return Instantiate(g, new Point(x, y));
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
		public GameObject Instantiate(GameObject original, Point position, Transform parent){
			if (original == null) return null;

			GameObject g = original.Clone();

			g.Transform.SetParent(parent);
			g.Transform.LocalPosition = position;

			InScene.Add(g);

			//instantiate all of the children as well
			for(int i = 0; i < original.Transform.Children.Count; i++)
			{
				Transform child = original.Transform.Children[i];
				g.Transform.AddChild(Instantiate(child.GameObject, child.LocalPosition, g.Transform).Transform);
			}

			if (Active)
			{
				Wake(g.Transform);
				Start(g.Transform);
			}

			return g;
		}

		//removes a GameObject from the scene
		public void Destroy(GameObject g){
			g.Transform.RemoveFromParent();

			int index = InScene.FindIndex(o => o.Name == g.Name);

			if(index >= 0){
				InScene.RemoveAt(index);
			}
		}

        #endregion

        #region GameObject Management

		public GameObject FindGameObjectByName(string name)
		{
			return InScene.Find(g => g.Name == name);
		}

		public GameObject[] FindGameObjectsWithTag(string tag)
		{
			return InScene.FindAll(g => g.Tag.Contains(tag)).ToArray();
		}

		public GameObject[] FindGameObjectsWithComponent<T>()
		{
			return InScene.FindAll(g => g.GetComponent<T>() != null).ToArray();
		}

		#endregion

		#region Updating, Starting, Colliding and Drawing

		//Wakes all of the gameobjects/assigns their Scene object
		private void WakeAll()
		{
			for(int i = 0; i < origin.Children.Count; i++)
			{
				Wake(origin.Children[i]);
			}
		}

		//starts a single GameObject
		private void Wake(Transform t)
		{
			if (t == null) return;

			GameObject go = t.GameObject;

			//update the GameObject
			if (go != null)
			{
				go.Wake(this);

				foreach (Transform child in t.Children)
				{
					Wake(child);
				}
			}
		}

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
				go.Start();

				for(int i = 0; i < t.Children.Count; i++)
				{
					Start(t.Children[i]);
				}
			}
		}

		//updates all of the GameObjects and their children
		//also draws them
		private void UpdateAll(){
			if (Camera.MainCamera == null) return;

			for(int i = 0; i < origin.Children.Count; i++){
				Update(origin.Children[i]);
			}
		}

		//updates one object
		private void Update(Transform t){
			if(t == null) return;

			GameObject go = t.GameObject;
			//update the GameObject
			if(go != null){
				go.Update();

				foreach(Transform child in t.Children){
					Update(child);
				}
			}
		}

		private void CollideAll()
		{
			for (int i = 0; i < InScene.Count; i++)
			{
				Collider one = InScene[i].GetComponent<Collider>();

				if (one == null) continue;
				if (one.IsKinematic()) continue;

				int count = 0;
				//check the other objects if this one has a collider and is moving
				for (int j = 0; j < InScene.Count; j++)
				{
					if (i == j) continue;//don't check self

					Collider two = InScene[j].GetComponent<Collider>();

					if (two == null) continue;

					//check if the collision will occur in the next frame
					if (one.CheckCollision(two)) count++;
				}
				if(count == 0 && one.Ignore != null)
				{
					one.Ignore = null;
				}
			}
		}

		//iterates through the objects again and makes sure none of them are inside of one another
		//all objects should already have their velocities fixed, so all of these collisions will be kinematic
		private void FixAll()
		{
			//run through backwards compared to the collisions, so placement is correct
			for (int i = InScene.Count - 1; i >= 0; i--)
			{
				Collider one = InScene[i].GetComponent<Collider>();

				if (one == null) continue;
				if (one.IsKinematic()) continue;

				//check the other objects if this one has a collider and is moving
				for (int j = 0; j < InScene.Count; j++)
				{
					if (i == j) continue;//don't check self

					Collider two = InScene[j].GetComponent<Collider>();

					if (two == null) continue;

					//find the one that is moving, and have it be fixed
					one.CheckFix(two);
				}
			}
		}

		//draws all of the GameObjects in view
		private void DrawAll()
		{
			for(int i = 0; i < origin.Children.Count; i++)
			{
				Draw(origin.Children[i]);
			}

			//finish by drawing the UI
			for(int i = 0; i < Camera.MainCamera.Transform.Children.Count; i++)
			{
				Draw(Camera.MainCamera.Transform.Children[i]);
			}
		}

		private void Draw(Transform t)
		{
			if (t == null) return;

			GameObject go = t.GameObject;

			//update the GameObject
			if (go != null)
			{
				//don't need to draw if it is not visible
				if (!go.Visible) return;

				//draw the Camera last
				if (go is Camera) return;

				if (go.Image.SupportsTransparency)
					ScreenBuffer.TransparentDraw(go.Image.RawData, go.Image.Size, go.ScreenPosition);
				else
					ScreenBuffer.Draw(go.Image.RawData, go.Image.Size, go.ScreenPosition);

				foreach (Transform child in t.Children)
				{
					Draw(child);
				}
			}
		}

        #endregion

        #region Statics

		private static void SetCurrentScene(Scene scene)
		{
			if(current != null) current.Stop();

			current = scene;

			ScreenBuffer.Initialize(current.screenSize, current.Name, current.FullScreen);
			ScreenBuffer.SetColors(current.foregroundColor, current.backgroundColor);
		}

		public static void SwitchScenes(Scene newScene)
		{
			if (newScene == current) return;

			if(current != null)
			{
				current.Stop();
			}

			current = newScene;

			ScreenBuffer.Initialize(current.screenSize, current.Name, current.FullScreen);
			ScreenBuffer.SetColors(current.foregroundColor, current.backgroundColor);

			current.Run();
		}

        #endregion
    }
}