using System;

// Token: 0x0200075E RID: 1886
public interface ILootBoxesModel
{
	// Token: 0x06002EAF RID: 11951
	LootBoxPackage GetBoxToBuy(ulong packageId);

	// Token: 0x06002EB0 RID: 11952
	LootBoxPackage[] GetPackages();
}
