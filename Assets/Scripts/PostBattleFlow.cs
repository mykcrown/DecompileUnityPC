// Decompile from assembly: Assembly-CSharp.dll

using System;

public class PostBattleFlow : IPostBattleFlow
{
	[Inject]
	public IDialogController dialog
	{
		get;
		set;
	}

	[Inject]
	public ILocalization localization
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
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public ICustomLobbyController lobbyController
	{
		get;
		set;
	}

	[Inject]
	public IRichPresence richPresence
	{
		get;
		set;
	}

	[Inject]
	public IEnterNewGame enterNewGame
	{
		get;
		set;
	}

	public void ExitPostGame(VictoryScreenPayload victoryPayload)
	{
		ScreenType type = ScreenType.CharacterSelect;
		if (victoryPayload.gamePayload != null)
		{
			if (this.battleServerAPI.IsConnected)
			{
				type = ScreenType.OnlineBlindPick;
				this.enterNewGame.InitPayload(GameStartType.FreePlay, victoryPayload.gamePayload);
				this.richPresence.SetPresence("InCharacterSelect", null, null, null);
			}
			else if (this.lobbyController.IsInLobby)
			{
				type = ScreenType.CustomLobbyScreen;
				this.richPresence.SetPresence("InCustomLobby", null, null, null);
			}
			else if (victoryPayload.gamePayload.isOnlineGame)
			{
				type = ScreenType.MainMenu;
				this.richPresence.SetPresence(null, null, null, null);
			}
		}
		this.events.Broadcast(new LoadScreenCommand(type, null, ScreenUpdateType.Next));
	}
}
