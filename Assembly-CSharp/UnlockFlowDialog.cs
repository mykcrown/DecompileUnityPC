using System;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

// Token: 0x02000A06 RID: 2566
public class UnlockFlowDialog : BaseWindow, IPurchaseResponseDialog
{
	// Token: 0x170011B9 RID: 4537
	// (get) Token: 0x06004A3C RID: 19004 RVA: 0x0013EBEC File Offset: 0x0013CFEC
	// (set) Token: 0x06004A3D RID: 19005 RVA: 0x0013EBF4 File Offset: 0x0013CFF4
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x170011BA RID: 4538
	// (get) Token: 0x06004A3E RID: 19006 RVA: 0x0013EBFD File Offset: 0x0013CFFD
	// (set) Token: 0x06004A3F RID: 19007 RVA: 0x0013EC05 File Offset: 0x0013D005
	[Inject]
	public ISoundFileManager soundFileManager { get; set; }

	// Token: 0x170011BB RID: 4539
	// (get) Token: 0x06004A40 RID: 19008 RVA: 0x0013EC0E File Offset: 0x0013D00E
	// (set) Token: 0x06004A41 RID: 19009 RVA: 0x0013EC16 File Offset: 0x0013D016
	[Inject]
	public GameDataManager gameDataManger { get; set; }

	// Token: 0x170011BC RID: 4540
	// (get) Token: 0x06004A42 RID: 19010 RVA: 0x0013EC1F File Offset: 0x0013D01F
	// (set) Token: 0x06004A43 RID: 19011 RVA: 0x0013EC27 File Offset: 0x0013D027
	public Action ConfirmCallback { get; set; }

	// Token: 0x170011BD RID: 4541
	// (get) Token: 0x06004A44 RID: 19012 RVA: 0x0013EC30 File Offset: 0x0013D030
	// (set) Token: 0x06004A45 RID: 19013 RVA: 0x0013EC38 File Offset: 0x0013D038
	public Action CancelCallback { get; set; }

	// Token: 0x170011BE RID: 4542
	// (get) Token: 0x06004A46 RID: 19014 RVA: 0x0013EC41 File Offset: 0x0013D041
	// (set) Token: 0x06004A47 RID: 19015 RVA: 0x0013EC49 File Offset: 0x0013D049
	public Action EquipCallback { get; set; }

	// Token: 0x06004A48 RID: 19016 RVA: 0x0013EC52 File Offset: 0x0013D052
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

	// Token: 0x06004A49 RID: 19017 RVA: 0x0013EC8C File Offset: 0x0013D08C
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

	// Token: 0x06004A4A RID: 19018 RVA: 0x0013ED64 File Offset: 0x0013D164
	private void addConfirmMenu()
	{
		this.confirmMenu = base.injector.GetInstance<MenuItemList>();
		this.confirmMenu.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		this.confirmMenu.AddButton(this.ConfirmButton, new Action(this.OnConfirmButton));
		this.confirmMenu.AddButton(this.CancelButton, new Action(this.onCancelButton));
		this.confirmMenu.Initialize();
	}

	// Token: 0x06004A4B RID: 19019 RVA: 0x0013EDD4 File Offset: 0x0013D1D4
	private void addErrorMenu()
	{
		this.errorMenu = base.injector.GetInstance<MenuItemList>();
		this.errorMenu.AddButton(this.ErrorContinueButton, new Action(this.onCancelButton));
		this.errorMenu.Initialize();
	}

	// Token: 0x06004A4C RID: 19020 RVA: 0x0013EE10 File Offset: 0x0013D210
	private void addCompleteMenu()
	{
		this.completeMenu = base.injector.GetInstance<MenuItemList>();
		this.completeMenu.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		this.completeMenu.AddButton(this.SuccessEquipButton, new Action(this.OnEquip));
		this.completeMenu.AddButton(this.SuccessContinueButton, new Action(this.OnClose));
		this.completeMenu.Initialize();
	}

	// Token: 0x06004A4D RID: 19021 RVA: 0x0013EE80 File Offset: 0x0013D280
	private void mapMenus()
	{
		this.firstSelectionMap[UnlockFlowDialogModes.CONFIRM] = new UnlockFlowDialog.MenuSelectionTarget(this.confirmMenu, this.ConfirmButton, null);
		this.firstSelectionMap[UnlockFlowDialogModes.ERROR] = new UnlockFlowDialog.MenuSelectionTarget(this.errorMenu, this.ErrorContinueButton, null);
		this.firstSelectionMap[UnlockFlowDialogModes.LOADING] = new UnlockFlowDialog.MenuSelectionTarget(null, null, null);
		this.firstSelectionMap[UnlockFlowDialogModes.COMPLETE] = new UnlockFlowDialog.MenuSelectionTarget(this.completeMenu, this.SuccessEquipButton, this.SuccessContinueButton);
	}

	// Token: 0x06004A4E RID: 19022 RVA: 0x0013EF00 File Offset: 0x0013D300
	private void configure()
	{
		this.modeMap[UnlockFlowDialogModes.CONFIRM] = this.ConfirmPrompt;
		this.modeMap[UnlockFlowDialogModes.ERROR] = this.ErrorResult;
		this.modeMap[UnlockFlowDialogModes.LOADING] = this.LoadingSpinny;
		this.modeMap[UnlockFlowDialogModes.COMPLETE] = this.SuccessResult;
		foreach (CanvasGroup canvasGroup in this.modeMap.Values)
		{
			canvasGroup.gameObject.SetActive(false);
		}
	}

	// Token: 0x06004A4F RID: 19023 RVA: 0x0013EFB0 File Offset: 0x0013D3B0
	public override void OnCancelPressed()
	{
		this.onCancelButton();
	}

	// Token: 0x06004A50 RID: 19024 RVA: 0x0013EFB8 File Offset: 0x0013D3B8
	public void OnConfirmButton()
	{
		if (this.ConfirmCallback != null)
		{
			this.ConfirmCallback();
		}
	}

	// Token: 0x06004A51 RID: 19025 RVA: 0x0013EFD0 File Offset: 0x0013D3D0
	private void onCancelButton()
	{
		this.OnCancelUnlock();
	}

	// Token: 0x06004A52 RID: 19026 RVA: 0x0013EFD8 File Offset: 0x0013D3D8
	public void OnCancelUnlock()
	{
		if (this.CancelCallback != null)
		{
			this.CancelCallback();
		}
		this.Close();
	}

	// Token: 0x06004A53 RID: 19027 RVA: 0x0013EFF6 File Offset: 0x0013D3F6
	public void OnClose()
	{
		this.Close();
	}

	// Token: 0x06004A54 RID: 19028 RVA: 0x0013EFFE File Offset: 0x0013D3FE
	public void OnEquip()
	{
		if (this.EquipCallback != null)
		{
			this.EquipCallback();
		}
		this.Close();
	}

	// Token: 0x06004A55 RID: 19029 RVA: 0x0013F01C File Offset: 0x0013D41C
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

	// Token: 0x06004A56 RID: 19030 RVA: 0x0013F058 File Offset: 0x0013D458
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

	// Token: 0x06004A57 RID: 19031 RVA: 0x0013F0B8 File Offset: 0x0013D4B8
	public void LoadSpinnyText(string title)
	{
		this.LoadingSpinnyTitle.text = title;
	}

	// Token: 0x06004A58 RID: 19032 RVA: 0x0013F0C6 File Offset: 0x0013D4C6
	public void LoadStaticErrorText(string confirmText)
	{
		this.ErrorResultConfirm.text = confirmText;
	}

	// Token: 0x06004A59 RID: 19033 RVA: 0x0013F0D4 File Offset: 0x0013D4D4
	public void LoadSuccess(string title, string body, string confirm, string equip, bool useEquipButton)
	{
		this.SuccessResultTitle.text = title;
		this.SuccessResultBody.text = body;
		this.SuccessResultConfirm.text = confirm;
		this.SuccessResultEquip.text = equip;
		this.SuccessEquipButton.gameObject.SetActive(useEquipButton);
		this.completeMenu.SetButtonEnabled(this.SuccessEquipButton, useEquipButton);
	}

	// Token: 0x06004A5A RID: 19034 RVA: 0x0013F137 File Offset: 0x0013D537
	public void ShowError(string title, string body)
	{
		this.ErrorResultTitle.text = title;
		this.ErrorResultBody.text = body;
		this.SwitchMode(UnlockFlowDialogModes.ERROR);
	}

	// Token: 0x06004A5B RID: 19035 RVA: 0x0013F158 File Offset: 0x0013D558
	public void ShowSuccess(CharacterID characterPurchaseSound = CharacterID.None)
	{
		this.SwitchMode(UnlockFlowDialogModes.COMPLETE);
		base.audioManager.PlayMenuSound(SoundKey.store_purchaseConfirmed, 0f);
		this.playCharacterLevelUpSound(characterPurchaseSound);
	}

	// Token: 0x06004A5C RID: 19036 RVA: 0x0013F17C File Offset: 0x0013D57C
	private void playCharacterLevelUpSound(CharacterID character)
	{
		if (this.characterPurchaseMapping.ContainsKey(character))
		{
			base.audioManager.PlayMenuSound(this.characterPurchaseMapping[character], this.gameDataManger.ConfigData.soundSettings.purchaseCharacterItemDelay);
		}
		else if (character != CharacterID.None && character != CharacterID.Any)
		{
			Debug.LogError("No Level up sound mapped for this character. Add to the mapping here.");
		}
	}

	// Token: 0x06004A5D RID: 19037 RVA: 0x0013F1E3 File Offset: 0x0013D5E3
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

	// Token: 0x06004A5E RID: 19038 RVA: 0x0013F21C File Offset: 0x0013D61C
	private void tweenOut(UnlockFlowDialogModes mode)
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

	// Token: 0x06004A5F RID: 19039 RVA: 0x0013F2B0 File Offset: 0x0013D6B0
	private void tweenIn(UnlockFlowDialogModes mode)
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

	// Token: 0x06004A60 RID: 19040 RVA: 0x0013F36C File Offset: 0x0013D76C
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

	// Token: 0x06004A61 RID: 19041 RVA: 0x0013F414 File Offset: 0x0013D814
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

	// Token: 0x040030D2 RID: 12498
	public TextMeshProUGUI Body;

	// Token: 0x040030D3 RID: 12499
	public TextMeshProUGUI Title;

	// Token: 0x040030D4 RID: 12500
	public TextMeshProUGUI TimerText;

	// Token: 0x040030D5 RID: 12501
	public TextMeshProUGUI ConfirmButtonText;

	// Token: 0x040030D6 RID: 12502
	public TextMeshProUGUI CancelButtonText;

	// Token: 0x040030D7 RID: 12503
	public CanvasGroup ConfirmPrompt;

	// Token: 0x040030D8 RID: 12504
	public CanvasGroup LoadingSpinny;

	// Token: 0x040030D9 RID: 12505
	public CanvasGroup ErrorResult;

	// Token: 0x040030DA RID: 12506
	public CanvasGroup SuccessResult;

	// Token: 0x040030DB RID: 12507
	public TextMeshProUGUI LoadingSpinnyTitle;

	// Token: 0x040030DC RID: 12508
	public TextMeshProUGUI ErrorResultTitle;

	// Token: 0x040030DD RID: 12509
	public TextMeshProUGUI ErrorResultBody;

	// Token: 0x040030DE RID: 12510
	public TextMeshProUGUI ErrorResultConfirm;

	// Token: 0x040030DF RID: 12511
	public TextMeshProUGUI SuccessResultTitle;

	// Token: 0x040030E0 RID: 12512
	public TextMeshProUGUI SuccessResultBody;

	// Token: 0x040030E1 RID: 12513
	public TextMeshProUGUI SuccessResultConfirm;

	// Token: 0x040030E2 RID: 12514
	public TextMeshProUGUI SuccessResultEquip;

	// Token: 0x040030E3 RID: 12515
	public MenuItemButton ConfirmButton;

	// Token: 0x040030E4 RID: 12516
	public MenuItemButton CancelButton;

	// Token: 0x040030E5 RID: 12517
	public MenuItemButton ErrorContinueButton;

	// Token: 0x040030E6 RID: 12518
	public MenuItemButton SuccessContinueButton;

	// Token: 0x040030E7 RID: 12519
	public MenuItemButton SuccessEquipButton;

	// Token: 0x040030EB RID: 12523
	private UnlockFlowDialogModes currentMode;

	// Token: 0x040030EC RID: 12524
	private Dictionary<UnlockFlowDialogModes, CanvasGroup> modeMap = new Dictionary<UnlockFlowDialogModes, CanvasGroup>();

	// Token: 0x040030ED RID: 12525
	private Dictionary<UnlockFlowDialogModes, Tweener> _modeTweens = new Dictionary<UnlockFlowDialogModes, Tweener>();

	// Token: 0x040030EE RID: 12526
	private Dictionary<UnlockFlowDialogModes, UnlockFlowDialog.MenuSelectionTarget> firstSelectionMap = new Dictionary<UnlockFlowDialogModes, UnlockFlowDialog.MenuSelectionTarget>();

	// Token: 0x040030EF RID: 12527
	private MenuItemList confirmMenu;

	// Token: 0x040030F0 RID: 12528
	private MenuItemList errorMenu;

	// Token: 0x040030F1 RID: 12529
	private MenuItemList completeMenu;

	// Token: 0x040030F2 RID: 12530
	private bool useTimer;

	// Token: 0x040030F3 RID: 12531
	private float endFlowTime;

	// Token: 0x040030F4 RID: 12532
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

	// Token: 0x02000A07 RID: 2567
	private class MenuSelectionTarget
	{
		// Token: 0x06004A62 RID: 19042 RVA: 0x0013F467 File Offset: 0x0013D867
		public MenuSelectionTarget(MenuItemList menu, MenuItemButton primaryButton, MenuItemButton secondaryButton = null)
		{
			this.menu = menu;
			this.primaryButton = primaryButton;
			this.secondaryButton = secondaryButton;
		}

		// Token: 0x040030F5 RID: 12533
		public MenuItemList menu;

		// Token: 0x040030F6 RID: 12534
		public MenuItemButton primaryButton;

		// Token: 0x040030F7 RID: 12535
		public MenuItemButton secondaryButton;
	}
}
