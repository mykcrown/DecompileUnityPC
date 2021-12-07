// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IUserPurchaseEquipment
{
	void Purchase(EquipmentID itemId, Action<UserPurchaseResult> callback);

	void PurchaseManual(ulong packageId, CurrencyType currencyType, ulong price, Action<UserPurchaseResult> callback);

	void UnlockTokenPurchase(CharacterID character, Action<UserPurchaseResult> callback);
}
