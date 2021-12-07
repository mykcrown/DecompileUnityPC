using System;

// Token: 0x02000759 RID: 1881
public class ServerBuyLootBox : IBuyLootBox
{
	// Token: 0x17000B55 RID: 2901
	// (get) Token: 0x06002E98 RID: 11928 RVA: 0x000EB9AD File Offset: 0x000E9DAD
	// (set) Token: 0x06002E99 RID: 11929 RVA: 0x000EB9B5 File Offset: 0x000E9DB5
	[Inject]
	public IUserPurchaseEquipment userPurchaseEquipment { get; set; }

	// Token: 0x17000B56 RID: 2902
	// (get) Token: 0x06002E9A RID: 11930 RVA: 0x000EB9BE File Offset: 0x000E9DBE
	// (set) Token: 0x06002E9B RID: 11931 RVA: 0x000EB9C6 File Offset: 0x000E9DC6
	[Inject]
	public ILootBoxesModel lootBoxesModel { get; set; }

	// Token: 0x06002E9C RID: 11932 RVA: 0x000EB9D0 File Offset: 0x000E9DD0
	public void Purchase(ulong packageId, Action<UserPurchaseResult> callback)
	{
		LootBoxPackage boxToBuy = this.lootBoxesModel.GetBoxToBuy(packageId);
		this.userPurchaseEquipment.PurchaseManual(boxToBuy.packageId, boxToBuy.currencyType, boxToBuy.price, callback);
	}

	// Token: 0x06002E9D RID: 11933 RVA: 0x000EBA08 File Offset: 0x000E9E08
	public CurrencyType GetCurrencyType(ulong packageId)
	{
		LootBoxPackage boxToBuy = this.lootBoxesModel.GetBoxToBuy(packageId);
		return boxToBuy.currencyType;
	}
}
