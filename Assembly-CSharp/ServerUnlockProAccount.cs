using System;

// Token: 0x0200076B RID: 1899
public class ServerUnlockProAccount : IUnlockProAccount
{
	// Token: 0x17000B65 RID: 2917
	// (get) Token: 0x06002EFE RID: 12030 RVA: 0x000ECB26 File Offset: 0x000EAF26
	// (set) Token: 0x06002EFF RID: 12031 RVA: 0x000ECB2E File Offset: 0x000EAF2E
	[Inject]
	public IUserProAccountUnlockedModel userProAccountUnlockModel { get; set; }

	// Token: 0x17000B66 RID: 2918
	// (get) Token: 0x06002F00 RID: 12032 RVA: 0x000ECB37 File Offset: 0x000EAF37
	// (set) Token: 0x06002F01 RID: 12033 RVA: 0x000ECB3F File Offset: 0x000EAF3F
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x17000B67 RID: 2919
	// (get) Token: 0x06002F02 RID: 12034 RVA: 0x000ECB48 File Offset: 0x000EAF48
	// (set) Token: 0x06002F03 RID: 12035 RVA: 0x000ECB50 File Offset: 0x000EAF50
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x17000B68 RID: 2920
	// (get) Token: 0x06002F04 RID: 12036 RVA: 0x000ECB59 File Offset: 0x000EAF59
	// (set) Token: 0x06002F05 RID: 12037 RVA: 0x000ECB61 File Offset: 0x000EAF61
	[Inject]
	public IUserPurchaseEquipment purchaseEquipment { get; set; }

	// Token: 0x06002F06 RID: 12038 RVA: 0x000ECB6C File Offset: 0x000EAF6C
	public void Purchase(Action<UserPurchaseResult> callback)
	{
		this.purchaseEquipment.PurchaseManual(this.packageId, this.currencyType, this.price, delegate(UserPurchaseResult result)
		{
			if (result == UserPurchaseResult.SUCCESS)
			{
				this.userProAccountUnlockModel.SetUnlocked();
			}
			callback(result);
		});
	}

	// Token: 0x06002F07 RID: 12039 RVA: 0x000ECBB8 File Offset: 0x000EAFB8
	public void PurchaseFoundersPack(Action<UserPurchaseResult> callback)
	{
		this.purchaseEquipment.PurchaseManual(this.foundersPackageId, this.foundersCurrencyType, this.foundersPrice, delegate(UserPurchaseResult result)
		{
			if (result == UserPurchaseResult.SUCCESS)
			{
				this.userProAccountUnlockModel.SetUnlocked();
			}
			callback(result);
		});
	}

	// Token: 0x06002F08 RID: 12040 RVA: 0x000ECC02 File Offset: 0x000EB002
	public int GetPrice()
	{
		return (int)this.price;
	}

	// Token: 0x06002F09 RID: 12041 RVA: 0x000ECC0C File Offset: 0x000EB00C
	public string GetPriceString()
	{
		float num = (float)this.GetPrice();
		return this.localization.GetHardPriceString(num);
	}

	// Token: 0x06002F0A RID: 12042 RVA: 0x000ECC2D File Offset: 0x000EB02D
	public float GetFoundersPackPrice()
	{
		return this.foundersPrice;
	}

	// Token: 0x06002F0B RID: 12043 RVA: 0x000ECC38 File Offset: 0x000EB038
	public string GetFoundersPackPriceString()
	{
		float foundersPackPrice = this.GetFoundersPackPrice();
		return this.localization.GetHardPriceString(foundersPackPrice);
	}

	// Token: 0x06002F0C RID: 12044 RVA: 0x000ECC58 File Offset: 0x000EB058
	public void SetPrice(ulong price, CurrencyType currencyType)
	{
		this.price = price;
		this.currencyType = currencyType;
	}

	// Token: 0x06002F0D RID: 12045 RVA: 0x000ECC68 File Offset: 0x000EB068
	public void SetPackageId(ulong id)
	{
		this.packageId = id;
	}

	// Token: 0x06002F0E RID: 12046 RVA: 0x000ECC71 File Offset: 0x000EB071
	public void SetFoundersPackageId(ulong id)
	{
		this.foundersPackageId = id;
	}

	// Token: 0x06002F0F RID: 12047 RVA: 0x000ECC7A File Offset: 0x000EB07A
	public void SetFoundersPrice(ulong price, CurrencyType currencyType)
	{
		this.foundersPrice = price;
		this.foundersCurrencyType = currencyType;
	}

	// Token: 0x06002F10 RID: 12048 RVA: 0x000ECC8A File Offset: 0x000EB08A
	public CurrencyType GetCurrencyType()
	{
		return this.currencyType;
	}

	// Token: 0x06002F11 RID: 12049 RVA: 0x000ECC92 File Offset: 0x000EB092
	public CurrencyType GetFoundersCurrencyType()
	{
		return this.foundersCurrencyType;
	}

	// Token: 0x040020E7 RID: 8423
	private ulong packageId;

	// Token: 0x040020E8 RID: 8424
	private ulong price;

	// Token: 0x040020E9 RID: 8425
	private CurrencyType currencyType;

	// Token: 0x040020EA RID: 8426
	private ulong foundersPackageId;

	// Token: 0x040020EB RID: 8427
	private ulong foundersPrice;

	// Token: 0x040020EC RID: 8428
	private CurrencyType foundersCurrencyType;
}
