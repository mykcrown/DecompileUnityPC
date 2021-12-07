using System;

// Token: 0x020006E1 RID: 1761
public class MockCurrencyReceiver : IUserCurrencyReceiver
{
	// Token: 0x17000ADA RID: 2778
	// (get) Token: 0x06002C48 RID: 11336 RVA: 0x000E5395 File Offset: 0x000E3795
	// (set) Token: 0x06002C49 RID: 11337 RVA: 0x000E539D File Offset: 0x000E379D
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000ADB RID: 2779
	// (get) Token: 0x06002C4A RID: 11338 RVA: 0x000E53A6 File Offset: 0x000E37A6
	// (set) Token: 0x06002C4B RID: 11339 RVA: 0x000E53AE File Offset: 0x000E37AE
	[Inject]
	public IServerConnectionManager serverConnectionManager { get; set; }

	// Token: 0x17000ADC RID: 2780
	// (get) Token: 0x06002C4C RID: 11340 RVA: 0x000E53B7 File Offset: 0x000E37B7
	// (set) Token: 0x06002C4D RID: 11341 RVA: 0x000E53BF File Offset: 0x000E37BF
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x17000ADD RID: 2781
	// (get) Token: 0x06002C4E RID: 11342 RVA: 0x000E53C8 File Offset: 0x000E37C8
	// (set) Token: 0x06002C4F RID: 11343 RVA: 0x000E53D0 File Offset: 0x000E37D0
	public int Spectra { get; private set; }

	// Token: 0x17000ADE RID: 2782
	// (get) Token: 0x06002C50 RID: 11344 RVA: 0x000E53D9 File Offset: 0x000E37D9
	// (set) Token: 0x06002C51 RID: 11345 RVA: 0x000E53E1 File Offset: 0x000E37E1
	public int CharacterUnlockTokens { get; private set; }

	// Token: 0x06002C52 RID: 11346 RVA: 0x000E53EC File Offset: 0x000E37EC
	[PostConstruct]
	public void Init()
	{
		this.Spectra = this.gameDataManager.ConfigData.DebugConfig.offlineModeSettings.startCurrency;
		this.CharacterUnlockTokens = this.gameDataManager.ConfigData.DebugConfig.offlineModeSettings.startUnlockTokens;
		this.signalBus.AddListener(ServerConnectionManager.UPDATED, new Action(this.onServerConnectionUpdate));
	}

	// Token: 0x06002C53 RID: 11347 RVA: 0x000E5455 File Offset: 0x000E3855
	public void Request()
	{
	}

	// Token: 0x06002C54 RID: 11348 RVA: 0x000E5458 File Offset: 0x000E3858
	private void onServerConnectionUpdate()
	{
		if (this.serverConnectionManager.IsConnectedToNexus != this.isConnected)
		{
			this.isConnected = this.serverConnectionManager.IsConnectedToNexus;
			if (this.isConnected)
			{
				this.setSpectra(10000);
				this.setUnlockTokens(2);
			}
		}
	}

	// Token: 0x06002C55 RID: 11349 RVA: 0x000E54A9 File Offset: 0x000E38A9
	private void setSpectra(int amount)
	{
		this.Spectra = amount;
		this.signalBus.Dispatch(UserCurrencyModel.CURRENCY_RECIEVED);
	}

	// Token: 0x06002C56 RID: 11350 RVA: 0x000E54C2 File Offset: 0x000E38C2
	private void setUnlockTokens(int amount)
	{
		this.CharacterUnlockTokens = amount;
		this.signalBus.Dispatch(UserCurrencyModel.UNLOCK_TOKENS_RECIEVED);
	}

	// Token: 0x04001F77 RID: 8055
	private bool isConnected;
}
