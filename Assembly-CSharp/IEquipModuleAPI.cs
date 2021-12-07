using System;
using UnityEngine;

// Token: 0x020009FC RID: 2556
public interface IEquipModuleAPI
{
	// Token: 0x06004962 RID: 18786
	void LoadTypeList(EquipmentTypes[] validTypes);

	// Token: 0x06004963 RID: 18787
	void MapEquipTypeIcon(EquipmentTypes type, Sprite icon);

	// Token: 0x06004964 RID: 18788
	EquipTypeDefinition[] GetValidEquipTypes();

	// Token: 0x17001191 RID: 4497
	// (get) Token: 0x06004965 RID: 18789
	// (set) Token: 0x06004966 RID: 18790
	EquippableItem SelectedEquipment { get; set; }

	// Token: 0x17001192 RID: 4498
	// (get) Token: 0x06004967 RID: 18791
	// (set) Token: 0x06004968 RID: 18792
	EquipmentTypes SelectedEquipType { get; set; }

	// Token: 0x17001193 RID: 4499
	// (get) Token: 0x06004969 RID: 18793
	// (set) Token: 0x0600496A RID: 18794
	EquipmentSelectorModule.MenuType SelectedMenuType { get; set; }

	// Token: 0x17001194 RID: 4500
	// (get) Token: 0x0600496B RID: 18795
	CharacterID SelectedCharacter { get; }

	// Token: 0x0600496C RID: 18796
	bool HasItem(EquipmentID id);

	// Token: 0x0600496D RID: 18797
	bool CanEquip(EquippableItem item);

	// Token: 0x0600496E RID: 18798
	bool HasPrice(EquippableItem item);

	// Token: 0x0600496F RID: 18799
	bool IsNew(EquipmentID id);

	// Token: 0x06004970 RID: 18800
	bool IsEquipped(EquippableItem item);

	// Token: 0x06004971 RID: 18801
	string GetLocalizedItemName(EquippableItem item);
}
