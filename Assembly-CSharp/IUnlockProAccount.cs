using System;

// Token: 0x0200076C RID: 1900
public interface IUnlockProAccount
{
	// Token: 0x06002F12 RID: 12050
	void SetPrice(ulong price, CurrencyType currencyType);

	// Token: 0x06002F13 RID: 12051
	void SetPackageId(ulong id);

	// Token: 0x06002F14 RID: 12052
	void SetFoundersPackageId(ulong id);

	// Token: 0x06002F15 RID: 12053
	void SetFoundersPrice(ulong price, CurrencyType currencyType);

	// Token: 0x06002F16 RID: 12054
	void Purchase(Action<UserPurchaseResult> callback);

	// Token: 0x06002F17 RID: 12055
	void PurchaseFoundersPack(Action<UserPurchaseResult> callback);

	// Token: 0x06002F18 RID: 12056
	CurrencyType GetCurrencyType();

	// Token: 0x06002F19 RID: 12057
	CurrencyType GetFoundersCurrencyType();

	// Token: 0x06002F1A RID: 12058
	int GetPrice();

	// Token: 0x06002F1B RID: 12059
	string GetPriceString();

	// Token: 0x06002F1C RID: 12060
	string GetFoundersPackPriceString();
}
