using System;
using UnityEngine;

// Token: 0x02000ABD RID: 2749
public class FPSCounter : MonoBehaviour, ITickable
{
	// Token: 0x170012EE RID: 4846
	// (get) Token: 0x06005087 RID: 20615 RVA: 0x00150211 File Offset: 0x0014E611
	// (set) Token: 0x06005088 RID: 20616 RVA: 0x00150219 File Offset: 0x0014E619
	public float FPS { get; private set; }

	// Token: 0x06005089 RID: 20617 RVA: 0x00150224 File Offset: 0x0014E624
	private void Awake()
	{
		for (int i = 0; i < this.timeRecord.Length; i++)
		{
			this.timeRecord[i] = 0;
		}
	}

	// Token: 0x0600508A RID: 20618 RVA: 0x00150253 File Offset: 0x0014E653
	private void Update()
	{
		if (this.RecordOnUpdate)
		{
			this.onNewFrame();
		}
	}

	// Token: 0x0600508B RID: 20619 RVA: 0x00150266 File Offset: 0x0014E666
	public void TickFrame()
	{
		if (!this.RecordOnUpdate && GameClient.IsCurrentFrame)
		{
			this.onNewFrame();
		}
	}

	// Token: 0x0600508C RID: 20620 RVA: 0x00150284 File Offset: 0x0014E684
	private void onNewFrame()
	{
		float num = Time.realtimeSinceStartup - this.lastTime;
		this.lastTime = Time.realtimeSinceStartup;
		this.timeRecord[this.arrayPosition] = (int)(num * 1000000f);
		this.arrayPosition++;
		if (this.arrayPosition >= this.timeRecord.Length)
		{
			this.arrayPosition = 0;
		}
		if (Time.realtimeSinceStartup >= this.lastRecordedTime + 1f)
		{
			this.recordTime();
		}
	}

	// Token: 0x0600508D RID: 20621 RVA: 0x00150304 File Offset: 0x0014E704
	private void recordTime()
	{
		this.lastRecordedTime = Time.realtimeSinceStartup;
		float worst = this.getWorst(0, 20);
		float worst2 = this.getWorst(20, 20);
		float worst3 = this.getWorst(40, 20);
		float num = (worst + worst2 + worst3) / 3f;
		if (num == 0f)
		{
			this.FPS = 60f;
		}
		else
		{
			this.FPS = 1000000f / num;
		}
		if (this.OnRecord != null)
		{
			this.OnRecord();
		}
	}

	// Token: 0x0600508E RID: 20622 RVA: 0x00150388 File Offset: 0x0014E788
	private float getWorst(int segmentStart, int segmentSize)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < segmentSize; i++)
		{
			int num3 = this.arrayPosition - i - segmentStart;
			if (num3 < 0)
			{
				num3 += this.timeRecord.Length;
			}
			num2 += this.timeRecord[num3];
			if (this.timeRecord[num3] > num)
			{
				num = this.timeRecord[num3];
			}
		}
		return (float)(num2 / segmentSize);
	}

	// Token: 0x040033C6 RID: 13254
	private int arrayPosition;

	// Token: 0x040033C7 RID: 13255
	private int[] timeRecord = new int[300];

	// Token: 0x040033C8 RID: 13256
	private float lastTime;

	// Token: 0x040033C9 RID: 13257
	private float lastRecordedTime;

	// Token: 0x040033CA RID: 13258
	public bool RecordOnUpdate;

	// Token: 0x040033CB RID: 13259
	public Action OnRecord;
}
