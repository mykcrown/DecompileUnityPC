using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200047A RID: 1146
public class HologramAnimationEvents : MonoBehaviour
{
	// Token: 0x1400001C RID: 28
	// (add) Token: 0x060018D1 RID: 6353 RVA: 0x00082DC8 File Offset: 0x000811C8
	// (remove) Token: 0x060018D2 RID: 6354 RVA: 0x00082E00 File Offset: 0x00081200
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event HologramAnimationEventHandler OnSlideAway;

	// Token: 0x1400001D RID: 29
	// (add) Token: 0x060018D3 RID: 6355 RVA: 0x00082E38 File Offset: 0x00081238
	// (remove) Token: 0x060018D4 RID: 6356 RVA: 0x00082E70 File Offset: 0x00081270
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event HologramAnimationEventHandler OnReleaseCamera;

	// Token: 0x060018D5 RID: 6357 RVA: 0x00082EA6 File Offset: 0x000812A6
	public void OnSlideAwayComplete()
	{
		this.OnSlideAway();
	}

	// Token: 0x060018D6 RID: 6358 RVA: 0x00082EB3 File Offset: 0x000812B3
	public void OnReleaseCameraEvent()
	{
		this.OnReleaseCamera();
	}
}
