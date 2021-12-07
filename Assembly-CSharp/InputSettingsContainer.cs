using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000972 RID: 2418
public class InputSettingsContainer : MonoBehaviour
{
	// Token: 0x060040ED RID: 16621 RVA: 0x00124A20 File Offset: 0x00122E20
	public int GetElementWidth()
	{
		GridLayoutGroup component = base.GetComponent<GridLayoutGroup>();
		if (component != null && component.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
		{
			return component.constraintCount;
		}
		Debug.LogError("Input Settings Containers only support objects with GridLayoutGroups.");
		return -1;
	}

	// Token: 0x04002BC4 RID: 11204
	public List<GameObject> settingsObjects;

	// Token: 0x04002BC5 RID: 11205
	public List<ButtonPress> bindingOrder;

	// Token: 0x04002BC6 RID: 11206
	public List<InputOptionToggleType> toggleOrder;
}
