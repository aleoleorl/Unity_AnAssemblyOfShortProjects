using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Subject
	{
		private Danke dan;

		public GameObject one;
		public float x;
		public float y;
		public float w;
		public float h;
		public float squareSize;
		public float defaultW;
		public float defaultH;

		public Subject (int numOfImg)
		{
			squareSize = 100f;

			dan = Danke.getInstance ();
			one = newObject (dan.componentLinks [numOfImg]);
			Renderer rend = one.GetComponent<Renderer>();
			Vector2 size = rend.bounds.size;
			x = 0;
			y = 0;
			w = size.x;
			h = size.y;
			defaultW = size.x;
			defaultH = size.y;
		}

		private GameObject newObject(string obj)
		{
			return MonoBehaviour.Instantiate (Resources.Load(obj, typeof(GameObject))) as GameObject;
		}
	}



	public class Component
	{
		public Component()
		{
			
		}
	}
}

