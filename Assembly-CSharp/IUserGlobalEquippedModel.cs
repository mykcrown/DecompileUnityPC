using System;

// Token: 0x02000747 RID: 1863
public interface IUserGlobalEquippedModel : IStartupLoader
{
	// Token: 0x06002E25 RID: 11813
	bool IsEquipped(EquippableItem item, int portId);

	// Token: 0x06002E26 RID: 11814
	void Equip(EquippableItem item, int portId, bool alertServer = true);

	// Token: 0x06002E27 RID: 11815
	EquipmentID GetEquippedByType(EquipmentTypes type, int portId);

	// Token: 0x06002E28 RID: 11816
	void EquipNetsuke(EquippableItem item, int index, int portId, bool alertServer = true);

	// Token: 0x06002E29 RID: 11817
	EquipmentID GetEquippedNetsuke(int index, int portId);

	// Token: 0x06002E2A RID: 11818
	int GetOpenNetsukeSlot(int portId);
}
