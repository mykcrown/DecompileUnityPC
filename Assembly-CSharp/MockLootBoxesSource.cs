using System;
using System.Collections.Generic;

// Token: 0x0200075C RID: 1884
public class MockLootBoxesSource : ILootBoxesSource
{
	// Token: 0x06002EAB RID: 11947 RVA: 0x000EBB94 File Offset: 0x000E9F94
	public MockLootBoxesSource()
	{
		this.list = new List<LootBoxPackage>
		{
			new LootBoxPackage(1UL, 2UL, 2UL, CurrencyType.Hard, 299UL),
			new LootBoxPackage(2UL, 2UL, 5UL, CurrencyType.Hard, 599UL),
			new LootBoxPackage(3UL, 2UL, 11UL, CurrencyType.Hard, 1299UL),
			new LootBoxPackage(309UL, 2UL, 25UL, CurrencyType.Hard, 2499UL),
			new LootBoxPackage(310UL, 2UL, 55UL, CurrencyType.Hard, 4999UL)
		}.ToArray();
	}

	// Token: 0x06002EAC RID: 11948 RVA: 0x000EBC48 File Offset: 0x000EA048
	public LootBoxPackage[] GetAllLootBoxes()
	{
		return this.list;
	}

	// Token: 0x06002EAD RID: 11949 RVA: 0x000EBC50 File Offset: 0x000EA050
	public LootBoxPackage GetLootBoxByPackageId(ulong packageId)
	{
		foreach (LootBoxPackage lootBoxPackage in this.list)
		{
			if (lootBoxPackage.packageId == packageId)
			{
				return lootBoxPackage;
			}
		}
		return null;
	}

	// Token: 0x040020BC RID: 8380
	private LootBoxPackage[] list = new LootBoxPackage[5];
}
