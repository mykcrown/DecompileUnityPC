// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

public class FloatyTimer : MonoBehaviour
{
	public Image TimerCircle;

	public void SetValue(float value)
	{
		this.TimerCircle.fillAmount = value;
	}
}
