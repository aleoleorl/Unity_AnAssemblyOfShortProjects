using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public class Factory 
{
	private Bitte bit;
	private InVitro vit;

	public Factory()
	{
		bit = Bitte.getInstance ();
		bit.clear ();
		vit = InVitro.getInstance ();
	}

	public bool make(List<BaseObject> entities)
	{
		if (vit.xmlAsset == null) 
		{
			return defaultMap (entities);
		} else
		{
			return parser (entities);
		}
	}

	private bool parser(List<BaseObject> entities)
	{
		try
		{
			BaseObject temp;

			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml (vit.xmlAsset.text);
			XmlNode xmlNode = null;
			for (int i = 0; i < xmlDoc.ChildNodes.Count; i++)
			{
				if (xmlDoc.ChildNodes [i].Name == "game") 
				{					
					xmlNode = xmlDoc.ChildNodes[i];
					bit.startX = float.Parse(xmlNode.Attributes["startX"].Value);
					bit.startY = float.Parse(xmlNode.Attributes["startY"].Value);
					break;
				}
			}

			bit.eventIdStatus = new List<EventOne> ();

			foreach (XmlNode sectionsNode in xmlNode) //sections
			{
				if (sectionsNode.Name == "baseObject") 
				{
					temp = new BaseObject ();
					bit.moveAccess.Add (Convert.ToBoolean(sectionsNode.Attributes["moveAccess"].Value));
					List<BaseComponent> baseTemp = new List<BaseComponent> ();
					foreach (XmlNode componentNode in sectionsNode) //components
					{
						if (componentNode.Name == "parameter") 
						{
							string name1 = componentNode.Attributes["name"].Value;
							string value1 = componentNode.Attributes["value"].Value;
							if (name1 == "show")
							{
								temp.parameters.show = Convert.ToBoolean (value1);
							}
							if (name1 == "x")
							{
								temp.parameters.x = float.Parse (value1);
							}
							if (name1 == "y")
							{
								temp.parameters.y = float.Parse (value1);
							}
							if (name1 == "width")
							{
								temp.parameters.width = float.Parse (value1);
							}
							if (name1 == "height")
							{
								temp.parameters.height = float.Parse (value1);
							}
							if (name1 == "move")
							{
								temp.parameters.move = Convert.ToBoolean (value1);
							}
						}
						if (componentNode.Name == "component") 
						{
							if (componentNode.Attributes ["name"].Value == "View") //could be a few
							{
								foreach (XmlNode listNode in componentNode) //list
								{
									if (listNode.Name == "name") 
									{
										baseTemp.Add (new ViewComponent (NewObject (listNode.InnerText)));
									}
								}
							}
							if (componentNode.Attributes ["name"].Value == "Controller") //should be one
							{
								if (alreadyExists ("Controller", baseTemp)) 
								{
									continue;
								}
								baseTemp.Add (new ControllerComponent ());
							}
							if (componentNode.Attributes ["name"].Value == "Phys") 
							{
								if (alreadyExists ("Phys", baseTemp)) 
								{
									continue;
								}
								float nx = 0;
								float ny = 0;
								foreach (XmlNode listNode in componentNode) //list
								{
									if (listNode.Name == "nx") 
									{
										nx = float.Parse(listNode.InnerText);
									}
									if (listNode.Name == "ny") 
									{
										ny = float.Parse(listNode.InnerText);
									}
								}
								baseTemp.Add (new PhysComponent (nx, ny));
							}
							if (componentNode.Attributes ["name"].Value == "Collider") 
							{
								if (alreadyExists ("Collider", baseTemp)) 
								{
									continue;
								}
								float vx = 0;
								float vy = 0;
								float vz = 0;
								float vw = 0;
								Vector4 vector4;
								List<Vector4> list4 = new List<Vector4> ();
								foreach (XmlNode listNode in componentNode) //list
								{
									if (listNode.Name == "v") 
									{
										vx = float.Parse(listNode.Attributes["vx"].Value);
									
										vy = float.Parse(listNode.Attributes["vy"].Value);
									
										vz = float.Parse(listNode.Attributes["vz"].Value);
									
										vw = float.Parse(listNode.Attributes["vw"].Value);
										vector4 = new Vector4 (vx, vy, vz, vw);
										list4.Add (vector4);
									}
								}
								baseTemp.Add (new ColliderComponent (list4, entities.Count));
								bit.colliders.Add (list4);
							}
							if (componentNode.Attributes ["name"].Value == "Move")
							{
								if (alreadyExists ("Move", baseTemp)) 
								{
									continue;
								}
								float vx = 0;
								float vy = 0;
								foreach (XmlNode listNode in componentNode) //list
								{
									if (listNode.Name == "vx") 
									{
										vx = float.Parse (listNode.InnerText);
									}
									if (listNode.Name == "vy") 
									{
										vy = float.Parse (listNode.InnerText);
									}
								}
								baseTemp.Add (new MoveComponent (vx, vy));
							}
							if (componentNode.Attributes ["name"].Value == "Event") 
							{
								bool status = true;
								int id = 0; 
								bool repeatedEvent = false;
								List<string> actions = new List<string> ();
								List<string> effects = new List<string>();
								foreach (XmlNode listNode in componentNode) //list
								{
									if (listNode.Name == "id") 
									{
										id = Convert.ToInt32 (listNode.InnerText);
									}
									if (listNode.Name == "status") 
									{
										status = Convert.ToBoolean (listNode.InnerText);
									}
									if (listNode.Name == "repeatedEvent") 
									{
										repeatedEvent = Convert.ToBoolean (listNode.InnerText);
									}
									if (listNode.Name == "action") 
									{
										actions.Add (listNode.InnerText);
									}
									if (listNode.Name == "effect") 
									{
										effects.Add (listNode.InnerText);
									}
								}
								bit.eventIdStatus.Add(new EventOne());
								bit.eventIdStatus[bit.eventIdStatus.Count-1].id=id;
								bit.eventIdStatus[bit.eventIdStatus.Count-1].status=status;
								baseTemp.Add (new EventComponent (id, status, repeatedEvent, actions, effects,bit.eventIdStatus.Count-1));
							}
						}
					}

					instantiate(baseTemp, temp, "view", true);
					instantiate(baseTemp, temp, "controller", false);
					instantiate(baseTemp, temp, "phys", false);
					instantiate(baseTemp, temp, "collider", false);
					instantiate(baseTemp, temp, "move", false);
					instantiate(baseTemp, temp, "event", true);
					entities.Add (temp);
				}
			}
		} catch(XmlException e) 
		{
			Debug.Log (e);
			return false;
		}
		catch(Exception e) 
		{
			Debug.Log (e);
			return false;
		}
		return true;
	}

	private void instantiate(List<BaseComponent> baseTemp, BaseObject temp, string name, bool allComponents)
	{
		for (int i = 0; i < baseTemp.Count; i++)
		{
			if (baseTemp [i].ComponentName.Equals (name)) 
			{
				temp.components.Add (baseTemp [i]);
				if (!allComponents) 
				{
					return;
				}
			}
		}
	}

	private bool alreadyExists(string name, List<BaseComponent> baseTemp)
	{
		for (int i = 0; i < baseTemp.Count; i++) 
		{
			if (baseTemp [i].ComponentName.Equals (name)) 
			{
				return true;
			}
		}
		return false;
	}

	private bool defaultMap(List<BaseObject> entities)
	{		
		bit.eventIdStatus = new List<EventOne> ();

		BaseObject temp = new BaseObject ();
		temp.components.Add (new ViewComponent (NewObject ("pers1")));
		temp.parameters.show = true;
		temp.parameters.x = 0f;
		temp.parameters.y = 0f;
		temp.parameters.width = 0.835f;
		temp.parameters.height =  1f;
		temp.parameters.move = true;

		temp.components.Add (new ControllerComponent ());

		temp.components.Add (new PhysComponent (0.125f, 2.5f));

		Vector4 vector4 = new Vector4 (0, 0, 0.835f, 1f);
		List<Vector4> list4 = new List<Vector4> ();
		list4.Add (vector4);
		temp.components.Add (new ColliderComponent (list4, entities.Count));
		bit.colliders.Add (list4);
		bit.moveAccess.Add (false);

		temp.components.Add (new MoveComponent (6.5f, 10f));

		bool status = true;
		int id = 0; 
		bit.eventIdStatus.Add (new EventOne ());
		bit.eventIdStatus[bit.eventIdStatus.Count-1].status=status;
		bit.eventIdStatus[bit.eventIdStatus.Count-1].id=id;
		bool repeatedEvent = false;
		List<string> actions = new List<string> ();
		actions.Add ("cameraInSquare:-4:5:3:1.54");
		List<string> effects = new List<string>();
		effects.Add ("makeVisual:false");
		effects.Add ("makeMove:false");
		effects.Add ("eventOnOff:1:true");
		temp.components.Add (new EventComponent (id, status, repeatedEvent, actions, effects, bit.eventIdStatus.Count-1));

		status = false;
		id = 1;
		bit.eventIdStatus.Add (new EventOne ());
		bit.eventIdStatus[bit.eventIdStatus.Count-1].status=status;
		bit.eventIdStatus[bit.eventIdStatus.Count-1].id=id;
		repeatedEvent = false;
		actions = new List<string> ();
		actions.Add ("pause:5");
		effects = new List<string>();
		effects.Add ("returnBack");
		temp.components.Add (new EventComponent (id, status, repeatedEvent, actions, effects, bit.eventIdStatus.Count-1));

		entities.Add (temp);


		temp = new BaseObject ();
		temp.components.Add (new ViewComponent (NewObject ("floor1")));
		temp.parameters.show = true;
		temp.parameters.x = 0f;
		temp.parameters.y = -5f;
		temp.parameters.width = 4.8f;
		temp.parameters.height =  0.52f;

		vector4 = new Vector4 (0, -5f, 4.8f, 0.52f);
		list4 = new List<Vector4> ();
		list4.Add (vector4);
		temp.components.Add (new ColliderComponent (list4, entities.Count));
		bit.colliders.Add (list4);
		bit.moveAccess.Add (false);

		entities.Add (temp);


		temp = new BaseObject ();
		temp.components.Add (new ViewComponent (NewObject ("floor1")));
		temp.parameters.show = true;
		temp.parameters.x = 4f;
		temp.parameters.y = -3f;
		temp.parameters.width = 4.8f;
		temp.parameters.height =  0.52f;

		vector4 = new Vector4 (4f, -3f, 4.8f, 0.52f);
		list4 = new List<Vector4> ();
		list4.Add (vector4);
		temp.components.Add (new ColliderComponent (list4, entities.Count));
		bit.colliders.Add (list4);
		bit.moveAccess.Add (false);

		entities.Add (temp);


		temp = new BaseObject ();
		temp.components.Add (new ViewComponent (NewObject ("floor1")));
		temp.parameters.show = true;
		temp.parameters.x = 6f;
		temp.parameters.y = -1f;
		temp.parameters.width = 4.8f;
		temp.parameters.height =  0.52f;

		vector4 = new Vector4 (6f, -1f, 4.8f, 0.52f);
		list4 = new List<Vector4> ();
		list4.Add (vector4);
		temp.components.Add (new ColliderComponent (list4, entities.Count));
		bit.colliders.Add (list4);
		bit.moveAccess.Add (false);

		entities.Add (temp);


		temp = new BaseObject ();
		temp.components.Add (new ViewComponent (NewObject ("floor1")));
		temp.parameters.show = true;
		temp.parameters.x = 2f;
		temp.parameters.y = 1f;
		temp.parameters.width = 4.8f;
		temp.parameters.height =  0.52f;

		vector4 = new Vector4 (2f, 1f, 4.8f, 0.52f);
		list4 = new List<Vector4> ();
		list4.Add (vector4);
		temp.components.Add (new ColliderComponent (list4, entities.Count));
		bit.colliders.Add (list4);
		bit.moveAccess.Add (false);

		entities.Add (temp);


		temp = new BaseObject ();
		temp.components.Add (new ViewComponent (NewObject ("floor1")));
		temp.parameters.show = true;
		temp.parameters.x = -1f;
		temp.parameters.y = 3f;
		temp.parameters.width = 4.8f;
		temp.parameters.height =  0.52f;

		vector4 = new Vector4 (-1f, 3f, 4.8f, 0.52f);
		list4 = new List<Vector4> ();
		list4.Add (vector4);
		temp.components.Add (new ColliderComponent (list4, entities.Count));
		bit.colliders.Add (list4);
		bit.moveAccess.Add (false);

		entities.Add (temp);


		temp = new BaseObject ();
		temp.components.Add (new ViewComponent (NewObject ("door1")));
		temp.parameters.show = true;
		temp.parameters.x = -4f;
		temp.parameters.y = 5f;
		temp.parameters.width = 2f;
		temp.parameters.height =  1.34f;

		bit.colliders.Add (null);
		bit.moveAccess.Add (true);

		entities.Add (temp);


		temp = new BaseObject ();
		temp.components.Add (new ViewComponent (NewObject ("congratz1")));
		temp.parameters.show = false;
		temp.parameters.x = 0f;
		temp.parameters.y = 5f;
		temp.parameters.width = 4f;
		temp.parameters.height =  2.58f;

		bit.colliders.Add (null);
		bit.moveAccess.Add (true);

		status = true;
		id = 2;
		bit.eventIdStatus.Add (new EventOne ());
		bit.eventIdStatus[bit.eventIdStatus.Count-1].status=status;
		bit.eventIdStatus[bit.eventIdStatus.Count-1].id=id;
		repeatedEvent = false;
		actions = new List<string> ();
		actions.Add ("cameraInSquare:-4:5:3:1.54");
		effects = new List<string>();
		effects.Add ("makeVisual:true");
		temp.components.Add (new EventComponent (id, status, repeatedEvent, actions, effects, bit.eventIdStatus.Count-1));

		entities.Add (temp);

		return true;
	}

	private GameObject NewObject(string obj)
	{
		return MonoBehaviour.Instantiate (Resources.Load(obj, typeof(GameObject))) as GameObject;
	}
}