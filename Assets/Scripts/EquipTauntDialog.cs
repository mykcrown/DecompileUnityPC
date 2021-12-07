// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipTauntDialog : BaseWindow
{
	private sealed class _addTauntSlot_c__AnonStorey0
	{
		internal TauntSlot slot;

		internal EquipTauntDialog _this;

		internal void __m__0()
		{
			this._this.clickedSlot(this.slot);
		}
	}

	public GameObject TauntSlotPrefab;

	public VerticalLayoutGroup SlotList;

	public MenuItemButton SaveButton;

	public MenuItemButton CancelButton;

	private MenuItemList mainMenu;

	private MenuItemList bottomButtons;

	private EquippableItem item;

	private Dictionary<TauntSlot, TauntSlotDisplay> displays = new Dictionary<TauntSlot, TauntSlotDisplay>();

	private List<TauntSlot> slotList = new List<TauntSlot>();

	[Inject]
	public EquipTauntDialogAPI api
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

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

	public void OnCancel()
	{
		this.Close();
	}

	private void onSave()
	{
		this.api.Save();
		this.Close();
	}

	public override void OnCancelPressed()
	{
		this.onSave();
	}

	public void Setup(EquippableItem item, CharacterID characterId)
	{
		this.item = item;
		this.api.Setup(characterId);
		this.populate();
		this.updateList();
		MenuItemButton menuItemButton = this.mainMenu.GetButtons()[0];
		this.FirstSelected = menuItemButton.InteractableButton.gameObject;
		this.mainMenu.AutoSelect(menuItemButton);
	}

	private void populate()
	{
		this.mainMenu = base.injector.GetInstance<MenuItemList>();
		this.addTauntSlot(TauntSlot.UP);
		this.addTauntSlot(TauntSlot.LEFT);
		this.addTauntSlot(TauntSlot.RIGHT);
		this.addTauntSlot(TauntSlot.DOWN);
		this.mainMenu.Initialize();
		this.bottomButtons = base.injector.GetInstance<MenuItemList>();
		this.bottomButtons.AddButton(this.SaveButton, new Action(this.onSave));
		this.bottomButtons.AddButton(this.CancelButton, new Action(this.OnCancel));
		this.bottomButtons.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		this.bottomButtons.Initialize();
		MenuItemButton[] buttons = this.mainMenu.GetButtons();
		MenuItemButton landingPoint = buttons[buttons.Length - 1];
		this.mainMenu.LandingPoint = landingPoint;
		this.bottomButtons.LandingPoint = this.SaveButton;
		this.mainMenu.AddEdgeNavigation(MoveDirection.Down, this.bottomButtons);
		this.bottomButtons.AddEdgeNavigation(MoveDirection.Up, this.mainMenu);
	}

	private void addTauntSlot(TauntSlot slot)
	{
		EquipTauntDialog._addTauntSlot_c__AnonStorey0 _addTauntSlot_c__AnonStorey = new EquipTauntDialog._addTauntSlot_c__AnonStorey0();
		_addTauntSlot_c__AnonStorey.slot = slot;
		_addTauntSlot_c__AnonStorey._this = this;
		TauntSlotDisplay component = UnityEngine.Object.Instantiate<GameObject>(this.TauntSlotPrefab).GetComponent<TauntSlotDisplay>();
		base.injector.Inject(component);
		component.transform.SetParent(this.SlotList.transform, false);
		component.SetSlot(_addTauntSlot_c__AnonStorey.slot);
		this.displays[_addTauntSlot_c__AnonStorey.slot] = component;
		this.slotList.Add(_addTauntSlot_c__AnonStorey.slot);
		MenuItemButton component2 = component.GetComponent<MenuItemButton>();
		this.mainMenu.AddButton(component2, new Action(_addTauntSlot_c__AnonStorey.__m__0));
	}

	private void clickedSlot(TauntSlot slot)
	{
		this.api.ToggleSlot(slot, this.item);
		this.updateList();
	}

	private void updateList()
	{
		Dictionary<TauntSlot, EquipmentID> slots = this.api.GetSlots();
		foreach (TauntSlot current in this.slotList)
		{
			TauntSlotDisplay tauntSlotDisplay = this.displays[current];
			if (slots.ContainsKey(current))
			{
				EquippableItem equippableItem = this.equipmentModel.GetItem(slots[current]);
				if (equippableItem != null)
				{
					tauntSlotDisplay.SetAssigned(equippableItem);
					continue;
				}
			}
			tauntSlotDisplay.SetUnassigned();
		}
	}
}
