// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IUnlockProAccount
{
	void SetPrice(ulong price, CurrencyType currencyType);

	void SetPackageId(ulong id);

	void SetFoundersPackageId(ulong id);

	void SetFoundersPrice(ulong price, CurrencyType currencyType);

	void Purchase(Action<UserPurchaseResult> callback);

	void PurchaseFoundersPack(Action<UserPurchaseResult> callback);

	CurrencyType GetCurrencyType();

	CurrencyType GetFoundersCurrencyType();

	int GetPrice();

	string GetPriceString();

	string GetFoundersPackPriceString();
}
