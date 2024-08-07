
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class PhysComponent : BaseComponent 
{
	private float y;
	private float count;
	private float tempCount;

	private bool checkUpControl;

	public PhysComponent(float y, float count)
	{
		ComponentName = "phys";
		this.y = y;
		this.count = count;
		tempCount = count;
		checkUpControl = false;
	}

	public override void work(ObjectContainer parameters)
	{
		if (parameters.fly) 
		{
			if (parameters.moveY > 0)
			{
				if (tempCount > 0) 
				{
					tempCount -= parameters.sensitivity;
				} else 
				{
					parameters.moveY -= y;
				}
			}
			if (parameters.moveY == 0 && tempCount > 0) 
			{
				tempCount = 0;
			}

			if (tempCount <= 0)
			{
				parameters.moveY -= y;
			} 
		} else 
		{
			if (parameters.upControl && checkUpControl) 
			{
				parameters.moveY = 0;
			}
			if (parameters.upControl && !checkUpControl) 
			{
				checkUpControl = parameters.upControl;
			}
			if (!parameters.upControl && checkUpControl) 
			{
				checkUpControl = parameters.upControl;
			}

			if (parameters.moveY > 0) 
			{
				tempCount = count;
				parameters.fly = true;
			} else
			{
				parameters.moveY -= y;
			}
		}
	}
}
