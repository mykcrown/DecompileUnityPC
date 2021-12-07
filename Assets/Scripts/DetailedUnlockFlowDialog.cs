// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DetailedUnlockFlowDialog : BaseWindow, IPurchaseResponseDialog
{
	private class MenuSelectionTarget
	{
		public MenuItemList menu;

		public MenuItemButton button;

		public MenuSelectionTarget(MenuItemList menu, MenuItemButton button)
		{
			this.menu = menu;
			this.button = button;
		}
	}

	public enum PurchaseType
	{
		HardCurrency,
		SoftCurrency,
		ProAccount,
		FoundersPack,
		UnlockToken
	}

	private sealed class _tweenOut_c__AnonStorey0
	{
		internal CanvasGroup target;

		internal DetailedUnlockFlowDialogModes mode;

		internal DetailedUnlockFlowDialog _this;

		internal float __m__0()
		{
			return this.target.alpha;
		}

		internal void __m__1(float x)
		{
			this.target.alpha = x;
		}

		internal void __m__2()
		{
			this.target.gameObject.SetActive(false);
			this._this.killTweens(this.mode);
		}
	}

	private sealed class _tweenIn_c__AnonStorey1
	{
		internal CanvasGroup target;

		internal DetailedUnlockFlowDialogModes mode;

		internal DetailedUnlockFlowDialog _this;

		internal float __m__0()
		{
			return this.target.alpha;
		}

		internal void __m__1(float x)
		{
			this.target.alpha = x;
		}

		internal void __m__2()
		{
			this._this.killTweens(this.mode);
		}
	}

	public RawImage PurchasePreviewImage;

	public TextMeshProUGUI SelectCurrencyTitle;

	public TextMeshProUGUI SelectCurrencyBody;

	public TextMeshProUGUI HardCurrencyButtonText;

	public TextMeshProUGUI SoftCurrencyButtonText;

	public TextMeshProUGUI ProAccountButtonText;

	public TextMeshProUGUI PurchaseFootNote;

	public TextMeshProUGUI CheckoutDescription;

	public TextMeshProUGUI CheckoutExtraDescription;

	public TextMeshProUGUI CheckoutPrice;

	public RectTransform CheckoutItemDivider;

	public CanvasGroup SelectCurrencyPrompt;

	public CanvasGroup CheckoutPrompt;

	public CanvasGroup LoadingSpinny;

	public CanvasGroup ErrorResult;

	public CanvasGroup SuccessResult;

	public TextMeshProUGUI LoadingSpinnyTitle;

	public TextMeshProUGUI ErrorResultTitle;

	public TextMeshProUGUI ErrorResultBody;

	public TextMeshProUGUI ErrorResultConfirm;

	public TextMeshProUGUI SuccessDescription;

	public TextMeshProUGUI SuccessExtraDescription;

	public TextMeshProUGUI SuccessPrice;

	public RectTransform SuccessItemDivider;

	public RadioButton TokenCurrencyRadio;

	public RadioButton HardCurrencyRadio;

	public RadioButton SoftCurrencyRadio;

	public RadioButton ProAccountRadio;

	public MenuItemButton PurchaseButton;

	public MenuItemButton CancelPurchaseButton;

	public MenuItemButton ConfirmButton;

	public MenuItemButton CancelButton;

	public MenuItemButton ErrorContinueButton;

	public MenuItemButton SuccessContinueButton;

	public GameObject[] PurchaseUnlockTokenObjects;

	public GameObject[] PurchaseCurrencyObjects;

	private DetailedUnlockFlowDialogModes currentMode;

	private Dictionary<DetailedUnlockFlowDialogModes, CanvasGroup> modeMap = new Dictionary<DetailedUnlockFlowDialogModes, CanvasGroup>();

	private Dictionary<DetailedUnlockFlowDialogModes, Tweener> _modeTweens = new Dictionary<DetailedUnlockFlowDialogModes, Tweener>();

	private Dictionary<DetailedUnlockFlowDialogModes, DetailedUnlockFlowDialog.MenuSelectionTarget> firstSelectionMap = new Dictionary<DetailedUnlockFlowDialogModes, DetailedUnlockFlowDialog.MenuSelectionTarget>();

	private MenuItemList currencyRadiosMenu;

	private MenuItemList currencyButtonsMenu;

	private MenuItemList confirmMenu;

	private MenuItemList errorMenu;

	private MenuItemList completeMenu;

	private DetailedUnlockFlowDialog.PurchaseType purchaseType;

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public IHyperlinkHandler hyperlinkHandler
	{
		get;
		set;
	}

	[Inject]
	public ISoundFileManager soundFileManager
	{
		get;
		set;
	}

	[Inject]
	public IUserCurrencyModel userCurrencyModel
	{
		private get;
		set;
	}

	public Action<DetailedUnlockFlowDialog.PurchaseType> PurchaseSelectedCallback
	{
		get;
		set;
	}

	public Action CheckoutConfirmCallback
	{
		get;
		set;
	}

	public Action CancelCallback
	{
		get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		this.configure();
		this.addSelectCurrencyMenu();
		this.addConfirmMenu();
		this.addErrorMenu();
		this.addCompleteMenu();
		this.mapMenus();
		this.onUpdated();
		base.UseOverrideOpenSound = true;
		base.OverrideOpenSound = this.soundFileManager.GetSoundAsAudioData(SoundKey.store_purchaseDialogOpened);
	}

	private void addSelectCurrencyMenu()
	{
		this.currencyRadiosMenu = base.injector.GetInstance<MenuItemList>();
		this.currencyButtonsMenu = base.injector.GetInstance<MenuItemList>();
		this.currencyRadiosMenu.SetNavigationType(MenuItemList.NavigationType.InOrderVertical, 0);
		this.currencyButtonsMenu.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		if (this.userCurrencyModel.CharacterUnlockTokens > 0)
		{
			this.TokenCurrencyRadio.gameObject.SetActive(true);
			this.currencyRadiosMenu.AddButton(this.TokenCurrencyRadio.Button, new Action<InputEventData>(this.onTokenCurrencyButtonClicked));
		}
		else
		{
			this.TokenCurrencyRadio.gameObject.SetActive(false);
		}
		this.currencyRadiosMenu.AddButton(this.HardCurrencyRadio.Button, new Action<InputEventData>(this.onHardCurrencyButtonClicked));
		this.currencyRadiosMenu.AddButton(this.SoftCurrencyRadio.Button, new Action<InputEventData>(this.onSoftCurrencyButtonClicked));
		this.currencyRadiosMenu.AddButton(this.ProAccountRadio.Button, new Action<InputEventData>(this.onProAccountButtonClicked));
		this.currencyButtonsMenu.AddButton(this.PurchaseButton, new Action(this.onPurchaseClicked));
		this.currencyButtonsMenu.AddButton(this.CancelPurchaseButton, new Action(this.onCancelButton));
		this.currencyRadiosMenu.AddEdgeNavigation(MoveDirection.Down, this.currencyButtonsMenu);
		this.currencyButtonsMenu.AddEdgeNavigation(MoveDirection.Up, this.currencyRadiosMenu);
		this.currencyRadiosMenu.LandingPoint = this.ProAccountRadio.Button;
		this.currencyButtonsMenu.LandingPoint = this.PurchaseButton;
		this.currencyRadiosMenu.DisableGridWrap();
		this.currencyRadiosMenu.Initialize();
		this.currencyButtonsMenu.Initialize();
	}

	private void addConfirmMenu()
	{
		this.confirmMenu = base.injector.GetInstance<MenuItemList>();
		this.confirmMenu.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		this.confirmMenu.AddButton(this.ConfirmButton, new Action(this.onCheckoutConfirmButton));
		this.confirmMenu.AddButton(this.CancelButton, new Action(this.onCancelButton));
		this.confirmMenu.Initialize();
	}

	private void addErrorMenu()
	{
		this.errorMenu = base.injector.GetInstance<MenuItemList>();
		this.errorMenu.AddButton(this.ErrorContinueButton, new Action(this.onCancelButton));
		this.errorMenu.Initialize();
	}

	private void addCompleteMenu()
	{
		this.completeMenu = base.injector.GetInstance<MenuItemList>();
		this.completeMenu.AddButton(this.SuccessContinueButton, new Action(this.OnClose));
		this.completeMenu.Initialize();
	}

	private void mapMenus()
	{
		this.purchaseType = ((this.userCurrencyModel.CharacterUnlockTokens <= 0) ? DetailedUnlockFlowDialog.PurchaseType.HardCurrency : DetailedUnlockFlowDialog.PurchaseType.UnlockToken);
		this.firstSelectionMap[DetailedUnlockFlowDialogModes.SELECT_CURRENCY] = new DetailedUnlockFlowDialog.MenuSelectionTarget(this.currencyRadiosMenu, (this.userCurrencyModel.CharacterUnlockTokens <= 0) ? this.HardCurrencyRadio.Button : this.TokenCurrencyRadio.Button);
		this.firstSelectionMap[DetailedUnlockFlowDialogModes.CHECKOUT] = new DetailedUnlockFlowDialog.MenuSelectionTarget(this.confirmMenu, this.ConfirmButton);
		this.firstSelectionMap[DetailedUnlockFlowDialogModes.ERROR] = new DetailedUnlockFlowDialog.MenuSelectionTarget(this.errorMenu, this.ErrorContinueButton);
		this.firstSelectionMap[DetailedUnlockFlowDialogModes.LOADING] = new DetailedUnlockFlowDialog.MenuSelectionTarget(null, null);
		this.firstSelectionMap[DetailedUnlockFlowDialogModes.COMPLETE] = new DetailedUnlockFlowDialog.MenuSelectionTarget(this.completeMenu, this.SuccessContinueButton);
	}

	private void configure()
	{
		this.modeMap[DetailedUnlockFlowDialogModes.SELECT_CURRENCY] = this.SelectCurrencyPrompt;
		this.modeMap[DetailedUnlockFlowDialogModes.CHECKOUT] = this.CheckoutPrompt;
		this.modeMap[DetailedUnlockFlowDialogModes.ERROR] = this.ErrorResult;
		this.modeMap[DetailedUnlockFlowDialogModes.LOADING] = this.LoadingSpinny;
		this.modeMap[DetailedUnlockFlowDialogModes.COMPLETE] = this.SuccessResult;
		foreach (CanvasGroup current in this.modeMap.Values)
		{
			current.gameObject.SetActive(false);
		}
	}

	private void onUpdated()
	{
		this.TokenCurrencyRadio.SetToggle(false);
		this.HardCurrencyRadio.SetToggle(false);
		this.SoftCurrencyRadio.SetToggle(false);
		this.ProAccountRadio.SetToggle(false);
		GameObject[] purchaseCurrencyObjects = this.PurchaseCurrencyObjects;
		for (int i = 0; i < purchaseCurrencyObjects.Length; i++)
		{
			GameObject gameObject = purchaseCurrencyObjects[i];
			gameObject.SetActive(this.purchaseType != DetailedUnlockFlowDialog.PurchaseType.UnlockToken);
		}
		GameObject[] purchaseUnlockTokenObjects = this.PurchaseUnlockTokenObjects;
		for (int j = 0; j < purchaseUnlockTokenObjects.Length; j++)
		{
			GameObject gameObject2 = purchaseUnlockTokenObjects[j];
			gameObject2.SetActive(this.purchaseType == DetailedUnlockFlowDialog.PurchaseType.UnlockToken);
		}
		switch (this.purchaseType)
		{
		case DetailedUnlockFlowDialog.PurchaseType.HardCurrency:
			this.HardCurrencyRadio.SetToggle(true);
			break;
		case DetailedUnlockFlowDialog.PurchaseType.SoftCurrency:
			this.SoftCurrencyRadio.SetToggle(true);
			break;
		case DetailedUnlockFlowDialog.PurchaseType.ProAccount:
			this.ProAccountRadio.SetToggle(true);
			break;
		case DetailedUnlockFlowDialog.PurchaseType.UnlockToken:
			this.TokenCurrencyRadio.SetToggle(true);
			break;
		}
	}

	public override void OnCancelPressed()
	{
		if (this.currentMode == DetailedUnlockFlowDialogModes.LOADING)
		{
			return;
		}
		this.onCancelButton();
	}

	private void onTokenCurrencyButtonClicked(InputEventData eventData)
	{
		if (!eventData.isMouseEvent && this.purchaseType == DetailedUnlockFlowDialog.PurchaseType.UnlockToken)
		{
			this.onPurchaseClicked();
		}
		else
		{
			this.purchaseType = DetailedUnlockFlowDialog.PurchaseType.UnlockToken;
			this.onUpdated();
		}
	}

	private void onHardCurrencyButtonClicked(InputEventData eventData)
	{
		if (!eventData.isMouseEvent && this.purchaseType == DetailedUnlockFlowDialog.PurchaseType.HardCurrency)
		{
			this.onPurchaseClicked();
		}
		else
		{
			this.purchaseType = DetailedUnlockFlowDialog.PurchaseType.HardCurrency;
			this.onUpdated();
		}
	}

	private void onSoftCurrencyButtonClicked(InputEventData eventData)
	{
		if (!eventData.isMouseEvent && this.purchaseType == DetailedUnlockFlowDialog.PurchaseType.SoftCurrency)
		{
			this.onPurchaseClicked();
		}
		else
		{
			this.purchaseType = DetailedUnlockFlowDialog.PurchaseType.SoftCurrency;
			this.onUpdated();
		}
	}

	private void onProAccountButtonClicked(InputEventData eventData)
	{
		if (!eventData.isMouseEvent && this.purchaseType == DetailedUnlockFlowDialog.PurchaseType.ProAccount)
		{
			this.onPurchaseClicked();
		}
		else
		{
			this.purchaseType = DetailedUnlockFlowDialog.PurchaseType.ProAccount;
			this.onUpdated();
		}
	}

	private void onPurchaseClicked()
	{
		if (this.PurchaseSelectedCallback != null)
		{
			this.PurchaseSelectedCallback(this.purchaseType);
		}
	}

	private void onCheckoutConfirmButton()
	{
		if (this.CheckoutConfirmCallback != null)
		{
			this.CheckoutConfirmCallback();
		}
	}

	private void onCancelButton()
	{
		this.OnCancelUnlock();
	}

	public void OnCancelUnlock()
	{
		if (this.CancelCallback != null)
		{
			this.CancelCallback();
		}
		this.Close();
	}

	public void OnClose()
	{
		this.Close();
	}

	public override void Close()
	{
		base.Close();
		Action closeCallback = base.CloseCallback;
		base.CloseCallback = null;
		this.CheckoutConfirmCallback = null;
		this.CancelCallback = null;
		if (closeCallback != null)
		{
			closeCallback();
		}
	}

	public void LoadSelectCurrency(RenderTexture previewTexture, string title, string body, string hardCurrency, string softCurrency, string proAccount, string footnote)
	{
		this.PurchasePreviewImage.texture = previewTexture;
		this.SelectCurrencyTitle.text = title;
		this.SelectCurrencyBody.text = body;
		this.HardCurrencyButtonText.text = hardCurrency;
		this.SoftCurrencyButtonText.text = softCurrency;
		this.ProAccountButtonText.text = proAccount;
		this.PurchaseFootNote.text = footnote;
	}

	public void LoadCheckoutText(string description, string price, string extraDescription = null)
	{
		this.CheckoutDescription.text = description;
		if (extraDescription == null)
		{
			this.CheckoutExtraDescription.gameObject.SetActive(false);
		}
		else
		{
			this.CheckoutExtraDescription.gameObject.SetActive(true);
			this.CheckoutExtraDescription.text = extraDescription;
		}
		this.CheckoutPrice.text = base.localization.GetText("ui.store.characters.checkout.total", price);
		float x = this.CheckoutDescription.rectTransform.offsetMin.x + this.CheckoutDescription.preferredWidth + 40f;
		float x2 = this.CheckoutPrice.rectTransform.offsetMax.x - this.CheckoutPrice.preferredWidth - 15f;
		this.CheckoutItemDivider.offsetMin = new Vector2(x, this.CheckoutItemDivider.offsetMin.y);
		this.CheckoutItemDivider.offsetMax = new Vector2(x2, this.CheckoutItemDivider.offsetMax.y);
	}

	public void LoadSpinnyText(string title)
	{
		this.LoadingSpinnyTitle.text = title;
	}

	public void LoadStaticErrorText(string confirmText)
	{
		this.ErrorResultConfirm.text = confirmText;
	}

	public void LoadSuccess(string description, string price, string extraDescription = null)
	{
		this.SuccessDescription.text = description;
		if (extraDescription == null)
		{
			this.SuccessExtraDescription.gameObject.SetActive(false);
		}
		else
		{
			this.SuccessExtraDescription.gameObject.SetActive(true);
			this.SuccessExtraDescription.text = extraDescription;
		}
		this.SuccessPrice.text = base.localization.GetText("ui.store.characters.checkout.total", price);
		float x = this.SuccessDescription.rectTransform.offsetMin.x + this.SuccessDescription.preferredWidth + 40f;
		float x2 = this.SuccessPrice.rectTransform.offsetMax.x - this.SuccessPrice.preferredWidth - 15f;
		this.SuccessItemDivider.offsetMin = new Vector2(x, this.SuccessItemDivider.offsetMin.y);
		this.SuccessItemDivider.offsetMax = new Vector2(x2, this.SuccessItemDivider.offsetMax.y);
	}

	public void ShowError(string title, string body)
	{
		this.ErrorResultTitle.text = title;
		this.ErrorResultBody.text = body;
		this.SwitchMode(DetailedUnlockFlowDialogModes.ERROR);
	}

	public void ShowSuccess()
	{
		this.SwitchMode(DetailedUnlockFlowDialogModes.COMPLETE);
		base.audioManager.PlayMenuSound(SoundKey.store_purchaseConfirmed, 0f);
	}

	public void SwitchMode(DetailedUnlockFlowDialogModes mode)
	{
		if (this.currentMode != DetailedUnlockFlowDialogModes.NONE)
		{
			this.tweenOut(this.currentMode);
		}
		this.currentMode = mode;
		if (this.currentMode != DetailedUnlockFlowDialogModes.NONE)
		{
			this.tweenIn(this.currentMode);
		}
	}

	private void tweenOut(DetailedUnlockFlowDialogModes mode)
	{
		DetailedUnlockFlowDialog._tweenOut_c__AnonStorey0 _tweenOut_c__AnonStorey = new DetailedUnlockFlowDialog._tweenOut_c__AnonStorey0();
		_tweenOut_c__AnonStorey.mode = mode;
		_tweenOut_c__AnonStorey._this = this;
		this.killTweens(_tweenOut_c__AnonStorey.mode);
		_tweenOut_c__AnonStorey.target = this.modeMap[_tweenOut_c__AnonStorey.mode];
		this._modeTweens[_tweenOut_c__AnonStorey.mode] = DOTween.To(new DOGetter<float>(_tweenOut_c__AnonStorey.__m__0), new DOSetter<float>(_tweenOut_c__AnonStorey.__m__1), 0f, 0.05f).SetEase(Ease.Linear).OnComplete(new TweenCallback(_tweenOut_c__AnonStorey.__m__2));
	}

	private void tweenIn(DetailedUnlockFlowDialogModes mode)
	{
		DetailedUnlockFlowDialog._tweenIn_c__AnonStorey1 _tweenIn_c__AnonStorey = new DetailedUnlockFlowDialog._tweenIn_c__AnonStorey1();
		_tweenIn_c__AnonStorey.mode = mode;
		_tweenIn_c__AnonStorey._this = this;
		this.killTweens(_tweenIn_c__AnonStorey.mode);
		_tweenIn_c__AnonStorey.target = this.modeMap[_tweenIn_c__AnonStorey.mode];
		_tweenIn_c__AnonStorey.target.alpha = 0f;
		_tweenIn_c__AnonStorey.target.gameObject.SetActive(true);
		this.setDefaultItem();
		this._modeTweens[_tweenIn_c__AnonStorey.mode] = DOTween.To(new DOGetter<float>(_tweenIn_c__AnonStorey.__m__0), new DOSetter<float>(_tweenIn_c__AnonStorey.__m__1), 1f, 0.05f).SetEase(Ease.Linear).OnComplete(new TweenCallback(_tweenIn_c__AnonStorey.__m__2));
	}

	private void setDefaultItem()
	{
		if (this.currentMode != DetailedUnlockFlowDialogModes.NONE)
		{
			MenuItemList menu = this.firstSelectionMap[this.currentMode].menu;
			MenuItemButton button = this.firstSelectionMap[this.currentMode].button;
			if (menu != null)
			{
				this.FirstSelected = button.InteractableButton.gameObject;
				menu.AutoSelect(button);
			}
			else
			{
				this.FirstSelected = null;
			}
		}
	}

	private void killTweens(DetailedUnlockFlowDialogModes mode)
	{
		if (this._modeTweens.ContainsKey(mode))
		{
			if (this._modeTweens[mode].IsPlaying())
			{
				this._modeTweens[mode].Kill(false);
			}
			this._modeTweens.Remove(mode);
		}
	}
}
