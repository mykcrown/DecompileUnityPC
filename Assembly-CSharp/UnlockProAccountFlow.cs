using System;
using UnityEngine;

// Token: 0x02000A14 RID: 2580
public class UnlockProAccountFlow : IUnlockProAccountFlow
{
	// Token: 0x170011DA RID: 4570
	// (get) Token: 0x06004AFA RID: 19194 RVA: 0x00140A2A File Offset: 0x0013EE2A
	// (set) Token: 0x06004AFB RID: 19195 RVA: 0x00140A32 File Offset: 0x0013EE32
	[Inject]
	public IUnlockProAccount unlockProAccount { get; set; }

	// Token: 0x170011DB RID: 4571
	// (get) Token: 0x06004AFC RID: 19196 RVA: 0x00140A3B File Offset: 0x0013EE3B
	// (set) Token: 0x06004AFD RID: 19197 RVA: 0x00140A43 File Offset: 0x0013EE43
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x170011DC RID: 4572
	// (get) Token: 0x06004AFE RID: 19198 RVA: 0x00140A4C File Offset: 0x0013EE4C
	// (set) Token: 0x06004AFF RID: 19199 RVA: 0x00140A54 File Offset: 0x0013EE54
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x170011DD RID: 4573
	// (get) Token: 0x06004B00 RID: 19200 RVA: 0x00140A5D File Offset: 0x0013EE5D
	// (set) Token: 0x06004B01 RID: 19201 RVA: 0x00140A65 File Offset: 0x0013EE65
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x170011DE RID: 4574
	// (get) Token: 0x06004B02 RID: 19202 RVA: 0x00140A6E File Offset: 0x0013EE6E
	// (set) Token: 0x06004B03 RID: 19203 RVA: 0x00140A76 File Offset: 0x0013EE76
	[Inject]
	public IDialogController dialogController { get; set; }

	// Token: 0x170011DF RID: 4575
	// (get) Token: 0x06004B04 RID: 19204 RVA: 0x00140A7F File Offset: 0x0013EE7F
	// (set) Token: 0x06004B05 RID: 19205 RVA: 0x00140A87 File Offset: 0x0013EE87
	[Inject]
	public IPurchaseResponseHandler purchaseErrorHandler { get; set; }

	// Token: 0x06004B06 RID: 19206 RVA: 0x00140A90 File Offset: 0x0013EE90
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

	// Token: 0x06004B07 RID: 19207 RVA: 0x00140ADC File Offset: 0x0013EEDC
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

	// Token: 0x06004B08 RID: 19208 RVA: 0x00140BC4 File Offset: 0x0013EFC4
	private void onClickUnlockConfirmed()
	{
		UnlockProAccountFlow.<onClickUnlockConfirmed>c__AnonStorey0 <onClickUnlockConfirmed>c__AnonStorey = new UnlockProAccountFlow.<onClickUnlockConfirmed>c__AnonStorey0();
		<onClickUnlockConfirmed>c__AnonStorey.$this = this;
		CurrencyType currencyType = this.unlockProAccount.GetCurrencyType();
		if (currencyType == CurrencyType.Hard && !this.purchaseErrorHandler.VerifySteam(this.dialog))
		{
			return;
		}
		this.dialog.SwitchMode(UnlockFlowDialogModes.LOADING);
		<onClickUnlockConfirmed>c__AnonStorey.time = WTime.currentTimeMs;
		this.unlockProAccount.Purchase(delegate(UserPurchaseResult result)
		{
			int num = (int)(WTime.currentTimeMs - <onClickUnlockConfirmed>c__AnonStorey.time);
			if ((float)num < UnlockProAccountFlow.MIN_WAIT_TIME * 1000f)
			{
				int time = (int)(UnlockProAccountFlow.MIN_WAIT_TIME * 1000f) - num;
				<onClickUnlockConfirmed>c__AnonStorey.$this.timer.SetTimeout(time, delegate
				{
					<onClickUnlockConfirmed>c__AnonStorey.handleUnlockResult(result);
				});
			}
			else
			{
				<onClickUnlockConfirmed>c__AnonStorey.$this.handleUnlockResult(result);
			}
		});
	}

	// Token: 0x06004B09 RID: 19209 RVA: 0x00140C36 File Offset: 0x0013F036
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

	// Token: 0x06004B0A RID: 19210 RVA: 0x00140C60 File Offset: 0x0013F060
	private void showSuccess()
	{
		this.dialog.ShowSuccess(CharacterID.None);
	}

	// Token: 0x06004B0B RID: 19211 RVA: 0x00140C6E File Offset: 0x0013F06E
	private void showUnlockError(UserPurchaseResult result)
	{
		this.dialog.LoadStaticErrorText(this.localization.GetText("ui.store.characters.unlockError.confirm"));
		this.purchaseErrorHandler.HandleUnlockError(result, this.dialog, new Action(this.cleanup));
	}

	// Token: 0x06004B0C RID: 19212 RVA: 0x00140CA9 File Offset: 0x0013F0A9
	private void cleanup()
	{
		if (this.dialog != null)
		{
			this.dialog.Close();
		}
		this.dialog = null;
		this.inProgress = false;
	}

	// Token: 0x04003152 RID: 12626
	private static float MIN_WAIT_TIME = 0.75f;

	// Token: 0x04003159 RID: 12633
	private UnlockFlowDialog dialog;

	// Token: 0x0400315A RID: 12634
	private bool inProgress;
}
