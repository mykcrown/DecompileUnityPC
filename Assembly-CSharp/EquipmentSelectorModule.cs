using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020009FD RID: 2557
public class EquipmentSelectorModule : ClientBehavior, IEquipmentSelectorModule
{
	// Token: 0x06004972 RID: 18802 RVA: 0x0013BC84 File Offset: 0x0013A084
	public EquipmentSelectorModule()
	{
		this.lockItemType = EquipmentTypes.NONE;
		this.forbidItemType = EquipmentTypes.NONE;
	}

	// Token: 0x17001195 RID: 4501
	// (get) Token: 0x06004973 RID: 18803 RVA: 0x0013BD07 File Offset: 0x0013A107
	// (set) Token: 0x06004974 RID: 18804 RVA: 0x0013BD0F File Offset: 0x0013A10F
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x17001196 RID: 4502
	// (get) Token: 0x06004975 RID: 18805 RVA: 0x0013BD18 File Offset: 0x0013A118
	// (set) Token: 0x06004976 RID: 18806 RVA: 0x0013BD20 File Offset: 0x0013A120
	[Inject]
	public IStoreTabsModel storeTabsModel { get; set; }

	// Token: 0x17001197 RID: 4503
	// (get) Token: 0x06004977 RID: 18807 RVA: 0x0013BD29 File Offset: 0x0013A129
	// (set) Token: 0x06004978 RID: 18808 RVA: 0x0013BD31 File Offset: 0x0013A131
	[Inject]
	public IUnlockEquipmentFlow unlockEquipmentFlow { get; set; }

	// Token: 0x17001198 RID: 4504
	// (get) Token: 0x06004979 RID: 18809 RVA: 0x0013BD3A File Offset: 0x0013A13A
	// (set) Token: 0x0600497A RID: 18810 RVA: 0x0013BD42 File Offset: 0x0013A142
	[Inject]
	public IUserCharacterUnlockModel userCharacterUnlockModel { get; set; }

	// Token: 0x17001199 RID: 4505
	// (get) Token: 0x0600497B RID: 18811 RVA: 0x0013BD4B File Offset: 0x0013A14B
	// (set) Token: 0x0600497C RID: 18812 RVA: 0x0013BD53 File Offset: 0x0013A153
	[Inject]
	public IEquipFlow equipFlow { get; set; }

	// Token: 0x1700119A RID: 4506
	// (get) Token: 0x0600497D RID: 18813 RVA: 0x0013BD5C File Offset: 0x0013A15C
	// (set) Token: 0x0600497E RID: 18814 RVA: 0x0013BD64 File Offset: 0x0013A164
	[Inject]
	public UIManager uiManager { get; set; }

	// Token: 0x1700119B RID: 4507
	// (get) Token: 0x0600497F RID: 18815 RVA: 0x0013BD6D File Offset: 0x0013A16D
	// (set) Token: 0x06004980 RID: 18816 RVA: 0x0013BD75 File Offset: 0x0013A175
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x1700119C RID: 4508
	// (get) Token: 0x06004981 RID: 18817 RVA: 0x0013BD7E File Offset: 0x0013A17E
	// (set) Token: 0x06004982 RID: 18818 RVA: 0x0013BD86 File Offset: 0x0013A186
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x1700119D RID: 4509
	// (get) Token: 0x06004983 RID: 18819 RVA: 0x0013BD8F File Offset: 0x0013A18F
	// (set) Token: 0x06004984 RID: 18820 RVA: 0x0013BD97 File Offset: 0x0013A197
	public EquipmentSelectorModule.ItemDisplay itemDisplayMode { private get; set; }

	// Token: 0x1700119E RID: 4510
	// (get) Token: 0x06004985 RID: 18821 RVA: 0x0013BDA0 File Offset: 0x0013A1A0
	// (set) Token: 0x06004986 RID: 18822 RVA: 0x0013BDA8 File Offset: 0x0013A1A8
	public EquipmentTypes lockItemType { private get; set; }

	// Token: 0x1700119F RID: 4511
	// (get) Token: 0x06004987 RID: 18823 RVA: 0x0013BDB1 File Offset: 0x0013A1B1
	// (set) Token: 0x06004988 RID: 18824 RVA: 0x0013BDB9 File Offset: 0x0013A1B9
	public EquipmentTypes forbidItemType { private get; set; }

	// Token: 0x170011A0 RID: 4512
	// (get) Token: 0x06004989 RID: 18825 RVA: 0x0013BDC2 File Offset: 0x0013A1C2
	// (set) Token: 0x0600498A RID: 18826 RVA: 0x0013BDCA File Offset: 0x0013A1CA
	public Func<string, string> GetTypeText { get; set; }

	// Token: 0x170011A1 RID: 4513
	// (get) Token: 0x0600498B RID: 18827 RVA: 0x0013BDD3 File Offset: 0x0013A1D3
	// (set) Token: 0x0600498C RID: 18828 RVA: 0x0013BDDB File Offset: 0x0013A1DB
	public Func<List<EquipmentLine>, List<EquipmentLine>> ListFilter { get; set; }

	// Token: 0x170011A2 RID: 4514
	// (get) Token: 0x0600498D RID: 18829 RVA: 0x0013BDE4 File Offset: 0x0013A1E4
	// (set) Token: 0x0600498E RID: 18830 RVA: 0x0013BDEC File Offset: 0x0013A1EC
	public Func<bool> IsAllowButtons { get; set; }

	// Token: 0x170011A3 RID: 4515
	// (get) Token: 0x0600498F RID: 18831 RVA: 0x0013BDF5 File Offset: 0x0013A1F5
	// (set) Token: 0x06004990 RID: 18832 RVA: 0x0013BDFD File Offset: 0x0013A1FD
	public Action OnClickedEquipmentWithButtonsDisabled { get; set; }

	// Token: 0x170011A4 RID: 4516
	// (get) Token: 0x06004991 RID: 18833 RVA: 0x0013BE08 File Offset: 0x0013A208
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

	// Token: 0x170011A5 RID: 4517
	// (get) Token: 0x06004992 RID: 18834 RVA: 0x0013BE49 File Offset: 0x0013A249
	// (set) Token: 0x06004993 RID: 18835 RVA: 0x0013BE51 File Offset: 0x0013A251
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

	// Token: 0x06004994 RID: 18836 RVA: 0x0013BE60 File Offset: 0x0013A260
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

	// Token: 0x06004995 RID: 18837 RVA: 0x0013C01A File Offset: 0x0013A41A
	public void Activate()
	{
		if (this.pendingItems != null)
		{
			this.loadPendingItems();
		}
	}

	// Token: 0x06004996 RID: 18838 RVA: 0x0013C02D File Offset: 0x0013A42D
	public void OnDrawComplete()
	{
		this.sortEquipment();
		this.onDataUpdate();
		this.updateEquipmentActionButtons();
	}

	// Token: 0x06004997 RID: 18839 RVA: 0x0013C044 File Offset: 0x0013A444
	public void AddRightEdgeNavigation(MenuItemList list)
	{
		this.currentRightEdgeNavigation = list;
		foreach (MenuItemList menuItemList in this.equipmentItemMenuIndex.Values)
		{
			menuItemList.AddEdgeNavigation(MoveDirection.Right, list);
		}
	}

	// Token: 0x06004998 RID: 18840 RVA: 0x0013C0B0 File Offset: 0x0013A4B0
	public void AddBottomEdgeNavigation(MenuItemList list)
	{
		this.equipTypeMenu.DisableGridWrap();
		this.equipTypeMenu.AddEdgeNavigation(MoveDirection.Down, list);
	}

	// Token: 0x06004999 RID: 18841 RVA: 0x0013C0CC File Offset: 0x0013A4CC
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

	// Token: 0x0600499A RID: 18842 RVA: 0x0013C0FC File Offset: 0x0013A4FC
	public void EnterFromBottom()
	{
		this.switchMenu(EquipmentSelectorModule.MenuType.TYPE);
		MenuItemButton[] buttons = this.currentMenuList.GetButtons();
		this.equipTypeMenu.AutoSelect(buttons[buttons.Length - 1]);
	}

	// Token: 0x0600499B RID: 18843 RVA: 0x0013C130 File Offset: 0x0013A530
	public void OnMouseModeUpdate()
	{
		this.HasItemInputInstructions.SetControlMode((this.uiManager.CurrentInputModule as UIInputModule).CurrentMode);
		this.UnownedItemInputInstructions.SetControlMode((this.uiManager.CurrentInputModule as UIInputModule).CurrentMode);
	}

	// Token: 0x0600499C RID: 18844 RVA: 0x0013C17D File Offset: 0x0013A57D
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

	// Token: 0x0600499D RID: 18845 RVA: 0x0013C19C File Offset: 0x0013A59C
	public void Deselect()
	{
		this.switchMenu(EquipmentSelectorModule.MenuType.NONE);
		this.selectEquipType(EquipmentTypes.NONE);
	}

	// Token: 0x0600499E RID: 18846 RVA: 0x0013C1AC File Offset: 0x0013A5AC
	private MenuItemButton getDefaultSelection()
	{
		foreach (MenuItemButton menuItemButton in this.equipTypeMenu.GetButtons())
		{
			if (menuItemButton.ButtonEnabled)
			{
				return menuItemButton;
			}
		}
		return null;
	}

	// Token: 0x0600499F RID: 18847 RVA: 0x0013C1EB File Offset: 0x0013A5EB
	private void switchMenu(EquipmentSelectorModule.MenuType newType)
	{
		this.api.SelectedMenuType = newType;
		this.syncSelection();
	}

	// Token: 0x060049A0 RID: 18848 RVA: 0x0013C200 File Offset: 0x0013A600
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
				foreach (MenuItemButton menuItemButton in this.equipTypeMenu.GetButtons())
				{
					if (menuItemButton.GetComponent<EquipTypeButton>().Type == this.currentlySelectedEquipType)
					{
						return menuItemButton;
					}
				}
			}
			else if (this.api.SelectedMenuType == EquipmentSelectorModule.MenuType.ITEM)
			{
				MenuItemButton[] buttons2 = this.equipmentItemMenuIndex[this.useEquipType].GetButtons();
				foreach (MenuItemButton menuItemButton2 in buttons2)
				{
					if (menuItemButton2.ButtonEnabled && menuItemButton2.GetComponent<EquipmentLine>().Item == this.currentlySelectedEquipment)
					{
						return menuItemButton2;
					}
				}
				foreach (MenuItemButton menuItemButton3 in buttons2)
				{
					if (menuItemButton3.ButtonEnabled)
					{
						return menuItemButton3;
					}
				}
			}
		}
		return null;
	}

	// Token: 0x060049A1 RID: 18849 RVA: 0x0013C325 File Offset: 0x0013A725
	private bool isMouseMode()
	{
		return this.uiManager.CurrentInputModule.IsMouseMode;
	}

	// Token: 0x060049A2 RID: 18850 RVA: 0x0013C337 File Offset: 0x0013A737
	private bool isKeyboardMode()
	{
		return this.uiManager.CurrentInputModule.CurrentMode == ControlMode.KeyboardMode;
	}

	// Token: 0x060049A3 RID: 18851 RVA: 0x0013C34C File Offset: 0x0013A74C
	public void SyncButtonModeSelection()
	{
		if (EventSystem.current.currentSelectedGameObject == null)
		{
			this.ForceSyncButtonSelection();
		}
	}

	// Token: 0x060049A4 RID: 18852 RVA: 0x0013C36C File Offset: 0x0013A76C
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

	// Token: 0x060049A5 RID: 18853 RVA: 0x0013C3E9 File Offset: 0x0013A7E9
	public bool OnCancelPressed()
	{
		if (this.currentlySelectedMenuType == EquipmentSelectorModule.MenuType.ITEM)
		{
			this.DeselectEquipment();
			return true;
		}
		return false;
	}

	// Token: 0x060049A6 RID: 18854 RVA: 0x0013C400 File Offset: 0x0013A800
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

	// Token: 0x060049A7 RID: 18855 RVA: 0x0013C487 File Offset: 0x0013A887
	public bool OnRight()
	{
		if (this.currentlySelectedMenuType == EquipmentSelectorModule.MenuType.TYPE && this.currentlyActiveLines.Count > 0)
		{
			this.switchMenu(EquipmentSelectorModule.MenuType.ITEM);
			return true;
		}
		return false;
	}

	// Token: 0x060049A8 RID: 18856 RVA: 0x0013C4B0 File Offset: 0x0013A8B0
	public void OnYButton()
	{
		this.clickedAvailableButton();
	}

	// Token: 0x060049A9 RID: 18857 RVA: 0x0013C4B8 File Offset: 0x0013A8B8
	public void DeselectEquipment()
	{
		this.selectEquipment(null);
		if (this.currentlySelectedMenuType == EquipmentSelectorModule.MenuType.ITEM)
		{
			this.switchMenu(EquipmentSelectorModule.MenuType.TYPE);
		}
	}

	// Token: 0x060049AA RID: 18858 RVA: 0x0013C4D4 File Offset: 0x0013A8D4
	public void ReleaseSelections()
	{
		this.equipTypeMenu.RemoveSelection();
		this.equipmentItemMenuIndex[this.useEquipType].RemoveSelection();
	}

	// Token: 0x060049AB RID: 18859 RVA: 0x0013C4F7 File Offset: 0x0013A8F7
	public void LoadItems(List<EquippableItem> items)
	{
		this.pendingItems = items;
	}

	// Token: 0x060049AC RID: 18860 RVA: 0x0013C500 File Offset: 0x0013A900
	private void loadPendingItems()
	{
		ProfilingUtil.BeginTimer();
		foreach (EquippableItem item in this.pendingItems)
		{
			this.addEquipmentLine(item);
		}
		this.pendingItems = null;
		ProfilingUtil.EndTimer("EQUIPMENT SELECTOR MODULE - add equipment lines");
		ProfilingUtil.BeginTimer();
		foreach (MenuItemList menuItemList in this.equipmentItemMenuIndex.Values)
		{
			if (this.itemDisplayMode == EquipmentSelectorModule.ItemDisplay.GRID)
			{
				this.gridWidth = (this.EquipmentMenu as GridLayoutGroup).constraintCount;
				menuItemList.SetNavigationType(MenuItemList.NavigationType.GridHorizontalFill, this.gridWidth);
			}
			menuItemList.Initialize();
			menuItemList.OnSelected = new Action<MenuItemButton, BaseEventData>(this.onEquipmentSelected);
		}
		ProfilingUtil.EndTimer("EQUIPMENT SELECTOR MODULE - setup menus");
		ProfilingUtil.BeginTimer();
		this.sortEquipment();
		ProfilingUtil.EndTimer("EQUIPMENT SELECTOR MODULE - sort equipment");
		this.redrawEquipmentList();
		this._needScrollInit = true;
	}

	// Token: 0x060049AD RID: 18861 RVA: 0x0013C634 File Offset: 0x0013AA34
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

	// Token: 0x060049AE RID: 18862 RVA: 0x0013C66C File Offset: 0x0013AA6C
	private void addEquipmentLine(EquippableItem item)
	{
		EquipmentLine equipmentLine = UnityEngine.Object.Instantiate<EquipmentLine>(this.EquipmentLinePrefab);
		base.injector.Inject(equipmentLine);
		this.equipmentTypeIndex[item.type].Add(equipmentLine);
		this.equipmentIndex[item.id] = equipmentLine;
		this.equipmentSort.Add(equipmentLine);
		equipmentLine.gameObject.SetActive(false);
		equipmentLine.SetItem(item, this.api);
		equipmentLine.transform.SetParent(this.scrollItemContainer.transform, false);
		if (!this.equipmentItemMenuIndex.ContainsKey(item.type))
		{
			MenuItemList instance = base.injector.GetInstance<MenuItemList>();
			if (this.currentRightEdgeNavigation != null)
			{
				instance.AddEdgeNavigation(MoveDirection.Right, this.currentRightEdgeNavigation);
			}
			this.equipmentItemMenuIndex.Add(item.type, instance);
		}
		this.equipmentItemMenuIndex[item.type].AddButton(equipmentLine.MenuItemButton, delegate(InputEventData data)
		{
			this.clickedEquipment(item, data);
		});
		this.equipmentItemMenuIndex[item.type].SetButtonEnabled(equipmentLine.MenuItemButton, false);
	}

	// Token: 0x060049AF RID: 18863 RVA: 0x0013C7C0 File Offset: 0x0013ABC0
	private void addEquipTypesList()
	{
		this.equipTypeMenu = base.injector.GetInstance<MenuItemList>();
		foreach (EquipTypeDefinition equipTypeDefinition in this.api.GetValidEquipTypes())
		{
			this.equipmentTypeIndex[equipTypeDefinition.type] = new List<EquipmentLine>();
			this.addEquipType(equipTypeDefinition);
		}
		this.equipTypeMenu.Initialize();
		this.equipTypeMenu.OnSelected = new Action<MenuItemButton, BaseEventData>(this.onEquipTypeSelected);
	}

	// Token: 0x060049B0 RID: 18864 RVA: 0x0013C841 File Offset: 0x0013AC41
	private bool isMouseEvent(BaseEventData eventData)
	{
		return eventData is PointerEventData;
	}

	// Token: 0x060049B1 RID: 18865 RVA: 0x0013C84C File Offset: 0x0013AC4C
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

	// Token: 0x060049B2 RID: 18866 RVA: 0x0013C884 File Offset: 0x0013AC84
	private void addEquipType(EquipTypeDefinition def)
	{
		EquipTypeButton component = UnityEngine.Object.Instantiate<GameObject>(this.EquipTypeButtonPrefab).GetComponent<EquipTypeButton>();
		this.equipTypeButtons[def.type] = component;
		component.transform.SetParent(this.EquipTypeMenu.transform, false);
		component.Type = def.type;
		component.Icon.sprite = def.icon;
		this.equipTypeMenu.AddButton(component.GetComponent<MenuItemButton>(), delegate(InputEventData data)
		{
			this.clickedEquipTypes(def.type, data);
		});
	}

	// Token: 0x060049B3 RID: 18867 RVA: 0x0013C928 File Offset: 0x0013AD28
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

	// Token: 0x060049B4 RID: 18868 RVA: 0x0013C9C7 File Offset: 0x0013ADC7
	private void clickedUnlockButton()
	{
		this.unlockEquipmentFlow.Start(this.currentlySelectedEquipment, new Action(this.onDataUpdate), new Action(this.onEquipNewPurchase));
	}

	// Token: 0x060049B5 RID: 18869 RVA: 0x0013C9F2 File Offset: 0x0013ADF2
	private void clickedEquipButton()
	{
		this.equipFlow.Start(this.currentlySelectedEquipment, this.api.SelectedCharacter);
	}

	// Token: 0x060049B6 RID: 18870 RVA: 0x0013CA10 File Offset: 0x0013AE10
	private void clickedAvailableButton()
	{
	}

	// Token: 0x060049B7 RID: 18871 RVA: 0x0013CA14 File Offset: 0x0013AE14
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

	// Token: 0x060049B8 RID: 18872 RVA: 0x0013CA74 File Offset: 0x0013AE74
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

	// Token: 0x060049B9 RID: 18873 RVA: 0x0013CB4F File Offset: 0x0013AF4F
	private void selectEquipment(EquippableItem item)
	{
		this.api.SelectedEquipment = item;
	}

	// Token: 0x060049BA RID: 18874 RVA: 0x0013CB5D File Offset: 0x0013AF5D
	private void onEquipNewPurchase()
	{
		this.equipFlow.Start(this.currentlySelectedEquipment, this.api.SelectedCharacter);
	}

	// Token: 0x060049BB RID: 18875 RVA: 0x0013CB7C File Offset: 0x0013AF7C
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

	// Token: 0x060049BC RID: 18876 RVA: 0x0013CBE4 File Offset: 0x0013AFE4
	private void syncSelection()
	{
		MenuItemButton controllerIntendedSelection = this.getControllerIntendedSelection();
		if (this.currentMenuList != null)
		{
			this.currentMenuList.AutoSelect(controllerIntendedSelection);
		}
	}

	// Token: 0x060049BD RID: 18877 RVA: 0x0013CC0F File Offset: 0x0013B00F
	private void selectEquipType(EquipmentTypes type)
	{
		this.api.SelectedEquipType = type;
	}

	// Token: 0x060049BE RID: 18878 RVA: 0x0013CC1D File Offset: 0x0013B01D
	private void onDataUpdate()
	{
		this.updateMenuType();
		this.updateCurrentlySelectedEquipType();
		this.updateEquipmentList();
		this.updateEquipmentSelected();
		this.updateEquipmentActionButtons();
		this.updateTitle();
	}

	// Token: 0x060049BF RID: 18879 RVA: 0x0013CC44 File Offset: 0x0013B044
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

	// Token: 0x060049C0 RID: 18880 RVA: 0x0013CC9F File Offset: 0x0013B09F
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

	// Token: 0x060049C1 RID: 18881 RVA: 0x0013CCD8 File Offset: 0x0013B0D8
	private string getTypeText()
	{
		string text = this.localization.GetText("equipType.plural." + this.api.SelectedEquipType);
		if (this.GetTypeText == null)
		{
			return text;
		}
		return this.GetTypeText(text);
	}

	// Token: 0x060049C2 RID: 18882 RVA: 0x0013CD24 File Offset: 0x0013B124
	private EquipTypeButton findButtonForType(EquipmentTypes id)
	{
		return (!this.equipTypeButtons.ContainsKey(id)) ? null : this.equipTypeButtons[id];
	}

	// Token: 0x060049C3 RID: 18883 RVA: 0x0013CD49 File Offset: 0x0013B149
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

	// Token: 0x060049C4 RID: 18884 RVA: 0x0013CD7C File Offset: 0x0013B17C
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

	// Token: 0x060049C5 RID: 18885 RVA: 0x0013CE19 File Offset: 0x0013B219
	public void RebuildList()
	{
		this.equipmentListDirty = true;
		this.updateEquipmentList();
		this.updateTitle();
	}

	// Token: 0x060049C6 RID: 18886 RVA: 0x0013CE2E File Offset: 0x0013B22E
	public void ForceRedraws()
	{
		this.redrawEquipmentList();
	}

	// Token: 0x060049C7 RID: 18887 RVA: 0x0013CE38 File Offset: 0x0013B238
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

	// Token: 0x170011A6 RID: 4518
	// (get) Token: 0x060049C8 RID: 18888 RVA: 0x0013CE84 File Offset: 0x0013B284
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

	// Token: 0x060049C9 RID: 18889 RVA: 0x0013CED4 File Offset: 0x0013B2D4
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
				foreach (EquipmentLine equipmentLine in array)
				{
					equipmentLine.gameObject.SetActive(false);
					equipmentLine.transform.SetParent(this.scrollItemContainer.transform, false);
					this.equipmentItemMenuIndex[equipmentTypes].SetButtonEnabled(equipmentLine.MenuItemButton, false);
				}
			}
			if (this.currentEquipmentList != EquipmentTypes.NONE)
			{
				foreach (EquipmentLine equipmentLine2 in this.currentlyActiveLines)
				{
					equipmentLine2.transform.SetParent(this.EquipmentMenu.transform, false);
					equipmentLine2.gameObject.SetActive(true);
					this.equipmentItemMenuIndex[this.currentEquipmentList].SetButtonEnabled(equipmentLine2.MenuItemButton, true);
					equipmentLine2.UpdateDynamicInfo();
				}
			}
		}
		else
		{
			foreach (EquipmentLine equipmentLine3 in this.currentlyActiveLines)
			{
				equipmentLine3.UpdateDynamicInfo();
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

	// Token: 0x060049CA RID: 18890 RVA: 0x0013D0C8 File Offset: 0x0013B4C8
	private void updateEquipmentList()
	{
		if (this.currentEquipmentList != this.api.SelectedEquipType || this.equipmentListDirty)
		{
			this.redrawEquipmentList();
		}
		else
		{
			foreach (EquipmentLine equipmentLine in this.currentlyActiveLines)
			{
				equipmentLine.UpdateDynamicInfo();
			}
		}
	}

	// Token: 0x060049CB RID: 18891 RVA: 0x0013D150 File Offset: 0x0013B550
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
			foreach (EquipmentLine equipmentLine in this.currentlyActiveLines)
			{
				if (!equipmentLine.Item.isDefault)
				{
					num++;
					if (this.api.HasItem(equipmentLine.Item.id))
					{
						num2++;
					}
				}
			}
			this.MainSelectorProgress.text = num2 + "/" + num;
		}
	}

	// Token: 0x060049CC RID: 18892 RVA: 0x0013D234 File Offset: 0x0013B634
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

	// Token: 0x060049CD RID: 18893 RVA: 0x0013D288 File Offset: 0x0013B688
	private EquippableItem getEquipmentToHighlight()
	{
		return this.api.SelectedEquipment;
	}

	// Token: 0x060049CE RID: 18894 RVA: 0x0013D298 File Offset: 0x0013B698
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

	// Token: 0x060049CF RID: 18895 RVA: 0x0013D3A0 File Offset: 0x0013B7A0
	private void onEquipmentUpdate()
	{
		foreach (EquipmentLine equipmentLine in this.currentlyActiveLines)
		{
			equipmentLine.UpdateDynamicInfo();
		}
		this.updateEquipmentActionButtons();
		this.updateProgressText();
	}

	// Token: 0x060049D0 RID: 18896 RVA: 0x0013D408 File Offset: 0x0013B808
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

	// Token: 0x060049D1 RID: 18897 RVA: 0x0013D510 File Offset: 0x0013B910
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

	// Token: 0x060049D2 RID: 18898 RVA: 0x0013D678 File Offset: 0x0013BA78
	private void tweenScroll(float target)
	{
		this.killScrollTween();
		this._scrollTween = DOTween.To(() => this.EquipmentScroll.verticalNormalizedPosition, delegate(float valueIn)
		{
			this.EquipmentScroll.verticalNormalizedPosition = valueIn;
		}, target, 0.15f).SetEase(Ease.OutCirc).OnComplete(new TweenCallback(this.killScrollTween));
	}

	// Token: 0x060049D3 RID: 18899 RVA: 0x0013D6CC File Offset: 0x0013BACC
	private void killScrollTween()
	{
		TweenUtil.Destroy(ref this._scrollTween);
	}

	// Token: 0x060049D4 RID: 18900 RVA: 0x0013D6DC File Offset: 0x0013BADC
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

	// Token: 0x060049D5 RID: 18901 RVA: 0x0013D868 File Offset: 0x0013BC68
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

	// Token: 0x060049D6 RID: 18902 RVA: 0x0013DA87 File Offset: 0x0013BE87
	private bool isAllowButtons()
	{
		return this.IsAllowButtons == null || this.IsAllowButtons();
	}

	// Token: 0x060049D7 RID: 18903 RVA: 0x0013DAA4 File Offset: 0x0013BEA4
	private void setBottomButtonsInteractable(bool value)
	{
		foreach (CharacterTabActionButton characterTabActionButton in this.bottomButtons)
		{
			characterTabActionButton.SetInteractable(value);
		}
	}

	// Token: 0x060049D8 RID: 18904 RVA: 0x0013DB00 File Offset: 0x0013BF00
	public override void OnDestroy()
	{
		if (this.scrollItemContainer != null)
		{
			UnityEngine.Object.DestroyImmediate(this.scrollItemContainer);
		}
		base.OnDestroy();
	}

	// Token: 0x04003064 RID: 12388
	public static string RESELECT_ITEM = "EquipmentSelectorModule.RESELECT_ITEM";

	// Token: 0x0400306D RID: 12397
	public EquipmentLine EquipmentLinePrefab;

	// Token: 0x0400306E RID: 12398
	public GameObject EquipTypeButtonPrefab;

	// Token: 0x0400306F RID: 12399
	public Sprite EquipTypeButtonHighlight;

	// Token: 0x04003070 RID: 12400
	public VerticalLayoutGroup EquipTypeMenu;

	// Token: 0x04003071 RID: 12401
	public LayoutGroup EquipmentMenu;

	// Token: 0x04003072 RID: 12402
	public TextMeshProUGUI MainSelectorTitle;

	// Token: 0x04003073 RID: 12403
	public TextMeshProUGUI MainSelectorProgress;

	// Token: 0x04003074 RID: 12404
	public GameObject EquipmentContent;

	// Token: 0x04003075 RID: 12405
	public CharacterTabActionButton UnlockButton;

	// Token: 0x04003076 RID: 12406
	public CharacterTabActionButton AvailableInButton;

	// Token: 0x04003077 RID: 12407
	public CharacterTabActionButton EquipButton;

	// Token: 0x04003078 RID: 12408
	public GameObject HasItemGroup;

	// Token: 0x04003079 RID: 12409
	public GameObject UnownedGroup;

	// Token: 0x0400307A RID: 12410
	public InputInstructions HasItemInputInstructions;

	// Token: 0x0400307B RID: 12411
	public InputInstructions UnownedItemInputInstructions;

	// Token: 0x0400307C RID: 12412
	public CanvasGroup ActionButtonsGroup;

	// Token: 0x0400307D RID: 12413
	public ScrollRect EquipmentScroll;

	// Token: 0x04003081 RID: 12417
	private int gridWidth;

	// Token: 0x04003086 RID: 12422
	private MenuItemList equipTypeMenu;

	// Token: 0x04003087 RID: 12423
	private MenuItemList bottomButtonMenu;

	// Token: 0x04003088 RID: 12424
	private List<EquippableItem> pendingItems;

	// Token: 0x04003089 RID: 12425
	private EquippableItem currentlySelectedEquipment;

	// Token: 0x0400308A RID: 12426
	private EquipmentTypes currentlySelectedEquipType = EquipmentTypes.NONE;

	// Token: 0x0400308B RID: 12427
	private EquipmentTypes currentEquipmentList = EquipmentTypes.NONE;

	// Token: 0x0400308C RID: 12428
	private EquipmentSelectorModule.MenuType currentlySelectedMenuType = EquipmentSelectorModule.MenuType.TYPE;

	// Token: 0x0400308D RID: 12429
	private MenuItemList currentRightEdgeNavigation;

	// Token: 0x0400308E RID: 12430
	private RectTransform equipContentRect;

	// Token: 0x0400308F RID: 12431
	private RectTransform equipListRect;

	// Token: 0x04003090 RID: 12432
	private Dictionary<EquipmentTypes, EquipTypeButton> equipTypeButtons = new Dictionary<EquipmentTypes, EquipTypeButton>();

	// Token: 0x04003091 RID: 12433
	private Dictionary<EquipmentTypes, List<EquipmentLine>> equipmentTypeIndex = new Dictionary<EquipmentTypes, List<EquipmentLine>>();

	// Token: 0x04003092 RID: 12434
	private Dictionary<EquipmentTypes, MenuItemList> equipmentItemMenuIndex = new Dictionary<EquipmentTypes, MenuItemList>();

	// Token: 0x04003093 RID: 12435
	private Dictionary<EquipmentID, EquipmentLine> equipmentIndex = new Dictionary<EquipmentID, EquipmentLine>();

	// Token: 0x04003094 RID: 12436
	private List<EquipmentLine> equipmentSort = new List<EquipmentLine>();

	// Token: 0x04003095 RID: 12437
	private List<EquipmentLine> currentlyActiveLines = new List<EquipmentLine>();

	// Token: 0x04003096 RID: 12438
	private List<CharacterTabActionButton> bottomButtons = new List<CharacterTabActionButton>();

	// Token: 0x04003097 RID: 12439
	private bool _needSyncScrollPosition;

	// Token: 0x04003098 RID: 12440
	private bool _needScrollInit;

	// Token: 0x04003099 RID: 12441
	private Vector3 _prevLinePosition;

	// Token: 0x0400309A RID: 12442
	private EquipmentLine _syncScrollTarget;

	// Token: 0x0400309B RID: 12443
	private Tweener _scrollTween;

	// Token: 0x0400309C RID: 12444
	private bool equipmentListDirty;

	// Token: 0x0400309D RID: 12445
	private IEquipModuleAPI api;

	// Token: 0x0400309E RID: 12446
	private bool isExternalMenuSelected;

	// Token: 0x0400309F RID: 12447
	private GameObject scrollItemContainer;

	// Token: 0x020009FE RID: 2558
	public enum MenuType
	{
		// Token: 0x040030A1 RID: 12449
		NONE,
		// Token: 0x040030A2 RID: 12450
		TYPE,
		// Token: 0x040030A3 RID: 12451
		ITEM
	}

	// Token: 0x020009FF RID: 2559
	public enum ItemDisplay
	{
		// Token: 0x040030A5 RID: 12453
		LIST,
		// Token: 0x040030A6 RID: 12454
		GRID
	}
}
