// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;

public class UserFacingSystemClock : BaseGamewideOverlay
{
	public TextMeshProUGUI Text;

	private StaticString displayString = new StaticString(8);

	[Inject]
	public IUserVideoSettingsModel userVideoSettingsModel
	{
		get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		base.signalBus.AddListener(UserVideoSettingsModel.UPDATED, new Action(this.onUpdate));
		this.onUpdate();
	}

	private void onUpdate()
	{
		base.gameObject.SetActive(this.userVideoSettingsModel.ShowSystemClock);
	}

	private void Update()
	{
		this.displayString.Reset();
		TimeUtil.FormatClockDisplay(DateTime.Now, ref this.displayString);
		this.Text.SetCharArray(this.displayString.arr, 0, this.displayString.len);
	}
}
