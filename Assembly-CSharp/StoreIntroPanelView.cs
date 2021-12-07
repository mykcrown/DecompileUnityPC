using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000A2E RID: 2606
public class StoreIntroPanelView : MonoBehaviour
{
	// Token: 0x1700120C RID: 4620
	// (get) Token: 0x06004BDA RID: 19418 RVA: 0x00142FF8 File Offset: 0x001413F8
	// (set) Token: 0x06004BDB RID: 19419 RVA: 0x00143000 File Offset: 0x00141400
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x1700120D RID: 4621
	// (get) Token: 0x06004BDC RID: 19420 RVA: 0x00143009 File Offset: 0x00141409
	// (set) Token: 0x06004BDD RID: 19421 RVA: 0x00143011 File Offset: 0x00141411
	[Inject]
	public UIManager uiManager { get; set; }

	// Token: 0x1700120E RID: 4622
	// (get) Token: 0x06004BDE RID: 19422 RVA: 0x0014301A File Offset: 0x0014141A
	// (set) Token: 0x06004BDF RID: 19423 RVA: 0x00143022 File Offset: 0x00141422
	[Inject]
	public IWindowDisplay windowDisplay { get; set; }

	// Token: 0x1700120F RID: 4623
	// (get) Token: 0x06004BE0 RID: 19424 RVA: 0x0014302B File Offset: 0x0014142B
	// (set) Token: 0x06004BE1 RID: 19425 RVA: 0x00143033 File Offset: 0x00141433
	public List<StoreIntroPanel> elements { get; private set; }

	// Token: 0x06004BE2 RID: 19426 RVA: 0x0014303C File Offset: 0x0014143C
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

	// Token: 0x06004BE3 RID: 19427 RVA: 0x00143188 File Offset: 0x00141588
	private void setupButton(StoreIntroPanel newElement, int itemIndex, Transform parent)
	{
		newElement.transform.SetParent(parent);
		newElement.Button.DisableType = ButtonAnimator.VisualDisableType.None;
		this.buttonList.AddButton(newElement.Button, delegate()
		{
			if (this.OnItemClicked != null)
			{
				this.OnItemClicked(itemIndex);
			}
		});
		WavedashUIButton interactableButton = newElement.Button.InteractableButton;
		interactableButton.OnSelectEvent = (Action<BaseEventData>)Delegate.Combine(interactableButton.OnSelectEvent, new Action<BaseEventData>(delegate(BaseEventData eventData)
		{
			newElement.transform.SetAsLastSibling();
			parent.SetAsLastSibling();
			if (!(this.uiManager.CurrentInputModule as UIInputModule).IsMouseMode)
			{
				this.mostRecentButtonModeSelection = newElement.Button;
			}
		}));
	}

	// Token: 0x06004BE4 RID: 19428 RVA: 0x00143234 File Offset: 0x00141634
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

	// Token: 0x06004BE5 RID: 19429 RVA: 0x00143294 File Offset: 0x00141694
	public void Setup(List<StoreIntroPanel.ItemLayoutData> allItemData)
	{
		for (int i = 0; i < this.elements.Count; i++)
		{
			StoreIntroPanel.ItemLayoutData itemData = allItemData[i];
			StoreIntroPanel storeIntroPanel = this.elements[i];
			storeIntroPanel.Initialize(itemData);
		}
	}

	// Token: 0x06004BE6 RID: 19430 RVA: 0x001432D9 File Offset: 0x001416D9
	public void UpdateMouseMode()
	{
		this.syncButtonNavigation();
	}

	// Token: 0x06004BE7 RID: 19431 RVA: 0x001432E4 File Offset: 0x001416E4
	public void Disable()
	{
		foreach (MenuItemButton menuItemButton in this.buttonList.GetButtons())
		{
			menuItemButton.SetInteractable(false);
		}
	}

	// Token: 0x06004BE8 RID: 19432 RVA: 0x0014331C File Offset: 0x0014171C
	public void Enable()
	{
		foreach (MenuItemButton menuItemButton in this.buttonList.GetButtons())
		{
			menuItemButton.SetInteractable(true);
		}
	}

	// Token: 0x06004BE9 RID: 19433 RVA: 0x00143354 File Offset: 0x00141754
	public void RemoveSelection()
	{
		this.buttonList.RemoveSelection();
	}

	// Token: 0x06004BEA RID: 19434 RVA: 0x00143364 File Offset: 0x00141764
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

	// Token: 0x06004BEB RID: 19435 RVA: 0x001433C4 File Offset: 0x001417C4
	private bool isButtonMode()
	{
		UIInputModule uiinputModule = this.uiManager.CurrentInputModule as UIInputModule;
		return (!uiinputModule || !uiinputModule.IsMouseMode) && this.windowDisplay.GetWindowCount() == 0;
	}

	// Token: 0x06004BEC RID: 19436 RVA: 0x00143409 File Offset: 0x00141809
	private void syncButtonNavigation()
	{
		if (this.isButtonMode() && this.buttonList.CurrentSelection == null)
		{
			this.selectButtonOrFirst(null);
		}
	}

	// Token: 0x06004BED RID: 19437 RVA: 0x00143433 File Offset: 0x00141833
	public void SyncToPreviousSelection()
	{
		if (this.isButtonMode())
		{
			this.selectButtonOrFirst(this.mostRecentButtonModeSelection);
		}
	}

	// Token: 0x06004BEE RID: 19438 RVA: 0x0014344C File Offset: 0x0014184C
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

	// Token: 0x040031DC RID: 12764
	public StoreIntroPanel ElementPrefab;

	// Token: 0x040031DD RID: 12765
	public List<HorizontalLayoutGroup> LayoutGroups;

	// Token: 0x040031DF RID: 12767
	public Action<int> OnItemClicked;

	// Token: 0x040031E0 RID: 12768
	private Func<StoreIntroPanel> findPanelForCurrentSelection;

	// Token: 0x040031E1 RID: 12769
	private MenuItemList buttonList;

	// Token: 0x040031E2 RID: 12770
	private MenuItemButton mostRecentButtonModeSelection;

	// Token: 0x040031E3 RID: 12771
	private bool readyToSyncNavigation;

	// Token: 0x040031E4 RID: 12772
	private bool syncNavigationWhenReady;

	// Token: 0x040031E5 RID: 12773
	private MenuItemButton deferedSyncButton;
}
