using System;
using UnityEngine;

// Token: 0x02000A04 RID: 2564
public class UnlockEquipmentFlow : IUnlockEquipmentFlow
{
	// Token: 0x170011AA RID: 4522
	// (get) Token: 0x06004A10 RID: 18960 RVA: 0x0013E50C File Offset: 0x0013C90C
	// (set) Token: 0x06004A11 RID: 18961 RVA: 0x0013E514 File Offset: 0x0013C914
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x170011AB RID: 4523
	// (get) Token: 0x06004A12 RID: 18962 RVA: 0x0013E51D File Offset: 0x0013C91D
	// (set) Token: 0x06004A13 RID: 18963 RVA: 0x0013E525 File Offset: 0x0013C925
	[Inject]
	public IDialogController dialogController { get; set; }

	// Token: 0x170011AC RID: 4524
	// (get) Token: 0x06004A14 RID: 18964 RVA: 0x0013E52E File Offset: 0x0013C92E
	// (set) Token: 0x06004A15 RID: 18965 RVA: 0x0013E536 File Offset: 0x0013C936
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x170011AD RID: 4525
	// (get) Token: 0x06004A16 RID: 18966 RVA: 0x0013E53F File Offset: 0x0013C93F
	// (set) Token: 0x06004A17 RID: 18967 RVA: 0x0013E547 File Offset: 0x0013C947
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x170011AE RID: 4526
	// (get) Token: 0x06004A18 RID: 18968 RVA: 0x0013E550 File Offset: 0x0013C950
	// (set) Token: 0x06004A19 RID: 18969 RVA: 0x0013E558 File Offset: 0x0013C958
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x170011AF RID: 4527
	// (get) Token: 0x06004A1A RID: 18970 RVA: 0x0013E561 File Offset: 0x0013C961
	// (set) Token: 0x06004A1B RID: 18971 RVA: 0x0013E569 File Offset: 0x0013C969
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x170011B0 RID: 4528
	// (get) Token: 0x06004A1C RID: 18972 RVA: 0x0013E572 File Offset: 0x0013C972
	// (set) Token: 0x06004A1D RID: 18973 RVA: 0x0013E57A File Offset: 0x0013C97A
	[Inject]
	public IUserPurchaseEquipment purchaseEquipment { get; set; }

	// Token: 0x170011B1 RID: 4529
	// (get) Token: 0x06004A1E RID: 18974 RVA: 0x0013E583 File Offset: 0x0013C983
	// (set) Token: 0x06004A1F RID: 18975 RVA: 0x0013E58B File Offset: 0x0013C98B
	[Inject]
	public IServerConnectionManager serverManager { get; set; }

	// Token: 0x170011B2 RID: 4530
	// (get) Token: 0x06004A20 RID: 18976 RVA: 0x0013E594 File Offset: 0x0013C994
	// (set) Token: 0x06004A21 RID: 18977 RVA: 0x0013E59C File Offset: 0x0013C99C
	[Inject]
	public IUserCurrencyModel userCurrencyModel { get; set; }

	// Token: 0x170011B3 RID: 4531
	// (get) Token: 0x06004A22 RID: 18978 RVA: 0x0013E5A5 File Offset: 0x0013C9A5
	// (set) Token: 0x06004A23 RID: 18979 RVA: 0x0013E5AD File Offset: 0x0013C9AD
	[Inject]
	public IPurchaseResponseHandler purchaseErrorHandler { get; set; }

	// Token: 0x170011B4 RID: 4532
	// (get) Token: 0x06004A24 RID: 18980 RVA: 0x0013E5B6 File Offset: 0x0013C9B6
	// (set) Token: 0x06004A25 RID: 18981 RVA: 0x0013E5BE File Offset: 0x0013C9BE
	[Inject]
	public IUserCharacterUnlockModel characterUnlockModel { get; set; }

	// Token: 0x170011B5 RID: 4533
	// (get) Token: 0x06004A26 RID: 18982 RVA: 0x0013E5C7 File Offset: 0x0013C9C7
	// (set) Token: 0x06004A27 RID: 18983 RVA: 0x0013E5CF File Offset: 0x0013C9CF
	[Inject]
	public ICharacterDataHelper characterDataHelper { get; set; }

	// Token: 0x170011B6 RID: 4534
	// (get) Token: 0x06004A28 RID: 18984 RVA: 0x0013E5D8 File Offset: 0x0013C9D8
	// (set) Token: 0x06004A29 RID: 18985 RVA: 0x0013E5E0 File Offset: 0x0013C9E0
	[Inject]
	public GameDataManager gameDataManger { get; set; }

	// Token: 0x170011B7 RID: 4535
	// (get) Token: 0x06004A2A RID: 18986 RVA: 0x0013E5E9 File Offset: 0x0013C9E9
	// (set) Token: 0x06004A2B RID: 18987 RVA: 0x0013E5F1 File Offset: 0x0013C9F1
	[Inject]
	public IInputBlocker inputBlocker { get; set; }

	// Token: 0x170011B8 RID: 4536
	// (get) Token: 0x06004A2C RID: 18988 RVA: 0x0013E5FA File Offset: 0x0013C9FA
	// (set) Token: 0x06004A2D RID: 18989 RVA: 0x0013E602 File Offset: 0x0013CA02
	[Inject]
	public IEquipFlow equipFlow { get; set; }

	// Token: 0x06004A2E RID: 18990 RVA: 0x0013E60B File Offset: 0x0013CA0B
	public void Start(EquippableItem item, Action closeCallback, Action equipNowCallback)
	{
		this.startDialog(item, closeCallback, equipNowCallback, false, 0f);
	}

	// Token: 0x06004A2F RID: 18991 RVA: 0x0013E61C File Offset: 0x0013CA1C
	public void StartWithTimer(EquippableItem item, Action closeCallback, Action equipNowCallback, float endTime)
	{
		this.startDialog(item, closeCallback, equipNowCallback, true, endTime);
	}

	// Token: 0x06004A30 RID: 18992 RVA: 0x0013E62C File Offset: 0x0013CA2C
	private void startDialog(EquippableItem item, Action closeCallback, Action equipNowCallback, bool displayTimer, float endtime = 0f)
	{
		if (this.inProgress)
		{
			throw new UnityException("Something activated an unlock equipment flow while the first was in progress, this should not be possible.");
		}
		if (this.userCurrencyModel.Spectra < item.price)
		{
			this.dialogController.ShowOneButtonDialog(this.localization.GetText("ui.store.characters.unlockError.money.title"), this.localization.GetText("ui.store.characters.unlockError.money.body"), this.localization.GetText("ui.store.characters.unlockError.money.confirm"), WindowTransition.STANDARD_FADE, false, default(AudioData));
			return;
		}
		this.inProgress = true;
		this.item = item;
		string text;
		string text2;
		string text3;
		string text4;
		bool useEquipButton;
		if (this.characterUnlockModel.IsUnlocked(item.character) || !this.equipFlow.IsValid(item, item.character))
		{
			text = this.localization.GetText("ui.store.characters.unlockConfirm.title");
			text2 = this.localization.GetText("ui.store.characters.unlockConfirm.body", this.equipmentModel.GetLocalizedItemName(item), this.localization.GetSoftPriceString(item.price));
			text3 = this.localization.GetText("ui.store.characters.unlockConfirm.confirm");
			text4 = this.localization.GetText("ui.store.characters.unlockConfirm.cancel");
			useEquipButton = true;
		}
		else
		{
			text = this.localization.GetText("ui.store.characters.unlockConfirmWithoutCharacter.title");
			text2 = this.localization.GetText("ui.store.characters.unlockConfirmWithoutCharacter.body", this.characterDataHelper.GetDisplayName(item.character), this.equipmentModel.GetLocalizedItemName(item), this.localization.GetSoftPriceString(item.price));
			text3 = this.localization.GetText("ui.store.characters.unlockConfirmWithoutCharacter.confirm");
			text4 = this.localization.GetText("ui.store.characters.unlockConfirmWithoutCharacter.cancel");
			useEquipButton = false;
		}
		this.dialog = this.dialogController.ShowUnlockFlowDialog();
		string replace = this.localization.GetText("equipType.singular." + item.type).ToLower();
		string localizedItemName = this.equipmentModel.GetLocalizedItemName(item);
		this.dialog.LoadConfirmationText(text, text2, text3, text4, displayTimer, endtime);
		this.dialog.LoadSpinnyText(this.localization.GetText("ui.store.characters.unlockLoading.title"));
		this.dialog.LoadStaticErrorText(this.localization.GetText("ui.store.characters.unlockError.confirm"));
		this.dialog.LoadSuccess(this.localization.GetText("ui.store.characters.unlockSuccess.title"), this.localization.GetText("ui.store.characters.unlockSuccess.body", replace, localizedItemName), this.localization.GetText("ui.store.characters.unlockSuccess.confirm"), this.localization.GetText("ui.store.characters.unlockSuccess.equip"), useEquipButton);
		this.dialog.SwitchMode(UnlockFlowDialogModes.CONFIRM);
		this.dialog.ConfirmCallback = new Action(this.onClickUnlockConfirmed);
		this.closeCallback = closeCallback;
		UnlockFlowDialog unlockFlowDialog = this.dialog;
		unlockFlowDialog.EquipCallback = (Action)Delegate.Combine(unlockFlowDialog.EquipCallback, equipNowCallback);
		UnlockFlowDialog unlockFlowDialog2 = this.dialog;
		unlockFlowDialog2.CloseCallback = (Action)Delegate.Combine(unlockFlowDialog2.CloseCallback, new Action(this.cleanup));
	}

	// Token: 0x06004A31 RID: 18993 RVA: 0x0013E918 File Offset: 0x0013CD18
	private void onClickUnlockConfirmed()
	{
		UnlockEquipmentFlow.<onClickUnlockConfirmed>c__AnonStorey0 <onClickUnlockConfirmed>c__AnonStorey = new UnlockEquipmentFlow.<onClickUnlockConfirmed>c__AnonStorey0();
		<onClickUnlockConfirmed>c__AnonStorey.$this = this;
		<onClickUnlockConfirmed>c__AnonStorey.block = this.inputBlocker.Request();
		this.startLoadTime = 0f;
		this.timer.SetTimeout(150, new Action(this.showLoadingDialog));
		this.purchaseEquipment.Purchase(this.item.id, delegate(UserPurchaseResult result)
		{
			<onClickUnlockConfirmed>c__AnonStorey.$this.timer.CancelTimeout(new Action(<onClickUnlockConfirmed>c__AnonStorey.$this.showLoadingDialog));
			<onClickUnlockConfirmed>c__AnonStorey.$this.proceedToResult(delegate
			{
				<onClickUnlockConfirmed>c__AnonStorey.inputBlocker.Release(<onClickUnlockConfirmed>c__AnonStorey.block);
				<onClickUnlockConfirmed>c__AnonStorey.handleUnlockResult(result);
			});
		});
	}

	// Token: 0x06004A32 RID: 18994 RVA: 0x0013E98C File Offset: 0x0013CD8C
	private void showLoadingDialog()
	{
		this.timer.CancelTimeout(new Action(this.showLoadingDialog));
		this.startLoadTime = Time.realtimeSinceStartup;
		this.dialog.SwitchMode(UnlockFlowDialogModes.LOADING);
	}

	// Token: 0x06004A33 RID: 18995 RVA: 0x0013E9BC File Offset: 0x0013CDBC
	private void handleUnlockResult(UserPurchaseResult result)
	{
		if (result == UserPurchaseResult.SUCCESS)
		{
			this.showSuccess();
			this.signalBus.Dispatch(UserInventoryModel.UPDATED);
		}
		else
		{
			this.handleUnlockError(result);
		}
	}

	// Token: 0x06004A34 RID: 18996 RVA: 0x0013E9E8 File Offset: 0x0013CDE8
	private void proceedToResult(Action callback)
	{
		float num = Time.realtimeSinceStartup - this.startLoadTime;
		if (num >= UnlockEquipmentFlow.MIN_LOADING_TIME)
		{
			callback();
		}
		else
		{
			float num2 = UnlockEquipmentFlow.MIN_LOADING_TIME - num;
			this.timer.SetTimeout((int)(num2 * 1000f), callback);
		}
	}

	// Token: 0x06004A35 RID: 18997 RVA: 0x0013EA34 File Offset: 0x0013CE34
	private void showSuccess()
	{
		this.dialog.ShowSuccess(this.item.character);
	}

	// Token: 0x06004A36 RID: 18998 RVA: 0x0013EA4C File Offset: 0x0013CE4C
	private void handleUnlockError(UserPurchaseResult result)
	{
		this.purchaseErrorHandler.HandleUnlockError(result, this.dialog, new Action(this.cleanup));
	}

	// Token: 0x06004A37 RID: 18999 RVA: 0x0013EA6C File Offset: 0x0013CE6C
	private void cleanup()
	{
		if (this.dialog != null)
		{
			this.dialog.Close();
		}
		this.dialog = null;
		this.inProgress = false;
		if (this.closeCallback != null)
		{
			this.closeCallback();
		}
	}

	// Token: 0x040030BA RID: 12474
	private static float MIN_LOADING_TIME = 1f;

	// Token: 0x040030CA RID: 12490
	private bool inProgress;

	// Token: 0x040030CB RID: 12491
	private EquippableItem item;

	// Token: 0x040030CC RID: 12492
	private UnlockFlowDialog dialog;

	// Token: 0x040030CD RID: 12493
	private Action closeCallback;

	// Token: 0x040030CE RID: 12494
	private float startLoadTime;
}
