// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputSettingsContainer : MonoBehaviour
{
	public List<GameObject> settingsObjects;

	public List<ButtonPress> bindingOrder;

	public List<InputOptionToggleType> toggleOrder;

	public int GetElementWidth()
	{
		GridLayoutGroup component = base.GetComponent<GridLayoutGroup>();
		if (component != null && component.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
		{
			return component.constraintCount;
		}
		UnityEngine.Debug.LogError("Input Settings Containers only support objects with GridLayoutGroups.");
		return -1;
	}
}
