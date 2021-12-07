// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;

public class ServerUnlockProAccount : IUnlockProAccount
{
	private sealed class _Purchase_c__AnonStorey0
	{
		internal Action<UserPurchaseResult> callback;

		internal ServerUnlockProAccount _this;

		internal void __m__0(UserPurchaseResult result)
		{
			if (result == UserPurchaseResult.SUCCESS)
			{
				this._this.userProAccountUnlockModel.SetUnlocked();
			}
			this.callback(result);
		}
	}

	private sealed class _PurchaseFoundersPack_c__AnonStorey1
	{
		internal Action<UserPurchaseResult> callback;

		internal ServerUnlockProAccount _this;

		internal void __m__0(UserPurchaseResult result)
		{
			if (result == UserPurchaseResult.SUCCESS)
			{
				this._this.userProAccountUnlockModel.SetUnlocked();
			}
			this.callback(result);
		}
	}

	private ulong packageId;

	private ulong price;

	private CurrencyType currencyType;

	private ulong foundersPackageId;

	private ulong foundersPrice;

	private CurrencyType foundersCurrencyType;

	[Inject]
	public IUserProAccountUnlockedModel userProAccountUnlockModel
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

	public void Purchase(Action<UserPurchaseResult> callback)
	{
		ServerUnlockProAccount._Purchase_c__AnonStorey0 _Purchase_c__AnonStorey = new ServerUnlockProAccount._Purchase_c__AnonStorey0();
		_Purchase_c__AnonStorey.callback = callback;
		_Purchase_c__AnonStorey._this = this;
		this.purchaseEquipment.PurchaseManual(this.packageId, this.currencyType, this.price, new Action<UserPurchaseResult>(_Purchase_c__AnonStorey.__m__0));
	}

	public void PurchaseFoundersPack(Action<UserPurchaseResult> callback)
	{
		ServerUnlockProAccount._PurchaseFoundersPack_c__AnonStorey1 _PurchaseFoundersPack_c__AnonStorey = new ServerUnlockProAccount._PurchaseFoundersPack_c__AnonStorey1();
		_PurchaseFoundersPack_c__AnonStorey.callback = callback;
		_PurchaseFoundersPack_c__AnonStorey._this = this;
		this.purchaseEquipment.PurchaseManual(this.foundersPackageId, this.foundersCurrencyType, this.foundersPrice, new Action<UserPurchaseResult>(_PurchaseFoundersPack_c__AnonStorey.__m__0));
	}

	public int GetPrice()
	{
		return (int)this.price;
	}

	public string GetPriceString()
	{
		float num = (float)this.GetPrice();
		return this.localization.GetHardPriceString(num);
	}

	public float GetFoundersPackPrice()
	{
		return this.foundersPrice;
	}

	public string GetFoundersPackPriceString()
	{
		float foundersPackPrice = this.GetFoundersPackPrice();
		return this.localization.GetHardPriceString(foundersPackPrice);
	}

	public void SetPrice(ulong price, CurrencyType currencyType)
	{
		this.price = price;
		this.currencyType = currencyType;
	}

	public void SetPackageId(ulong id)
	{
		this.packageId = id;
	}

	public void SetFoundersPackageId(ulong id)
	{
		this.foundersPackageId = id;
	}

	public void SetFoundersPrice(ulong price, CurrencyType currencyType)
	{
		this.foundersPrice = price;
		this.foundersCurrencyType = currencyType;
	}

	public CurrencyType GetCurrencyType()
	{
		return this.currencyType;
	}

	public CurrencyType GetFoundersCurrencyType()
	{
		return this.foundersCurrencyType;
	}
}
