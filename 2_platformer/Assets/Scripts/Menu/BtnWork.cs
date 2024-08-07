using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BtnWork : MonoBehaviour 
{
	private Animator anim;
	public BtnChoice btnChoice;
	public string text;

	private InVitro vit;

	public bool clicked;

	// Use this for initialization
	void Start () 
	{
		GameObject innerText = this.transform.Find("txt").gameObject;
		var textMesh = innerText.GetComponent<TextMesh>();
		textMesh.text = text;

		anim = this.GetComponent<Animator>();
		anim.SetInteger("state", 0);

		vit = InVitro.getInstance ();
		clicked = false;
	}

	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnMouseEnter() 
	{
		anim.SetInteger("state", 1);
	}

	void OnMouseExit() 
	{
		anim.SetInteger("state", 0);
	}

	void OnMouseDown()
	{
		anim.SetInteger("state", 2);
	}
	void OnMouseUp()
	{
		anim.SetInteger("state", 1);
		vit.btnChoice = btnChoice;
		clicked = true;
	}
}