using System;
using System.Collections.Generic;

// Token: 0x02000389 RID: 905
public class CharacterLists : ICharacterLists
{
	// Token: 0x1700038C RID: 908
	// (get) Token: 0x06001368 RID: 4968 RVA: 0x0006FF33 File Offset: 0x0006E333
	// (set) Token: 0x06001369 RID: 4969 RVA: 0x0006FF3B File Offset: 0x0006E33B
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x1700038D RID: 909
	// (get) Token: 0x0600136A RID: 4970 RVA: 0x0006FF44 File Offset: 0x0006E344
	// (set) Token: 0x0600136B RID: 4971 RVA: 0x0006FF4C File Offset: 0x0006E34C
	[Inject]
	public GameEnvironmentData environmentData { get; set; }

	// Token: 0x0600136C RID: 4972 RVA: 0x0006FF58 File Offset: 0x0006E358
	[PostConstruct]
	public void Init()
	{
		List<CharacterDefinition> legalCharactersAsList = this.getLegalCharactersAsList();
		this.index = legalCharactersAsList.ToArray();
		foreach (CharacterDefinition characterDefinition in this.index)
		{
			if (this.IsEnabled(characterDefinition))
			{
				this.indexByName[characterDefinition.characterName] = characterDefinition;
				if (!characterDefinition.isPartner)
				{
					this.indexById[characterDefinition.characterID] = characterDefinition;
				}
				if (characterDefinition.isRandom)
				{
					this.random = characterDefinition;
				}
			}
		}
		List<CharacterDefinition> legalCharactersAsList2 = this.getLegalCharactersAsList();
		for (int j = legalCharactersAsList2.Count - 1; j >= 0; j--)
		{
			if (legalCharactersAsList2[j].isRandom)
			{
				legalCharactersAsList2.RemoveAt(j);
			}
		}
		this.nonRandom = legalCharactersAsList2.ToArray();
		this.indexById[CharacterID.None] = null;
	}

	// Token: 0x0600136D RID: 4973 RVA: 0x00070040 File Offset: 0x0006E440
	public CharacterDefinition[] GetLegalCharacters()
	{
		return this.index;
	}

	// Token: 0x0600136E RID: 4974 RVA: 0x00070048 File Offset: 0x0006E448
	public CharacterDefinition GetRandom()
	{
		return this.random;
	}

	// Token: 0x0600136F RID: 4975 RVA: 0x00070050 File Offset: 0x0006E450
	public CharacterDefinition[] GetNonRandomCharacters()
	{
		return this.nonRandom;
	}

	// Token: 0x06001370 RID: 4976 RVA: 0x00070058 File Offset: 0x0006E458
	private List<CharacterDefinition> getLegalCharactersAsList()
	{
		List<CharacterDefinition> list = new List<CharacterDefinition>();
		foreach (CharacterDefinition characterDefinition in this.environmentData.characters)
		{
			if (this.IsEnabled(characterDefinition))
			{
				list.Add(characterDefinition);
			}
		}
		return list;
	}

	// Token: 0x06001371 RID: 4977 RVA: 0x000700CC File Offset: 0x0006E4CC
	public bool IsEnabled(CharacterDefinition characterDef)
	{
		return !(characterDef == null) && characterDef.enabled && (!this.config.uiuxSettings.DemoModeEnabled || characterDef.demoEnabled);
	}

	// Token: 0x06001372 RID: 4978 RVA: 0x0007010A File Offset: 0x0006E50A
	public bool IsEnabled(SkinDefinition skinDefinition)
	{
		return !(skinDefinition == null) && skinDefinition.enabled && (!this.config.uiuxSettings.DemoModeEnabled || skinDefinition.demoEnabled);
	}

	// Token: 0x06001373 RID: 4979 RVA: 0x00070148 File Offset: 0x0006E548
	public CharacterDefinition GetCharacterDefinition(CharacterID characterId)
	{
		return this.indexById[characterId];
	}

	// Token: 0x06001374 RID: 4980 RVA: 0x00070156 File Offset: 0x0006E556
	public CharacterDefinition GetCharacterDefinition(string characterName)
	{
		return this.indexByName[characterName];
	}

	// Token: 0x04000CD2 RID: 3282
	private CharacterDefinition[] index;

	// Token: 0x04000CD3 RID: 3283
	private CharacterDefinition[] nonRandom;

	// Token: 0x04000CD4 RID: 3284
	private CharacterDefinition random;

	// Token: 0x04000CD5 RID: 3285
	private Dictionary<CharacterID, CharacterDefinition> indexById = new Dictionary<CharacterID, CharacterDefinition>(default(CharacterIDComparer));

	// Token: 0x04000CD6 RID: 3286
	private Dictionary<string, CharacterDefinition> indexByName = new Dictionary<string, CharacterDefinition>();
}
