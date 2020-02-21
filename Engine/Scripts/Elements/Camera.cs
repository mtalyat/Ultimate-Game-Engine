using System;

namespace UltimateEngine {
	public class Camera : GameObject {
		public static Camera MainCamera;

		public Point Position => Transform.Position;
		public double X => Transform.X;
		public double Y => Transform.Y;

		public Camera(string name = "") : base(name){
			if(MainCamera == null) MainCamera = this;
		}
	}
}