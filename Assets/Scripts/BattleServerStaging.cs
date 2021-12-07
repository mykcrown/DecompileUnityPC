// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using MatchMaking;
using P2P;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleServerStaging : IBattleServerStagingAPI
{
	private static float PURCHASE_BUFFER = 10f;

	private static Func<ushort, EquipmentID> __f__am_cache0;

	private static Func<ushort, EquipmentID> __f__am_cache1;

	[Inject]
	public IBattleServerAPI battleServerAPI
	{
		get;
		set;
	}

	[Inject]
	public IEquipmentModel equipmentModel
	{
		get;
		set;
	}

	public Guid MatchID
	{
		get
		{
			return this.battleServerAPI.MatchID;
		}
	}

	public float SelectionEndTime
	{
		get;
		private set;
	}

	public float PurchaseEndTime
	{
		get
		{
			return this.SelectionEndTime - BattleServerStaging.PURCHASE_BUFFER;
		}
	}

	public int NumMatches
	{
		get
		{
			return this.battleServerAPI.NumMatches;
		}
	}

	public int CurrentMatchIndex
	{
		get
		{
			return this.battleServerAPI.CurrentMatchIndex;
		}
	}

	public List<StageID> Stages
	{
		get
		{
			return this.battleServerAPI.Stages;
		}
	}

	public List<PlayerNum> LocalPlayerNumIds
	{
		get
		{
			return this.battleServerAPI.LocalPlayerNumIDs;
		}
	}

	public int NumLives
	{
		get
		{
			return this.battleServerAPI.NumLives;
		}
	}

	public int AssistCount
	{
		get
		{
			return this.battleServerAPI.AssistCount;
		}
	}

	public int MatchTime
	{
		get
		{
			return this.battleServerAPI.MatchTime;
		}
	}

	public Dictionary<TeamNum, List<SBasicMatchPlayerDesc>> TeamPlayers
	{
		get
		{
			return this.battleServerAPI.TeamPlayers;
		}
	}

	public GameMode MatchGameMode
	{
		get
		{
			return this.battleServerAPI.MatchGameMode;
		}
	}

	public GameRules MatchRules
	{
		get
		{
			return this.battleServerAPI.MatchRules;
		}
	}

	public bool TeamAttack
	{
		get
		{
			return this.battleServerAPI.TeamAttack;
		}
	}

	public Action<bool, PlayerNum> OnLockInSelection
	{
		get;
		set;
	}

	public Action OnMatchDetailsComplete
	{
		get;
		set;
	}

	public List<MatchPlayerDetailsData> PlayerDetails
	{
		get;
		private set;
	}

	public void OnRoomJoined()
	{
		if (this.PlayerDetails == null)
		{
			this.PlayerDetails = new List<MatchPlayerDetailsData>();
		}
		this.battleServerAPI.Listen<LockInSelectionAckEvent>(new ServerEventHandler(this.onLockInSelectionAck));
		this.battleServerAPI.Listen<MatchDetailsEvent>(new ServerEventHandler(this.onMatchDetails));
	}

	public void OnRoomDestroyed()
	{
		this.battleServerAPI.Unsubscribe<LockInSelectionAckEvent>(new ServerEventHandler(this.onLockInSelectionAck));
		this.battleServerAPI.Unsubscribe<MatchDetailsEvent>(new ServerEventHandler(this.onMatchDetails));
	}

	public void LockInSelection(CharacterID characterID, int characterIndex, int skinID, bool isRandom)
	{
		this.battleServerAPI.LockInSelection(PlayerUtil.GetCharacterTypeFromCharacterID(characterID), (uint)characterIndex, (ulong)skinID, isRandom);
	}

	private void onLockInSelectionAck(ServerEvent message)
	{
		LockInSelectionAckEvent lockInSelectionAckEvent = message as LockInSelectionAckEvent;
		foreach (PlayerNum current in this.battleServerAPI.LocalPlayerNumIDs)
		{
			this.OnLockInSelection(lockInSelectionAckEvent.success, current);
		}
	}

	private void onMatchDetails(ServerEvent message)
	{
		MatchDetailsEvent matchDetailsEvent = message as MatchDetailsEvent;
		this.PlayerDetails.Clear();
		int num = 0;
		P2PMatchDetailsMsg.SPlayerDesc[] players = matchDetailsEvent.players;
		for (int i = 0; i < players.Length; i++)
		{
			P2PMatchDetailsMsg.SPlayerDesc sPlayerDesc = players[i];
			MatchPlayerDetailsData item = default(MatchPlayerDetailsData);
			item.characterID = PlayerUtil.GetCharacterIDFromCharacterType(sPlayerDesc.characterSelection);
			item.skinID = (int)sPlayerDesc.skinId;
			item.playerName = sPlayerDesc.name;
			item.userID = sPlayerDesc.userID;
			item.isSpectator = sPlayerDesc.isSpectator;
			item.playerNum = this.battleServerAPI.GetPlayerNum(num);
			item.characterIndex = (int)sPlayerDesc.characterIndex;
			IEnumerable<ushort> arg_BB_0 = sPlayerDesc.equippedCharacterItemIds;
			if (BattleServerStaging.__f__am_cache0 == null)
			{
				BattleServerStaging.__f__am_cache0 = new Func<ushort, EquipmentID>(BattleServerStaging._onMatchDetails_m__0);
			}
			item.equippedToCharacter = arg_BB_0.Select(BattleServerStaging.__f__am_cache0).ToList<EquipmentID>();
			IEnumerable<ushort> arg_EF_0 = sPlayerDesc.equippedPlayerItemIds;
			if (BattleServerStaging.__f__am_cache1 == null)
			{
				BattleServerStaging.__f__am_cache1 = new Func<ushort, EquipmentID>(BattleServerStaging._onMatchDetails_m__1);
			}
			item.equippedToPlayer = arg_EF_0.Select(BattleServerStaging.__f__am_cache1).ToList<EquipmentID>();
			this.PlayerDetails.Add(item);
			num++;
		}
		this.OnMatchDetailsComplete();
	}

	public void ResetSelectionTimer()
	{
		this.SelectionEndTime = Time.realtimeSinceStartup + this.battleServerAPI.SelectionTime - 3f;
	}

	public bool ResultsForMatch(int MatchIndex)
	{
		return this.battleServerAPI.ResultsForMatch(MatchIndex);
	}

	private static EquipmentID _onMatchDetails_m__0(ushort id)
	{
		return new EquipmentID((long)id);
	}

	private static EquipmentID _onMatchDetails_m__1(ushort id)
	{
		return new EquipmentID((long)id);
	}
}
