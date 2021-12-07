using System;

// Token: 0x02000A0B RID: 2571
public interface IFeaturedTabAPI
{
	// Token: 0x06004A8C RID: 19084
	void BuyProAccount();

	// Token: 0x06004A8D RID: 19085
	void BuyFoundersPack();

	// Token: 0x06004A8E RID: 19086
	string GetProAccountPriceString();

	// Token: 0x06004A8F RID: 19087
	string GetFoundersPackPriceString();

	// Token: 0x06004A90 RID: 19088
	ulong GetLootbox25BundlePackageId();

	// Token: 0x06004A91 RID: 19089
	ulong GetLootbox55BundlePackageId();

	// Token: 0x06004A92 RID: 19090
	string GetLootbox25BundlePriceString();

	// Token: 0x06004A93 RID: 19091
	string GetLootbox55BundlePriceString();

	// Token: 0x06004A94 RID: 19092
	bool IsProAccountUnlocked();
}
