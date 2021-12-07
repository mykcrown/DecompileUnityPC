using System;
using System.Collections.Generic;

// Token: 0x02000737 RID: 1847
public class EquipTauntDialogAPI
{
	// Token: 0x17000B25 RID: 2853
	// (get) Token: 0x06002D9F RID: 11679 RVA: 0x000E956D File Offset: 0x000E796D
	// (set) Token: 0x06002DA0 RID: 11680 RVA: 0x000E9575 File Offset: 0x000E7975
	[Inject]
	public IUserTauntsModel userTauntsModel { get; set; }

	// Token: 0x17000B26 RID: 2854
	// (get) Token: 0x06002DA1 RID: 11681 RVA: 0x000E957E File Offset: 0x000E797E
	// (set) Token: 0x06002DA2 RID: 11682 RVA: 0x000E9586 File Offset: 0x000E7986
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x17000B27 RID: 2855
	// (get) Token: 0x06002DA3 RID: 11683 RVA: 0x000E958F File Offset: 0x000E798F
	// (set) Token: 0x06002DA4 RID: 11684 RVA: 0x000E9597 File Offset: 0x000E7997
	[Inject]
	public IUserInventory userInventory { get; set; }

	// Token: 0x17000B28 RID: 2856
	// (get) Token: 0x06002DA5 RID: 11685 RVA: 0x000E95A0 File Offset: 0x000E79A0
	// (set) Token: 0x06002DA6 RID: 11686 RVA: 0x000E95A8 File Offset: 0x000E79A8
	[Inject]
	public IStoreAPI storeAPI { get; set; }

	// Token: 0x06002DA7 RID: 11687 RVA: 0x000E95B4 File Offset: 0x000E79B4
	public void Setup(CharacterID characterId)
	{
		this.characterId = characterId;
		Dictionary<TauntSlot, EquipmentID> slotsForCharacter = this.userTauntsModel.GetSlotsForCharacter(characterId, this.storeAPI.Port);
		foreach (TauntSlot key in slotsForCharacter.Keys)
		{
			EquippableItem item = this.equipmentModel.GetItem(slotsForCharacter[key]);
			this.data[key] = item.id;
		}
	}

	// Token: 0x06002DA8 RID: 11688 RVA: 0x000E9650 File Offset: 0x000E7A50
	public TauntSlotMap GetSlots()
	{
		return this.data;
	}

	// Token: 0x06002DA9 RID: 11689 RVA: 0x000E9658 File Offset: 0x000E7A58
	public string GetLocalizedEquipmentName(EquippableItem item)
	{
		return this.equipmentModel.GetLocalizedItemName(item);
	}

	// Token: 0x06002DAA RID: 11690 RVA: 0x000E9668 File Offset: 0x000E7A68
	public void ToggleSlot(TauntSlot slot, EquippableItem item)
	{
		this.userInventory.MarkAsNotNew(item.id, true);
		if (this.data.ContainsKey(slot) && this.data[slot] == item.id)
		{
			this.data.Remove(slot);
		}
		else
		{
			this.data[slot] = item.id;
		}
	}

	// Token: 0x06002DAB RID: 11691 RVA: 0x000E96D8 File Offset: 0x000E7AD8
	public void Save()
	{
		this.userTauntsModel.Save(this.characterId, this.data, this.storeAPI.Port, true);
	}

	// Token: 0x04002057 RID: 8279
	private TauntSlotMap data = new TauntSlotMap();

	// Token: 0x04002058 RID: 8280
	private CharacterID characterId;
}
