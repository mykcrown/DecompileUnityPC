using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020009E2 RID: 2530
public class DetailedUnlockFlowDialog : BaseWindow, IPurchaseResponseDialog
{
	// Token: 0x17001134 RID: 4404
	// (get) Token: 0x060047C3 RID: 18371 RVA: 0x00137365 File Offset: 0x00135765
	// (set) Token: 0x060047C4 RID: 18372 RVA: 0x0013736D File Offset: 0x0013576D
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17001135 RID: 4405
	// (get) Token: 0x060047C5 RID: 18373 RVA: 0x00137376 File Offset: 0x00135776
	// (set) Token: 0x060047C6 RID: 18374 RVA: 0x0013737E File Offset: 0x0013577E
	[Inject]
	public IHyperlinkHandler hyperlinkHandler { get; set; }

	// Token: 0x17001136 RID: 4406
	// (get) Token: 0x060047C7 RID: 18375 RVA: 0x00137387 File Offset: 0x00135787
	// (set) Token: 0x060047C8 RID: 18376 RVA: 0x0013738F File Offset: 0x0013578F
	[Inject]
	public ISoundFileManager soundFileManager { get; set; }

	// Token: 0x17001137 RID: 4407
	// (get) Token: 0x060047C9 RID: 18377 RVA: 0x00137398 File Offset: 0x00135798
	// (set) Token: 0x060047CA RID: 18378 RVA: 0x001373A0 File Offset: 0x001357A0
	[Inject]
	public IUserCurrencyModel userCurrencyModel { private get; set; }

	// Token: 0x17001138 RID: 4408
	// (get) Token: 0x060047CB RID: 18379 RVA: 0x001373A9 File Offset: 0x001357A9
	// (set) Token: 0x060047CC RID: 18380 RVA: 0x001373B1 File Offset: 0x001357B1
	public Action<DetailedUnlockFlowDialog.PurchaseType> PurchaseSelectedCallback { get; set; }

	// Token: 0x17001139 RID: 4409
	// (get) Token: 0x060047CD RID: 18381 RVA: 0x001373BA File Offset: 0x001357BA
	// (set) Token: 0x060047CE RID: 18382 RVA: 0x001373C2 File Offset: 0x001357C2
	public Action CheckoutConfirmCallback { get; set; }

	// Token: 0x1700113A RID: 4410
	// (get) Token: 0x060047CF RID: 18383 RVA: 0x001373CB File Offset: 0x001357CB
	// (set) Token: 0x060047D0 RID: 18384 RVA: 0x001373D3 File Offset: 0x001357D3
	public Action CancelCallback { get; set; }

	// Token: 0x060047D1 RID: 18385 RVA: 0x001373DC File Offset: 0x001357DC
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

	// Token: 0x060047D2 RID: 18386 RVA: 0x00137430 File Offset: 0x00135830
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

	// Token: 0x060047D3 RID: 18387 RVA: 0x001375E0 File Offset: 0x001359E0
	private void addConfirmMenu()
	{
		this.confirmMenu = base.injector.GetInstance<MenuItemList>();
		this.confirmMenu.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		this.confirmMenu.AddButton(this.ConfirmButton, new Action(this.onCheckoutConfirmButton));
		this.confirmMenu.AddButton(this.CancelButton, new Action(this.onCancelButton));
		this.confirmMenu.Initialize();
	}

	// Token: 0x060047D4 RID: 18388 RVA: 0x00137650 File Offset: 0x00135A50
	private void addErrorMenu()
	{
		this.errorMenu = base.injector.GetInstance<MenuItemList>();
		this.errorMenu.AddButton(this.ErrorContinueButton, new Action(this.onCancelButton));
		this.errorMenu.Initialize();
	}

	// Token: 0x060047D5 RID: 18389 RVA: 0x0013768B File Offset: 0x00135A8B
	private void addCompleteMenu()
	{
		this.completeMenu = base.injector.GetInstance<MenuItemList>();
		this.completeMenu.AddButton(this.SuccessContinueButton, new Action(this.OnClose));
		this.completeMenu.Initialize();
	}

	// Token: 0x060047D6 RID: 18390 RVA: 0x001376C8 File Offset: 0x00135AC8
	private void mapMenus()
	{
		this.purchaseType = ((this.userCurrencyModel.CharacterUnlockTokens <= 0) ? DetailedUnlockFlowDialog.PurchaseType.HardCurrency : DetailedUnlockFlowDialog.PurchaseType.UnlockToken);
		this.firstSelectionMap[DetailedUnlockFlowDialogModes.SELECT_CURRENCY] = new DetailedUnlockFlowDialog.MenuSelectionTarget(this.currencyRadiosMenu, (this.userCurrencyModel.CharacterUnlockTokens <= 0) ? this.HardCurrencyRadio.Button : this.TokenCurrencyRadio.Button);
		this.firstSelectionMap[DetailedUnlockFlowDialogModes.CHECKOUT] = new DetailedUnlockFlowDialog.MenuSelectionTarget(this.confirmMenu, this.ConfirmButton);
		this.firstSelectionMap[DetailedUnlockFlowDialogModes.ERROR] = new DetailedUnlockFlowDialog.MenuSelectionTarget(this.errorMenu, this.ErrorContinueButton);
		this.firstSelectionMap[DetailedUnlockFlowDialogModes.LOADING] = new DetailedUnlockFlowDialog.MenuSelectionTarget(null, null);
		this.firstSelectionMap[DetailedUnlockFlowDialogModes.COMPLETE] = new DetailedUnlockFlowDialog.MenuSelectionTarget(this.completeMenu, this.SuccessContinueButton);
	}

	// Token: 0x060047D7 RID: 18391 RVA: 0x001377A0 File Offset: 0x00135BA0
	private void configure()
	{
		this.modeMap[DetailedUnlockFlowDialogModes.SELECT_CURRENCY] = this.SelectCurrencyPrompt;
		this.modeMap[DetailedUnlockFlowDialogModes.CHECKOUT] = this.CheckoutPrompt;
		this.modeMap[DetailedUnlockFlowDialogModes.ERROR] = this.ErrorResult;
		this.modeMap[DetailedUnlockFlowDialogModes.LOADING] = this.LoadingSpinny;
		this.modeMap[DetailedUnlockFlowDialogModes.COMPLETE] = this.SuccessResult;
		foreach (CanvasGroup canvasGroup in this.modeMap.Values)
		{
			canvasGroup.gameObject.SetActive(false);
		}
	}

	// Token: 0x060047D8 RID: 18392 RVA: 0x00137860 File Offset: 0x00135C60
	private void onUpdated()
	{
		this.TokenCurrencyRadio.SetToggle(false);
		this.HardCurrencyRadio.SetToggle(false);
		this.SoftCurrencyRadio.SetToggle(false);
		this.ProAccountRadio.SetToggle(false);
		foreach (GameObject gameObject in this.PurchaseCurrencyObjects)
		{
			gameObject.SetActive(this.purchaseType != DetailedUnlockFlowDialog.PurchaseType.UnlockToken);
		}
		foreach (GameObject gameObject2 in this.PurchaseUnlockTokenObjects)
		{
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

	// Token: 0x060047D9 RID: 18393 RVA: 0x00137970 File Offset: 0x00135D70
	public override void OnCancelPressed()
	{
		if (this.currentMode == DetailedUnlockFlowDialogModes.LOADING)
		{
			return;
		}
		this.onCancelButton();
	}

	// Token: 0x060047DA RID: 18394 RVA: 0x00137985 File Offset: 0x00135D85
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

	// Token: 0x060047DB RID: 18395 RVA: 0x001379B7 File Offset: 0x00135DB7
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

	// Token: 0x060047DC RID: 18396 RVA: 0x001379E8 File Offset: 0x00135DE8
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

	// Token: 0x060047DD RID: 18397 RVA: 0x00137A1A File Offset: 0x00135E1A
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

	// Token: 0x060047DE RID: 18398 RVA: 0x00137A4C File Offset: 0x00135E4C
	private void onPurchaseClicked()
	{
		if (this.PurchaseSelectedCallback != null)
		{
			this.PurchaseSelectedCallback(this.purchaseType);
		}
	}

	// Token: 0x060047DF RID: 18399 RVA: 0x00137A6A File Offset: 0x00135E6A
	private void onCheckoutConfirmButton()
	{
		if (this.CheckoutConfirmCallback != null)
		{
			this.CheckoutConfirmCallback();
		}
	}

	// Token: 0x060047E0 RID: 18400 RVA: 0x00137A82 File Offset: 0x00135E82
	private void onCancelButton()
	{
		this.OnCancelUnlock();
	}

	// Token: 0x060047E1 RID: 18401 RVA: 0x00137A8A File Offset: 0x00135E8A
	public void OnCancelUnlock()
	{
		if (this.CancelCallback != null)
		{
			this.CancelCallback();
		}
		this.Close();
	}

	// Token: 0x060047E2 RID: 18402 RVA: 0x00137AA8 File Offset: 0x00135EA8
	public void OnClose()
	{
		this.Close();
	}

	// Token: 0x060047E3 RID: 18403 RVA: 0x00137AB0 File Offset: 0x00135EB0
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

	// Token: 0x060047E4 RID: 18404 RVA: 0x00137AEC File Offset: 0x00135EEC
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

	// Token: 0x060047E5 RID: 18405 RVA: 0x00137B54 File Offset: 0x00135F54
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

	// Token: 0x060047E6 RID: 18406 RVA: 0x00137C62 File Offset: 0x00136062
	public void LoadSpinnyText(string title)
	{
		this.LoadingSpinnyTitle.text = title;
	}

	// Token: 0x060047E7 RID: 18407 RVA: 0x00137C70 File Offset: 0x00136070
	public void LoadStaticErrorText(string confirmText)
	{
		this.ErrorResultConfirm.text = confirmText;
	}

	// Token: 0x060047E8 RID: 18408 RVA: 0x00137C80 File Offset: 0x00136080
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

	// Token: 0x060047E9 RID: 18409 RVA: 0x00137D8E File Offset: 0x0013618E
	public void ShowError(string title, string body)
	{
		this.ErrorResultTitle.text = title;
		this.ErrorResultBody.text = body;
		this.SwitchMode(DetailedUnlockFlowDialogModes.ERROR);
	}

	// Token: 0x060047EA RID: 18410 RVA: 0x00137DAF File Offset: 0x001361AF
	public void ShowSuccess()
	{
		this.SwitchMode(DetailedUnlockFlowDialogModes.COMPLETE);
		base.audioManager.PlayMenuSound(SoundKey.store_purchaseConfirmed, 0f);
	}

	// Token: 0x060047EB RID: 18411 RVA: 0x00137DCA File Offset: 0x001361CA
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

	// Token: 0x060047EC RID: 18412 RVA: 0x00137E04 File Offset: 0x00136204
	private void tweenOut(DetailedUnlockFlowDialogModes mode)
	{
		this.killTweens(mode);
		CanvasGroup target = this.modeMap[mode];
		this._modeTweens[mode] = DOTween.To(() => target.alpha, delegate(float x)
		{
			target.alpha = x;
		}, 0f, 0.05f).SetEase(Ease.Linear).OnComplete(delegate
		{
			target.gameObject.SetActive(false);
			this.killTweens(mode);
		});
	}

	// Token: 0x060047ED RID: 18413 RVA: 0x00137E98 File Offset: 0x00136298
	private void tweenIn(DetailedUnlockFlowDialogModes mode)
	{
		this.killTweens(mode);
		CanvasGroup target = this.modeMap[mode];
		target.alpha = 0f;
		target.gameObject.SetActive(true);
		this.setDefaultItem();
		this._modeTweens[mode] = DOTween.To(() => target.alpha, delegate(float x)
		{
			target.alpha = x;
		}, 1f, 0.05f).SetEase(Ease.Linear).OnComplete(delegate
		{
			this.killTweens(mode);
		});
	}

	// Token: 0x060047EE RID: 18414 RVA: 0x00137F54 File Offset: 0x00136354
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

	// Token: 0x060047EF RID: 18415 RVA: 0x00137FC4 File Offset: 0x001363C4
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

	// Token: 0x04002F50 RID: 12112
	public RawImage PurchasePreviewImage;

	// Token: 0x04002F51 RID: 12113
	public TextMeshProUGUI SelectCurrencyTitle;

	// Token: 0x04002F52 RID: 12114
	public TextMeshProUGUI SelectCurrencyBody;

	// Token: 0x04002F53 RID: 12115
	public TextMeshProUGUI HardCurrencyButtonText;

	// Token: 0x04002F54 RID: 12116
	public TextMeshProUGUI SoftCurrencyButtonText;

	// Token: 0x04002F55 RID: 12117
	public TextMeshProUGUI ProAccountButtonText;

	// Token: 0x04002F56 RID: 12118
	public TextMeshProUGUI PurchaseFootNote;

	// Token: 0x04002F57 RID: 12119
	public TextMeshProUGUI CheckoutDescription;

	// Token: 0x04002F58 RID: 12120
	public TextMeshProUGUI CheckoutExtraDescription;

	// Token: 0x04002F59 RID: 12121
	public TextMeshProUGUI CheckoutPrice;

	// Token: 0x04002F5A RID: 12122
	public RectTransform CheckoutItemDivider;

	// Token: 0x04002F5B RID: 12123
	public CanvasGroup SelectCurrencyPrompt;

	// Token: 0x04002F5C RID: 12124
	public CanvasGroup CheckoutPrompt;

	// Token: 0x04002F5D RID: 12125
	public CanvasGroup LoadingSpinny;

	// Token: 0x04002F5E RID: 12126
	public CanvasGroup ErrorResult;

	// Token: 0x04002F5F RID: 12127
	public CanvasGroup SuccessResult;

	// Token: 0x04002F60 RID: 12128
	public TextMeshProUGUI LoadingSpinnyTitle;

	// Token: 0x04002F61 RID: 12129
	public TextMeshProUGUI ErrorResultTitle;

	// Token: 0x04002F62 RID: 12130
	public TextMeshProUGUI ErrorResultBody;

	// Token: 0x04002F63 RID: 12131
	public TextMeshProUGUI ErrorResultConfirm;

	// Token: 0x04002F64 RID: 12132
	public TextMeshProUGUI SuccessDescription;

	// Token: 0x04002F65 RID: 12133
	public TextMeshProUGUI SuccessExtraDescription;

	// Token: 0x04002F66 RID: 12134
	public TextMeshProUGUI SuccessPrice;

	// Token: 0x04002F67 RID: 12135
	public RectTransform SuccessItemDivider;

	// Token: 0x04002F68 RID: 12136
	public RadioButton TokenCurrencyRadio;

	// Token: 0x04002F69 RID: 12137
	public RadioButton HardCurrencyRadio;

	// Token: 0x04002F6A RID: 12138
	public RadioButton SoftCurrencyRadio;

	// Token: 0x04002F6B RID: 12139
	public RadioButton ProAccountRadio;

	// Token: 0x04002F6C RID: 12140
	public MenuItemButton PurchaseButton;

	// Token: 0x04002F6D RID: 12141
	public MenuItemButton CancelPurchaseButton;

	// Token: 0x04002F6E RID: 12142
	public MenuItemButton ConfirmButton;

	// Token: 0x04002F6F RID: 12143
	public MenuItemButton CancelButton;

	// Token: 0x04002F70 RID: 12144
	public MenuItemButton ErrorContinueButton;

	// Token: 0x04002F71 RID: 12145
	public MenuItemButton SuccessContinueButton;

	// Token: 0x04002F72 RID: 12146
	public GameObject[] PurchaseUnlockTokenObjects;

	// Token: 0x04002F73 RID: 12147
	public GameObject[] PurchaseCurrencyObjects;

	// Token: 0x04002F77 RID: 12151
	private DetailedUnlockFlowDialogModes currentMode;

	// Token: 0x04002F78 RID: 12152
	private Dictionary<DetailedUnlockFlowDialogModes, CanvasGroup> modeMap = new Dictionary<DetailedUnlockFlowDialogModes, CanvasGroup>();

	// Token: 0x04002F79 RID: 12153
	private Dictionary<DetailedUnlockFlowDialogModes, Tweener> _modeTweens = new Dictionary<DetailedUnlockFlowDialogModes, Tweener>();

	// Token: 0x04002F7A RID: 12154
	private Dictionary<DetailedUnlockFlowDialogModes, DetailedUnlockFlowDialog.MenuSelectionTarget> firstSelectionMap = new Dictionary<DetailedUnlockFlowDialogModes, DetailedUnlockFlowDialog.MenuSelectionTarget>();

	// Token: 0x04002F7B RID: 12155
	private MenuItemList currencyRadiosMenu;

	// Token: 0x04002F7C RID: 12156
	private MenuItemList currencyButtonsMenu;

	// Token: 0x04002F7D RID: 12157
	private MenuItemList confirmMenu;

	// Token: 0x04002F7E RID: 12158
	private MenuItemList errorMenu;

	// Token: 0x04002F7F RID: 12159
	private MenuItemList completeMenu;

	// Token: 0x04002F80 RID: 12160
	private DetailedUnlockFlowDialog.PurchaseType purchaseType;

	// Token: 0x020009E3 RID: 2531
	private class MenuSelectionTarget
	{
		// Token: 0x060047F0 RID: 18416 RVA: 0x00138017 File Offset: 0x00136417
		public MenuSelectionTarget(MenuItemList menu, MenuItemButton button)
		{
			this.menu = menu;
			this.button = button;
		}

		// Token: 0x04002F81 RID: 12161
		public MenuItemList menu;

		// Token: 0x04002F82 RID: 12162
		public MenuItemButton button;
	}

	// Token: 0x020009E4 RID: 2532
	public enum PurchaseType
	{
		// Token: 0x04002F84 RID: 12164
		HardCurrency,
		// Token: 0x04002F85 RID: 12165
		SoftCurrency,
		// Token: 0x04002F86 RID: 12166
		ProAccount,
		// Token: 0x04002F87 RID: 12167
		FoundersPack,
		// Token: 0x04002F88 RID: 12168
		UnlockToken
	}
}
