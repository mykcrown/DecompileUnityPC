using System;
using UnityEngine;

// Token: 0x02000B3A RID: 2874
public class CameraRenderPerformance : MonoBehaviour
{
	// Token: 0x06005358 RID: 21336 RVA: 0x001AF290 File Offset: 0x001AD690
	private void FixedUpdate()
	{
		this.fullFrameTime = Time.realtimeSinceStartup;
	}

	// Token: 0x06005359 RID: 21337 RVA: 0x001AF2A0 File Offset: 0x001AD6A0
	private void OnGUI()
	{
		float num = (Time.realtimeSinceStartup - this.fullFrameTime) * 1000f;
		this.fullFrameCount++;
		this.fullFrameSum += num;
		if (this.fullFrameCount == 300)
		{
			float num2 = this.fullFrameSum / (float)this.fullFrameCount;
			Debug.Log("Frame time " + num2);
			this.fullFrameCount = 0;
			this.fullFrameSum = 0f;
		}
	}

	// Token: 0x040034E9 RID: 13545
	private float fullFrameTime;

	// Token: 0x040034EA RID: 13546
	private int fullFrameCount;

	// Token: 0x040034EB RID: 13547
	private float fullFrameSum;
}
