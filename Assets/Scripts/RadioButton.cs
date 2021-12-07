// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class RadioButton : MonoBehaviour
{
	public MenuItemButton Button;

	public CanvasGroup Toggle;

	public void SetToggle(bool isOn)
	{
		if (isOn)
		{
			this.Toggle.alpha = 1f;
		}
		else
		{
			this.Toggle.alpha = 0f;
		}
	}
}
