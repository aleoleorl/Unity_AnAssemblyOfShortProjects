using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Main : MonoBehaviour 
{
	private List<BaseObject> entities;
	private InVitro vit;
	private Bitte bit;
	private TextMesh attempts;

	void Start () 
	{
		vit = InVitro.getInstance ();
		bit = Bitte.getInstance ();
		entities = new List<BaseObject> ();
		Factory factory = new Factory ();
		if (!factory.make (entities))
		{
			StartCoroutine("Fade");
		}
		doInfoText ();

		GameObject innerText = transform.Find ("txtCount").gameObject;
		attempts = innerText.GetComponent<TextMesh> ();
		attempts.text = "0";
	}

	void Update () 
	{
		for (int i = 0; i < entities.Count; i++) 
		{
			entities [i].work ();
		}
		if (vit.btnChoice == BtnChoice.Back) 
		{
			vit.result = Result.Return;
			SceneManager.LoadScene(vit.previousScene);
		}
		if (bit.restart) 
		{
			for (int i = 0; i < entities.Count; i++) 
			{
				entities [i].destroy ();
			}

			bit.restart = false;
			bit.clear ();
			Factory factory = new Factory ();
			entities = new List<BaseObject> ();
			if (!factory.make (entities))
			{
				StartCoroutine("Fade"); 
			}
		}
		if (Convert.ToInt32 (attempts.text) != bit.tries) 
		{
			attempts.text = bit.tries.ToString();
		}

	}

	private void doInfoText()
	{
		GameObject innerText = transform.Find ("txt1").gameObject;
		var textMesh = innerText.GetComponent<TextMesh>();
		textMesh.text = "Level "+(vit.currentLevel+1)+": "+vit.level [vit.currentLevel].name;
		StartCoroutine("textOut");
	}

	IEnumerator Fade() 
	{
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene(vit.previousScene);
	}

	IEnumerator textOut() 
	{
		yield return new WaitForSeconds(3);
		GameObject innerText = transform.Find ("txt1").gameObject;
		var textMesh = innerText.GetComponent<TextMesh>();
		textMesh.text = "";
	}
}