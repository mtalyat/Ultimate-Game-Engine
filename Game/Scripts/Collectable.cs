using System;
using UltimateEngine;

namespace Game {
	public class Collectable : GameObject {
		Collider collider;

		public Collectable(string name, Image image) : base (name, image){

		}

		public override void OnStart(){
			collider = new Collider();
			collider.IsTrigger = true;

			AddComponent(collider);
		}

		public override void OnTrigger(GameObject go, int side){
			if(go.Tag == "Player"){//if player touches this, remove it
				Scene.Delete(this);
			}
		}
	}
}