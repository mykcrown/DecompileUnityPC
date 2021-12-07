using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020008D9 RID: 2265
public class CharacterSelectModel : ICharacterSelectModel, IDataDependency
{
	// Token: 0x17000DCD RID: 3533
	// (get) Token: 0x0600394C RID: 14668 RVA: 0x0010C9CE File Offset: 0x0010ADCE
	// (set) Token: 0x0600394D RID: 14669 RVA: 0x0010C9D6 File Offset: 0x0010ADD6
	[Inject]
	public IDevConsole devConsole { get; set; }

	// Token: 0x17000DCE RID: 3534
	// (get) Token: 0x0600394E RID: 14670 RVA: 0x0010C9DF File Offset: 0x0010ADDF
	// (set) Token: 0x0600394F RID: 14671 RVA: 0x0010C9E7 File Offset: 0x0010ADE7
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x17000DCF RID: 3535
	// (get) Token: 0x06003950 RID: 14672 RVA: 0x0010C9F0 File Offset: 0x0010ADF0
	// (set) Token: 0x06003951 RID: 14673 RVA: 0x0010C9F8 File Offset: 0x0010ADF8
	[Inject]
	public IUserCharacterEquippedModel userCharacterEquippedModel { get; set; }

	// Token: 0x17000DD0 RID: 3536
	// (get) Token: 0x06003952 RID: 14674 RVA: 0x0010CA01 File Offset: 0x0010AE01
	// (set) Token: 0x06003953 RID: 14675 RVA: 0x0010CA09 File Offset: 0x0010AE09
	[Inject]
	public IBattleServerAPI battleServerAPI { get; set; }

	// Token: 0x17000DD1 RID: 3537
	// (get) Token: 0x06003954 RID: 14676 RVA: 0x0010CA12 File Offset: 0x0010AE12
	// (set) Token: 0x06003955 RID: 14677 RVA: 0x0010CA1A File Offset: 0x0010AE1A
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x17000DD2 RID: 3538
	// (get) Token: 0x06003956 RID: 14678 RVA: 0x0010CA23 File Offset: 0x0010AE23
	// (set) Token: 0x06003957 RID: 14679 RVA: 0x0010CA2B File Offset: 0x0010AE2B
	[Inject]
	public PlayerInputLocator playerInput { get; set; }

	// Token: 0x17000DD3 RID: 3539
	// (get) Token: 0x06003958 RID: 14680 RVA: 0x0010CA34 File Offset: 0x0010AE34
	// (set) Token: 0x06003959 RID: 14681 RVA: 0x0010CA3C File Offset: 0x0010AE3C
	[Inject]
	public ITokenManager tokenManager { get; set; }

	// Token: 0x17000DD4 RID: 3540
	// (get) Token: 0x0600395A RID: 14682 RVA: 0x0010CA45 File Offset: 0x0010AE45
	// (set) Token: 0x0600395B RID: 14683 RVA: 0x0010CA4D File Offset: 0x0010AE4D
	[Inject]
	public IUserCharacterUnlockModel characterUnlockModel { get; set; }

	// Token: 0x17000DD5 RID: 3541
	// (get) Token: 0x0600395C RID: 14684 RVA: 0x0010CA56 File Offset: 0x0010AE56
	// (set) Token: 0x0600395D RID: 14685 RVA: 0x0010CA5E File Offset: 0x0010AE5E
	[Inject]
	public IUserInventory userInventory { get; set; }

	// Token: 0x17000DD6 RID: 3542
	// (get) Token: 0x0600395E RID: 14686 RVA: 0x0010CA67 File Offset: 0x0010AE67
	// (set) Token: 0x0600395F RID: 14687 RVA: 0x0010CA6F File Offset: 0x0010AE6F
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17000DD7 RID: 3543
	// (get) Token: 0x06003960 RID: 14688 RVA: 0x0010CA78 File Offset: 0x0010AE78
	// (set) Token: 0x06003961 RID: 14689 RVA: 0x0010CA80 File Offset: 0x0010AE80
	[Inject]
	public ICharacterDataHelper characterDataHelper { get; set; }

	// Token: 0x17000DD8 RID: 3544
	// (get) Token: 0x06003962 RID: 14690 RVA: 0x0010CA89 File Offset: 0x0010AE89
	// (set) Token: 0x06003963 RID: 14691 RVA: 0x0010CA91 File Offset: 0x0010AE91
	[Inject]
	public ICharacterLists characterLists { get; set; }

	// Token: 0x17000DD9 RID: 3545
	// (get) Token: 0x06003964 RID: 14692 RVA: 0x0010CA9A File Offset: 0x0010AE9A
	// (set) Token: 0x06003965 RID: 14693 RVA: 0x0010CAA2 File Offset: 0x0010AEA2
	[Inject]
	public ISkinDataManager skinDataManager { private get; set; }

	// Token: 0x06003966 RID: 14694 RVA: 0x0010CAAC File Offset: 0x0010AEAC
	public void Load(Action<DataLoadResult> callback)
	{
		this.devConsole.AddAdminCommand(new Action(this.toggleAlwaysUnlocked), "toggleAlwaysUnlockedCharacters", null);
		callback(new DataLoadResult
		{
			status = DataLoadStatus.SUCCESS
		});
	}

	// Token: 0x06003967 RID: 14695 RVA: 0x0010CAEA File Offset: 0x0010AEEA
	private void toggleAlwaysUnlocked()
	{
		this.alwaysUnlockedEnabled = !this.alwaysUnlockedEnabled;
		this.devConsole.PrintLn("'Always Unlocked' character data is " + ((!this.alwaysUnlockedEnabled) ? "DISABLED" : "ENABLED"));
	}

	// Token: 0x06003968 RID: 14696 RVA: 0x0010CB2A File Offset: 0x0010AF2A
	public bool IsCharacterUnlocked(CharacterID characterId)
	{
		return this.characterUnlockModel.IsUnlocked(characterId);
	}

	// Token: 0x06003969 RID: 14697 RVA: 0x0010CB38 File Offset: 0x0010AF38
	public SkinDefinition GetDefaultSkin(CharacterID characterId, int portId)
	{
		return this.userCharacterEquippedModel.GetEquippedSkin(characterId, portId);
	}

	// Token: 0x0600396A RID: 14698 RVA: 0x0010CB47 File Offset: 0x0010AF47
	public CharacterSelectModel.HighlightInfo GetHighlightInfo(PlayerNum playerNum)
	{
		if (!this.highlightInfo.ContainsKey(playerNum))
		{
			this.highlightInfo[playerNum] = new CharacterSelectModel.HighlightInfo();
		}
		return this.highlightInfo[playerNum];
	}

	// Token: 0x0600396B RID: 14699 RVA: 0x0010CB78 File Offset: 0x0010AF78
	public PlayerNum GetActingPlayer(int pointerId)
	{
		PlayerNum playerNumFromPointer = PlayerUtil.GetPlayerNumFromPointer(pointerId);
		PlayerNum result;
		if (playerNumFromPointer == PlayerNum.All)
		{
			if (this.battleServerAPI.IsConnected)
			{
				result = this.battleServerAPI.GetPrimaryLocalPlayer;
			}
			else
			{
				PlayerNum playerNum = this.userInputManager.GetPlayerNum(this.playerInput.Input.GetKeyboard());
				if (playerNum == PlayerNum.None)
				{
					result = PlayerNum.Player1;
				}
				else
				{
					result = playerNum;
				}
			}
		}
		else
		{
			result = playerNumFromPointer;
		}
		PlayerToken currentlyGrabbing = this.tokenManager.GetCurrentlyGrabbing(playerNumFromPointer);
		if (currentlyGrabbing != null)
		{
			result = currentlyGrabbing.PlayerNum;
		}
		return result;
	}

	// Token: 0x0600396C RID: 14700 RVA: 0x0010CC09 File Offset: 0x0010B009
	public void SetPlayerProfileName(PlayerSelectionInfo info, string name)
	{
		if (name != null)
		{
			name = name.ToUpper();
		}
		info.curProfile.profileName = name;
		this.events.Broadcast(new PlayerSelectionInfoChangedEvent(info));
	}

	// Token: 0x0600396D RID: 14701 RVA: 0x0010CC36 File Offset: 0x0010B036
	public void SetPlayerLocalName(PlayerSelectionInfo info, string name)
	{
		if (name != null)
		{
			name = name.ToUpper();
		}
		info.curProfile.localName = name;
		this.events.Broadcast(new PlayerSelectionInfoChangedEvent(info));
	}

	// Token: 0x0600396E RID: 14702 RVA: 0x0010CC64 File Offset: 0x0010B064
	public bool IsValidPayload(GameModeData modeData, GameLoadPayload payload)
	{
		GameModeSettings settings = modeData.settings;
		int num = 0;
		Dictionary<TeamNum, int> dictionary = new Dictionary<TeamNum, int>();
		IEnumerator enumerator = ((IEnumerable)payload.players).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)obj;
				int intFromPlayerNum = PlayerUtil.GetIntFromPlayerNum(playerSelectionInfo.playerNum, true);
				if (playerSelectionInfo.type != PlayerType.None && intFromPlayerNum <= settings.maxPlayerCount)
				{
					if (playerSelectionInfo.characterID == CharacterID.None)
					{
						return false;
					}
					if (!this.characterUnlockModel.IsUnlockedInGameMode(playerSelectionInfo.characterID, payload.battleConfig.mode))
					{
						return false;
					}
					if (!this.characterLists.GetCharacterDefinition(playerSelectionInfo.characterID).isRandom)
					{
						SkinDefinition skinDefinition = this.characterDataHelper.GetSkinDefinition(playerSelectionInfo.characterID, playerSelectionInfo.skinKey);
						if (!this.IsSkinLegal(playerSelectionInfo.characterID, skinDefinition, SkinSelectMode.Offline))
						{
							return false;
						}
					}
					num++;
					if (!dictionary.ContainsKey(playerSelectionInfo.team))
					{
						dictionary.Add(playerSelectionInfo.team, 0);
					}
					Dictionary<TeamNum, int> dictionary2;
					TeamNum team;
					(dictionary2 = dictionary)[team = playerSelectionInfo.team] = dictionary2[team] + 1;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		if (num < settings.minPlayerCount)
		{
			return false;
		}
		if (settings.usesTeams)
		{
			if (dictionary.Count < 2)
			{
				return false;
			}
			if (settings.requireEvenTeams)
			{
				int num2 = -1;
				foreach (int num3 in dictionary.Values)
				{
					if (num2 == -1)
					{
						num2 = num3;
					}
					else if (num2 != num3)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	// Token: 0x0600396F RID: 14703 RVA: 0x0010CE84 File Offset: 0x0010B284
	public bool IsSkinLegal(CharacterID characterId, SkinDefinition skinDefinition, SkinSelectMode selectMode)
	{
		if (skinDefinition == null || !this.characterLists.IsEnabled(skinDefinition))
		{
			return false;
		}
		if (selectMode == SkinSelectMode.Online)
		{
			if (!this.userInventory.HasSkin(skinDefinition))
			{
				return false;
			}
		}
		else if (selectMode == SkinSelectMode.Offline && !this.userInventory.HasSkin(skinDefinition) && !this.isSkinTemporarilyAvailable(characterId, skinDefinition))
		{
			return false;
		}
		return true;
	}

	// Token: 0x06003970 RID: 14704 RVA: 0x0010CEF8 File Offset: 0x0010B2F8
	private bool isSkinTemporarilyAvailable(CharacterID characterID, SkinDefinition skinDefinition)
	{
		int ownedCharacterItemCount = this.userInventory.GetOwnedCharacterItemCount(characterID, EquipmentTypes.SKIN);
		int minimumSkinChoices = this.gameDataManager.ConfigData.characterSelectSettings.minimumSkinChoices;
		int num = minimumSkinChoices - ownedCharacterItemCount;
		int num2 = -1;
		SkinDefinition[] skins = this.skinDataManager.GetSkins(characterID);
		for (int i = 0; i < skins.Length; i++)
		{
			SkinDefinition skinDefinition2 = skins[i];
			if (skinDefinition2 == skinDefinition)
			{
				num2 = i;
			}
			if (this.userInventory.HasSkin(skinDefinition2) && i < num)
			{
				num++;
			}
		}
		return num2 < num;
	}

	// Token: 0x06003971 RID: 14705 RVA: 0x0010CF8F File Offset: 0x0010B38F
	public bool IsSkinAvailable(CharacterID characterId, SkinDefinition skinData, PlayerNum playerNum, PlayerSelectionList allPlayers, SkinSelectMode selectMode)
	{
		return this.IsSkinLegal(characterId, skinData, selectMode) && !allPlayers.IsSkinInUse(characterId, skinData, playerNum);
	}

	// Token: 0x06003972 RID: 14706 RVA: 0x0010CFB4 File Offset: 0x0010B3B4
	public SkinDefinition GetAvailableEquippedOrDefaultSkin(CharacterDefinition characterDef, PlayerNum playerNum, PlayerSelectionList allPlayers, SkinSelectMode selectMode)
	{
		if (characterDef == null || characterDef.isRandom)
		{
			return null;
		}
		int bestPortId = this.userInputManager.GetBestPortId(playerNum);
		SkinDefinition defaultSkin = this.GetDefaultSkin(characterDef.characterID, bestPortId);
		SkinDefinition[] skins = this.skinDataManager.GetSkins(characterDef.characterID);
		SkinDefinition defaultSkin2 = this.skinDataManager.GetDefaultSkin(characterDef.characterID);
		if (this.IsSkinAvailable(characterDef.characterID, defaultSkin, playerNum, allPlayers, selectMode))
		{
			return defaultSkin;
		}
		if (this.IsSkinAvailable(characterDef.characterID, defaultSkin2, playerNum, allPlayers, selectMode))
		{
			return defaultSkin2;
		}
		foreach (SkinDefinition skinDefinition in skins)
		{
			if (this.IsSkinAvailable(characterDef.characterID, skinDefinition, playerNum, allPlayers, selectMode))
			{
				return skinDefinition;
			}
		}
		return defaultSkin2;
	}

	// Token: 0x06003973 RID: 14707 RVA: 0x0010D088 File Offset: 0x0010B488
	public SkinDefinition GetNextSkin(PlayerNum playerNum, int direction, CharacterID characterID, string skinKey, PlayerSelectionList allPlayers, SkinSelectMode selectMode)
	{
		PlayerSelectionInfo player = allPlayers.GetPlayer(playerNum);
		if (player == null || characterID == CharacterID.None)
		{
			return null;
		}
		SkinDefinition[] skins = this.skinDataManager.GetSkins(characterID);
		int num = -1;
		for (int i = 0; i < skins.Length; i++)
		{
			if (skins[i].uniqueKey == skinKey)
			{
				num = i;
				break;
			}
		}
		for (int j = 1; j < skins.Length; j++)
		{
			int k;
			for (k = num + direction * j; k < 0; k += skins.Length)
			{
			}
			k %= skins.Length;
			SkinDefinition skinDefinition = skins[k];
			if (this.IsSkinAvailable(characterID, skinDefinition, playerNum, allPlayers, selectMode))
			{
				return skinDefinition;
			}
		}
		return null;
	}

	// Token: 0x06003974 RID: 14708 RVA: 0x0010D14C File Offset: 0x0010B54C
	public void ClearHighlightInfoSelectedSkins()
	{
		foreach (KeyValuePair<PlayerNum, CharacterSelectModel.HighlightInfo> keyValuePair in this.highlightInfo)
		{
			PlayerNum key = keyValuePair.Key;
			CharacterSelectModel.HighlightInfo value = keyValuePair.Value;
			value.skinIDs.Clear();
		}
	}

	// Token: 0x040027A3 RID: 10147
	private Dictionary<PlayerNum, CharacterSelectModel.HighlightInfo> highlightInfo = new Dictionary<PlayerNum, CharacterSelectModel.HighlightInfo>();

	// Token: 0x040027A4 RID: 10148
	private bool alwaysUnlockedEnabled = true;

	// Token: 0x020008DA RID: 2266
	public class HighlightInfo
	{
		// Token: 0x06003976 RID: 14710 RVA: 0x0010D1CF File Offset: 0x0010B5CF
		public string GetSkinKey(CharacterID characterID)
		{
			if (characterID != CharacterID.None && this.skinIDs.ContainsKey(characterID))
			{
				return this.skinIDs[characterID];
			}
			return null;
		}

		// Token: 0x040027A5 RID: 10149
		public CharacterID characterID;

		// Token: 0x040027A6 RID: 10150
		public Dictionary<CharacterID, string> skinIDs = new Dictionary<CharacterID, string>();
	}
}
