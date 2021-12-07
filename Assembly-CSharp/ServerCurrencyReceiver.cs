using System;
using IconsServer;

// Token: 0x020006E2 RID: 1762
public class ServerCurrencyReceiver : IUserCurrencyReceiver
{
	// Token: 0x17000ADF RID: 2783
	// (get) Token: 0x06002C58 RID: 11352 RVA: 0x000E54E3 File Offset: 0x000E38E3
	// (set) Token: 0x06002C59 RID: 11353 RVA: 0x000E54EB File Offset: 0x000E38EB
	[Inject]
	public IIconsServerAPI iconsServerAPI { get; set; }

	// Token: 0x17000AE0 RID: 2784
	// (get) Token: 0x06002C5A RID: 11354 RVA: 0x000E54F4 File Offset: 0x000E38F4
	// (set) Token: 0x06002C5B RID: 11355 RVA: 0x000E54FC File Offset: 0x000E38FC
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000AE1 RID: 2785
	// (get) Token: 0x06002C5C RID: 11356 RVA: 0x000E5505 File Offset: 0x000E3905
	// (set) Token: 0x06002C5D RID: 11357 RVA: 0x000E550D File Offset: 0x000E390D
	[Inject]
	public IStaticDataSource staticDataSource { get; set; }

	// Token: 0x17000AE2 RID: 2786
	// (get) Token: 0x06002C5E RID: 11358 RVA: 0x000E5516 File Offset: 0x000E3916
	public int Spectra
	{
		get
		{
			return (int)this.spectraBalance;
		}
	}

	// Token: 0x17000AE3 RID: 2787
	// (get) Token: 0x06002C5F RID: 11359 RVA: 0x000E551F File Offset: 0x000E391F
	public int CharacterUnlockTokens
	{
		get
		{
			return (int)this.unlockTokenBalance;
		}
	}

	// Token: 0x06002C60 RID: 11360 RVA: 0x000E5528 File Offset: 0x000E3928
	[PostConstruct]
	public void Init()
	{
	}

	// Token: 0x04001F7B RID: 8059
	private ulong spectraBalance;

	// Token: 0x04001F7C RID: 8060
	private ulong unlockTokenBalance;
}
