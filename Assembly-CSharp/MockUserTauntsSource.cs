using System;
using System.Collections.Generic;

// Token: 0x02000734 RID: 1844
public class MockUserTauntsSource : IUserTauntsSource
{
	// Token: 0x17000B20 RID: 2848
	// (get) Token: 0x06002D87 RID: 11655 RVA: 0x000E9329 File Offset: 0x000E7729
	// (set) Token: 0x06002D88 RID: 11656 RVA: 0x000E9331 File Offset: 0x000E7731
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x17000B21 RID: 2849
	// (get) Token: 0x06002D89 RID: 11657 RVA: 0x000E933A File Offset: 0x000E773A
	// (set) Token: 0x06002D8A RID: 11658 RVA: 0x000E9342 File Offset: 0x000E7742
	[Inject]
	public ICharacterLists characterLists { get; set; }

	// Token: 0x06002D8B RID: 11659 RVA: 0x000E934C File Offset: 0x000E774C
	[PostConstruct]
	public void Init()
	{
		foreach (CharacterDefinition characterDefinition in this.characterLists.GetLegalCharacters())
		{
			this.setupCharacterTaunts(characterDefinition.characterID);
		}
	}

	// Token: 0x06002D8C RID: 11660 RVA: 0x000E938C File Offset: 0x000E778C
	private void setupCharacterTaunts(CharacterID characterId)
	{
		Dictionary<TauntSlot, EquipmentID> value = new Dictionary<TauntSlot, EquipmentID>();
		this.tauntLookup[characterId] = value;
	}

	// Token: 0x06002D8D RID: 11661 RVA: 0x000E93AC File Offset: 0x000E77AC
	public Dictionary<CharacterID, Dictionary<TauntSlot, EquipmentID>> GetSourceData()
	{
		return this.tauntLookup;
	}

	// Token: 0x04002050 RID: 8272
	private Dictionary<CharacterID, Dictionary<TauntSlot, EquipmentID>> tauntLookup = new Dictionary<CharacterID, Dictionary<TauntSlot, EquipmentID>>();
}
