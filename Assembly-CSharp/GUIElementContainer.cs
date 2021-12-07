using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008CA RID: 2250
public class GUIElementContainer : GameBehavior, IGUIBarElement, ITickable
{
	// Token: 0x060038B7 RID: 14519 RVA: 0x0010A603 File Offset: 0x00108A03
	public void AddElement(IGUIBarElement element)
	{
		this.elements.Add(element);
		element.transform.SetParent(base.transform, false);
	}

	// Token: 0x17000DAE RID: 3502
	// (get) Token: 0x060038B8 RID: 14520 RVA: 0x0010A624 File Offset: 0x00108A24
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

	// Token: 0x060038B9 RID: 14521 RVA: 0x0010A666 File Offset: 0x00108A66
	public void setPosition(float x, float y)
	{
		base.transform.position = new Vector3(x, y);
	}

	// Token: 0x060038BA RID: 14522 RVA: 0x0010A67C File Offset: 0x00108A7C
	public void TickFrame()
	{
		for (int i = 0; i < this.elements.Count; i++)
		{
			this.elements[i].TickFrame();
		}
	}

	// Token: 0x060038BB RID: 14523 RVA: 0x0010A6B6 File Offset: 0x00108AB6
	Transform IGUIBarElement.get_transform()
	{
		return base.transform;
	}

	// Token: 0x04002708 RID: 9992
	private List<IGUIBarElement> elements = new List<IGUIBarElement>();
}
