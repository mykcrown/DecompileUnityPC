// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UnlockProAccountFlow : IUnlockProAccountFlow
{
	private sealed class _onClickUnlockConfirmed_c__AnonStorey0
	{
		private sealed class _onClickUnlockConfirmed_c__AnonStorey1
		{
			internal UserPurchaseResult result;

			internal UnlockProAccountFlow._onClickUnlockConfirmed_c__AnonStorey0 __f__ref_0;

			internal void __m__0()
			{
				this.__f__ref_0._this.handleUnlockResult(this.result);
			}
		}

		internal long time;

		internal UnlockProAccountFlow _this;

		internal void __m__0(UserPurchaseResult result)
		{
			UnlockProAccountFlow._onClickUnlockConfirmed_c__AnonStorey0._onClickUnlockConfirmed_c__AnonStorey1 _onClickUnlockConfirmed_c__AnonStorey = new UnlockProAccountFlow._onClickUnlockConfirmed_c__AnonStorey0._onClickUnlockConfirmed_c__AnonStorey1();
			_onClickUnlockConfirmed_c__AnonStorey.__f__ref_0 = this;
			_onClickUnlockConfirmed_c__AnonStorey.result = result;
			int num = (int)(WTime.currentTimeMs - this.time);
			if ((float)num < UnlockProAccountFlow.MIN_WAIT_TIME * 1000f)
			{
				int num2 = (int)(UnlockProAccountFlow.MIN_WAIT_TIME * 1000f) - num;
				this._this.timer.SetTimeout(num2, new Action(_onClickUnlockConfirmed_c__AnonStorey.__m__0));
			}
			else
			{
				this._this.handleUnlockResult(_onClickUnlockConfirmed_c__AnonStorey.result);
			}
		}
	}

	private static float MIN_WAIT_TIME = 0.75f;

	private UnlockFlowDialog dialog;

	private bool inProgress;

	[Inject]
	public IUnlockProAccount unlockProAccount
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

	public void Start()
	{
		if (this.inProgress)
		{
			throw new UnityException("Something activated an unlock pro account flow while the first was in progress, this should not be possible.");
		}
		this.inProgress = true;
		string title = "pro account plz";
		string body = "become super pro rite now?";
		string confirmText = "yes plz";
		string cancelText = "nothx";
		this.startDialog(title, body, confirmText, cancelText);
	}

	private void startDialog(string title, string body, string confirmText, string cancelText)
	{
		this.dialog = this.dialogController.ShowUnlockFlowDialog();
		this.dialog.LoadConfirmationText(title, body, confirmText, cancelText, false, 0f);
		this.dialog.LoadSpinnyText(this.localization.GetText("ui.store.proAccount.unlockLoading.title"));
		this.dialog.LoadStaticErrorText(this.localization.GetText("ui.store.proAccount.unlockError.confirm"));
		this.dialog.LoadSuccess(this.localization.GetText("ui.store.proAccount.unlockSuccess.title"), this.localization.GetText("ui.store.proAccount.unlockSuccess.body"), this.localization.GetText("ui.store.proAccount.unlockSuccess.confirm"), string.Empty, false);
		this.dialog.SwitchMode(UnlockFlowDialogModes.CONFIRM);
		this.dialog.ConfirmCallback = new Action(this.onClickUnlockConfirmed);
		this.dialog.CloseCallback = new Action(this.cleanup);
	}

	private void onClickUnlockConfirmed()
	{
		UnlockProAccountFlow._onClickUnlockConfirmed_c__AnonStorey0 _onClickUnlockConfirmed_c__AnonStorey = new UnlockProAccountFlow._onClickUnlockConfirmed_c__AnonStorey0();
		_onClickUnlockConfirmed_c__AnonStorey._this = this;
		CurrencyType currencyType = this.unlockProAccount.GetCurrencyType();
		if (currencyType == CurrencyType.Hard && !this.purchaseErrorHandler.VerifySteam(this.dialog))
		{
			return;
		}
		this.dialog.SwitchMode(UnlockFlowDialogModes.LOADING);
		_onClickUnlockConfirmed_c__AnonStorey.time = WTime.currentTimeMs;
		this.unlockProAccount.Purchase(new Action<UserPurchaseResult>(_onClickUnlockConfirmed_c__AnonStorey.__m__0));
	}

	private void handleUnlockResult(UserPurchaseResult result)
	{
		if (result == UserPurchaseResult.SUCCESS)
		{
			this.showSuccess();
			this.signalBus.Dispatch(UserProAccountUnlockedModel.UPDATED);
		}
		else
		{
			this.showUnlockError(result);
		}
	}

	private void showSuccess()
	{
		this.dialog.ShowSuccess(CharacterID.None);
	}

	private void showUnlockError(UserPurchaseResult result)
	{
		this.dialog.LoadStaticErrorText(this.localization.GetText("ui.store.characters.unlockError.confirm"));
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
	}
}
