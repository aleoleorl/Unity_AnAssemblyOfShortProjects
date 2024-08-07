using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpecialBehaviour
{
	public static int maxSpheres = 15;

	private int StepCounter(Vector3 Pos1, Vector3 Pos2)
	{
		Vector3 pos1 = Pos1; 
		Vector3 pos2 = Pos2;
		int steps = 0;
		do {
			steps++;
			pos1 = Vector3.MoveTowards (pos1, pos2, 0.1f); 
		} while (pos1!=pos2);
		return steps;
	}

	public Vector3 AnalyzeMove(Ball ball, List<Sphere> sp, bool to=true)
	{
		List<Vector3> variants = new List<Vector3> ();
		for (int i = 0; i < ball.list.Count; i++) 
		{
			Vector3 temp = CheckSteps (ball, sp, i);
			if (!Collisions(ball, sp, temp))
			{
				variants.Add (temp);
			}
		}
		if (variants.Count > 0) 
		{
			Vector3 res = variants [UnityEngine.Random.Range (0, variants.Count)];
			return res;
		}

		bool ret = true;
		float x=0;
		float y=0;
		while (ret) 
		{
			float angle = UnityEngine.Random.Range (0, 6.28f);
			x = 0 + ((sp.Count + 3) * 2) * Mathf.Cos (angle);
			y = 0 + ((sp.Count + 3) * 2) * Mathf.Sin (angle);

			if (!Collisions(ball, sp, new Vector3(x,y,-1f)))
			{
				ret=false;
			}
		}
		return new Vector3 (x, y, -1);
	}

	public float AnalyzeNewSphere(Ball ball, List<Sphere> sp)
	{
		bool ret = true;
		float x=0;
		float y=0;
		float angle=0;
		while (ret) 
		{
			angle = UnityEngine.Random.Range (0, 6.28f);
			x = 0 + ((sp.Count + 1) * 2) * Mathf.Cos (angle);
			y = 0 + ((sp.Count + 1) * 2) * Mathf.Sin (angle);

			if (!Collisions(ball, sp, new Vector3(x,y,-1f)))
			{
				ret=false;
			}
		}
		return angle;
	}


	private bool Collisions(Ball ball, List<Sphere> sp, Vector3 choice)
	{		
		for (int i = 0; i < sp.Count; i++)
		{
			if (i==Convert.ToInt32(choice.z))
			{
				continue;
			}
			float angle = sp [i].angle;
			float size = 0.25f;
			int repeats = 0;

			Vector3 pos1 = ball.one.transform.position; 
			Vector3 pos2 = new Vector3 (choice.x, choice.y, 0);
			do 
			{
				pos1 = Vector3.MoveTowards (pos1, pos2, 0.1f); 
				angle += sp [i].cw * 0.01f / (i + 1);
				float x = 0 + ((i + 1) * 2) * Mathf.Cos (angle);
				float y = 0 + ((i + 1) * 2) * Mathf.Sin (angle);

				if (pos1.x > x - size && pos1.x < x + size &&
					pos1.y > y - size && pos1.y < y + size) 
				{
					if (i == ball.prevTarget && repeats<2)
					{
						repeats++;
					} else
					{
						return true;
					}
				}
			} while (pos1 != pos2);
		}
		return false;
	}

	private Vector3 CheckSteps(Ball ball, List<Sphere> sp, int choice, bool to=true)
	{
		int index = ball.list[choice];
		int steps;
		if (to)
		{
			steps = StepCounter (ball.one.transform.position, sp [index].one.transform.position);
		} else
		{
			steps = StepCounter (ball.one.transform.position, new Vector3(-1f,-1f,0f));
		}
		int steps2 = steps;
		float x, y;
		int cycle = 0;
		do {
			steps=steps2;

			float newAngle = sp [index].angle;
			for (int i = 0; i < steps; i++) {
				newAngle += sp [index].cw * 0.01f / (index + 1);
			}
			x = 0 + ((index + 1) * 2) * Mathf.Cos (newAngle);
			y = 0 + ((index + 1) * 2) * Mathf.Sin (newAngle);

			steps2 = StepCounter (ball.one.transform.position, new Vector3 (x, y, 1f));
			cycle++;
			if (cycle>10)
			{
				break;
			}
		} while (steps!=steps2);
		Vector3 temp = new Vector3 (x, y, index);
		return temp;
	}
}