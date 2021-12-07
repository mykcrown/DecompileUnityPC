// Decompile from assembly: Assembly-CSharp.dll

using System;

public class LootBoxPackage
{
	public ulong packageId;

	public ulong lootBoxId;

	public ulong quantity;

	public ulong price;

	public CurrencyType currencyType;

	public LootBoxPackage(ulong packageId, ulong lootBoxId, ulong quantity, CurrencyType currencyType, ulong price)
	{
		this.packageId = packageId;
		this.lootBoxId = lootBoxId;
		this.quantity = quantity;
		this.price = price;
		this.currencyType = currencyType;
	}
}
