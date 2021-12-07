using System;

// Token: 0x0200075D RID: 1885
public class LootBoxPackage
{
	// Token: 0x06002EAE RID: 11950 RVA: 0x000EBC8B File Offset: 0x000EA08B
	public LootBoxPackage(ulong packageId, ulong lootBoxId, ulong quantity, CurrencyType currencyType, ulong price)
	{
		this.packageId = packageId;
		this.lootBoxId = lootBoxId;
		this.quantity = quantity;
		this.price = price;
		this.currencyType = currencyType;
	}

	// Token: 0x040020BD RID: 8381
	public ulong packageId;

	// Token: 0x040020BE RID: 8382
	public ulong lootBoxId;

	// Token: 0x040020BF RID: 8383
	public ulong quantity;

	// Token: 0x040020C0 RID: 8384
	public ulong price;

	// Token: 0x040020C1 RID: 8385
	public CurrencyType currencyType;
}
