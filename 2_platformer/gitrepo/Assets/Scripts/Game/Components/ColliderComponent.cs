using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class ColliderComponent : BaseComponent 
{
	private List<Vector4> colliders; // the idea of every square is object.x+x; object.y+y; width=z; height=w
	private readonly int indexInBitte;
	private Bitte bit;


	public ColliderComponent(List<Vector4> colliders, int i)
	{
		ComponentName = "collider";
		this.colliders = colliders;
		indexInBitte = i;
		bit = Bitte.getInstance ();
	}

	public override void work(ObjectContainer parameters)
	{
		if (bit.colliders [indexInBitte] != colliders) 
		{
			bit.colliders [indexInBitte] = colliders;
		}

		if (parameters.moveX != 0) 
		{
			float x = parameters.x + parameters.moveX;
			CollisionResult res = result (x, true, parameters);
			if (res == CollisionResult.Collision) 
			{
				//Debug.Log ("x stop");
				parameters.moveX = 0;
			}
		}
		if (parameters.moveY != 0) 
		{
			float y = parameters.y + parameters.moveY;
			CollisionResult res = result (y, false, parameters);
			if (res == CollisionResult.Collision) 
			{
				//Debug.Log ("y stop");
				parameters.moveY = 0;
				parameters.fly = false;
			}
		}
	}

	private CollisionResult result(float param, bool thisIsX, ObjectContainer parameters)
	{
		CollisionResult result = CollisionResult.No;
		for (int i = 0; i < bit.colliders.Count; i++) 
		{
			if (i == indexInBitte) 
			{
				continue;
			}
			if (bit.colliders [i] == null)
			{
				continue;
			}
			//Debug.Log ("i=" + i);
			for (int j=0; j<bit.colliders[i].Count; j++)
			{
				float betweenCenters1=0;
				float whSumm1=0;
				float betweenCenters2 = 0;
				float whSumm2 = 0;
				for (int k = 0; k < colliders.Count; k++)
				{
					if (thisIsX) 
					{
						betweenCenters1 = Mathf.Abs (param+colliders[k].x - bit.colliders [i] [j].x);
						whSumm1 = colliders[k].z / 2 + bit.colliders [i] [j].z / 2;
						betweenCenters2 = Mathf.Abs (parameters.y+colliders[k].y - bit.colliders [i] [j].y);
						whSumm2 = colliders[k].w / 2 + bit.colliders [i] [j].w / 2;
						//Debug.Log (">" + param + ":" + bit.colliders [i] [j].x + "::" + parameters.width / 2 + ":" + bit.colliders [i] [j].z / 2);
						//Debug.Log (">" + parameters.y + ":" + bit.colliders [i] [j].y + "::" + parameters.height / 2 + ":" + bit.colliders [i] [j].w / 2);
					} else
					{
						betweenCenters1 = Mathf.Abs (param+colliders[k].y - bit.colliders [i] [j].y);
						whSumm1 = colliders[k].w / 2 + bit.colliders [i] [j].w / 2;
						betweenCenters2 = Mathf.Abs (parameters.x+colliders[k].x - bit.colliders [i] [j].x);
						whSumm2 = colliders[k].z / 2 + bit.colliders [i] [j].z / 2;
					}
					//Debug.Log (betweenCenters1 + ":" + whSumm1 + "::" + betweenCenters2 + ":" + whSumm2);
					if (betweenCenters1 < whSumm1 && betweenCenters2 < whSumm2)
					{						
						if (!bit.moveAccess [i])
						{
							return CollisionResult.Collision;
						} else
						{
							result = CollisionResult.SmartCollision; //not enough done, could go but some bahaviour
							//also .Collision is prefer than .SmartCollision
						}
					}	
				}
			}
		}
		return result;
	}
}

public enum CollisionResult
{ 
	No, Collision, SmartCollision
}
