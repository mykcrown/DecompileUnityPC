using System;
using UnityEngine.UI;

// Token: 0x02000A3D RID: 2621
public class HurtboxWidget : TrainingWidget
{
	// Token: 0x17001232 RID: 4658
	// (get) Token: 0x06004CBD RID: 19645 RVA: 0x001453B4 File Offset: 0x001437B4
	// (set) Token: 0x06004CBE RID: 19646 RVA: 0x001453C1 File Offset: 0x001437C1
	public bool IsEnabled
	{
		get
		{
			return DebugDraw.Instance.IsChannelActive(DebugDrawChannel.HurtBoxes);
		}
		set
		{
			DebugDraw.Instance.SetChannelActive(DebugDrawChannel.HurtBoxes, value);
		}
	}

	// Token: 0x06004CBF RID: 19647 RVA: 0x001453CF File Offset: 0x001437CF
	protected override void Start()
	{
		base.Start();
		this.IsEnabled = false;
	}

	// Token: 0x06004CC0 RID: 19648 RVA: 0x001453DE File Offset: 0x001437DE
	public override void OnLeft()
	{
		this.IsEnabled = false;
	}

	// Token: 0x06004CC1 RID: 19649 RVA: 0x001453E7 File Offset: 0x001437E7
	public override void OnRight()
	{
		this.IsEnabled = true;
	}

	// Token: 0x06004CC2 RID: 19650 RVA: 0x001453F0 File Offset: 0x001437F0
	private void Update()
	{
		if (this.EnabledText != null)
		{
			this.EnabledText.text = ((!this.IsEnabled) ? "Off" : "On");
		}
	}

	// Token: 0x04003257 RID: 12887
	public Text EnabledText;
}
