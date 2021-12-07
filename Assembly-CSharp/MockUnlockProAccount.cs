using System;

// Token: 0x02000769 RID: 1897
public class MockUnlockProAccount : IUnlockProAccount
{
	// Token: 0x17000B62 RID: 2914
	// (get) Token: 0x06002EE9 RID: 12009 RVA: 0x000EC9BA File Offset: 0x000EADBA
	// (set) Token: 0x06002EEA RID: 12010 RVA: 0x000EC9C2 File Offset: 0x000EADC2
	[Inject]
	public IUserProAccountUnlockedModel userProAccountUnlockModel { get; set; }

	// Token: 0x17000B63 RID: 2915
	// (get) Token: 0x06002EEB RID: 12011 RVA: 0x000EC9CB File Offset: 0x000EADCB
	// (set) Token: 0x06002EEC RID: 12012 RVA: 0x000EC9D3 File Offset: 0x000EADD3
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x17000B64 RID: 2916
	// (get) Token: 0x06002EED RID: 12013 RVA: 0x000EC9DC File Offset: 0x000EADDC
	// (set) Token: 0x06002EEE RID: 12014 RVA: 0x000EC9E4 File Offset: 0x000EADE4
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x06002EEF RID: 12015 RVA: 0x000EC9F0 File Offset: 0x000EADF0
	public void Purchase(Action<UserPurchaseResult> callback)
	{
		this.timer.SetTimeout(50, delegate
		{
			this.userProAccountUnlockModel.SetUnlocked();
			callback(UserPurchaseResult.SUCCESS);
		});
	}

	// Token: 0x06002EF0 RID: 12016 RVA: 0x000ECA2C File Offset: 0x000EAE2C
	public void PurchaseFoundersPack(Action<UserPurchaseResult> callback)
	{
		this.timer.SetTimeout(50, delegate
		{
			this.userProAccountUnlockModel.SetUnlocked();
			callback(UserPurchaseResult.SUCCESS);
		});
	}

	// Token: 0x06002EF1 RID: 12017 RVA: 0x000ECA66 File Offset: 0x000EAE66
	public int GetPrice()
	{
		return 6524;
	}

	// Token: 0x06002EF2 RID: 12018 RVA: 0x000ECA70 File Offset: 0x000EAE70
	public string GetPriceString()
	{
		float price = (float)this.GetPrice();
		return this.localization.GetHardPriceString(price);
	}

	// Token: 0x06002EF3 RID: 12019 RVA: 0x000ECA91 File Offset: 0x000EAE91
	public int GetFoundersPackPrice()
	{
		return 3999;
	}

	// Token: 0x06002EF4 RID: 12020 RVA: 0x000ECA98 File Offset: 0x000EAE98
	public string GetFoundersPackPriceString()
	{
		float price = (float)this.GetFoundersPackPrice();
		return this.localization.GetHardPriceString(price);
	}

	// Token: 0x06002EF5 RID: 12021 RVA: 0x000ECAB9 File Offset: 0x000EAEB9
	public void SetPackageId(ulong id)
	{
	}

	// Token: 0x06002EF6 RID: 12022 RVA: 0x000ECABB File Offset: 0x000EAEBB
	public void SetPrice(ulong price, CurrencyType currencyType)
	{
	}

	// Token: 0x06002EF7 RID: 12023 RVA: 0x000ECABD File Offset: 0x000EAEBD
	public void SetFoundersPackageId(ulong id)
	{
	}

	// Token: 0x06002EF8 RID: 12024 RVA: 0x000ECABF File Offset: 0x000EAEBF
	public void SetFoundersPrice(ulong price, CurrencyType currencyType)
	{
	}

	// Token: 0x06002EF9 RID: 12025 RVA: 0x000ECAC1 File Offset: 0x000EAEC1
	public CurrencyType GetCurrencyType()
	{
		return CurrencyType.Hard;
	}

	// Token: 0x06002EFA RID: 12026 RVA: 0x000ECAC4 File Offset: 0x000EAEC4
	public CurrencyType GetFoundersCurrencyType()
	{
		return CurrencyType.Hard;
	}
}
