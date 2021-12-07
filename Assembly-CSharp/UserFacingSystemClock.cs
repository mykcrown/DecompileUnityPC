using System;
using TMPro;

// Token: 0x02000A81 RID: 2689
public class UserFacingSystemClock : BaseGamewideOverlay
{
	// Token: 0x1700129C RID: 4764
	// (get) Token: 0x06004E95 RID: 20117 RVA: 0x00149E3C File Offset: 0x0014823C
	// (set) Token: 0x06004E96 RID: 20118 RVA: 0x00149E44 File Offset: 0x00148244
	[Inject]
	public IUserVideoSettingsModel userVideoSettingsModel { get; set; }

	// Token: 0x06004E97 RID: 20119 RVA: 0x00149E4D File Offset: 0x0014824D
	[PostConstruct]
	public void Init()
	{
		base.signalBus.AddListener(UserVideoSettingsModel.UPDATED, new Action(this.onUpdate));
		this.onUpdate();
	}

	// Token: 0x06004E98 RID: 20120 RVA: 0x00149E71 File Offset: 0x00148271
	private void onUpdate()
	{
		base.gameObject.SetActive(this.userVideoSettingsModel.ShowSystemClock);
	}

	// Token: 0x06004E99 RID: 20121 RVA: 0x00149E89 File Offset: 0x00148289
	private void Update()
	{
		this.displayString.Reset();
		TimeUtil.FormatClockDisplay(DateTime.Now, ref this.displayString);
		this.Text.SetCharArray(this.displayString.arr, 0, this.displayString.len);
	}

	// Token: 0x04003346 RID: 13126
	public TextMeshProUGUI Text;

	// Token: 0x04003347 RID: 13127
	private StaticString displayString = new StaticString(8);
}
