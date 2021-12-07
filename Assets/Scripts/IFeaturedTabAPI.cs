// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IFeaturedTabAPI
{
	void BuyProAccount();

	void BuyFoundersPack();

	string GetProAccountPriceString();

	string GetFoundersPackPriceString();

	ulong GetLootbox25BundlePackageId();

	ulong GetLootbox55BundlePackageId();

	string GetLootbox25BundlePriceString();

	string GetLootbox55BundlePriceString();

	bool IsProAccountUnlocked();
}
