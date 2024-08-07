using System;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class ComponentCategory
	{
		public int max; 
		public List<ComponentInEditor> components;
		public string name;

		public ComponentCategory (string name, int max)
		{
			this.max = max;
			components = new List<ComponentInEditor> ();
			this.name = name;
		}
	}

	public class ComponentInEditor
	{
		public string name;
		public List<ParameterInEditor> parameters;
		public List<EventPart> actions;
		public List<EventPart> effects;

		public ComponentInEditor(string name)
		{
			this.name = name;
			parameters = new List<ParameterInEditor> ();
			actions = new List<EventPart> ();
			effects = new List<EventPart> ();
		}
	}

	public class ParameterInEditor
	{
		public string name;
		public bool multi;
		public string tip; //for single;
		public bool inner;
		public List<string> attrName; //for multi;
		public List<string> attrTip; //for multi;

		public ParameterInEditor()
		{
			name = "";
			tip = "";
			multi = false;
			inner = false;
			attrName = new List<string> ();
			attrTip = new List<string> ();
		}
	}

	public enum EventPartStatus
	{
		Action, Effect
	}
}