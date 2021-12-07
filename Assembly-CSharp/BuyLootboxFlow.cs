using System;
using UnityEngine;

// Token: 0x02000A0C RID: 2572
public class BuyLootboxFlow : IBuyLootboxFlow
{
	// Token: 0x170011C7 RID: 4551
	// (get) Token: 0x06004A96 RID: 19094 RVA: 0x0013F9BE File Offset: 0x0013DDBE
	// (set) Token: 0x06004A97 RID: 19095 RVA: 0x0013F9C6 File Offset: 0x0013DDC6
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x170011C8 RID: 4552
	// (get) Token: 0x06004A98 RID: 19096 RVA: 0x0013F9CF File Offset: 0x0013DDCF
	// (set) Token: 0x06004A99 RID: 19097 RVA: 0x0013F9D7 File Offset: 0x0013DDD7
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x170011C9 RID: 4553
	// (get) Token: 0x06004A9A RID: 19098 RVA: 0x0013F9E0 File Offset: 0x0013DDE0
	// (set) Token: 0x06004A9B RID: 19099 RVA: 0x0013F9E8 File Offset: 0x0013DDE8
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x170011CA RID: 4554
	// (get) Token: 0x06004A9C RID: 19100 RVA: 0x0013F9F1 File Offset: 0x0013DDF1
	// (set) Token: 0x06004A9D RID: 19101 RVA: 0x0013F9F9 File Offset: 0x0013DDF9
	[Inject]
	public IDialogController dialogController { get; set; }

	// Token: 0x170011CB RID: 4555
	// (get) Token: 0x06004A9E RID: 19102 RVA: 0x0013FA02 File Offset: 0x0013DE02
	// (set) Token: 0x06004A9F RID: 19103 RVA: 0x0013FA0A File Offset: 0x0013DE0A
	[Inject]
	public IPurchaseResponseHandler purchaseErrorHandler { get; set; }

	// Token: 0x170011CC RID: 4556
	// (get) Token: 0x06004AA0 RID: 19104 RVA: 0x0013FA13 File Offset: 0x0013DE13
	// (set) Token: 0x06004AA1 RID: 19105 RVA: 0x0013FA1B File Offset: 0x0013DE1B
	[Inject]
	public IBuyLootBox buyLootBox { get; set; }

	// Token: 0x170011CD RID: 4557
	// (get) Token: 0x06004AA2 RID: 19106 RVA: 0x0013FA24 File Offset: 0x0013DE24
	// (set) Token: 0x06004AA3 RID: 19107 RVA: 0x0013FA2C File Offset: 0x0013DE2C
	[Inject]
	public ILootBoxesModel lootBoxesModel { get; set; }

	// Token: 0x06004AA4 RID: 19108 RVA: 0x0013FA38 File Offset: 0x0013DE38
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

	// Token: 0x06004AA5 RID: 19109 RVA: 0x0013FB24 File Offset: 0x0013DF24
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
		this.buyLootBox.Purchase(this.packageId, delegate(UserPurchaseResult result)
		{
			this.proceedToResult(delegate
			{
				this.handleUnlockResult(result);
			});
		});
	}

	// Token: 0x06004AA6 RID: 19110 RVA: 0x0013FBB0 File Offset: 0x0013DFB0
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

	// Token: 0x06004AA7 RID: 19111 RVA: 0x0013FBFC File Offset: 0x0013DFFC
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

	// Token: 0x06004AA8 RID: 19112 RVA: 0x0013FC1D File Offset: 0x0013E01D
	private void showSuccess()
	{
		this.dialog.ShowSuccess();
	}

	// Token: 0x06004AA9 RID: 19113 RVA: 0x0013FC2A File Offset: 0x0013E02A
	private void handleError(UserPurchaseResult result)
	{
		this.dialog.LoadStaticErrorText(this.localization.GetText("ui.store.lootBox.unlockError.confirm"));
		this.purchaseErrorHandler.HandleUnlockError(result, this.dialog, new Action(this.cleanup));
	}

	// Token: 0x06004AAA RID: 19114 RVA: 0x0013FC68 File Offset: 0x0013E068
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

	// Token: 0x04003111 RID: 12561
	private static float MIN_LOADING_TIME = 1f;

	// Token: 0x04003119 RID: 12569
	private DetailedUnlockFlowDialog dialog;

	// Token: 0x0400311A RID: 12570
	private bool inProgress;

	// Token: 0x0400311B RID: 12571
	private ulong packageId;

	// Token: 0x0400311C RID: 12572
	private float startLoadTime;

	// Token: 0x0400311D RID: 12573
	private Action<UserPurchaseResult> callback;

	// Token: 0x0400311E RID: 12574
	private UserPurchaseResult result;
}
