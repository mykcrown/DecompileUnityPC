using System;

// Token: 0x020009A4 RID: 2468
public class FeedbackDialog : GenericDialog
{
	// Token: 0x17000FFF RID: 4095
	// (get) Token: 0x0600439E RID: 17310 RVA: 0x0012AE16 File Offset: 0x00129216
	// (set) Token: 0x0600439F RID: 17311 RVA: 0x0012AE1E File Offset: 0x0012921E
	[Inject]
	public ISendFeedback sendFeedback { get; set; }

	// Token: 0x060043A0 RID: 17312 RVA: 0x0012AE27 File Offset: 0x00129227
	[PostConstruct]
	public void Init()
	{
		base.selectTextField(this.InputField);
	}

	// Token: 0x060043A1 RID: 17313 RVA: 0x0012AE35 File Offset: 0x00129235
	public override void OnConfirm()
	{
		base.OnConfirm();
		this.sendFeedback.Send(this.InputField.text);
	}

	// Token: 0x04002D0D RID: 11533
	public WavedashTMProInput InputField;
}
