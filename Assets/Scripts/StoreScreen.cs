// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreScreen : GameScreen
{
	private sealed class _fadeToBlack_c__AnonStorey0
	{
		internal Action callback;

		internal StoreScreen _this;

		internal float __m__0()
		{
			return this._this.FadeToBlack.alpha;
		}

		internal void __m__1(float x)
		{
			this._this.FadeToBlack.alpha = x;
		}

		internal void __m__2()
		{
			this._this.killFadeToBlackTween();
			this.callback();
		}
	}

	private sealed class _setupTab_c__AnonStorey1
	{
		internal TabHeader obj;

		internal StoreScreen _this;

		internal void __m__0()
		{
			this._this.onTabClicked(this.obj);
		}
	}

	public HorizontalLayoutGroup StoreTabHeaderAnchor;

	public RectTransform StoreTabsDivSeparator;

	public GameObject StoreTabAnchor;

	public GameObject StoreTabHeaderPrefab;

	public GameObject TweenInLeft;

	public GameObject TweenInRight;

	public GameObject Header;

	public GameObject FeaturedScreenPrefab;

	public GameObject LootBoxScreenPrefab;

	public GameObject BundlesScreenPrefab;

	public GameObject CharactersScreenPrefab;

	public GameObject CollectiblesScreenPrefab;

	public Transform BackButtonAnchor;

	public Transform UnboxingBackButtonStub;

	public Transform TabNavigationAnchorLeft;

	public Transform TabNavigationAnchorRight;

	public float TabInstructionsOffset = 54f;

	public GameObject BackButtonPrefab;

	public GameObject TabNavigationPrefabLeft;

	public GameObject TabNavigationPrefabRight;

	public CanvasGroup MainCanvas;

	public CanvasGroup UnboxingCanvas;

	public CanvasGroup FadeToBlack;

	public TextMeshProUGUI Title;

	private InputInstructions tabInstructionsLeft;

	private InputInstructions tabInstructionsRight;

	private MenuItemList storeTabsController;

	private List<TabHeader> tabHeaders = new List<TabHeader>();

	private Dictionary<StoreTab, StoreTabElement> elements = new Dictionary<StoreTab, StoreTabElement>(default(StoreTabComparer));

	private Vector3 storeTabAnchorOriginalPosition;

	private bool storeDrawCompleted;

	private bool tabHeadersDirty;

	private StoreTab _currentTabDisplay;

	private Tweener _tabTween;

	private Tweener fadeToBlackTween;

	private StoreMode currentMode;

	private StoreScene3D storeScene;

	[Inject]
	public IStoreTabsModel storeTabsModel
	{
		get;
		set;
	}

	[Inject]
	public IStoreAPI api
	{
		get;
		set;
	}

	[Inject]
	public ICollectiblesTabAPI collectiblesTabAPI
	{
		get;
		set;
	}

	[Inject]
	public IFeaturedTabAPI featuredTabAPI
	{
		get;
		set;
	}

	[Inject]
	public IRichPresence richPresence
	{
		get;
		set;
	}

	protected override bool SupportsAutoJoin
	{
		get
		{
			return true;
		}
	}

	private StoreTabElement currentTab
	{
		get
		{
			if (this._currentTabDisplay == StoreTab.NONE)
			{
				return null;
			}
			return this.elements[this._currentTabDisplay];
		}
	}

	public override bool WaitForDrawCallback
	{
		get
		{
			return true;
		}
	}

	protected override void AddScreenListeners()
	{
		base.AddScreenListeners();
		this.mapPrefab(StoreTab.FEATURED, this.FeaturedScreenPrefab);
		if (base.gameDataManager.IsFeatureEnabled(FeatureID.PortalPacks))
		{
			this.mapPrefab(StoreTab.LOOT_BOXES, this.LootBoxScreenPrefab);
		}
		this.mapPrefab(StoreTab.BUNDLES, this.BundlesScreenPrefab);
		this.mapPrefab(StoreTab.CHARACTERS, this.CharactersScreenPrefab);
		this.mapPrefab(StoreTab.COLLECTIBLES, this.CollectiblesScreenPrefab);
		this.Title.text = base.localization.GetText("ui.store.title", this.api.PortDisplay);
		base.listen("StoreTabSelectionModel.STORE_TAB_UPDATED", new Action(this.updateTab));
		base.listen("CollectiblesTabAPI.UPDATED", new Action(this.updateHeaderState));
		base.listen(StoreAPI.UPDATE, new Action(this.updateMode));
	}

	protected override void onAutoJoinRequest()
	{
		this.currentTab.OnCancelPressed();
		base.onAutoJoinRequest();
	}

	private void mapPrefab(StoreTab id, GameObject prefab)
	{
		this.storeTabsModel.MapPrefab(id, prefab);
	}

	protected override void postStartAndPayload()
	{
		base.postStartAndPayload();
		this.api.OnScreenOpened();
		this.storeScene = base.uiAdapter.GetUIScene<StoreScene3D>();
		this.storeScene.Set3DScene(base.gameDataManager.IsFeatureEnabled(FeatureID.StoreScene3D));
		this.storeTabsController = base.createMenuController();
		this.storeTabsController.MouseOnly = true;
		this.storeTabAnchorOriginalPosition = this.StoreTabHeaderAnchor.transform.localPosition;
		Vector3 localPosition = this.StoreTabHeaderAnchor.transform.localPosition;
		localPosition.x += base.gameDataManager.ConfigData.uiConfig.canvasScaler.referenceResolution.x;
		this.StoreTabHeaderAnchor.transform.localPosition = localPosition;
		this.FadeToBlack.alpha = 0f;
		this.addTabHeaders();
		base.addBackButtonForMenuScreen(this.BackButtonAnchor, this.BackButtonPrefab);
		base.addBackButtonForMenuScreen(this.UnboxingBackButtonStub, this.BackButtonPrefab);
		this.addTabHeaderInputInstructions();
		this.storeTabsController.Initialize();
		this.updateMode();
		this.updateTab();
		this.updateMouseMode();
		this.syncMusic();
		this.richPresence.SetPresence("CustomizingCharacter", null, null, null);
	}

	private void addTabHeaderInputInstructions()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.TabNavigationPrefabLeft);
		gameObject.transform.SetParent(this.TabNavigationAnchorLeft, false);
		this.tabInstructionsLeft = gameObject.GetComponent<InputInstructions>();
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.TabNavigationPrefabRight);
		gameObject2.transform.SetParent(this.TabNavigationAnchorRight, false);
		this.tabInstructionsRight = gameObject2.GetComponent<InputInstructions>();
		this.updateMouseMode();
	}

	private void tweenIn()
	{
		base.lockInput();
		this.deactivateUnusedTabs();
		base.tweenInItem(this.TweenInRight, 1, 0, 0.25f, 0f, null);
		base.tweenInItem(this.TweenInLeft, -1, 0, 0.25f, 0f, new Action(this.onAnimationsComplete));
	}

	private void onAnimationsComplete()
	{
		base.unlockInput();
	}

	protected override void handleMenuSelectionOnButtonMode()
	{
	}

	public override void OnAnythingPressed()
	{
		base.OnAnythingPressed();
		this.updateMouseMode();
	}

	public override void OnAnyMouseEvent()
	{
		base.OnAnyMouseEvent();
		this.updateMouseMode();
	}

	public override void OnAnyNavigationButtonPressed()
	{
		base.OnAnyNavigationButtonPressed();
	}

	private void updateMouseMode()
	{
		foreach (KeyValuePair<StoreTab, StoreTabElement> current in this.elements)
		{
			StoreTabElement value = current.Value;
			value.UpdateMouseMode();
		}
		if (this.tabInstructionsLeft != null)
		{
			this.tabInstructionsLeft.SetControlMode(base.uiManager.CurrentInputModule.CurrentMode);
			this.tabInstructionsLeft.gameObject.SetActive(this.storeTabsModel.ShowTabs);
			this.tabInstructionsRight.SetControlMode(base.uiManager.CurrentInputModule.CurrentMode);
			this.tabInstructionsRight.gameObject.SetActive(this.storeTabsModel.ShowTabs);
		}
	}

	private void updateMode()
	{
		if (this.currentMode == this.api.Mode)
		{
			this.setCorrectCanvas();
		}
		else
		{
			this.currentMode = this.api.Mode;
			base.lockInput();
			this.FadeToBlack.gameObject.SetActive(true);
			base.audioManager.StopMusic(null, 0.5f);
			this.fadeToBlack(1f, new Action(this._updateMode_m__0));
		}
	}

	private void syncMusic()
	{
		if (this.api.Mode == StoreMode.UNBOXING)
		{
			base.audioManager.PlayMusic(SoundKey.unboxing_music);
		}
		else
		{
			base.audioManager.PlayMusic(SoundKey.store_music);
		}
	}

	private void setCorrectCanvas()
	{
		this.storeScene.UpdateMode(this.api.Mode);
		this.MainCanvas.gameObject.SetActive(true);
		this.UnboxingCanvas.gameObject.SetActive(true);
		if (this.api.Mode == StoreMode.NORMAL)
		{
			this.MainCanvas.transform.localPosition = Vector3.zero;
			this.UnboxingCanvas.transform.localPosition = new Vector3(100000f, 0f, 0f);
		}
		else
		{
			this.UnboxingCanvas.transform.localPosition = Vector3.zero;
			this.MainCanvas.transform.localPosition = new Vector3(100000f, 0f, 0f);
		}
	}

	private void fadeToBlack(float alpha, Action callback)
	{
		StoreScreen._fadeToBlack_c__AnonStorey0 _fadeToBlack_c__AnonStorey = new StoreScreen._fadeToBlack_c__AnonStorey0();
		_fadeToBlack_c__AnonStorey.callback = callback;
		_fadeToBlack_c__AnonStorey._this = this;
		float duration = 0.35f;
		this.killFadeToBlackTween();
		this.fadeToBlackTween = DOTween.To(new DOGetter<float>(_fadeToBlack_c__AnonStorey.__m__0), new DOSetter<float>(_fadeToBlack_c__AnonStorey.__m__1), alpha, duration).SetEase(Ease.OutSine).OnComplete(new TweenCallback(_fadeToBlack_c__AnonStorey.__m__2));
	}

	private void killFadeToBlackTween()
	{
		TweenUtil.Destroy(ref this.fadeToBlackTween);
	}

	private void updateTab()
	{
		this.StoreTabHeaderAnchor.gameObject.SetActive(this.storeTabsModel.ShowTabs);
		if (this.storeTabsModel.Current != this._currentTabDisplay)
		{
			if (this._currentTabDisplay != StoreTab.NONE)
			{
				base.audioManager.PlayMenuSound(SoundKey.store_storeTabChange, 0f);
			}
			if (this.storeTabsModel.ShowTabs)
			{
				StoreTabElement target = this.elements[this.storeTabsModel.Current];
				this.tabTransition(target);
				this.updateTabHeader();
			}
			this._currentTabDisplay = this.storeTabsModel.Current;
			if (this.api.Mode == StoreMode.NORMAL)
			{
				base.selectionManager.Select(null);
			}
			if (this.currentTab != null)
			{
				this.currentTab.OnActivate();
			}
		}
		this.updateHeaderState();
	}

	private void updateHeaderState()
	{
		bool flag = this.api.Mode == StoreMode.NORMAL && this.storeTabsModel.Current == StoreTab.COLLECTIBLES && this.collectiblesTabAPI.GetState() == CollectiblesTabState.NetsukeEquipView;
		this.Header.SetActive(!flag);
	}

	private void updateTabHeader()
	{
		foreach (TabHeader current in this.tabHeaders)
		{
			current.SetCurrentlySelected(current.def.id == (int)this.storeTabsModel.Current);
			current.SetTitle(base.localization.GetText("ui.store.tab.title." + (StoreTab)current.def.id));
			current.ShowNotification(current.def.id == 1 && !this.featuredTabAPI.IsProAccountUnlocked());
		}
	}

	private void tabTransition(StoreTabElement target)
	{
		StoreTabElement storeTabElement = null;
		if (this._currentTabDisplay != StoreTab.NONE)
		{
			storeTabElement = this.elements[this._currentTabDisplay];
		}
		Vector3 zero = Vector3.zero;
		zero.x = (float)(-(float)target.Def.ordinal) * base.screenWidth;
		this.killTabTween();
		this.activateAllTabs();
		this.disableRaycastAllTabs();
		if (storeTabElement == null)
		{
			this.StoreTabAnchor.transform.localPosition = zero;
			this.onTabTweenComplete();
		}
		else
		{
			int num = Math.Abs(target.Def.ordinal - storeTabElement.Def.ordinal);
			float num2 = 1f / (float)this.storeTabsModel.Tabs.Length * 4f;
			float duration = (float)Math.Pow((double)((float)num * num2), 0.5) * 0.5f;
			this._tabTween = DOTween.To(new DOGetter<Vector3>(this._tabTransition_m__1), new DOSetter<Vector3>(this._tabTransition_m__2), zero, duration).SetEase(Ease.OutExpo).OnComplete(new TweenCallback(this.onTabTweenComplete));
		}
	}

	private void onTabTweenComplete()
	{
		this.killTabTween();
		this.enableRaycastAllTabs();
		this.deactivateUnusedTabs();
	}

	private void disableRaycastAllTabs()
	{
		foreach (StoreTab current in this.elements.Keys)
		{
			this.elements[current].GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}

	private void enableRaycastAllTabs()
	{
		foreach (StoreTab current in this.elements.Keys)
		{
			this.elements[current].GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
	}

	private void activateAllTabs()
	{
		foreach (StoreTab current in this.elements.Keys)
		{
			this.elements[current].gameObject.SetActive(true);
		}
	}

	private void deactivateUnusedTabs()
	{
	}

	private void killTabTween()
	{
		if (this._tabTween != null && this._tabTween.IsPlaying())
		{
			this._tabTween.Kill(false);
		}
		this._tabTween = null;
	}

	private void addTabHeaders()
	{
		TabDefinition[] tabs = this.storeTabsModel.Tabs;
		for (int i = 0; i < tabs.Length; i++)
		{
			TabDefinition def = tabs[i];
			TabHeader tabHeader = this.createTabHeader(def);
			this.tabHeaders.Add(tabHeader);
			this.setupTab(tabHeader, def);
		}
		this.tabHeadersDirty = true;
	}

	private bool allowInteraction(StoreTab tab)
	{
		return this._currentTabDisplay == tab;
	}

	private void setupTab(TabHeader obj, TabDefinition def)
	{
		StoreScreen._setupTab_c__AnonStorey1 _setupTab_c__AnonStorey = new StoreScreen._setupTab_c__AnonStorey1();
		_setupTab_c__AnonStorey.obj = obj;
		_setupTab_c__AnonStorey._this = this;
		_setupTab_c__AnonStorey.obj.transform.SetParent(this.StoreTabHeaderAnchor.transform, false);
		StoreTabElement component = UnityEngine.Object.Instantiate<GameObject>(def.prefab).GetComponent<StoreTabElement>();
		this.elements[(StoreTab)def.id] = component;
		component.Def = def;
		component.AllowInteraction = new Func<StoreTab, bool>(this.allowInteraction);
		component.transform.SetParent(this.StoreTabAnchor.transform, false);
		Vector3 localPosition = component.transform.localPosition;
		localPosition.x += base.screenWidth * (float)def.ordinal;
		component.transform.localPosition = localPosition;
		this.storeTabsController.AddButton(_setupTab_c__AnonStorey.obj.Button, new Action(_setupTab_c__AnonStorey.__m__0));
	}

	private TabHeader createTabHeader(TabDefinition def)
	{
		TabHeader component = UnityEngine.Object.Instantiate<GameObject>(this.StoreTabHeaderPrefab).GetComponent<TabHeader>();
		base.injector.Inject(component);
		component.Init(def);
		return component;
	}

	protected override void Update()
	{
		base.Update();
		if (this.tabHeadersDirty && !this.isAnyTabHeaderDirty())
		{
			this.tabHeadersDirty = false;
			this.StoreTabHeaderAnchor.Redraw();
			this.storeDrawComplete();
		}
		this.StoreTabsDivSeparator.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)this.calculateHeaderWidth());
	}

	private bool isAnyTabHeaderDirty()
	{
		if (this.storeTabsModel.ShowTabs)
		{
			foreach (TabHeader current in this.tabHeaders)
			{
				if (current.IsDirty)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	private void storeDrawComplete()
	{
		if (!this.storeDrawCompleted)
		{
			this.storeDrawCompleted = true;
			this.StoreTabHeaderAnchor.transform.localPosition = this.storeTabAnchorOriginalPosition;
			this.alignTabInstructions();
			foreach (StoreTabElement current in this.elements.Values)
			{
				current.OnDrawComplete();
			}
			base.onDrawComplete();
			this.tweenIn();
		}
	}

	private void alignTabInstructions()
	{
		int num = this.calculateHeaderWidth();
		Vector3 localPosition = this.TabNavigationAnchorLeft.transform.localPosition;
		localPosition.x = (float)(-(float)num / 2) - this.TabInstructionsOffset;
		this.TabNavigationAnchorLeft.transform.localPosition = localPosition;
		localPosition = this.TabNavigationAnchorRight.transform.localPosition;
		localPosition.x = (float)(num / 2) + this.TabInstructionsOffset;
		this.TabNavigationAnchorRight.transform.localPosition = localPosition;
	}

	private int calculateHeaderWidth()
	{
		int num = 0;
		foreach (TabHeader current in this.tabHeaders)
		{
			num += (int)current.GetComponent<RectTransform>().sizeDelta.x;
		}
		num += (int)this.StoreTabHeaderAnchor.spacing * (this.tabHeaders.Count - 1);
		return num;
	}

	private void onTabClicked(TabHeader header)
	{
		this.setTab((StoreTab)header.def.id);
	}

	private void setTab(StoreTab tab)
	{
		this.storeTabsModel.Current = tab;
	}

	public override void OnRightTriggerPressed()
	{
		if (this.api.Mode == StoreMode.NORMAL && !this.currentTab.OnRightTriggerPressed())
		{
			this.storeTabsModel.Shift(1);
		}
	}

	public override void OnLeftTriggerPressed()
	{
		if (this.api.Mode == StoreMode.NORMAL && !this.currentTab.OnLeftTriggerPressed())
		{
			this.storeTabsModel.Shift(-1);
		}
	}

	public override void OnRightStickLeft()
	{
		this.currentTab.OnRightStickLeft();
	}

	public override void OnRightStickRight()
	{
		this.currentTab.OnRightStickRight();
	}

	public override void OnRightStickUp()
	{
		this.currentTab.OnRightStickUp();
	}

	public override void OnRightStickDown()
	{
		this.currentTab.OnRightStickDown();
	}

	public override void OnDPadLeft()
	{
		this.currentTab.OnDPadLeft();
	}

	public override void OnDPadRight()
	{
		this.currentTab.OnDPadRight();
	}

	public override void UpdateRightStick(float x, float y)
	{
		if (this.currentTab != null)
		{
			this.currentTab.UpdateRightStick(x, y);
		}
	}

	public override void OnCancelPressed()
	{
		bool flag = this.currentTab.OnCancelPressed();
		this.tryGoBack(ref flag);
	}

	protected override void onBackButtonClicked(InputEventData inputEventData)
	{
		bool flag = this.currentTab.OnBackButtonClicked();
		this.tryGoBack(ref flag);
	}

	private void tryGoBack(ref bool usedInput)
	{
		if (!usedInput)
		{
			this.richPresence.SetPresence(null, null, null, null);
			this.GoToPreviousScreen();
		}
	}

	public override void OnSubmitPressed()
	{
		this.currentTab.OnSubmitPressed();
	}

	public override void OnLeftBumperPressed()
	{
		this.currentTab.OnLeftBumperPressed();
	}

	public override void OnZPressed()
	{
		this.currentTab.OnZPressed();
	}

	public override void OnLeft()
	{
		this.currentTab.OnLeft();
	}

	public override void OnRight()
	{
		this.currentTab.OnRight();
	}

	public override void OnUp()
	{
		this.currentTab.OnUp();
	}

	public override void OnDown()
	{
		this.currentTab.OnDown();
	}

	public override void OnYButtonPressed()
	{
		this.currentTab.OnYButtonPressed();
	}

	public override void OnDestroy()
	{
		this.killTabTween();
		base.OnDestroy();
	}

	private void _updateMode_m__0()
	{
		this.setCorrectCanvas();
		this.fadeToBlack(0f, new Action(this._updateMode_m__3));
	}

	private Vector3 _tabTransition_m__1()
	{
		return this.StoreTabAnchor.transform.localPosition;
	}

	private void _tabTransition_m__2(Vector3 valueIn)
	{
		this.StoreTabAnchor.transform.localPosition = valueIn;
	}

	private void _updateMode_m__3()
	{
		this.FadeToBlack.gameObject.SetActive(false);
		base.unlockInput();
		this.storeScene.OnSceneTransitionComplete();
		this.syncMusic();
	}
}
