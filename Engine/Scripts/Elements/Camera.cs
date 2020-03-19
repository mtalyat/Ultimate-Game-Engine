using System;
using System.Collections.Generic;

namespace UltimateEngine {
	[Serializable]
	public class Camera : GameObject {
		public static Camera MainCamera { get; set; }

		public override Point ScreenPosition => Transform.Position.Floor();

		static List<Camera> allCameras = new List<Camera>();
		public static int CameraCount => allCameras.Count;

		public override Rect Bounds
		{
			get
			{
				return new Rect(Transform.Position, ScreenBuffer.Size);
			}
		}

		public Camera(string name = "Camera") : base(name){
			Tag = "Camera";

			if(MainCamera == null) MainCamera = this;

			allCameras.Add(this);
		}

		//when the Camera deletes itself, automatically remove it from the list
		~Camera()
		{
			allCameras.Remove(this);
		}

		//moves to the next Camera in the scene
		public static void NextCamera()
		{
			if (CameraCount <= 0) return;

			int index = allCameras.IndexOf(MainCamera);

			if(index < 0)
			{
				MainCamera = allCameras[0];
			} else
			{
				MainCamera = allCameras[(index + 1) % CameraCount];
			}
		}
	}
}