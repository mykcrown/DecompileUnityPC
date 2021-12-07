// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public interface IEquipModuleAPI
{
	EquippableItem SelectedEquipment
	{
		get;
		set;
	}

	EquipmentTypes SelectedEquipType
	{
		get;
		set;
	}

	EquipmentSelectorModule.MenuType SelectedMenuType
	{
		get;
		set;
	}

	CharacterID SelectedCharacter
	{
		get;
	}

	void LoadTypeList(EquipmentTypes[] validTypes);

	void MapEquipTypeIcon(EquipmentTypes type, Sprite icon);

	EquipTypeDefinition[] GetValidEquipTypes();

	bool HasItem(EquipmentID id);

	bool CanEquip(EquippableItem item);

	bool HasPrice(EquippableItem item);

	bool IsNew(EquipmentID id);

	bool IsEquipped(EquippableItem item);

	string GetLocalizedItemName(EquippableItem item);
}
