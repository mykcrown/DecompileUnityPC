// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class EquipTauntDialogAPI
{
	private TauntSlotMap data = new TauntSlotMap();

	private CharacterID characterId;

	[Inject]
	public IUserTauntsModel userTauntsModel
	{
		get;
		set;
	}

	[Inject]
	public IEquipmentModel equipmentModel
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
	public IStoreAPI storeAPI
	{
		get;
		set;
	}

	public void Setup(CharacterID characterId)
	{
		this.characterId = characterId;
		Dictionary<TauntSlot, EquipmentID> slotsForCharacter = this.userTauntsModel.GetSlotsForCharacter(characterId, this.storeAPI.Port);
		foreach (TauntSlot current in slotsForCharacter.Keys)
		{
			EquippableItem item = this.equipmentModel.GetItem(slotsForCharacter[current]);
			this.data[current] = item.id;
		}
	}

	public TauntSlotMap GetSlots()
	{
		return this.data;
	}

	public string GetLocalizedEquipmentName(EquippableItem item)
	{
		return this.equipmentModel.GetLocalizedItemName(item);
	}

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

	public void Save()
	{
		this.userTauntsModel.Save(this.characterId, this.data, this.storeAPI.Port, true);
	}
}
