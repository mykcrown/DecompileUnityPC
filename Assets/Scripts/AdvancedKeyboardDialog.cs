// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AdvancedKeyboardDialog : BaseWindow
{
	private sealed class _addBindingButton_c__AnonStorey0
	{
		internal AbsoluteDirection moveDirection;

		internal AdvancedKeyboardDialog _this;

		internal void __m__0()
		{
			this._this.onBindingClicked(this.moveDirection);
		}
	}

	public Transform ButtonGroupAnchor;

	public MenuItemButton ButtonGroupPrefab;

	public TextMeshProUGUI ButtonGroupTitle;

	public Sprite SecondarySelect;

	[Space]
	public BindingsUIGroup BindingButtonPrefab;

	public CanvasGroup AnchorsCanvasGroup;

	public Transform UpAnchor;

	public Transform DownAnchor;

	public Transform LeftAnchor;

	public Transform RightAnchor;

	[Space]
	public Transform UnbindTextAnchor;

	public UnbindText UnbindTextPrefab;

	public Transform MovementUnboundAnchor;

	public GameObject MovementUnboundPrefab;

	[Space]
	public MenuItemButton ConfirmButton;

	public MenuItemButton ResetButton;

	private Sprite cachedGroupButtonBackground;

	private Dictionary<DirectionalBindingHelper.DirectionalButtonGroup, MenuItemButton> buttonGroupButtons = new Dictionary<DirectionalBindingHelper.DirectionalButtonGroup, MenuItemButton>();

	private Dictionary<AbsoluteDirection, InputBindingButton> bindingButtons = new Dictionary<AbsoluteDirection, InputBindingButton>();

	private MenuItemList buttonGroupMenu;

	private MenuItemList confirmMenu;

	private Dictionary<AbsoluteDirection, MenuItemList> bindingMenus = new Dictionary<AbsoluteDirection, MenuItemList>();

	private UnbindText unbindText;

	private GameObject movementUnboundText;

	private DirectionalBindingHelper.DirectionalButtonGroup _selectedGroup;

	[Inject]
	public IInputSettingsScreenAPI api
	{
		get;
		set;
	}

	[Inject]
	public ISelectionManager selectionManager
	{
		get;
		set;
	}

	[Inject]
	public DirectionalBindingHelper bindingHelper
	{
		get;
		set;
	}

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

	[PostConstruct]
	public void Init()
	{
		this.buttonGroupMenu = base.injector.GetInstance<MenuItemList>();
		this.addButtonGroupButton(DirectionalBindingHelper.DirectionalButtonGroup.Movement);
		this.addButtonGroupButton(DirectionalBindingHelper.DirectionalButtonGroup.Emotes);
		this.addButtonGroupButton(DirectionalBindingHelper.DirectionalButtonGroup.Tilts);
		this.addButtonGroupButton(DirectionalBindingHelper.DirectionalButtonGroup.Strikes);
		this.addButtonGroupButton(DirectionalBindingHelper.DirectionalButtonGroup.Specials);
		MenuItemList expr_3A = this.buttonGroupMenu;
		expr_3A.OnSelected = (Action<MenuItemButton, BaseEventData>)Delegate.Combine(expr_3A.OnSelected, new Action<MenuItemButton, BaseEventData>(this.onButtonGroupSelected));
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
		foreach (AbsoluteDirection current in this.bindingMenus.Keys)
		{
			MenuItemList menuItemList = this.bindingMenus[current];
			menuItemList.AddEdgeNavigation(MoveDirection.Up, this.bindingMenus[AbsoluteDirection.Up]);
			menuItemList.AddEdgeNavigation(MoveDirection.Down, this.bindingMenus[AbsoluteDirection.Down]);
			menuItemList.AddEdgeNavigation(MoveDirection.Left, this.bindingMenus[AbsoluteDirection.Left]);
			menuItemList.AddEdgeNavigation(MoveDirection.Right, this.bindingMenus[AbsoluteDirection.Right]);
		}
		this.buttonGroupMenu.AddEdgeNavigation(MoveDirection.Right, this.bindingMenus[AbsoluteDirection.Left]);
		this.bindingMenus[AbsoluteDirection.Left].AddEdgeNavigation(MoveDirection.Left, this.buttonGroupMenu);
		this.confirmMenu.AddEdgeNavigation(MoveDirection.Up, this.bindingMenus[AbsoluteDirection.Down]);
		this.bindingMenus[AbsoluteDirection.Down].AddEdgeNavigation(MoveDirection.Down, this.confirmMenu);
		foreach (MenuItemList current2 in this.bindingMenus.Values)
		{
			current2.Initialize();
		}
		this.buttonGroupMenu.Initialize();
		this.confirmMenu.Initialize();
		this.unbindText = UnityEngine.Object.Instantiate<UnbindText>(this.UnbindTextPrefab, this.UnbindTextAnchor);
		this.movementUnboundText = UnityEngine.Object.Instantiate<GameObject>(this.MovementUnboundPrefab, this.MovementUnboundAnchor);
		this.FirstSelected = this.buttonGroupMenu.GetButtons()[0].InteractableButton.gameObject;
		base.listen(InputSettingsScreenAPI.UPDATED, new Action(this.onUpdate));
		base.listen(InputSettingsScreenAPI.UNBOUND, new Action(this.onUnbound));
	}

	private void Start()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.ButtonGroupAnchor.transform);
		this.onUpdate();
	}

	private void addButtonGroupButton(DirectionalBindingHelper.DirectionalButtonGroup buttonGroup)
	{
		MenuItemButton menuItemButton = UnityEngine.Object.Instantiate<MenuItemButton>(this.ButtonGroupPrefab, this.ButtonGroupAnchor);
		menuItemButton.TextField.text = ControlsSettingsTextHelper.GetDirectionalButtonGroupText(base.localization, buttonGroup);
		this.buttonGroupMenu.AddButton(menuItemButton, new Action(this.onButtonGroupPressed));
		this.buttonGroupButtons[buttonGroup] = menuItemButton;
	}

	private void addBindingButton(AbsoluteDirection moveDirection, string title, Transform anchor)
	{
		AdvancedKeyboardDialog._addBindingButton_c__AnonStorey0 _addBindingButton_c__AnonStorey = new AdvancedKeyboardDialog._addBindingButton_c__AnonStorey0();
		_addBindingButton_c__AnonStorey.moveDirection = moveDirection;
		_addBindingButton_c__AnonStorey._this = this;
		BindingsUIGroup bindingsUIGroup = UnityEngine.Object.Instantiate<BindingsUIGroup>(this.BindingButtonPrefab, anchor);
		bindingsUIGroup.Title.text = title;
		InputBindingButton inputBindingButton = bindingsUIGroup.InputBindingButton[0];
		this.bindingButtons[_addBindingButton_c__AnonStorey.moveDirection] = inputBindingButton;
		MenuItemList instance = base.injector.GetInstance<MenuItemList>();
		instance.AddButton(inputBindingButton.Button, new Action(_addBindingButton_c__AnonStorey.__m__0));
		instance.LandingPoint = inputBindingButton.Button;
		this.bindingMenus[_addBindingButton_c__AnonStorey.moveDirection] = instance;
	}

	private void onUpdate()
	{
		foreach (AbsoluteDirection current in this.bindingButtons.Keys)
		{
			InputBindingButton inputBindingButton = this.bindingButtons[current];
			ButtonPress buttonFromButtonGroup = this.bindingHelper.GetButtonFromButtonGroup(this.selectedGroup, current);
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
			foreach (DirectionalBindingHelper.DirectionalButtonGroup current2 in this.buttonGroupButtons.Keys)
			{
				MenuItemButton menuItemButton = this.buttonGroupButtons[current2];
				if (current2 == this.selectedGroup)
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

	private void onButtonGroupSelected(MenuItemButton button, BaseEventData eventData)
	{
		foreach (DirectionalBindingHelper.DirectionalButtonGroup current in this.buttonGroupButtons.Keys)
		{
			MenuItemButton x = this.buttonGroupButtons[current];
			if (x == button)
			{
				this.selectedGroup = current;
				break;
			}
		}
	}

	private void onButtonGroupPressed()
	{
		MenuItemList menuItemList = this.bindingMenus[AbsoluteDirection.Left];
		menuItemList.AutoSelect(menuItemList.GetButtons()[0]);
	}

	private void onBindingClicked(AbsoluteDirection direction)
	{
		InputBindingButton inputBindingButton = this.bindingButtons[direction];
		ButtonPress buttonFromButtonGroup = this.bindingHelper.GetButtonFromButtonGroup(this.selectedGroup, direction);
		this.api.ListenForBindingSource(buttonFromButtonGroup, 0);
		base.audioManager.PlayMenuSound(SoundKey.settings_controlBindingSelect, 0f);
	}

	private void onConfirmButton()
	{
		this.Close();
	}

	private void onResetButton()
	{
		GenericDialog genericDialog = base.dialogController.ShowTwoButtonDialog(base.localization.GetText("ui.settings.resetWarning.title"), base.localization.GetText("ui.settings.resetWarning.body"), base.localization.GetText("ui.settings.resetWarning.confirm"), base.localization.GetText("ui.settings.resetWarning.cancel"));
		GenericDialog expr_4D = genericDialog;
		expr_4D.ConfirmCallback = (Action)Delegate.Combine(expr_4D.ConfirmCallback, new Action(this._onResetButton_m__0));
	}

	public override void OnCancelPressed()
	{
		foreach (MenuItemList current in this.bindingMenus.Values)
		{
			MenuItemButton y = current.GetButtons()[0];
			if (current.CurrentSelection == y)
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

	private void onUnbound()
	{
		string unboundBindingText = ControlsSettingsTextHelper.GetUnboundBindingText(base.localization, this.api.LastUnboundButtonPress, this.api.LastUnboundBinding);
		if (!string.IsNullOrEmpty(unboundBindingText))
		{
			this.unbindText.FlashText(unboundBindingText, 0f, 1f, 1f);
		}
	}

	private void _onResetButton_m__0()
	{
		this.api.ResetControls();
		base.audioManager.PlayMenuSound(SoundKey.settings_settingsReset, 0f);
	}
}
