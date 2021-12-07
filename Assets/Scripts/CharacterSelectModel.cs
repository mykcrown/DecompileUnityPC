// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;

public class CharacterSelectModel : ICharacterSelectModel, IDataDependency
{
	public class HighlightInfo
	{
		public CharacterID characterID;

		public Dictionary<CharacterID, string> skinIDs = new Dictionary<CharacterID, string>();

		public string GetSkinKey(CharacterID characterID)
		{
			if (characterID != CharacterID.None && this.skinIDs.ContainsKey(characterID))
			{
				return this.skinIDs[characterID];
			}
			return null;
		}
	}

	private Dictionary<PlayerNum, CharacterSelectModel.HighlightInfo> highlightInfo = new Dictionary<PlayerNum, CharacterSelectModel.HighlightInfo>();

	private bool alwaysUnlockedEnabled = true;

	[Inject]
	public IDevConsole devConsole
	{
		get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	[Inject]
	public IUserCharacterEquippedModel userCharacterEquippedModel
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
	public IUserInputManager userInputManager
	{
		get;
		set;
	}

	[Inject]
	public PlayerInputLocator playerInput
	{
		get;
		set;
	}

	[Inject]
	public ITokenManager tokenManager
	{
		get;
		set;
	}

	[Inject]
	public IUserCharacterUnlockModel characterUnlockModel
	{
		get;
		set;
	}

	[Inject]
	public IUserInventory userInventory
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
	public ICharacterDataHelper characterDataHelper
	{
		get;
		set;
	}

	[Inject]
	public ICharacterLists characterLists
	{
		get;
		set;
	}

	[Inject]
	public ISkinDataManager skinDataManager
	{
		private get;
		set;
	}

	public void Load(Action<DataLoadResult> callback)
	{
		this.devConsole.AddAdminCommand(new Action(this.toggleAlwaysUnlocked), "toggleAlwaysUnlockedCharacters", null);
		callback(new DataLoadResult
		{
			status = DataLoadStatus.SUCCESS
		});
	}

	private void toggleAlwaysUnlocked()
	{
		this.alwaysUnlockedEnabled = !this.alwaysUnlockedEnabled;
		this.devConsole.PrintLn("'Always Unlocked' character data is " + ((!this.alwaysUnlockedEnabled) ? "DISABLED" : "ENABLED"));
	}

	public bool IsCharacterUnlocked(CharacterID characterId)
	{
		return this.characterUnlockModel.IsUnlocked(characterId);
	}

	public SkinDefinition GetDefaultSkin(CharacterID characterId, int portId)
	{
		return this.userCharacterEquippedModel.GetEquippedSkin(characterId, portId);
	}

	public CharacterSelectModel.HighlightInfo GetHighlightInfo(PlayerNum playerNum)
	{
		if (!this.highlightInfo.ContainsKey(playerNum))
		{
			this.highlightInfo[playerNum] = new CharacterSelectModel.HighlightInfo();
		}
		return this.highlightInfo[playerNum];
	}

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

	public void SetPlayerProfileName(PlayerSelectionInfo info, string name)
	{
		if (name != null)
		{
			name = name.ToUpper();
		}
		info.curProfile.profileName = name;
		this.events.Broadcast(new PlayerSelectionInfoChangedEvent(info));
	}

	public void SetPlayerLocalName(PlayerSelectionInfo info, string name)
	{
		if (name != null)
		{
			name = name.ToUpper();
		}
		info.curProfile.localName = name;
		this.events.Broadcast(new PlayerSelectionInfoChangedEvent(info));
	}

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
				PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)enumerator.Current;
				int intFromPlayerNum = PlayerUtil.GetIntFromPlayerNum(playerSelectionInfo.playerNum, true);
				if (playerSelectionInfo.type != PlayerType.None && intFromPlayerNum <= settings.maxPlayerCount)
				{
					if (playerSelectionInfo.characterID == CharacterID.None)
					{
						bool result = false;
						return result;
					}
					if (!this.characterUnlockModel.IsUnlockedInGameMode(playerSelectionInfo.characterID, payload.battleConfig.mode))
					{
						bool result = false;
						return result;
					}
					if (!this.characterLists.GetCharacterDefinition(playerSelectionInfo.characterID).isRandom)
					{
						SkinDefinition skinDefinition = this.characterDataHelper.GetSkinDefinition(playerSelectionInfo.characterID, playerSelectionInfo.skinKey);
						if (!this.IsSkinLegal(playerSelectionInfo.characterID, skinDefinition, SkinSelectMode.Offline))
						{
							bool result = false;
							return result;
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
				foreach (int current in dictionary.Values)
				{
					if (num2 == -1)
					{
						num2 = current;
					}
					else if (num2 != current)
					{
						bool result = false;
						return result;
					}
				}
			}
		}
		return true;
	}

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

	public bool IsSkinAvailable(CharacterID characterId, SkinDefinition skinData, PlayerNum playerNum, PlayerSelectionList allPlayers, SkinSelectMode selectMode)
	{
		return this.IsSkinLegal(characterId, skinData, selectMode) && !allPlayers.IsSkinInUse(characterId, skinData, playerNum);
	}

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
		SkinDefinition[] array = skins;
		for (int i = 0; i < array.Length; i++)
		{
			SkinDefinition skinDefinition = array[i];
			if (this.IsSkinAvailable(characterDef.characterID, skinDefinition, playerNum, allPlayers, selectMode))
			{
				return skinDefinition;
			}
		}
		return defaultSkin2;
	}

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

	public void ClearHighlightInfoSelectedSkins()
	{
		foreach (KeyValuePair<PlayerNum, CharacterSelectModel.HighlightInfo> current in this.highlightInfo)
		{
			PlayerNum key = current.Key;
			CharacterSelectModel.HighlightInfo value = current.Value;
			value.skinIDs.Clear();
		}
	}
}
