// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;

public class AccountAPI : IAccountAPI
{
	public const string UPDATED = "AccountAPI.UPDATE";

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

	public string UserName
	{
		get;
		private set;
	}

	public ulong ID
	{
		get;
		private set;
	}

	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(SteamManager.STEAM_INITIALIZED, new Action(this.onUpdateName));
	}

	public void Initialize()
	{
		this.UserName = this.iconsServerAPI.Username;
	}

	private void onUpdateName()
	{
		this.UserName = this.iconsServerAPI.Username;
	}
}
