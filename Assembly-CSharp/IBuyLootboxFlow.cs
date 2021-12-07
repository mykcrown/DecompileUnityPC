using System;

// Token: 0x02000A0D RID: 2573
public interface IBuyLootboxFlow
{
	// Token: 0x06004AAD RID: 19117
	void Start(ulong packageId, Action<UserPurchaseResult> callback);
}
