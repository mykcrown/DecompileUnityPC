// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UnlockCharacterFlow : IUnlockCharacterFlow
{
	private bool inProgress;

	private CharacterID characterId;

	private UnlockFlowDialog dialog;

	private Action closeCallback;

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
	public IUnlockCharacter unlockCharacter
	{
		get;
		set;
	}

	[Inject]
	public ICharacterDataHelper characterDataHelper
	{
		get;
		set;
	}

	[Inject]
	public IUIAdapter uiAdapter
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
	public ISignalBus signalBus
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
		UnlockFlowDialog expr_DC = this.dialog;
		expr_DC.CloseCallback = (Action)Delegate.Combine(expr_DC.CloseCallback, new Action(this.cleanup));
		this.closeCallback = closeCallback;
		this.signalBus.AddListener(UIManager.SCREEN_CLOSED, new Action(this.cleanup));
	}

	private void onClickUnlockConfirmed()
	{
		this.dialog.SwitchMode(UnlockFlowDialogModes.LOADING);
		this.unlockCharacter.PurchaseWithSoft(this.characterId, new Action<UserPurchaseResult>(this._onClickUnlockConfirmed_m__0));
	}

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

	private void showSuccess()
	{
		this.dialog.ShowSuccess(CharacterID.None);
	}

	private void handleUnlockError(UserPurchaseResult result)
	{
		this.purchaseErrorHandler.HandleUnlockError(result, this.dialog, new Action(this.cleanup));
	}

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

	private void _onClickUnlockConfirmed_m__0(UserPurchaseResult result)
	{
		this.handleUnlockResult(result);
	}
}
