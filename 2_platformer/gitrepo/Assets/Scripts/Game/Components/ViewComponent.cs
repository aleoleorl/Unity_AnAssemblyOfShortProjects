using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using System.ComponentModel.Design;

public class ViewComponent : BaseComponent
{
	private GameObject one;

	public ViewComponent(GameObject one)
	{
		ComponentName = "view";
		this.one = one;
	}

	public override void work(ObjectContainer parameters)
	{
		vision (parameters);
		if (one.activeSelf) 
		{
			move (parameters);
			scale (parameters);
		}
	}

	public override void destroy()
	{
		MonoBehaviour.Destroy (one);
	}

	private void vision(ObjectContainer parameters)
	{
		if (parameters.show && !one.activeSelf) 
		{
			one.SetActive (true);
		}
		if (!parameters.show && one.activeSelf) 
		{
			one.SetActive (false);
		}
	}

	private void move(ObjectContainer parameters)
	{
		if (one.transform.position.x != parameters.x || 
			one.transform.position.y != parameters.y) 
		{
			one.transform.position = new Vector3(parameters.x, parameters.y, 0);
		}
	}

	private void scale(ObjectContainer parameters)
	{
		Renderer rend = one.GetComponent<Renderer>();
		Vector2 temp0 = rend.bounds.size;
		if (temp0 != new Vector2 (parameters.width, parameters.height)) 
		{
			Vector3 temp = new Vector3 (parameters.width/temp0.x, parameters.height/temp0.y, 1f);
			one.transform.localScale = temp;
		}
	}
}