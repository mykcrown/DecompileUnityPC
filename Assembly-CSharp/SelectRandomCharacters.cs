using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000620 RID: 1568
public class SelectRandomCharacters
{
	// Token: 0x1700098A RID: 2442
	// (get) Token: 0x060026B2 RID: 9906 RVA: 0x000BD876 File Offset: 0x000BBC76
	// (set) Token: 0x060026B3 RID: 9907 RVA: 0x000BD87E File Offset: 0x000BBC7E
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x1700098B RID: 2443
	// (get) Token: 0x060026B4 RID: 9908 RVA: 0x000BD887 File Offset: 0x000BBC87
	// (set) Token: 0x060026B5 RID: 9909 RVA: 0x000BD88F File Offset: 0x000BBC8F
	[Inject]
	public ICharacterLists characterLists { get; set; }

	// Token: 0x1700098C RID: 2444
	// (get) Token: 0x060026B6 RID: 9910 RVA: 0x000BD898 File Offset: 0x000BBC98
	// (set) Token: 0x060026B7 RID: 9911 RVA: 0x000BD8A0 File Offset: 0x000BBCA0
	[Inject]
	public IUserCharacterUnlockModel characterUnlockModel { get; set; }

	// Token: 0x1700098D RID: 2445
	// (get) Token: 0x060026B8 RID: 9912 RVA: 0x000BD8A9 File Offset: 0x000BBCA9
	// (set) Token: 0x060026B9 RID: 9913 RVA: 0x000BD8B1 File Offset: 0x000BBCB1
	[Inject]
	public IUserInventory userInventory { get; set; }

	// Token: 0x1700098E RID: 2446
	// (get) Token: 0x060026BA RID: 9914 RVA: 0x000BD8BA File Offset: 0x000BBCBA
	// (set) Token: 0x060026BB RID: 9915 RVA: 0x000BD8C2 File Offset: 0x000BBCC2
	[Inject]
	public ICharacterSelectModel characterSelectModel { get; set; }

	// Token: 0x1700098F RID: 2447
	// (get) Token: 0x060026BC RID: 9916 RVA: 0x000BD8CB File Offset: 0x000BBCCB
	// (set) Token: 0x060026BD RID: 9917 RVA: 0x000BD8D3 File Offset: 0x000BBCD3
	[Inject]
	public ICharacterDataLoader characterDataLoader { get; set; }

	// Token: 0x17000990 RID: 2448
	// (get) Token: 0x060026BE RID: 9918 RVA: 0x000BD8DC File Offset: 0x000BBCDC
	// (set) Token: 0x060026BF RID: 9919 RVA: 0x000BD8E4 File Offset: 0x000BBCE4
	[Inject]
	public ICharacterDataHelper characterDataHelper { get; set; }

	// Token: 0x17000991 RID: 2449
	// (get) Token: 0x060026C0 RID: 9920 RVA: 0x000BD8ED File Offset: 0x000BBCED
	// (set) Token: 0x060026C1 RID: 9921 RVA: 0x000BD8F5 File Offset: 0x000BBCF5
	[Inject]
	public ISkinDataManager skinDataManager { get; set; }

	// Token: 0x060026C2 RID: 9922 RVA: 0x000BD900 File Offset: 0x000BBD00
	private CharacterDefinition getRandomCharacter(CharacterDefinition[] nonRandomCharacters)
	{
		int num = UnityEngine.Random.Range(0, nonRandomCharacters.Length);
		if (num < 0 || num > nonRandomCharacters.Length)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Random has calculated incorrect values! ",
				num,
				" doesn't fit in a list of ",
				nonRandomCharacters.Length
			}));
		}
		return nonRandomCharacters[num];
	}

	// Token: 0x060026C3 RID: 9923 RVA: 0x000BD960 File Offset: 0x000BBD60
	public CharacterDefinition GetRandomCharacter(GameMode gameMode)
	{
		CharacterDefinition[] nonRandomCharacters = this.characterLists.GetNonRandomCharacters();
		List<CharacterDefinition> list = new List<CharacterDefinition>();
		foreach (CharacterDefinition characterDefinition in nonRandomCharacters)
		{
			if (this.characterUnlockModel.IsUnlockedInGameMode(characterDefinition.characterID, gameMode))
			{
				list.Add(characterDefinition);
			}
		}
		return this.getRandomCharacter(list.ToArray());
	}

	// Token: 0x060026C4 RID: 9924 RVA: 0x000BD9C8 File Offset: 0x000BBDC8
	public SkinDefinition GetRandomSkinForCharacter(GameLoadPayload payload, CharacterID characterId, PlayerNum playerNum, SkinSelectMode skinMode)
	{
		SkinDefinition[] skins = this.skinDataManager.GetSkins(characterId);
		List<SkinDefinition> list = new List<SkinDefinition>();
		foreach (SkinDefinition skinDefinition in skins)
		{
			if (this.characterSelectModel.IsSkinAvailable(characterId, skinDefinition, playerNum, payload.players, skinMode))
			{
				list.Add(skinDefinition);
			}
		}
		return (list.Count <= 0) ? null : list.SelectRandom<SkinDefinition>();
	}

	// Token: 0x060026C5 RID: 9925 RVA: 0x000BDA3C File Offset: 0x000BBE3C
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
