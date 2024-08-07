using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public class LevelInfo
{
	public string name;
	public string xmlName;

	public LevelInfo (string name, string xmlName)
	{
		this.name = name;
		this.xmlName = xmlName;
	}
}