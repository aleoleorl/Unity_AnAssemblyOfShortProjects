using System;
using UnityEngine;
using UnityEngine.UI;

namespace AssemblyCSharp
{
	public class Parameters
	{
		private Danke dan;
		private GameObject canvasCommon;

		public Parameters (GameObject canvasCommon)
		{
			dan = Danke.getInstance ();
			this.canvasCommon = canvasCommon;
		}

		public void work()
		{
			checkParameters ();
		}

		private void checkParameters()
		{
			if (dan.idOfCurrentSubject == -1) 
			{
				GameObject field = canvasCommon.transform.Find ("commonNameInput").gameObject;
				InputField fieldInput = field.GetComponent<InputField>();
				try
				{
					if (!fieldInput.text.Equals (dan.paramName)) 
					{
						dan.paramName = fieldInput.text;
					}
				} catch
				{
					fieldInput.text = dan.paramName;
				}

				field = canvasCommon.transform.Find ("commonIdInput").gameObject;
				fieldInput = field.GetComponent<InputField>();
				try
				{
					if (!fieldInput.text.Equals (dan.paramId)) 
					{
						dan.paramId = fieldInput.text;
					}
				} catch
				{
					fieldInput.text = dan.paramId;
				}

				field = canvasCommon.transform.Find ("commonStartXInput").gameObject;
				fieldInput = field.GetComponent<InputField>();
				try
				{
					if (!fieldInput.text.Equals (dan.paramStartX.ToString())) 
					{
						dan.paramStartX = float.Parse(fieldInput.text);
					}
				} catch
				{
					fieldInput.text = dan.paramId.ToString();
				}

				field = canvasCommon.transform.Find ("commonStartYInput").gameObject;
				fieldInput = field.GetComponent<InputField>();
				try
				{
					if (!fieldInput.text.Equals (dan.paramStartY.ToString())) 
					{
						dan.paramStartY = float.Parse(fieldInput.text);
					}
				} catch
				{
					fieldInput.text = dan.paramStartY.ToString();
				}
			}
		}
	}
}