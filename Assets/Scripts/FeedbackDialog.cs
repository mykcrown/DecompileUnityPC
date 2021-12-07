// Decompile from assembly: Assembly-CSharp.dll

using System;

public class FeedbackDialog : GenericDialog
{
	public WavedashTMProInput InputField;

	[Inject]
	public ISendFeedback sendFeedback
	{
		get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		base.selectTextField(this.InputField);
	}

	public override void OnConfirm()
	{
		base.OnConfirm();
		this.sendFeedback.Send(this.InputField.text);
	}
}
