using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Xml;
using System.IO;

public class Menu : MonoBehaviour 
{
	private InVitro vit;
	private Bitte bit;

	public List<GameObject> mainMenu;
	public List<GameObject> levelChoose;
	private List<GameObject> btns;

	void Start () 
	{
		vit = InVitro.getInstance ();
		bit = Bitte.getInstance ();
		vit.previousScene="sceneMenu";
		doStart ();
		visual ();
	}

	void Update () 
	{
		visual ();
		if (vit.btnChoice == BtnChoice.NewGame)
		{
			vit.currentLevel = 0;
		}
		if (vit.btnChoice == BtnChoice.Default) 
		{
			vit.btnChoice = BtnChoice.No;
			for (int i = 0; i < btns.Count; i++) 
			{
				var script = btns[i].GetComponent<BtnWork> ();
				if (script.clicked) 
				{
					script.clicked = false;

					bit.tries = 0;
					vit.btnChoice = BtnChoice.NewGame;
					vit.result = Result.No;
					vit.currentLevel = i;
					vit.xmlAsset = Resources.Load<TextAsset>("Information/"+vit.level[i].xmlName);
					SceneManager.LoadScene("scene1");

					break;
				}
			}
		}
		if (vit.btnChoice == BtnChoice.Continue) 
		{
			vit.btnChoice = BtnChoice.NewGame;
			vit.result = Result.No;

			vit.xmlAsset = Resources.Load<TextAsset>("Information/"+vit.level[vit.currentLevel].xmlName);
			SceneManager.LoadScene("scene1");
		}
		if (vit.btnChoice == BtnChoice.Exit)
		{
			Application.Quit ();
		} 
		if (vit.btnChoice == BtnChoice.Edit)
		{
			vit.btnChoice = BtnChoice.No;
			vit.result = Result.No;
			SceneManager.LoadScene("sceneEditor");
		} 
	}

	private void visual()
	{
		if (vit.btnChoice == BtnChoice.NewGame)
		{
			vit.btnChoice = BtnChoice.No;
			for (int i = 0; i < levelChoose.Count; i++) 
			{
				levelChoose[i].SetActive (true);
			}
			for (int i = 0; i < mainMenu.Count; i++) 
			{
				mainMenu[i].SetActive (false);
			}
			for (int i = 0; i < btns.Count; i++) 
			{
				btns [i].SetActive (true);
			}

		}
		if (vit.btnChoice == BtnChoice.Back)
		{
			vit.btnChoice = BtnChoice.No;
			for (int i = 0; i < levelChoose.Count; i++) 
			{
				levelChoose[i].SetActive (false);
			}
			for (int i = 0; i < mainMenu.Count; i++) 
			{
				mainMenu[i].SetActive (true);
			}
			for (int i = 0; i < btns.Count; i++) 
			{
				btns [i].SetActive (false);
			}
		}
	}

	private void doStart()
	{
		//Debug.Log (vit.result);
		bool change = false;
		btns = new List<GameObject> ();
		if (vit.result == Result.Return)
		{
			vit.btnChoice = BtnChoice.NewGame;
			vit.result = Result.No;
			change = true;
		}
		if (vit.result == Result.No) 
		{
			vit.result = Result.No;
			vit.level = new List<LevelInfo>();
			TextAsset xmlAsset = Resources.Load<TextAsset> ("Information/"+vit.mainName);

			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml (xmlAsset.text);
			XmlNode xmlNode = null;
			for (int i = 0; i < xmlDoc.ChildNodes.Count; i++) 
			{
				if (xmlDoc.ChildNodes [i].Name == "game") 
				{
					xmlNode = xmlDoc.ChildNodes [i];
					break;
				}
			}
			foreach (XmlNode sectionsNode in xmlNode) //level
			{
				if (sectionsNode.Name == "level")
				{
					vit.level.Add( new LevelInfo (sectionsNode.Attributes["name"].Value, sectionsNode.InnerText));
				}
			}

			vit.result = Result.Return;

			int x = 0;
			int y = 0;
			for (int i = 0; i < vit.level.Count; i++)
			{
				y++;
				if (y == 4)
				{
					y = 0;
					x++;
				}
				GameObject temp = Instantiate (Resources.Load("Main/btn", typeof(GameObject))) as GameObject;
				temp.transform.position = new Vector2 (-4f + x * 4f, 3 - y * 1.2f);
				temp.transform.localScale = new Vector3 (2f, 2f, 1f);
				GameObject innerText = temp.transform.Find ("txt").gameObject;
				var textMesh = innerText.GetComponent<TextMesh>();
				var script = temp.GetComponent<BtnWork> ();
				script.text = vit.level [i].name;
				textMesh.text = vit.level[i].name;
				btns.Add (temp);
			}
			if (!change) 
			{
				vit.btnChoice = BtnChoice.Back;
			}
		} 
		if (vit.result == Result.Win) 
		{
			vit.btnChoice = BtnChoice.NewGame;
			vit.result = Result.No;
			vit.currentLevel++;
			if (vit.level.Count <= vit.currentLevel) 
			{
				vit.currentLevel = 0;
			}
			vit.xmlAsset = Resources.Load<TextAsset> ("Information/" + vit.level [vit.currentLevel].xmlName);
			SceneManager.LoadScene("scene1");
		}
	}
}