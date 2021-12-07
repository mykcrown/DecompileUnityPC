// Decompile from assembly: Assembly-CSharp.dll

using System;

public class EnterCustomLobbyController
{
	private bool wasInLobby;

	[Inject]
	public ICustomLobbyController customLobby
	{
		get;
		set;
	}

	[Inject]
	public IEvents events
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

	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(CustomLobbyController.UPDATED, new Action(this.checkLoadCustomLobby));
		this.wasInLobby = false;
	}

	private void checkLoadCustomLobby()
	{
		if (this.customLobby.IsInLobby && !this.wasInLobby)
		{
			this.events.Broadcast(new LoadScreenCommand(ScreenType.CustomLobbyScreen, null, ScreenUpdateType.Next));
		}
		this.wasInLobby = this.customLobby.IsInLobby;
	}
}
