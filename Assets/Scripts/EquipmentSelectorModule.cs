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

public class EquipmentSelectorModule : ClientBehavior, IEquipmentSelectorModule
{
	public enum MenuType
	{
		NONE,
		TYPE,
		ITEM
	}

	public enum ItemDisplay
	{
		LIST,
		GRID
	}

	private sealed class _addEquipmentLine_c__AnonStorey0
	{
		internal EquippableItem item;

		internal EquipmentSelectorModule _this;

		internal void __m__0(InputEventData data)
		{
			this._this.clickedEquipment(this.item, data);
		}
	}

	private sealed class _addEquipType_c__AnonStorey1
	{
		internal EquipTypeDefinition def;

		internal EquipmentSelectorModule _this;

		internal void __m__0(InputEventData data)
		{
			this._this.clickedEquipTypes(this.def.type, data);
		}
	}

	public static string RESELECT_ITEM = "EquipmentSelectorModule.RESELECT_ITEM";

	public EquipmentLine EquipmentLinePrefab;

	public GameObject EquipTypeButtonPrefab;

	public Sprite EquipTypeButtonHighlight;

	public VerticalLayoutGroup EquipTypeMenu;

	public LayoutGroup EquipmentMenu;

	public TextMeshProUGUI MainSelectorTitle;

	public TextMeshProUGUI MainSelectorProgress;

	public GameObject EquipmentContent;

	public CharacterTabActionButton UnlockButton;

	public CharacterTabActionButton AvailableInButton;

	public CharacterTabActionButton EquipButton;

	public GameObject HasItemGroup;

	public GameObject UnownedGroup;

	public InputInstructions HasItemInputInstructions;

	public InputInstructions UnownedItemInputInstructions;

	public CanvasGroup ActionButtonsGroup;

	public ScrollRect EquipmentScroll;

	private int gridWidth;

	private MenuItemList equipTypeMenu;

	private MenuItemList bottomButtonMenu;

	private List<EquippableItem> pendingItems;

	private EquippableItem currentlySelectedEquipment;

	private EquipmentTypes currentlySelectedEquipType = EquipmentTypes.NONE;

	private EquipmentTypes currentEquipmentList = EquipmentTypes.NONE;

	private EquipmentSelectorModule.MenuType currentlySelectedMenuType = EquipmentSelectorModule.MenuType.TYPE;

	private MenuItemList currentRightEdgeNavigation;

	private RectTransform equipContentRect;

	private RectTransform equipListRect;

	private Dictionary<EquipmentTypes, EquipTypeButton> equipTypeButtons = new Dictionary<EquipmentTypes, EquipTypeButton>();

	private Dictionary<EquipmentTypes, List<EquipmentLine>> equipmentTypeIndex = new Dictionary<EquipmentTypes, List<EquipmentLine>>();

	private Dictionary<EquipmentTypes, MenuItemList> equipmentItemMenuIndex = new Dictionary<EquipmentTypes, MenuItemList>();

	private Dictionary<EquipmentID, EquipmentLine> equipmentIndex = new Dictionary<EquipmentID, EquipmentLine>();

	private List<EquipmentLine> equipmentSort = new List<EquipmentLine>();

	private List<EquipmentLine> currentlyActiveLines = new List<EquipmentLine>();

	private List<CharacterTabActionButton> bottomButtons = new List<CharacterTabActionButton>();

	private bool _needSyncScrollPosition;

	private bool _needScrollInit;

	private Vector3 _prevLinePosition;

	private EquipmentLine _syncScrollTarget;

	private Tweener _scrollTween;

	private bool equipmentListDirty;

	private IEquipModuleAPI api;

	private bool isExternalMenuSelected;

	private GameObject scrollItemContainer;

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	[Inject]
	public IStoreTabsModel storeTabsModel
	{
		get;
		set;
	}

	[Inject]
	public IUnlockEquipmentFlow unlockEquipmentFlow
	{
		get;
		set;
	}

	[Inject]
	public IUserCharacterUnlockModel userCharacterUnlockModel
	{
		get;
		set;
	}

	[Inject]
	public IEquipFlow equipFlow
	{
		get;
		set;
	}

	[Inject]
	public UIManager uiManager
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
	public IEquipmentModel equipmentModel
	{
		get;
		set;
	}

	public EquipmentSelectorModule.ItemDisplay itemDisplayMode
	{
		private get;
		set;
	}

	public EquipmentTypes lockItemType
	{
		private get;
		set;
	}

	public EquipmentTypes forbidItemType
	{
		private get;
		set;
	}

	public Func<string, string> GetTypeText
	{
		get;
		set;
	}

	public Func<List<EquipmentLine>, List<EquipmentLine>> ListFilter
	{
		get;
		set;
	}

	public Func<bool> IsAllowButtons
	{
		get;
		set;
	}

	public Action OnClickedEquipmentWithButtonsDisabled
	{
		get;
		set;
	}

	private MenuItemList currentMenuList
	{
		get
		{
			EquipmentSelectorModule.MenuType menuType = this.currentlySelectedMenuType;
			if (menuType == EquipmentSelectorModule.MenuType.TYPE)
			{
				return this.equipTypeMenu;
			}
			if (menuType != EquipmentSelectorModule.MenuType.ITEM)
			{
				return null;
			}
			return this.equipmentItemMenuIndex[this.useEquipType];
		}
	}

	public bool IsExternalMenuSelected
	{
		get
		{
			return this.isExternalMenuSelected;
		}
		set
		{
			this.isExternalMenuSelected = value;
			this.onDataUpdate();
		}
	}

	private EquipmentTypes useEquipType
	{
		get
		{
			EquipmentTypes equipmentTypes = (this.lockItemType != EquipmentTypes.NONE) ? this.lockItemType : this.api.SelectedEquipType;
			if (this.forbidItemType != EquipmentTypes.NONE && equipmentTypes == this.forbidItemType)
			{
				equipmentTypes = this.currentEquipmentList;
			}
			return equipmentTypes;
		}
	}

	public EquipmentSelectorModule()
	{
		this.lockItemType = EquipmentTypes.NONE;
		this.forbidItemType = EquipmentTypes.NONE;
	}

	public void Init(IEquipModuleAPI api)
	{
		this.api = api;
		this.equipContentRect = this.EquipmentContent.GetComponent<RectTransform>();
		this.equipListRect = this.EquipmentMenu.GetComponent<RectTransform>();
		this.scrollItemContainer = new GameObject("ScrollItemContainer");
		this.scrollItemContainer.SetActive(false);
		this.addEquipTypesList();
		this.addBottomButtons();
		this.bottomButtons.Add(this.UnlockButton);
		this.AvailableInButton.gameObject.SetActive(false);
		this.bottomButtons.Add(this.EquipButton);
		this.UnlockButton.DisableType = ButtonAnimator.VisualDisableType.Grey;
		this.UnlockButton.DisableDuration = 0.075f;
		this.AvailableInButton.DisableType = ButtonAnimator.VisualDisableType.Grey;
		this.AvailableInButton.DisableDuration = 0.075f;
		this.EquipButton.DisableType = ButtonAnimator.VisualDisableType.Grey;
		this.EquipButton.DisableDuration = 0.075f;
		base.listen(CharacterEquipViewAPI.UPDATED, new Action(this.onDataUpdate));
		base.listen(EquipmentSelectorAPI.UPDATED, new Action(this.onDataUpdate));
		base.listen(UserCharacterUnlockModel.UPDATED, new Action(this.onDataUpdate));
		base.listen(UserProAccountUnlockedModel.UPDATED, new Action(this.onDataUpdate));
		base.listen(EquipmentModel.UPDATED, new Action(this.onDataUpdate));
		base.listen(UserInventoryModel.UPDATED, new Action(this.onEquipmentUpdate));
		base.listen(UserTauntsModel.UPDATED, new Action(this.onEquipmentUpdate));
		base.listen(UserCharacterEquippedModel.UPDATED, new Action(this.onEquipmentUpdate));
		base.listen(UserGlobalEquippedModel.UPDATED, new Action(this.onEquipmentUpdate));
		this.SyncButtonModeSelection();
	}

	public void Activate()
	{
		if (this.pendingItems != null)
		{
			this.loadPendingItems();
		}
	}

	public void OnDrawComplete()
	{
		this.sortEquipment();
		this.onDataUpdate();
		this.updateEquipmentActionButtons();
	}

	public void AddRightEdgeNavigation(MenuItemList list)
	{
		this.currentRightEdgeNavigation = list;
		foreach (MenuItemList current in this.equipmentItemMenuIndex.Values)
		{
			current.AddEdgeNavigation(MoveDirection.Right, list);
		}
	}

	public void AddBottomEdgeNavigation(MenuItemList list)
	{
		this.equipTypeMenu.DisableGridWrap();
		this.equipTypeMenu.AddEdgeNavigation(MoveDirection.Down, list);
	}

	public void EnterFromRight()
	{
		EquipmentSelectorModule.MenuType newType;
		if (this.currentlyActiveLines.Count > 0)
		{
			newType = EquipmentSelectorModule.MenuType.ITEM;
		}
		else
		{
			newType = EquipmentSelectorModule.MenuType.TYPE;
		}
		this.switchMenu(newType);
	}

	public void EnterFromBottom()
	{
		this.switchMenu(EquipmentSelectorModule.MenuType.TYPE);
		MenuItemButton[] buttons = this.currentMenuList.GetButtons();
		this.equipTypeMenu.AutoSelect(buttons[buttons.Length - 1]);
	}

	public void OnMouseModeUpdate()
	{
		this.HasItemInputInstructions.SetControlMode((this.uiManager.CurrentInputModule as UIInputModule).CurrentMode);
		this.UnownedItemInputInstructions.SetControlMode((this.uiManager.CurrentInputModule as UIInputModule).CurrentMode);
	}

	public void BeginMenuFocus()
	{
		if (this.currentlySelectedMenuType == EquipmentSelectorModule.MenuType.NONE)
		{
			this.switchMenu(EquipmentSelectorModule.MenuType.TYPE);
		}
		else
		{
			this.SyncButtonModeSelection();
		}
	}

	public void Deselect()
	{
		this.switchMenu(EquipmentSelectorModule.MenuType.NONE);
		this.selectEquipType(EquipmentTypes.NONE);
	}

	private MenuItemButton getDefaultSelection()
	{
		MenuItemButton[] buttons = this.equipTypeMenu.GetButtons();
		for (int i = 0; i < buttons.Length; i++)
		{
			MenuItemButton menuItemButton = buttons[i];
			if (menuItemButton.ButtonEnabled)
			{
				return menuItemButton;
			}
		}
		return null;
	}

	private void switchMenu(EquipmentSelectorModule.MenuType newType)
	{
		this.api.SelectedMenuType = newType;
		this.syncSelection();
	}

	private MenuItemButton getControllerIntendedSelection()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return null;
		}
		if (this.api.SelectedMenuType != EquipmentSelectorModule.MenuType.NONE)
		{
			if (this.api.SelectedMenuType == EquipmentSelectorModule.MenuType.TYPE)
			{
				MenuItemButton[] buttons = this.equipTypeMenu.GetButtons();
				for (int i = 0; i < buttons.Length; i++)
				{
					MenuItemButton menuItemButton = buttons[i];
					if (menuItemButton.GetComponent<EquipTypeButton>().Type == this.currentlySelectedEquipType)
					{
						return menuItemButton;
					}
				}
			}
			else if (this.api.SelectedMenuType == EquipmentSelectorModule.MenuType.ITEM)
			{
				MenuItemButton[] buttons2 = this.equipmentItemMenuIndex[this.useEquipType].GetButtons();
				MenuItemButton[] array = buttons2;
				for (int j = 0; j < array.Length; j++)
				{
					MenuItemButton menuItemButton2 = array[j];
					if (menuItemButton2.ButtonEnabled && menuItemButton2.GetComponent<EquipmentLine>().Item == this.currentlySelectedEquipment)
					{
						return menuItemButton2;
					}
				}
				MenuItemButton[] array2 = buttons2;
				for (int k = 0; k < array2.Length; k++)
				{
					MenuItemButton menuItemButton3 = array2[k];
					if (menuItemButton3.ButtonEnabled)
					{
						return menuItemButton3;
					}
				}
			}
		}
		return null;
	}

	private bool isMouseMode()
	{
		return this.uiManager.CurrentInputModule.IsMouseMode;
	}

	private bool isKeyboardMode()
	{
		return this.uiManager.CurrentInputModule.CurrentMode == ControlMode.KeyboardMode;
	}

	public void SyncButtonModeSelection()
	{
		if (EventSystem.current.currentSelectedGameObject == null)
		{
			this.ForceSyncButtonSelection();
		}
	}

	public void ForceSyncButtonSelection()
	{
		if (this.isMouseMode())
		{
			return;
		}
		if (this.IsExternalMenuSelected)
		{
			return;
		}
		if (this.api.SelectedMenuType != EquipmentSelectorModule.MenuType.NONE)
		{
			this.updateMenuType();
			MenuItemButton controllerIntendedSelection = this.getControllerIntendedSelection();
			GameObject x = null;
			if (controllerIntendedSelection != null)
			{
				x = controllerIntendedSelection.InteractableButton.gameObject;
			}
			if (x != EventSystem.current.currentSelectedGameObject)
			{
				this.currentMenuList.AutoSelect(controllerIntendedSelection);
			}
		}
	}

	public bool OnCancelPressed()
	{
		if (this.currentlySelectedMenuType == EquipmentSelectorModule.MenuType.ITEM)
		{
			this.DeselectEquipment();
			return true;
		}
		return false;
	}

	public bool OnLeft()
	{
		bool flag = false;
		if (this.currentlySelectedMenuType == EquipmentSelectorModule.MenuType.ITEM)
		{
			if (this.itemDisplayMode == EquipmentSelectorModule.ItemDisplay.LIST)
			{
				flag |= true;
			}
			else if (this.currentMenuList.CurrentSelection == null)
			{
				flag |= true;
			}
			else if (this.currentMenuList.CurrentSelection.GridLocationIndex.x == (float)(this.gridWidth - 1))
			{
				flag |= true;
			}
		}
		if (flag)
		{
			this.DeselectEquipment();
			return true;
		}
		return false;
	}

	public bool OnRight()
	{
		if (this.currentlySelectedMenuType == EquipmentSelectorModule.MenuType.TYPE && this.currentlyActiveLines.Count > 0)
		{
			this.switchMenu(EquipmentSelectorModule.MenuType.ITEM);
			return true;
		}
		return false;
	}

	public void OnYButton()
	{
		this.clickedAvailableButton();
	}

	public void DeselectEquipment()
	{
		this.selectEquipment(null);
		if (this.currentlySelectedMenuType == EquipmentSelectorModule.MenuType.ITEM)
		{
			this.switchMenu(EquipmentSelectorModule.MenuType.TYPE);
		}
	}

	public void ReleaseSelections()
	{
		this.equipTypeMenu.RemoveSelection();
		this.equipmentItemMenuIndex[this.useEquipType].RemoveSelection();
	}

	public void LoadItems(List<EquippableItem> items)
	{
		this.pendingItems = items;
	}

	private void loadPendingItems()
	{
		ProfilingUtil.BeginTimer();
		foreach (EquippableItem current in this.pendingItems)
		{
			this.addEquipmentLine(current);
		}
		this.pendingItems = null;
		ProfilingUtil.EndTimer("EQUIPMENT SELECTOR MODULE - add equipment lines");
		ProfilingUtil.BeginTimer();
		foreach (MenuItemList current2 in this.equipmentItemMenuIndex.Values)
		{
			if (this.itemDisplayMode == EquipmentSelectorModule.ItemDisplay.GRID)
			{
				this.gridWidth = (this.EquipmentMenu as GridLayoutGroup).constraintCount;
				current2.SetNavigationType(MenuItemList.NavigationType.GridHorizontalFill, this.gridWidth);
			}
			current2.Initialize();
			current2.OnSelected = new Action<MenuItemButton, BaseEventData>(this.onEquipmentSelected);
		}
		ProfilingUtil.EndTimer("EQUIPMENT SELECTOR MODULE - setup menus");
		ProfilingUtil.BeginTimer();
		this.sortEquipment();
		ProfilingUtil.EndTimer("EQUIPMENT SELECTOR MODULE - sort equipment");
		this.redrawEquipmentList();
		this._needScrollInit = true;
	}

	private void onEquipmentSelected(MenuItemButton theButton, BaseEventData eventData)
	{
		if (theButton != null)
		{
			EquipmentLine component = theButton.GetComponent<EquipmentLine>();
			if (!this.isMouseEvent(eventData))
			{
				this.selectEquipment(component.Item);
			}
		}
	}

	private void addEquipmentLine(EquippableItem item)
	{
		EquipmentSelectorModule._addEquipmentLine_c__AnonStorey0 _addEquipmentLine_c__AnonStorey = new EquipmentSelectorModule._addEquipmentLine_c__AnonStorey0();
		_addEquipmentLine_c__AnonStorey.item = item;
		_addEquipmentLine_c__AnonStorey._this = this;
		EquipmentLine equipmentLine = UnityEngine.Object.Instantiate<EquipmentLine>(this.EquipmentLinePrefab);
		base.injector.Inject(equipmentLine);
		this.equipmentTypeIndex[_addEquipmentLine_c__AnonStorey.item.type].Add(equipmentLine);
		this.equipmentIndex[_addEquipmentLine_c__AnonStorey.item.id] = equipmentLine;
		this.equipmentSort.Add(equipmentLine);
		equipmentLine.gameObject.SetActive(false);
		equipmentLine.SetItem(_addEquipmentLine_c__AnonStorey.item, this.api);
		equipmentLine.transform.SetParent(this.scrollItemContainer.transform, false);
		if (!this.equipmentItemMenuIndex.ContainsKey(_addEquipmentLine_c__AnonStorey.item.type))
		{
			MenuItemList instance = base.injector.GetInstance<MenuItemList>();
			if (this.currentRightEdgeNavigation != null)
			{
				instance.AddEdgeNavigation(MoveDirection.Right, this.currentRightEdgeNavigation);
			}
			this.equipmentItemMenuIndex.Add(_addEquipmentLine_c__AnonStorey.item.type, instance);
		}
		this.equipmentItemMenuIndex[_addEquipmentLine_c__AnonStorey.item.type].AddButton(equipmentLine.MenuItemButton, new Action<InputEventData>(_addEquipmentLine_c__AnonStorey.__m__0));
		this.equipmentItemMenuIndex[_addEquipmentLine_c__AnonStorey.item.type].SetButtonEnabled(equipmentLine.MenuItemButton, false);
	}

	private void addEquipTypesList()
	{
		this.equipTypeMenu = base.injector.GetInstance<MenuItemList>();
		EquipTypeDefinition[] validEquipTypes = this.api.GetValidEquipTypes();
		for (int i = 0; i < validEquipTypes.Length; i++)
		{
			EquipTypeDefinition equipTypeDefinition = validEquipTypes[i];
			this.equipmentTypeIndex[equipTypeDefinition.type] = new List<EquipmentLine>();
			this.addEquipType(equipTypeDefinition);
		}
		this.equipTypeMenu.Initialize();
		this.equipTypeMenu.OnSelected = new Action<MenuItemButton, BaseEventData>(this.onEquipTypeSelected);
	}

	private bool isMouseEvent(BaseEventData eventData)
	{
		return eventData is PointerEventData;
	}

	private void onEquipTypeSelected(MenuItemButton theButton, BaseEventData eventData)
	{
		if (theButton != null)
		{
			EquipTypeButton component = theButton.GetComponent<EquipTypeButton>();
			if (!this.isMouseEvent(eventData))
			{
				this.selectEquipType(component.Type);
			}
		}
	}

	private void addEquipType(EquipTypeDefinition def)
	{
		EquipmentSelectorModule._addEquipType_c__AnonStorey1 _addEquipType_c__AnonStorey = new EquipmentSelectorModule._addEquipType_c__AnonStorey1();
		_addEquipType_c__AnonStorey.def = def;
		_addEquipType_c__AnonStorey._this = this;
		EquipTypeButton component = UnityEngine.Object.Instantiate<GameObject>(this.EquipTypeButtonPrefab).GetComponent<EquipTypeButton>();
		this.equipTypeButtons[_addEquipType_c__AnonStorey.def.type] = component;
		component.transform.SetParent(this.EquipTypeMenu.transform, false);
		component.Type = _addEquipType_c__AnonStorey.def.type;
		component.Icon.sprite = _addEquipType_c__AnonStorey.def.icon;
		this.equipTypeMenu.AddButton(component.GetComponent<MenuItemButton>(), new Action<InputEventData>(_addEquipType_c__AnonStorey.__m__0));
	}

	private void addBottomButtons()
	{
		this.bottomButtonMenu = base.injector.GetInstance<MenuItemList>();
		this.bottomButtonMenu.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		if (base.gameDataManager.IsFeatureEnabled(FeatureID.PortalPacks))
		{
			this.bottomButtonMenu.AddButton(this.AvailableInButton, new Action(this.clickedAvailableButton));
		}
		this.bottomButtonMenu.AddButton(this.UnlockButton, new Action(this.clickedUnlockButton));
		this.bottomButtonMenu.AddButton(this.EquipButton, new Action(this.clickedEquipButton));
		this.bottomButtonMenu.Initialize();
	}

	private void clickedUnlockButton()
	{
		this.unlockEquipmentFlow.Start(this.currentlySelectedEquipment, new Action(this.onDataUpdate), new Action(this.onEquipNewPurchase));
	}

	private void clickedEquipButton()
	{
		this.equipFlow.Start(this.currentlySelectedEquipment, this.api.SelectedCharacter);
	}

	private void clickedAvailableButton()
	{
	}

	private void clickedEquipment(EquippableItem item, InputEventData data)
	{
		if (data.isMouseEvent)
		{
			if (this.api.SelectedEquipment == item)
			{
				base.signalBus.Dispatch(EquipmentSelectorModule.RESELECT_ITEM);
			}
			this.selectEquipment(item);
		}
		else
		{
			this.controllerClickEquipment(item);
		}
		base.audioManager.PlayMenuSound(SoundKey.store_equipmentSelected, 0f);
	}

	private void controllerClickEquipment(EquippableItem item)
	{
		if (this.isAllowButtons())
		{
			if (!this.api.HasItem(this.currentlySelectedEquipment.id))
			{
				this.unlockEquipmentFlow.Start(this.currentlySelectedEquipment, new Action(this.onDataUpdate), new Action(this.onEquipNewPurchase));
			}
			else if (this.api.CanEquip(this.currentlySelectedEquipment))
			{
				this.equipFlow.Start(this.currentlySelectedEquipment, this.api.SelectedCharacter);
			}
			else
			{
				if (this.api.SelectedEquipment == item)
				{
					base.signalBus.Dispatch(EquipmentSelectorModule.RESELECT_ITEM);
				}
				this.selectEquipment(item);
			}
		}
		else if (this.OnClickedEquipmentWithButtonsDisabled != null)
		{
			this.OnClickedEquipmentWithButtonsDisabled();
		}
	}

	private void selectEquipment(EquippableItem item)
	{
		this.api.SelectedEquipment = item;
	}

	private void onEquipNewPurchase()
	{
		this.equipFlow.Start(this.currentlySelectedEquipment, this.api.SelectedCharacter);
	}

	private void clickedEquipTypes(EquipmentTypes type, InputEventData data)
	{
		this.selectEquipType(type);
		if (!data.isMouseEvent)
		{
			if (this.currentlyActiveLines.Count > 0)
			{
				this.switchMenu(EquipmentSelectorModule.MenuType.ITEM);
			}
		}
		else
		{
			this.api.SelectedMenuType = EquipmentSelectorModule.MenuType.TYPE;
			this.api.SelectedEquipment = null;
		}
		base.audioManager.PlayMenuSound(SoundKey.store_categorySelected, 0f);
	}

	private void syncSelection()
	{
		MenuItemButton controllerIntendedSelection = this.getControllerIntendedSelection();
		if (this.currentMenuList != null)
		{
			this.currentMenuList.AutoSelect(controllerIntendedSelection);
		}
	}

	private void selectEquipType(EquipmentTypes type)
	{
		this.api.SelectedEquipType = type;
	}

	private void onDataUpdate()
	{
		this.updateMenuType();
		this.updateCurrentlySelectedEquipType();
		this.updateEquipmentList();
		this.updateEquipmentSelected();
		this.updateEquipmentActionButtons();
		this.updateTitle();
	}

	private void updateMenuType()
	{
		if (this.currentlySelectedMenuType != this.api.SelectedMenuType)
		{
			if (this.currentlySelectedMenuType == EquipmentSelectorModule.MenuType.TYPE && this.api.SelectedMenuType == EquipmentSelectorModule.MenuType.NONE)
			{
				this.equipTypeMenu.Disable();
			}
			this.currentlySelectedMenuType = this.api.SelectedMenuType;
		}
	}

	private void updateTitle()
	{
		if (this.api.SelectedEquipType == EquipmentTypes.NONE)
		{
			this.MainSelectorTitle.text = string.Empty;
		}
		else
		{
			this.MainSelectorTitle.text = this.getTypeText();
		}
	}

	private string getTypeText()
	{
		string text = this.localization.GetText("equipType.plural." + this.api.SelectedEquipType);
		if (this.GetTypeText == null)
		{
			return text;
		}
		return this.GetTypeText(text);
	}

	private EquipTypeButton findButtonForType(EquipmentTypes id)
	{
		return (!this.equipTypeButtons.ContainsKey(id)) ? null : this.equipTypeButtons[id];
	}

	private EquipmentLine findEquipmentLine(EquippableItem item)
	{
		if (item == null)
		{
			return null;
		}
		if (!this.equipmentIndex.ContainsKey(item.id))
		{
			return null;
		}
		return this.equipmentIndex[item.id];
	}

	private void updateCurrentlySelectedEquipType()
	{
		EquipmentTypes selectedEquipType = this.api.SelectedEquipType;
		if (this.currentlySelectedEquipType != selectedEquipType)
		{
			if (this.currentlySelectedEquipType != EquipmentTypes.NONE)
			{
				EquipTypeButton equipTypeButton = this.findButtonForType(this.currentlySelectedEquipType);
				if (equipTypeButton != null)
				{
					equipTypeButton.Background.overrideSprite = null;
					equipTypeButton.GetComponent<MenuItemButton>().Unfreeze();
				}
			}
			this.currentlySelectedEquipType = selectedEquipType;
			EquipTypeButton equipTypeButton2 = this.findButtonForType(this.currentlySelectedEquipType);
			if (equipTypeButton2 != null)
			{
				equipTypeButton2.GetComponent<MenuItemButton>().Freeze();
				equipTypeButton2.Background.overrideSprite = this.EquipTypeButtonHighlight;
			}
		}
	}

	public void RebuildList()
	{
		this.equipmentListDirty = true;
		this.updateEquipmentList();
		this.updateTitle();
	}

	public void ForceRedraws()
	{
		this.redrawEquipmentList();
	}

	private bool isEqual(EquipmentLine[] list1, List<EquipmentLine> list2)
	{
		if (list1.Length != list2.Count)
		{
			return false;
		}
		for (int i = list1.Length - 1; i >= 0; i--)
		{
			if (list1[i] != list2[i])
			{
				return false;
			}
		}
		return true;
	}

	private void redrawEquipmentList()
	{
		this.equipmentListDirty = false;
		EquipmentLine[] array = this.currentlyActiveLines.ToArray();
		EquipmentTypes equipmentTypes = this.currentEquipmentList;
		this.currentEquipmentList = this.useEquipType;
		this.currentlyActiveLines = this.getFilteredLineList();
		if (!this.isEqual(array, this.currentlyActiveLines))
		{
			if (equipmentTypes != EquipmentTypes.NONE)
			{
				EquipmentLine[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					EquipmentLine equipmentLine = array2[i];
					equipmentLine.gameObject.SetActive(false);
					equipmentLine.transform.SetParent(this.scrollItemContainer.transform, false);
					this.equipmentItemMenuIndex[equipmentTypes].SetButtonEnabled(equipmentLine.MenuItemButton, false);
				}
			}
			if (this.currentEquipmentList != EquipmentTypes.NONE)
			{
				foreach (EquipmentLine current in this.currentlyActiveLines)
				{
					current.transform.SetParent(this.EquipmentMenu.transform, false);
					current.gameObject.SetActive(true);
					this.equipmentItemMenuIndex[this.currentEquipmentList].SetButtonEnabled(current.MenuItemButton, true);
					current.UpdateDynamicInfo();
				}
			}
		}
		else
		{
			foreach (EquipmentLine current2 in this.currentlyActiveLines)
			{
				current2.UpdateDynamicInfo();
			}
		}
		this.EquipmentMenu.Redraw();
		this.sortEquipment();
		float y = this.equipListRect.sizeDelta.y;
		Vector2 sizeDelta = this.equipContentRect.sizeDelta;
		sizeDelta.y = y;
		this.equipContentRect.sizeDelta = sizeDelta;
		this.updateProgressText();
	}

	private void updateEquipmentList()
	{
		if (this.currentEquipmentList != this.api.SelectedEquipType || this.equipmentListDirty)
		{
			this.redrawEquipmentList();
		}
		else
		{
			foreach (EquipmentLine current in this.currentlyActiveLines)
			{
				current.UpdateDynamicInfo();
			}
		}
	}

	private void updateProgressText()
	{
		this.MainSelectorProgress.gameObject.SetActive(false);
		if (this.api.SelectedEquipType == EquipmentTypes.NONE)
		{
			this.MainSelectorProgress.text = string.Empty;
		}
		else
		{
			int num = 0;
			int num2 = 0;
			foreach (EquipmentLine current in this.currentlyActiveLines)
			{
				if (!current.Item.isDefault)
				{
					num++;
					if (this.api.HasItem(current.Item.id))
					{
						num2++;
					}
				}
			}
			this.MainSelectorProgress.text = num2 + "/" + num;
		}
	}

	private List<EquipmentLine> getFilteredLineList()
	{
		if (this.useEquipType == EquipmentTypes.NONE)
		{
			return new List<EquipmentLine>();
		}
		List<EquipmentLine> list = this.equipmentTypeIndex[this.useEquipType];
		if (this.ListFilter == null)
		{
			return new List<EquipmentLine>(list);
		}
		return new List<EquipmentLine>(this.ListFilter(list));
	}

	private EquippableItem getEquipmentToHighlight()
	{
		return this.api.SelectedEquipment;
	}

	private void updateEquipmentSelected()
	{
		EquippableItem equipmentToHighlight = this.getEquipmentToHighlight();
		if (this.currentlySelectedEquipment != equipmentToHighlight)
		{
			if (this.currentlySelectedEquipment != null)
			{
				EquipmentLine equipmentLine = this.findEquipmentLine(this.currentlySelectedEquipment);
				if (equipmentLine != null)
				{
					MenuItemButton component = equipmentLine.GetComponent<MenuItemButton>();
					component.GetComponent<EquipmentLine>().SetHighlightMode(false);
					component.Unfreeze();
				}
			}
			this.currentlySelectedEquipment = equipmentToHighlight;
			if (this.currentlySelectedEquipment != null && this.currentlySelectedMenuType != EquipmentSelectorModule.MenuType.ITEM)
			{
				this.switchMenu(EquipmentSelectorModule.MenuType.ITEM);
			}
			if (this.currentlySelectedEquipment != null)
			{
				EquipmentLine equipmentLine2 = this.findEquipmentLine(this.currentlySelectedEquipment);
				if (equipmentLine2 != null)
				{
					MenuItemButton component2 = equipmentLine2.GetComponent<MenuItemButton>();
					component2.Freeze();
					component2.GetComponent<EquipmentLine>().SetHighlightMode(true);
					this._needSyncScrollPosition = true;
					this._syncScrollTarget = component2.GetComponent<EquipmentLine>();
					this._prevLinePosition = new Vector3(-99f, -99f, -99f);
				}
			}
			this.updateEquipmentActionButtons();
			this.EquipButton.InteractableButton.OnDeselect(null);
		}
	}

	private void onEquipmentUpdate()
	{
		foreach (EquipmentLine current in this.currentlyActiveLines)
		{
			current.UpdateDynamicInfo();
		}
		this.updateEquipmentActionButtons();
		this.updateProgressText();
	}

	private void sortEquipment()
	{
		EquipmentLine equipmentLine = this.findEquipmentLine(this.currentlySelectedEquipment);
		if (equipmentLine != null)
		{
			this._needSyncScrollPosition = true;
			this._syncScrollTarget = equipmentLine;
			this._prevLinePosition = equipmentLine.transform.localPosition;
		}
		if (this.equipmentItemMenuIndex.ContainsKey(this.useEquipType))
		{
			MenuItemList menuItemList = this.equipmentItemMenuIndex[this.useEquipType];
			this.equipmentTypeIndex[this.useEquipType].Sort(new Comparison<EquipmentLine>(this.sortFn));
			List<MenuItemButton> list = new List<MenuItemButton>();
			for (int i = 0; i < this.equipmentTypeIndex[this.useEquipType].Count; i++)
			{
				this.equipmentTypeIndex[this.useEquipType][i].transform.SetSiblingIndex(i);
				list.Add(this.equipmentTypeIndex[this.useEquipType][i].MenuItemButton);
			}
			menuItemList.Reload(list);
		}
	}

	private void Update()
	{
		if (this._needScrollInit)
		{
			this._needScrollInit = false;
			this.EquipmentScroll.verticalNormalizedPosition = 1f;
		}
		if (this._needSyncScrollPosition && this._syncScrollTarget.transform.localPosition != this._prevLinePosition)
		{
			this._needSyncScrollPosition = false;
			float num = 20f;
			float y = this.EquipmentScroll.content.sizeDelta.y;
			float y2 = this.EquipmentScroll.GetComponent<RectTransform>().sizeDelta.y;
			float num2 = y - y2;
			if (num2 > 0f)
			{
				float value = this.EquipmentScroll.verticalNormalizedPosition;
				value = Mathf.Clamp(value, 0f, 1f);
				float num3 = num2 * this.EquipmentScroll.verticalNormalizedPosition;
				float num4 = num2 - num3;
				float num5 = num4;
				float num6 = num4 + y2;
				float y3 = this._syncScrollTarget.GetComponent<RectTransform>().sizeDelta.y;
				float num7 = -this._syncScrollTarget.transform.localPosition.y;
				if (num7 < num5 || num7 + y3 > num6)
				{
					float num8 = (num7 - num) / num2;
					float num9 = 1f - num8;
					num9 = Mathf.Clamp(num9, 0f, 1f);
					this.tweenScroll(num9);
				}
			}
		}
	}

	private void tweenScroll(float target)
	{
		this.killScrollTween();
		this._scrollTween = DOTween.To(new DOGetter<float>(this._tweenScroll_m__0), new DOSetter<float>(this._tweenScroll_m__1), target, 0.15f).SetEase(Ease.OutCirc).OnComplete(new TweenCallback(this.killScrollTween));
	}

	private void killScrollTween()
	{
		TweenUtil.Destroy(ref this._scrollTween);
	}

	private int sortFn(EquipmentLine e1, EquipmentLine e2)
	{
		if (e1.gameObject.activeInHierarchy != e2.gameObject.activeInHierarchy)
		{
			if (e2.gameObject.activeInHierarchy)
			{
				return 1000001;
			}
			return -1000001;
		}
		else
		{
			bool flag = this.api.IsNew(e1.Item.id);
			bool flag2 = this.api.IsNew(e2.Item.id);
			if (flag != flag2)
			{
				if (flag2)
				{
					return 1000000;
				}
				return -1000000;
			}
			else
			{
				bool flag3 = this.api.HasItem(e1.Item.id);
				bool flag4 = this.api.HasItem(e2.Item.id);
				if (flag3 != flag4)
				{
					if (flag4)
					{
						return 100000;
					}
					return -100000;
				}
				else
				{
					if (e1.Item.rarity != e2.Item.rarity)
					{
						return (e1.Item.rarity - e2.Item.rarity) * 1000;
					}
					if (e1.Item.type == EquipmentTypes.SKIN)
					{
						SkinDefinition skinFromItem = this.equipmentModel.GetSkinFromItem(e1.Item.id);
						SkinDefinition skinFromItem2 = this.equipmentModel.GetSkinFromItem(e2.Item.id);
						if (skinFromItem != null && skinFromItem2 != null)
						{
							return (skinFromItem.priority - skinFromItem2.priority) * 10;
						}
					}
					return string.Compare(e1.LocalizedName, e2.LocalizedName, true);
				}
			}
		}
	}

	private void updateEquipmentActionButtons()
	{
		if (this.currentlySelectedEquipment == null)
		{
			this.setBottomButtonsInteractable(false);
			if (!this.isMouseMode())
			{
				this.UnownedGroup.SetActive(false);
				this.HasItemGroup.SetActive(false);
			}
			else
			{
				this.UnownedGroup.SetActive(true);
				this.HasItemGroup.SetActive(false);
			}
		}
		else
		{
			this.setBottomButtonsInteractable(true);
			if (this.api.HasItem(this.currentlySelectedEquipment.id))
			{
				this.UnownedGroup.SetActive(false);
				this.HasItemGroup.SetActive(true);
				if (base.gameDataManager.IsFeatureEnabled(FeatureID.PortalPacks))
				{
					this.bottomButtonMenu.SetButtonEnabled(this.AvailableInButton, false);
				}
				this.bottomButtonMenu.SetButtonEnabled(this.UnlockButton, false);
				this.bottomButtonMenu.SetButtonEnabled(this.EquipButton, true);
				if (!this.api.CanEquip(this.currentlySelectedEquipment))
				{
					this.setBottomButtonsInteractable(false);
					if (!this.isMouseMode())
					{
						this.HasItemGroup.SetActive(false);
					}
					if (this.api.IsEquipped(this.currentlySelectedEquipment))
					{
						this.EquipButton.SetText(this.localization.GetText("ui.store.characters.equipped"));
					}
					else
					{
						this.EquipButton.SetText(this.localization.GetText("ui.store.characters.equip"));
					}
				}
				else
				{
					this.EquipButton.SetText(this.localization.GetText("ui.store.characters.equip"));
				}
			}
			else
			{
				if (base.gameDataManager.IsFeatureEnabled(FeatureID.PortalPacks))
				{
					this.bottomButtonMenu.SetButtonEnabled(this.AvailableInButton, true);
				}
				this.bottomButtonMenu.SetButtonEnabled(this.UnlockButton, true);
				this.bottomButtonMenu.SetButtonEnabled(this.EquipButton, false);
				this.UnownedGroup.SetActive(true);
				this.HasItemGroup.SetActive(false);
				if (!this.api.HasPrice(this.currentlySelectedEquipment))
				{
					this.setBottomButtonsInteractable(false);
				}
			}
			if (!this.isAllowButtons())
			{
				this.setBottomButtonsInteractable(false);
			}
		}
	}

	private bool isAllowButtons()
	{
		return this.IsAllowButtons == null || this.IsAllowButtons();
	}

	private void setBottomButtonsInteractable(bool value)
	{
		foreach (CharacterTabActionButton current in this.bottomButtons)
		{
			current.SetInteractable(value);
		}
	}

	public override void OnDestroy()
	{
		if (this.scrollItemContainer != null)
		{
			UnityEngine.Object.DestroyImmediate(this.scrollItemContainer);
		}
		base.OnDestroy();
	}

	private float _tweenScroll_m__0()
	{
		return this.EquipmentScroll.verticalNormalizedPosition;
	}

	private void _tweenScroll_m__1(float valueIn)
	{
		this.EquipmentScroll.verticalNormalizedPosition = valueIn;
	}
}
