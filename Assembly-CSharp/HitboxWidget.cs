using System;
using UnityEngine.UI;

// Token: 0x02000A3C RID: 2620
public class HitboxWidget : TrainingWidget
{
	// Token: 0x17001231 RID: 4657
	// (get) Token: 0x06004CB6 RID: 19638 RVA: 0x00145338 File Offset: 0x00143738
	// (set) Token: 0x06004CB7 RID: 19639 RVA: 0x00145345 File Offset: 0x00143745
	public bool IsEnabled
	{
		get
		{
			return DebugDraw.Instance.IsChannelActive(DebugDrawChannel.HitBoxes);
		}
		set
		{
			DebugDraw.Instance.SetChannelActive(DebugDrawChannel.HitBoxes, value);
		}
	}

	// Token: 0x06004CB8 RID: 19640 RVA: 0x00145353 File Offset: 0x00143753
	protected override void Start()
	{
		base.Start();
		this.IsEnabled = false;
	}

	// Token: 0x06004CB9 RID: 19641 RVA: 0x00145362 File Offset: 0x00143762
	public override void OnLeft()
	{
		this.IsEnabled = false;
	}

	// Token: 0x06004CBA RID: 19642 RVA: 0x0014536B File Offset: 0x0014376B
	public override void OnRight()
	{
		this.IsEnabled = true;
	}

	// Token: 0x06004CBB RID: 19643 RVA: 0x00145374 File Offset: 0x00143774
	private void Update()
	{
		if (this.EnabledText != null)
		{
			this.EnabledText.text = ((!this.IsEnabled) ? "Off" : "On");
		}
	}

	// Token: 0x04003256 RID: 12886
	public Text EnabledText;
}
