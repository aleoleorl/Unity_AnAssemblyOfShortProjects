using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ani : MonoBehaviour
{

	private Animator anim;

	void Start () 
	{
		anim = this.GetComponent<Animator>();
		anim.SetInteger("state", 0);

		StartCoroutine("WaitAndShow");
	}

	void Update () 
	{
		
	}

	IEnumerator WaitAndShow()
	{
		int current=0;
		int state = 0;
		while (true) 
		{
			yield return new WaitForSeconds (4f);

			while (state == current)
			{
				state = Random.Range (0, 6);
			}
			current = state;
			anim.SetInteger ("state", state);
			float x = Random.Range (0, 4f);
			float y = Random.Range (-4, 1f);
			this.transform.position = new Vector2 (x,y);

		}
	}
}
