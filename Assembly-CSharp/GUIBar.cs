using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008C0 RID: 2240
[RequireComponent(typeof(HorizontalLayoutGroup))]
public class GUIBar : GameBehavior, ITickable
{
	// Token: 0x0600387E RID: 14462 RVA: 0x00109024 File Offset: 0x00107424
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

	// Token: 0x0600387F RID: 14463 RVA: 0x00109070 File Offset: 0x00107470
	public void TickFrame()
	{
		for (int i = 0; i < this.uiElements.Count; i++)
		{
			IGUIBarElement iguibarElement = this.uiElements[i];
			iguibarElement.TickFrame();
		}
	}

	// Token: 0x040026D9 RID: 9945
	public GameObject Center;

	// Token: 0x040026DA RID: 9946
	public GameObject elementPrefab;

	// Token: 0x040026DB RID: 9947
	protected List<IGUIBarElement> uiElements = new List<IGUIBarElement>();
}
