using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball
{
	public GameObject one;
	public bool inGame;
	public float targetX;
	public float targetY;
	public List<int> list;

	public bool inAnalyze;
	public int target;
	public int prevTarget;

	public Ball()
	{
		inGame = false;
		list = new List<int> ();
		inAnalyze = false;
		target = -1;
		prevTarget = -1;
	}

	public void Create(GameObject one, int count)
	{
		this.one = one;
		one.transform.localScale = new Vector3 (0.25f, 0.25f, 1.0f);
		list = new List<int> ();
		for (int i = 0; i < count; i++) 
		{
			list.Add (i);
		}
	}
}