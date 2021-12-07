using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000947 RID: 2375
public class MenuItemList
{
	// Token: 0x17000EEB RID: 3819
	// (get) Token: 0x06003ED2 RID: 16082 RVA: 0x0011DB40 File Offset: 0x0011BF40
	// (set) Token: 0x06003ED3 RID: 16083 RVA: 0x0011DB48 File Offset: 0x0011BF48
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17000EEC RID: 3820
	// (get) Token: 0x06003ED4 RID: 16084 RVA: 0x0011DB51 File Offset: 0x0011BF51
	// (set) Token: 0x06003ED5 RID: 16085 RVA: 0x0011DB59 File Offset: 0x0011BF59
	[Inject]
	public IUIComponentLocator uiComponentLocator { get; set; }

	// Token: 0x17000EED RID: 3821
	// (get) Token: 0x06003ED6 RID: 16086 RVA: 0x0011DB62 File Offset: 0x0011BF62
	// (set) Token: 0x06003ED7 RID: 16087 RVA: 0x0011DB6A File Offset: 0x0011BF6A
	[Inject]
	public ISelectionManager selectionManager { get; set; }

	// Token: 0x17000EEE RID: 3822
	// (get) Token: 0x06003ED8 RID: 16088 RVA: 0x0011DB73 File Offset: 0x0011BF73
	// (set) Token: 0x06003ED9 RID: 16089 RVA: 0x0011DB7B File Offset: 0x0011BF7B
	public bool ForbidNullSelection { get; set; }

	// Token: 0x17000EEF RID: 3823
	// (get) Token: 0x06003EDA RID: 16090 RVA: 0x0011DB84 File Offset: 0x0011BF84
	// (set) Token: 0x06003EDB RID: 16091 RVA: 0x0011DB8C File Offset: 0x0011BF8C
	public MenuItemButton LandingPoint { get; set; }

	// Token: 0x17000EF0 RID: 3824
	// (get) Token: 0x06003EDC RID: 16092 RVA: 0x0011DB95 File Offset: 0x0011BF95
	// (set) Token: 0x06003EDD RID: 16093 RVA: 0x0011DB9D File Offset: 0x0011BF9D
	public bool MouseOnly { get; set; }

	// Token: 0x06003EDE RID: 16094 RVA: 0x0011DBA8 File Offset: 0x0011BFA8
	public void Initialize()
	{
		for (int i = 0; i < this.buttonList.Count; i++)
		{
			MenuItemButton menuItemButton = this.buttonList[i];
			menuItemButton.transform.localScale = this._baseButtonScale;
			menuItemButton.Select = new Action<MenuItemButton, BaseEventData>(this.onSelect);
			menuItemButton.Deselect = new Action<MenuItemButton, BaseEventData>(this.onDeselect);
			menuItemButton.Submit = new Action<MenuItemButton, InputEventData>(this.onSubmit);
			menuItemButton.RightClick = new Action<MenuItemButton, InputEventData>(this.onRightClick);
			menuItemButton.Move = new Action<MenuItemButton, AxisEventData>(this.onMove);
			menuItemButton.PointerEnter = new Action<MenuItemButton, InputEventData>(this.onPointerEnter);
			menuItemButton.PointerExit = new Action<MenuItemButton, InputEventData>(this.onPointerExit);
		}
		this.syncNavigation();
		if (this.selectorImage != null)
		{
			this.selectorImage.SetActive(false);
		}
	}

	// Token: 0x06003EDF RID: 16095 RVA: 0x0011DC90 File Offset: 0x0011C090
	public void ClearButtons()
	{
		this.buttonList.Clear();
	}

	// Token: 0x06003EE0 RID: 16096 RVA: 0x0011DCA0 File Offset: 0x0011C0A0
	private void clearButtonListeners()
	{
		for (int i = 0; i < this.buttonList.Count; i++)
		{
			MenuItemButton menuItemButton = this.buttonList[i];
			MenuItemButton menuItemButton2 = menuItemButton;
			menuItemButton2.Select = (Action<MenuItemButton, BaseEventData>)Delegate.Remove(menuItemButton2.Select, new Action<MenuItemButton, BaseEventData>(this.onSelect));
			MenuItemButton menuItemButton3 = menuItemButton;
			menuItemButton3.Deselect = (Action<MenuItemButton, BaseEventData>)Delegate.Remove(menuItemButton3.Deselect, new Action<MenuItemButton, BaseEventData>(this.onDeselect));
			MenuItemButton menuItemButton4 = menuItemButton;
			menuItemButton4.Submit = (Action<MenuItemButton, InputEventData>)Delegate.Remove(menuItemButton4.Submit, new Action<MenuItemButton, InputEventData>(this.onSubmit));
			MenuItemButton menuItemButton5 = menuItemButton;
			menuItemButton5.RightClick = (Action<MenuItemButton, InputEventData>)Delegate.Remove(menuItemButton5.RightClick, new Action<MenuItemButton, InputEventData>(this.onRightClick));
			MenuItemButton menuItemButton6 = menuItemButton;
			menuItemButton6.Move = (Action<MenuItemButton, AxisEventData>)Delegate.Remove(menuItemButton6.Move, new Action<MenuItemButton, AxisEventData>(this.onMove));
			MenuItemButton menuItemButton7 = menuItemButton;
			menuItemButton7.PointerEnter = (Action<MenuItemButton, InputEventData>)Delegate.Remove(menuItemButton7.PointerEnter, new Action<MenuItemButton, InputEventData>(this.onPointerEnter));
			MenuItemButton menuItemButton8 = menuItemButton;
			menuItemButton8.PointerExit = (Action<MenuItemButton, InputEventData>)Delegate.Remove(menuItemButton8.PointerExit, new Action<MenuItemButton, InputEventData>(this.onPointerExit));
		}
	}

	// Token: 0x06003EE1 RID: 16097 RVA: 0x0011DDC4 File Offset: 0x0011C1C4
	public void Reload(List<MenuItemButton> list)
	{
		foreach (MenuItemButton item in list)
		{
			if (!this.buttonList.Contains(item))
			{
				throw new UnityException("This method is only for reloading already established buttons, you must first have added the button with AddButton");
			}
		}
		this.buttonList = list;
		this.syncNavigation();
	}

	// Token: 0x06003EE2 RID: 16098 RVA: 0x0011DE40 File Offset: 0x0011C240
	public void AddEdgeNavigation(MoveDirection direction, MenuItemList list)
	{
		this.edgeNavigation[direction] = list;
	}

	// Token: 0x06003EE3 RID: 16099 RVA: 0x0011DE4F File Offset: 0x0011C24F
	public void DisableGridWrap()
	{
		this.gridWrap = false;
	}

	// Token: 0x06003EE4 RID: 16100 RVA: 0x0011DE58 File Offset: 0x0011C258
	public void SetNavigationType(MenuItemList.NavigationType navType, int lineCount = 0)
	{
		switch (navType)
		{
		case MenuItemList.NavigationType.InOrderVertical:
			this._lineCount = 1;
			this._isHorizontalFill = true;
			break;
		case MenuItemList.NavigationType.InOrderHorizontal:
			this._lineCount = 1;
			this._isHorizontalFill = false;
			break;
		case MenuItemList.NavigationType.GridVerticalFill:
			this._lineCount = lineCount;
			this._isHorizontalFill = false;
			break;
		case MenuItemList.NavigationType.GridHorizontalFill:
			this._lineCount = lineCount;
			this._isHorizontalFill = true;
			break;
		}
	}

	// Token: 0x06003EE5 RID: 16101 RVA: 0x0011DECC File Offset: 0x0011C2CC
	private void syncGridIndex()
	{
		int lineCount = this._lineCount;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.buttonList.Count; i++)
		{
			MenuItemButton menuItemButton = this.buttonList[i];
			if (this._isHorizontalFill)
			{
				menuItemButton.GridLocationIndex = new Vector2((float)num, (float)num2);
			}
			else
			{
				menuItemButton.GridLocationIndex = new Vector2((float)num2, (float)num);
			}
			num++;
			if (num >= lineCount)
			{
				num = 0;
				num2++;
			}
		}
	}

	// Token: 0x06003EE6 RID: 16102 RVA: 0x0011DF4F File Offset: 0x0011C34F
	private void syncNavigation()
	{
		this.syncGridIndex();
	}

	// Token: 0x06003EE7 RID: 16103 RVA: 0x0011DF58 File Offset: 0x0011C358
	private int getCurrentButtonIndex()
	{
		for (int i = this.buttonList.Count - 1; i >= 0; i--)
		{
			if (this.buttonList[i] == this._currentSelection)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06003EE8 RID: 16104 RVA: 0x0011DFA4 File Offset: 0x0011C3A4
	private Button gridFindLink(int index, MoveDirection direction)
	{
		if (index >= this.buttonList.Count)
		{
			throw new UnityException(string.Concat(new object[]
			{
				"Invalid index ",
				index,
				" would cause infinite loop. List size is ",
				this.buttonList.Count
			}));
		}
		MenuItemButton menuItemButton = this.buttonList[index];
		Vector2 gridLocationIndex = menuItemButton.GridLocationIndex;
		bool flag = direction == MoveDirection.Left || direction == MoveDirection.Up;
		bool flag2 = direction == MoveDirection.Down || direction == MoveDirection.Up;
		int num = index;
		MenuItemButton menuItemButton2;
		for (;;)
		{
			num += ((!flag) ? 1 : -1);
			if (num < 0)
			{
				num = this.buttonList.Count - 1;
			}
			else if (num > this.buttonList.Count - 1)
			{
				num = 0;
			}
			menuItemButton2 = this.buttonList[num];
			if (menuItemButton != menuItemButton2)
			{
				if (this.isSearchBeyondEdge(direction, gridLocationIndex, menuItemButton2))
				{
					if (this.hasEdgeNavigation(direction))
					{
						break;
					}
					if (!this.gridWrap)
					{
						goto Block_10;
					}
				}
				if (this.isValidButton(menuItemButton2))
				{
					if (flag2)
					{
						if (menuItemButton2.GridLocationIndex.x == gridLocationIndex.x)
						{
							goto Block_13;
						}
					}
					else if (menuItemButton2.GridLocationIndex.y == gridLocationIndex.y)
					{
						goto Block_14;
					}
				}
			}
			if (num == index)
			{
				goto Block_15;
			}
		}
		return this.getEdgeNavigation(direction);
		Block_10:
		return null;
		Block_13:
		return menuItemButton2.InteractableButton;
		Block_14:
		return menuItemButton2.InteractableButton;
		Block_15:
		if (this.hasEdgeNavigation(direction))
		{
			return this.getEdgeNavigation(direction);
		}
		return null;
	}

	// Token: 0x06003EE9 RID: 16105 RVA: 0x0011E144 File Offset: 0x0011C544
	private bool isSearchBeyondEdge(MoveDirection direction, Vector2 sourcePosition, MenuItemButton target)
	{
		switch (direction)
		{
		case MoveDirection.Left:
			return target.GridLocationIndex.x > sourcePosition.x;
		case MoveDirection.Up:
			return target.GridLocationIndex.y > sourcePosition.y;
		case MoveDirection.Right:
			return target.GridLocationIndex.x < sourcePosition.x;
		case MoveDirection.Down:
			return target.GridLocationIndex.y < sourcePosition.y;
		default:
			return false;
		}
	}

	// Token: 0x06003EEA RID: 16106 RVA: 0x0011E1CD File Offset: 0x0011C5CD
	private bool hasEdgeNavigation(MoveDirection direction)
	{
		return this.edgeNavigation.ContainsKey(direction);
	}

	// Token: 0x06003EEB RID: 16107 RVA: 0x0011E1DC File Offset: 0x0011C5DC
	private Button getEdgeNavigation(MoveDirection direction)
	{
		if (this.edgeNavigation.ContainsKey(direction) && this.edgeNavigation[direction].LandingPoint != null && this.isValidButton(this.edgeNavigation[direction].LandingPoint))
		{
			return this.edgeNavigation[direction].LandingPoint.InteractableButton;
		}
		return null;
	}

	// Token: 0x06003EEC RID: 16108 RVA: 0x0011E24A File Offset: 0x0011C64A
	private bool isValidButton(MenuItemButton button)
	{
		return button.ButtonEnabled;
	}

	// Token: 0x06003EED RID: 16109 RVA: 0x0011E254 File Offset: 0x0011C654
	public void AutoSelect(MenuItemButton firstSelected)
	{
		if (firstSelected != null)
		{
			firstSelected.InteractableButton.Select();
			this.updateSelection(firstSelected);
			if (!this.MouseOnly)
			{
				this.selectionManager.Select(firstSelected.InteractableButton.gameObject);
			}
		}
	}

	// Token: 0x06003EEE RID: 16110 RVA: 0x0011E2A0 File Offset: 0x0011C6A0
	public void RemoveSelection()
	{
		this.updateSelection(null);
		if (!this.MouseOnly && this.isThisMenuSelected())
		{
			this.selectionManager.Select(null);
		}
	}

	// Token: 0x06003EEF RID: 16111 RVA: 0x0011E2CC File Offset: 0x0011C6CC
	private bool isThisMenuSelected()
	{
		foreach (MenuItemButton menuItemButton in this.buttonList)
		{
			if (menuItemButton.InteractableButton.gameObject == EventSystem.current.currentSelectedGameObject)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003EF0 RID: 16112 RVA: 0x0011E34C File Offset: 0x0011C74C
	public void Disable()
	{
		this.updateSelection(null);
	}

	// Token: 0x06003EF1 RID: 16113 RVA: 0x0011E355 File Offset: 0x0011C755
	public void Lock()
	{
		this.isLocked = true;
	}

	// Token: 0x06003EF2 RID: 16114 RVA: 0x0011E35E File Offset: 0x0011C75E
	public void Unlock()
	{
		this.isLocked = false;
	}

	// Token: 0x06003EF3 RID: 16115 RVA: 0x0011E367 File Offset: 0x0011C767
	public void SetSelectorImage(GameObject image)
	{
		this.selectorImage = image;
	}

	// Token: 0x06003EF4 RID: 16116 RVA: 0x0011E370 File Offset: 0x0011C770
	public void SetSelectorSpriteSwaps(Sprite selectedSprite, Sprite defaultSprite)
	{
		this.selectedSprite = selectedSprite;
		this.defaultSprite = defaultSprite;
	}

	// Token: 0x06003EF5 RID: 16117 RVA: 0x0011E380 File Offset: 0x0011C780
	public void AddButton(MenuItemButton button, Action callback)
	{
		this.buttonList.Add(button);
		this.buttonHandlers[button] = callback;
		if (this.MouseOnly)
		{
			button.InteractableButton.Unselectable = true;
		}
	}

	// Token: 0x06003EF6 RID: 16118 RVA: 0x0011E3B2 File Offset: 0x0011C7B2
	public void AddButton(MenuItemButton button, Action<InputEventData> callback)
	{
		this.buttonList.Add(button);
		this.buttonDetailedHandlers[button] = callback;
		if (this.MouseOnly)
		{
			button.InteractableButton.Unselectable = true;
		}
	}

	// Token: 0x06003EF7 RID: 16119 RVA: 0x0011E3E4 File Offset: 0x0011C7E4
	public void RemoveButton(MenuItemButton button)
	{
		this.buttonList.Remove(button);
	}

	// Token: 0x06003EF8 RID: 16120 RVA: 0x0011E3F3 File Offset: 0x0011C7F3
	public void AddRightClick(MenuItemButton button, Action<InputEventData> callback)
	{
		this.rightClickHandlers[button] = callback;
	}

	// Token: 0x06003EF9 RID: 16121 RVA: 0x0011E402 File Offset: 0x0011C802
	public MenuItemButton[] GetButtons()
	{
		return this.buttonList.ToArray();
	}

	// Token: 0x06003EFA RID: 16122 RVA: 0x0011E410 File Offset: 0x0011C810
	public void SetButtonEnabled(MenuItemButton button, bool isEnabled)
	{
		if (button == null && !isEnabled)
		{
			return;
		}
		button.SetInteractable(isEnabled);
		button.ButtonEnabled = isEnabled;
		this.syncNavigation();
		if (!isEnabled && this.LandingPoint == button)
		{
			this.findNewLandingPoint();
		}
	}

	// Token: 0x06003EFB RID: 16123 RVA: 0x0011E464 File Offset: 0x0011C864
	private void findNewLandingPoint()
	{
		foreach (MenuItemButton menuItemButton in this.buttonList)
		{
			if (menuItemButton.ButtonEnabled)
			{
				this.LandingPoint = menuItemButton;
				break;
			}
		}
	}

	// Token: 0x17000EF1 RID: 3825
	// (get) Token: 0x06003EFC RID: 16124 RVA: 0x0011E4D0 File Offset: 0x0011C8D0
	public MenuItemButton CurrentSelection
	{
		get
		{
			return this._currentSelection;
		}
	}

	// Token: 0x17000EF2 RID: 3826
	// (get) Token: 0x06003EFD RID: 16125 RVA: 0x0011E4D8 File Offset: 0x0011C8D8
	public MenuItemButton PreviousSelection
	{
		get
		{
			return this._prevSelection;
		}
	}

	// Token: 0x06003EFE RID: 16126 RVA: 0x0011E4E0 File Offset: 0x0011C8E0
	private bool acceptInput(MenuItemButton theButton)
	{
		return theButton.InteractableButton.interactable;
	}

	// Token: 0x06003EFF RID: 16127 RVA: 0x0011E4F0 File Offset: 0x0011C8F0
	private void onMove(MenuItemButton button, AxisEventData eventData)
	{
		if (this.isLocked)
		{
			return;
		}
		int currentButtonIndex = this.getCurrentButtonIndex();
		if (currentButtonIndex != -1)
		{
			Button button2 = this.gridFindLink(currentButtonIndex, eventData.moveDir);
			if (button2 != null)
			{
				button2.Select();
			}
		}
	}

	// Token: 0x06003F00 RID: 16128 RVA: 0x0011E537 File Offset: 0x0011C937
	private void onRightClick(MenuItemButton theButton, InputEventData data)
	{
		if (!this.acceptInput(theButton))
		{
			return;
		}
		if (this.rightClickHandlers.ContainsKey(theButton))
		{
			this.rightClickHandlers[theButton](data);
		}
	}

	// Token: 0x06003F01 RID: 16129 RVA: 0x0011E56C File Offset: 0x0011C96C
	private void onSubmit(MenuItemButton clickedButton, InputEventData data)
	{
		if (!this.acceptInput(clickedButton))
		{
			return;
		}
		MenuItemButton menuItemButton = this._currentSelection;
		if (menuItemButton == null)
		{
			menuItemButton = clickedButton;
		}
		if (this.buttonHandlers.ContainsKey(menuItemButton))
		{
			this.buttonHandlers[menuItemButton]();
		}
		if (this.buttonDetailedHandlers.ContainsKey(menuItemButton))
		{
			this.buttonDetailedHandlers[menuItemButton](data);
		}
	}

	// Token: 0x06003F02 RID: 16130 RVA: 0x0011E5E0 File Offset: 0x0011C9E0
	private void onPointerEnter(MenuItemButton theButton, InputEventData data)
	{
		if (!this.acceptInput(theButton))
		{
			return;
		}
		if (this.MouseOnly)
		{
			this.updateSelection(theButton);
		}
	}

	// Token: 0x06003F03 RID: 16131 RVA: 0x0011E601 File Offset: 0x0011CA01
	private void onPointerExit(MenuItemButton theButton, InputEventData data)
	{
		if (!this.acceptInput(theButton))
		{
			return;
		}
		if (this.MouseOnly && theButton == this._currentSelection && !this.ForbidNullSelection)
		{
			this.updateSelection(null);
		}
	}

	// Token: 0x06003F04 RID: 16132 RVA: 0x0011E63E File Offset: 0x0011CA3E
	private void onSelect(MenuItemButton theButton, BaseEventData eventData)
	{
		if (!this.acceptInput(theButton))
		{
			return;
		}
		if (!(eventData is PointerEventData))
		{
			this.LandingPoint = theButton;
		}
		this.updateSelection(theButton);
		if (this.OnSelected != null)
		{
			this.OnSelected(theButton, eventData);
		}
	}

	// Token: 0x06003F05 RID: 16133 RVA: 0x0011E680 File Offset: 0x0011CA80
	private void onDeselect(MenuItemButton theButton, BaseEventData eventData)
	{
		if (!this.acceptInput(theButton))
		{
			return;
		}
		if (theButton == this._currentSelection && !this.ForbidNullSelection)
		{
			this.updateSelection(null);
		}
		if (this.OnDeselected != null)
		{
			this.OnDeselected(theButton, eventData);
		}
	}

	// Token: 0x06003F06 RID: 16134 RVA: 0x0011E6D8 File Offset: 0x0011CAD8
	private void updateSelection(MenuItemButton theButton)
	{
		if (this.isLocked)
		{
			return;
		}
		if (this._currentSelection != theButton)
		{
			this._prevSelection = this._currentSelection;
			if (this._currentSelection != null && !this.isFrozen(this._currentSelection))
			{
				this.scaleDownButton(this._currentSelection);
				this.setDefaultColor(this._currentSelection);
				this.setDefaultSprite(this._currentSelection);
				this.hideOverlayImage(this._currentSelection);
				if (this._currentSelection.HighlightComponent != null)
				{
					this._currentSelection.HighlightComponent.SetHighlightMode(false);
				}
			}
			this._currentSelection = theButton;
			if (this._currentSelection != null && !this.isFrozen(this._currentSelection))
			{
				this.scaleUpSelected(this._currentSelection);
				this.setHighlightColor(this._currentSelection);
				this.setSelectedSprite(this._currentSelection);
				this.showOverlayImage(this._currentSelection);
				if (this._currentSelection.HighlightComponent != null)
				{
					this._currentSelection.HighlightComponent.SetHighlightMode(true);
				}
			}
			this.updateSelectorImage();
		}
	}

	// Token: 0x06003F07 RID: 16135 RVA: 0x0011E801 File Offset: 0x0011CC01
	private bool isFrozen(MenuItemButton theButton)
	{
		return theButton.IsFrozen;
	}

	// Token: 0x06003F08 RID: 16136 RVA: 0x0011E80C File Offset: 0x0011CC0C
	private void setDefaultSprite(MenuItemButton theButton)
	{
		if (this.defaultSprite != null)
		{
			theButton.GetComponent<Image>().sprite = this.defaultSprite;
		}
		if (theButton.SwapImage != null)
		{
			theButton.SwapImage.overrideSprite = null;
		}
	}

	// Token: 0x06003F09 RID: 16137 RVA: 0x0011E858 File Offset: 0x0011CC58
	private void setSelectedSprite(MenuItemButton theButton)
	{
		if (this.selectedSprite != null)
		{
			theButton.GetComponent<Image>().sprite = this.selectedSprite;
		}
		if (theButton.SwapImage != null)
		{
			theButton.SwapImage.overrideSprite = theButton.SwapSprite;
		}
	}

	// Token: 0x06003F0A RID: 16138 RVA: 0x0011E8A9 File Offset: 0x0011CCA9
	private void setDefaultColor(MenuItemButton theButton)
	{
		if (theButton.UseTextColorHighlight && theButton.OriginalTextColor != WColor.NullColor)
		{
			theButton.TextField.color = theButton.OriginalTextColor;
		}
	}

	// Token: 0x06003F0B RID: 16139 RVA: 0x0011E8DC File Offset: 0x0011CCDC
	private void hideOverlayImage(MenuItemButton theButton)
	{
		if (theButton.OverlayImage != null)
		{
			theButton.OverlayImage.gameObject.SetActive(false);
		}
	}

	// Token: 0x06003F0C RID: 16140 RVA: 0x0011E900 File Offset: 0x0011CD00
	private void scaleUpSelected(MenuItemButton theButton)
	{
		if (theButton.ScaleTarget != null)
		{
			Transform target = theButton.ScaleTarget.transform;
			this.killButtonScaleTween(theButton);
			Vector3 endValue = new Vector3(theButton.ScaleUpSize, theButton.ScaleUpSize, theButton.ScaleUpSize);
			this._buttonScaleTweens[theButton] = DOTween.To(() => target.localScale, delegate(Vector3 x)
			{
				target.localScale = x;
			}, endValue, theButton.ScaleUpTime).SetEase(Ease.Linear).OnComplete(delegate
			{
				this.killButtonScaleTween(theButton);
			});
		}
	}

	// Token: 0x06003F0D RID: 16141 RVA: 0x0011E9E0 File Offset: 0x0011CDE0
	private void scaleDownButton(MenuItemButton theButton)
	{
		if (theButton.ScaleTarget != null)
		{
			Transform target = theButton.ScaleTarget.transform;
			this.killButtonScaleTween(theButton);
			this._buttonScaleTweens[theButton] = DOTween.To(() => target.localScale, delegate(Vector3 x)
			{
				target.localScale = x;
			}, this._baseButtonScale, theButton.ScaleDownTime).SetEase(Ease.Linear).OnComplete(delegate
			{
				this.killButtonScaleTween(theButton);
			});
		}
	}

	// Token: 0x06003F0E RID: 16142 RVA: 0x0011EA9C File Offset: 0x0011CE9C
	private void setHighlightColor(MenuItemButton theButton)
	{
		if (theButton.UseTextColorHighlight && theButton.TextColorHighlight != WColor.NullColor)
		{
			if (theButton.TextField.color != theButton.TextColorHighlight)
			{
				theButton.OriginalTextColor = theButton.TextField.color;
			}
			theButton.TextField.color = theButton.TextColorHighlight;
		}
	}

	// Token: 0x06003F0F RID: 16143 RVA: 0x0011EB06 File Offset: 0x0011CF06
	private void showOverlayImage(MenuItemButton theButton)
	{
		if (theButton.OverlayImage != null)
		{
			theButton.OverlayImage.gameObject.SetActive(true);
		}
	}

	// Token: 0x06003F10 RID: 16144 RVA: 0x0011EB2C File Offset: 0x0011CF2C
	private void killButtonScaleTween(MenuItemButton theButton)
	{
		if (this._buttonScaleTweens.ContainsKey(theButton))
		{
			if (this._buttonScaleTweens[theButton].IsPlaying())
			{
				this._buttonScaleTweens[theButton].Kill(false);
			}
			this._buttonScaleTweens.Remove(theButton);
		}
	}

	// Token: 0x06003F11 RID: 16145 RVA: 0x0011EB80 File Offset: 0x0011CF80
	private void killAllTweens()
	{
		List<MenuItemButton> list = new List<MenuItemButton>();
		foreach (MenuItemButton item in this._buttonScaleTweens.Keys)
		{
			list.Add(item);
		}
		foreach (MenuItemButton theButton in list)
		{
			this.killButtonScaleTween(theButton);
		}
	}

	// Token: 0x06003F12 RID: 16146 RVA: 0x0011EC30 File Offset: 0x0011D030
	private void updateSelectorImage()
	{
		if (this.selectorImage != null)
		{
			if (this._currentSelection == null)
			{
				this.selectorImage.SetActive(false);
			}
			else
			{
				this.selectorImage.SetActive(true);
				if (this.OnSelectorImageChanged != null)
				{
					this.OnSelectorImageChanged(this._currentSelection);
				}
				Vector3 position = this._currentSelection.transform.position;
				this.selectorImage.transform.position = position;
			}
		}
	}

	// Token: 0x06003F13 RID: 16147 RVA: 0x0011ECBA File Offset: 0x0011D0BA
	public void OnDestroy()
	{
		this.killAllTweens();
		this.OnSelected = null;
		this.buttonHandlers.Clear();
		this.buttonDetailedHandlers.Clear();
		this.rightClickHandlers.Clear();
		this.clearButtonListeners();
	}

	// Token: 0x04002AA2 RID: 10914
	private GameObject selectorImage;

	// Token: 0x04002AA3 RID: 10915
	private Sprite selectedSprite;

	// Token: 0x04002AA4 RID: 10916
	private Sprite defaultSprite;

	// Token: 0x04002AA5 RID: 10917
	private List<MenuItemButton> buttonList = new List<MenuItemButton>();

	// Token: 0x04002AA6 RID: 10918
	private Dictionary<MenuItemButton, Tweener> _buttonScaleTweens = new Dictionary<MenuItemButton, Tweener>();

	// Token: 0x04002AA7 RID: 10919
	private Vector3 _baseButtonScale = new Vector3(1f, 1f, 1f);

	// Token: 0x04002AA8 RID: 10920
	private MenuItemButton _currentSelection;

	// Token: 0x04002AA9 RID: 10921
	private MenuItemButton _prevSelection;

	// Token: 0x04002AAA RID: 10922
	private int _lineCount = 1;

	// Token: 0x04002AAB RID: 10923
	private bool _isHorizontalFill = true;

	// Token: 0x04002AAC RID: 10924
	private bool isLocked;

	// Token: 0x04002AAD RID: 10925
	private Dictionary<MenuItemButton, Action> buttonHandlers = new Dictionary<MenuItemButton, Action>();

	// Token: 0x04002AAE RID: 10926
	private Dictionary<MenuItemButton, Action<InputEventData>> buttonDetailedHandlers = new Dictionary<MenuItemButton, Action<InputEventData>>();

	// Token: 0x04002AAF RID: 10927
	private Dictionary<MenuItemButton, Action<InputEventData>> rightClickHandlers = new Dictionary<MenuItemButton, Action<InputEventData>>();

	// Token: 0x04002AB0 RID: 10928
	public Action<MenuItemButton, BaseEventData> OnSelected;

	// Token: 0x04002AB1 RID: 10929
	public Action<MenuItemButton, BaseEventData> OnDeselected;

	// Token: 0x04002AB2 RID: 10930
	public Action<MenuItemButton> OnSelectorImageChanged;

	// Token: 0x04002AB6 RID: 10934
	private bool gridWrap = true;

	// Token: 0x04002AB7 RID: 10935
	private Dictionary<MoveDirection, MenuItemList> edgeNavigation = new Dictionary<MoveDirection, MenuItemList>();

	// Token: 0x02000948 RID: 2376
	public enum NavigationType
	{
		// Token: 0x04002AB9 RID: 10937
		InOrderVertical,
		// Token: 0x04002ABA RID: 10938
		InOrderHorizontal,
		// Token: 0x04002ABB RID: 10939
		GridVerticalFill,
		// Token: 0x04002ABC RID: 10940
		GridHorizontalFill
	}
}
