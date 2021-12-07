using System;

// Token: 0x02000A0A RID: 2570
public class FeaturedTabAPI : IFeaturedTabAPI
{
	// Token: 0x170011C2 RID: 4546
	// (get) Token: 0x06004A79 RID: 19065 RVA: 0x0013F85B File Offset: 0x0013DC5B
	// (set) Token: 0x06004A7A RID: 19066 RVA: 0x0013F863 File Offset: 0x0013DC63
	[Inject]
	public IDetailedUnlockCharacterFlow detailedUnlockFlow { get; set; }

	// Token: 0x170011C3 RID: 4547
	// (get) Token: 0x06004A7B RID: 19067 RVA: 0x0013F86C File Offset: 0x0013DC6C
	// (set) Token: 0x06004A7C RID: 19068 RVA: 0x0013F874 File Offset: 0x0013DC74
	[Inject]
	public IUnlockProAccount unlockProAccount { get; set; }

	// Token: 0x170011C4 RID: 4548
	// (get) Token: 0x06004A7D RID: 19069 RVA: 0x0013F87D File Offset: 0x0013DC7D
	// (set) Token: 0x06004A7E RID: 19070 RVA: 0x0013F885 File Offset: 0x0013DC85
	[Inject]
	public IUserProAccountUnlockedModel userProAccountModel { get; set; }

	// Token: 0x170011C5 RID: 4549
	// (get) Token: 0x06004A7F RID: 19071 RVA: 0x0013F88E File Offset: 0x0013DC8E
	// (set) Token: 0x06004A80 RID: 19072 RVA: 0x0013F896 File Offset: 0x0013DC96
	[Inject]
	public ILootBoxesSource lootboxSource { get; set; }

	// Token: 0x170011C6 RID: 4550
	// (get) Token: 0x06004A81 RID: 19073 RVA: 0x0013F89F File Offset: 0x0013DC9F
	// (set) Token: 0x06004A82 RID: 19074 RVA: 0x0013F8A7 File Offset: 0x0013DCA7
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x06004A83 RID: 19075 RVA: 0x0013F8B0 File Offset: 0x0013DCB0
	public void BuyProAccount()
	{
		this.detailedUnlockFlow.StartProAccount();
	}

	// Token: 0x06004A84 RID: 19076 RVA: 0x0013F8BD File Offset: 0x0013DCBD
	public void BuyFoundersPack()
	{
		this.detailedUnlockFlow.StartFoundersPack();
	}

	// Token: 0x06004A85 RID: 19077 RVA: 0x0013F8CA File Offset: 0x0013DCCA
	public string GetProAccountPriceString()
	{
		return this.unlockProAccount.GetPriceString();
	}

	// Token: 0x06004A86 RID: 19078 RVA: 0x0013F8D7 File Offset: 0x0013DCD7
	public string GetFoundersPackPriceString()
	{
		return this.unlockProAccount.GetFoundersPackPriceString();
	}

	// Token: 0x06004A87 RID: 19079 RVA: 0x0013F8E4 File Offset: 0x0013DCE4
	public ulong GetLootbox25BundlePackageId()
	{
		return 309UL;
	}

	// Token: 0x06004A88 RID: 19080 RVA: 0x0013F8EC File Offset: 0x0013DCEC
	public ulong GetLootbox55BundlePackageId()
	{
		return 310UL;
	}

	// Token: 0x06004A89 RID: 19081 RVA: 0x0013F8F4 File Offset: 0x0013DCF4
	public string GetLootbox25BundlePriceString()
	{
		LootBoxPackage lootBoxByPackageId = this.lootboxSource.GetLootBoxByPackageId(this.GetLootbox25BundlePackageId());
		if (lootBoxByPackageId == null)
		{
			return "ERROR: MISSING PACKAGE " + this.GetLootbox25BundlePackageId().ToString();
		}
		ulong price = lootBoxByPackageId.price;
		return this.localization.GetHardPriceString(price);
	}

	// Token: 0x06004A8A RID: 19082 RVA: 0x0013F950 File Offset: 0x0013DD50
	public string GetLootbox55BundlePriceString()
	{
		LootBoxPackage lootBoxByPackageId = this.lootboxSource.GetLootBoxByPackageId(this.GetLootbox55BundlePackageId());
		if (lootBoxByPackageId == null)
		{
			return "ERROR: MISSING PACKAGE " + this.GetLootbox55BundlePackageId().ToString();
		}
		ulong price = lootBoxByPackageId.price;
		return this.localization.GetHardPriceString(price);
	}

	// Token: 0x06004A8B RID: 19083 RVA: 0x0013F9A9 File Offset: 0x0013DDA9
	public bool IsProAccountUnlocked()
	{
		return this.userProAccountModel.IsUnlocked();
	}
}
