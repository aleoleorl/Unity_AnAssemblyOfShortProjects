using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public abstract class BaseComponent
{
	public string ComponentName 
	{
		get;
		protected set;
	}

	public virtual void work(ObjectContainer parameters)
	{
		ComponentName = "base";
	}

	public virtual void destroy()
	{
	}
}
