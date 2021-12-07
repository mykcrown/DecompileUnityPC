// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIElementContainer : GameBehavior, IGUIBarElement, ITickable
{
	private List<IGUIBarElement> elements = new List<IGUIBarElement>();

	public bool Visible
	{
		get
		{
			for (int i = 0; i < this.elements.Count; i++)
			{
				if (this.elements[i].Visible)
				{
					return true;
				}
			}
			return false;
		}
	}

	public void AddElement(IGUIBarElement element)
	{
		this.elements.Add(element);
		element.transform.SetParent(base.transform, false);
	}

	public void setPosition(float x, float y)
	{
		base.transform.position = new Vector3(x, y);
	}

	public void TickFrame()
	{
		for (int i = 0; i < this.elements.Count; i++)
		{
			this.elements[i].TickFrame();
		}
	}

	Transform IGUIBarElement.get_transform()
	{
		return base.transform;
	}
}
