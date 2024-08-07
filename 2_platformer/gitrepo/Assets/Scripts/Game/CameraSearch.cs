using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSearch : MonoBehaviour 
{
	private Bitte bit;
	private Camera camera1;

	void Start () 
	{
		bit = Bitte.getInstance ();
		camera1 = GetComponent<Camera>();
	}

	void Update ()
	{
		if (bit.cameraY > 0) 
		{
			camera1.transform.position = new Vector3(camera1.transform.position.x, bit.cameraY, camera1.transform.position.z);
		}
	}
}