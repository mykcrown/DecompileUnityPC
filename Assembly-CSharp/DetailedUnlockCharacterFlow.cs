using System;
using UnityEngine;

// Token: 0x020009E0 RID: 2528
public class DetailedUnlockCharacterFlow : IDetailedUnlockCharacterFlow
{
	// Token: 0x17001127 RID: 4391
	// (get) Token: 0x06004792 RID: 18322 RVA: 0x00136694 File Offset: 0x00134A94
	// (set) Token: 0x06004793 RID: 18323 RVA: 0x0013669C File Offset: 0x00134A9C
	[Inject]
	public ILocalization localization { private get; set; }

	// Token: 0x17001128 RID: 4392
	// (get) Token: 0x06004794 RID: 18324 RVA: 0x001366A5 File Offset: 0x00134AA5
	// (set) Token: 0x06004795 RID: 18325 RVA: 0x001366AD File Offset: 0x00134AAD
	[Inject]
	public IMainThreadTimer timer { private get; set; }

	// Token: 0x17001129 RID: 4393
	// (get) Token: 0x06004796 RID: 18326 RVA: 0x001366B6 File Offset: 0x00134AB6
	// (set) Token: 0x06004797 RID: 18327 RVA: 0x001366BE File Offset: 0x00134ABE
	[Inject]
	public IDialogController dialogController { private get; set; }

	// Token: 0x1700112A RID: 4394
	// (get) Token: 0x06004798 RID: 18328 RVA: 0x001366C7 File Offset: 0x00134AC7
	// (set) Token: 0x06004799 RID: 18329 RVA: 0x001366CF File Offset: 0x00134ACF
	[Inject]
	public IUnlockCharacter unlockCharacter { private get; set; }

	// Token: 0x1700112B RID: 4395
	// (get) Token: 0x0600479A RID: 18330 RVA: 0x001366D8 File Offset: 0x00134AD8
	// (set) Token: 0x0600479B RID: 18331 RVA: 0x001366E0 File Offset: 0x00134AE0
	[Inject]
	public ICharacterDataHelper characterDataHelper { private get; set; }

	// Token: 0x1700112C RID: 4396
	// (get) Token: 0x0600479C RID: 18332 RVA: 0x001366E9 File Offset: 0x00134AE9
	// (set) Token: 0x0600479D RID: 18333 RVA: 0x001366F1 File Offset: 0x00134AF1
	[Inject]
	public IUIAdapter uiAdapter { private get; set; }

	// Token: 0x1700112D RID: 4397
	// (get) Token: 0x0600479E RID: 18334 RVA: 0x001366FA File Offset: 0x00134AFA
	// (set) Token: 0x0600479F RID: 18335 RVA: 0x00136702 File Offset: 0x00134B02
	[Inject]
	public IUnlockProAccount unlockProAccount { private get; set; }

	// Token: 0x1700112E RID: 4398
	// (get) Token: 0x060047A0 RID: 18336 RVA: 0x0013670B File Offset: 0x00134B0B
	// (set) Token: 0x060047A1 RID: 18337 RVA: 0x00136713 File Offset: 0x00134B13
	[Inject]
	public IUserProAccountUnlockedModel userProAccountModel { private get; set; }

	// Token: 0x1700112F RID: 4399
	// (get) Token: 0x060047A2 RID: 18338 RVA: 0x0013671C File Offset: 0x00134B1C
	// (set) Token: 0x060047A3 RID: 18339 RVA: 0x00136724 File Offset: 0x00134B24
	[Inject]
	public IPurchaseResponseHandler purchaseErrorHandler { private get; set; }

	// Token: 0x17001130 RID: 4400
	// (get) Token: 0x060047A4 RID: 18340 RVA: 0x0013672D File Offset: 0x00134B2D
	// (set) Token: 0x060047A5 RID: 18341 RVA: 0x00136735 File Offset: 0x00134B35
	[Inject]
	public IUserCurrencyModel userCurrencyModel { private get; set; }

	// Token: 0x17001131 RID: 4401
	// (get) Token: 0x060047A6 RID: 18342 RVA: 0x0013673E File Offset: 0x00134B3E
	// (set) Token: 0x060047A7 RID: 18343 RVA: 0x00136746 File Offset: 0x00134B46
	[Inject]
	public IInputBlocker inputBlocker { private get; set; }

	// Token: 0x17001132 RID: 4402
	// (get) Token: 0x060047A8 RID: 18344 RVA: 0x0013674F File Offset: 0x00134B4F
	// (set) Token: 0x060047A9 RID: 18345 RVA: 0x00136757 File Offset: 0x00134B57
	[Inject]
	public ISkinDataManager skinDataManager { private get; set; }

	// Token: 0x17001133 RID: 4403
	// (get) Token: 0x060047AA RID: 18346 RVA: 0x00136760 File Offset: 0x00134B60
	// (set) Token: 0x060047AB RID: 18347 RVA: 0x00136768 File Offset: 0x00134B68
	[Inject]
	public ICharacterLists characterLists { private get; set; }

	// Token: 0x060047AC RID: 18348 RVA: 0x00136774 File Offset: 0x00134B74
	public void Start(CharacterID characterId, Action closeCallback)
	{
		if (this.inProgress)
		{
			throw new UnityException("Something activated an unlock character flow while the first was in progress, this should not be possible.");
		}
		this.inProgress = true;
		this.characterId = characterId;
		this.dialog = this.dialogController.ShowDetailedUnlockFlowDialog();
		PurchasePreviewScene3D uiscene = this.uiAdapter.GetUIScene<PurchasePreviewScene3D>();
		float width = this.dialog.PurchasePreviewImage.rectTransform.rect.width;
		float height = this.dialog.PurchasePreviewImage.rectTransform.rect.height;
		RenderTexture renderTexture = new RenderTexture((int)width, (int)height, 0);
		uiscene.RenderToTexture(renderTexture, characterId, this.skinDataManager.GetDefaultSkin(characterId));
		string displayName = this.characterDataHelper.GetDisplayName(characterId);
		string text = this.localization.GetText("ui.store.characters.unlockSelection.title", displayName);
		string text2 = this.localization.GetText("ui.store.characters.unlockSelection.body");
		string hardPriceString = this.unlockCharacter.GetHardPriceString(characterId);
		string softCurrency = string.Format("<sprite=0> {0}", this.unlockCharacter.GetSoftPriceString(characterId));
		string text3 = this.localization.GetText("ui.store.characters.unlockSelection.proAccountCost", this.unlockProAccount.GetPriceString(), "<sprite=0>");
		string footnoteString = this.GetFootnoteString(characterId);
		this.dialog.LoadSelectCurrency(renderTexture, text, text2, hardPriceString, softCurrency, text3, footnoteString);
		this.dialog.LoadStaticErrorText(this.localization.GetText("ui.store.characters.unlockError.confirm"));
		this.dialog.SwitchMode(DetailedUnlockFlowDialogModes.SELECT_CURRENCY);
		this.dialog.PurchaseSelectedCallback = new Action<DetailedUnlockFlowDialog.PurchaseType>(this.onClickPurchase);
		this.dialog.CloseCallback = delegate()
		{
			this.cleanup();
			closeCallback();
		};
	}

	// Token: 0x060047AD RID: 18349 RVA: 0x00136929 File Offset: 0x00134D29
	private string getTokenCountString(int tokens)
	{
		return this.localization.GetText("ui.store.characters.unlockSelection.token" + ((tokens <= 1) ? string.Empty : "s"), tokens.ToString());
	}

	// Token: 0x060047AE RID: 18350 RVA: 0x00136964 File Offset: 0x00134D64
	private void onClickPurchase(DetailedUnlockFlowDialog.PurchaseType purchaseType)
	{
		this.purchaseType = purchaseType;
		if (purchaseType != DetailedUnlockFlowDialog.PurchaseType.UnlockToken && this.userCurrencyModel.CharacterUnlockTokens > 0)
		{
			GenericDialog genericDialog = this.dialogController.ShowTwoButtonDialog(this.localization.GetText("ui.store.characters.unlockSelection.purchase"), this.localization.GetText("ui.store.characters.unlockSelection.haveUnlockToken", this.characterDataHelper.GetDisplayName(this.characterId), this.GetPriceString(purchaseType, this.characterId), this.getTokenCountString(this.userCurrencyModel.CharacterUnlockTokens)), this.localization.GetText("ui.store.characters.unlockConfirm.ok"), this.localization.GetText("ui.store.characters.unlockConfirm.cancel"));
			GenericDialog genericDialog2 = genericDialog;
			genericDialog2.ConfirmCallback = (Action)Delegate.Combine(genericDialog2.ConfirmCallback, new Action(this.purchaseFlowFollowThrough));
		}
		else
		{
			this.purchaseFlowFollowThrough();
		}
	}

	// Token: 0x060047AF RID: 18351 RVA: 0x00136A38 File Offset: 0x00134E38
	private void purchaseFlowFollowThrough()
	{
		if (this.purchaseType == DetailedUnlockFlowDialog.PurchaseType.SoftCurrency && this.userCurrencyModel.Spectra < this.unlockCharacter.GetSoftPrice(this.characterId))
		{
			string text = this.localization.GetText("ui.store.characters.unlockError.money.title");
			string text2 = this.localization.GetText("ui.store.characters.unlockError.money.body");
			this.dialog.ShowError(text, text2);
		}
		else
		{
			string descriptionString = this.GetDescriptionString(this.purchaseType, this.characterId);
			string extraDescriptionString = this.GetExtraDescriptionString(this.purchaseType);
			string priceString = this.GetPriceString(this.purchaseType, this.characterId);
			this.dialog.LoadCheckoutText(descriptionString, priceString, extraDescriptionString);
			this.dialog.SwitchMode(DetailedUnlockFlowDialogModes.CHECKOUT);
			this.dialog.CheckoutConfirmCallback = new Action(this.onClickUnlockConfirmed);
		}
	}

	// Token: 0x060047B0 RID: 18352 RVA: 0x00136B0C File Offset: 0x00134F0C
	public void StartProAccount()
	{
		this.purchaseType = DetailedUnlockFlowDialog.PurchaseType.ProAccount;
		this.startProAccount();
	}

	// Token: 0x060047B1 RID: 18353 RVA: 0x00136B1B File Offset: 0x00134F1B
	public void StartFoundersPack()
	{
		this.purchaseType = DetailedUnlockFlowDialog.PurchaseType.FoundersPack;
		this.startProAccount();
	}

	// Token: 0x060047B2 RID: 18354 RVA: 0x00136B2C File Offset: 0x00134F2C
	private void startProAccount()
	{
		if (this.userProAccountModel.IsUnlocked())
		{
			return;
		}
		this.dialog = this.dialogController.ShowDetailedUnlockFlowDialog();
		this.dialog.CloseCallback = delegate()
		{
			this.cleanup();
		};
		string descriptionString = this.GetDescriptionString(this.purchaseType, CharacterID.None);
		string extraDescriptionString = this.GetExtraDescriptionString(this.purchaseType);
		string priceString = this.GetPriceString(this.purchaseType, CharacterID.None);
		this.dialog.LoadCheckoutText(descriptionString, priceString, extraDescriptionString);
		this.dialog.SwitchMode(DetailedUnlockFlowDialogModes.CHECKOUT);
		this.dialog.CheckoutConfirmCallback = new Action(this.onClickUnlockConfirmed);
	}

	// Token: 0x060047B3 RID: 18355 RVA: 0x00136BCC File Offset: 0x00134FCC
	private void onClickUnlockConfirmed()
	{
		DetailedUnlockCharacterFlow.<onClickUnlockConfirmed>c__AnonStorey1 <onClickUnlockConfirmed>c__AnonStorey = new DetailedUnlockCharacterFlow.<onClickUnlockConfirmed>c__AnonStorey1();
		<onClickUnlockConfirmed>c__AnonStorey.$this = this;
		CurrencyType currencyType = this.getCurrencyType();
		if (currencyType == CurrencyType.Hard && !this.purchaseErrorHandler.VerifySteam(this.dialog))
		{
			return;
		}
		this.dialog.SwitchMode(DetailedUnlockFlowDialogModes.LOADING);
		this.startLoadTime = Time.realtimeSinceStartup;
		<onClickUnlockConfirmed>c__AnonStorey.block = null;
		switch (this.purchaseType)
		{
		case DetailedUnlockFlowDialog.PurchaseType.HardCurrency:
			this.dialog.LoadSpinnyText(this.localization.GetText("ui.store.characters.unlockLoading.title"));
			this.unlockCharacter.PurchaseWithHard(this.characterId, delegate(UserPurchaseResult result)
			{
				<onClickUnlockConfirmed>c__AnonStorey.$this.proceedToResult(delegate
				{
					<onClickUnlockConfirmed>c__AnonStorey.handleCharacterUnlockResult(result);
				});
			});
			break;
		case DetailedUnlockFlowDialog.PurchaseType.SoftCurrency:
			<onClickUnlockConfirmed>c__AnonStorey.block = this.inputBlocker.Request();
			this.dialog.LoadSpinnyText(this.localization.GetText("ui.store.characters.unlockLoading.title"));
			this.unlockCharacter.PurchaseWithSoft(this.characterId, delegate(UserPurchaseResult result)
			{
				<onClickUnlockConfirmed>c__AnonStorey.$this.proceedToResult(delegate
				{
					<onClickUnlockConfirmed>c__AnonStorey.inputBlocker.Release(<onClickUnlockConfirmed>c__AnonStorey.block);
					<onClickUnlockConfirmed>c__AnonStorey.handleCharacterUnlockResult(result);
				});
			});
			break;
		case DetailedUnlockFlowDialog.PurchaseType.ProAccount:
			this.dialog.LoadSpinnyText(this.localization.GetText("ui.store.proAccount.unlockLoading.title"));
			this.unlockProAccount.Purchase(delegate(UserPurchaseResult result)
			{
				<onClickUnlockConfirmed>c__AnonStorey.$this.proceedToResult(delegate
				{
					<onClickUnlockConfirmed>c__AnonStorey.handleProAccountUnlockResult(result);
				});
			});
			break;
		case DetailedUnlockFlowDialog.PurchaseType.FoundersPack:
			this.dialog.LoadSpinnyText(this.localization.GetText("ui.store.foundersPack.unlockLoading.title"));
			this.unlockProAccount.PurchaseFoundersPack(delegate(UserPurchaseResult result)
			{
				<onClickUnlockConfirmed>c__AnonStorey.$this.proceedToResult(delegate
				{
					<onClickUnlockConfirmed>c__AnonStorey.handleProAccountUnlockResult(result);
				});
			});
			break;
		case DetailedUnlockFlowDialog.PurchaseType.UnlockToken:
			<onClickUnlockConfirmed>c__AnonStorey.block = this.inputBlocker.Request();
			this.dialog.LoadSpinnyText(this.localization.GetText("ui.store.characters.unlockLoading.title"));
			this.unlockCharacter.PurchaseWithToken(this.characterId, delegate(UserPurchaseResult result)
			{
				<onClickUnlockConfirmed>c__AnonStorey.$this.proceedToResult(delegate
				{
					<onClickUnlockConfirmed>c__AnonStorey.inputBlocker.Release(<onClickUnlockConfirmed>c__AnonStorey.block);
					<onClickUnlockConfirmed>c__AnonStorey.handleCharacterUnlockResult(result);
				});
			});
			break;
		}
	}

	// Token: 0x060047B4 RID: 18356 RVA: 0x00136D98 File Offset: 0x00135198
	private CurrencyType getCurrencyType()
	{
		switch (this.purchaseType)
		{
		case DetailedUnlockFlowDialog.PurchaseType.HardCurrency:
			return CurrencyType.Hard;
		case DetailedUnlockFlowDialog.PurchaseType.SoftCurrency:
			return CurrencyType.Soft;
		case DetailedUnlockFlowDialog.PurchaseType.ProAccount:
			return this.unlockProAccount.GetCurrencyType();
		case DetailedUnlockFlowDialog.PurchaseType.FoundersPack:
			return this.unlockProAccount.GetFoundersCurrencyType();
		default:
			return CurrencyType.Soft;
		}
	}

	// Token: 0x060047B5 RID: 18357 RVA: 0x00136DE4 File Offset: 0x001351E4
	private void proceedToResult(Action callback)
	{
		float num = Time.realtimeSinceStartup - this.startLoadTime;
		if (num >= DetailedUnlockCharacterFlow.MIN_LOADING_TIME)
		{
			callback();
		}
		else
		{
			float num2 = DetailedUnlockCharacterFlow.MIN_LOADING_TIME - num;
			this.timer.SetTimeout((int)(num2 * 1000f), callback);
		}
	}

	// Token: 0x060047B6 RID: 18358 RVA: 0x00136E30 File Offset: 0x00135230
	private void handleCharacterUnlockResult(UserPurchaseResult result)
	{
		if (result == UserPurchaseResult.SUCCESS)
		{
			string descriptionString = this.GetDescriptionString(this.purchaseType, this.characterId);
			string extraDescriptionString = this.GetExtraDescriptionString(this.purchaseType);
			string priceString = this.GetPriceString(this.purchaseType, this.characterId);
			this.dialog.LoadSuccess(descriptionString, priceString, extraDescriptionString);
			this.dialog.ShowSuccess();
		}
		else
		{
			this.purchaseErrorHandler.HandleUnlockError(result, this.dialog, new Action(this.cleanup));
		}
	}

	// Token: 0x060047B7 RID: 18359 RVA: 0x00136EB4 File Offset: 0x001352B4
	private void handleProAccountUnlockResult(UserPurchaseResult result)
	{
		if (result == UserPurchaseResult.SUCCESS)
		{
			string descriptionString = this.GetDescriptionString(this.purchaseType, this.characterId);
			string extraDescriptionString = this.GetExtraDescriptionString(this.purchaseType);
			string priceString = this.GetPriceString(this.purchaseType, this.characterId);
			this.dialog.LoadSuccess(descriptionString, priceString, extraDescriptionString);
			this.dialog.ShowSuccess();
		}
		else
		{
			this.purchaseErrorHandler.HandleUnlockError(result, this.dialog, new Action(this.cleanup));
		}
	}

	// Token: 0x060047B8 RID: 18360 RVA: 0x00136F36 File Offset: 0x00135336
	private void cleanup()
	{
		this.dialog.Close();
		this.dialog = null;
		this.inProgress = false;
	}

	// Token: 0x060047B9 RID: 18361 RVA: 0x00136F54 File Offset: 0x00135354
	private string GetFootnoteString(CharacterID character = CharacterID.None)
	{
		CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(character);
		string replace = string.Empty;
		if (characterDefinition != null)
		{
			replace = this.localization.GetText("gameData.characters.name." + characterDefinition.characterName);
			return this.localization.GetText("ui.store.characters.unlockSelection.footnote", replace);
		}
		return string.Empty;
	}

	// Token: 0x060047BA RID: 18362 RVA: 0x00136FB4 File Offset: 0x001353B4
	private string GetDescriptionString(DetailedUnlockFlowDialog.PurchaseType purchaseType, CharacterID character = CharacterID.None)
	{
		string replace = string.Empty;
		if (this.characterId != CharacterID.None)
		{
			replace = this.characterDataHelper.GetDisplayName(this.characterId);
		}
		switch (purchaseType)
		{
		case DetailedUnlockFlowDialog.PurchaseType.HardCurrency:
		case DetailedUnlockFlowDialog.PurchaseType.SoftCurrency:
		case DetailedUnlockFlowDialog.PurchaseType.UnlockToken:
			return this.localization.GetText("ui.store.characters.checkout.description", replace);
		case DetailedUnlockFlowDialog.PurchaseType.ProAccount:
			return this.localization.GetText("ui.store.proAccount.checkout.description");
		case DetailedUnlockFlowDialog.PurchaseType.FoundersPack:
			return this.localization.GetText("ui.store.foundersPack.checkout.description");
		default:
			Debug.LogError("No text set for purchase type of: " + purchaseType);
			return "UNKOWN CASE";
		}
	}

	// Token: 0x060047BB RID: 18363 RVA: 0x00137051 File Offset: 0x00135451
	public string GetExtraDescriptionString(DetailedUnlockFlowDialog.PurchaseType purchaseType)
	{
		if (purchaseType != DetailedUnlockFlowDialog.PurchaseType.FoundersPack)
		{
			return null;
		}
		return this.localization.GetText("ui.store.foundersPack.checkout.extraDescription");
	}

	// Token: 0x060047BC RID: 18364 RVA: 0x00137074 File Offset: 0x00135474
	private string GetPriceString(DetailedUnlockFlowDialog.PurchaseType purchaseType, CharacterID character = CharacterID.None)
	{
		switch (purchaseType)
		{
		case DetailedUnlockFlowDialog.PurchaseType.HardCurrency:
			return this.unlockCharacter.GetHardPriceString(this.characterId);
		case DetailedUnlockFlowDialog.PurchaseType.SoftCurrency:
			return string.Format("{0} <sprite=0>", this.unlockCharacter.GetSoftPriceString(this.characterId));
		case DetailedUnlockFlowDialog.PurchaseType.ProAccount:
			return this.unlockProAccount.GetPriceString();
		case DetailedUnlockFlowDialog.PurchaseType.FoundersPack:
			return this.unlockProAccount.GetFoundersPackPriceString();
		case DetailedUnlockFlowDialog.PurchaseType.UnlockToken:
			return "1 <sprite=\"character_unlock_token_ui\" index=0>";
		default:
			Debug.LogError("No text set for purchase type of: " + purchaseType);
			return "UNKOWN CASE";
		}
	}

	// Token: 0x04002F39 RID: 12089
	private static float MIN_LOADING_TIME = 1f;

	// Token: 0x04002F47 RID: 12103
	private bool inProgress;

	// Token: 0x04002F48 RID: 12104
	private CharacterID characterId;

	// Token: 0x04002F49 RID: 12105
	private float startLoadTime;

	// Token: 0x04002F4A RID: 12106
	private DetailedUnlockFlowDialog dialog;

	// Token: 0x04002F4B RID: 12107
	private DetailedUnlockFlowDialog.PurchaseType purchaseType;
}
