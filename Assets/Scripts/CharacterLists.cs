// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class CharacterLists : ICharacterLists
{
	private CharacterDefinition[] index;

	private CharacterDefinition[] nonRandom;

	private CharacterDefinition random;

	private Dictionary<CharacterID, CharacterDefinition> indexById = new Dictionary<CharacterID, CharacterDefinition>(default(CharacterIDComparer));

	private Dictionary<string, CharacterDefinition> indexByName = new Dictionary<string, CharacterDefinition>();

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

	[Inject]
	public GameEnvironmentData environmentData
	{
		get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		List<CharacterDefinition> legalCharactersAsList = this.getLegalCharactersAsList();
		this.index = legalCharactersAsList.ToArray();
		CharacterDefinition[] array = this.index;
		for (int i = 0; i < array.Length; i++)
		{
			CharacterDefinition characterDefinition = array[i];
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

	public CharacterDefinition[] GetLegalCharacters()
	{
		return this.index;
	}

	public CharacterDefinition GetRandom()
	{
		return this.random;
	}

	public CharacterDefinition[] GetNonRandomCharacters()
	{
		return this.nonRandom;
	}

	private List<CharacterDefinition> getLegalCharactersAsList()
	{
		List<CharacterDefinition> list = new List<CharacterDefinition>();
		foreach (CharacterDefinition current in this.environmentData.characters)
		{
			if (this.IsEnabled(current))
			{
				list.Add(current);
			}
		}
		return list;
	}

	public bool IsEnabled(CharacterDefinition characterDef)
	{
		return !(characterDef == null) && characterDef.enabled && (!this.config.uiuxSettings.DemoModeEnabled || characterDef.demoEnabled);
	}

	public bool IsEnabled(SkinDefinition skinDefinition)
	{
		return !(skinDefinition == null) && skinDefinition.enabled && (!this.config.uiuxSettings.DemoModeEnabled || skinDefinition.demoEnabled);
	}

	public CharacterDefinition GetCharacterDefinition(CharacterID characterId)
	{
		return this.indexById[characterId];
	}

	public CharacterDefinition GetCharacterDefinition(string characterName)
	{
		return this.indexByName[characterName];
	}
}
