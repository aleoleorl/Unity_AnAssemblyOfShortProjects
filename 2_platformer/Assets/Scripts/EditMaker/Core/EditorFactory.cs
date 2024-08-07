using System;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

namespace AssemblyCSharp
{
	public class EditorFactory
	{
		private Danke dan;

		public EditorFactory ()
		{
			dan = Danke.getInstance ();
		}

		public void work(EditorMain one)
		{
			TextAsset xmlAsset = Resources.Load<TextAsset> ("Information/" + "EditorContent");
			XmlDocument xmlDoc = new XmlDocument ();
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
			foreach (XmlNode sectionsNode in xmlNode)
			{
				if (sectionsNode.Name == "subject")
				{
					dan.componentLinks.Add (sectionsNode.Attributes ["path"].Value);
				}
				if (sectionsNode.Name == "componentCategory")
				{
					makeCategory (sectionsNode);
				}
			}

			makeSubjects (one);
		}

		private void makeCategory(XmlNode categoryNode)
		{
			dan.componentCategory.Add (new ComponentCategory (categoryNode.Attributes ["name"].Value,
				Convert.ToInt32(categoryNode.Attributes ["max"].Value)));
			int categoryI = dan.componentCategory.Count - 1;
			foreach (XmlNode componentNode in categoryNode)
			{
				ComponentInEditor temp = new ComponentInEditor (categoryNode.Attributes ["name"].Value);

				foreach (XmlNode paramNode in componentNode)
				{
					if (paramNode.Name == "param") 
					{
						ParameterInEditor paramTemp = new ParameterInEditor ();
						paramTemp.tip = paramNode.Attributes ["tip"].Value;
						paramTemp.multi = false;
						paramTemp.inner = false;
						paramTemp.name = paramNode.Attributes ["name"].Value;

						temp.parameters.Add (paramTemp);
					}
					if (paramNode.Name == "multiParam") 
					{
						ParameterInEditor paramTemp = new ParameterInEditor ();
						paramTemp.tip = "";
						paramTemp.multi = true;
						paramTemp.inner = false;
						paramTemp.name = paramNode.Attributes ["name"].Value;
						foreach (XmlNode attrNode in paramNode)
						{
							paramTemp.attrName.Add(attrNode.Attributes ["name"].Value);
							paramTemp.attrTip.Add(attrNode.Attributes ["tip"].Value);
						}
					}
					if (paramNode.Name == "innerParam") 
					{
						ParameterInEditor paramTemp = new ParameterInEditor ();
						paramTemp.name = paramNode.Attributes ["name"].Value;
						paramTemp.tip = paramNode.Attributes ["tip"].Value;
						paramTemp.multi = false;
						paramTemp.inner = true;
					}
					if (paramNode.Name == "action") 
					{
						EventPart action = new EventPart ();
						foreach (XmlNode actionNode in paramNode)
						{
							action.parts.Add (actionNode.InnerText);

						}
						temp.actions.Add (action);
					}
					if (paramNode.Name == "effect") 
					{
						EventPart effect = new EventPart ();
						foreach (XmlNode effectNode in paramNode)
						{
							effect.parts.Add (effectNode.InnerText);

						}
						temp.effects.Add (effect);
					}
				}

				dan.componentCategory [categoryI].components.Add (temp);
			}
		}

		private void makeSubjects(EditorMain one)
		{
			DefaultControls.Resources uiResources;
			GameObject uiPanel;
			Image temp;
			Vector3 pos;
			int x=150;
			int y=400;
			for (int i = 0; i < dan.componentLinks.Count; i++) 
			{			
				uiResources = new DefaultControls.Resources ();
				uiPanel = DefaultControls.CreateImage (uiResources);
				uiPanel.transform.SetParent (one.transform, false);

				pos = new Vector3 (x, y, 1);
				Sprite currentSprite = Resources.Load (dan.componentLinks[i], typeof(Sprite)) as Sprite;

				temp = uiPanel.GetComponent<Image> ();

				temp.sprite = currentSprite;
				temp.transform.position = pos;

				temp.rectTransform.sizeDelta = new Vector2(80f, 80f);
				uiPanel.name = dan.componentLinks [i];
				dan.componmetImages.Add (uiPanel);
				uiPanel = new GameObject ();
				dan.componmetImages[dan.componmetImages.Count-1].SetActive(false);

				DefaultControls.Resources uiResources2 = new DefaultControls.Resources();
				GameObject uiText = DefaultControls.CreateText(uiResources2);
				uiText.transform.SetParent(one.transform, false);
				Text fieldInput = uiText.GetComponent<Text>();
				fieldInput.text=dan.componentLinks [i];
				uiText.transform.position = new Vector3 (x+50f, y - 60f, 1f);
				dan.componmetTexts.Add (uiText);
				dan.componmetTexts [dan.componmetTexts.Count - 1].SetActive (false);

				x += 100;
				if (x == 650)
				{
					x = 150;
					y -= 100;
				}
			}
		}
	}
}