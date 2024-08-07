using System;
using UnityEngine;
using UnityEngine.UI;

namespace AssemblyCSharp
{
	public class Subjects
	{
		private Danke dan;
		private GameObject canvasItem;

		public Subjects (GameObject canvasItem)
		{
			dan = Danke.getInstance ();
			this.canvasItem = canvasItem;
		}

		public void work()
		{
			creationOfSubject ();
			checkOfSubject ();
		}

		private void creationOfSubject()
		{
			if (dan.idOfCreatedElement != -1) 
			{
				Subject item = new Subject (dan.idOfCreatedElement);
				dan.items.Add (item);

				dan.editorButtons = StatusEnum.Close;

				dan.idOfCreatedElement = -1;
			}
		}

		private void checkOfSubject()
		{
			if (dan.idOfCurrentSubject != dan.prevIdOfCurrentSubject)
			{
				dan.prevIdOfCurrentSubject = dan.idOfCurrentSubject;
				if (dan.idOfCurrentSubject == -1) 
				{
					canvasItem.SetActive (false);
					//canvasCommon.SetActive (false);
				} else 
				{
					GameObject field = canvasItem.transform.Find ("fieldX").gameObject;
					InputField fieldInput = field.GetComponent<InputField>();
					fieldInput.text = dan.items[dan.idOfCurrentSubject].x.ToString();

					field = canvasItem.transform.Find ("fieldY").gameObject;
					fieldInput = field.GetComponent<InputField>();
					fieldInput.text = dan.items[dan.idOfCurrentSubject].y.ToString();

					field = canvasItem.transform.Find ("fieldW").gameObject;
					fieldInput = field.GetComponent<InputField>();
					fieldInput.text = dan.items[dan.idOfCurrentSubject].w.ToString();

					field = canvasItem.transform.Find ("fieldH").gameObject;
					fieldInput = field.GetComponent<InputField>();
					fieldInput.text = dan.items[dan.idOfCurrentSubject].h.ToString();

					field = canvasItem.transform.Find ("fieldId").gameObject;
					fieldInput = field.GetComponent<InputField>();
					fieldInput.text = dan.idOfCurrentSubject.ToString();

					canvasItem.SetActive (true);
					//canvasCommon.SetActive (false);
				}
			}
			if (dan.idOfCurrentSubject != -1) 
			{
				GameObject field = canvasItem.transform.Find ("fieldX").gameObject;
				InputField fieldInput = field.GetComponent<InputField>();
				try
				{
					if (!fieldInput.text.Equals (dan.items [dan.idOfCurrentSubject].x)) 
					{
						dan.items [dan.idOfCurrentSubject].x = float.Parse(fieldInput.text);
						dan.items [dan.idOfCurrentSubject].one.transform.position = 
							new Vector3(float.Parse(fieldInput.text), dan.items [dan.idOfCurrentSubject].y, 1);
					}
				} catch
				{
					fieldInput.text = dan.items[dan.idOfCurrentSubject].x.ToString();
				}

				field = canvasItem.transform.Find ("fieldY").gameObject;
				fieldInput = field.GetComponent<InputField>();
				try
				{
					if (!fieldInput.text.Equals (dan.items [dan.idOfCurrentSubject].y)) 
					{
						dan.items [dan.idOfCurrentSubject].y = float.Parse(fieldInput.text);
						dan.items [dan.idOfCurrentSubject].one.transform.position = 
							new Vector3(dan.items [dan.idOfCurrentSubject].x, float.Parse(fieldInput.text), 1);
					}
				} catch
				{
					fieldInput.text = dan.items[dan.idOfCurrentSubject].y.ToString();
				}

				float w = 1f;
				float h = 1f;
				field = canvasItem.transform.Find ("fieldW").gameObject;
				fieldInput = field.GetComponent<InputField>();
				try
				{
					if (!fieldInput.text.Equals (dan.items [dan.idOfCurrentSubject].w.ToString())) 
					{
						w = float.Parse(fieldInput.text)/dan.items [dan.idOfCurrentSubject].defaultW;
						h = dan.items [dan.idOfCurrentSubject].h/dan.items [dan.idOfCurrentSubject].defaultH;
					}
				} catch
				{
					fieldInput.text = dan.items[dan.idOfCurrentSubject].w.ToString();
				}

				field = canvasItem.transform.Find ("fieldH").gameObject;
				fieldInput = field.GetComponent<InputField>();
				try
				{
					if (!fieldInput.text.Equals (dan.items [dan.idOfCurrentSubject].h.ToString())) 
					{
						h = float.Parse(fieldInput.text)/dan.items [dan.idOfCurrentSubject].defaultH;
						if (w==1f)
						{
							w = dan.items [dan.idOfCurrentSubject].w/dan.items [dan.idOfCurrentSubject].defaultW;
						}
					}
				} catch
				{
					fieldInput.text = dan.items[dan.idOfCurrentSubject].h.ToString();
				}

				if (w != 1f)
				{
					Vector3 temp = new Vector3 (w, h, 1f);
					dan.items [dan.idOfCurrentSubject].one.transform.localScale = temp;
				}
			}
		}
	}
}