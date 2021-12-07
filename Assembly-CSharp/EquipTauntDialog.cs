using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000A01 RID: 2561
public class EquipTauntDialog : BaseWindow
{
	// Token: 0x170011A7 RID: 4519
	// (get) Token: 0x060049ED RID: 18925 RVA: 0x0013DBA6 File Offset: 0x0013BFA6
	// (set) Token: 0x060049EE RID: 18926 RVA: 0x0013DBAE File Offset: 0x0013BFAE
	[Inject]
	public EquipTauntDialogAPI api { get; set; }

	// Token: 0x170011A8 RID: 4520
	// (get) Token: 0x060049EF RID: 18927 RVA: 0x0013DBB7 File Offset: 0x0013BFB7
	// (set) Token: 0x060049F0 RID: 18928 RVA: 0x0013DBBF File Offset: 0x0013BFBF
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x170011A9 RID: 4521
	// (get) Token: 0x060049F1 RID: 18929 RVA: 0x0013DBC8 File Offset: 0x0013BFC8
	// (set) Token: 0x060049F2 RID: 18930 RVA: 0x0013DBD0 File Offset: 0x0013BFD0
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x060049F3 RID: 18931 RVA: 0x0013DBD9 File Offset: 0x0013BFD9
	public void OnCancel()
	{
		this.Close();
	}

	// Token: 0x060049F4 RID: 18932 RVA: 0x0013DBE1 File Offset: 0x0013BFE1
	private void onSave()
	{
		this.api.Save();
		this.Close();
	}

	// Token: 0x060049F5 RID: 18933 RVA: 0x0013DBF4 File Offset: 0x0013BFF4
	public override void OnCancelPressed()
	{
		this.onSave();
	}

	// Token: 0x060049F6 RID: 18934 RVA: 0x0013DBFC File Offset: 0x0013BFFC
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

	// Token: 0x060049F7 RID: 18935 RVA: 0x0013DC54 File Offset: 0x0013C054
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

	// Token: 0x060049F8 RID: 18936 RVA: 0x0013DD54 File Offset: 0x0013C154
	private void addTauntSlot(TauntSlot slot)
	{
		TauntSlotDisplay component = UnityEngine.Object.Instantiate<GameObject>(this.TauntSlotPrefab).GetComponent<TauntSlotDisplay>();
		base.injector.Inject(component);
		component.transform.SetParent(this.SlotList.transform, false);
		component.SetSlot(slot);
		this.displays[slot] = component;
		this.slotList.Add(slot);
		MenuItemButton component2 = component.GetComponent<MenuItemButton>();
		this.mainMenu.AddButton(component2, delegate()
		{
			this.clickedSlot(slot);
		});
	}

	// Token: 0x060049F9 RID: 18937 RVA: 0x0013DDF7 File Offset: 0x0013C1F7
	private void clickedSlot(TauntSlot slot)
	{
		this.api.ToggleSlot(slot, this.item);
		this.updateList();
	}

	// Token: 0x060049FA RID: 18938 RVA: 0x0013DE14 File Offset: 0x0013C214
	private void updateList()
	{
		Dictionary<TauntSlot, EquipmentID> slots = this.api.GetSlots();
		foreach (TauntSlot key in this.slotList)
		{
			TauntSlotDisplay tauntSlotDisplay = this.displays[key];
			if (slots.ContainsKey(key))
			{
				EquippableItem equippableItem = this.equipmentModel.GetItem(slots[key]);
				if (equippableItem != null)
				{
					tauntSlotDisplay.SetAssigned(equippableItem);
					continue;
				}
			}
			tauntSlotDisplay.SetUnassigned();
		}
	}

	// Token: 0x040030AA RID: 12458
	public GameObject TauntSlotPrefab;

	// Token: 0x040030AB RID: 12459
	public VerticalLayoutGroup SlotList;

	// Token: 0x040030AC RID: 12460
	public MenuItemButton SaveButton;

	// Token: 0x040030AD RID: 12461
	public MenuItemButton CancelButton;

	// Token: 0x040030AE RID: 12462
	private MenuItemList mainMenu;

	// Token: 0x040030AF RID: 12463
	private MenuItemList bottomButtons;

	// Token: 0x040030B0 RID: 12464
	private EquippableItem item;

	// Token: 0x040030B1 RID: 12465
	private Dictionary<TauntSlot, TauntSlotDisplay> displays = new Dictionary<TauntSlot, TauntSlotDisplay>();

	// Token: 0x040030B2 RID: 12466
	private List<TauntSlot> slotList = new List<TauntSlot>();
}
