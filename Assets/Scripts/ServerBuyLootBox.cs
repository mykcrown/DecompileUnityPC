// Decompile from assembly: Assembly-CSharp.dll

using System;

public class ServerBuyLootBox : IBuyLootBox
{
	[Inject]
	public IUserPurchaseEquipment userPurchaseEquipment
	{
		get;
		set;
	}

	[Inject]
	public ILootBoxesModel lootBoxesModel
	{
		get;
		set;
	}

	public void Purchase(ulong packageId, Action<UserPurchaseResult> callback)
	{
		LootBoxPackage boxToBuy = this.lootBoxesModel.GetBoxToBuy(packageId);
		this.userPurchaseEquipment.PurchaseManual(boxToBuy.packageId, boxToBuy.currencyType, boxToBuy.price, callback);
	}

	public CurrencyType GetCurrencyType(ulong packageId)
	{
		LootBoxPackage boxToBuy = this.lootBoxesModel.GetBoxToBuy(packageId);
		return boxToBuy.currencyType;
	}
}
