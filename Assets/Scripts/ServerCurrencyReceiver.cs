// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;

public class ServerCurrencyReceiver : IUserCurrencyReceiver
{
	private ulong spectraBalance;

	private ulong unlockTokenBalance;

	[Inject]
	public IIconsServerAPI iconsServerAPI
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public IStaticDataSource staticDataSource
	{
		get;
		set;
	}

	public int Spectra
	{
		get
		{
			return (int)this.spectraBalance;
		}
	}

	public int CharacterUnlockTokens
	{
		get
		{
			return (int)this.unlockTokenBalance;
		}
	}

	[PostConstruct]
	public void Init()
	{
	}
}
