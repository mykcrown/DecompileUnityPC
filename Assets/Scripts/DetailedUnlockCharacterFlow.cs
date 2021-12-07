// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DetailedUnlockCharacterFlow : IDetailedUnlockCharacterFlow
{
	private sealed class _Start_c__AnonStorey0
	{
		internal Action closeCallback;

		internal DetailedUnlockCharacterFlow _this;

		internal void __m__0()
		{
			this._this.cleanup();
			this.closeCallback();
		}
	}

	private sealed class _onClickUnlockConfirmed_c__AnonStorey1
	{
		private sealed class _onClickUnlockConfirmed_c__AnonStorey2
		{
			internal UserPurchaseResult result;

			internal DetailedUnlockCharacterFlow._onClickUnlockConfirmed_c__AnonStorey1 __f__ref_1;

			internal void __m__0()
			{
				this.__f__ref_1._this.inputBlocker.Release(this.__f__ref_1.block);
				this.__f__ref_1._this.handleCharacterUnlockResult(this.result);
			}
		}

		private sealed class _onClickUnlockConfirmed_c__AnonStorey3
		{
			internal UserPurchaseResult result;

			internal DetailedUnlockCharacterFlow._onClickUnlockConfirmed_c__AnonStorey1 __f__ref_1;

			internal void __m__0()
			{
				this.__f__ref_1._this.handleCharacterUnlockResult(this.result);
			}
		}

		private sealed class _onClickUnlockConfirmed_c__AnonStorey4
		{
			internal UserPurchaseResult result;

			internal DetailedUnlockCharacterFlow._onClickUnlockConfirmed_c__AnonStorey1 __f__ref_1;

			internal void __m__0()
			{
				this.__f__ref_1._this.inputBlocker.Release(this.__f__ref_1.block);
				this.__f__ref_1._this.handleCharacterUnlockResult(this.result);
			}
		}

		private sealed class _onClickUnlockConfirmed_c__AnonStorey5
		{
			internal UserPurchaseResult result;

			internal DetailedUnlockCharacterFlow._onClickUnlockConfirmed_c__AnonStorey1 __f__ref_1;

			internal void __m__0()
			{
				this.__f__ref_1._this.handleProAccountUnlockResult(this.result);
			}
		}

		private sealed class _onClickUnlockConfirmed_c__AnonStorey6
		{
			internal UserPurchaseResult result;

			internal DetailedUnlockCharacterFlow._onClickUnlockConfirmed_c__AnonStorey1 __f__ref_1;

			internal void __m__0()
			{
				this.__f__ref_1._this.handleProAccountUnlockResult(this.result);
			}
		}

		internal InputBlock block;

		internal DetailedUnlockCharacterFlow _this;

		internal void __m__0(UserPurchaseResult result)
		{
			DetailedUnlockCharacterFlow._onClickUnlockConfirmed_c__AnonStorey1._onClickUnlockConfirmed_c__AnonStorey2 _onClickUnlockConfirmed_c__AnonStorey = new DetailedUnlockCharacterFlow._onClickUnlockConfirmed_c__AnonStorey1._onClickUnlockConfirmed_c__AnonStorey2();
			_onClickUnlockConfirmed_c__AnonStorey.__f__ref_1 = this;
			_onClickUnlockConfirmed_c__AnonStorey.result = result;
			this._this.proceedToResult(new Action(_onClickUnlockConfirmed_c__AnonStorey.__m__0));
		}

		internal void __m__1(UserPurchaseResult result)
		{
			DetailedUnlockCharacterFlow._onClickUnlockConfirmed_c__AnonStorey1._onClickUnlockConfirmed_c__AnonStorey3 _onClickUnlockConfirmed_c__AnonStorey = new DetailedUnlockCharacterFlow._onClickUnlockConfirmed_c__AnonStorey1._onClickUnlockConfirmed_c__AnonStorey3();
			_onClickUnlockConfirmed_c__AnonStorey.__f__ref_1 = this;
			_onClickUnlockConfirmed_c__AnonStorey.result = result;
			this._this.proceedToResult(new Action(_onClickUnlockConfirmed_c__AnonStorey.__m__0));
		}

		internal void __m__2(UserPurchaseResult result)
		{
			DetailedUnlockCharacterFlow._onClickUnlockConfirmed_c__AnonStorey1._onClickUnlockConfirmed_c__AnonStorey4 _onClickUnlockConfirmed_c__AnonStorey = new DetailedUnlockCharacterFlow._onClickUnlockConfirmed_c__AnonStorey1._onClickUnlockConfirmed_c__AnonStorey4();
			_onClickUnlockConfirmed_c__AnonStorey.__f__ref_1 = this;
			_onClickUnlockConfirmed_c__AnonStorey.result = result;
			this._this.proceedToResult(new Action(_onClickUnlockConfirmed_c__AnonStorey.__m__0));
		}

		internal void __m__3(UserPurchaseResult result)
		{
			DetailedUnlockCharacterFlow._onClickUnlockConfirmed_c__AnonStorey1._onClickUnlockConfirmed_c__AnonStorey5 _onClickUnlockConfirmed_c__AnonStorey = new DetailedUnlockCharacterFlow._onClickUnlockConfirmed_c__AnonStorey1._onClickUnlockConfirmed_c__AnonStorey5();
			_onClickUnlockConfirmed_c__AnonStorey.__f__ref_1 = this;
			_onClickUnlockConfirmed_c__AnonStorey.result = result;
			this._this.proceedToResult(new Action(_onClickUnlockConfirmed_c__AnonStorey.__m__0));
		}

		internal void __m__4(UserPurchaseResult result)
		{
			DetailedUnlockCharacterFlow._onClickUnlockConfirmed_c__AnonStorey1._onClickUnlockConfirmed_c__AnonStorey6 _onClickUnlockConfirmed_c__AnonStorey = new DetailedUnlockCharacterFlow._onClickUnlockConfirmed_c__AnonStorey1._onClickUnlockConfirmed_c__AnonStorey6();
			_onClickUnlockConfirmed_c__AnonStorey.__f__ref_1 = this;
			_onClickUnlockConfirmed_c__AnonStorey.result = result;
			this._this.proceedToResult(new Action(_onClickUnlockConfirmed_c__AnonStorey.__m__0));
		}
	}

	private static float MIN_LOADING_TIME = 1f;

	private bool inProgress;

	private CharacterID characterId;

	private float startLoadTime;

	private DetailedUnlockFlowDialog dialog;

	private DetailedUnlockFlowDialog.PurchaseType purchaseType;

	[Inject]
	public ILocalization localization
	{
		private get;
		set;
	}

	[Inject]
	public IMainThreadTimer timer
	{
		private get;
		set;
	}

	[Inject]
	public IDialogController dialogController
	{
		private get;
		set;
	}

	[Inject]
	public IUnlockCharacter unlockCharacter
	{
		private get;
		set;
	}

	[Inject]
	public ICharacterDataHelper characterDataHelper
	{
		private get;
		set;
	}

	[Inject]
	public IUIAdapter uiAdapter
	{
		private get;
		set;
	}

	[Inject]
	public IUnlockProAccount unlockProAccount
	{
		private get;
		set;
	}

	[Inject]
	public IUserProAccountUnlockedModel userProAccountModel
	{
		private get;
		set;
	}

	[Inject]
	public IPurchaseResponseHandler purchaseErrorHandler
	{
		private get;
		set;
	}

	[Inject]
	public IUserCurrencyModel userCurrencyModel
	{
		private get;
		set;
	}

	[Inject]
	public IInputBlocker inputBlocker
	{
		private get;
		set;
	}

	[Inject]
	public ISkinDataManager skinDataManager
	{
		private get;
		set;
	}

	[Inject]
	public ICharacterLists characterLists
	{
		private get;
		set;
	}

	public void Start(CharacterID characterId, Action closeCallback)
	{
		DetailedUnlockCharacterFlow._Start_c__AnonStorey0 _Start_c__AnonStorey = new DetailedUnlockCharacterFlow._Start_c__AnonStorey0();
		_Start_c__AnonStorey.closeCallback = closeCallback;
		_Start_c__AnonStorey._this = this;
		if (this.inProgress)
		{
			throw new UnityException("Something activated an unlock character flow while the first was in progress, this should not be possible.");
		}
		this.inProgress = true;
		this.characterId = characterId;
		this.dialog = this.dialogController.ShowDetailedUnlockFlowDialog();
		PurchasePreviewScene3D uIScene = this.uiAdapter.GetUIScene<PurchasePreviewScene3D>();
		float width = this.dialog.PurchasePreviewImage.rectTransform.rect.width;
		float height = this.dialog.PurchasePreviewImage.rectTransform.rect.height;
		RenderTexture renderTexture = new RenderTexture((int)width, (int)height, 0);
		uIScene.RenderToTexture(renderTexture, characterId, this.skinDataManager.GetDefaultSkin(characterId));
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
		this.dialog.CloseCallback = new Action(_Start_c__AnonStorey.__m__0);
	}

	private string getTokenCountString(int tokens)
	{
		return this.localization.GetText("ui.store.characters.unlockSelection.token" + ((tokens <= 1) ? string.Empty : "s"), tokens.ToString());
	}

	private void onClickPurchase(DetailedUnlockFlowDialog.PurchaseType purchaseType)
	{
		this.purchaseType = purchaseType;
		if (purchaseType != DetailedUnlockFlowDialog.PurchaseType.UnlockToken && this.userCurrencyModel.CharacterUnlockTokens > 0)
		{
			GenericDialog genericDialog = this.dialogController.ShowTwoButtonDialog(this.localization.GetText("ui.store.characters.unlockSelection.purchase"), this.localization.GetText("ui.store.characters.unlockSelection.haveUnlockToken", this.characterDataHelper.GetDisplayName(this.characterId), this.GetPriceString(purchaseType, this.characterId), this.getTokenCountString(this.userCurrencyModel.CharacterUnlockTokens)), this.localization.GetText("ui.store.characters.unlockConfirm.ok"), this.localization.GetText("ui.store.characters.unlockConfirm.cancel"));
			GenericDialog expr_9B = genericDialog;
			expr_9B.ConfirmCallback = (Action)Delegate.Combine(expr_9B.ConfirmCallback, new Action(this.purchaseFlowFollowThrough));
		}
		else
		{
			this.purchaseFlowFollowThrough();
		}
	}

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

	public void StartProAccount()
	{
		this.purchaseType = DetailedUnlockFlowDialog.PurchaseType.ProAccount;
		this.startProAccount();
	}

	public void StartFoundersPack()
	{
		this.purchaseType = DetailedUnlockFlowDialog.PurchaseType.FoundersPack;
		this.startProAccount();
	}

	private void startProAccount()
	{
		if (this.userProAccountModel.IsUnlocked())
		{
			return;
		}
		this.dialog = this.dialogController.ShowDetailedUnlockFlowDialog();
		this.dialog.CloseCallback = new Action(this._startProAccount_m__0);
		string descriptionString = this.GetDescriptionString(this.purchaseType, CharacterID.None);
		string extraDescriptionString = this.GetExtraDescriptionString(this.purchaseType);
		string priceString = this.GetPriceString(this.purchaseType, CharacterID.None);
		this.dialog.LoadCheckoutText(descriptionString, priceString, extraDescriptionString);
		this.dialog.SwitchMode(DetailedUnlockFlowDialogModes.CHECKOUT);
		this.dialog.CheckoutConfirmCallback = new Action(this.onClickUnlockConfirmed);
	}

	private void onClickUnlockConfirmed()
	{
		DetailedUnlockCharacterFlow._onClickUnlockConfirmed_c__AnonStorey1 _onClickUnlockConfirmed_c__AnonStorey = new DetailedUnlockCharacterFlow._onClickUnlockConfirmed_c__AnonStorey1();
		_onClickUnlockConfirmed_c__AnonStorey._this = this;
		CurrencyType currencyType = this.getCurrencyType();
		if (currencyType == CurrencyType.Hard && !this.purchaseErrorHandler.VerifySteam(this.dialog))
		{
			return;
		}
		this.dialog.SwitchMode(DetailedUnlockFlowDialogModes.LOADING);
		this.startLoadTime = Time.realtimeSinceStartup;
		_onClickUnlockConfirmed_c__AnonStorey.block = null;
		switch (this.purchaseType)
		{
		case DetailedUnlockFlowDialog.PurchaseType.HardCurrency:
			this.dialog.LoadSpinnyText(this.localization.GetText("ui.store.characters.unlockLoading.title"));
			this.unlockCharacter.PurchaseWithHard(this.characterId, new Action<UserPurchaseResult>(_onClickUnlockConfirmed_c__AnonStorey.__m__1));
			break;
		case DetailedUnlockFlowDialog.PurchaseType.SoftCurrency:
			_onClickUnlockConfirmed_c__AnonStorey.block = this.inputBlocker.Request();
			this.dialog.LoadSpinnyText(this.localization.GetText("ui.store.characters.unlockLoading.title"));
			this.unlockCharacter.PurchaseWithSoft(this.characterId, new Action<UserPurchaseResult>(_onClickUnlockConfirmed_c__AnonStorey.__m__2));
			break;
		case DetailedUnlockFlowDialog.PurchaseType.ProAccount:
			this.dialog.LoadSpinnyText(this.localization.GetText("ui.store.proAccount.unlockLoading.title"));
			this.unlockProAccount.Purchase(new Action<UserPurchaseResult>(_onClickUnlockConfirmed_c__AnonStorey.__m__3));
			break;
		case DetailedUnlockFlowDialog.PurchaseType.FoundersPack:
			this.dialog.LoadSpinnyText(this.localization.GetText("ui.store.foundersPack.unlockLoading.title"));
			this.unlockProAccount.PurchaseFoundersPack(new Action<UserPurchaseResult>(_onClickUnlockConfirmed_c__AnonStorey.__m__4));
			break;
		case DetailedUnlockFlowDialog.PurchaseType.UnlockToken:
			_onClickUnlockConfirmed_c__AnonStorey.block = this.inputBlocker.Request();
			this.dialog.LoadSpinnyText(this.localization.GetText("ui.store.characters.unlockLoading.title"));
			this.unlockCharacter.PurchaseWithToken(this.characterId, new Action<UserPurchaseResult>(_onClickUnlockConfirmed_c__AnonStorey.__m__0));
			break;
		}
	}

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

	private void cleanup()
	{
		this.dialog.Close();
		this.dialog = null;
		this.inProgress = false;
	}

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
			UnityEngine.Debug.LogError("No text set for purchase type of: " + purchaseType);
			return "UNKOWN CASE";
		}
	}

	public string GetExtraDescriptionString(DetailedUnlockFlowDialog.PurchaseType purchaseType)
	{
		if (purchaseType != DetailedUnlockFlowDialog.PurchaseType.FoundersPack)
		{
			return null;
		}
		return this.localization.GetText("ui.store.foundersPack.checkout.extraDescription");
	}

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
			UnityEngine.Debug.LogError("No text set for purchase type of: " + purchaseType);
			return "UNKOWN CASE";
		}
	}

	private void _startProAccount_m__0()
	{
		this.cleanup();
	}
}
