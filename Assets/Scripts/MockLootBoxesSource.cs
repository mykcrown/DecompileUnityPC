// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class MockLootBoxesSource : ILootBoxesSource
{
	private LootBoxPackage[] list = new LootBoxPackage[5];

	public MockLootBoxesSource()
	{
		this.list = new List<LootBoxPackage>
		{
			new LootBoxPackage(1uL, 2uL, 2uL, CurrencyType.Hard, 299uL),
			new LootBoxPackage(2uL, 2uL, 5uL, CurrencyType.Hard, 599uL),
			new LootBoxPackage(3uL, 2uL, 11uL, CurrencyType.Hard, 1299uL),
			new LootBoxPackage(309uL, 2uL, 25uL, CurrencyType.Hard, 2499uL),
			new LootBoxPackage(310uL, 2uL, 55uL, CurrencyType.Hard, 4999uL)
		}.ToArray();
	}

	public LootBoxPackage[] GetAllLootBoxes()
	{
		return this.list;
	}

	public LootBoxPackage GetLootBoxByPackageId(ulong packageId)
	{
		LootBoxPackage[] array = this.list;
		for (int i = 0; i < array.Length; i++)
		{
			LootBoxPackage lootBoxPackage = array[i];
			if (lootBoxPackage.packageId == packageId)
			{
				return lootBoxPackage;
			}
		}
		return null;
	}
}
