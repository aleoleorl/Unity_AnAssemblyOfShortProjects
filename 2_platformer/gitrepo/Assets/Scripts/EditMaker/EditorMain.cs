using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Xml;
using System.IO;
using UnityEngine.UI;
using AssemblyCSharp;

public class EditorMain : MonoBehaviour 
{
	public Camera camera1;
	private Bitte bit;
	private Danke dan;

	private GameObject bgComponent;
	private GameObject closeComponent;

	private GameObject canvasItem;
	private GameObject canvasCommon;
	private GameObject canvasComponent;

	private Statuses status;
	private Subjects subject;
	private Controllers controls;
	private Parameters parameters;

	void Start () 
	{
		bit = Bitte.getInstance ();
		dan = Danke.getInstance ();

		bit.cameraY = 0;
		dan.clear ();

		bgComponent = transform.Find ("BgComponent").gameObject;
		closeComponent = transform.Find ("CloseComponent").gameObject;
		bgComponent.SetActive (false);
		closeComponent.SetActive (false);

		canvasItem = transform.Find ("CanvasItem").gameObject;
		canvasItem.SetActive (false);
		canvasCommon = transform.Find ("CanvasCommon").gameObject;
		canvasCommon.SetActive (false);
		canvasComponent = transform.Find ("CanvasComponent").gameObject;
		canvasComponent.SetActive (false);

		status = new Statuses (canvasItem, canvasCommon, bgComponent, closeComponent, canvasComponent, this);
		subject = new Subjects (canvasItem);
		controls = new Controllers (camera1);
		parameters = new Parameters (canvasCommon);

		EditorFactory factory = new EditorFactory ();
		factory.work (this);
	}

	void Update () 
	{
		controls.work ();
		status.work ();
		subject.work ();
		parameters.work ();
	}
}