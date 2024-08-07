using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere 
{
	public GameObject one;
	public float angle;
	public int cw;

	private float x;
	private float y;

	public int score;

	public Sphere(GameObject one, int num, float angle=3.14f + 1.62f)
	{
		cw = Random.Range (-1, 1);
		if (cw == 0)
		{
			cw = 1;
		}
		this.one = one;
		score = 0;
		this.one.transform.localScale = new Vector3 (0.5f, 0.5f, 1.0f);

		this.angle = angle;
	}

	public void Move(int i)
	{
		//x=x0+r*cosa
		//y=y0+r*sina
		if ( angle < 6.28f) 
		{
			angle += cw*0.01f/(i+1);
		} else
		{
			angle = cw*0.0f/(i+1);
		}
		x = 0 + ((i+1) * 2) * Mathf.Cos (angle);
		y = 0 + ((i+1) * 2) * Mathf.Sin (angle);
		one.transform.position = new Vector3(x, y, 0.0f);
	}

	public void Rotation()
	{
		if (score > 0) 
		{
			score--;
			one.transform.Rotate (0, 0, -6f);
		} else
		{
			Vector2 dir = new Vector3 (0, 0, 0) - one.transform.position;
			one.transform.right = -dir;
		}
	}

	public void Observing(Ball bal)
	{
		Vector2 dir = bal.one.transform.position - one.transform.position;

		if (score < 10) 
		{
			score++;
			one.transform.Rotate (0, 0, 6f);
		} else 
		{
			one.transform.right = dir;
		}

	}
}