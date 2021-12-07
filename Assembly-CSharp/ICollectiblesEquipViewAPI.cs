using System;

// Token: 0x020009EC RID: 2540
public interface ICollectiblesEquipViewAPI
{
	// Token: 0x0600486F RID: 18543
	EquipmentTypes[] GetValidEquipTypes();

	// Token: 0x06004870 RID: 18544
	EquippableItem[] GetItems(EquipmentTypes type);

	// Token: 0x06004871 RID: 18545
	Netsuke GetNetsukeFromItem(EquippableItem item);

	// Token: 0x06004872 RID: 18546
	PlayerToken GetPlayerTokenFromItem(EquippableItem item);

	// Token: 0x06004873 RID: 18547
	PlayerCardIconData GetPlayerIconDataFromItem(EquippableItem item);
}
