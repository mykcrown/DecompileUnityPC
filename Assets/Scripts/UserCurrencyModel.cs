// Decompile from assembly: Assembly-CSharp.dll

using System;

public class UserCurrencyModel : IUserCurrencyModel
{
	public static string UPDATED = "UserCurrencyModel.UPDATED";

	public static string CURRENCY_RECIEVED = "UserCurrencyReceiver.CURRENCY_RECIEVED";

	public static string UNLOCK_TOKENS_RECIEVED = "UserCurrencyReceiver.UNLOCK_TOKENS_RECIEVED";

	private int spectra;

	private int characterUnlockTokens;

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public IUserCurrencyReceiver userCurrencyReceiver
	{
		get;
		set;
	}

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

	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(UserCurrencyModel.CURRENCY_RECIEVED, new Action(this.onCurrencyReceived));
		this.signalBus.AddListener(UserCurrencyModel.UNLOCK_TOKENS_RECIEVED, new Action(this.onUnlockTokensReceived));
		this.Spectra = this.userCurrencyReceiver.Spectra;
		this.CharacterUnlockTokens = this.userCurrencyReceiver.CharacterUnlockTokens;
	}

	private void onCurrencyReceived()
	{
		this.Spectra = this.userCurrencyReceiver.Spectra;
	}

	private void onUnlockTokensReceived()
	{
		this.CharacterUnlockTokens = this.userCurrencyReceiver.CharacterUnlockTokens;
	}
}
