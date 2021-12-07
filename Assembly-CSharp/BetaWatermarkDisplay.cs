using System;
using TMPro;

// Token: 0x02000A7C RID: 2684
public class BetaWatermarkDisplay : BaseGamewideOverlay
{
	// Token: 0x17001291 RID: 4753
	// (get) Token: 0x06004E70 RID: 20080 RVA: 0x001499BA File Offset: 0x00147DBA
	// (set) Token: 0x06004E71 RID: 20081 RVA: 0x001499C2 File Offset: 0x00147DC2
	[Inject]
	public IAccountAPI accountAPI { get; set; }

	// Token: 0x17001292 RID: 4754
	// (get) Token: 0x06004E72 RID: 20082 RVA: 0x001499CB File Offset: 0x00147DCB
	// (set) Token: 0x06004E73 RID: 20083 RVA: 0x001499D3 File Offset: 0x00147DD3
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x06004E74 RID: 20084 RVA: 0x001499DC File Offset: 0x00147DDC
	[PostConstruct]
	public void Init()
	{
		base.signalBus.AddListener("AccountAPI.UPDATE", new Action(this.onUpdated));
		this.onUpdated();
	}

	// Token: 0x06004E75 RID: 20085 RVA: 0x00149A00 File Offset: 0x00147E00
	private void onUpdated()
	{
		string userName = this.accountAPI.UserName;
		string displayVersion = this.config.uiuxSettings.displayVersion;
		this.MainText.text = string.Format("{0} - Early Access\n{1}", displayVersion, userName);
	}

	// Token: 0x04003334 RID: 13108
	public TextMeshProUGUI MainText;
}
