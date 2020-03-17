﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateEngine
{
    [Serializable]
    public class SceneObject
    {
        [NonSerialized]
        private Scene _scene;
        public Scene Scene
        {
            get
            {
                return _scene;
            }
            protected set
            {
                _scene = value;
            }
        }

        public SceneObject()
        {

        }

        public virtual void SetScene(Scene scene)
        {
            Scene = scene;
        }

        #region Instantiating/Destroying

        protected void Instantiate(GameObject go, Point position, Transform parent = null)
        {
            Scene.Instantiate(go, position, parent);
        }

        protected void Instantiate(GameObject go, Transform parent = null)
        {
            Scene.Instantiate(go, parent);
        }

        protected void Instantiate(GameObject go, double x, double y)
        {
            Scene.Instantiate(go, x, y);
        }

        protected void Destroy(GameObject go)
        {
            Scene.Destroy(go);
        }

        #endregion

        #region Finding

        public GameObject FindGameObjectByName(string name)
        {
            return Scene.FindGameObjectByName(name);
        }

        public GameObject[] FindGameObjectsWithTag(string tag)
        {
            return Scene.FindGameObjectsWithTag(tag);
        }

        public GameObject[] FindGameObjectsWithComponent<T>()
        {
            return Scene.FindGameObjectsWithComponent<T>();
        }

        #endregion
    }
}