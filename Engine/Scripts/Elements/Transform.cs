using System;
using System.Collections.Generic;

namespace UltimateEngine{
	/*
	* This class mananges positions and parenting/children of objects in
	* the scene.
	*/
	[Serializable]
	public class Transform{
		public static readonly Transform Origin = new Transform{
			Parent = null,
			GameObject = null
		};

		//the transform that changes where this transform is
		public Transform Parent { get; private set; }

		//the GameObject that the Transform is connected to
		public GameObject GameObject { get; private set; }

		//the children of this Transform
		public List<Transform> Children { get; private set; } = new List<Transform>();

		//position is not mananged by the transform, only local position is
		public Point Position {
			get => GetPosition();
			set => SetPosition(value);
		}
		public double X
		{
			get
			{
				return Position.X;
			}
			set
			{
				Position = new Point(value, Position.Y);
			}
		}
		public double Y
		{
			get
			{
				return Position.Y;
			}
			set
			{
				Position = new Point(Position.X, value);
			}
		}

		//determines where the Transform falls when being drawn
		//higher number = 'on top'
		private double layer = 0;
		public double Z {
			get {
				return layer;
			}
			set {
				SetZ(value);
			}
		}

		//local position is the position based on the parenting
		//if no parent, the transform is based on the origin: (0, 0)
		private Point _local = new Point();
		public Point LocalPosition { get => _local; set => _local = value; }
		public double LocalX => LocalPosition.X;
		public double LocalY => LocalPosition.Y;

		public Transform(GameObject go = null){
			GameObject = go;
		}

		//makes a copy of the transform
		public Transform(Transform t, GameObject go = null)
		{
			LocalPosition = t.LocalPosition;
			//Don't set parent
			GameObject = go;
		}

        #region Positioning

        //sets the position
        //inputs a world position, adjusts based on the parent
        public void SetPosition(Point p){
			if(Parent == null)
			{
				LocalPosition = p;
			} else
			{
				LocalPosition = p - Parent.Position;
			}
		}

		//adjusts the current position
		public void Move(Point p)
		{
			LocalPosition += p;
		}

		//returns the world position based on the parent
		public Point GetPosition(){
			if(Parent == null){//parent does not exist
				return LocalPosition;
			}
			//parent exists
			return Parent.Position + LocalPosition;
		}

        #endregion

        #region Layers

        //sets the new layer and adjusts position in parent list
        public void SetZ(double z){
			layer = z;

			if(Parent != null) Parent.UpdateChild(this);
		}

		//Brings this Transform to the front of the drawing order
		public void BringToFront()
		{
			Z = Parent.Children[Parent.Children.Count - 1].Z + 1;
		}

		//Pushes this Transform to the back of the drawing order
		public void SendToBack()
		{
			Z = Parent.Children[0].Z - 1;
		}

        #endregion

        #region Parenting

        //sets a new parent, removes self from parent's children
        //function redoes the local position so that even when a new parent is set,
        //it still reflects the same world position
        public void SetParent(Transform t){
			//if parent is null, make it not null
			if(Parent == null){
				Parent = Origin;
			}

			//get the world position
			Point w = Parent.Position + LocalPosition;

			//remove self from old parent
			Parent.Children.Remove(this);

			//set the parent
			//if the new parent is null, set to the origin
			Parent = t ?? Origin;

			//add self to new parent
			Parent.AddChild(this);

			//fix the local position based on the new parent
			LocalPosition = w - Parent.Position;
		}

		//separates self from current Parent
		public void RemoveFromParent(){
			if(Parent != null){
				Parent.Children.Remove(this);
			}
		}

		//inserts a child into the correct location in the Children array for drawing
		public void AddChild(Transform child){
			if(Children.Count <= 0){//if list is empty
				Children.Add(child);
				child.Parent = this;
				return;
			}

			//list is not empty:
			for(int i = 0; i < Children.Count; i++){
				if(child.Z <= Children[i].Z){
					Children.Insert(i, child);
					child.Parent = this;
					return;
				}
			}

			//made it this far, then the new transform goes at the end
			Children.Add(child);
			child.Parent = this;
		}

		//inserts several children using AddChild
		public void AddChildren(Transform[] childs)
		{
			for(int i = childs.Length - 1; i >= 0; i--)
			{
				AddChild(childs[i]);
			}
		}

		//removes a child
		public void RemoveChild(Transform child){
			Children.Remove(child);
		}

		//updates a child when Z changes
		public void UpdateChild(Transform child){
			RemoveChild(child);
			AddChild(child);
		}
		
		//gets all children of this Transform, and the children's children, and so on
		public Transform[] GetAllChildren(){
			List<Transform> trans = new List<Transform>(Children);

			foreach(Transform child in Children){
				trans.AddRange(child.GetAllChildren());
			}

			return trans.ToArray();
		}

		public Transform[] GetChildrenWithTag(string tag)
		{
			return Children.FindAll(t => t.GameObject.Tag == tag).ToArray();
		}

		public Transform[] GetChildrenOfType<T>()
		{
			return Children.FindAll(t => t.GameObject is T).ToArray();
		}

		public Transform[] RemoveAllChildren()
		{
			Transform[] ts = Children.ToArray();

			Children.Clear();

			return ts;
		}

		public Transform[] RemoveChildrenWithTag(string tag)
		{
			Transform[] childs = Children.FindAll(t => t.GameObject.Tag == tag).ToArray();

			Children.RemoveAll(t => t.GameObject.Tag == tag);

			return childs; 
		}

		public Transform[] RemoveChildrenOfType<T>()
		{
			Transform[] childs = Children.FindAll(t => t.GameObject is T).ToArray();

			Children.RemoveAll(t => t.GameObject is T);

			return childs;
		}

        #endregion

        public override string ToString(){
			return Position.ToString();
		}
	}
}