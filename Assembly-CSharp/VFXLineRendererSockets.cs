using System;
using UnityEngine;

// Token: 0x0200029A RID: 666
public class VFXLineRendererSockets : MonoBehaviour
{
	// Token: 0x06000E09 RID: 3593 RVA: 0x00058918 File Offset: 0x00056D18
	private void Start()
	{
		LineRenderer component = base.GetComponent<LineRenderer>();
		component.SetPosition(0, this.lineStart.transform.localPosition);
		component.SetPosition(1, this.lineEnd.transform.localPosition);
	}

	// Token: 0x06000E0A RID: 3594 RVA: 0x0005895A File Offset: 0x00056D5A
	private void Update()
	{
	}

	// Token: 0x04000840 RID: 2112
	public GameObject lineStart;

	// Token: 0x04000841 RID: 2113
	public GameObject lineEnd;
}
