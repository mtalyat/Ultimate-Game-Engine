using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;

namespace UltimateEngine{
	public class Scene{
		public static Scene Current;

		private Transform Origin => Transform.Origin;
		
		private Stopwatch watch = new Stopwatch();
		public int DeltaTime { get; private set; } = 0;

		Thread updateThread;
		Thread collisionsThread;

		public bool Active { get; private set; } = false;

		List<GameObject> InScene = new List<GameObject>();

		//for debugging
		public bool DebugMode { get; set; } = true;
		public int FPS { get; set; } = 30;

		public Scene(int width = 100, int height = 20)
		{
			ScreenBuffer.Initialize(new Size(width, height));

			Current = this;
		}

		public Scene(Size s){
			ScreenBuffer.Initialize(s);

			Current = this;
		}

		//starts the Scene
		public void Run(){
			Input.Start();

			Active = true;

			updateThread = new Thread(new ThreadStart(() => {
				while(Active){
					watch.Start();

					//update all objects, then update the screen to reflect changes
					UpdateAll();

					//draw Debug stuff
					if(DebugMode){
						ScreenBuffer.Draw("dT: " + DeltaTime, 0, 0);
						ScreenBuffer.Draw("FPS: " + FPS, 0, 1);
					}

					ScreenBuffer.Print();

					watch.Stop();
					DeltaTime = watch.Elapsed.Milliseconds;
					Thread.Sleep(Math.Max(0, (1000 / FPS) - DeltaTime));//try to make up for when it takes longer to draw objects
					watch.Reset();
				}
			}));

			//putting collisions in its own thread so it does not slow down update when there are many objects in the Scene
			collisionsThread = new Thread(new ThreadStart(() => {
				while(Active){
					for(int i = 0; i < InScene.Count; i++){
						Collider one = InScene[i].GetComponent<Collider>();

						if(one == null) continue;
						if(!one.IsMoving()) continue;

						for(int j = 0; j < InScene.Count; j++){
							Collider two = InScene[j].GetComponent<Collider>();

							if(two == null) continue;
							if((two.IsMoving() && j < i) || j == i) continue;//don't check self or any that has been checked

							//check if the collision will occur in the next frame
							one.CheckCollision(two, 4);
						}
					}
				}
			}));

			StartAll();

			updateThread.Name = "Update";
			updateThread.Start();

			collisionsThread.Name = "Collisions";
			collisionsThread.Start();
		}

		public void Stop(){
			Active = false;
			updateThread = null;
		}

		#region Management

		//spawns a new object in the scene
		public void Instantiate(GameObject g){
			Instantiate(g, g.Transform.Position);
		}

		//spawns a new object in the scene with a new position
		public void Instantiate(GameObject g, Point position){
			Instantiate(g, position, Origin);
		}

		public void Instantiate(GameObject g, Transform parent)
		{
			Instantiate(g, new Point(0, 0), parent);
		}

		//spawns a new object in the scene with a new position and parent
		public void Instantiate(GameObject g, Point position, Transform parent){
			if (g == null) return;

			g.Transform.SetParent(parent);
			g.Transform.SetPosition(position);

			InScene.Add(g);

			if(Active)
				Start(g.Transform);
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
			foreach(Transform t in Origin.GetAllChildren()){
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
			for(int i = 0; i < Origin.Children.Count; i++){
				Update(Origin.Children[i]);
			}
		}

		//updates one object
		private void Update(Transform t){
			if(t == null) return;

			GameObject go = t.GameObject;
			//update the GameObject
			if(go != null){
				go.Update();
				ScreenBuffer.Draw(go.Image.ToJaggedArray(), go.Image.Size, t.Position);

				foreach(Transform child in t.Children){
					Update(t);
				}
			}
		}

		#endregion
	}
}