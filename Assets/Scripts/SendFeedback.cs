// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;
using UnityEngine;

public class SendFeedback : ISendFeedback
{
	private float MIN_TIME = 1.25f;

	private GenericDialog spinny;

	private float sendBeginTime;

	[Inject]
	public IIconsServerAPI iconsServerAPI
	{
		get;
		set;
	}

	[Inject]
	public IDialogController dialogController
	{
		get;
		set;
	}

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	[Inject]
	public IMainThreadTimer timer
	{
		get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
	}

	private void onPlayerFeedback(ServerEvent evt)
	{
	}

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

	private void showResult()
	{
		if (this.spinny != null)
		{
			this.spinny.Close();
			this.spinny = null;
		}
	}

	public void Send(string text)
	{
		this.sendBeginTime = Time.realtimeSinceStartup;
		this.spinny = this.dialogController.ShowSpinnyDialog(this.localization.GetText("ui.feedbackSending.title"), WindowTransition.STANDARD_FADE);
	}
}
