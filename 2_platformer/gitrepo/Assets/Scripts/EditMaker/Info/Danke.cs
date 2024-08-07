using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

public class Danke 
{
	public StatusEnum editorButtons;
	public StatusEnum currentPosition;
	public List<string> componentLinks;
	public List<GameObject> componmetImages;
	public List<GameObject> componmetTexts;

	public List<Subject> items;

	public int idOfCreatedElement;

	public int prevIdOfCurrentSubject;
	public int idOfCurrentSubject;

	public string paramName;
	public string paramId;
	public float paramStartX;
	public float paramStartY;

	public List<ComponentCategory> componentCategory;

	private Danke()
	{
		editorButtons = StatusEnum.No;
		componentLinks = new List<string> ();
		componmetImages = new List<GameObject> ();
		componmetTexts = new List<GameObject> ();
		idOfCreatedElement = -1;
		currentPosition = StatusEnum.No;
		items = new List<Subject> ();
		idOfCurrentSubject = -1;
		prevIdOfCurrentSubject = -1;

		paramName = " ";
		paramId = " ";
		paramStartX = 0;
		paramStartY = 0;

		componentCategory = new List<ComponentCategory> ();
	}

	private static Danke dan;
	public static Danke getInstance()
	{
		if (dan == null) 
		{
			dan = new Danke ();
		}
		return dan;
	}

	public void clear()
	{
		editorButtons = StatusEnum.No;
		componentLinks = new List<string> ();
		componmetImages = new List<GameObject> ();
		componmetTexts = new List<GameObject> ();
		idOfCreatedElement = -1;
		currentPosition = StatusEnum.No;
		items = new List<Subject> ();
		idOfCurrentSubject = -1;
		prevIdOfCurrentSubject = -1;

		paramName = " ";
		paramId = " ";
		paramStartX = 0;
		paramStartY = 0;

		componentCategory = new List<ComponentCategory> ();
	}
}