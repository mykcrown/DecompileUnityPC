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

public class CharacterEquipView : StoreTabWithEquipModule
{
	private sealed class _animateTransition_c__AnonStorey0
	{
		internal CharactersModuleMode from;

		internal CharactersModuleMode to;

		internal CharacterEquipView _this;

		internal float __m__0()
		{
			return this.from.CanvasGroup.alpha;
		}

		internal void __m__1(float valueIn)
		{
			this.from.CanvasGroup.alpha = valueIn;
		}

		internal float __m__2()
		{
			return this.to.CanvasGroup.alpha;
		}

		internal void __m__3(float valueIn)
		{
			this.to.CanvasGroup.alpha = valueIn;
		}

		internal void __m__4()
		{
			this.from.transform.localPosition = new Vector3(100000f, 0f, 0f);
			this._this.killFromModuleTweens();
		}
	}

	private sealed class _addCharacter_c__AnonStorey1
	{
		internal CharacterID characterId;

		internal CharacterEquipView _this;

		internal void __m__0()
		{
			this._this.clickedCharacter(this.characterId);
		}
	}

	public const string INJECTION_TAG = "CharacterEquipView";

	public Animator ZoomAnimator;

	public GameObject CharacterMiniButtonPrefab;

	public Sprite SkinsIcon;

	public Sprite EmotesIcon;

	public Sprite VoiceLinesIcon;

	public Sprite SpraysIcon;

	public Sprite VictoryPosesIcon;

	public Sprite PlatformsIcon;

	public HorizontalLayoutGroup CharacterMiniMenu;

	public Transform CharacterDisplay3D;

	public Transform CharacterItemDisplay3D;

	public TextMeshProUGUI ItemTitle;

	public TextMeshProUGUI ItemSubtitle;

	public TextMeshProUGUI ItemDesc;

	public MenuItemButton UnlockCharacterButton;

	public GameObject ProAccountSignage;

	public TextMeshProUGUI ProAccountBody;

	public CharacterTabActionButton CharacterSwapButton;

	public CursorTargetButton CollectiblesArrowButton;

	public GameObject InputInstructionsRightBumperPrefab;

	public Transform InputInstructionsRightBumperAnchor;

	public Transform UIContainer;

	public EquipmentSelectorModule GridModule;

	public CharactersModuleMode MainMode;

	public CharactersModuleMode GridMode;

	public GameObject DisabledGallerySpinErrorContainer;

	private Vector3 initialPosition;

	private ModuleMode currentModuleMode;

	private Tweener _fromModuleTween;

	private Tweener _toModuleTween;

	private InputInstructions instructionsRightBumper;

	private MenuItemList characterMiniMenu;

	private MenuItemList characterUnlockButtonMenu;

	private CharacterDefinition lastCharacterDisplayed;

	private CharacterID currentCharacter;

	private Dictionary<CharacterID, CharacterMiniButton> characterMiniButtons = new Dictionary<CharacterID, CharacterMiniButton>();

	private List<CharacterID> characterButtonList = new List<CharacterID>();

	private EquippableItem previousAnimationItem;

	private static HashSet<EquipmentTypes> nonSpinnedEquipmentTypes = new HashSet<EquipmentTypes>
	{
		EquipmentTypes.HOLOGRAM,
		EquipmentTypes.VOICE_TAUNT
	};

	private static HashSet<EquipmentTypes> gridModeEquipmentTypes = new HashSet<EquipmentTypes>
	{
		EquipmentTypes.HOLOGRAM
	};

	private StoreScene3D storeScene;

	[Inject]
	public ICharacterEquipViewAPI api
	{
		get;
		set;
	}

	[Inject]
	public IDetailedUnlockCharacterFlow detailedUnlockCharacterFlow
	{
		get;
		set;
	}

	[Inject("CharacterEquipView")]
	public IEquipModuleAPI equipModuleAPI
	{
		get;
		set;
	}

	[Inject]
	public ICharactersTabAPI charactersTabAPI
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
	public ICharacterLists characterLists
	{
		get;
		set;
	}

	[Inject]
	public ICharacterMenusDataLoader characterMenusDataLoader
	{
		private get;
		set;
	}

	[Inject]
	public IPreviewAnimationHelper previewAnimationHelper
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
	public IStoreTabsModel storeTabAPI
	{
		get;
		set;
	}

	[Inject]
	public ICollectiblesTabAPI collectiblesTabAPI
	{
		get;
		set;
	}

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
		MenuItemList expr_13F = this.characterUnlockButtonMenu;
		expr_13F.OnSelected = (Action<MenuItemButton, BaseEventData>)Delegate.Combine(expr_13F.OnSelected, new Action<MenuItemButton, BaseEventData>(this.onCharacterUnlockMenuSelected));
		MenuItemList expr_166 = this.characterUnlockButtonMenu;
		expr_166.OnDeselected = (Action<MenuItemButton, BaseEventData>)Delegate.Combine(expr_166.OnDeselected, new Action<MenuItemButton, BaseEventData>(this.onCharacterUnlockMenuDeselected));
		this.EquipModule.AddRightEdgeNavigation(this.characterUnlockButtonMenu);
		MenuItemButton[] buttons = this.characterUnlockButtonMenu.GetButtons();
		this.characterUnlockButtonMenu.LandingPoint = buttons[0];
		CursorTargetButton expr_1B8 = this.CollectiblesArrowButton;
		expr_1B8.ClickCallback = (Action<CursorTargetButton>)Delegate.Combine(expr_1B8.ClickCallback, new Action<CursorTargetButton>(this.onCollectiblesArrowClicked));
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

	private void onCharacterUnlockMenuSelected(MenuItemButton button, BaseEventData eventData)
	{
		this.updateEquipModuleState();
	}

	private void onCharacterUnlockMenuDeselected(MenuItemButton button, BaseEventData eventData)
	{
		this.updateEquipModuleState();
	}

	private void onCollectiblesArrowClicked(CursorTargetButton button)
	{
		this.storeTabAPI.Current = StoreTab.COLLECTIBLES;
		this.collectiblesTabAPI.SetState(CollectiblesTabState.EquipView, true);
	}

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

	private void updateEquipModuleState()
	{
		this.EquipModule.IsExternalMenuSelected = !this.isEquipModuleFocused();
	}

	private string getTypeText(string typeText)
	{
		return this.api.GetCharacterDisplayName(this.api.SelectedCharacter) + " " + typeText;
	}

	private List<EquipmentLine> listFilter(List<EquipmentLine> inlist)
	{
		List<EquipmentLine> list = new List<EquipmentLine>();
		foreach (EquipmentLine current in inlist)
		{
			bool flag = (this.equipModuleAPI.HasPrice(current.Item) && current.Item.promoted) || this.equipModuleAPI.HasItem(current.Item.id);
			if (flag && (current.Item.character == this.api.SelectedCharacter || current.Item.character == CharacterID.Any))
			{
				list.Add(current);
			}
		}
		return list;
	}

	private void animateTransition(CharactersModuleMode from, CharactersModuleMode to)
	{
		CharacterEquipView._animateTransition_c__AnonStorey0 _animateTransition_c__AnonStorey = new CharacterEquipView._animateTransition_c__AnonStorey0();
		_animateTransition_c__AnonStorey.from = from;
		_animateTransition_c__AnonStorey.to = to;
		_animateTransition_c__AnonStorey._this = this;
		float duration = 0.07f;
		this.killFromModuleTweens();
		_animateTransition_c__AnonStorey.to.transform.localPosition = Vector3.zero;
		_animateTransition_c__AnonStorey.to.Activate();
		this._fromModuleTween = DOTween.To(new DOGetter<float>(_animateTransition_c__AnonStorey.__m__0), new DOSetter<float>(_animateTransition_c__AnonStorey.__m__1), 0f, duration).SetEase(Ease.Linear);
		this._toModuleTween = DOTween.To(new DOGetter<float>(_animateTransition_c__AnonStorey.__m__2), new DOSetter<float>(_animateTransition_c__AnonStorey.__m__3), 1f, duration).SetEase(Ease.Linear).OnComplete(new TweenCallback(_animateTransition_c__AnonStorey.__m__4));
	}

	private void killFromModuleTweens()
	{
		TweenUtil.Destroy(ref this._fromModuleTween);
		TweenUtil.Destroy(ref this._toModuleTween);
	}

	protected override bool isEquipViewActive()
	{
		return this.charactersTabAPI.GetState() == CharactersTabState.EquipView;
	}

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

	private bool isAllowButtons()
	{
		return true;
	}

	public void OnDrawComplete()
	{
		this.module.OnDrawComplete();
		this.onDataUpdate();
	}

	public override void OnActivate()
	{
		this.updateEquipModuleState();
		base.OnActivate();
	}

	private void mapSpriteToEquip(EquipmentTypes id, Sprite icon)
	{
		this.equipModuleAPI.MapEquipTypeIcon(id, icon);
	}

	private void addEquipment()
	{
		this.EquipModule.Init(this.equipModuleAPI);
		this.GridModule.Init(this.equipModuleAPI);
		List<EquippableItem> list = new List<EquippableItem>();
		List<EquippableItem> list2 = new List<EquippableItem>();
		CharacterID[] characters = this.api.GetCharacters();
		for (int i = 0; i < characters.Length; i++)
		{
			CharacterID characterId = characters[i];
			EquipTypeDefinition[] validEquipTypes = this.equipModuleAPI.GetValidEquipTypes();
			for (int j = 0; j < validEquipTypes.Length; j++)
			{
				EquipTypeDefinition equipTypeDefinition = validEquipTypes[j];
				EquippableItem[] items = this.api.GetItems(characterId, equipTypeDefinition.type);
				for (int k = 0; k < items.Length; k++)
				{
					EquippableItem item = items[k];
					list.Add(item);
				}
			}
		}
		EquipTypeDefinition[] validEquipTypes2 = this.equipModuleAPI.GetValidEquipTypes();
		for (int l = 0; l < validEquipTypes2.Length; l++)
		{
			EquipTypeDefinition equipTypeDefinition2 = validEquipTypes2[l];
			EquippableItem[] items2 = this.api.GetItems(CharacterID.Any, equipTypeDefinition2.type);
			for (int m = 0; m < items2.Length; m++)
			{
				EquippableItem equippableItem = items2[m];
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

	private void addCharacterUnlockButton()
	{
		this.characterUnlockButtonMenu = base.injector.GetInstance<MenuItemList>();
		this.characterUnlockButtonMenu.SetNavigationType(MenuItemList.NavigationType.InOrderVertical, 0);
		this.characterUnlockButtonMenu.AddButton(this.UnlockCharacterButton, new Action(this.clickedUnlockCharacterButton));
		this.UnlockCharacterButton.DisableType = ButtonAnimator.VisualDisableType.DisappearCanvas;
		this.characterUnlockButtonMenu.Initialize();
	}

	private void addCharacterMiniMenu()
	{
		this.characterMiniMenu = base.injector.GetInstance<MenuItemList>();
		this.characterMiniMenu.MouseOnly = true;
		this.characterMiniMenu.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		CharacterID[] characters = this.api.GetCharacters();
		for (int i = 0; i < characters.Length; i++)
		{
			CharacterID characterId = characters[i];
			this.addCharacter(characterId);
		}
		this.characterMiniMenu.Initialize();
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.InputInstructionsRightBumperPrefab);
		this.instructionsRightBumper = gameObject.GetComponent<InputInstructions>();
		gameObject.transform.SetParent(this.InputInstructionsRightBumperAnchor, false);
		this.instructionsRightBumper.SetControlMode(base.uiManager.CurrentInputModule.CurrentMode);
	}

	private void addCharacter(CharacterID characterId)
	{
		CharacterEquipView._addCharacter_c__AnonStorey1 _addCharacter_c__AnonStorey = new CharacterEquipView._addCharacter_c__AnonStorey1();
		_addCharacter_c__AnonStorey.characterId = characterId;
		_addCharacter_c__AnonStorey._this = this;
		CharacterMiniButton component = UnityEngine.Object.Instantiate<GameObject>(this.CharacterMiniButtonPrefab).GetComponent<CharacterMiniButton>();
		this.characterMiniButtons[_addCharacter_c__AnonStorey.characterId] = component;
		this.characterButtonList.Add(_addCharacter_c__AnonStorey.characterId);
		component.character = _addCharacter_c__AnonStorey.characterId;
		component.transform.SetParent(this.CharacterMiniMenu.transform, false);
		component.CharacterImage.sprite = this.characterMenusDataLoader.GetData(_addCharacter_c__AnonStorey.characterId).miniPortrait;
		MenuItemButton component2 = component.GetComponent<MenuItemButton>();
		this.characterMiniMenu.AddButton(component2, new Action(_addCharacter_c__AnonStorey.__m__0));
	}

	private void clickedUnlockCharacterButton()
	{
		this.detailedUnlockCharacterFlow.Start(this.api.SelectedCharacter, new Action(this.onDataUpdate));
	}

	private void promptUnlockCharacter()
	{
		this.detailedUnlockCharacterFlow.Start(this.api.SelectedCharacter, new Action(this.onDataUpdate));
	}

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

	public override void OnRightStickUp()
	{
		this.attemptSwapCharacter();
	}

	public override void OnRightStickDown()
	{
		this.attemptSwapCharacter();
	}

	private void attemptSwapCharacter()
	{
		CharacterDefinition characterDefinition = this.assetPreviewCharacter();
		CharacterDefinition[] linkedCharacters = this.characterDataHelper.GetLinkedCharacters(characterDefinition);
		if (characterDefinition != null && linkedCharacters.Length > 1)
		{
			this.swapCharacter();
		}
	}

	private void swapCharacter()
	{
		if (!this.storeScene.IsCharacterSwapping && !this.isViewingVictoryPose())
		{
			base.audioManager.PlayMenuSound(SoundKey.characterSelect_totemPartnerCycle, 0f);
			this.api.SelectedCharacterIndex = (this.api.SelectedCharacterIndex + 1) % 2;
		}
	}

	private bool isViewingVictoryPose()
	{
		return this.equipModuleAPI.SelectedEquipType == EquipmentTypes.VICTORY_POSE && this.equipModuleAPI.SelectedEquipment != null;
	}

	private void selectUnlockCharacter()
	{
		this.characterUnlockButtonMenu.AutoSelect(this.UnlockCharacterButton);
	}

	private void clickedCharacter(CharacterID characterId)
	{
		this.api.SelectedCharacter = characterId;
		base.audioManager.PlayMenuSound(SoundKey.store_miniCharacterSelected, 0f);
	}

	private void Update()
	{
		this.DisabledGallerySpinErrorContainer.SetActive(UIInputModule.DisableMenuRightStick);
	}

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

	private void updateModule()
	{
		ModuleMode mode = (!CharacterEquipView.gridModeEquipmentTypes.Contains(this.equipModuleAPI.SelectedEquipType)) ? ModuleMode.MAIN : ModuleMode.GRID;
		this.switchToModule(mode);
	}

	private void onReselectItem()
	{
		this.playItemAnimation();
	}

	private CharacterMiniButton findButtonForCharacter(CharacterID id)
	{
		return this.characterMiniButtons[id];
	}

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

	private void updateCharacterLockState()
	{
		bool flag = !this.api.IsCharacterUnlocked(this.api.SelectedCharacter);
		this.characterUnlockButtonMenu.SetButtonEnabled(this.UnlockCharacterButton, flag);
		this.ProAccountSignage.SetActive(flag);
	}

	private CharacterDefinition assetPreviewCharacter()
	{
		if (this.equipModuleAPI.SelectedEquipType == EquipmentTypes.PLATFORM || this.equipModuleAPI.SelectedEquipType == EquipmentTypes.HOLOGRAM || this.equipModuleAPI.SelectedEquipType == EquipmentTypes.VOICE_TAUNT)
		{
			return null;
		}
		CharacterDefinition[] selectedCharacterLinkedCharacters = this.api.SelectedCharacterLinkedCharacters;
		return selectedCharacterLinkedCharacters[this.api.SelectedCharacterIndex];
	}

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

	private void onTabUpdate()
	{
		this.updateTabMode();
	}

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

	public void PlayTransition()
	{
		this.ZoomAnimator.SetBool("visible", true);
		base.transform.localPosition = this.initialPosition;
	}

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

	private void onEquipmentUpdate()
	{
		this.updateAssetPreview();
		this.checkItemAnimation();
	}

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

	public void ReleaseSelections()
	{
		this.characterMiniMenu.RemoveSelection();
		this.characterUnlockButtonMenu.RemoveSelection();
	}

	public void OnStateActivate()
	{
		this.module.Activate();
		this.updateEquipModuleState();
		this.module.BeginMenuFocus();
	}

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

	public override void OnZPressed()
	{
		if (base.allowInteraction())
		{
			this.advanceCharacter(1);
		}
	}

	public void OnLeftBumperPressed()
	{
		if (base.allowInteraction())
		{
			this.advanceCharacter(-1);
		}
	}

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

	public override void UpdateRightStick(float x, float y)
	{
		if (!CharacterEquipView.nonSpinnedEquipmentTypes.Contains(this.equipModuleAPI.SelectedEquipType) && x != 0f)
		{
			this.storeScene.SpinItemManual(StoreScene3D.ItemType.CHARACTER_EQUIP, x);
		}
	}

	protected override bool isEquipModuleFocused()
	{
		return base.allowInteraction() && this.characterUnlockButtonMenu.CurrentSelection == null;
	}

	public override void UpdateMouseMode()
	{
		base.UpdateMouseMode();
		if (this.instructionsRightBumper != null)
		{
			this.instructionsRightBumper.SetControlMode(base.uiManager.CurrentInputModule.CurrentMode);
		}
	}

	public override void OnDestroy()
	{
		if (this.storeScene != null)
		{
			this.storeScene.HideCharacterEquipObject();
		}
		base.OnDestroy();
	}

	private void OnEnable()
	{
		this.onTabUpdate();
	}
}
