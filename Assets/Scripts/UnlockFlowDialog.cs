// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using UnityEngine;

public class UnlockFlowDialog : BaseWindow, IPurchaseResponseDialog
{
	private class MenuSelectionTarget
	{
		public MenuItemList menu;

		public MenuItemButton primaryButton;

		public MenuItemButton secondaryButton;

		public MenuSelectionTarget(MenuItemList menu, MenuItemButton primaryButton, MenuItemButton secondaryButton = null)
		{
			this.menu = menu;
			this.primaryButton = primaryButton;
			this.secondaryButton = secondaryButton;
		}
	}

	private sealed class _tweenOut_c__AnonStorey0
	{
		internal CanvasGroup target;

		internal UnlockFlowDialogModes mode;

		internal UnlockFlowDialog _this;

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

		internal UnlockFlowDialogModes mode;

		internal UnlockFlowDialog _this;

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

	public TextMeshProUGUI Body;

	public TextMeshProUGUI Title;

	public TextMeshProUGUI TimerText;

	public TextMeshProUGUI ConfirmButtonText;

	public TextMeshProUGUI CancelButtonText;

	public CanvasGroup ConfirmPrompt;

	public CanvasGroup LoadingSpinny;

	public CanvasGroup ErrorResult;

	public CanvasGroup SuccessResult;

	public TextMeshProUGUI LoadingSpinnyTitle;

	public TextMeshProUGUI ErrorResultTitle;

	public TextMeshProUGUI ErrorResultBody;

	public TextMeshProUGUI ErrorResultConfirm;

	public TextMeshProUGUI SuccessResultTitle;

	public TextMeshProUGUI SuccessResultBody;

	public TextMeshProUGUI SuccessResultConfirm;

	public TextMeshProUGUI SuccessResultEquip;

	public MenuItemButton ConfirmButton;

	public MenuItemButton CancelButton;

	public MenuItemButton ErrorContinueButton;

	public MenuItemButton SuccessContinueButton;

	public MenuItemButton SuccessEquipButton;

	private UnlockFlowDialogModes currentMode;

	private Dictionary<UnlockFlowDialogModes, CanvasGroup> modeMap = new Dictionary<UnlockFlowDialogModes, CanvasGroup>();

	private Dictionary<UnlockFlowDialogModes, Tweener> _modeTweens = new Dictionary<UnlockFlowDialogModes, Tweener>();

	private Dictionary<UnlockFlowDialogModes, UnlockFlowDialog.MenuSelectionTarget> firstSelectionMap = new Dictionary<UnlockFlowDialogModes, UnlockFlowDialog.MenuSelectionTarget>();

	private MenuItemList confirmMenu;

	private MenuItemList errorMenu;

	private MenuItemList completeMenu;

	private bool useTimer;

	private float endFlowTime;

	private Dictionary<CharacterID, SoundKey> characterPurchaseMapping = new Dictionary<CharacterID, SoundKey>
	{
		{
			CharacterID.Ashani,
			SoundKey.ashaniStore_purchaseItem
		},
		{
			CharacterID.Kidd,
			SoundKey.kiddStore_purchaseItem
		},
		{
			CharacterID.Xana,
			SoundKey.xanaStore_purchaseItem
		},
		{
			CharacterID.Raymer,
			SoundKey.raymerStore_purchaseItem
		},
		{
			CharacterID.Zhurong,
			SoundKey.zhurongStore_purchaseItem
		},
		{
			CharacterID.AfiGalu,
			SoundKey.afiStore_purchaseItem
		},
		{
			CharacterID.Weishan,
			SoundKey.weishanStore_purchaseItem
		}
	};

	[Inject]
	public IEvents events
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
	public GameDataManager gameDataManger
	{
		get;
		set;
	}

	public Action ConfirmCallback
	{
		get;
		set;
	}

	public Action CancelCallback
	{
		get;
		set;
	}

	public Action EquipCallback
	{
		get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		this.configure();
		this.addConfirmMenu();
		this.addErrorMenu();
		this.addCompleteMenu();
		this.mapMenus();
		base.UseOverrideOpenSound = true;
		base.OverrideOpenSound = this.soundFileManager.GetSoundAsAudioData(SoundKey.store_purchaseDialogOpened);
	}

	private void Update()
	{
		if (!this.useTimer)
		{
			return;
		}
		float num = Math.Max(0f, this.endFlowTime - Time.realtimeSinceStartup);
		StringBuilder stringBuilder = new StringBuilder();
		if (num > 5f)
		{
			this.TimerText.color = Color.white;
			TimeUtil.FormatTime((float)((int)Math.Floor((double)num)), stringBuilder, 0, true);
			this.TimerText.text = stringBuilder.ToString();
		}
		else
		{
			float num2 = 1f - num % 1f;
			this.TimerText.color = new Color(1f, num2, num2);
			TimeUtil.FormatTime(num, stringBuilder, -10, true);
			this.TimerText.text = stringBuilder.ToString();
		}
		if (num == 0f && this.currentMode == UnlockFlowDialogModes.CONFIRM)
		{
			this.Close();
		}
	}

	private void addConfirmMenu()
	{
		this.confirmMenu = base.injector.GetInstance<MenuItemList>();
		this.confirmMenu.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		this.confirmMenu.AddButton(this.ConfirmButton, new Action(this.OnConfirmButton));
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
		this.completeMenu.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		this.completeMenu.AddButton(this.SuccessEquipButton, new Action(this.OnEquip));
		this.completeMenu.AddButton(this.SuccessContinueButton, new Action(this.OnClose));
		this.completeMenu.Initialize();
	}

	private void mapMenus()
	{
		this.firstSelectionMap[UnlockFlowDialogModes.CONFIRM] = new UnlockFlowDialog.MenuSelectionTarget(this.confirmMenu, this.ConfirmButton, null);
		this.firstSelectionMap[UnlockFlowDialogModes.ERROR] = new UnlockFlowDialog.MenuSelectionTarget(this.errorMenu, this.ErrorContinueButton, null);
		this.firstSelectionMap[UnlockFlowDialogModes.LOADING] = new UnlockFlowDialog.MenuSelectionTarget(null, null, null);
		this.firstSelectionMap[UnlockFlowDialogModes.COMPLETE] = new UnlockFlowDialog.MenuSelectionTarget(this.completeMenu, this.SuccessEquipButton, this.SuccessContinueButton);
	}

	private void configure()
	{
		this.modeMap[UnlockFlowDialogModes.CONFIRM] = this.ConfirmPrompt;
		this.modeMap[UnlockFlowDialogModes.ERROR] = this.ErrorResult;
		this.modeMap[UnlockFlowDialogModes.LOADING] = this.LoadingSpinny;
		this.modeMap[UnlockFlowDialogModes.COMPLETE] = this.SuccessResult;
		foreach (CanvasGroup current in this.modeMap.Values)
		{
			current.gameObject.SetActive(false);
		}
	}

	public override void OnCancelPressed()
	{
		this.onCancelButton();
	}

	public void OnConfirmButton()
	{
		if (this.ConfirmCallback != null)
		{
			this.ConfirmCallback();
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

	public void OnEquip()
	{
		if (this.EquipCallback != null)
		{
			this.EquipCallback();
		}
		this.Close();
	}

	public override void Close()
	{
		base.Close();
		Action closeCallback = base.CloseCallback;
		base.CloseCallback = null;
		this.ConfirmCallback = null;
		this.CancelCallback = null;
		if (closeCallback != null)
		{
			closeCallback();
		}
	}

	public void LoadConfirmationText(string title, string body, string confirmText, string cancelText, bool useTimer, float endTime)
	{
		this.Title.text = title;
		this.Body.text = body;
		this.ConfirmButtonText.text = confirmText;
		this.CancelButtonText.text = cancelText;
		this.useTimer = useTimer;
		this.endFlowTime = endTime;
		this.TimerText.gameObject.SetActive(useTimer);
	}

	public void LoadSpinnyText(string title)
	{
		this.LoadingSpinnyTitle.text = title;
	}

	public void LoadStaticErrorText(string confirmText)
	{
		this.ErrorResultConfirm.text = confirmText;
	}

	public void LoadSuccess(string title, string body, string confirm, string equip, bool useEquipButton)
	{
		this.SuccessResultTitle.text = title;
		this.SuccessResultBody.text = body;
		this.SuccessResultConfirm.text = confirm;
		this.SuccessResultEquip.text = equip;
		this.SuccessEquipButton.gameObject.SetActive(useEquipButton);
		this.completeMenu.SetButtonEnabled(this.SuccessEquipButton, useEquipButton);
	}

	public void ShowError(string title, string body)
	{
		this.ErrorResultTitle.text = title;
		this.ErrorResultBody.text = body;
		this.SwitchMode(UnlockFlowDialogModes.ERROR);
	}

	public void ShowSuccess(CharacterID characterPurchaseSound = CharacterID.None)
	{
		this.SwitchMode(UnlockFlowDialogModes.COMPLETE);
		base.audioManager.PlayMenuSound(SoundKey.store_purchaseConfirmed, 0f);
		this.playCharacterLevelUpSound(characterPurchaseSound);
	}

	private void playCharacterLevelUpSound(CharacterID character)
	{
		if (this.characterPurchaseMapping.ContainsKey(character))
		{
			base.audioManager.PlayMenuSound(this.characterPurchaseMapping[character], this.gameDataManger.ConfigData.soundSettings.purchaseCharacterItemDelay);
		}
		else if (character != CharacterID.None && character != CharacterID.Any)
		{
			UnityEngine.Debug.LogError("No Level up sound mapped for this character. Add to the mapping here.");
		}
	}

	public void SwitchMode(UnlockFlowDialogModes mode)
	{
		if (this.currentMode != UnlockFlowDialogModes.NONE)
		{
			this.tweenOut(this.currentMode);
		}
		this.currentMode = mode;
		if (this.currentMode != UnlockFlowDialogModes.NONE)
		{
			this.tweenIn(this.currentMode);
		}
	}

	private void tweenOut(UnlockFlowDialogModes mode)
	{
		UnlockFlowDialog._tweenOut_c__AnonStorey0 _tweenOut_c__AnonStorey = new UnlockFlowDialog._tweenOut_c__AnonStorey0();
		_tweenOut_c__AnonStorey.mode = mode;
		_tweenOut_c__AnonStorey._this = this;
		this.killTweens(_tweenOut_c__AnonStorey.mode);
		_tweenOut_c__AnonStorey.target = this.modeMap[_tweenOut_c__AnonStorey.mode];
		this._modeTweens[_tweenOut_c__AnonStorey.mode] = DOTween.To(new DOGetter<float>(_tweenOut_c__AnonStorey.__m__0), new DOSetter<float>(_tweenOut_c__AnonStorey.__m__1), 0f, 0.05f).SetEase(Ease.Linear).OnComplete(new TweenCallback(_tweenOut_c__AnonStorey.__m__2));
	}

	private void tweenIn(UnlockFlowDialogModes mode)
	{
		UnlockFlowDialog._tweenIn_c__AnonStorey1 _tweenIn_c__AnonStorey = new UnlockFlowDialog._tweenIn_c__AnonStorey1();
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
		if (this.currentMode != UnlockFlowDialogModes.NONE)
		{
			MenuItemList menu = this.firstSelectionMap[this.currentMode].menu;
			MenuItemButton menuItemButton = this.firstSelectionMap[this.currentMode].primaryButton;
			if (menu != null)
			{
				this.FirstSelected = menuItemButton.InteractableButton.gameObject;
				if (!this.FirstSelected.activeInHierarchy)
				{
					menuItemButton = this.firstSelectionMap[this.currentMode].secondaryButton;
					this.FirstSelected = menuItemButton.InteractableButton.gameObject;
				}
				menu.AutoSelect(menuItemButton);
			}
			else
			{
				this.FirstSelected = null;
			}
		}
	}

	private void killTweens(UnlockFlowDialogModes mode)
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
