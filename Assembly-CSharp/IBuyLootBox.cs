using System;

// Token: 0x0200075A RID: 1882
public interface IBuyLootBox
{
	// Token: 0x06002E9E RID: 11934
	void Purchase(ulong packageId, Action<UserPurchaseResult> callback);

	// Token: 0x06002E9F RID: 11935
	CurrencyType GetCurrencyType(ulong packageId);
}
