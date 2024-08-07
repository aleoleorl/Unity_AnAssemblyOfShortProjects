using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using System;
using UnityEngine.SceneManagement;

public class EventComponent : BaseComponent 
{
	//event
	//id:status(on/off):action:action:action:effect:effect:effect

	//action
	//cameraInSquare:posX:posY:allWidth:allHeight
	//pause:sec

	//event
	//returnBack - return to previous scene
	//makeVisual:status(true/false)
	//eventOnOff:numOfId:status(true/false)
	public readonly int id;
	private readonly int externalId;
	public bool status;
	public bool repeatedEvent;
	public List<Action> actions;
	public List<Action> effects;

	private Bitte bit;
	private InVitro vit;

	public EventComponent(int id, bool status, bool repeatedEvent, List<string> actions, List<string> effects, int externalId)
	{
		ComponentName = "event";
		bit = Bitte.getInstance ();
		vit = InVitro.getInstance ();
		this.id = id;
		this.status = status;
		this.repeatedEvent = repeatedEvent;
		this.actions = new List<Action> ();
		this.externalId = externalId;
		if (actions != null) 
		{
			for (int i = 0; i < actions.Count; i++)
			{
				this.actions.Add (new Action (actions[i]));
			}
		}
		this.effects = new List<Action> ();
		if (effects != null) 
		{
			for (int i = 0; i < effects.Count; i++)
			{
				this.effects.Add (new Action (effects[i]));
			}
		}
	}

	public override void work(ObjectContainer parameters)
	{
		if (bit.eventIdStatus [externalId].status != status)
		{
			status = bit.eventIdStatus [externalId].status;
		}
		if (!status) 
		{
			return;
		}
		bool ready = action (parameters);
		if (ready) 
		{
			effect (parameters);
		}
	}

	private bool wait()
	{
		return true;
	}

	private bool action(ObjectContainer parameters)
	{
		bool ready = false;
		for (int i = 0; i < actions.Count; i++) 
		{
			if (actions [i].name == "cameraYLessThan") 
			{
				//cameraYLessThan:-4f;
				if (bit.cameraY < float.Parse (actions [i].listOfParameters [0])) 
				{
					ready = true;
				} else 
				{
					ready = false;
					break;
				}
			}
			if  (actions [i].name == "cameraInSquare") 
			{
				//cameraInSquare:-4f:5f:3f:1.54f
				//Debug.Log(actions [i].listOfParameters [0])
				float x = float.Parse (actions [i].listOfParameters [0]);
				float y = float.Parse (actions [i].listOfParameters [1]);
				float wid = float.Parse (actions [i].listOfParameters [2]) / 2;
				float hei = float.Parse (actions [i].listOfParameters [3]) / 2;

				if (bit.cameraX >= x - wid && bit.cameraX <= x + wid &&
					bit.cameraY >= y - hei && bit.cameraY <= y + hei) 
				{
					ready = true;
					//Debug.Log ("id=" + id);
				} else 
				{
					ready = false;
					break;
				}
			}
			if (actions [i].name == "pause") 
			{
				//"pause:3"
				//Debug.Log(actions[i].listOfParameters[0]);
				float a = float.Parse(actions[i].listOfParameters[0]);
				a -= 0.1f;
				actions [i].listOfParameters [0] = Convert.ToString(a);
				if (a <= 0) 
				{
					ready = true;
				} else 
				{
					ready = false;
					break;
				}
			}
		}
		return ready;
	}

	private void effect(ObjectContainer parameters)
	{
		if (!repeatedEvent) 
		{
			status = false;
			bit.eventIdStatus [externalId].status = status;
		}
		for (int i = 0; i < effects.Count; i++) 
		{
			if (effects [i].name == "returnBack") 
			{
				//Debug.Log ("returnBack");
				//"returnBack"
				SceneManager.LoadScene(vit.previousScene);
			}
			if (effects [i].name == "win") 
			{
				//Debug.Log ("retWin");
				vit.result = Result.Win;
			}
			if (effects [i].name == "ret") 
			{
				vit.result = Result.Return;
			}
			if (effects [i].name == "eventOnOff") 
			{
				//"eventOnOff:1:true"
				int findId = Convert.ToInt32(effects[i].listOfParameters[0]);
				for (int j = 0; j < bit.eventIdStatus.Count; j++) 
				{
					if (bit.eventIdStatus [j].id == findId)
					{
						findId = bit.eventIdStatus [j].id;
						break;
					}
				}
				bool result = Convert.ToBoolean(effects[i].listOfParameters[1]);
				bit.eventIdStatus [findId].status = result;
			}
			if (effects [i].name == "makeVisual")
			{
				//"makeVisual:false"
				parameters.show = Convert.ToBoolean(effects[i].listOfParameters[0]);
			}
			if (effects [i].name == "makeMove")
			{
				//Debug.Log ("makeMove");
				parameters.move = Convert.ToBoolean(effects[i].listOfParameters[0]);
			}
			if (effects [i].name == "levelStartAgain")
			{
				bit.tries++;
				bit.restart = true;
			}
		}
	}
}

public class Action
{
	public string name;
	public List<string> listOfParameters;

	public Action(string action)
	{
		listOfParameters = new List<string> ();
		//Debug.Log (action);
		var actions = action.Split(':');
		name = actions [0];
		//Debug.Log ("name="+name);
		for (int i = 1; i < actions.Length; i++)
		{
			//Debug.Log (actions [i]);
			listOfParameters.Add (actions [i]);
		}
	}
}

public class EventOne
{
	public int id;
	public bool status;

	public EventOne()
	{
		id = 0;
		status = true;
	}
}