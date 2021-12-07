using System;

// Token: 0x02000495 RID: 1173
public class MatchHistoryModel : IMatchHistory
{
	// Token: 0x17000550 RID: 1360
	// (get) Token: 0x060019A9 RID: 6569 RVA: 0x000851B0 File Offset: 0x000835B0
	// (set) Token: 0x060019AA RID: 6570 RVA: 0x000851B8 File Offset: 0x000835B8
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000551 RID: 1361
	// (get) Token: 0x060019AB RID: 6571 RVA: 0x000851C1 File Offset: 0x000835C1
	// (set) Token: 0x060019AC RID: 6572 RVA: 0x000851C9 File Offset: 0x000835C9
	public VictoryScreenPayload LastVictoryPayload { get; private set; }

	// Token: 0x060019AD RID: 6573 RVA: 0x000851D2 File Offset: 0x000835D2
	[PostConstruct]
	public void Init()
	{
		this.signalBus.GetSignal<EndGameSignal>().AddListener(new Action<VictoryScreenPayload>(this.onEndGame));
	}

	// Token: 0x060019AE RID: 6574 RVA: 0x000851F0 File Offset: 0x000835F0
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

	// Token: 0x060019AF RID: 6575 RVA: 0x0008524B File Offset: 0x0008364B
	private void onEndGame(VictoryScreenPayload payload)
	{
		this.LastVictoryPayload = (VictoryScreenPayload)payload.Clone();
	}
}
