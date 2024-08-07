using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class InVitro
{

	public string previousScene;
	public TextAsset xmlAsset;
	public BtnChoice btnChoice;
	public string mainName;
	public List<LevelInfo> level;
	public Result result;
	public int currentLevel = 0;

	private static InVitro vit;

	public static InVitro getInstance()
	{
		if (vit == null) 
		{
			vit = new InVitro ();
		}
		return vit;
	}
	private InVitro()
	{
		mainName = "baseXml";
		previousScene="sceneMain";
		xmlAsset = null;
		btnChoice = BtnChoice.No;
		level = new List<LevelInfo> ();
		result = Result.No;
	}
}

public enum BtnChoice
{
	Continue, NewGame, Edit, Exit, No, Back, Default
}

public enum Result
{
	No, Win, Return
}