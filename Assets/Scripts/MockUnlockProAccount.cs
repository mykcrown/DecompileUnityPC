// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;

public class MockUnlockProAccount : IUnlockProAccount
{
	private sealed class _Purchase_c__AnonStorey0
	{
		internal Action<UserPurchaseResult> callback;

		internal MockUnlockProAccount _this;

		internal void __m__0()
		{
			this._this.userProAccountUnlockModel.SetUnlocked();
			this.callback(UserPurchaseResult.SUCCESS);
		}
	}

	private sealed class _PurchaseFoundersPack_c__AnonStorey1
	{
		internal Action<UserPurchaseResult> callback;

		internal MockUnlockProAccount _this;

		internal void __m__0()
		{
			this._this.userProAccountUnlockModel.SetUnlocked();
			this.callback(UserPurchaseResult.SUCCESS);
		}
	}

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

	public void Purchase(Action<UserPurchaseResult> callback)
	{
		MockUnlockProAccount._Purchase_c__AnonStorey0 _Purchase_c__AnonStorey = new MockUnlockProAccount._Purchase_c__AnonStorey0();
		_Purchase_c__AnonStorey.callback = callback;
		_Purchase_c__AnonStorey._this = this;
		this.timer.SetTimeout(50, new Action(_Purchase_c__AnonStorey.__m__0));
	}

	public void PurchaseFoundersPack(Action<UserPurchaseResult> callback)
	{
		MockUnlockProAccount._PurchaseFoundersPack_c__AnonStorey1 _PurchaseFoundersPack_c__AnonStorey = new MockUnlockProAccount._PurchaseFoundersPack_c__AnonStorey1();
		_PurchaseFoundersPack_c__AnonStorey.callback = callback;
		_PurchaseFoundersPack_c__AnonStorey._this = this;
		this.timer.SetTimeout(50, new Action(_PurchaseFoundersPack_c__AnonStorey.__m__0));
	}

	public int GetPrice()
	{
		return 6524;
	}

	public string GetPriceString()
	{
		float price = (float)this.GetPrice();
		return this.localization.GetHardPriceString(price);
	}

	public int GetFoundersPackPrice()
	{
		return 3999;
	}

	public string GetFoundersPackPriceString()
	{
		float price = (float)this.GetFoundersPackPrice();
		return this.localization.GetHardPriceString(price);
	}

	public void SetPackageId(ulong id)
	{
	}

	public void SetPrice(ulong price, CurrencyType currencyType)
	{
	}

	public void SetFoundersPackageId(ulong id)
	{
	}

	public void SetFoundersPrice(ulong price, CurrencyType currencyType)
	{
	}

	public CurrencyType GetCurrencyType()
	{
		return CurrencyType.Hard;
	}

	public CurrencyType GetFoundersCurrencyType()
	{
		return CurrencyType.Hard;
	}
}
