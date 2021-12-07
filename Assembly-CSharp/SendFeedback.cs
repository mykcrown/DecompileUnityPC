using System;
using IconsServer;
using UnityEngine;

// Token: 0x020007F6 RID: 2038
public class SendFeedback : ISendFeedback
{
	// Token: 0x17000C43 RID: 3139
	// (get) Token: 0x0600324A RID: 12874 RVA: 0x000F2C02 File Offset: 0x000F1002
	// (set) Token: 0x0600324B RID: 12875 RVA: 0x000F2C0A File Offset: 0x000F100A
	[Inject]
	public IIconsServerAPI iconsServerAPI { get; set; }

	// Token: 0x17000C44 RID: 3140
	// (get) Token: 0x0600324C RID: 12876 RVA: 0x000F2C13 File Offset: 0x000F1013
	// (set) Token: 0x0600324D RID: 12877 RVA: 0x000F2C1B File Offset: 0x000F101B
	[Inject]
	public IDialogController dialogController { get; set; }

	// Token: 0x17000C45 RID: 3141
	// (get) Token: 0x0600324E RID: 12878 RVA: 0x000F2C24 File Offset: 0x000F1024
	// (set) Token: 0x0600324F RID: 12879 RVA: 0x000F2C2C File Offset: 0x000F102C
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x17000C46 RID: 3142
	// (get) Token: 0x06003250 RID: 12880 RVA: 0x000F2C35 File Offset: 0x000F1035
	// (set) Token: 0x06003251 RID: 12881 RVA: 0x000F2C3D File Offset: 0x000F103D
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x06003252 RID: 12882 RVA: 0x000F2C46 File Offset: 0x000F1046
	[PostConstruct]
	public void Init()
	{
	}

	// Token: 0x06003253 RID: 12883 RVA: 0x000F2C48 File Offset: 0x000F1048
	private void onPlayerFeedback(ServerEvent evt)
	{
	}

	// Token: 0x06003254 RID: 12884 RVA: 0x000F2C4C File Offset: 0x000F104C
	private void onResult()
	{
		float num = Time.realtimeSinceStartup - this.sendBeginTime;
		if (num < this.MIN_TIME)
		{
			float num2 = this.MIN_TIME - num + 0.1f;
			this.timer.SetTimeout((int)(num2 * 1000f), new Action(this.onResult));
		}
		else
		{
			this.showResult();
		}
	}

	// Token: 0x06003255 RID: 12885 RVA: 0x000F2CAB File Offset: 0x000F10AB
	private void showResult()
	{
		if (this.spinny != null)
		{
			this.spinny.Close();
			this.spinny = null;
		}
	}

	// Token: 0x06003256 RID: 12886 RVA: 0x000F2CD0 File Offset: 0x000F10D0
	public void Send(string text)
	{
		this.sendBeginTime = Time.realtimeSinceStartup;
		this.spinny = this.dialogController.ShowSpinnyDialog(this.localization.GetText("ui.feedbackSending.title"), WindowTransition.STANDARD_FADE);
	}

	// Token: 0x04002363 RID: 9059
	private float MIN_TIME = 1.25f;

	// Token: 0x04002368 RID: 9064
	private GenericDialog spinny;

	// Token: 0x04002369 RID: 9065
	private float sendBeginTime;
}
