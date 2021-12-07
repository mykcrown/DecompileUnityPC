// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ServerUnlockCharacter : IUnlockCharacter
{
	private class PurchaseInfo
	{
		public ServerUnlockCharacter.PackageInfo softCurrency;

		public ServerUnlockCharacter.PackageInfo hardCurrency;
	}

	private class PackageInfo
	{
		public ulong packageId;

		public ulong price;

		public PackageInfo(ulong packageId, ulong price)
		{
			this.packageId = packageId;
			this.price = price;
		}
	}

	private sealed class _PurchaseWithHard_c__AnonStorey0
	{
		internal CharacterID characterId;

		internal Action<UserPurchaseResult> callback;

		internal ServerUnlockCharacter _this;

		internal void __m__0(UserPurchaseResult result)
		{
			if (result == UserPurchaseResult.SUCCESS)
			{
				this._this.userCharacterUnlockModel.SetUnlocked(this.characterId, true);
			}
			this.callback(result);
		}
	}

	private sealed class _PurchaseWithSoft_c__AnonStorey1
	{
		internal CharacterID characterId;

		internal Action<UserPurchaseResult> callback;

		internal ServerUnlockCharacter _this;

		internal void __m__0(UserPurchaseResult result)
		{
			if (result == UserPurchaseResult.SUCCESS)
			{
				this._this.userCharacterUnlockModel.SetUnlocked(this.characterId, true);
			}
			this.callback(result);
		}
	}

	private sealed class _PurchaseWithToken_c__AnonStorey2
	{
		internal CharacterID characterId;

		internal Action<UserPurchaseResult> callback;

		internal ServerUnlockCharacter _this;

		internal void __m__0(UserPurchaseResult result)
		{
			if (result == UserPurchaseResult.SUCCESS)
			{
				this._this.userCharacterUnlockModel.SetUnlocked(this.characterId, true);
			}
			this.callback(result);
		}
	}

	private Dictionary<CharacterID, ServerUnlockCharacter.PurchaseInfo> prices = new Dictionary<CharacterID, ServerUnlockCharacter.PurchaseInfo>();

	[Inject]
	public IUserCharacterUnlockModel userCharacterUnlockModel
	{
		get;
		set;
	}

	[Inject]
	public IMainThreadTimer timer
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

	[Inject]
	public IUserPurchaseEquipment purchaseEquipment
	{
		get;
		set;
	}

	public void SetSoftPrice(CharacterID id, ulong packageInfo, ulong price)
	{
		if (!this.prices.ContainsKey(id))
		{
			this.prices[id] = new ServerUnlockCharacter.PurchaseInfo();
		}
		this.prices[id].softCurrency = new ServerUnlockCharacter.PackageInfo(packageInfo, price);
	}

	public void SetHardPrice(CharacterID id, ulong packageInfo, ulong price)
	{
		if (!this.prices.ContainsKey(id))
		{
			this.prices[id] = new ServerUnlockCharacter.PurchaseInfo();
		}
		this.prices[id].hardCurrency = new ServerUnlockCharacter.PackageInfo(packageInfo, price);
	}

	public float GetHardPrice(CharacterID characterId)
	{
		if (this.prices.ContainsKey(characterId) && this.prices[characterId].hardCurrency != null)
		{
			return (float)((int)this.prices[characterId].hardCurrency.price);
		}
		return -1f;
	}

	public int GetSoftPrice(CharacterID characterId)
	{
		if (this.prices.ContainsKey(characterId) && this.prices[characterId].softCurrency != null)
		{
			return (int)this.prices[characterId].softCurrency.price;
		}
		return -1;
	}

	public string GetHardPriceString(CharacterID characterId)
	{
		float hardPrice = this.GetHardPrice(characterId);
		return this.localization.GetHardPriceString(hardPrice);
	}

	public string GetSoftPriceString(CharacterID characterId)
	{
		int softPrice = this.GetSoftPrice(characterId);
		return this.localization.GetSoftPriceString(softPrice);
	}

	public void PurchaseWithHard(CharacterID characterId, Action<UserPurchaseResult> callback)
	{
		ServerUnlockCharacter._PurchaseWithHard_c__AnonStorey0 _PurchaseWithHard_c__AnonStorey = new ServerUnlockCharacter._PurchaseWithHard_c__AnonStorey0();
		_PurchaseWithHard_c__AnonStorey.characterId = characterId;
		_PurchaseWithHard_c__AnonStorey.callback = callback;
		_PurchaseWithHard_c__AnonStorey._this = this;
		ServerUnlockCharacter.PackageInfo hardCurrency = this.prices[_PurchaseWithHard_c__AnonStorey.characterId].hardCurrency;
		this.purchaseEquipment.PurchaseManual(hardCurrency.packageId, CurrencyType.Hard, hardCurrency.price, new Action<UserPurchaseResult>(_PurchaseWithHard_c__AnonStorey.__m__0));
	}

	public void PurchaseWithSoft(CharacterID characterId, Action<UserPurchaseResult> callback)
	{
		ServerUnlockCharacter._PurchaseWithSoft_c__AnonStorey1 _PurchaseWithSoft_c__AnonStorey = new ServerUnlockCharacter._PurchaseWithSoft_c__AnonStorey1();
		_PurchaseWithSoft_c__AnonStorey.characterId = characterId;
		_PurchaseWithSoft_c__AnonStorey.callback = callback;
		_PurchaseWithSoft_c__AnonStorey._this = this;
		ServerUnlockCharacter.PackageInfo softCurrency = this.prices[_PurchaseWithSoft_c__AnonStorey.characterId].softCurrency;
		this.purchaseEquipment.PurchaseManual(softCurrency.packageId, CurrencyType.Soft, softCurrency.price, new Action<UserPurchaseResult>(_PurchaseWithSoft_c__AnonStorey.__m__0));
	}

	public void PurchaseWithToken(CharacterID characterId, Action<UserPurchaseResult> callback)
	{
		ServerUnlockCharacter._PurchaseWithToken_c__AnonStorey2 _PurchaseWithToken_c__AnonStorey = new ServerUnlockCharacter._PurchaseWithToken_c__AnonStorey2();
		_PurchaseWithToken_c__AnonStorey.characterId = characterId;
		_PurchaseWithToken_c__AnonStorey.callback = callback;
		_PurchaseWithToken_c__AnonStorey._this = this;
		this.purchaseEquipment.UnlockTokenPurchase(_PurchaseWithToken_c__AnonStorey.characterId, new Action<UserPurchaseResult>(_PurchaseWithToken_c__AnonStorey.__m__0));
	}
}
