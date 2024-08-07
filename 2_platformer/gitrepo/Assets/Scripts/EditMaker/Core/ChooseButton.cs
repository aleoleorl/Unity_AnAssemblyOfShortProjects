using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class ChooseButton : MonoBehaviour 
{
	private Danke dan;

	void Start () 
	{
		dan = Danke.getInstance ();
	}

	void Update () 
	{
		
	}

	public void btnClose() 
	{
		dan.editorButtons = StatusEnum.Close;
	}

	public void btnComponent() 
	{
		dan.editorButtons = StatusEnum.Component;
	}

	public void btnBack() 
	{
		dan.editorButtons = StatusEnum.Back;
	}

	public void btnSubject() 
	{
		dan.editorButtons = StatusEnum.Subject;
	}

	public void btnParameters() 
	{
		dan.editorButtons = StatusEnum.Parameters;
	}
}
