using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainWork : MonoBehaviour 
{
	private List<Sphere> sp;
	private Ball ball;
	private GameObject tur;
	private bool turetTime;

	private SpecialBehaviour beh;

    private float elapsedTime = 0f;
    private const float interval = 0.01f;

    void Start () 
	{		
		sp = new List<Sphere>();
		tur = new GameObject();
		ball = new Ball ();

		tur = NewObject ("turet");
		tur.transform.localScale = new Vector3 (0.25f, 0.25f, 1.0f);
		turetTime = true;

		sp.Add (new Sphere (NewObject ("sphere2"), sp.Count));
		sp.Add (new Sphere (NewObject ("sphere2"), sp.Count));

		beh = new SpecialBehaviour ();
	}

	void Update ()
	{
        elapsedTime += Time.deltaTime;
		if (elapsedTime >= interval)
		{
            elapsedTime = 0f;
            Scale();
			Turret();
			Rotation();
			Move();
		}
	}

	private void Scale()
	{
		if (Camera.main.orthographicSize < sp.Count*2.1) 
		{
			Camera.main.orthographicSize += 0.02f;
		}
	}

	private void Turret()
	{
		if (turetTime)
		{
			Vector2 dir = new Vector3 (ball.targetX, ball.targetY, 0) - tur.transform.position;
			tur.transform.right = dir;
			tur.transform.Rotate (0, 0, 88f);
			turetTime = false;
		}
		if (!ball.inGame) 
		{
			ball.Create (NewObject ("ball"), sp.Count);
			ball.inGame = true;
			ball.inAnalyze = true;
			turetTime = true;
		}
	}

	private GameObject NewObject(string obj)
	{
        return Instantiate(Resources.Load<GameObject>(obj));
	}

	private void Rotation()
	{
		for (int i = 0; i < sp.Count; i++) 
		{
			sp [i].Move (i);
			if (ball.target != i) 
			{
				sp [i].Rotation ();
			} else 
			{
				sp [i].Observing (ball);
			}
		}
	}

	private void Move()
	{	
		if (ball.inGame) 
		{
			if (ball.inAnalyze)
			{
				Vector3 temp = beh.AnalyzeMove (ball, sp);
				ball.targetX = temp.x;
				ball.targetY = temp.y;
				ball.target = Convert.ToInt32(temp.z);
				ball.inAnalyze = false;
			} else 
			{
				Vector3 pos1 = ball.one.transform.position; 
				Vector3 pos2 = new Vector3 (ball.targetX, ball.targetY, 0);     
				ball.one.transform.position = Vector3.MoveTowards (pos1, pos2, 0.1f); 
				if (pos1 == pos2) 
				{
					ball.prevTarget = ball.target;
					if (ball.target==-1) 
					{
						Destroy (ball.one);
						ball.inGame = false;
						ball.inAnalyze = false;
						return;
					}
					ball.list.Remove (ball.target);

					if (ball.list.Count == 0) 
					{						
						if (sp.Count < SpecialBehaviour.maxSpheres)
						{
							int tempI = sp.Count;
							float rnd = beh.AnalyzeNewSphere (ball, sp);//UnityEngine.Random.Range (0f, 6.28f);
							sp.Add (new Sphere (NewObject ("sphere2"), tempI, rnd));
							ball.list.Add (tempI);
							sp [tempI].Move (tempI);
						} else 
						{							
							ball.target = -1;
						}
						ball.inAnalyze = true;
					} else
					{
						ball.inAnalyze = true;
					}
				}
			}
		}
	}
}