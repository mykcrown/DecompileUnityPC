using System;
using UnityEngine;

// Token: 0x020009E8 RID: 2536
public class UnlockCharacterFlow : IUnlockCharacterFlow
{
	// Token: 0x17001140 RID: 4416
	// (get) Token: 0x06004805 RID: 18437 RVA: 0x001387C1 File Offset: 0x00136BC1
	// (set) Token: 0x06004806 RID: 18438 RVA: 0x001387C9 File Offset: 0x00136BC9
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x17001141 RID: 4417
	// (get) Token: 0x06004807 RID: 18439 RVA: 0x001387D2 File Offset: 0x00136BD2
	// (set) Token: 0x06004808 RID: 18440 RVA: 0x001387DA File Offset: 0x00136BDA
	[Inject]
	public IDialogController dialogController { get; set; }

	// Token: 0x17001142 RID: 4418
	// (get) Token: 0x06004809 RID: 18441 RVA: 0x001387E3 File Offset: 0x00136BE3
	// (set) Token: 0x0600480A RID: 18442 RVA: 0x001387EB File Offset: 0x00136BEB
	[Inject]
	public IUnlockCharacter unlockCharacter { get; set; }

	// Token: 0x17001143 RID: 4419
	// (get) Token: 0x0600480B RID: 18443 RVA: 0x001387F4 File Offset: 0x00136BF4
	// (set) Token: 0x0600480C RID: 18444 RVA: 0x001387FC File Offset: 0x00136BFC
	[Inject]
	public ICharacterDataHelper characterDataHelper { get; set; }

	// Token: 0x17001144 RID: 4420
	// (get) Token: 0x0600480D RID: 18445 RVA: 0x00138805 File Offset: 0x00136C05
	// (set) Token: 0x0600480E RID: 18446 RVA: 0x0013880D File Offset: 0x00136C0D
	[Inject]
	public IUIAdapter uiAdapter { get; set; }

	// Token: 0x17001145 RID: 4421
	// (get) Token: 0x0600480F RID: 18447 RVA: 0x00138816 File Offset: 0x00136C16
	// (set) Token: 0x06004810 RID: 18448 RVA: 0x0013881E File Offset: 0x00136C1E
	[Inject]
	public IPurchaseResponseHandler purchaseErrorHandler { get; set; }

	// Token: 0x17001146 RID: 4422
	// (get) Token: 0x06004811 RID: 18449 RVA: 0x00138827 File Offset: 0x00136C27
	// (set) Token: 0x06004812 RID: 18450 RVA: 0x0013882F File Offset: 0x00136C2F
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17001147 RID: 4423
	// (get) Token: 0x06004813 RID: 18451 RVA: 0x00138838 File Offset: 0x00136C38
	// (set) Token: 0x06004814 RID: 18452 RVA: 0x00138840 File Offset: 0x00136C40
	[Inject]
	public IUserCurrencyModel userCurrencyModel { get; set; }

	// Token: 0x06004815 RID: 18453 RVA: 0x0013884C File Offset: 0x00136C4C
	public void Start(CharacterID characterId, Action closeCallback)
	{
		if (this.inProgress)
		{
			throw new UnityException("Something activated an unlock character flow while the first was in progress, this should not be possible.");
		}
		if (this.userCurrencyModel.Spectra < this.unlockCharacter.GetSoftPrice(characterId))
		{
			this.dialogController.ShowOneButtonDialog(this.localization.GetText("ui.store.characters.unlockError.money.title"), this.localization.GetText("ui.store.characters.unlockError.money.body"), this.localization.GetText("ui.store.characters.unlockError.money.confirm"), WindowTransition.STANDARD_FADE, false, default(AudioData));
			return;
		}
		this.inProgress = true;
		this.characterId = characterId;
		string text = this.localization.GetText("ui.store.characters.unlockCharConfirm.title");
		string text2 = this.localization.GetText("ui.store.characters.unlockCharConfirm.body", this.characterDataHelper.GetDisplayName(characterId), this.unlockCharacter.GetSoftPriceString(characterId));
		string text3 = this.localization.GetText("ui.store.characters.unlockCharConfirm.confirm");
		string text4 = this.localization.GetText("ui.store.characters.unlockCharConfirm.cancel");
		this.startDialog(characterId, closeCallback, text, text2, text3, text4, false, 0f);
	}

	// Token: 0x06004816 RID: 18454 RVA: 0x00138954 File Offset: 0x00136D54
	public void StartWithTimer(CharacterID characterId, Action closeCallback, float endTime)
	{
		if (this.inProgress)
		{
			throw new UnityException("Something activated an unlock character flow while the first was in progress, this should not be possible.");
		}
		if (this.userCurrencyModel.Spectra < this.unlockCharacter.GetSoftPrice(characterId))
		{
			this.dialogController.ShowOneButtonDialog(this.localization.GetText("ui.store.characters.unlockError.money.title"), this.localization.GetText("ui.store.characters.unlockError.money.body"), this.localization.GetText("ui.store.characters.unlockError.money.confirm"), WindowTransition.STANDARD_FADE, false, default(AudioData));
			return;
		}
		this.inProgress = true;
		this.characterId = characterId;
		string text = this.localization.GetText("ui.store.characters.unlockCharConfirm.timedTitle");
		string text2 = this.localization.GetText("ui.store.characters.unlockCharConfirm.body", this.characterDataHelper.GetDisplayName(characterId), this.unlockCharacter.GetSoftPriceString(characterId));
		string text3 = this.localization.GetText("ui.store.characters.unlockCharConfirm.confirm");
		string text4 = this.localization.GetText("ui.store.characters.unlockCharConfirm.cancel");
		this.startDialog(characterId, closeCallback, text, text2, text3, text4, true, endTime);
	}

	// Token: 0x06004817 RID: 18455 RVA: 0x00138A58 File Offset: 0x00136E58
	public void StartAsPrompt(CharacterID characterId, Action closeCallback)
	{
		if (this.inProgress)
		{
			throw new UnityException("Something activated an unlock character flow while the first was in progress, this should not be possible.");
		}
		if (this.userCurrencyModel.Spectra < this.unlockCharacter.GetSoftPrice(characterId))
		{
			this.dialogController.ShowOneButtonDialog(this.localization.GetText("ui.store.characters.unlockError.money.title"), this.localization.GetText("ui.store.characters.unlockError.money.body"), this.localization.GetText("ui.store.characters.unlockError.money.confirm"), WindowTransition.STANDARD_FADE, false, default(AudioData));
			return;
		}
		this.inProgress = true;
		this.characterId = characterId;
		string text = this.localization.GetText("ui.store.characters.unlockCharPrompt.title");
		string text2 = this.localization.GetText("ui.store.characters.unlockCharPrompt.body", this.characterDataHelper.GetDisplayName(characterId), this.unlockCharacter.GetSoftPriceString(characterId));
		string text3 = this.localization.GetText("ui.store.characters.unlockCharPrompt.confirm");
		string text4 = this.localization.GetText("ui.store.characters.unlockCharPrompt.cancel");
		this.startDialog(characterId, closeCallback, text, text2, text3, text4, false, 0f);
	}

	// Token: 0x06004818 RID: 18456 RVA: 0x00138B60 File Offset: 0x00136F60
	private void startDialog(CharacterID characterId, Action closeCallback, string title, string body, string confirmText, string cancelText, bool displayTimer, float endtime = 0f)
	{
		this.dialog = this.dialogController.ShowUnlockFlowDialog();
		string replace = "character";
		string displayName = this.characterDataHelper.GetDisplayName(characterId);
		this.dialog.LoadConfirmationText(title, body, confirmText, cancelText, displayTimer, endtime);
		this.dialog.LoadSpinnyText(this.localization.GetText("ui.store.characters.unlockLoading.title"));
		this.dialog.LoadStaticErrorText(this.localization.GetText("ui.store.characters.unlockError.confirm"));
		this.dialog.LoadSuccess(this.localization.GetText("ui.store.characters.unlockSuccess.title"), this.localization.GetText("ui.store.characters.unlockSuccess.body", replace, displayName), this.localization.GetText("ui.store.characters.unlockSuccess.confirm"), string.Empty, false);
		this.dialog.SwitchMode(UnlockFlowDialogModes.CONFIRM);
		this.dialog.ConfirmCallback = new Action(this.onClickUnlockConfirmed);
		UnlockFlowDialog unlockFlowDialog = this.dialog;
		unlockFlowDialog.CloseCallback = (Action)Delegate.Combine(unlockFlowDialog.CloseCallback, new Action(this.cleanup));
		this.closeCallback = closeCallback;
		this.signalBus.AddListener(UIManager.SCREEN_CLOSED, new Action(this.cleanup));
	}

	// Token: 0x06004819 RID: 18457 RVA: 0x00138C8D File Offset: 0x0013708D
	private void onClickUnlockConfirmed()
	{
		this.dialog.SwitchMode(UnlockFlowDialogModes.LOADING);
		this.unlockCharacter.PurchaseWithSoft(this.characterId, delegate(UserPurchaseResult result)
		{
			this.handleUnlockResult(result);
		});
	}

	// Token: 0x0600481A RID: 18458 RVA: 0x00138CB8 File Offset: 0x001370B8
	private void handleUnlockResult(UserPurchaseResult result)
	{
		if (result == UserPurchaseResult.SUCCESS)
		{
			this.showSuccess();
		}
		else
		{
			this.handleUnlockError(result);
		}
	}

	// Token: 0x0600481B RID: 18459 RVA: 0x00138CD2 File Offset: 0x001370D2
	private void showSuccess()
	{
		this.dialog.ShowSuccess(CharacterID.None);
	}

	// Token: 0x0600481C RID: 18460 RVA: 0x00138CE0 File Offset: 0x001370E0
	private void handleUnlockError(UserPurchaseResult result)
	{
		this.purchaseErrorHandler.HandleUnlockError(result, this.dialog, new Action(this.cleanup));
	}

	// Token: 0x0600481D RID: 18461 RVA: 0x00138D00 File Offset: 0x00137100
	private void cleanup()
	{
		this.dialog.Close();
		this.dialog = null;
		this.inProgress = false;
		if (this.closeCallback != null)
		{
			this.closeCallback();
		}
		this.signalBus.RemoveListener(UIManager.SCREEN_CLOSED, new Action(this.cleanup));
	}

	// Token: 0x04002FB6 RID: 12214
	private bool inProgress;

	// Token: 0x04002FB7 RID: 12215
	private CharacterID characterId;

	// Token: 0x04002FB8 RID: 12216
	private UnlockFlowDialog dialog;

	// Token: 0x04002FB9 RID: 12217
	private Action closeCallback;
}
