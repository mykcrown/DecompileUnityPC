using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008BD RID: 2237
public class FloatyTimer : MonoBehaviour
{
	// Token: 0x0600386E RID: 14446 RVA: 0x00108B98 File Offset: 0x00106F98
	public void SetValue(float value)
	{
		this.TimerCircle.fillAmount = value;
	}

	// Token: 0x040026CF RID: 9935
	public Image TimerCircle;
}
