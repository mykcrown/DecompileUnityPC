// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IUserGlobalEquippedModel : IStartupLoader
{
	bool IsEquipped(EquippableItem item, int portId);

	void Equip(EquippableItem item, int portId, bool alertServer = true);

	EquipmentID GetEquippedByType(EquipmentTypes type, int portId);

	void EquipNetsuke(EquippableItem item, int index, int portId, bool alertServer = true);

	EquipmentID GetEquippedNetsuke(int index, int portId);

	int GetOpenNetsukeSlot(int portId);
}
