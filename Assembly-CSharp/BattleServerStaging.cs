using System;
using System.Collections.Generic;
using System.Linq;
using IconsServer;
using MatchMaking;
using P2P;
using UnityEngine;

// Token: 0x020007A5 RID: 1957
public class BattleServerStaging : IBattleServerStagingAPI
{
	// Token: 0x17000BCE RID: 3022
	// (get) Token: 0x060030B3 RID: 12467 RVA: 0x000F0728 File Offset: 0x000EEB28
	// (set) Token: 0x060030B4 RID: 12468 RVA: 0x000F0730 File Offset: 0x000EEB30
	[Inject]
	public IBattleServerAPI battleServerAPI { get; set; }

	// Token: 0x17000BCF RID: 3023
	// (get) Token: 0x060030B5 RID: 12469 RVA: 0x000F0739 File Offset: 0x000EEB39
	// (set) Token: 0x060030B6 RID: 12470 RVA: 0x000F0741 File Offset: 0x000EEB41
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x17000BD0 RID: 3024
	// (get) Token: 0x060030B7 RID: 12471 RVA: 0x000F074A File Offset: 0x000EEB4A
	public Guid MatchID
	{
		get
		{
			return this.battleServerAPI.MatchID;
		}
	}

	// Token: 0x17000BD1 RID: 3025
	// (get) Token: 0x060030B8 RID: 12472 RVA: 0x000F0757 File Offset: 0x000EEB57
	// (set) Token: 0x060030B9 RID: 12473 RVA: 0x000F075F File Offset: 0x000EEB5F
	public float SelectionEndTime { get; private set; }

	// Token: 0x17000BD2 RID: 3026
	// (get) Token: 0x060030BA RID: 12474 RVA: 0x000F0768 File Offset: 0x000EEB68
	public float PurchaseEndTime
	{
		get
		{
			return this.SelectionEndTime - BattleServerStaging.PURCHASE_BUFFER;
		}
	}

	// Token: 0x17000BD3 RID: 3027
	// (get) Token: 0x060030BB RID: 12475 RVA: 0x000F0776 File Offset: 0x000EEB76
	public int NumMatches
	{
		get
		{
			return this.battleServerAPI.NumMatches;
		}
	}

	// Token: 0x17000BD4 RID: 3028
	// (get) Token: 0x060030BC RID: 12476 RVA: 0x000F0783 File Offset: 0x000EEB83
	public int CurrentMatchIndex
	{
		get
		{
			return this.battleServerAPI.CurrentMatchIndex;
		}
	}

	// Token: 0x17000BD5 RID: 3029
	// (get) Token: 0x060030BD RID: 12477 RVA: 0x000F0790 File Offset: 0x000EEB90
	public List<StageID> Stages
	{
		get
		{
			return this.battleServerAPI.Stages;
		}
	}

	// Token: 0x17000BD6 RID: 3030
	// (get) Token: 0x060030BE RID: 12478 RVA: 0x000F079D File Offset: 0x000EEB9D
	public List<PlayerNum> LocalPlayerNumIds
	{
		get
		{
			return this.battleServerAPI.LocalPlayerNumIDs;
		}
	}

	// Token: 0x17000BD7 RID: 3031
	// (get) Token: 0x060030BF RID: 12479 RVA: 0x000F07AA File Offset: 0x000EEBAA
	public int NumLives
	{
		get
		{
			return this.battleServerAPI.NumLives;
		}
	}

	// Token: 0x17000BD8 RID: 3032
	// (get) Token: 0x060030C0 RID: 12480 RVA: 0x000F07B7 File Offset: 0x000EEBB7
	public int AssistCount
	{
		get
		{
			return this.battleServerAPI.AssistCount;
		}
	}

	// Token: 0x17000BD9 RID: 3033
	// (get) Token: 0x060030C1 RID: 12481 RVA: 0x000F07C4 File Offset: 0x000EEBC4
	public int MatchTime
	{
		get
		{
			return this.battleServerAPI.MatchTime;
		}
	}

	// Token: 0x17000BDA RID: 3034
	// (get) Token: 0x060030C2 RID: 12482 RVA: 0x000F07D1 File Offset: 0x000EEBD1
	public Dictionary<TeamNum, List<SBasicMatchPlayerDesc>> TeamPlayers
	{
		get
		{
			return this.battleServerAPI.TeamPlayers;
		}
	}

	// Token: 0x17000BDB RID: 3035
	// (get) Token: 0x060030C3 RID: 12483 RVA: 0x000F07DE File Offset: 0x000EEBDE
	public GameMode MatchGameMode
	{
		get
		{
			return this.battleServerAPI.MatchGameMode;
		}
	}

	// Token: 0x17000BDC RID: 3036
	// (get) Token: 0x060030C4 RID: 12484 RVA: 0x000F07EB File Offset: 0x000EEBEB
	public GameRules MatchRules
	{
		get
		{
			return this.battleServerAPI.MatchRules;
		}
	}

	// Token: 0x17000BDD RID: 3037
	// (get) Token: 0x060030C5 RID: 12485 RVA: 0x000F07F8 File Offset: 0x000EEBF8
	public bool TeamAttack
	{
		get
		{
			return this.battleServerAPI.TeamAttack;
		}
	}

	// Token: 0x17000BDE RID: 3038
	// (get) Token: 0x060030C6 RID: 12486 RVA: 0x000F0805 File Offset: 0x000EEC05
	// (set) Token: 0x060030C7 RID: 12487 RVA: 0x000F080D File Offset: 0x000EEC0D
	public Action<bool, PlayerNum> OnLockInSelection { get; set; }

	// Token: 0x17000BDF RID: 3039
	// (get) Token: 0x060030C8 RID: 12488 RVA: 0x000F0816 File Offset: 0x000EEC16
	// (set) Token: 0x060030C9 RID: 12489 RVA: 0x000F081E File Offset: 0x000EEC1E
	public Action OnMatchDetailsComplete { get; set; }

	// Token: 0x17000BE0 RID: 3040
	// (get) Token: 0x060030CA RID: 12490 RVA: 0x000F0827 File Offset: 0x000EEC27
	// (set) Token: 0x060030CB RID: 12491 RVA: 0x000F082F File Offset: 0x000EEC2F
	public List<MatchPlayerDetailsData> PlayerDetails { get; private set; }

	// Token: 0x060030CC RID: 12492 RVA: 0x000F0838 File Offset: 0x000EEC38
	public void OnRoomJoined()
	{
		if (this.PlayerDetails == null)
		{
			this.PlayerDetails = new List<MatchPlayerDetailsData>();
		}
		this.battleServerAPI.Listen<LockInSelectionAckEvent>(new ServerEventHandler(this.onLockInSelectionAck));
		this.battleServerAPI.Listen<MatchDetailsEvent>(new ServerEventHandler(this.onMatchDetails));
	}

	// Token: 0x060030CD RID: 12493 RVA: 0x000F0889 File Offset: 0x000EEC89
	public void OnRoomDestroyed()
	{
		this.battleServerAPI.Unsubscribe<LockInSelectionAckEvent>(new ServerEventHandler(this.onLockInSelectionAck));
		this.battleServerAPI.Unsubscribe<MatchDetailsEvent>(new ServerEventHandler(this.onMatchDetails));
	}

	// Token: 0x060030CE RID: 12494 RVA: 0x000F08B9 File Offset: 0x000EECB9
	public void LockInSelection(CharacterID characterID, int characterIndex, int skinID, bool isRandom)
	{
		this.battleServerAPI.LockInSelection(PlayerUtil.GetCharacterTypeFromCharacterID(characterID), (uint)characterIndex, (ulong)skinID, isRandom);
	}

	// Token: 0x060030CF RID: 12495 RVA: 0x000F08D4 File Offset: 0x000EECD4
	private void onLockInSelectionAck(ServerEvent message)
	{
		LockInSelectionAckEvent lockInSelectionAckEvent = message as LockInSelectionAckEvent;
		foreach (PlayerNum arg in this.battleServerAPI.LocalPlayerNumIDs)
		{
			this.OnLockInSelection(lockInSelectionAckEvent.success, arg);
		}
	}

	// Token: 0x060030D0 RID: 12496 RVA: 0x000F0948 File Offset: 0x000EED48
	private void onMatchDetails(ServerEvent message)
	{
		MatchDetailsEvent matchDetailsEvent = message as MatchDetailsEvent;
		this.PlayerDetails.Clear();
		int num = 0;
		foreach (P2PMatchDetailsMsg.SPlayerDesc splayerDesc in matchDetailsEvent.players)
		{
			MatchPlayerDetailsData item = default(MatchPlayerDetailsData);
			item.characterID = PlayerUtil.GetCharacterIDFromCharacterType(splayerDesc.characterSelection);
			item.skinID = (int)splayerDesc.skinId;
			item.playerName = splayerDesc.name;
			item.userID = splayerDesc.userID;
			item.isSpectator = splayerDesc.isSpectator;
			item.playerNum = this.battleServerAPI.GetPlayerNum(num);
			item.characterIndex = (int)splayerDesc.characterIndex;
			item.equippedToCharacter = (from id in splayerDesc.equippedCharacterItemIds
			select new EquipmentID((long)id)).ToList<EquipmentID>();
			item.equippedToPlayer = (from id in splayerDesc.equippedPlayerItemIds
			select new EquipmentID((long)id)).ToList<EquipmentID>();
			this.PlayerDetails.Add(item);
			num++;
		}
		this.OnMatchDetailsComplete();
	}

	// Token: 0x060030D1 RID: 12497 RVA: 0x000F0A7F File Offset: 0x000EEE7F
	public void ResetSelectionTimer()
	{
		this.SelectionEndTime = Time.realtimeSinceStartup + this.battleServerAPI.SelectionTime - 3f;
	}

	// Token: 0x060030D2 RID: 12498 RVA: 0x000F0A9E File Offset: 0x000EEE9E
	public bool ResultsForMatch(int MatchIndex)
	{
		return this.battleServerAPI.ResultsForMatch(MatchIndex);
	}

	// Token: 0x040021EE RID: 8686
	private static float PURCHASE_BUFFER = 10f;
}
