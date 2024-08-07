using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;
using System.IO;

public class Bitte 
{
	public List<List<Vector4>> colliders;
	public List<bool> moveAccess;

	public List<EventOne> eventIdStatus;

	public float cameraX;
	public float cameraY;

	public float startX;
	public float startY;

	public string std="asd";

	public int tries;
	public bool restart;



	private Bitte()
	{		
		tries = 0;
		cameraX = 0;
		cameraY = 0;
		startX = 0;
		startY = 0;
		restart = false;
		colliders = new List<List<Vector4>> ();
		moveAccess = new List<bool> ();
		eventIdStatus = new List<EventOne> ();
	}

	public void clear()
	{
		cameraX = 0;
		cameraY = 0;
		colliders = new List<List<Vector4>> ();
		moveAccess = new List<bool> ();
		eventIdStatus = new List<EventOne> ();
	}

	private static Bitte bit;
	public static Bitte getInstance()
	{
		if (bit == null) 
		{
			bit = new Bitte ();
		}
		return bit;
	}
}

