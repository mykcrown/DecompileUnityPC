using System;

// Token: 0x0200075F RID: 1887
public interface ILootBoxesSource
{
	// Token: 0x06002EB1 RID: 11953
	LootBoxPackage[] GetAllLootBoxes();

	// Token: 0x06002EB2 RID: 11954
	LootBoxPackage GetLootBoxByPackageId(ulong packageId);
}
