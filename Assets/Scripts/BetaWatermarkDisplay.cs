// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;

public class BetaWatermarkDisplay : BaseGamewideOverlay
{
	public TextMeshProUGUI MainText;

	[Inject]
	public IAccountAPI accountAPI
	{
		get;
		set;
	}

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		base.signalBus.AddListener("AccountAPI.UPDATE", new Action(this.onUpdated));
		this.onUpdated();
	}

	private void onUpdated()
	{
		string userName = this.accountAPI.UserName;
		string displayVersion = this.config.uiuxSettings.displayVersion;
		this.MainText.text = string.Format("{0} - Early Access\n{1}", displayVersion, userName);
	}
}
