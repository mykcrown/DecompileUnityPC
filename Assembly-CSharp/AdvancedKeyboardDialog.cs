using System;
using System.Collections.Generic;
using InControl;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200096C RID: 2412
public class AdvancedKeyboardDialog : BaseWindow
{
	// Token: 0x17000F43 RID: 3907
	// (get) Token: 0x0600409B RID: 16539 RVA: 0x00122DF7 File Offset: 0x001211F7
	// (set) Token: 0x0600409C RID: 16540 RVA: 0x00122DFF File Offset: 0x001211FF
	[Inject]
	public IInputSettingsScreenAPI api { get; set; }

	// Token: 0x17000F44 RID: 3908
	// (get) Token: 0x0600409D RID: 16541 RVA: 0x00122E08 File Offset: 0x00121208
	// (set) Token: 0x0600409E RID: 16542 RVA: 0x00122E10 File Offset: 0x00121210
	[Inject]
	public ISelectionManager selectionManager { get; set; }

	// Token: 0x17000F45 RID: 3909
	// (get) Token: 0x0600409F RID: 16543 RVA: 0x00122E19 File Offset: 0x00121219
	// (set) Token: 0x060040A0 RID: 16544 RVA: 0x00122E21 File Offset: 0x00121221
	[Inject]
	public DirectionalBindingHelper bindingHelper { get; set; }

	// Token: 0x17000F46 RID: 3910
	// (get) Token: 0x060040A1 RID: 16545 RVA: 0x00122E2A File Offset: 0x0012122A
	// (set) Token: 0x060040A2 RID: 16546 RVA: 0x00122E34 File Offset: 0x00121234
	private DirectionalBindingHelper.DirectionalButtonGroup selectedGroup
	{
		get
		{
			return this._selectedGroup;
		}
		set
		{
			if (this._selectedGroup != value)
			{
				this._selectedGroup = value;
				if (this.buttonGroupButtons.ContainsKey(this._selectedGroup))
				{
					this.FirstSelected = this.buttonGroupButtons[this._selectedGroup].InteractableButton.gameObject;
				}
				this.onUpdate();
			}
		}
	}

	// Token: 0x060040A3 RID: 16547 RVA: 0x00122E94 File Offset: 0x00121294
	[PostConstruct]
	public void Init()
	{
		this.buttonGroupMenu = base.injector.GetInstance<MenuItemList>();
		this.addButtonGroupButton(DirectionalBindingHelper.DirectionalButtonGroup.Movement);
		this.addButtonGroupButton(DirectionalBindingHelper.DirectionalButtonGroup.Emotes);
		this.addButtonGroupButton(DirectionalBindingHelper.DirectionalButtonGroup.Tilts);
		this.addButtonGroupButton(DirectionalBindingHelper.DirectionalButtonGroup.Strikes);
		this.addButtonGroupButton(DirectionalBindingHelper.DirectionalButtonGroup.Specials);
		MenuItemList menuItemList = this.buttonGroupMenu;
		menuItemList.OnSelected = (Action<MenuItemButton, BaseEventData>)Delegate.Combine(menuItemList.OnSelected, new Action<MenuItemButton, BaseEventData>(this.onButtonGroupSelected));
		this.confirmMenu = base.injector.GetInstance<MenuItemList>();
		this.confirmMenu.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		this.confirmMenu.AddButton(this.ConfirmButton, new Action(this.onConfirmButton));
		this.confirmMenu.AddButton(this.ResetButton, new Action(this.onResetButton));
		this.addBindingButton(AbsoluteDirection.Up, base.localization.GetText("ui.dialog.advancedKeyboard.up"), this.UpAnchor);
		this.addBindingButton(AbsoluteDirection.Down, base.localization.GetText("ui.dialog.advancedKeyboard.down"), this.DownAnchor);
		this.addBindingButton(AbsoluteDirection.Left, base.localization.GetText("ui.dialog.advancedKeyboard.left"), this.LeftAnchor);
		this.addBindingButton(AbsoluteDirection.Right, base.localization.GetText("ui.dialog.advancedKeyboard.right"), this.RightAnchor);
		this.cachedGroupButtonBackground = this.buttonGroupButtons[DirectionalBindingHelper.DirectionalButtonGroup.Movement].ButtonBackground.sprite;
		this.buttonGroupMenu.LandingPoint = this.buttonGroupMenu.GetButtons()[0];
		this.confirmMenu.LandingPoint = this.ConfirmButton;
		foreach (AbsoluteDirection key in this.bindingMenus.Keys)
		{
			MenuItemList menuItemList2 = this.bindingMenus[key];
			menuItemList2.AddEdgeNavigation(MoveDirection.Up, this.bindingMenus[AbsoluteDirection.Up]);
			menuItemList2.AddEdgeNavigation(MoveDirection.Down, this.bindingMenus[AbsoluteDirection.Down]);
			menuItemList2.AddEdgeNavigation(MoveDirection.Left, this.bindingMenus[AbsoluteDirection.Left]);
			menuItemList2.AddEdgeNavigation(MoveDirection.Right, this.bindingMenus[AbsoluteDirection.Right]);
		}
		this.buttonGroupMenu.AddEdgeNavigation(MoveDirection.Right, this.bindingMenus[AbsoluteDirection.Left]);
		this.bindingMenus[AbsoluteDirection.Left].AddEdgeNavigation(MoveDirection.Left, this.buttonGroupMenu);
		this.confirmMenu.AddEdgeNavigation(MoveDirection.Up, this.bindingMenus[AbsoluteDirection.Down]);
		this.bindingMenus[AbsoluteDirection.Down].AddEdgeNavigation(MoveDirection.Down, this.confirmMenu);
		foreach (MenuItemList menuItemList3 in this.bindingMenus.Values)
		{
			menuItemList3.Initialize();
		}
		this.buttonGroupMenu.Initialize();
		this.confirmMenu.Initialize();
		this.unbindText = UnityEngine.Object.Instantiate<UnbindText>(this.UnbindTextPrefab, this.UnbindTextAnchor);
		this.movementUnboundText = UnityEngine.Object.Instantiate<GameObject>(this.MovementUnboundPrefab, this.MovementUnboundAnchor);
		this.FirstSelected = this.buttonGroupMenu.GetButtons()[0].InteractableButton.gameObject;
		base.listen(InputSettingsScreenAPI.UPDATED, new Action(this.onUpdate));
		base.listen(InputSettingsScreenAPI.UNBOUND, new Action(this.onUnbound));
	}

	// Token: 0x060040A4 RID: 16548 RVA: 0x001231F4 File Offset: 0x001215F4
	private void Start()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.ButtonGroupAnchor.transform);
		this.onUpdate();
	}

	// Token: 0x060040A5 RID: 16549 RVA: 0x00123214 File Offset: 0x00121614
	private void addButtonGroupButton(DirectionalBindingHelper.DirectionalButtonGroup buttonGroup)
	{
		MenuItemButton menuItemButton = UnityEngine.Object.Instantiate<MenuItemButton>(this.ButtonGroupPrefab, this.ButtonGroupAnchor);
		menuItemButton.TextField.text = ControlsSettingsTextHelper.GetDirectionalButtonGroupText(base.localization, buttonGroup);
		this.buttonGroupMenu.AddButton(menuItemButton, new Action(this.onButtonGroupPressed));
		this.buttonGroupButtons[buttonGroup] = menuItemButton;
	}

	// Token: 0x060040A6 RID: 16550 RVA: 0x00123270 File Offset: 0x00121670
	private void addBindingButton(AbsoluteDirection moveDirection, string title, Transform anchor)
	{
		BindingsUIGroup bindingsUIGroup = UnityEngine.Object.Instantiate<BindingsUIGroup>(this.BindingButtonPrefab, anchor);
		bindingsUIGroup.Title.text = title;
		InputBindingButton inputBindingButton = bindingsUIGroup.InputBindingButton[0];
		this.bindingButtons[moveDirection] = inputBindingButton;
		MenuItemList instance = base.injector.GetInstance<MenuItemList>();
		instance.AddButton(inputBindingButton.Button, delegate()
		{
			this.onBindingClicked(moveDirection);
		});
		instance.LandingPoint = inputBindingButton.Button;
		this.bindingMenus[moveDirection] = instance;
	}

	// Token: 0x060040A7 RID: 16551 RVA: 0x0012330C File Offset: 0x0012170C
	private void onUpdate()
	{
		foreach (AbsoluteDirection absoluteDirection in this.bindingButtons.Keys)
		{
			InputBindingButton inputBindingButton = this.bindingButtons[absoluteDirection];
			ButtonPress buttonFromButtonGroup = this.bindingHelper.GetButtonFromButtonGroup(this.selectedGroup, absoluteDirection);
			bool flag = this.api.IsListeningForBinding && buttonFromButtonGroup == this.api.ListeningButtonPress;
			BindingSource bindingSource = this.api.GetBindingSource(buttonFromButtonGroup, 0);
			inputBindingButton.BindingText.text = ControlsSettingsTextHelper.GetBindingText(base.localization, bindingSource, flag);
			inputBindingButton.SetFlashing(flag);
		}
		if (this.selectedGroup == DirectionalBindingHelper.DirectionalButtonGroup.None)
		{
			this.AnchorsCanvasGroup.alpha = 0f;
		}
		else
		{
			this.AnchorsCanvasGroup.alpha = 1f;
			foreach (DirectionalBindingHelper.DirectionalButtonGroup directionalButtonGroup in this.buttonGroupButtons.Keys)
			{
				MenuItemButton menuItemButton = this.buttonGroupButtons[directionalButtonGroup];
				if (directionalButtonGroup == this.selectedGroup)
				{
					menuItemButton.ButtonBackground.sprite = this.SecondarySelect;
				}
				else
				{
					menuItemButton.ButtonBackground.sprite = this.cachedGroupButtonBackground;
				}
			}
		}
		this.ButtonGroupTitle.text = ControlsSettingsTextHelper.GetDirectionalButtonGroupText(base.localization, this.selectedGroup);
		this.movementUnboundText.gameObject.SetActive(this.api.IsMovementUnbound);
	}

	// Token: 0x060040A8 RID: 16552 RVA: 0x001234D4 File Offset: 0x001218D4
	private void onButtonGroupSelected(MenuItemButton button, BaseEventData eventData)
	{
		foreach (DirectionalBindingHelper.DirectionalButtonGroup directionalButtonGroup in this.buttonGroupButtons.Keys)
		{
			MenuItemButton x = this.buttonGroupButtons[directionalButtonGroup];
			if (x == button)
			{
				this.selectedGroup = directionalButtonGroup;
				break;
			}
		}
	}

	// Token: 0x060040A9 RID: 16553 RVA: 0x00123554 File Offset: 0x00121954
	private void onButtonGroupPressed()
	{
		MenuItemList menuItemList = this.bindingMenus[AbsoluteDirection.Left];
		menuItemList.AutoSelect(menuItemList.GetButtons()[0]);
	}

	// Token: 0x060040AA RID: 16554 RVA: 0x0012357C File Offset: 0x0012197C
	private void onBindingClicked(AbsoluteDirection direction)
	{
		InputBindingButton inputBindingButton = this.bindingButtons[direction];
		ButtonPress buttonFromButtonGroup = this.bindingHelper.GetButtonFromButtonGroup(this.selectedGroup, direction);
		this.api.ListenForBindingSource(buttonFromButtonGroup, 0);
		base.audioManager.PlayMenuSound(SoundKey.settings_controlBindingSelect, 0f);
	}

	// Token: 0x060040AB RID: 16555 RVA: 0x001235C8 File Offset: 0x001219C8
	private void onConfirmButton()
	{
		this.Close();
	}

	// Token: 0x060040AC RID: 16556 RVA: 0x001235D0 File Offset: 0x001219D0
	private void onResetButton()
	{
		GenericDialog genericDialog = base.dialogController.ShowTwoButtonDialog(base.localization.GetText("ui.settings.resetWarning.title"), base.localization.GetText("ui.settings.resetWarning.body"), base.localization.GetText("ui.settings.resetWarning.confirm"), base.localization.GetText("ui.settings.resetWarning.cancel"));
		GenericDialog genericDialog2 = genericDialog;
		genericDialog2.ConfirmCallback = (Action)Delegate.Combine(genericDialog2.ConfirmCallback, new Action(delegate()
		{
			this.api.ResetControls();
			base.audioManager.PlayMenuSound(SoundKey.settings_settingsReset, 0f);
		}));
	}

	// Token: 0x060040AD RID: 16557 RVA: 0x0012364C File Offset: 0x00121A4C
	public override void OnCancelPressed()
	{
		foreach (MenuItemList menuItemList in this.bindingMenus.Values)
		{
			MenuItemButton y = menuItemList.GetButtons()[0];
			if (menuItemList.CurrentSelection == y)
			{
				if (this.buttonGroupButtons.ContainsKey(this.selectedGroup))
				{
					MenuItemButton firstSelected = this.buttonGroupButtons[this.selectedGroup];
					this.buttonGroupMenu.AutoSelect(firstSelected);
				}
				else
				{
					this.selectionManager.Select(null);
				}
				return;
			}
		}
		this.Close();
	}

	// Token: 0x060040AE RID: 16558 RVA: 0x00123710 File Offset: 0x00121B10
	private void onUnbound()
	{
		string unboundBindingText = ControlsSettingsTextHelper.GetUnboundBindingText(base.localization, this.api.LastUnboundButtonPress, this.api.LastUnboundBinding);
		if (!string.IsNullOrEmpty(unboundBindingText))
		{
			this.unbindText.FlashText(unboundBindingText, 0f, 1f, 1f);
		}
	}

	// Token: 0x04002B89 RID: 11145
	public Transform ButtonGroupAnchor;

	// Token: 0x04002B8A RID: 11146
	public MenuItemButton ButtonGroupPrefab;

	// Token: 0x04002B8B RID: 11147
	public TextMeshProUGUI ButtonGroupTitle;

	// Token: 0x04002B8C RID: 11148
	public Sprite SecondarySelect;

	// Token: 0x04002B8D RID: 11149
	[Space]
	public BindingsUIGroup BindingButtonPrefab;

	// Token: 0x04002B8E RID: 11150
	public CanvasGroup AnchorsCanvasGroup;

	// Token: 0x04002B8F RID: 11151
	public Transform UpAnchor;

	// Token: 0x04002B90 RID: 11152
	public Transform DownAnchor;

	// Token: 0x04002B91 RID: 11153
	public Transform LeftAnchor;

	// Token: 0x04002B92 RID: 11154
	public Transform RightAnchor;

	// Token: 0x04002B93 RID: 11155
	[Space]
	public Transform UnbindTextAnchor;

	// Token: 0x04002B94 RID: 11156
	public UnbindText UnbindTextPrefab;

	// Token: 0x04002B95 RID: 11157
	public Transform MovementUnboundAnchor;

	// Token: 0x04002B96 RID: 11158
	public GameObject MovementUnboundPrefab;

	// Token: 0x04002B97 RID: 11159
	[Space]
	public MenuItemButton ConfirmButton;

	// Token: 0x04002B98 RID: 11160
	public MenuItemButton ResetButton;

	// Token: 0x04002B99 RID: 11161
	private Sprite cachedGroupButtonBackground;

	// Token: 0x04002B9A RID: 11162
	private Dictionary<DirectionalBindingHelper.DirectionalButtonGroup, MenuItemButton> buttonGroupButtons = new Dictionary<DirectionalBindingHelper.DirectionalButtonGroup, MenuItemButton>();

	// Token: 0x04002B9B RID: 11163
	private Dictionary<AbsoluteDirection, InputBindingButton> bindingButtons = new Dictionary<AbsoluteDirection, InputBindingButton>();

	// Token: 0x04002B9C RID: 11164
	private MenuItemList buttonGroupMenu;

	// Token: 0x04002B9D RID: 11165
	private MenuItemList confirmMenu;

	// Token: 0x04002B9E RID: 11166
	private Dictionary<AbsoluteDirection, MenuItemList> bindingMenus = new Dictionary<AbsoluteDirection, MenuItemList>();

	// Token: 0x04002B9F RID: 11167
	private UnbindText unbindText;

	// Token: 0x04002BA0 RID: 11168
	private GameObject movementUnboundText;

	// Token: 0x04002BA1 RID: 11169
	private DirectionalBindingHelper.DirectionalButtonGroup _selectedGroup;
}
