using System;

// Token: 0x020006E3 RID: 1763
public class UserCurrencyModel : IUserCurrencyModel
{
	// Token: 0x17000AE4 RID: 2788
	// (get) Token: 0x06002C62 RID: 11362 RVA: 0x000E5532 File Offset: 0x000E3932
	// (set) Token: 0x06002C63 RID: 11363 RVA: 0x000E553A File Offset: 0x000E393A
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000AE5 RID: 2789
	// (get) Token: 0x06002C64 RID: 11364 RVA: 0x000E5543 File Offset: 0x000E3943
	// (set) Token: 0x06002C65 RID: 11365 RVA: 0x000E554B File Offset: 0x000E394B
	[Inject]
	public IUserCurrencyReceiver userCurrencyReceiver { get; set; }

	// Token: 0x06002C66 RID: 11366 RVA: 0x000E5554 File Offset: 0x000E3954
	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(UserCurrencyModel.CURRENCY_RECIEVED, new Action(this.onCurrencyReceived));
		this.signalBus.AddListener(UserCurrencyModel.UNLOCK_TOKENS_RECIEVED, new Action(this.onUnlockTokensReceived));
		this.Spectra = this.userCurrencyReceiver.Spectra;
		this.CharacterUnlockTokens = this.userCurrencyReceiver.CharacterUnlockTokens;
	}

	// Token: 0x06002C67 RID: 11367 RVA: 0x000E55BB File Offset: 0x000E39BB
	private void onCurrencyReceived()
	{
		this.Spectra = this.userCurrencyReceiver.Spectra;
	}

	// Token: 0x06002C68 RID: 11368 RVA: 0x000E55CE File Offset: 0x000E39CE
	private void onUnlockTokensReceived()
	{
		this.CharacterUnlockTokens = this.userCurrencyReceiver.CharacterUnlockTokens;
	}

	// Token: 0x17000AE6 RID: 2790
	// (get) Token: 0x06002C69 RID: 11369 RVA: 0x000E55E1 File Offset: 0x000E39E1
	// (set) Token: 0x06002C6A RID: 11370 RVA: 0x000E55E9 File Offset: 0x000E39E9
	public int Spectra
	{
		get
		{
			return this.spectra;
		}
		set
		{
			if (this.spectra != value)
			{
				this.spectra = value;
				this.signalBus.Dispatch(UserCurrencyModel.UPDATED);
			}
		}
	}

	// Token: 0x17000AE7 RID: 2791
	// (get) Token: 0x06002C6B RID: 11371 RVA: 0x000E560E File Offset: 0x000E3A0E
	// (set) Token: 0x06002C6C RID: 11372 RVA: 0x000E5616 File Offset: 0x000E3A16
	public int CharacterUnlockTokens
	{
		get
		{
			return this.characterUnlockTokens;
		}
		set
		{
			if (this.characterUnlockTokens != value)
			{
				this.characterUnlockTokens = value;
				this.signalBus.Dispatch(UserCurrencyModel.UPDATED);
			}
		}
	}

	// Token: 0x04001F7D RID: 8061
	public static string UPDATED = "UserCurrencyModel.UPDATED";

	// Token: 0x04001F7E RID: 8062
	public static string CURRENCY_RECIEVED = "UserCurrencyReceiver.CURRENCY_RECIEVED";

	// Token: 0x04001F7F RID: 8063
	public static string UNLOCK_TOKENS_RECIEVED = "UserCurrencyReceiver.UNLOCK_TOKENS_RECIEVED";

	// Token: 0x04001F82 RID: 8066
	private int spectra;

	// Token: 0x04001F83 RID: 8067
	private int characterUnlockTokens;
}
