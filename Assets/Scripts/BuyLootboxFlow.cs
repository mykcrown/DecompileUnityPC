// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BuyLootboxFlow : IBuyLootboxFlow
{
	private sealed class _onClickUnlockConfirmed_c__AnonStorey0
	{
		internal UserPurchaseResult result;

		internal BuyLootboxFlow _this;

		internal void __m__0()
		{
			this._this.handleUnlockResult(this.result);
		}
	}

	private static float MIN_LOADING_TIME = 1f;

	private DetailedUnlockFlowDialog dialog;

	private bool inProgress;

	private ulong packageId;

	private float startLoadTime;

	private Action<UserPurchaseResult> callback;

	private UserPurchaseResult result;

	[Inject]
	public IMainThreadTimer timer
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
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
	public IDialogController dialogController
	{
		get;
		set;
	}

	[Inject]
	public IPurchaseResponseHandler purchaseErrorHandler
	{
		get;
		set;
	}

	[Inject]
	public IBuyLootBox buyLootBox
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

	public void Start(ulong packageId, Action<UserPurchaseResult> callback)
	{
		if (this.inProgress)
		{
			throw new UnityException("Something activated a buy lootbox flow while the first was in progress, this should not be possible.");
		}
		this.inProgress = true;
		this.packageId = packageId;
		this.callback = callback;
		this.result = UserPurchaseResult.USER_CANCELLED;
		this.dialog = this.dialogController.ShowDetailedUnlockFlowDialog();
		LootBoxPackage boxToBuy = this.lootBoxesModel.GetBoxToBuy(packageId);
		string text = this.localization.GetText("ui.store.lootBox.title", boxToBuy.quantity.ToString());
		string extraDescription = null;
		string hardPriceString = this.localization.GetHardPriceString(boxToBuy.price);
		this.dialog.LoadCheckoutText(text, hardPriceString, extraDescription);
		this.dialog.LoadSuccess(text, hardPriceString, extraDescription);
		this.dialog.SwitchMode(DetailedUnlockFlowDialogModes.CHECKOUT);
		this.dialog.CloseCallback = new Action(this.cleanup);
		this.dialog.CheckoutConfirmCallback = new Action(this.onClickUnlockConfirmed);
	}

	private void onClickUnlockConfirmed()
	{
		CurrencyType currencyType = this.buyLootBox.GetCurrencyType(this.packageId);
		if (currencyType == CurrencyType.Hard && !this.purchaseErrorHandler.VerifySteam(this.dialog))
		{
			return;
		}
		this.dialog.SwitchMode(DetailedUnlockFlowDialogModes.LOADING);
		this.startLoadTime = Time.realtimeSinceStartup;
		this.dialog.LoadSpinnyText(this.localization.GetText("ui.store.lootBox.unlockLoading.title"));
		this.buyLootBox.Purchase(this.packageId, new Action<UserPurchaseResult>(this._onClickUnlockConfirmed_m__0));
	}

	private void proceedToResult(Action callback)
	{
		float num = Time.realtimeSinceStartup - this.startLoadTime;
		if (num >= BuyLootboxFlow.MIN_LOADING_TIME)
		{
			callback();
		}
		else
		{
			float num2 = BuyLootboxFlow.MIN_LOADING_TIME - num;
			this.timer.SetTimeout((int)(num2 * 1000f), callback);
		}
	}

	private void handleUnlockResult(UserPurchaseResult result)
	{
		this.result = result;
		if (result == UserPurchaseResult.SUCCESS)
		{
			this.showSuccess();
		}
		else
		{
			this.handleError(result);
		}
	}

	private void showSuccess()
	{
		this.dialog.ShowSuccess();
	}

	private void handleError(UserPurchaseResult result)
	{
		this.dialog.LoadStaticErrorText(this.localization.GetText("ui.store.lootBox.unlockError.confirm"));
		this.purchaseErrorHandler.HandleUnlockError(result, this.dialog, new Action(this.cleanup));
	}

	private void cleanup()
	{
		if (this.dialog != null)
		{
			this.dialog.Close();
		}
		this.dialog = null;
		this.inProgress = false;
		if (this.callback != null)
		{
			Action<UserPurchaseResult> action = this.callback;
			this.callback = null;
			action(this.result);
		}
	}

	private void _onClickUnlockConfirmed_m__0(UserPurchaseResult result)
	{
		BuyLootboxFlow._onClickUnlockConfirmed_c__AnonStorey0 _onClickUnlockConfirmed_c__AnonStorey = new BuyLootboxFlow._onClickUnlockConfirmed_c__AnonStorey0();
		_onClickUnlockConfirmed_c__AnonStorey.result = result;
		_onClickUnlockConfirmed_c__AnonStorey._this = this;
		this.proceedToResult(new Action(_onClickUnlockConfirmed_c__AnonStorey.__m__0));
	}
}
