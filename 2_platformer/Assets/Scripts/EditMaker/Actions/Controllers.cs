using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Controllers
	{
		private Bitte bit;
		private Danke dan;
		private Camera camera1;

		public Controllers (Camera camera1)
		{
			bit = Bitte.getInstance ();
			dan = Danke.getInstance ();
			this.camera1 = camera1;
		}

		public void work()
		{
			btns ();
			mouseDown ();
		}

		private void btns()
		{
			if (Input.GetKey("w"))
			{
				bit.cameraY += 0.1f;
			}
			if (Input.GetKey("s"))
			{
				bit.cameraY -= 0.1f;
			}

			if (bit.cameraY >= 0) 
			{
				camera1.transform.position = new Vector3 (camera1.transform.position.x, bit.cameraY, camera1.transform.position.z);
			} else 
			{
				bit.cameraY = 0;
			}
		}

		private void mouseDown()
		{
			if (Input.GetMouseButtonDown (0))
			{
				if (dan.currentPosition == StatusEnum.Subject)
				{	
					Vector3 pos = Input.mousePosition;
					int x = 150;
					int y = 400;
					for (int i = 0; i < dan.componmetImages.Count; i++)
					{
						if (pos.x > x - 40f && pos.x < x + 40f &&
							pos.y > y - 40f && pos.y < y + 40f)
						{
							dan.idOfCreatedElement = i;
							break;
						}

						x += 100;
						if (x == 650) 
						{
							x = 150;
							y -= 100;
						}
					}
				}

				if (dan.currentPosition == StatusEnum.No)
				{
					Vector3 pos = Input.mousePosition;
					bool found = false;

					for (int j = 0; j < dan.items.Count; j++)
					{
						Vector2 size = dan.items[j].one.GetComponent<SpriteRenderer>().sprite.rect.size;
						Vector3 screenPos = camera1.WorldToScreenPoint(dan.items [j].one.transform.position);
						if (pos.x > screenPos.x - dan.items [j].w / 4 * dan.items [j].squareSize &&
							pos.x < screenPos.x + dan.items [j].w / 4 * dan.items [j].squareSize &&
							pos.y > screenPos.y - dan.items [j].h / 4 * dan.items [j].squareSize &&
							pos.y < screenPos.y + dan.items [j].h / 4 * dan.items [j].squareSize) 
						{
							if (j != dan.idOfCurrentSubject)
							{
								dan.idOfCurrentSubject = j;
								found = true;
								break;
							}
						}
					}
					if (!found && pos.y>54) 
					{
						dan.idOfCurrentSubject = -1;
					}
				}
			}	
		}
	}
}