using System;

// Token: 0x02000A1E RID: 2590
public interface ICreate3DItemDisplay
{
	// Token: 0x06004B5B RID: 19291
	Item3DPreview CreateDisplay(EquippableItem item);

	// Token: 0x06004B5C RID: 19292
	Item3DPreview CreateDefault(EquipmentTypes type);

	// Token: 0x06004B5D RID: 19293
	void DestroyPreview(Item3DPreview preview);
}
