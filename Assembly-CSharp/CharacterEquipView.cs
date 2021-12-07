using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020009D5 RID: 2517
public class CharacterEquipView : StoreTabWithEquipModule
{
	// Token: 0x170010EE RID: 4334
	// (get) Token: 0x06004695 RID: 18069 RVA: 0x00133FE9 File Offset: 0x001323E9
	// (set) Token: 0x06004696 RID: 18070 RVA: 0x00133FF1 File Offset: 0x001323F1
	[Inject]
	public ICharacterEquipViewAPI api { get; set; }

	// Token: 0x170010EF RID: 4335
	// (get) Token: 0x06004697 RID: 18071 RVA: 0x00133FFA File Offset: 0x001323FA
	// (set) Token: 0x06004698 RID: 18072 RVA: 0x00134002 File Offset: 0x00132402
	[Inject]
	public IDetailedUnlockCharacterFlow detailedUnlockCharacterFlow { get; set; }

	// Token: 0x170010F0 RID: 4336
	// (get) Token: 0x06004699 RID: 18073 RVA: 0x0013400B File Offset: 0x0013240B
	// (set) Token: 0x0600469A RID: 18074 RVA: 0x00134013 File Offset: 0x00132413
	[Inject("CharacterEquipView")]
	public IEquipModuleAPI equipModuleAPI { get; set; }

	// Token: 0x170010F1 RID: 4337
	// (get) Token: 0x0600469B RID: 18075 RVA: 0x0013401C File Offset: 0x0013241C
	// (set) Token: 0x0600469C RID: 18076 RVA: 0x00134024 File Offset: 0x00132424
	[Inject]
	public ICharactersTabAPI charactersTabAPI { get; set; }

	// Token: 0x170010F2 RID: 4338
	// (get) Token: 0x0600469D RID: 18077 RVA: 0x0013402D File Offset: 0x0013242D
	// (set) Token: 0x0600469E RID: 18078 RVA: 0x00134035 File Offset: 0x00132435
	[Inject]
	public ICharacterDataHelper characterDataHelper { get; set; }

	// Token: 0x170010F3 RID: 4339
	// (get) Token: 0x0600469F RID: 18079 RVA: 0x0013403E File Offset: 0x0013243E
	// (set) Token: 0x060046A0 RID: 18080 RVA: 0x00134046 File Offset: 0x00132446
	[Inject]
	public ICharacterLists characterLists { get; set; }

	// Token: 0x170010F4 RID: 4340
	// (get) Token: 0x060046A1 RID: 18081 RVA: 0x0013404F File Offset: 0x0013244F
	// (set) Token: 0x060046A2 RID: 18082 RVA: 0x00134057 File Offset: 0x00132457
	[Inject]
	public ICharacterMenusDataLoader characterMenusDataLoader { private get; set; }

	// Token: 0x170010F5 RID: 4341
	// (get) Token: 0x060046A3 RID: 18083 RVA: 0x00134060 File Offset: 0x00132460
	// (set) Token: 0x060046A4 RID: 18084 RVA: 0x00134068 File Offset: 0x00132468
	[Inject]
	public IPreviewAnimationHelper previewAnimationHelper { get; set; }

	// Token: 0x170010F6 RID: 4342
	// (get) Token: 0x060046A5 RID: 18085 RVA: 0x00134071 File Offset: 0x00132471
	// (set) Token: 0x060046A6 RID: 18086 RVA: 0x00134079 File Offset: 0x00132479
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x170010F7 RID: 4343
	// (get) Token: 0x060046A7 RID: 18087 RVA: 0x00134082 File Offset: 0x00132482
	// (set) Token: 0x060046A8 RID: 18088 RVA: 0x0013408A File Offset: 0x0013248A
	[Inject]
	public IStoreTabsModel storeTabAPI { get; set; }

	// Token: 0x170010F8 RID: 4344
	// (get) Token: 0x060046A9 RID: 18089 RVA: 0x00134093 File Offset: 0x00132493
	// (set) Token: 0x060046AA RID: 18090 RVA: 0x0013409B File Offset: 0x0013249B
	[Inject]
	public ICollectiblesTabAPI collectiblesTabAPI { get; set; }

	// Token: 0x060046AB RID: 18091 RVA: 0x001340A4 File Offset: 0x001324A4
	[PostConstruct]
	public override void Init()
	{
		base.Init();
		this.storeScene = base.uiAdapter.GetUIScene<StoreScene3D>();
		this.MainMode.Init(this.storeScene);
		this.GridMode.Init(this.storeScene);
		this.configureModules();
		this.mapSpriteToEquip(EquipmentTypes.SKIN, this.SkinsIcon);
		this.mapSpriteToEquip(EquipmentTypes.EMOTE, this.EmotesIcon);
		this.mapSpriteToEquip(EquipmentTypes.VOICE_TAUNT, this.VoiceLinesIcon);
		this.mapSpriteToEquip(EquipmentTypes.HOLOGRAM, this.SpraysIcon);
		this.mapSpriteToEquip(EquipmentTypes.VICTORY_POSE, this.VictoryPosesIcon);
		this.mapSpriteToEquip(EquipmentTypes.PLATFORM, this.PlatformsIcon);
		this.equipModuleAPI.LoadTypeList(this.api.GetValidEquipTypes());
		this.addCharacterMiniMenu();
		this.addEquipment();
		this.addCharacterUnlockButton();
		this.CharacterSwapButton.InteractableButton.Unselectable = true;
		this.CharacterSwapButton.Submit = new Action<MenuItemButton, InputEventData>(this.clickedCharacterSwapButton);
		this.EquipModule.GetTypeText = new Func<string, string>(this.getTypeText);
		this.EquipModule.ListFilter = new Func<List<EquipmentLine>, List<EquipmentLine>>(this.listFilter);
		this.EquipModule.IsAllowButtons = new Func<bool>(this.isAllowButtons);
		this.EquipModule.OnClickedEquipmentWithButtonsDisabled = new Action(this.promptUnlockCharacter);
		MenuItemList menuItemList = this.characterUnlockButtonMenu;
		menuItemList.OnSelected = (Action<MenuItemButton, BaseEventData>)Delegate.Combine(menuItemList.OnSelected, new Action<MenuItemButton, BaseEventData>(this.onCharacterUnlockMenuSelected));
		MenuItemList menuItemList2 = this.characterUnlockButtonMenu;
		menuItemList2.OnDeselected = (Action<MenuItemButton, BaseEventData>)Delegate.Combine(menuItemList2.OnDeselected, new Action<MenuItemButton, BaseEventData>(this.onCharacterUnlockMenuDeselected));
		this.EquipModule.AddRightEdgeNavigation(this.characterUnlockButtonMenu);
		MenuItemButton[] buttons = this.characterUnlockButtonMenu.GetButtons();
		this.characterUnlockButtonMenu.LandingPoint = buttons[0];
		CursorTargetButton collectiblesArrowButton = this.CollectiblesArrowButton;
		collectiblesArrowButton.ClickCallback = (Action<CursorTargetButton>)Delegate.Combine(collectiblesArrowButton.ClickCallback, new Action<CursorTargetButton>(this.onCollectiblesArrowClicked));
		this.initialPosition = base.transform.localPosition;
		base.listen(CharacterEquipViewAPI.UPDATED, new Action(this.onDataUpdate));
		base.listen(EquipmentSelectorAPI.UPDATED, new Action(this.onDataUpdate));
		base.listen(UserCharacterUnlockModel.UPDATED, new Action(this.onDataUpdate));
		base.listen(UserProAccountUnlockedModel.UPDATED, new Action(this.onDataUpdate));
		base.listen(UserInventoryModel.UPDATED, new Action(this.onEquipmentUpdate));
		base.listen(UserTauntsModel.UPDATED, new Action(this.onEquipmentUpdate));
		base.listen(UserCharacterEquippedModel.UPDATED, new Action(this.onEquipmentUpdate));
		base.listen(EquipmentSelectorModule.RESELECT_ITEM, new Action(this.onReselectItem));
		base.listen("CharactersTabAPI.UPDATED", new Action(this.onDataUpdate));
		base.listen("StoreTabSelectionModel.STORE_TAB_UPDATED", new Action(this.onTabUpdate));
	}

	// Token: 0x060046AC RID: 18092 RVA: 0x00134381 File Offset: 0x00132781
	private void onCharacterUnlockMenuSelected(MenuItemButton button, BaseEventData eventData)
	{
		this.updateEquipModuleState();
	}

	// Token: 0x060046AD RID: 18093 RVA: 0x00134389 File Offset: 0x00132789
	private void onCharacterUnlockMenuDeselected(MenuItemButton button, BaseEventData eventData)
	{
		this.updateEquipModuleState();
	}

	// Token: 0x060046AE RID: 18094 RVA: 0x00134391 File Offset: 0x00132791
	private void onCollectiblesArrowClicked(CursorTargetButton button)
	{
		this.storeTabAPI.Current = StoreTab.COLLECTIBLES;
		this.collectiblesTabAPI.SetState(CollectiblesTabState.EquipView, true);
	}

	// Token: 0x060046AF RID: 18095 RVA: 0x001343AC File Offset: 0x001327AC
	private void configureModules()
	{
		base.injector.Inject(this.GridModule);
		this.module = new TwoModuleManager();
		(this.module as TwoModuleManager).AddModule(this.EquipModule);
		(this.module as TwoModuleManager).AddModule(this.GridModule);
		this.GridModule.itemDisplayMode = EquipmentSelectorModule.ItemDisplay.GRID;
		this.GridModule.lockItemType = EquipmentTypes.HOLOGRAM;
		this.EquipModule.forbidItemType = EquipmentTypes.HOLOGRAM;
		this.switchToModule(ModuleMode.MAIN);
	}

	// Token: 0x060046B0 RID: 18096 RVA: 0x0013442C File Offset: 0x0013282C
	private void switchToModule(ModuleMode mode)
	{
		if (this.currentModuleMode != mode)
		{
			EquipmentSelectorModule equipmentSelectorModule = (mode != ModuleMode.MAIN) ? this.GridModule : this.EquipModule;
			equipmentSelectorModule.Activate();
			(this.module as TwoModuleManager).SetActive(equipmentSelectorModule);
			this.MainMode.gameObject.SetActive(true);
			this.GridMode.gameObject.SetActive(true);
			if (mode == ModuleMode.MAIN)
			{
				this.animateTransition(this.GridMode, this.MainMode);
			}
			else
			{
				this.animateTransition(this.MainMode, this.GridMode);
			}
			this.currentModuleMode = mode;
			this.timer.EndOfFrame(new Action(this.syncModuleAtEndOfFrame));
		}
	}

	// Token: 0x060046B1 RID: 18097 RVA: 0x001344E5 File Offset: 0x001328E5
	private void updateEquipModuleState()
	{
		this.EquipModule.IsExternalMenuSelected = !this.isEquipModuleFocused();
	}

	// Token: 0x060046B2 RID: 18098 RVA: 0x001344FB File Offset: 0x001328FB
	private string getTypeText(string typeText)
	{
		return this.api.GetCharacterDisplayName(this.api.SelectedCharacter) + " " + typeText;
	}

	// Token: 0x060046B3 RID: 18099 RVA: 0x00134520 File Offset: 0x00132920
	private List<EquipmentLine> listFilter(List<EquipmentLine> inlist)
	{
		List<EquipmentLine> list = new List<EquipmentLine>();
		foreach (EquipmentLine equipmentLine in inlist)
		{
			bool flag = (this.equipModuleAPI.HasPrice(equipmentLine.Item) && equipmentLine.Item.promoted) || this.equipModuleAPI.HasItem(equipmentLine.Item.id);
			if (flag && (equipmentLine.Item.character == this.api.SelectedCharacter || equipmentLine.Item.character == CharacterID.Any))
			{
				list.Add(equipmentLine);
			}
		}
		return list;
	}

	// Token: 0x060046B4 RID: 18100 RVA: 0x001345F4 File Offset: 0x001329F4
	private void animateTransition(CharactersModuleMode from, CharactersModuleMode to)
	{
		float duration = 0.07f;
		this.killFromModuleTweens();
		to.transform.localPosition = Vector3.zero;
		to.Activate();
		this._fromModuleTween = DOTween.To(() => from.CanvasGroup.alpha, delegate(float valueIn)
		{
			from.CanvasGroup.alpha = valueIn;
		}, 0f, duration).SetEase(Ease.Linear);
		this._toModuleTween = DOTween.To(() => to.CanvasGroup.alpha, delegate(float valueIn)
		{
			to.CanvasGroup.alpha = valueIn;
		}, 1f, duration).SetEase(Ease.Linear).OnComplete(delegate
		{
			from.transform.localPosition = new Vector3(100000f, 0f, 0f);
			this.killFromModuleTweens();
		});
	}

	// Token: 0x060046B5 RID: 18101 RVA: 0x001346B7 File Offset: 0x00132AB7
	private void killFromModuleTweens()
	{
		TweenUtil.Destroy(ref this._fromModuleTween);
		TweenUtil.Destroy(ref this._toModuleTween);
	}

	// Token: 0x060046B6 RID: 18102 RVA: 0x001346CF File Offset: 0x00132ACF
	protected override bool isEquipViewActive()
	{
		return this.charactersTabAPI.GetState() == CharactersTabState.EquipView;
	}

	// Token: 0x060046B7 RID: 18103 RVA: 0x001346E0 File Offset: 0x00132AE0
	private void syncModuleAtEndOfFrame()
	{
		if (this == null)
		{
			return;
		}
		EquipmentSelectorModule current = (this.module as TwoModuleManager).GetCurrent();
		current.Activate();
		current.ForceSyncButtonSelection();
		current.ForceRedraws();
	}

	// Token: 0x060046B8 RID: 18104 RVA: 0x0013471D File Offset: 0x00132B1D
	private bool isAllowButtons()
	{
		return true;
	}

	// Token: 0x060046B9 RID: 18105 RVA: 0x00134720 File Offset: 0x00132B20
	public void OnDrawComplete()
	{
		this.module.OnDrawComplete();
		this.onDataUpdate();
	}

	// Token: 0x060046BA RID: 18106 RVA: 0x00134733 File Offset: 0x00132B33
	public override void OnActivate()
	{
		this.updateEquipModuleState();
		base.OnActivate();
	}

	// Token: 0x060046BB RID: 18107 RVA: 0x00134741 File Offset: 0x00132B41
	private void mapSpriteToEquip(EquipmentTypes id, Sprite icon)
	{
		this.equipModuleAPI.MapEquipTypeIcon(id, icon);
	}

	// Token: 0x060046BC RID: 18108 RVA: 0x00134750 File Offset: 0x00132B50
	private void addEquipment()
	{
		this.EquipModule.Init(this.equipModuleAPI);
		this.GridModule.Init(this.equipModuleAPI);
		List<EquippableItem> list = new List<EquippableItem>();
		List<EquippableItem> list2 = new List<EquippableItem>();
		foreach (CharacterID characterId in this.api.GetCharacters())
		{
			foreach (EquipTypeDefinition equipTypeDefinition in this.equipModuleAPI.GetValidEquipTypes())
			{
				foreach (EquippableItem item in this.api.GetItems(characterId, equipTypeDefinition.type))
				{
					list.Add(item);
				}
			}
		}
		foreach (EquipTypeDefinition equipTypeDefinition2 in this.equipModuleAPI.GetValidEquipTypes())
		{
			foreach (EquippableItem equippableItem in this.api.GetItems(CharacterID.Any, equipTypeDefinition2.type))
			{
				if (CharacterEquipView.gridModeEquipmentTypes.Contains(equippableItem.type))
				{
					list2.Add(equippableItem);
				}
				else
				{
					list.Add(equippableItem);
				}
			}
		}
		this.EquipModule.LoadItems(list);
		this.GridModule.LoadItems(list2);
	}

	// Token: 0x060046BD RID: 18109 RVA: 0x001348C4 File Offset: 0x00132CC4
	private void addCharacterUnlockButton()
	{
		this.characterUnlockButtonMenu = base.injector.GetInstance<MenuItemList>();
		this.characterUnlockButtonMenu.SetNavigationType(MenuItemList.NavigationType.InOrderVertical, 0);
		this.characterUnlockButtonMenu.AddButton(this.UnlockCharacterButton, new Action(this.clickedUnlockCharacterButton));
		this.UnlockCharacterButton.DisableType = ButtonAnimator.VisualDisableType.DisappearCanvas;
		this.characterUnlockButtonMenu.Initialize();
	}

	// Token: 0x060046BE RID: 18110 RVA: 0x00134924 File Offset: 0x00132D24
	private void addCharacterMiniMenu()
	{
		this.characterMiniMenu = base.injector.GetInstance<MenuItemList>();
		this.characterMiniMenu.MouseOnly = true;
		this.characterMiniMenu.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		foreach (CharacterID characterId in this.api.GetCharacters())
		{
			this.addCharacter(characterId);
		}
		this.characterMiniMenu.Initialize();
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.InputInstructionsRightBumperPrefab);
		this.instructionsRightBumper = gameObject.GetComponent<InputInstructions>();
		gameObject.transform.SetParent(this.InputInstructionsRightBumperAnchor, false);
		this.instructionsRightBumper.SetControlMode(base.uiManager.CurrentInputModule.CurrentMode);
	}

	// Token: 0x060046BF RID: 18111 RVA: 0x001349D8 File Offset: 0x00132DD8
	private void addCharacter(CharacterID characterId)
	{
		CharacterMiniButton component = UnityEngine.Object.Instantiate<GameObject>(this.CharacterMiniButtonPrefab).GetComponent<CharacterMiniButton>();
		this.characterMiniButtons[characterId] = component;
		this.characterButtonList.Add(characterId);
		component.character = characterId;
		component.transform.SetParent(this.CharacterMiniMenu.transform, false);
		component.CharacterImage.sprite = this.characterMenusDataLoader.GetData(characterId).miniPortrait;
		MenuItemButton component2 = component.GetComponent<MenuItemButton>();
		this.characterMiniMenu.AddButton(component2, delegate()
		{
			this.clickedCharacter(characterId);
		});
	}

	// Token: 0x060046C0 RID: 18112 RVA: 0x00134A90 File Offset: 0x00132E90
	private void clickedUnlockCharacterButton()
	{
		this.detailedUnlockCharacterFlow.Start(this.api.SelectedCharacter, new Action(this.onDataUpdate));
	}

	// Token: 0x060046C1 RID: 18113 RVA: 0x00134AB4 File Offset: 0x00132EB4
	private void promptUnlockCharacter()
	{
		this.detailedUnlockCharacterFlow.Start(this.api.SelectedCharacter, new Action(this.onDataUpdate));
	}

	// Token: 0x060046C2 RID: 18114 RVA: 0x00134AD8 File Offset: 0x00132ED8
	private void clickedCharacterSwapButton(MenuItemButton button, InputEventData eventData)
	{
		int clickedCharacterIndex = this.storeScene.GetClickedCharacterIndex(eventData.mousePosition);
		CharacterDefinition[] selectedCharacterLinkedCharacters = this.api.SelectedCharacterLinkedCharacters;
		bool flag = false;
		if (clickedCharacterIndex != -1 && selectedCharacterLinkedCharacters.Length > 1 && clickedCharacterIndex != this.api.SelectedCharacterIndex)
		{
			flag = true;
		}
		if (flag)
		{
			this.swapCharacter();
		}
		else
		{
			this.playItemAnimation();
		}
	}

	// Token: 0x060046C3 RID: 18115 RVA: 0x00134B40 File Offset: 0x00132F40
	public override void OnRightStickUp()
	{
		this.attemptSwapCharacter();
	}

	// Token: 0x060046C4 RID: 18116 RVA: 0x00134B48 File Offset: 0x00132F48
	public override void OnRightStickDown()
	{
		this.attemptSwapCharacter();
	}

	// Token: 0x060046C5 RID: 18117 RVA: 0x00134B50 File Offset: 0x00132F50
	private void attemptSwapCharacter()
	{
		CharacterDefinition characterDefinition = this.assetPreviewCharacter();
		CharacterDefinition[] linkedCharacters = this.characterDataHelper.GetLinkedCharacters(characterDefinition);
		if (characterDefinition != null && linkedCharacters.Length > 1)
		{
			this.swapCharacter();
		}
	}

	// Token: 0x060046C6 RID: 18118 RVA: 0x00134B8C File Offset: 0x00132F8C
	private void swapCharacter()
	{
		if (!this.storeScene.IsCharacterSwapping && !this.isViewingVictoryPose())
		{
			base.audioManager.PlayMenuSound(SoundKey.characterSelect_totemPartnerCycle, 0f);
			this.api.SelectedCharacterIndex = (this.api.SelectedCharacterIndex + 1) % 2;
		}
	}

	// Token: 0x060046C7 RID: 18119 RVA: 0x00134BE0 File Offset: 0x00132FE0
	private bool isViewingVictoryPose()
	{
		return this.equipModuleAPI.SelectedEquipType == EquipmentTypes.VICTORY_POSE && this.equipModuleAPI.SelectedEquipment != null;
	}

	// Token: 0x060046C8 RID: 18120 RVA: 0x00134C07 File Offset: 0x00133007
	private void selectUnlockCharacter()
	{
		this.characterUnlockButtonMenu.AutoSelect(this.UnlockCharacterButton);
	}

	// Token: 0x060046C9 RID: 18121 RVA: 0x00134C1A File Offset: 0x0013301A
	private void clickedCharacter(CharacterID characterId)
	{
		this.api.SelectedCharacter = characterId;
		base.audioManager.PlayMenuSound(SoundKey.store_miniCharacterSelected, 0f);
	}

	// Token: 0x060046CA RID: 18122 RVA: 0x00134C3A File Offset: 0x0013303A
	private void Update()
	{
		this.DisabledGallerySpinErrorContainer.SetActive(UIInputModule.DisableMenuRightStick);
	}

	// Token: 0x060046CB RID: 18123 RVA: 0x00134C4C File Offset: 0x0013304C
	private void onDataUpdate()
	{
		this.updateModule();
		this.updateCharacterSelected();
		this.updateAssetPreview();
		this.updateSideText();
		this.updateCharacterLockState();
		this.updateTabMode();
		this.checkItemAnimation();
	}

	// Token: 0x060046CC RID: 18124 RVA: 0x00134C78 File Offset: 0x00133078
	private void updateModule()
	{
		ModuleMode mode = (!CharacterEquipView.gridModeEquipmentTypes.Contains(this.equipModuleAPI.SelectedEquipType)) ? ModuleMode.MAIN : ModuleMode.GRID;
		this.switchToModule(mode);
	}

	// Token: 0x060046CD RID: 18125 RVA: 0x00134CAE File Offset: 0x001330AE
	private void onReselectItem()
	{
		this.playItemAnimation();
	}

	// Token: 0x060046CE RID: 18126 RVA: 0x00134CB6 File Offset: 0x001330B6
	private CharacterMiniButton findButtonForCharacter(CharacterID id)
	{
		return this.characterMiniButtons[id];
	}

	// Token: 0x060046CF RID: 18127 RVA: 0x00134CC4 File Offset: 0x001330C4
	private void updateCharacterSelected()
	{
		CharacterID selectedCharacter = this.api.SelectedCharacter;
		if (this.currentCharacter != selectedCharacter)
		{
			if (this.currentCharacter != CharacterID.None)
			{
				CharacterMiniButton characterMiniButton = this.findButtonForCharacter(this.currentCharacter);
				characterMiniButton.SelectedImage.gameObject.SetActive(false);
			}
			this.currentCharacter = selectedCharacter;
			CharacterMiniButton characterMiniButton2 = this.findButtonForCharacter(this.currentCharacter);
			characterMiniButton2.SelectedImage.gameObject.SetActive(true);
			this.UnlockCharacterButton.SetText(base.localization.GetText("ui.store.characters.unlockCharacter", this.api.GetCharacterDisplayName(this.api.SelectedCharacter)));
			this.ProAccountBody.text = base.localization.GetText("ui.store.characters.proAccount.body", this.api.GetProAccountPriceString());
			this.module.RebuildList();
		}
	}

	// Token: 0x060046D0 RID: 18128 RVA: 0x00134D9C File Offset: 0x0013319C
	private void updateCharacterLockState()
	{
		bool flag = !this.api.IsCharacterUnlocked(this.api.SelectedCharacter);
		this.characterUnlockButtonMenu.SetButtonEnabled(this.UnlockCharacterButton, flag);
		this.ProAccountSignage.SetActive(flag);
	}

	// Token: 0x060046D1 RID: 18129 RVA: 0x00134DE4 File Offset: 0x001331E4
	private CharacterDefinition assetPreviewCharacter()
	{
		if (this.equipModuleAPI.SelectedEquipType == EquipmentTypes.PLATFORM || this.equipModuleAPI.SelectedEquipType == EquipmentTypes.HOLOGRAM || this.equipModuleAPI.SelectedEquipType == EquipmentTypes.VOICE_TAUNT)
		{
			return null;
		}
		CharacterDefinition[] selectedCharacterLinkedCharacters = this.api.SelectedCharacterLinkedCharacters;
		return selectedCharacterLinkedCharacters[this.api.SelectedCharacterIndex];
	}

	// Token: 0x060046D2 RID: 18130 RVA: 0x00134E40 File Offset: 0x00133240
	private void updateAssetPreview()
	{
		CharacterDefinition characterDef = this.assetPreviewCharacter();
		if (this.equipModuleAPI.SelectedEquipType == EquipmentTypes.PLATFORM)
		{
			if (this.equipModuleAPI.SelectedEquipment == null)
			{
				this.storeScene.HideCharacterEquipObject();
			}
			else
			{
				this.storeScene.ShowPlatform(this.equipModuleAPI.SelectedEquipment);
			}
		}
		else if (this.equipModuleAPI.SelectedEquipType == EquipmentTypes.HOLOGRAM)
		{
			if (this.equipModuleAPI.SelectedEquipment == null)
			{
				this.storeScene.HideCharacterEquipObject();
			}
			else
			{
				this.storeScene.ShowHologram(this.equipModuleAPI.SelectedEquipment);
			}
		}
		else if (this.equipModuleAPI.SelectedEquipType == EquipmentTypes.VOICE_TAUNT)
		{
			if (this.equipModuleAPI.SelectedEquipment == null)
			{
				this.storeScene.HideCharacterEquipObject();
			}
			else
			{
				this.storeScene.ShowVoiceTaunt(this.equipModuleAPI.SelectedEquipment);
			}
		}
		else
		{
			SkinDefinition skinDef = this.api.EquippedSkin;
			if (this.equipModuleAPI.SelectedEquipment != null && this.equipModuleAPI.SelectedEquipment.type == EquipmentTypes.SKIN)
			{
				skinDef = this.api.GetSkinFromItem(this.equipModuleAPI.SelectedEquipment, this.api.SelectedCharacter);
			}
			CharacterMenusData data = this.characterMenusDataLoader.GetData(characterDef);
			this.storeScene.ShowCharacter(data, skinDef, this.equipModuleAPI.SelectedEquipment, this.api.SelectedCharacterIndex);
			CharacterMenusData.UICharacterAdjustments aligner = null;
			if (this.equipModuleAPI.SelectedEquipType == EquipmentTypes.EMOTE)
			{
				aligner = data.galleryAdjustmentsEmote;
			}
			else if (this.equipModuleAPI.SelectedEquipType == EquipmentTypes.VICTORY_POSE)
			{
				aligner = data.galleryAdjustmentsVictoryPose;
			}
			this.storeScene.SetTypeAligner(data, aligner);
			if (this.equipModuleAPI.SelectedEquipment != null && this.equipModuleAPI.SelectedEquipment.type == EquipmentTypes.VICTORY_POSE)
			{
				this.storeScene.SetMode(UIAssetDisplayMode.Centered);
			}
			else
			{
				this.storeScene.SetMode(UIAssetDisplayMode.OffsetRotate);
			}
		}
	}

	// Token: 0x060046D3 RID: 18131 RVA: 0x00135042 File Offset: 0x00133442
	private void onTabUpdate()
	{
		this.updateTabMode();
	}

	// Token: 0x060046D4 RID: 18132 RVA: 0x0013504C File Offset: 0x0013344C
	private void updateTabMode()
	{
		if (this.ZoomAnimator.gameObject.activeInHierarchy)
		{
			this.ZoomAnimator.SetBool("initialize", true);
			this.ZoomAnimator.SetBool("skip animation", this.charactersTabAPI.SkipAnimation);
			if (this.charactersTabAPI.GetState() == CharactersTabState.EquipView)
			{
				this.ZoomAnimator.SetBool("visible", true);
				base.transform.localPosition = this.initialPosition;
			}
			else
			{
				this.ZoomAnimator.SetBool("visible", false);
				base.transform.localPosition = this.initialPosition + new Vector3(0f, 2000f, 0f);
			}
		}
	}

	// Token: 0x060046D5 RID: 18133 RVA: 0x0013510D File Offset: 0x0013350D
	public void PlayTransition()
	{
		this.ZoomAnimator.SetBool("visible", true);
		base.transform.localPosition = this.initialPosition;
	}

	// Token: 0x060046D6 RID: 18134 RVA: 0x00135134 File Offset: 0x00133534
	private void playItemAnimation()
	{
		CharacterDefinition characterDefinition = this.assetPreviewCharacter();
		if (characterDefinition != null)
		{
			if (this.equipModuleAPI.SelectedEquipment == null)
			{
				List<WavedashAnimationData> allDefaultAnimations = this.characterDataHelper.GetAllDefaultAnimations(characterDefinition, CharacterDefaultAnimationKey.STORE_IDLE);
				this.storeScene.SetDefaultAnimations(UISceneAnimRequestHelper.GetAnimRequests(allDefaultAnimations));
			}
			else
			{
				List<WavedashAnimationData> allDefaultAnimations2 = this.characterDataHelper.GetAllDefaultAnimations(characterDefinition, CharacterDefaultAnimationKey.STORE_IDLE);
				if (this.equipModuleAPI.SelectedEquipType != EquipmentTypes.VICTORY_POSE)
				{
					this.storeScene.SetDefaultAnimations(UISceneAnimRequestHelper.GetAnimRequests(allDefaultAnimations2));
				}
				if (this.equipModuleAPI.SelectedEquipType == EquipmentTypes.VICTORY_POSE)
				{
					this.previewAnimationHelper.PlayVictoryPose(this.storeScene.GetCurrentCharacter(), characterDefinition, this.equipModuleAPI.SelectedEquipment);
				}
				else if (this.equipModuleAPI.SelectedEquipType == EquipmentTypes.SKIN)
				{
					List<WavedashAnimationData> allDefaultAnimations3 = this.characterDataHelper.GetAllDefaultAnimations(characterDefinition, CharacterDefaultAnimationKey.STORE_IDLE);
					this.storeScene.PlayCharacterAnimations(UISceneAnimRequestHelper.GetAnimRequests(allDefaultAnimations3));
				}
				else
				{
					this.previewAnimationHelper.PlayBasicAnimation(this.storeScene.GetCurrentCharacter(), characterDefinition, this.equipModuleAPI.SelectedEquipment, this.api.SelectedCharacterIndex);
				}
			}
		}
		else if (this.equipModuleAPI.SelectedEquipType == EquipmentTypes.HOLOGRAM)
		{
			this.storeScene.ReplayHologram();
		}
		else if (this.equipModuleAPI.SelectedEquipType == EquipmentTypes.VOICE_TAUNT)
		{
			this.storeScene.ReplayVoiceTaunt();
		}
		this.previousAnimationItem = this.equipModuleAPI.SelectedEquipment;
	}

	// Token: 0x060046D7 RID: 18135 RVA: 0x001352A4 File Offset: 0x001336A4
	private void checkItemAnimation()
	{
		CharacterDefinition characterDefinition = this.assetPreviewCharacter();
		if (this.previousAnimationItem != this.equipModuleAPI.SelectedEquipment)
		{
			this.playItemAnimation();
		}
		bool flag = this.lastCharacterDisplayed != characterDefinition && this.characterDataHelper.LinkedCharactersContains(this.api.SelectedCharacterLinkedCharacters, this.lastCharacterDisplayed);
		this.lastCharacterDisplayed = characterDefinition;
		if (flag && characterDefinition != null && this.characterDataHelper.GetLinkedCharacters(characterDefinition).Length > 1)
		{
			this.storeScene.ChangeFrontCharIndex(this.api.SelectedCharacterIndex);
			List<WavedashAnimationData> allDefaultAnimations = this.characterDataHelper.GetAllDefaultAnimations(characterDefinition, CharacterDefaultAnimationKey.STORE_IDLE);
			List<WavedashAnimationData> allDefaultAnimations2 = this.characterDataHelper.GetAllDefaultAnimations(characterDefinition, CharacterDefaultAnimationKey.SWAP_IN_ANIMATION);
			List<UISceneCharacterAnimRequest> list = new List<UISceneCharacterAnimRequest>();
			for (int i = 0; i < allDefaultAnimations2.Count; i++)
			{
				list.Add(new UISceneCharacterAnimRequest
				{
					animData = allDefaultAnimations2[i],
					type = UISceneCharacterAnimRequest.AnimRequestType.AnimData,
					mode = UISceneCharacter.AnimationMode.TRANSITION
				});
			}
			this.storeScene.SetDefaultAnimations(UISceneAnimRequestHelper.GetAnimRequests(allDefaultAnimations));
			this.storeScene.PlayTransition(list);
		}
	}

	// Token: 0x060046D8 RID: 18136 RVA: 0x001353D4 File Offset: 0x001337D4
	private void onEquipmentUpdate()
	{
		this.updateAssetPreview();
		this.checkItemAnimation();
	}

	// Token: 0x060046D9 RID: 18137 RVA: 0x001353E4 File Offset: 0x001337E4
	private void updateSideText()
	{
		if (this.api.SelectedCharacter == CharacterID.None)
		{
			this.ItemTitle.gameObject.SetActive(false);
			this.ItemSubtitle.gameObject.SetActive(false);
			this.ItemDesc.gameObject.SetActive(false);
		}
		else
		{
			this.ItemTitle.gameObject.SetActive(true);
			this.ItemDesc.gameObject.SetActive(false);
			this.ItemTitle.text = this.api.GetCharacterDisplayName(this.api.SelectedCharacter);
			this.ItemDesc.text = base.localization.GetText("gameData.store.characterDesc." + this.api.SelectedCharacter);
			if (this.equipModuleAPI.SelectedEquipment != null)
			{
				this.ItemSubtitle.gameObject.SetActive(true);
				this.ItemSubtitle.text = this.equipModuleAPI.GetLocalizedItemName(this.equipModuleAPI.SelectedEquipment);
			}
			else
			{
				this.ItemSubtitle.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060046DA RID: 18138 RVA: 0x00135504 File Offset: 0x00133904
	public void ReleaseSelections()
	{
		this.characterMiniMenu.RemoveSelection();
		this.characterUnlockButtonMenu.RemoveSelection();
	}

	// Token: 0x060046DB RID: 18139 RVA: 0x0013551C File Offset: 0x0013391C
	public void OnStateActivate()
	{
		this.module.Activate();
		this.updateEquipModuleState();
		this.module.BeginMenuFocus();
	}

	// Token: 0x060046DC RID: 18140 RVA: 0x0013553C File Offset: 0x0013393C
	public bool OnCancelPressed()
	{
		if (this.characterUnlockButtonMenu.CurrentSelection != null)
		{
			this.characterUnlockButtonMenu.CurrentSelection.InteractableButton.OnDeselect(null);
			this.module.SyncButtonModeSelection();
			return true;
		}
		return this.isEquipModuleFocused() && this.module.OnCancelPressed();
	}

	// Token: 0x060046DD RID: 18141 RVA: 0x0013559A File Offset: 0x0013399A
	public void OnLeft()
	{
		if (this.characterUnlockButtonMenu.CurrentSelection != null)
		{
			this.module.EnterFromRight();
		}
		else if (this.isEquipModuleFocused())
		{
			this.module.OnLeft();
		}
	}

	// Token: 0x060046DE RID: 18142 RVA: 0x001355D9 File Offset: 0x001339D9
	public override void OnZPressed()
	{
		if (base.allowInteraction())
		{
			this.advanceCharacter(1);
		}
	}

	// Token: 0x060046DF RID: 18143 RVA: 0x001355ED File Offset: 0x001339ED
	public void OnLeftBumperPressed()
	{
		if (base.allowInteraction())
		{
			this.advanceCharacter(-1);
		}
	}

	// Token: 0x060046E0 RID: 18144 RVA: 0x00135604 File Offset: 0x00133A04
	private void advanceCharacter(int direction)
	{
		int num = 0;
		for (int i = 0; i < this.characterButtonList.Count; i++)
		{
			if (this.characterButtonList[i] == this.api.SelectedCharacter)
			{
				num = i;
				break;
			}
		}
		num += direction;
		if (num >= this.characterButtonList.Count)
		{
			num = 0;
		}
		if (num < 0)
		{
			num = this.characterButtonList.Count - 1;
		}
		this.api.SelectedCharacter = this.characterButtonList[num];
	}

	// Token: 0x060046E1 RID: 18145 RVA: 0x00135695 File Offset: 0x00133A95
	public override void UpdateRightStick(float x, float y)
	{
		if (!CharacterEquipView.nonSpinnedEquipmentTypes.Contains(this.equipModuleAPI.SelectedEquipType) && x != 0f)
		{
			this.storeScene.SpinItemManual(StoreScene3D.ItemType.CHARACTER_EQUIP, x);
		}
	}

	// Token: 0x060046E2 RID: 18146 RVA: 0x001356C9 File Offset: 0x00133AC9
	protected override bool isEquipModuleFocused()
	{
		return base.allowInteraction() && this.characterUnlockButtonMenu.CurrentSelection == null;
	}

	// Token: 0x060046E3 RID: 18147 RVA: 0x001356E9 File Offset: 0x00133AE9
	public override void UpdateMouseMode()
	{
		base.UpdateMouseMode();
		if (this.instructionsRightBumper != null)
		{
			this.instructionsRightBumper.SetControlMode(base.uiManager.CurrentInputModule.CurrentMode);
		}
	}

	// Token: 0x060046E4 RID: 18148 RVA: 0x0013571D File Offset: 0x00133B1D
	public override void OnDestroy()
	{
		if (this.storeScene != null)
		{
			this.storeScene.HideCharacterEquipObject();
		}
		base.OnDestroy();
	}

	// Token: 0x060046E5 RID: 18149 RVA: 0x00135741 File Offset: 0x00133B41
	private void OnEnable()
	{
		this.onTabUpdate();
	}

	// Token: 0x04002EC3 RID: 11971
	public const string INJECTION_TAG = "CharacterEquipView";

	// Token: 0x04002ECF RID: 11983
	public Animator ZoomAnimator;

	// Token: 0x04002ED0 RID: 11984
	public GameObject CharacterMiniButtonPrefab;

	// Token: 0x04002ED1 RID: 11985
	public Sprite SkinsIcon;

	// Token: 0x04002ED2 RID: 11986
	public Sprite EmotesIcon;

	// Token: 0x04002ED3 RID: 11987
	public Sprite VoiceLinesIcon;

	// Token: 0x04002ED4 RID: 11988
	public Sprite SpraysIcon;

	// Token: 0x04002ED5 RID: 11989
	public Sprite VictoryPosesIcon;

	// Token: 0x04002ED6 RID: 11990
	public Sprite PlatformsIcon;

	// Token: 0x04002ED7 RID: 11991
	public HorizontalLayoutGroup CharacterMiniMenu;

	// Token: 0x04002ED8 RID: 11992
	public Transform CharacterDisplay3D;

	// Token: 0x04002ED9 RID: 11993
	public Transform CharacterItemDisplay3D;

	// Token: 0x04002EDA RID: 11994
	public TextMeshProUGUI ItemTitle;

	// Token: 0x04002EDB RID: 11995
	public TextMeshProUGUI ItemSubtitle;

	// Token: 0x04002EDC RID: 11996
	public TextMeshProUGUI ItemDesc;

	// Token: 0x04002EDD RID: 11997
	public MenuItemButton UnlockCharacterButton;

	// Token: 0x04002EDE RID: 11998
	public GameObject ProAccountSignage;

	// Token: 0x04002EDF RID: 11999
	public TextMeshProUGUI ProAccountBody;

	// Token: 0x04002EE0 RID: 12000
	public CharacterTabActionButton CharacterSwapButton;

	// Token: 0x04002EE1 RID: 12001
	public CursorTargetButton CollectiblesArrowButton;

	// Token: 0x04002EE2 RID: 12002
	public GameObject InputInstructionsRightBumperPrefab;

	// Token: 0x04002EE3 RID: 12003
	public Transform InputInstructionsRightBumperAnchor;

	// Token: 0x04002EE4 RID: 12004
	public Transform UIContainer;

	// Token: 0x04002EE5 RID: 12005
	public EquipmentSelectorModule GridModule;

	// Token: 0x04002EE6 RID: 12006
	public CharactersModuleMode MainMode;

	// Token: 0x04002EE7 RID: 12007
	public CharactersModuleMode GridMode;

	// Token: 0x04002EE8 RID: 12008
	public GameObject DisabledGallerySpinErrorContainer;

	// Token: 0x04002EE9 RID: 12009
	private Vector3 initialPosition;

	// Token: 0x04002EEA RID: 12010
	private ModuleMode currentModuleMode;

	// Token: 0x04002EEB RID: 12011
	private Tweener _fromModuleTween;

	// Token: 0x04002EEC RID: 12012
	private Tweener _toModuleTween;

	// Token: 0x04002EED RID: 12013
	private InputInstructions instructionsRightBumper;

	// Token: 0x04002EEE RID: 12014
	private MenuItemList characterMiniMenu;

	// Token: 0x04002EEF RID: 12015
	private MenuItemList characterUnlockButtonMenu;

	// Token: 0x04002EF0 RID: 12016
	private CharacterDefinition lastCharacterDisplayed;

	// Token: 0x04002EF1 RID: 12017
	private CharacterID currentCharacter;

	// Token: 0x04002EF2 RID: 12018
	private Dictionary<CharacterID, CharacterMiniButton> characterMiniButtons = new Dictionary<CharacterID, CharacterMiniButton>();

	// Token: 0x04002EF3 RID: 12019
	private List<CharacterID> characterButtonList = new List<CharacterID>();

	// Token: 0x04002EF4 RID: 12020
	private EquippableItem previousAnimationItem;

	// Token: 0x04002EF5 RID: 12021
	private static HashSet<EquipmentTypes> nonSpinnedEquipmentTypes = new HashSet<EquipmentTypes>
	{
		EquipmentTypes.HOLOGRAM,
		EquipmentTypes.VOICE_TAUNT
	};

	// Token: 0x04002EF6 RID: 12022
	private static HashSet<EquipmentTypes> gridModeEquipmentTypes = new HashSet<EquipmentTypes>
	{
		EquipmentTypes.HOLOGRAM
	};

	// Token: 0x04002EF7 RID: 12023
	private StoreScene3D storeScene;
}
