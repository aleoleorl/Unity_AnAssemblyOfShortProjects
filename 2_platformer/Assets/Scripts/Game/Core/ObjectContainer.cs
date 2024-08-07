using System;

namespace AssemblyCSharp
{	
	public class ObjectContainer
	{
		public bool show;
		public float x;
		public float y;
		public float width; // in size coordinats, not in pixels
		public float height;

		public float sensitivity;
		public float moveX;
		public float moveY;
		public bool moveAccess; //access or not movingCrossTarget
		public bool move;

		public bool fly;
		public bool upControl;

		public ObjectContainer()
		{
			show = true;
			x = 0;
			y = 0;
			width = 0.01f;
			height = 0.01f;

			sensitivity = 0.1f;
			moveX = 0;
			moveY = 0;
			moveAccess = false;
			move = true;

			fly = true;
			upControl = false;

		}
	}
}

