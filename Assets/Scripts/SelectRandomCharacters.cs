// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectRandomCharacters
{
	[Inject]
	public GameDataManager gameDataManager
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
	public ICharacterSelectModel characterSelectModel
	{
		get;
		set;
	}

	[Inject]
	public ICharacterDataLoader characterDataLoader
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
	public ISkinDataManager skinDataManager
	{
		get;
		set;
	}

	private CharacterDefinition getRandomCharacter(CharacterDefinition[] nonRandomCharacters)
	{
		int num = UnityEngine.Random.Range(0, nonRandomCharacters.Length);
		if (num < 0 || num > nonRandomCharacters.Length)
		{
			UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				"Random has calculated incorrect values! ",
				num,
				" doesn't fit in a list of ",
				nonRandomCharacters.Length
			}));
		}
		return nonRandomCharacters[num];
	}

	public CharacterDefinition GetRandomCharacter(GameMode gameMode)
	{
		CharacterDefinition[] nonRandomCharacters = this.characterLists.GetNonRandomCharacters();
		List<CharacterDefinition> list = new List<CharacterDefinition>();
		CharacterDefinition[] array = nonRandomCharacters;
		for (int i = 0; i < array.Length; i++)
		{
			CharacterDefinition characterDefinition = array[i];
			if (this.characterUnlockModel.IsUnlockedInGameMode(characterDefinition.characterID, gameMode))
			{
				list.Add(characterDefinition);
			}
		}
		return this.getRandomCharacter(list.ToArray());
	}

	public SkinDefinition GetRandomSkinForCharacter(GameLoadPayload payload, CharacterID characterId, PlayerNum playerNum, SkinSelectMode skinMode)
	{
		SkinDefinition[] skins = this.skinDataManager.GetSkins(characterId);
		List<SkinDefinition> list = new List<SkinDefinition>();
		for (int i = 0; i < skins.Length; i++)
		{
			SkinDefinition skinDefinition = skins[i];
			if (this.characterSelectModel.IsSkinAvailable(characterId, skinDefinition, playerNum, payload.players, skinMode))
			{
				list.Add(skinDefinition);
			}
		}
		return (list.Count <= 0) ? null : list.SelectRandom<SkinDefinition>();
	}

	public void Execute(GameLoadPayload payload)
	{
		for (int i = 0; i < payload.players.Length; i++)
		{
			PlayerSelectionInfo playerSelectionInfo = payload.players[i];
			if (playerSelectionInfo.type != PlayerType.None && !playerSelectionInfo.isSpectator)
			{
				CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(playerSelectionInfo.characterID);
				if (playerSelectionInfo.isRandom && characterDefinition.isRandom)
				{
					CharacterDefinition randomCharacter = this.GetRandomCharacter(payload.battleConfig.mode);
					SkinSelectMode skinMode = (!payload.isOnlineGame) ? SkinSelectMode.Offline : SkinSelectMode.Online;
					SkinDefinition randomSkinForCharacter = this.GetRandomSkinForCharacter(payload, randomCharacter.characterID, playerSelectionInfo.playerNum, skinMode);
					playerSelectionInfo.SetCharacter(randomCharacter.characterID, this.skinDataManager, randomSkinForCharacter);
					CharacterDefinition[] linkedCharacters = this.characterDataHelper.GetLinkedCharacters(randomCharacter);
					if (linkedCharacters != null && linkedCharacters.Length > 1)
					{
						playerSelectionInfo.characterIndex = UnityEngine.Random.Range(0, linkedCharacters.Length);
					}
				}
			}
		}
	}
}
