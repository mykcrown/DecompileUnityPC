// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UnlockEquipmentFlow : IUnlockEquipmentFlow
{
	private sealed class _onClickUnlockConfirmed_c__AnonStorey0
	{
		private sealed class _onClickUnlockConfirmed_c__AnonStorey1
		{
			internal UserPurchaseResult result;

			internal UnlockEquipmentFlow._onClickUnlockConfirmed_c__AnonStorey0 __f__ref_0;

			internal void __m__0()
			{
				this.__f__ref_0._this.inputBlocker.Release(this.__f__ref_0.block);
				this.__f__ref_0._this.handleUnlockResult(this.result);
			}
		}

		internal InputBlock block;

		internal UnlockEquipmentFlow _this;

		internal void __m__0(UserPurchaseResult result)
		{
			UnlockEquipmentFlow._onClickUnlockConfirmed_c__AnonStorey0._onClickUnlockConfirmed_c__AnonStorey1 _onClickUnlockConfirmed_c__AnonStorey = new UnlockEquipmentFlow._onClickUnlockConfirmed_c__AnonStorey0._onClickUnlockConfirmed_c__AnonStorey1();
			_onClickUnlockConfirmed_c__AnonStorey.__f__ref_0 = this;
			_onClickUnlockConfirmed_c__AnonStorey.result = result;
			this._this.timer.CancelTimeout(new Action(this._this.showLoadingDialog));
			this._this.proceedToResult(new Action(_onClickUnlockConfirmed_c__AnonStorey.__m__0));
		}
	}

	private static float MIN_LOADING_TIME = 1f;

	private bool inProgress;

	private EquippableItem item;

	private UnlockFlowDialog dialog;

	private Action closeCallback;

	private float startLoadTime;

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
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public IEquipmentModel equipmentModel
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

	[Inject]
	public IServerConnectionManager serverManager
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

	[Inject]
	public IPurchaseResponseHandler purchaseErrorHandler
	{
		get;
		set;
	}

	[Inject]
	public IUserCharacterUnlockModel characterUnlockModel
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
	public GameDataManager gameDataManger
	{
		get;
		set;
	}

	[Inject]
	public IInputBlocker inputBlocker
	{
		get;
		set;
	}

	[Inject]
	public IEquipFlow equipFlow
	{
		get;
		set;
	}

	public void Start(EquippableItem item, Action closeCallback, Action equipNowCallback)
	{
		this.startDialog(item, closeCallback, equipNowCallback, false, 0f);
	}

	public void StartWithTimer(EquippableItem item, Action closeCallback, Action equipNowCallback, float endTime)
	{
		this.startDialog(item, closeCallback, equipNowCallback, true, endTime);
	}

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
		UnlockFlowDialog expr_2A0 = this.dialog;
		expr_2A0.EquipCallback = (Action)Delegate.Combine(expr_2A0.EquipCallback, equipNowCallback);
		UnlockFlowDialog expr_2BC = this.dialog;
		expr_2BC.CloseCallback = (Action)Delegate.Combine(expr_2BC.CloseCallback, new Action(this.cleanup));
	}

	private void onClickUnlockConfirmed()
	{
		UnlockEquipmentFlow._onClickUnlockConfirmed_c__AnonStorey0 _onClickUnlockConfirmed_c__AnonStorey = new UnlockEquipmentFlow._onClickUnlockConfirmed_c__AnonStorey0();
		_onClickUnlockConfirmed_c__AnonStorey._this = this;
		_onClickUnlockConfirmed_c__AnonStorey.block = this.inputBlocker.Request();
		this.startLoadTime = 0f;
		this.timer.SetTimeout(150, new Action(this.showLoadingDialog));
		this.purchaseEquipment.Purchase(this.item.id, new Action<UserPurchaseResult>(_onClickUnlockConfirmed_c__AnonStorey.__m__0));
	}

	private void showLoadingDialog()
	{
		this.timer.CancelTimeout(new Action(this.showLoadingDialog));
		this.startLoadTime = Time.realtimeSinceStartup;
		this.dialog.SwitchMode(UnlockFlowDialogModes.LOADING);
	}

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

	private void showSuccess()
	{
		this.dialog.ShowSuccess(this.item.character);
	}

	private void handleUnlockError(UserPurchaseResult result)
	{
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
		if (this.closeCallback != null)
		{
			this.closeCallback();
		}
	}
}
