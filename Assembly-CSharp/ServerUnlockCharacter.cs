using System;
using System.Collections.Generic;

// Token: 0x020006D9 RID: 1753
public class ServerUnlockCharacter : IUnlockCharacter
{
	// Token: 0x17000AD2 RID: 2770
	// (get) Token: 0x06002C18 RID: 11288 RVA: 0x000E4EAD File Offset: 0x000E32AD
	// (set) Token: 0x06002C19 RID: 11289 RVA: 0x000E4EB5 File Offset: 0x000E32B5
	[Inject]
	public IUserCharacterUnlockModel userCharacterUnlockModel { get; set; }

	// Token: 0x17000AD3 RID: 2771
	// (get) Token: 0x06002C1A RID: 11290 RVA: 0x000E4EBE File Offset: 0x000E32BE
	// (set) Token: 0x06002C1B RID: 11291 RVA: 0x000E4EC6 File Offset: 0x000E32C6
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x17000AD4 RID: 2772
	// (get) Token: 0x06002C1C RID: 11292 RVA: 0x000E4ECF File Offset: 0x000E32CF
	// (set) Token: 0x06002C1D RID: 11293 RVA: 0x000E4ED7 File Offset: 0x000E32D7
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x17000AD5 RID: 2773
	// (get) Token: 0x06002C1E RID: 11294 RVA: 0x000E4EE0 File Offset: 0x000E32E0
	// (set) Token: 0x06002C1F RID: 11295 RVA: 0x000E4EE8 File Offset: 0x000E32E8
	[Inject]
	public IUserPurchaseEquipment purchaseEquipment { get; set; }

	// Token: 0x06002C20 RID: 11296 RVA: 0x000E4EF1 File Offset: 0x000E32F1
	public void SetSoftPrice(CharacterID id, ulong packageInfo, ulong price)
	{
		if (!this.prices.ContainsKey(id))
		{
			this.prices[id] = new ServerUnlockCharacter.PurchaseInfo();
		}
		this.prices[id].softCurrency = new ServerUnlockCharacter.PackageInfo(packageInfo, price);
	}

	// Token: 0x06002C21 RID: 11297 RVA: 0x000E4F2D File Offset: 0x000E332D
	public void SetHardPrice(CharacterID id, ulong packageInfo, ulong price)
	{
		if (!this.prices.ContainsKey(id))
		{
			this.prices[id] = new ServerUnlockCharacter.PurchaseInfo();
		}
		this.prices[id].hardCurrency = new ServerUnlockCharacter.PackageInfo(packageInfo, price);
	}

	// Token: 0x06002C22 RID: 11298 RVA: 0x000E4F6C File Offset: 0x000E336C
	public float GetHardPrice(CharacterID characterId)
	{
		if (this.prices.ContainsKey(characterId) && this.prices[characterId].hardCurrency != null)
		{
			return (float)((int)this.prices[characterId].hardCurrency.price);
		}
		return -1f;
	}

	// Token: 0x06002C23 RID: 11299 RVA: 0x000E4FC0 File Offset: 0x000E33C0
	public int GetSoftPrice(CharacterID characterId)
	{
		if (this.prices.ContainsKey(characterId) && this.prices[characterId].softCurrency != null)
		{
			return (int)this.prices[characterId].softCurrency.price;
		}
		return -1;
	}

	// Token: 0x06002C24 RID: 11300 RVA: 0x000E5010 File Offset: 0x000E3410
	public string GetHardPriceString(CharacterID characterId)
	{
		float hardPrice = this.GetHardPrice(characterId);
		return this.localization.GetHardPriceString(hardPrice);
	}

	// Token: 0x06002C25 RID: 11301 RVA: 0x000E5034 File Offset: 0x000E3434
	public string GetSoftPriceString(CharacterID characterId)
	{
		int softPrice = this.GetSoftPrice(characterId);
		return this.localization.GetSoftPriceString(softPrice);
	}

	// Token: 0x06002C26 RID: 11302 RVA: 0x000E5058 File Offset: 0x000E3458
	public void PurchaseWithHard(CharacterID characterId, Action<UserPurchaseResult> callback)
	{
		ServerUnlockCharacter.PackageInfo hardCurrency = this.prices[characterId].hardCurrency;
		this.purchaseEquipment.PurchaseManual(hardCurrency.packageId, CurrencyType.Hard, hardCurrency.price, delegate(UserPurchaseResult result)
		{
			if (result == UserPurchaseResult.SUCCESS)
			{
				this.userCharacterUnlockModel.SetUnlocked(characterId, true);
			}
			callback(result);
		});
	}

	// Token: 0x06002C27 RID: 11303 RVA: 0x000E50BC File Offset: 0x000E34BC
	public void PurchaseWithSoft(CharacterID characterId, Action<UserPurchaseResult> callback)
	{
		ServerUnlockCharacter.PackageInfo softCurrency = this.prices[characterId].softCurrency;
		this.purchaseEquipment.PurchaseManual(softCurrency.packageId, CurrencyType.Soft, softCurrency.price, delegate(UserPurchaseResult result)
		{
			if (result == UserPurchaseResult.SUCCESS)
			{
				this.userCharacterUnlockModel.SetUnlocked(characterId, true);
			}
			callback(result);
		});
	}

	// Token: 0x06002C28 RID: 11304 RVA: 0x000E5120 File Offset: 0x000E3520
	public void PurchaseWithToken(CharacterID characterId, Action<UserPurchaseResult> callback)
	{
		this.purchaseEquipment.UnlockTokenPurchase(characterId, delegate(UserPurchaseResult result)
		{
			if (result == UserPurchaseResult.SUCCESS)
			{
				this.userCharacterUnlockModel.SetUnlocked(characterId, true);
			}
			callback(result);
		});
	}

	// Token: 0x04001F63 RID: 8035
	private Dictionary<CharacterID, ServerUnlockCharacter.PurchaseInfo> prices = new Dictionary<CharacterID, ServerUnlockCharacter.PurchaseInfo>();

	// Token: 0x020006DA RID: 1754
	private class PurchaseInfo
	{
		// Token: 0x04001F64 RID: 8036
		public ServerUnlockCharacter.PackageInfo softCurrency;

		// Token: 0x04001F65 RID: 8037
		public ServerUnlockCharacter.PackageInfo hardCurrency;
	}

	// Token: 0x020006DB RID: 1755
	private class PackageInfo
	{
		// Token: 0x06002C2A RID: 11306 RVA: 0x000E516D File Offset: 0x000E356D
		public PackageInfo(ulong packageId, ulong price)
		{
			this.packageId = packageId;
			this.price = price;
		}

		// Token: 0x04001F66 RID: 8038
		public ulong packageId;

		// Token: 0x04001F67 RID: 8039
		public ulong price;
	}
}
