// Decompile from assembly: Assembly-CSharp.dll

using System;

public class MatchHistoryModel : IMatchHistory
{
	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	public VictoryScreenPayload LastVictoryPayload
	{
		get;
		private set;
	}

	[PostConstruct]
	public void Init()
	{
		this.signalBus.GetSignal<EndGameSignal>().AddListener(new Action<VictoryScreenPayload>(this.onEndGame));
	}

	public CharacterID GetFirstLocalCharacterID(VictoryScreenPayload victoryPayload)
	{
		if (victoryPayload == null)
		{
			return CharacterID.None;
		}
		for (int i = 0; i < victoryPayload.gamePayload.players.Length; i++)
		{
			PlayerSelectionInfo playerSelectionInfo = victoryPayload.gamePayload.players[i];
			if (playerSelectionInfo.isLocal)
			{
				return playerSelectionInfo.characterID;
			}
		}
		return CharacterID.None;
	}

	private void onEndGame(VictoryScreenPayload payload)
	{
		this.LastVictoryPayload = (VictoryScreenPayload)payload.Clone();
	}
}
