using System;
using System.Collections.Generic;

namespace UltimateEngine{
	public class GameObject{
		public string Name { get; set; } = "";
		public string Tag { get; set; } = "";

		//the Scene the GameObject is in
		public Scene Scene { get; private set; }

		//Position
		public Transform Transform { get; private set; }

		public Point ScreenPosition => Transform.Position.Ceiling();

		//Image
		public Image Image { get; set; }

		public Rect Bounds {
			get {
				return new Rect(Transform.Position, Image.Size);
			}
		}

		//GameObjects with a higher layer are printed 'on top'
		public int Layer { get; set; } = 0;

		private bool started = false;

		private List<Component> components = new List<Component>();

		public GameObject(string name = "GameObject", Image image = null){
			Name = name;
			Image = image ?? new Image();
			Transform = new Transform(this);

			Scene = Scene.Current;
		}

		#region Starting and Updating

		public void Start(Scene scene){
			started = true;
			Scene = scene;

			foreach(Component c in components){
				c.Start();
			}
			OnStart();
		}

		public void Update(){
			foreach(Component c in components){
				c.Update();
			}
			OnUpdate();
		}

		#endregion

		#region Virtuals

		public virtual void OnStart(){}
		public virtual void OnUpdate(){}
		public virtual void OnCollision(GameObject go, int side){}
		public virtual void OnTrigger(GameObject go, int side){}

		#endregion

		#region Components

		//adds a component, and sets its GameObject to this
		public void AddComponent(Component c){
			c.GameObject = this;

			if(started){
				c.Start();
			}

			components.Add(c);
		}

		//gets a component based 
		public T GetComponent<T>(){
			foreach(Component comp in components)
			{
				if(comp is T t)
				{
					//found a match, so, return it
					return t;
				}
			}
			return default;//typically null if nothing found
		}

		//removes the first instance of a type of Component
		public void RemoveComponent<T>()
		{
			for(int i = 0; i < components.Count; i++)
			{
				if(components[i] is T)
				{
					components.Remove(components[i]);
					return;
				}
			}
		}

		#endregion

		protected void InstantiateChild(GameObject child)
		{
			InstantiateChild(child, new Point(0, 0));
		}

		//Instantiates a child GameObject with this GameObject as the parent, into the scene
		protected void InstantiateChild(GameObject child, Point position)
		{
			if (Scene == null) return;

			Scene.Instantiate(child, position, this.Transform);
		}

		public override string ToString(){
			return Name;
		}
	}
}