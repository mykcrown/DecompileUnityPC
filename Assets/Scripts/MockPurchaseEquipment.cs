// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MockPurchaseEquipment : IUserPurchaseEquipment
{
	private sealed class _Purchase_c__AnonStorey0
	{
		internal EquipmentID itemId;

		internal Action<UserPurchaseResult> callback;

		internal MockPurchaseEquipment _this;

		internal void __m__0()
		{
			this._this.userInventory.AddItem(this.itemId, true);
			this.callback(UserPurchaseResult.SUCCESS);
		}
	}

	private sealed class _PurchaseManual_c__AnonStorey1
	{
		internal ulong packageId;

		internal CurrencyType currencyType;

		internal ulong price;

		internal Action<UserPurchaseResult> callback;

		internal MockPurchaseEquipment _this;

		internal void __m__0()
		{
			LootBoxPackage boxToBuy = this._this.lootBoxesModel.GetBoxToBuy(this.packageId);
			if (boxToBuy != null)
			{
				this._this.userLootboxesModel.Add((int)boxToBuy.lootBoxId, (int)boxToBuy.quantity);
			}
			else
			{
				UnityEngine.Debug.LogWarning("Offline manual purchase does not grant item, but will clear success");
			}
			if (this.currencyType == CurrencyType.Soft)
			{
				this._this.userCurrencyModel.Spectra -= (int)this.price;
			}
			this.callback(UserPurchaseResult.SUCCESS);
		}
	}

	private sealed class _UnlockTokenPurchase_c__AnonStorey2
	{
		internal Action<UserPurchaseResult> callback;

		internal MockPurchaseEquipment _this;

		internal void __m__0()
		{
			this._this.userCurrencyModel.CharacterUnlockTokens--;
			this.callback(UserPurchaseResult.SUCCESS);
		}
	}

	[Inject]
	public IUserInventory userInventory
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
	public ILootBoxesModel lootBoxesModel
	{
		get;
		set;
	}

	[Inject]
	public IUserLootboxesModel userLootboxesModel
	{
		get;
		set;
	}

	[Inject]
	public IUserCurrencyModel userCurrencyModel
	{
		get;
		set;
	}

	public void Purchase(EquipmentID itemId, Action<UserPurchaseResult> callback)
	{
		MockPurchaseEquipment._Purchase_c__AnonStorey0 _Purchase_c__AnonStorey = new MockPurchaseEquipment._Purchase_c__AnonStorey0();
		_Purchase_c__AnonStorey.itemId = itemId;
		_Purchase_c__AnonStorey.callback = callback;
		_Purchase_c__AnonStorey._this = this;
		this.timer.SetTimeout(100, new Action(_Purchase_c__AnonStorey.__m__0));
	}

	public void PurchaseManual(ulong packageId, CurrencyType currencyType, ulong price, Action<UserPurchaseResult> callback)
	{
		MockPurchaseEquipment._PurchaseManual_c__AnonStorey1 _PurchaseManual_c__AnonStorey = new MockPurchaseEquipment._PurchaseManual_c__AnonStorey1();
		_PurchaseManual_c__AnonStorey.packageId = packageId;
		_PurchaseManual_c__AnonStorey.currencyType = currencyType;
		_PurchaseManual_c__AnonStorey.price = price;
		_PurchaseManual_c__AnonStorey.callback = callback;
		_PurchaseManual_c__AnonStorey._this = this;
		this.timer.SetTimeout(100, new Action(_PurchaseManual_c__AnonStorey.__m__0));
	}

	public void UnlockTokenPurchase(CharacterID character, Action<UserPurchaseResult> callback)
	{
		MockPurchaseEquipment._UnlockTokenPurchase_c__AnonStorey2 _UnlockTokenPurchase_c__AnonStorey = new MockPurchaseEquipment._UnlockTokenPurchase_c__AnonStorey2();
		_UnlockTokenPurchase_c__AnonStorey.callback = callback;
		_UnlockTokenPurchase_c__AnonStorey._this = this;
		this.timer.SetTimeout(100, new Action(_UnlockTokenPurchase_c__AnonStorey.__m__0));
	}
}
