using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace UltimateEngine
{
	[Serializable]
	public class GameObject : SceneObject {
		public string Name { get; set; } = "";
		public string Tag { get; set; } = "";
		public bool Visible { get; set; } = true;

		//Position
		public Transform Transform { get; private set; }

		public Point ScreenPosition => Transform.Position.Floor();

		//Image
		public virtual Image Image { get; set; }
		public virtual Rect Bounds {
			get {
				return new Rect(Transform.Position, Image.Size);
			}
		}

		//GameObjects with a higher layer are printed 'on top'
		public double Layer
		{
			get
			{
				return Transform.Z;
			}
			set
			{
				Transform.Z = value;
			}
		}

		public bool IsStarted { get; private set; } = false;

		private List<Component> components = new List<Component>();

		public GameObject(string name = "GameObject", Image image = null){
			Name = name;
			Image = image ?? new Image();
			Transform = new Transform(this);

			SetScene(Scene.Current);
		}

		#region Starting and Updating

		public void Start(Scene scene){
			IsStarted = true;
			SetScene(scene);

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
		public virtual void OnCollision(GameObject go, Direction side){}
		public virtual void OnTrigger(GameObject go, Direction side){}

		#endregion

		#region Components

		//adds a component, and sets its GameObject to this
		public void AddComponent(Component c){
			c.GameObject = this;

			if(IsStarted){
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

        #region Scene Interaction

		//sets the Scene for this and all of its components
		public override void SetScene(Scene scene)
		{
			Scene = scene;

			foreach(Component c in components)
			{
				c.SetScene(scene);
			}
		}

        protected GameObject InstantiateChild(GameObject child)
		{
			return InstantiateChild(child, new Point(0, 0));
		}

		//Instantiates a child GameObject with this GameObject as the parent, into the scene
		protected GameObject InstantiateChild(GameObject child, Point position)
		{
			if (Scene == null) return null;

			return Scene.Instantiate(child, position, this.Transform);
		}

        #endregion

        public override string ToString(){
			return Name;
		}

		public GameObject Clone()
		{
			IFormatter formatter = new BinaryFormatter();
			Stream stream = new MemoryStream();
			using (stream)
			{
				formatter.Serialize(stream, this);
				stream.Seek(0, SeekOrigin.Begin);
				return (GameObject)formatter.Deserialize(stream);
			}
		}
	}
}