using System;

// Token: 0x0200072B RID: 1835
public interface IEquipmentSource
{
	// Token: 0x06002D4C RID: 11596
	bool IsReady();

	// Token: 0x06002D4D RID: 11597
	EquippableItem[] GetAll();
}
