// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;

public class ConnectionController
{
	[Inject]
	public IAccountAPI accountAPI
	{
		get;
		set;
	}

	[Inject]
	public IBattleServerAPI battleServerAPI
	{
		get;
		set;
	}

	[Inject]
	public IIconsServerAPI iconsServerAPI
	{
		get;
		set;
	}

	[Inject]
	public ICustomLobbyController customLobby
	{
		get;
		set;
	}

	public void Initialize()
	{
		this.accountAPI.Initialize();
		this.battleServerAPI.Initialize();
		this.customLobby.Initialize();
	}

	public void OnDestroy()
	{
		this.battleServerAPI.OnDestroy();
		this.iconsServerAPI.Shutdown();
	}
}
