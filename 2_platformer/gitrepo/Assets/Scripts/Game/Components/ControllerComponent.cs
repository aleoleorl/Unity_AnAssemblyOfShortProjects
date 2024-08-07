using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class ControllerComponent : BaseComponent 
{
	private float sensitivity;

	public ControllerComponent()
	{
		ComponentName = "controller";
		sensitivity = 0.1f;
	}

	public override void work(ObjectContainer parameters)
	{
		checkSensitivity (parameters);
		//checkMove (parameters);
		checkKeys (parameters);
	}

	private void checkSensitivity(ObjectContainer parameters)
	{
		if (parameters.sensitivity != sensitivity)
		{
			sensitivity = parameters.sensitivity;
		}
	}

	private void checkKeys(ObjectContainer parameters)
	{
		if (Input.GetKey("a"))//Input.GetKeyDown (KeyCode.Space))
		{
			parameters.moveX -= sensitivity;
		}
		if (Input.GetKey("d"))//Input.GetKeyDown (KeyCode.Space))
		{
			parameters.moveX += sensitivity;
		}
		if (Input.GetKey("w"))//Input.GetKeyDown (KeyCode.Space))
		{
			parameters.upControl = true;
			parameters.moveY += sensitivity;
		}
		if (Input.GetKey("s"))//Input.GetKeyDown (KeyCode.Space))
		{
			parameters.moveY -= sensitivity;
		}

		if (Input.GetKeyUp("w"))//Input.GetKeyDown (KeyCode.Space))
		{
			parameters.upControl = false;
		}
	}
}