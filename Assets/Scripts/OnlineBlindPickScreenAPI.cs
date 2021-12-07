// Decompile from assembly: Assembly-CSharp.dll

using MatchMaking;
using System;
using System.Collections.Generic;
using UnityEngine;

public class OnlineBlindPickScreenAPI : IOnlineBlindPickScreenAPI
{
	private struct LastSelectionData
	{
		public CharacterID characterID;

		public int skinID;

		public int characterIndex;
	}

	public static string UPDATED = "OnlineBlindPickScreenAPI.UPDATED";

	private OnlineBlindPickScreenAPI.LastSelectionData[] lastSelections = new OnlineBlindPickScreenAPI.LastSelectionData[3];

	private bool lockedIn;

	private bool canStartGame;

	[Inject]
	public ISignalBus signalBus
	{
		private get;
		set;
	}

	[Inject]
	public IBattleServerStagingAPI battleServerStagingAPI
	{
		private get;
		set;
	}

	[Inject]
	public IBattleServerAPI battleServerAPI
	{
		private get;
		set;
	}

	[Inject]
	public ICharacterSelectModel model
	{
		private get;
		set;
	}

	[Inject]
	public IDialogController dialogController
	{
		private get;
		set;
	}

	[Inject]
	public IEvents events
	{
		private get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
	{
		private get;
		set;
	}

	[Inject]
	public ICharacterDataHelper characterDataHelper
	{
		private get;
		set;
	}

	[Inject]
	public ICharacterLists characterLists
	{
		private get;
		set;
	}

	[Inject]
	public IUserInputManager userInputManager
	{
		private get;
		set;
	}

	[Inject]
	public SelectRandomCharacters selectRandomChars
	{
		private get;
		set;
	}

	[Inject]
	public IEnterNewGame enterNewGame
	{
		private get;
		set;
	}

	[Inject]
	public IMainThreadTimer timer
	{
		private get;
		set;
	}

	[Inject]
	public ICustomLobbyController lobbyController
	{
		private get;
		set;
	}

	[Inject]
	public IUserCharacterUnlockModel charcterUnlockModel
	{
		private get;
		set;
	}

	[Inject]
	public IUserInventory userInventory
	{
		private get;
		set;
	}

	[Inject]
	public IEquipmentModel equipmentModel
	{
		private get;
		set;
	}

	[Inject]
	public AudioManager audioManager
	{
		private get;
		set;
	}

	[Inject]
	public ISkinDataManager skinDataManager
	{
		private get;
		set;
	}

	private GameLoadPayload gamePayload
	{
		get
		{
			return this.enterNewGame.GamePayload;
		}
	}

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

	public bool IsSpectator
	{
		get
		{
			return this.lobbyController.IsSpectator;
		}
	}

	public ulong MyUserID
	{
		get
		{
			return this.lobbyController.MyUserID;
		}
	}

	public bool IsTeams
	{
		get
		{
			return this.lobbyController.IsTeams;
		}
	}

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

	[PostConstruct]
	public void Init()
	{
		for (int i = 0; i < this.lastSelections.Length; i++)
		{
			this.lastSelections[i].characterID = CharacterID.None;
		}
	}

	private void dispatchUpdate()
	{
		this.signalBus.Dispatch(OnlineBlindPickScreenAPI.UPDATED);
	}

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

	public void OnScreenShown()
	{
		this.Reset();
		this.battleServerStagingAPI.OnRoomJoined();
		IBattleServerStagingAPI expr_17 = this.battleServerStagingAPI;
		expr_17.OnMatchDetailsComplete = (Action)Delegate.Combine(expr_17.OnMatchDetailsComplete, new Action(this.onMatchDetailsComplete));
		this.events.Subscribe(typeof(SelectCharacterRequest), new Events.EventHandler(this.onCharacterSelected));
		this.events.Subscribe(typeof(CyclePlayerSkinRequest), new Events.EventHandler(this.onCyclePlayerSkinCommand));
		this.events.Subscribe(typeof(CycleCharacterIndex), new Events.EventHandler(this.onCycleCharacterIndexCommand));
		IBattleServerStagingAPI expr_A1 = this.battleServerStagingAPI;
		expr_A1.OnLockInSelection = (Action<bool, PlayerNum>)Delegate.Combine(expr_A1.OnLockInSelection, new Action<bool, PlayerNum>(this.onLockInSelectionAck));
		for (int i = 0; i < this.gamePayload.players.Length; i++)
		{
			PlayerSelectionInfo playerSelectionInfo = this.gamePayload.players[i];
			bool isLocal = this.battleServerStagingAPI.LocalPlayerNumIds.Contains(playerSelectionInfo.playerNum);
			playerSelectionInfo.isLocal = isLocal;
		}
		int num = 0;
		foreach (PlayerNum current in this.battleServerStagingAPI.LocalPlayerNumIds)
		{
			this.setPlayerType(current, PlayerType.Human);
			if (num < this.lastSelections.Length)
			{
				OnlineBlindPickScreenAPI.LastSelectionData lastSelectionData = this.lastSelections[num];
				if (lastSelectionData.characterID != CharacterID.None)
				{
					if (lastSelectionData.characterID == CharacterID.Random)
					{
						PlayerSelectionInfo playerSelectionInfo2 = this.gamePayload.FindPlayerInfo(current);
						playerSelectionInfo2.SetCharacter(this.characterLists.GetRandom().characterID, this.skinDataManager, null);
					}
					else
					{
						this.selectCharacterPreLockIn(current, lastSelectionData.characterID, lastSelectionData.characterIndex, lastSelectionData.skinID, false, true);
					}
				}
			}
			num++;
		}
		foreach (KeyValuePair<TeamNum, List<SBasicMatchPlayerDesc>> current2 in this.battleServerStagingAPI.TeamPlayers)
		{
			TeamNum key = current2.Key;
			List<SBasicMatchPlayerDesc> value = current2.Value;
			int num2 = 0;
			foreach (SBasicMatchPlayerDesc current3 in value)
			{
				PlayerNum playerNum = this.battleServerAPI.GetPlayerNum(num2);
				PlayerSelectionInfo playerSelectionInfo3 = this.gamePayload.FindPlayerInfo(playerNum);
				if (playerSelectionInfo3 != null)
				{
					this.model.SetPlayerProfileName(playerSelectionInfo3, current3.name);
					this.signalBus.Dispatch(OnlineBlindPickScreenAPI.UPDATED);
				}
				num2++;
			}
		}
	}

	public void OnScreenDestroyed()
	{
		IBattleServerStagingAPI expr_06 = this.battleServerStagingAPI;
		expr_06.OnLockInSelection = (Action<bool, PlayerNum>)Delegate.Remove(expr_06.OnLockInSelection, new Action<bool, PlayerNum>(this.onLockInSelectionAck));
		this.battleServerStagingAPI.OnRoomDestroyed();
		IBattleServerStagingAPI expr_38 = this.battleServerStagingAPI;
		expr_38.OnMatchDetailsComplete = (Action)Delegate.Remove(expr_38.OnMatchDetailsComplete, new Action(this.onMatchDetailsComplete));
		this.events.Unsubscribe(typeof(SelectCharacterRequest), new Events.EventHandler(this.onCharacterSelected));
		this.events.Unsubscribe(typeof(CyclePlayerSkinRequest), new Events.EventHandler(this.onCyclePlayerSkinCommand));
		this.events.Unsubscribe(typeof(CycleCharacterIndex), new Events.EventHandler(this.onCycleCharacterIndexCommand));
	}

	private void onCyclePlayerSkinCommand(GameEvent message)
	{
		if (this.LockedIn)
		{
			return;
		}
		CyclePlayerSkinRequest cyclePlayerSkinRequest = message as CyclePlayerSkinRequest;
		this.cycleSkin(cyclePlayerSkinRequest.playerNum, cyclePlayerSkinRequest.direction, true);
	}

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

	public void onCycleCharacterIndexCommand(GameEvent message)
	{
		if (this.LockedIn)
		{
			return;
		}
		CycleCharacterIndex cycleCharacterIndex = message as CycleCharacterIndex;
		this.cycleCharacterIndex(cycleCharacterIndex.playerNum);
	}

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

	public void LeaveRoom()
	{
		this.events.Broadcast(new DisconnectUpdate(NetErrorCode.ClientAbandoned));
	}

	private void onLockInSelectionAck(bool success, PlayerNum playerNum)
	{
		this.LockedIn = success;
		if (!this.LockedIn)
		{
			this.selectCharacterPreLockIn(playerNum, CharacterID.None, 0, -1, true, true);
		}
		this.signalBus.Dispatch(OnlineBlindPickScreenAPI.UPDATED);
	}

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
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Sync state ",
			playerSelectionInfo.userID,
			" ",
			playerDetails.playerNum
		}));
		this.signalBus.Dispatch(OnlineBlindPickScreenAPI.UPDATED);
	}

	private void forceUniqueSkins()
	{
	}

	private void onMatchDetailsComplete()
	{
		this.timer.SetTimeout(500, new Action(this.onTimerComplete));
	}

	private void onTimerComplete()
	{
		List<MatchPlayerDetailsData> playerDetails = this.battleServerStagingAPI.PlayerDetails;
		foreach (MatchPlayerDetailsData current in playerDetails)
		{
			this.onMatchPlayerDetails(current);
		}
		this.forceUniqueSkins();
		this.CanStartGame = true;
	}

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

	public Dictionary<ulong, LobbyPlayerData> GetLobbyPlayers()
	{
		return this.lobbyController.Players;
	}
}
