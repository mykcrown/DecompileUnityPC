// Decompile from assembly: Assembly-CSharp.dll

using System;

public class FeaturedTabAPI : IFeaturedTabAPI
{
	[Inject]
	public IDetailedUnlockCharacterFlow detailedUnlockFlow
	{
		get;
		set;
	}

	[Inject]
	public IUnlockProAccount unlockProAccount
	{
		get;
		set;
	}

	[Inject]
	public IUserProAccountUnlockedModel userProAccountModel
	{
		get;
		set;
	}

	[Inject]
	public ILootBoxesSource lootboxSource
	{
		get;
		set;
	}

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	public void BuyProAccount()
	{
		this.detailedUnlockFlow.StartProAccount();
	}

	public void BuyFoundersPack()
	{
		this.detailedUnlockFlow.StartFoundersPack();
	}

	public string GetProAccountPriceString()
	{
		return this.unlockProAccount.GetPriceString();
	}

	public string GetFoundersPackPriceString()
	{
		return this.unlockProAccount.GetFoundersPackPriceString();
	}

	public ulong GetLootbox25BundlePackageId()
	{
		return 309uL;
	}

	public ulong GetLootbox55BundlePackageId()
	{
		return 310uL;
	}

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

	public bool IsProAccountUnlocked()
	{
		return this.userProAccountModel.IsUnlocked();
	}
}
