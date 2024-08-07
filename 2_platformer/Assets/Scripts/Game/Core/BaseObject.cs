using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class BaseObject 
{
	public List<BaseComponent> components;
	public ObjectContainer parameters;
	public int id;

	public BaseObject()
	{
		components = new List<BaseComponent> ();
		parameters = new ObjectContainer ();
		id = -1;
	}

	public void work()
	{
		for (int i = 0; i < components.Count; i++) 
		{
			components[i].work (parameters);
		}
	}

	public void destroy()
	{
		for (int i = 0; i < components.Count; i++) 
		{
			components[i].destroy ();
		}
	}
}