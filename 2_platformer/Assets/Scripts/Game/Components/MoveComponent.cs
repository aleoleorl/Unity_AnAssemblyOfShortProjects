using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class MoveComponent : BaseComponent 
{
	private float horizontalBorders;
	private float topBorder;

	private Bitte bit;

	public MoveComponent(float horizontalBorders, float topBorder)
	{
		ComponentName = "move";
		this.horizontalBorders = horizontalBorders;
		this.topBorder = topBorder;

		bit = Bitte.getInstance ();
	}

	public override void work(ObjectContainer parameters)
	{
		if (parameters.move) 
		{
			checkMove (parameters);
		}
	}

	private void checkMove(ObjectContainer parameters)
	{
		//Debug.Log ((parameters.x + parameters.moveX) + ":" + horizontalBorders);
		Debug.Log("parameters.moveY:" + parameters.moveY);

        if (parameters.moveX != 0 || parameters.moveY != 0) 
		{
			if (parameters.x + parameters.moveX > -horizontalBorders &&
			    parameters.x + parameters.moveX < horizontalBorders)
			{
				parameters.x += parameters.moveX;
			}
			if (parameters.y + parameters.moveY < topBorder) 
			{
				parameters.y += parameters.moveY;
			}
			parameters.moveX = 0;
			parameters.moveY = 0;

			bit.cameraX = parameters.x;
			bit.cameraY = parameters.y;
		}
	}
}
