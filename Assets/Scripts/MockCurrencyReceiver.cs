// Decompile from assembly: Assembly-CSharp.dll

using System;

public class MockCurrencyReceiver : IUserCurrencyReceiver
{
	private bool isConnected;

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public IServerConnectionManager serverConnectionManager
	{
		get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	public int Spectra
	{
		get;
		private set;
	}

	public int CharacterUnlockTokens
	{
		get;
		private set;
	}

	[PostConstruct]
	public void Init()
	{
		this.Spectra = this.gameDataManager.ConfigData.DebugConfig.offlineModeSettings.startCurrency;
		this.CharacterUnlockTokens = this.gameDataManager.ConfigData.DebugConfig.offlineModeSettings.startUnlockTokens;
		this.signalBus.AddListener(ServerConnectionManager.UPDATED, new Action(this.onServerConnectionUpdate));
	}

	public void Request()
	{
	}

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

	private void setSpectra(int amount)
	{
		this.Spectra = amount;
		this.signalBus.Dispatch(UserCurrencyModel.CURRENCY_RECIEVED);
	}

	private void setUnlockTokens(int amount)
	{
		this.CharacterUnlockTokens = amount;
		this.signalBus.Dispatch(UserCurrencyModel.UNLOCK_TOKENS_RECIEVED);
	}
}
