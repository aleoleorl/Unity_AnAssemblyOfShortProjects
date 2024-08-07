using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AssemblyCSharp
{
	public class Statuses
	{
		private Danke dan;
		private GameObject canvasItem;
		private GameObject canvasCommon;
		private GameObject bgComponent;
		private GameObject closeComponent;
		private GameObject canvasComponent;
		private EditorMain one;

		public Statuses (GameObject canvasItem, GameObject canvasCommon, 
			GameObject bgComponent, GameObject closeComponent, GameObject canvasComponent,
			EditorMain one)
		{
			dan = Danke.getInstance ();
			this.canvasItem = canvasItem;
			this.canvasCommon = canvasCommon;
			this.bgComponent = bgComponent;
			this.closeComponent = closeComponent;
			this.canvasComponent = canvasComponent;
			this.one = one;
		}

		public void work()
		{
			if (dan.editorButtons == StatusEnum.Subject) 
			{
				subject ();
			}
			if (dan.editorButtons == StatusEnum.Close) 
			{
				close ();
			}
			if (dan.editorButtons == StatusEnum.Back) 
			{
				back ();
			}
			if (dan.editorButtons == StatusEnum.Parameters) 
			{
				parameters ();
			}
			if (dan.editorButtons == StatusEnum.Component) 
			{
				component ();
			}
		}

		private void subject() 
		{		
			if (dan.currentPosition == StatusEnum.Parameters) 
			{
				canvasCommon.SetActive (false);
			}
			bgComponent.SetActive (true);
			closeComponent.SetActive (true);
			dan.editorButtons = StatusEnum.No;
			dan.currentPosition = StatusEnum.Subject;

			for (int i = 0; i < dan.componmetImages.Count; i++) 
			{
				dan.componmetImages [i].SetActive (true);
				dan.componmetTexts [i].SetActive (true);
			}
		}
		private void close() 
		{
			if (dan.currentPosition == StatusEnum.Subject) 
			{
				dan.currentPosition = StatusEnum.No;

				bgComponent.SetActive (false);
				closeComponent.SetActive (false);
				dan.editorButtons = StatusEnum.No;
				for (int i = 0; i < dan.componmetImages.Count; i++) 
				{
					dan.componmetImages [i].SetActive (false);
					dan.componmetTexts [i].SetActive (false);
				}
			}
			if (dan.currentPosition == StatusEnum.Parameters) 
			{
				dan.currentPosition = StatusEnum.No;

				bgComponent.SetActive (false);
				closeComponent.SetActive (false);
				canvasCommon.SetActive (false);
				dan.editorButtons = StatusEnum.No;
			}
			if (dan.currentPosition == StatusEnum.Component)
			{
				bgComponent.SetActive (false);
				closeComponent.SetActive (false);
			}
		}
		private void back() 
		{
			SceneManager.LoadScene ("sceneMenu");
		}
		private void parameters() 
		{
			//Debug.Log ("!dan.currentPosition=" + dan.currentPosition);
			if (dan.currentPosition == StatusEnum.Subject || canvasItem.activeSelf) 
			{
				canvasItem.SetActive (false);
				for (int i = 0; i < dan.componmetImages.Count; i++) 
				{
					dan.componmetImages [i].SetActive (false);
					dan.componmetTexts [i].SetActive (false);
				}
				dan.idOfCurrentSubject = -1;
			}
			dan.currentPosition = StatusEnum.Parameters;

			bgComponent.SetActive (true);
			closeComponent.SetActive (true);
			canvasCommon.SetActive (true);

			dan.editorButtons = StatusEnum.No;		

			GameObject field = canvasCommon.transform.Find ("commonNameInput").gameObject;
			InputField fieldInput = field.GetComponent<InputField>();
			fieldInput.text = dan.paramName;

			field = canvasCommon.transform.Find ("commonIdInput").gameObject;
			fieldInput = field.GetComponent<InputField>();
			fieldInput.text = dan.paramId;

			field = canvasCommon.transform.Find ("commonStartXInput").gameObject;
			fieldInput = field.GetComponent<InputField>();
			fieldInput.text = dan.paramStartX.ToString();

			field = canvasCommon.transform.Find ("commonStartYInput").gameObject;
			fieldInput = field.GetComponent<InputField>();
			fieldInput.text = dan.paramStartY.ToString();
		}
		private void component()
		{
			if (dan.currentPosition != StatusEnum.Component) 
			{
				dan.currentPosition = StatusEnum.Component;
				bgComponent.SetActive (true);
				closeComponent.SetActive (true);

				Debug.Log ("dan.componentCategory.Count=" + dan.componentCategory.Count);
				for (int i = 0; i < dan.componentCategory.Count; i++)
				{
					DefaultControls.Resources uiResources = new DefaultControls.Resources ();
					GameObject uiButton = DefaultControls.CreateButton (uiResources);
					uiButton.transform.SetParent (one.transform, false);
					uiButton.transform.position = new Vector3 (Screen.width/2, Screen.height/2, 0f);

					//Button button = uiPanel.GetComponent<UnityEngine.UI.Button> ();
					//button.name = "s" + i.ToString ();
					//button.onClick.AddListener (() => FooOnClick (button));
				}
			}
		}

		void FooOnClick(Button button)
		{
			Debug.Log("Ta-Da!");
		}
	}
}