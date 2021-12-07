using System;
using TMPro;

// Token: 0x02000B72 RID: 2930
internal class TextRedrawTracker
{
	// Token: 0x17001386 RID: 4998
	// (get) Token: 0x060054B7 RID: 21687 RVA: 0x001B2F00 File Offset: 0x001B1300
	// (set) Token: 0x060054B8 RID: 21688 RVA: 0x001B2F08 File Offset: 0x001B1308
	public bool monitor { get; private set; }

	// Token: 0x060054B9 RID: 21689 RVA: 0x001B2F11 File Offset: 0x001B1311
	public void Release()
	{
		this.onResizeCallback = null;
		this.textField = null;
	}

	// Token: 0x060054BA RID: 21690 RVA: 0x001B2F21 File Offset: 0x001B1321
	public void BeginMonitoring()
	{
		this.monitor = true;
		this.monitorFrameCount = 0;
	}

	// Token: 0x060054BB RID: 21691 RVA: 0x001B2F31 File Offset: 0x001B1331
	public void TickMonitoring()
	{
		this.monitorFrameCount++;
		if (this.monitorFrameCount >= this.maxMonitoringFrames)
		{
			this.endMonitoring();
		}
	}

	// Token: 0x060054BC RID: 21692 RVA: 0x001B2F58 File Offset: 0x001B1358
	private void endMonitoring()
	{
		this.monitor = false;
	}

	// Token: 0x04003593 RID: 13715
	private int maxMonitoringFrames = 10;

	// Token: 0x04003594 RID: 13716
	public TextMeshProUGUI textField;

	// Token: 0x04003595 RID: 13717
	public float prevWidth;

	// Token: 0x04003597 RID: 13719
	private int monitorFrameCount;

	// Token: 0x04003598 RID: 13720
	public Action onResizeCallback;
}
