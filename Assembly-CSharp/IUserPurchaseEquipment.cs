using System;

// Token: 0x0200074B RID: 1867
public interface IUserPurchaseEquipment
{
	// Token: 0x06002E54 RID: 11860
	void Purchase(EquipmentID itemId, Action<UserPurchaseResult> callback);

	// Token: 0x06002E55 RID: 11861
	void PurchaseManual(ulong packageId, CurrencyType currencyType, ulong price, Action<UserPurchaseResult> callback);

	// Token: 0x06002E56 RID: 11862
	void UnlockTokenPurchase(CharacterID character, Action<UserPurchaseResult> callback);
}
