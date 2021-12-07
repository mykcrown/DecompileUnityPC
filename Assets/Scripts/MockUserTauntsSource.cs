// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class MockUserTauntsSource : IUserTauntsSource
{
	private Dictionary<CharacterID, Dictionary<TauntSlot, EquipmentID>> tauntLookup = new Dictionary<CharacterID, Dictionary<TauntSlot, EquipmentID>>();

	[Inject]
	public IEquipmentModel equipmentModel
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

	[PostConstruct]
	public void Init()
	{
		CharacterDefinition[] legalCharacters = this.characterLists.GetLegalCharacters();
		for (int i = 0; i < legalCharacters.Length; i++)
		{
			CharacterDefinition characterDefinition = legalCharacters[i];
			this.setupCharacterTaunts(characterDefinition.characterID);
		}
	}

	private void setupCharacterTaunts(CharacterID characterId)
	{
		Dictionary<TauntSlot, EquipmentID> value = new Dictionary<TauntSlot, EquipmentID>();
		this.tauntLookup[characterId] = value;
	}

	public Dictionary<CharacterID, Dictionary<TauntSlot, EquipmentID>> GetSourceData()
	{
		return this.tauntLookup;
	}
}
