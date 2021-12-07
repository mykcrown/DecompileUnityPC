// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class GUIBar : GameBehavior, ITickable
{
	public GameObject Center;

	public GameObject elementPrefab;

	protected List<IGUIBarElement> uiElements = new List<IGUIBarElement>();

	protected GameObject createNewElement()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.elementPrefab);
		if (gameObject != null)
		{
			IGUIBarElement component = gameObject.GetComponent<IGUIBarElement>();
			this.uiElements.Add(component);
			gameObject.transform.SetParent(base.transform, false);
		}
		return gameObject;
	}

	public void TickFrame()
	{
		for (int i = 0; i < this.uiElements.Count; i++)
		{
			IGUIBarElement iGUIBarElement = this.uiElements[i];
			iGUIBarElement.TickFrame();
		}
	}
}
