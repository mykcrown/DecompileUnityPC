// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreIntroPanelView : MonoBehaviour
{
	private sealed class _setupButton_c__AnonStorey0
	{
		internal int itemIndex;

		internal StoreIntroPanel newElement;

		internal Transform parent;

		internal StoreIntroPanelView _this;

		internal void __m__0()
		{
			if (this._this.OnItemClicked != null)
			{
				this._this.OnItemClicked(this.itemIndex);
			}
		}

		internal void __m__1(BaseEventData eventData)
		{
			this.newElement.transform.SetAsLastSibling();
			this.parent.SetAsLastSibling();
			if (!(this._this.uiManager.CurrentInputModule as UIInputModule).IsMouseMode)
			{
				this._this.mostRecentButtonModeSelection = this.newElement.Button;
			}
		}
	}

	public StoreIntroPanel ElementPrefab;

	public List<HorizontalLayoutGroup> LayoutGroups;

	public Action<int> OnItemClicked;

	private Func<StoreIntroPanel> findPanelForCurrentSelection;

	private MenuItemList buttonList;

	private MenuItemButton mostRecentButtonModeSelection;

	private bool readyToSyncNavigation;

	private bool syncNavigationWhenReady;

	private MenuItemButton deferedSyncButton;

	[Inject]
	public IDependencyInjection injector
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
	public IWindowDisplay windowDisplay
	{
		get;
		set;
	}

	public List<StoreIntroPanel> elements
	{
		get;
		private set;
	}

	public void Initialize(int panelCount, Func<StoreIntroPanel> findPanelForCurrentSelection)
	{
		this.findPanelForCurrentSelection = findPanelForCurrentSelection;
		this.buttonList = this.injector.GetInstance<MenuItemList>();
		this.elements = new List<StoreIntroPanel>();
		for (int i = 0; i < panelCount; i++)
		{
			int itemIndex = i;
			int index = 0;
			if (panelCount > 3)
			{
				index = i * this.LayoutGroups.Count / panelCount;
			}
			Transform transform = this.LayoutGroups[index].transform;
			StoreIntroPanel storeIntroPanel = UnityEngine.Object.Instantiate<StoreIntroPanel>(this.ElementPrefab, transform);
			this.setupButton(storeIntroPanel, itemIndex, transform);
			this.elements.Add(storeIntroPanel);
		}
		Vector2 sizeDelta = this.ElementPrefab.GetComponent<RectTransform>().sizeDelta;
		int num = panelCount % this.LayoutGroups.Count;
		for (int j = 0; j < num; j++)
		{
			HorizontalLayoutGroup horizontalLayoutGroup = this.LayoutGroups[this.LayoutGroups.Count - j - 1];
			GameObject gameObject = new GameObject("Spacer");
			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			rectTransform.sizeDelta = sizeDelta;
			gameObject.transform.SetParent(horizontalLayoutGroup.transform);
		}
		int lineCount = Mathf.CeilToInt((float)panelCount / (float)this.LayoutGroups.Count);
		this.buttonList.SetNavigationType(MenuItemList.NavigationType.GridHorizontalFill, lineCount);
		this.buttonList.Initialize();
	}

	private void setupButton(StoreIntroPanel newElement, int itemIndex, Transform parent)
	{
		StoreIntroPanelView._setupButton_c__AnonStorey0 _setupButton_c__AnonStorey = new StoreIntroPanelView._setupButton_c__AnonStorey0();
		_setupButton_c__AnonStorey.itemIndex = itemIndex;
		_setupButton_c__AnonStorey.newElement = newElement;
		_setupButton_c__AnonStorey.parent = parent;
		_setupButton_c__AnonStorey._this = this;
		_setupButton_c__AnonStorey.newElement.transform.SetParent(_setupButton_c__AnonStorey.parent);
		_setupButton_c__AnonStorey.newElement.Button.DisableType = ButtonAnimator.VisualDisableType.None;
		this.buttonList.AddButton(_setupButton_c__AnonStorey.newElement.Button, new Action(_setupButton_c__AnonStorey.__m__0));
		WavedashUIButton expr_7B = _setupButton_c__AnonStorey.newElement.Button.InteractableButton;
		expr_7B.OnSelectEvent = (Action<BaseEventData>)Delegate.Combine(expr_7B.OnSelectEvent, new Action<BaseEventData>(_setupButton_c__AnonStorey.__m__1));
	}

	private void Update()
	{
		for (int i = 0; i < this.LayoutGroups.Count; i++)
		{
			this.LayoutGroups[i].enabled = false;
		}
		this.readyToSyncNavigation = true;
		if (this.syncNavigationWhenReady)
		{
			this.syncNavigationWhenReady = false;
			this.selectButtonOrFirst(this.deferedSyncButton);
		}
	}

	public void Setup(List<StoreIntroPanel.ItemLayoutData> allItemData)
	{
		for (int i = 0; i < this.elements.Count; i++)
		{
			StoreIntroPanel.ItemLayoutData itemData = allItemData[i];
			StoreIntroPanel storeIntroPanel = this.elements[i];
			storeIntroPanel.Initialize(itemData);
		}
	}

	public void UpdateMouseMode()
	{
		this.syncButtonNavigation();
	}

	public void Disable()
	{
		MenuItemButton[] buttons = this.buttonList.GetButtons();
		for (int i = 0; i < buttons.Length; i++)
		{
			MenuItemButton menuItemButton = buttons[i];
			menuItemButton.SetInteractable(false);
		}
	}

	public void Enable()
	{
		MenuItemButton[] buttons = this.buttonList.GetButtons();
		for (int i = 0; i < buttons.Length; i++)
		{
			MenuItemButton menuItemButton = buttons[i];
			menuItemButton.SetInteractable(true);
		}
	}

	public void RemoveSelection()
	{
		this.buttonList.RemoveSelection();
	}

	private void selectButtonOrFirst(MenuItemButton selectedButton = null)
	{
		if (this.readyToSyncNavigation)
		{
			if (selectedButton == null)
			{
				this.buttonList.AutoSelect(this.buttonList.GetButtons()[0]);
			}
			else
			{
				this.buttonList.AutoSelect(selectedButton);
			}
		}
		else
		{
			this.syncNavigationWhenReady = true;
			this.deferedSyncButton = selectedButton;
		}
	}

	private bool isButtonMode()
	{
		UIInputModule uIInputModule = this.uiManager.CurrentInputModule as UIInputModule;
		return (!uIInputModule || !uIInputModule.IsMouseMode) && this.windowDisplay.GetWindowCount() == 0;
	}

	private void syncButtonNavigation()
	{
		if (this.isButtonMode() && this.buttonList.CurrentSelection == null)
		{
			this.selectButtonOrFirst(null);
		}
	}

	public void SyncToPreviousSelection()
	{
		if (this.isButtonMode())
		{
			this.selectButtonOrFirst(this.mostRecentButtonModeSelection);
		}
	}

	public void SyncButtonSelectionToEquipView()
	{
		if (this.isButtonMode())
		{
			StoreIntroPanel storeIntroPanel = this.findPanelForCurrentSelection();
			MenuItemButton selectedButton = null;
			if (storeIntroPanel != null)
			{
				selectedButton = storeIntroPanel.Button;
			}
			this.selectButtonOrFirst(selectedButton);
		}
	}
}
