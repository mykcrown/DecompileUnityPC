using System;
using System.Collections.Generic;
using MatchMaking;
using UnityEngine;

// Token: 0x020009B5 RID: 2485
public class OnlineBlindPickScreenAPI : IOnlineBlindPickScreenAPI
{
	// Token: 0x17001047 RID: 4167
	// (get) Token: 0x060044EB RID: 17643 RVA: 0x0012F3D0 File Offset: 0x0012D7D0
	// (set) Token: 0x060044EC RID: 17644 RVA: 0x0012F3D8 File Offset: 0x0012D7D8
	[Inject]
	public ISignalBus signalBus { private get; set; }

	// Token: 0x17001048 RID: 4168
	// (get) Token: 0x060044ED RID: 17645 RVA: 0x0012F3E1 File Offset: 0x0012D7E1
	// (set) Token: 0x060044EE RID: 17646 RVA: 0x0012F3E9 File Offset: 0x0012D7E9
	[Inject]
	public IBattleServerStagingAPI battleServerStagingAPI { private get; set; }

	// Token: 0x17001049 RID: 4169
	// (get) Token: 0x060044EF RID: 17647 RVA: 0x0012F3F2 File Offset: 0x0012D7F2
	// (set) Token: 0x060044F0 RID: 17648 RVA: 0x0012F3FA File Offset: 0x0012D7FA
	[Inject]
	public IBattleServerAPI battleServerAPI { private get; set; }

	// Token: 0x1700104A RID: 4170
	// (get) Token: 0x060044F1 RID: 17649 RVA: 0x0012F403 File Offset: 0x0012D803
	// (set) Token: 0x060044F2 RID: 17650 RVA: 0x0012F40B File Offset: 0x0012D80B
	[Inject]
	public ICharacterSelectModel model { private get; set; }

	// Token: 0x1700104B RID: 4171
	// (get) Token: 0x060044F3 RID: 17651 RVA: 0x0012F414 File Offset: 0x0012D814
	// (set) Token: 0x060044F4 RID: 17652 RVA: 0x0012F41C File Offset: 0x0012D81C
	[Inject]
	public IDialogController dialogController { private get; set; }

	// Token: 0x1700104C RID: 4172
	// (get) Token: 0x060044F5 RID: 17653 RVA: 0x0012F425 File Offset: 0x0012D825
	// (set) Token: 0x060044F6 RID: 17654 RVA: 0x0012F42D File Offset: 0x0012D82D
	[Inject]
	public IEvents events { private get; set; }

	// Token: 0x1700104D RID: 4173
	// (get) Token: 0x060044F7 RID: 17655 RVA: 0x0012F436 File Offset: 0x0012D836
	// (set) Token: 0x060044F8 RID: 17656 RVA: 0x0012F43E File Offset: 0x0012D83E
	[Inject]
	public GameDataManager gameDataManager { private get; set; }

	// Token: 0x1700104E RID: 4174
	// (get) Token: 0x060044F9 RID: 17657 RVA: 0x0012F447 File Offset: 0x0012D847
	// (set) Token: 0x060044FA RID: 17658 RVA: 0x0012F44F File Offset: 0x0012D84F
	[Inject]
	public ICharacterDataHelper characterDataHelper { private get; set; }

	// Token: 0x1700104F RID: 4175
	// (get) Token: 0x060044FB RID: 17659 RVA: 0x0012F458 File Offset: 0x0012D858
	// (set) Token: 0x060044FC RID: 17660 RVA: 0x0012F460 File Offset: 0x0012D860
	[Inject]
	public ICharacterLists characterLists { private get; set; }

	// Token: 0x17001050 RID: 4176
	// (get) Token: 0x060044FD RID: 17661 RVA: 0x0012F469 File Offset: 0x0012D869
	// (set) Token: 0x060044FE RID: 17662 RVA: 0x0012F471 File Offset: 0x0012D871
	[Inject]
	public IUserInputManager userInputManager { private get; set; }

	// Token: 0x17001051 RID: 4177
	// (get) Token: 0x060044FF RID: 17663 RVA: 0x0012F47A File Offset: 0x0012D87A
	// (set) Token: 0x06004500 RID: 17664 RVA: 0x0012F482 File Offset: 0x0012D882
	[Inject]
	public SelectRandomCharacters selectRandomChars { private get; set; }

	// Token: 0x17001052 RID: 4178
	// (get) Token: 0x06004501 RID: 17665 RVA: 0x0012F48B File Offset: 0x0012D88B
	// (set) Token: 0x06004502 RID: 17666 RVA: 0x0012F493 File Offset: 0x0012D893
	[Inject]
	public IEnterNewGame enterNewGame { private get; set; }

	// Token: 0x17001053 RID: 4179
	// (get) Token: 0x06004503 RID: 17667 RVA: 0x0012F49C File Offset: 0x0012D89C
	// (set) Token: 0x06004504 RID: 17668 RVA: 0x0012F4A4 File Offset: 0x0012D8A4
	[Inject]
	public IMainThreadTimer timer { private get; set; }

	// Token: 0x17001054 RID: 4180
	// (get) Token: 0x06004505 RID: 17669 RVA: 0x0012F4AD File Offset: 0x0012D8AD
	// (set) Token: 0x06004506 RID: 17670 RVA: 0x0012F4B5 File Offset: 0x0012D8B5
	[Inject]
	public ICustomLobbyController lobbyController { private get; set; }

	// Token: 0x17001055 RID: 4181
	// (get) Token: 0x06004507 RID: 17671 RVA: 0x0012F4BE File Offset: 0x0012D8BE
	// (set) Token: 0x06004508 RID: 17672 RVA: 0x0012F4C6 File Offset: 0x0012D8C6
	[Inject]
	public IUserCharacterUnlockModel charcterUnlockModel { private get; set; }

	// Token: 0x17001056 RID: 4182
	// (get) Token: 0x06004509 RID: 17673 RVA: 0x0012F4CF File Offset: 0x0012D8CF
	// (set) Token: 0x0600450A RID: 17674 RVA: 0x0012F4D7 File Offset: 0x0012D8D7
	[Inject]
	public IUserInventory userInventory { private get; set; }

	// Token: 0x17001057 RID: 4183
	// (get) Token: 0x0600450B RID: 17675 RVA: 0x0012F4E0 File Offset: 0x0012D8E0
	// (set) Token: 0x0600450C RID: 17676 RVA: 0x0012F4E8 File Offset: 0x0012D8E8
	[Inject]
	public IEquipmentModel equipmentModel { private get; set; }

	// Token: 0x17001058 RID: 4184
	// (get) Token: 0x0600450D RID: 17677 RVA: 0x0012F4F1 File Offset: 0x0012D8F1
	// (set) Token: 0x0600450E RID: 17678 RVA: 0x0012F4F9 File Offset: 0x0012D8F9
	[Inject]
	public AudioManager audioManager { private get; set; }

	// Token: 0x17001059 RID: 4185
	// (get) Token: 0x0600450F RID: 17679 RVA: 0x0012F502 File Offset: 0x0012D902
	// (set) Token: 0x06004510 RID: 17680 RVA: 0x0012F50A File Offset: 0x0012D90A
	[Inject]
	public ISkinDataManager skinDataManager { private get; set; }

	// Token: 0x1700105A RID: 4186
	// (get) Token: 0x06004511 RID: 17681 RVA: 0x0012F513 File Offset: 0x0012D913
	private GameLoadPayload gamePayload
	{
		get
		{
			return this.enterNewGame.GamePayload;
		}
	}

	// Token: 0x1700105B RID: 4187
	// (get) Token: 0x06004512 RID: 17682 RVA: 0x0012F520 File Offset: 0x0012D920
	// (set) Token: 0x06004513 RID: 17683 RVA: 0x0012F528 File Offset: 0x0012D928
	public bool LockedIn
	{
		get
		{
			return this.lockedIn;
		}
		private set
		{
			if (this.lockedIn != value)
			{
				this.lockedIn = value;
				this.signalBus.Dispatch(OnlineBlindPickScreenAPI.UPDATED);
			}
		}
	}

	// Token: 0x1700105C RID: 4188
	// (get) Token: 0x06004514 RID: 17684 RVA: 0x0012F54D File Offset: 0x0012D94D
	// (set) Token: 0x06004515 RID: 17685 RVA: 0x0012F555 File Offset: 0x0012D955
	public bool CanStartGame
	{
		get
		{
			return this.canStartGame;
		}
		private set
		{
			if (this.canStartGame != value)
			{
				this.canStartGame = value;
				this.signalBus.Dispatch(OnlineBlindPickScreenAPI.UPDATED);
			}
		}
	}

	// Token: 0x06004516 RID: 17686 RVA: 0x0012F57C File Offset: 0x0012D97C
	[PostConstruct]
	public void Init()
	{
		for (int i = 0; i < this.lastSelections.Length; i++)
		{
			this.lastSelections[i].characterID = CharacterID.None;
		}
	}

	// Token: 0x06004517 RID: 17687 RVA: 0x0012F5B4 File Offset: 0x0012D9B4
	private void dispatchUpdate()
	{
		this.signalBus.Dispatch(OnlineBlindPickScreenAPI.UPDATED);
	}

	// Token: 0x06004518 RID: 17688 RVA: 0x0012F5C8 File Offset: 0x0012D9C8
	private void Reset()
	{
		this.CanStartGame = false;
		this.LockedIn = false;
		for (PlayerNum playerNum = PlayerNum.Player1; playerNum < PlayerNum.All; playerNum++)
		{
			PlayerSelectionInfo playerSelectionInfo = this.gamePayload.FindPlayerInfo(playerNum);
			if (playerSelectionInfo == null)
			{
				return;
			}
			playerSelectionInfo.type = PlayerType.None;
		}
	}

	// Token: 0x1700105D RID: 4189
	// (get) Token: 0x06004519 RID: 17689 RVA: 0x0012F610 File Offset: 0x0012DA10
	public bool IsSpectator
	{
		get
		{
			return this.lobbyController.IsSpectator;
		}
	}

	// Token: 0x1700105E RID: 4190
	// (get) Token: 0x0600451A RID: 17690 RVA: 0x0012F61D File Offset: 0x0012DA1D
	public ulong MyUserID
	{
		get
		{
			return this.lobbyController.MyUserID;
		}
	}

	// Token: 0x1700105F RID: 4191
	// (get) Token: 0x0600451B RID: 17691 RVA: 0x0012F62A File Offset: 0x0012DA2A
	public bool IsTeams
	{
		get
		{
			return this.lobbyController.IsTeams;
		}
	}

	// Token: 0x0600451C RID: 17692 RVA: 0x0012F638 File Offset: 0x0012DA38
	public void OnScreenShown()
	{
		this.Reset();
		this.battleServerStagingAPI.OnRoomJoined();
		IBattleServerStagingAPI battleServerStagingAPI = this.battleServerStagingAPI;
		battleServerStagingAPI.OnMatchDetailsComplete = (Action)Delegate.Combine(battleServerStagingAPI.OnMatchDetailsComplete, new Action(this.onMatchDetailsComplete));
		this.events.Subscribe(typeof(SelectCharacterRequest), new Events.EventHandler(this.onCharacterSelected));
		this.events.Subscribe(typeof(CyclePlayerSkinRequest), new Events.EventHandler(this.onCyclePlayerSkinCommand));
		this.events.Subscribe(typeof(CycleCharacterIndex), new Events.EventHandler(this.onCycleCharacterIndexCommand));
		IBattleServerStagingAPI battleServerStagingAPI2 = this.battleServerStagingAPI;
		battleServerStagingAPI2.OnLockInSelection = (Action<bool, PlayerNum>)Delegate.Combine(battleServerStagingAPI2.OnLockInSelection, new Action<bool, PlayerNum>(this.onLockInSelectionAck));
		for (int i = 0; i < this.gamePayload.players.Length; i++)
		{
			PlayerSelectionInfo playerSelectionInfo = this.gamePayload.players[i];
			bool isLocal = this.battleServerStagingAPI.LocalPlayerNumIds.Contains(playerSelectionInfo.playerNum);
			playerSelectionInfo.isLocal = isLocal;
		}
		int num = 0;
		foreach (PlayerNum playerNum in this.battleServerStagingAPI.LocalPlayerNumIds)
		{
			this.setPlayerType(playerNum, PlayerType.Human);
			if (num < this.lastSelections.Length)
			{
				OnlineBlindPickScreenAPI.LastSelectionData lastSelectionData = this.lastSelections[num];
				if (lastSelectionData.characterID != CharacterID.None)
				{
					if (lastSelectionData.characterID == CharacterID.Random)
					{
						PlayerSelectionInfo playerSelectionInfo2 = this.gamePayload.FindPlayerInfo(playerNum);
						playerSelectionInfo2.SetCharacter(this.characterLists.GetRandom().characterID, this.skinDataManager, null);
					}
					else
					{
						this.selectCharacterPreLockIn(playerNum, lastSelectionData.characterID, lastSelectionData.characterIndex, lastSelectionData.skinID, false, true);
					}
				}
			}
			num++;
		}
		foreach (KeyValuePair<TeamNum, List<SBasicMatchPlayerDesc>> keyValuePair in this.battleServerStagingAPI.TeamPlayers)
		{
			TeamNum key = keyValuePair.Key;
			List<SBasicMatchPlayerDesc> value = keyValuePair.Value;
			int num2 = 0;
			foreach (SBasicMatchPlayerDesc sbasicMatchPlayerDesc in value)
			{
				PlayerNum playerNum2 = this.battleServerAPI.GetPlayerNum(num2);
				PlayerSelectionInfo playerSelectionInfo3 = this.gamePayload.FindPlayerInfo(playerNum2);
				if (playerSelectionInfo3 != null)
				{
					this.model.SetPlayerProfileName(playerSelectionInfo3, sbasicMatchPlayerDesc.name);
					this.signalBus.Dispatch(OnlineBlindPickScreenAPI.UPDATED);
				}
				num2++;
			}
		}
	}

	// Token: 0x0600451D RID: 17693 RVA: 0x0012F934 File Offset: 0x0012DD34
	public void OnScreenDestroyed()
	{
		IBattleServerStagingAPI battleServerStagingAPI = this.battleServerStagingAPI;
		battleServerStagingAPI.OnLockInSelection = (Action<bool, PlayerNum>)Delegate.Remove(battleServerStagingAPI.OnLockInSelection, new Action<bool, PlayerNum>(this.onLockInSelectionAck));
		this.battleServerStagingAPI.OnRoomDestroyed();
		IBattleServerStagingAPI battleServerStagingAPI2 = this.battleServerStagingAPI;
		battleServerStagingAPI2.OnMatchDetailsComplete = (Action)Delegate.Remove(battleServerStagingAPI2.OnMatchDetailsComplete, new Action(this.onMatchDetailsComplete));
		this.events.Unsubscribe(typeof(SelectCharacterRequest), new Events.EventHandler(this.onCharacterSelected));
		this.events.Unsubscribe(typeof(CyclePlayerSkinRequest), new Events.EventHandler(this.onCyclePlayerSkinCommand));
		this.events.Unsubscribe(typeof(CycleCharacterIndex), new Events.EventHandler(this.onCycleCharacterIndexCommand));
	}

	// Token: 0x0600451E RID: 17694 RVA: 0x0012FA00 File Offset: 0x0012DE00
	private void onCyclePlayerSkinCommand(GameEvent message)
	{
		if (this.LockedIn)
		{
			return;
		}
		CyclePlayerSkinRequest cyclePlayerSkinRequest = message as CyclePlayerSkinRequest;
		this.cycleSkin(cyclePlayerSkinRequest.playerNum, cyclePlayerSkinRequest.direction, true);
	}

	// Token: 0x0600451F RID: 17695 RVA: 0x0012FA34 File Offset: 0x0012DE34
	private void cycleSkin(PlayerNum playerNum, int direction, bool mustOwn = true)
	{
		PlayerSelectionInfo player = this.gamePayload.players.GetPlayer(playerNum);
		if (player == null)
		{
			return;
		}
		bool flag = false;
		if (player.characterID != CharacterID.None)
		{
			SkinSelectMode selectMode = (!mustOwn) ? SkinSelectMode.AnySkin : SkinSelectMode.Online;
			SkinDefinition nextSkin = this.model.GetNextSkin(playerNum, direction, player.characterID, player.skinKey, this.gamePayload.players, selectMode);
			if (nextSkin != null)
			{
				player.skinKey = nextSkin.uniqueKey;
				flag = true;
				if (this.battleServerAPI.IsLocalPlayer(playerNum) && this.lastSelections[0].characterID != CharacterID.Random)
				{
					this.lastSelections[0].skinID = nextSkin.ID;
				}
			}
		}
		else
		{
			CharacterSelectModel.HighlightInfo highlightInfo = this.model.GetHighlightInfo(playerNum);
			if (highlightInfo.characterID != CharacterID.None)
			{
				SkinDefinition nextSkin2 = this.model.GetNextSkin(playerNum, direction, highlightInfo.characterID, highlightInfo.skinIDs[highlightInfo.characterID], this.gamePayload.players, SkinSelectMode.Online);
				if (nextSkin2 != null)
				{
					highlightInfo.skinIDs[highlightInfo.characterID] = nextSkin2.uniqueKey;
					flag = true;
				}
			}
		}
		if (flag)
		{
			this.signalBus.Dispatch(OnlineBlindPickScreenAPI.UPDATED);
		}
	}

	// Token: 0x06004520 RID: 17696 RVA: 0x0012FB88 File Offset: 0x0012DF88
	public void onCycleCharacterIndexCommand(GameEvent message)
	{
		if (this.LockedIn)
		{
			return;
		}
		CycleCharacterIndex cycleCharacterIndex = message as CycleCharacterIndex;
		this.cycleCharacterIndex(cycleCharacterIndex.playerNum);
	}

	// Token: 0x06004521 RID: 17697 RVA: 0x0012FBB4 File Offset: 0x0012DFB4
	private void cycleCharacterIndex(PlayerNum playerNum)
	{
		PlayerSelectionInfo player = this.gamePayload.players.GetPlayer(playerNum);
		if (player == null)
		{
			return;
		}
		CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(player.characterID);
		if (characterDefinition == null || this.characterDataHelper.GetLinkedCharacters(characterDefinition).Length <= 1)
		{
			return;
		}
		this.audioManager.PlayMenuSound(SoundKey.characterSelect_totemPartnerCycle, 0f);
		player.characterIndex = (player.characterIndex + 1) % 2;
		if (this.battleServerAPI.IsLocalPlayer(playerNum) && this.lastSelections[0].characterID != CharacterID.Random)
		{
			this.lastSelections[0].characterIndex = player.characterIndex;
		}
		this.signalBus.Dispatch(OnlineBlindPickScreenAPI.UPDATED);
	}

	// Token: 0x06004522 RID: 17698 RVA: 0x0012FC7E File Offset: 0x0012E07E
	public void LeaveRoom()
	{
		this.events.Broadcast(new DisconnectUpdate(NetErrorCode.ClientAbandoned));
	}

	// Token: 0x17001060 RID: 4192
	// (get) Token: 0x06004523 RID: 17699 RVA: 0x0012FC94 File Offset: 0x0012E094
	public bool CanLockInSelection
	{
		get
		{
			if (this.LockedIn)
			{
				return false;
			}
			int i = 0;
			while (i < this.gamePayload.players.Length)
			{
				PlayerSelectionInfo playerSelectionInfo = this.gamePayload.players[i];
				if (playerSelectionInfo.playerNum == this.battleServerAPI.GetPrimaryLocalPlayer)
				{
					if (playerSelectionInfo.characterID == CharacterID.None)
					{
						return false;
					}
					if (!this.charcterUnlockModel.IsUnlocked(playerSelectionInfo.characterID))
					{
						return false;
					}
					if (!this.characterLists.GetCharacterDefinition(playerSelectionInfo.characterID).isRandom)
					{
						SkinDefinition skinDefinition = this.characterDataHelper.GetSkinDefinition(playerSelectionInfo.characterID, playerSelectionInfo.skinKey);
						if (!this.model.IsSkinLegal(playerSelectionInfo.characterID, skinDefinition, SkinSelectMode.Online))
						{
							return false;
						}
					}
					return true;
				}
				else
				{
					i++;
				}
			}
			return false;
		}
	}

	// Token: 0x06004524 RID: 17700 RVA: 0x0012FD6C File Offset: 0x0012E16C
	private void onLockInSelectionAck(bool success, PlayerNum playerNum)
	{
		this.LockedIn = success;
		if (!this.LockedIn)
		{
			this.selectCharacterPreLockIn(playerNum, CharacterID.None, 0, -1, true, true);
		}
		this.signalBus.Dispatch(OnlineBlindPickScreenAPI.UPDATED);
	}

	// Token: 0x06004525 RID: 17701 RVA: 0x0012FDA0 File Offset: 0x0012E1A0
	private void onMatchPlayerDetails(MatchPlayerDetailsData playerDetails)
	{
		if (this.userInputManager.GetPortWithPlayerNum(playerDetails.playerNum) == null)
		{
			this.userInputManager.ForceBindAvailablePortToPlayerNoUser(playerDetails.playerNum);
		}
		int localSkinId = -1;
		if (!playerDetails.isSpectator)
		{
			this.setPlayerType(playerDetails.playerNum, PlayerType.Human);
			SkinDefinition skinDefinition = this.equipmentModel.GetSkinFromItem(new EquipmentID((long)playerDetails.skinID));
			if (skinDefinition == null)
			{
				skinDefinition = this.skinDataManager.GetDefaultSkin(playerDetails.characterID);
			}
			if (skinDefinition != null)
			{
				localSkinId = skinDefinition.ID;
			}
		}
		else
		{
			this.setPlayerType(playerDetails.playerNum, PlayerType.Spectator);
		}
		this.syncServerCharacterSelectionState(playerDetails, localSkinId);
	}

	// Token: 0x06004526 RID: 17702 RVA: 0x0012FE60 File Offset: 0x0012E260
	private void syncServerCharacterSelectionState(MatchPlayerDetailsData playerDetails, int localSkinId)
	{
		PlayerSelectionInfo playerSelectionInfo = this.gamePayload.FindPlayerInfo(playerDetails.playerNum);
		if (playerSelectionInfo == null)
		{
			return;
		}
		if (!playerDetails.isSpectator)
		{
			SkinDefinition skin;
			if (localSkinId == -1)
			{
				skin = this.skinDataManager.GetDefaultSkin(playerDetails.characterID);
			}
			else
			{
				skin = this.characterDataHelper.GetSkinDefinition(playerDetails.characterID, localSkinId);
			}
			playerSelectionInfo.SetCharacter(playerDetails.characterID, this.skinDataManager, skin);
			playerSelectionInfo.characterIndex = playerDetails.characterIndex;
			playerSelectionInfo.SetEquipment(playerDetails.equippedToPlayer, playerDetails.equippedToCharacter);
			playerSelectionInfo.curProfile.profileName = playerDetails.playerName.ToUpper();
		}
		playerSelectionInfo.isSpectator = playerDetails.isSpectator;
		playerSelectionInfo.userID = playerDetails.userID;
		Debug.Log(string.Concat(new object[]
		{
			"Sync state ",
			playerSelectionInfo.userID,
			" ",
			playerDetails.playerNum
		}));
		this.signalBus.Dispatch(OnlineBlindPickScreenAPI.UPDATED);
	}

	// Token: 0x06004527 RID: 17703 RVA: 0x0012FF7B File Offset: 0x0012E37B
	private void forceUniqueSkins()
	{
	}

	// Token: 0x06004528 RID: 17704 RVA: 0x0012FF7D File Offset: 0x0012E37D
	private void onMatchDetailsComplete()
	{
		this.timer.SetTimeout(500, new Action(this.onTimerComplete));
	}

	// Token: 0x06004529 RID: 17705 RVA: 0x0012FF9C File Offset: 0x0012E39C
	private void onTimerComplete()
	{
		List<MatchPlayerDetailsData> playerDetails = this.battleServerStagingAPI.PlayerDetails;
		foreach (MatchPlayerDetailsData playerDetails2 in playerDetails)
		{
			this.onMatchPlayerDetails(playerDetails2);
		}
		this.forceUniqueSkins();
		this.CanStartGame = true;
	}

	// Token: 0x0600452A RID: 17706 RVA: 0x0013000C File Offset: 0x0012E40C
	public void LockInSelection()
	{
		if (!this.CanLockInSelection)
		{
			return;
		}
		for (int i = 0; i < this.gamePayload.players.Length; i++)
		{
			PlayerSelectionInfo playerSelectionInfo = this.gamePayload.players[i];
			if (playerSelectionInfo.playerNum == this.battleServerAPI.GetPrimaryLocalPlayer)
			{
				CharacterID characterID = playerSelectionInfo.characterID;
				int characterIndex = playerSelectionInfo.characterIndex;
				bool flag = characterID == CharacterID.Random;
				SkinDefinition skinDefinition;
				if (flag)
				{
					CharacterDefinition randomCharacter = this.selectRandomChars.GetRandomCharacter(this.gamePayload.battleConfig.mode);
					characterID = randomCharacter.characterID;
					CharacterDefinition[] linkedCharacters = this.characterDataHelper.GetLinkedCharacters(randomCharacter);
					characterIndex = ((linkedCharacters == null || linkedCharacters.Length <= 1) ? 0 : UnityEngine.Random.Range(0, linkedCharacters.Length));
					skinDefinition = this.selectRandomChars.GetRandomSkinForCharacter(this.gamePayload, characterID, playerSelectionInfo.playerNum, SkinSelectMode.Online);
				}
				else
				{
					skinDefinition = this.characterDataHelper.GetSkinDefinition(playerSelectionInfo.characterID, playerSelectionInfo.skinKey);
				}
				this.LockedIn = true;
				EquippableItem itemFromSkinKey = this.equipmentModel.GetItemFromSkinKey(skinDefinition.uniqueKey);
				int skinID = (itemFromSkinKey == null || itemFromSkinKey.isDefault || itemFromSkinKey.id.IsNull()) ? 0 : ((int)itemFromSkinKey.id.id);
				this.battleServerStagingAPI.LockInSelection(characterID, characterIndex, skinID, flag);
				break;
			}
		}
	}

	// Token: 0x0600452B RID: 17707 RVA: 0x00130188 File Offset: 0x0012E588
	private void onCharacterSelected(GameEvent message)
	{
		if (this.LockedIn)
		{
			return;
		}
		SelectCharacterRequest selectCharacterRequest = message as SelectCharacterRequest;
		if (selectCharacterRequest.useDefaultCharacter)
		{
			this.attemptSelectCharacter(selectCharacterRequest.playerNum, this.characterLists.GetRandom().characterID, -1);
		}
		else
		{
			this.attemptSelectCharacter(selectCharacterRequest.playerNum, selectCharacterRequest.characterID, -1);
		}
	}

	// Token: 0x0600452C RID: 17708 RVA: 0x001301E8 File Offset: 0x0012E5E8
	private void attemptSelectCharacter(PlayerNum playerNum, CharacterID characterID, int skinID)
	{
		if (this.selectCharacterPreLockIn(playerNum, characterID, 0, skinID, true, true) && this.battleServerAPI.IsLocalPlayer(playerNum))
		{
			PlayerSelectionInfo playerSelectionInfo = this.gamePayload.FindPlayerInfo(playerNum);
			this.lastSelections[0].characterID = playerSelectionInfo.characterID;
			if (this.lastSelections[0].characterID != CharacterID.Random)
			{
				this.lastSelections[0].characterIndex = playerSelectionInfo.characterIndex;
				this.lastSelections[0].skinID = skinID;
			}
		}
	}

	// Token: 0x0600452D RID: 17709 RVA: 0x0013027C File Offset: 0x0012E67C
	private bool selectCharacterPreLockIn(PlayerNum playerNum, CharacterID characterID, int characterIndex, int skinID, bool setRandom, bool requireOwnership = true)
	{
		PlayerSelectionInfo playerSelectionInfo = this.gamePayload.FindPlayerInfo(playerNum);
		if (playerSelectionInfo == null)
		{
			return false;
		}
		CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(characterID);
		SkinDefinition skinDefinition = null;
		if (skinID == -1)
		{
			CharacterSelectModel.HighlightInfo highlightInfo = this.model.GetHighlightInfo(playerNum);
			string skinKey = highlightInfo.GetSkinKey(characterID);
			if (skinKey != null)
			{
				skinDefinition = this.characterDataHelper.GetSkinDefinition(characterID, skinKey);
			}
		}
		else
		{
			skinDefinition = this.characterDataHelper.GetSkinDefinition(characterID, skinID);
		}
		if (requireOwnership && !this.model.IsSkinAvailable(characterID, skinDefinition, playerNum, this.gamePayload.players, SkinSelectMode.Online))
		{
			skinDefinition = null;
		}
		if (skinDefinition == null)
		{
			skinDefinition = this.model.GetAvailableEquippedOrDefaultSkin(characterDefinition, playerSelectionInfo.playerNum, this.gamePayload.players, SkinSelectMode.Online);
		}
		playerSelectionInfo.SetCharacter(characterID, this.skinDataManager, skinDefinition);
		if (setRandom)
		{
			playerSelectionInfo.isRandom = (characterID != CharacterID.None && characterDefinition.isRandom);
		}
		playerSelectionInfo.characterIndex = characterIndex;
		this.signalBus.Dispatch(OnlineBlindPickScreenAPI.UPDATED);
		return true;
	}

	// Token: 0x0600452E RID: 17710 RVA: 0x00130390 File Offset: 0x0012E790
	private void setPlayerType(PlayerNum playerNum, PlayerType playerType)
	{
		PlayerSelectionInfo playerSelectionInfo = this.gamePayload.FindPlayerInfo(playerNum);
		if (playerSelectionInfo == null)
		{
			return;
		}
		playerSelectionInfo.type = playerType;
		this.signalBus.Dispatch(OnlineBlindPickScreenAPI.UPDATED);
	}

	// Token: 0x0600452F RID: 17711 RVA: 0x001303C8 File Offset: 0x0012E7C8
	public Dictionary<ulong, LobbyPlayerData> GetLobbyPlayers()
	{
		return this.lobbyController.Players;
	}

	// Token: 0x04002DE1 RID: 11745
	public static string UPDATED = "OnlineBlindPickScreenAPI.UPDATED";

	// Token: 0x04002DF5 RID: 11765
	private OnlineBlindPickScreenAPI.LastSelectionData[] lastSelections = new OnlineBlindPickScreenAPI.LastSelectionData[3];

	// Token: 0x04002DF6 RID: 11766
	private bool lockedIn;

	// Token: 0x04002DF7 RID: 11767
	private bool canStartGame;

	// Token: 0x020009B6 RID: 2486
	private struct LastSelectionData
	{
		// Token: 0x04002DF8 RID: 11768
		public CharacterID characterID;

		// Token: 0x04002DF9 RID: 11769
		public int skinID;

		// Token: 0x04002DFA RID: 11770
		public int characterIndex;
	}
}
