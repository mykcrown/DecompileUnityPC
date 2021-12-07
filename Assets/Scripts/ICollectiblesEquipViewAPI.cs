// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ICollectiblesEquipViewAPI
{
	EquipmentTypes[] GetValidEquipTypes();

	EquippableItem[] GetItems(EquipmentTypes type);

	Netsuke GetNetsukeFromItem(EquippableItem item);

	PlayerToken GetPlayerTokenFromItem(EquippableItem item);

	PlayerCardIconData GetPlayerIconDataFromItem(EquippableItem item);
}
