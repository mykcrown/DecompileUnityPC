// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuItemList
{
	public enum NavigationType
	{
		InOrderVertical,
		InOrderHorizontal,
		GridVerticalFill,
		GridHorizontalFill
	}

	private sealed class _scaleUpSelected_c__AnonStorey1
	{
		internal MenuItemButton theButton;

		internal MenuItemList _this;
	}

	private sealed class _scaleUpSelected_c__AnonStorey0
	{
		internal Transform target;

		internal MenuItemList._scaleUpSelected_c__AnonStorey1 __f__ref_1;

		internal Vector3 __m__0()
		{
			return this.target.localScale;
		}

		internal void __m__1(Vector3 x)
		{
			this.target.localScale = x;
		}

		internal void __m__2()
		{
			this.__f__ref_1._this.killButtonScaleTween(this.__f__ref_1.theButton);
		}
	}

	private sealed class _scaleDownButton_c__AnonStorey3
	{
		internal MenuItemButton theButton;

		internal MenuItemList _this;
	}

	private sealed class _scaleDownButton_c__AnonStorey2
	{
		internal Transform target;

		internal MenuItemList._scaleDownButton_c__AnonStorey3 __f__ref_3;

		internal Vector3 __m__0()
		{
			return this.target.localScale;
		}

		internal void __m__1(Vector3 x)
		{
			this.target.localScale = x;
		}

		internal void __m__2()
		{
			this.__f__ref_3._this.killButtonScaleTween(this.__f__ref_3.theButton);
		}
	}

	private GameObject selectorImage;

	private Sprite selectedSprite;

	private Sprite defaultSprite;

	private List<MenuItemButton> buttonList = new List<MenuItemButton>();

	private Dictionary<MenuItemButton, Tweener> _buttonScaleTweens = new Dictionary<MenuItemButton, Tweener>();

	private Vector3 _baseButtonScale = new Vector3(1f, 1f, 1f);

	private MenuItemButton _currentSelection;

	private MenuItemButton _prevSelection;

	private int _lineCount = 1;

	private bool _isHorizontalFill = true;

	private bool isLocked;

	private Dictionary<MenuItemButton, Action> buttonHandlers = new Dictionary<MenuItemButton, Action>();

	private Dictionary<MenuItemButton, Action<InputEventData>> buttonDetailedHandlers = new Dictionary<MenuItemButton, Action<InputEventData>>();

	private Dictionary<MenuItemButton, Action<InputEventData>> rightClickHandlers = new Dictionary<MenuItemButton, Action<InputEventData>>();

	public Action<MenuItemButton, BaseEventData> OnSelected;

	public Action<MenuItemButton, BaseEventData> OnDeselected;

	public Action<MenuItemButton> OnSelectorImageChanged;

	private bool gridWrap = true;

	private Dictionary<MoveDirection, MenuItemList> edgeNavigation = new Dictionary<MoveDirection, MenuItemList>();

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public IUIComponentLocator uiComponentLocator
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

	public bool ForbidNullSelection
	{
		get;
		set;
	}

	public MenuItemButton LandingPoint
	{
		get;
		set;
	}

	public bool MouseOnly
	{
		get;
		set;
	}

	public MenuItemButton CurrentSelection
	{
		get
		{
			return this._currentSelection;
		}
	}

	public MenuItemButton PreviousSelection
	{
		get
		{
			return this._prevSelection;
		}
	}

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

	public void ClearButtons()
	{
		this.buttonList.Clear();
	}

	private void clearButtonListeners()
	{
		for (int i = 0; i < this.buttonList.Count; i++)
		{
			MenuItemButton menuItemButton = this.buttonList[i];
			MenuItemButton expr_15 = menuItemButton;
			expr_15.Select = (Action<MenuItemButton, BaseEventData>)Delegate.Remove(expr_15.Select, new Action<MenuItemButton, BaseEventData>(this.onSelect));
			MenuItemButton expr_37 = menuItemButton;
			expr_37.Deselect = (Action<MenuItemButton, BaseEventData>)Delegate.Remove(expr_37.Deselect, new Action<MenuItemButton, BaseEventData>(this.onDeselect));
			MenuItemButton expr_59 = menuItemButton;
			expr_59.Submit = (Action<MenuItemButton, InputEventData>)Delegate.Remove(expr_59.Submit, new Action<MenuItemButton, InputEventData>(this.onSubmit));
			MenuItemButton expr_7B = menuItemButton;
			expr_7B.RightClick = (Action<MenuItemButton, InputEventData>)Delegate.Remove(expr_7B.RightClick, new Action<MenuItemButton, InputEventData>(this.onRightClick));
			MenuItemButton expr_9D = menuItemButton;
			expr_9D.Move = (Action<MenuItemButton, AxisEventData>)Delegate.Remove(expr_9D.Move, new Action<MenuItemButton, AxisEventData>(this.onMove));
			MenuItemButton expr_BF = menuItemButton;
			expr_BF.PointerEnter = (Action<MenuItemButton, InputEventData>)Delegate.Remove(expr_BF.PointerEnter, new Action<MenuItemButton, InputEventData>(this.onPointerEnter));
			MenuItemButton expr_E1 = menuItemButton;
			expr_E1.PointerExit = (Action<MenuItemButton, InputEventData>)Delegate.Remove(expr_E1.PointerExit, new Action<MenuItemButton, InputEventData>(this.onPointerExit));
		}
	}

	public void Reload(List<MenuItemButton> list)
	{
		foreach (MenuItemButton current in list)
		{
			if (!this.buttonList.Contains(current))
			{
				throw new UnityException("This method is only for reloading already established buttons, you must first have added the button with AddButton");
			}
		}
		this.buttonList = list;
		this.syncNavigation();
	}

	public void AddEdgeNavigation(MoveDirection direction, MenuItemList list)
	{
		this.edgeNavigation[direction] = list;
	}

	public void DisableGridWrap()
	{
		this.gridWrap = false;
	}

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

	private void syncNavigation()
	{
		this.syncGridIndex();
	}

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
		while (true)
		{
			num += ((!flag) ? 1 : (-1));
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

	private bool hasEdgeNavigation(MoveDirection direction)
	{
		return this.edgeNavigation.ContainsKey(direction);
	}

	private Button getEdgeNavigation(MoveDirection direction)
	{
		if (this.edgeNavigation.ContainsKey(direction) && this.edgeNavigation[direction].LandingPoint != null && this.isValidButton(this.edgeNavigation[direction].LandingPoint))
		{
			return this.edgeNavigation[direction].LandingPoint.InteractableButton;
		}
		return null;
	}

	private bool isValidButton(MenuItemButton button)
	{
		return button.ButtonEnabled;
	}

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

	public void RemoveSelection()
	{
		this.updateSelection(null);
		if (!this.MouseOnly && this.isThisMenuSelected())
		{
			this.selectionManager.Select(null);
		}
	}

	private bool isThisMenuSelected()
	{
		foreach (MenuItemButton current in this.buttonList)
		{
			if (current.InteractableButton.gameObject == EventSystem.current.currentSelectedGameObject)
			{
				return true;
			}
		}
		return false;
	}

	public void Disable()
	{
		this.updateSelection(null);
	}

	public void Lock()
	{
		this.isLocked = true;
	}

	public void Unlock()
	{
		this.isLocked = false;
	}

	public void SetSelectorImage(GameObject image)
	{
		this.selectorImage = image;
	}

	public void SetSelectorSpriteSwaps(Sprite selectedSprite, Sprite defaultSprite)
	{
		this.selectedSprite = selectedSprite;
		this.defaultSprite = defaultSprite;
	}

	public void AddButton(MenuItemButton button, Action callback)
	{
		this.buttonList.Add(button);
		this.buttonHandlers[button] = callback;
		if (this.MouseOnly)
		{
			button.InteractableButton.Unselectable = true;
		}
	}

	public void AddButton(MenuItemButton button, Action<InputEventData> callback)
	{
		this.buttonList.Add(button);
		this.buttonDetailedHandlers[button] = callback;
		if (this.MouseOnly)
		{
			button.InteractableButton.Unselectable = true;
		}
	}

	public void RemoveButton(MenuItemButton button)
	{
		this.buttonList.Remove(button);
	}

	public void AddRightClick(MenuItemButton button, Action<InputEventData> callback)
	{
		this.rightClickHandlers[button] = callback;
	}

	public MenuItemButton[] GetButtons()
	{
		return this.buttonList.ToArray();
	}

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

	private void findNewLandingPoint()
	{
		foreach (MenuItemButton current in this.buttonList)
		{
			if (current.ButtonEnabled)
			{
				this.LandingPoint = current;
				break;
			}
		}
	}

	private bool acceptInput(MenuItemButton theButton)
	{
		return theButton.InteractableButton.interactable;
	}

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

	private bool isFrozen(MenuItemButton theButton)
	{
		return theButton.IsFrozen;
	}

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

	private void setDefaultColor(MenuItemButton theButton)
	{
		if (theButton.UseTextColorHighlight && theButton.OriginalTextColor != WColor.NullColor)
		{
			theButton.TextField.color = theButton.OriginalTextColor;
		}
	}

	private void hideOverlayImage(MenuItemButton theButton)
	{
		if (theButton.OverlayImage != null)
		{
			theButton.OverlayImage.gameObject.SetActive(false);
		}
	}

	private void scaleUpSelected(MenuItemButton theButton)
	{
		MenuItemList._scaleUpSelected_c__AnonStorey1 _scaleUpSelected_c__AnonStorey = new MenuItemList._scaleUpSelected_c__AnonStorey1();
		_scaleUpSelected_c__AnonStorey.theButton = theButton;
		_scaleUpSelected_c__AnonStorey._this = this;
		if (_scaleUpSelected_c__AnonStorey.theButton.ScaleTarget != null)
		{
			MenuItemList._scaleUpSelected_c__AnonStorey0 _scaleUpSelected_c__AnonStorey2 = new MenuItemList._scaleUpSelected_c__AnonStorey0();
			_scaleUpSelected_c__AnonStorey2.__f__ref_1 = _scaleUpSelected_c__AnonStorey;
			_scaleUpSelected_c__AnonStorey2.target = _scaleUpSelected_c__AnonStorey.theButton.ScaleTarget.transform;
			this.killButtonScaleTween(_scaleUpSelected_c__AnonStorey.theButton);
			Vector3 endValue = new Vector3(_scaleUpSelected_c__AnonStorey.theButton.ScaleUpSize, _scaleUpSelected_c__AnonStorey.theButton.ScaleUpSize, _scaleUpSelected_c__AnonStorey.theButton.ScaleUpSize);
			this._buttonScaleTweens[_scaleUpSelected_c__AnonStorey.theButton] = DOTween.To(new DOGetter<Vector3>(_scaleUpSelected_c__AnonStorey2.__m__0), new DOSetter<Vector3>(_scaleUpSelected_c__AnonStorey2.__m__1), endValue, _scaleUpSelected_c__AnonStorey.theButton.ScaleUpTime).SetEase(Ease.Linear).OnComplete(new TweenCallback(_scaleUpSelected_c__AnonStorey2.__m__2));
		}
	}

	private void scaleDownButton(MenuItemButton theButton)
	{
		MenuItemList._scaleDownButton_c__AnonStorey3 _scaleDownButton_c__AnonStorey = new MenuItemList._scaleDownButton_c__AnonStorey3();
		_scaleDownButton_c__AnonStorey.theButton = theButton;
		_scaleDownButton_c__AnonStorey._this = this;
		if (_scaleDownButton_c__AnonStorey.theButton.ScaleTarget != null)
		{
			MenuItemList._scaleDownButton_c__AnonStorey2 _scaleDownButton_c__AnonStorey2 = new MenuItemList._scaleDownButton_c__AnonStorey2();
			_scaleDownButton_c__AnonStorey2.__f__ref_3 = _scaleDownButton_c__AnonStorey;
			_scaleDownButton_c__AnonStorey2.target = _scaleDownButton_c__AnonStorey.theButton.ScaleTarget.transform;
			this.killButtonScaleTween(_scaleDownButton_c__AnonStorey.theButton);
			this._buttonScaleTweens[_scaleDownButton_c__AnonStorey.theButton] = DOTween.To(new DOGetter<Vector3>(_scaleDownButton_c__AnonStorey2.__m__0), new DOSetter<Vector3>(_scaleDownButton_c__AnonStorey2.__m__1), this._baseButtonScale, _scaleDownButton_c__AnonStorey.theButton.ScaleDownTime).SetEase(Ease.Linear).OnComplete(new TweenCallback(_scaleDownButton_c__AnonStorey2.__m__2));
		}
	}

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

	private void showOverlayImage(MenuItemButton theButton)
	{
		if (theButton.OverlayImage != null)
		{
			theButton.OverlayImage.gameObject.SetActive(true);
		}
	}

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

	private void killAllTweens()
	{
		List<MenuItemButton> list = new List<MenuItemButton>();
		foreach (MenuItemButton current in this._buttonScaleTweens.Keys)
		{
			list.Add(current);
		}
		foreach (MenuItemButton current2 in list)
		{
			this.killButtonScaleTween(current2);
		}
	}

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

	public void OnDestroy()
	{
		this.killAllTweens();
		this.OnSelected = null;
		this.buttonHandlers.Clear();
		this.buttonDetailedHandlers.Clear();
		this.rightClickHandlers.Clear();
		this.clearButtonListeners();
	}
}
