// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class InputSettingsScreen : GameScreen
{
	private sealed class _setupTab_c__AnonStorey0
	{
		internal TabHeader obj;

		internal InputSettingsScreen _this;

		internal void __m__0()
		{
			this._this.onTabClicked(this.obj);
		}
	}

	public GameObject Splash;

	public HorizontalLayoutGroup SettingsTabHeaderAnchor;

	public RectTransform SettingsTabsDivSeparator;

	public GameObject SettingsTabAnchor;

	public GameObject SettingsTabHeaderPrefab;

	public GameObject TweenInLeft;

	public GameObject TweenInRight;

	public GameObject GameplayScreenPrefab;

	public GameObject ControlsScreenPrefab;

	public GameObject VidoScreenPrefab;

	public GameObject AudioScreenPrefab;

	public Transform BackButtonAnchor;

	public Transform TabNavigationAnchorLeft;

	public Transform TabNavigationAnchorRight;

	public float TabInstructionsOffset = 54f;

	public GameObject BackButtonPrefab;

	public GameObject TabNavigationPrefabLeft;

	public GameObject TabNavigationPrefabRight;

	private MenuItemList settingsTabsController;

	private List<TabHeader> tabHeaders = new List<TabHeader>();

	private Dictionary<SettingsTab, SettingsTabElement> elements = new Dictionary<SettingsTab, SettingsTabElement>();

	private Vector3 settingsTabAnchorOriginalPosition;

	private bool settingsDrawCompleted;

	private bool tabHeadersDirty;

	private SettingsTab _currentTabDisplay;

	private Tweener _tabTween;

	private ScreenType previousScreen;

	[Inject]
	public ISettingsScreenAPI api
	{
		get;
		set;
	}

	[Inject]
	public IInputSettingsScreenAPI controlsAPI
	{
		get;
		set;
	}

	[Inject]
	public ISettingsTabsModel settingsTabsModel
	{
		get;
		set;
	}

	private SettingsTabElement currentTab
	{
		get
		{
			if (this._currentTabDisplay == SettingsTab.NONE)
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
		this.mapPrefab(SettingsTab.CONTROLS, this.ControlsScreenPrefab);
		this.mapPrefab(SettingsTab.VIDEO, this.VidoScreenPrefab);
		this.mapPrefab(SettingsTab.AUDIO, this.AudioScreenPrefab);
		base.listen("SettingsTabSelectionModel.SETTINGS_TAB_UPDATED", new Action(this.updateTab));
	}

	private void mapPrefab(SettingsTab id, GameObject prefab)
	{
		this.settingsTabsModel.MapPrefab(id, prefab);
	}

	protected override void postStartAndPayload()
	{
		base.postStartAndPayload();
		InputSettingsPayload inputSettingsPayload = (InputSettingsPayload)this.payload;
		this.api.SetPlayer(inputSettingsPayload.portId);
		string value;
		base.config.uiConfig.scenes.TryGetValue(ScreenType.SettingsScreen, out value);
		if (!string.IsNullOrEmpty(value))
		{
			this.Splash.SetActive(false);
		}
		base.uiManager.LockingPort = this.api.InputPort;
		this.settingsTabsController = base.createMenuController();
		this.settingsTabAnchorOriginalPosition = this.SettingsTabHeaderAnchor.transform.localPosition;
		Vector3 localPosition = this.SettingsTabHeaderAnchor.transform.localPosition;
		localPosition.x += base.gameDataManager.ConfigData.uiConfig.canvasScaler.referenceResolution.x;
		this.SettingsTabHeaderAnchor.transform.localPosition = localPosition;
		this.addTabHeaders();
		this.addTabHeaderInputInstructions();
		this.settingsTabsController.Initialize();
		base.addBackButtonForMenuScreen(this.BackButtonAnchor, this.BackButtonPrefab);
		if (base.userAudioSettings.UseAltMenuMusic())
		{
			base.audioManager.StopMusic(null, 0.5f);
		}
		this.updateTab();
		this.updateMouseMode();
	}

	private void addTabHeaderInputInstructions()
	{
		base.addInputInstuctionsForMenuScreen(this.TabNavigationAnchorLeft, this.TabNavigationPrefabLeft);
		base.addInputInstuctionsForMenuScreen(this.TabNavigationAnchorRight, this.TabNavigationPrefabRight);
	}

	private void onAutoJoin()
	{
		base.events.Broadcast(new LoadScreenCommand(ScreenType.MainMenu, null, ScreenUpdateType.Next));
	}

	private void tweenIn()
	{
		this.deactivateUnusedTabs();
		base.tweenInItem(this.TweenInRight, 1, 0, 0.25f, 0f, null);
		base.tweenInItem(this.TweenInLeft, -1, 0, 0.25f, 0f, new Action(this._tweenIn_m__0));
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

	private void updateMouseMode()
	{
		foreach (SettingsTabElement current in this.elements.Values)
		{
			current.UpdateMouseMode();
		}
	}

	private void updateTab()
	{
		if (this.settingsTabsModel.Current != this._currentTabDisplay)
		{
			if (this._currentTabDisplay != SettingsTab.NONE)
			{
				base.audioManager.PlayMenuSound(SoundKey.settings_settingsTabChange, 0f);
			}
			if (this.currentTab != null)
			{
				this.currentTab.OnDeactivate();
			}
			SettingsTabElement target = this.elements[this.settingsTabsModel.Current];
			this.tabTransition(target);
			this.updateTabHeader();
			this._currentTabDisplay = this.settingsTabsModel.Current;
			base.selectionManager.Select(null);
			if (this.currentTab != null)
			{
				this.currentTab.OnActivate();
			}
		}
	}

	private void updateTabHeader()
	{
		foreach (TabHeader current in this.tabHeaders)
		{
			current.SetCurrentlySelected(current.def.id == (int)this.settingsTabsModel.Current);
			current.SetTitle(base.localization.GetText("ui.settings.tab.title." + (SettingsTab)current.def.id));
			current.ShowNotification(false);
		}
	}

	private void tabTransition(SettingsTabElement target)
	{
		SettingsTabElement settingsTabElement = null;
		if (this._currentTabDisplay != SettingsTab.NONE)
		{
			settingsTabElement = this.elements[this._currentTabDisplay];
		}
		Vector3 zero = Vector3.zero;
		zero.x = (float)(-(float)target.Def.ordinal) * base.screenWidth;
		this.killTabTween();
		this.activateAllTabs();
		this.disableRaycastAllTabs();
		if (settingsTabElement == null)
		{
			this.SettingsTabAnchor.transform.localPosition = zero;
			this.onTabTweenComplete();
		}
		else
		{
			int num = Math.Abs(target.Def.ordinal - settingsTabElement.Def.ordinal);
			float num2 = 1f / (float)this.settingsTabsModel.Tabs.Length * 4f;
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

	public override void Deactivate()
	{
		base.uiManager.LockingPort = null;
		base.Deactivate();
	}

	private void disableRaycastAllTabs()
	{
		foreach (SettingsTab current in this.elements.Keys)
		{
			this.elements[current].GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}

	private void enableRaycastAllTabs()
	{
		foreach (SettingsTab current in this.elements.Keys)
		{
			this.elements[current].GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
	}

	private void activateAllTabs()
	{
		foreach (SettingsTab current in this.elements.Keys)
		{
			this.elements[current].gameObject.SetActive(true);
		}
	}

	private void deactivateUnusedTabs()
	{
		foreach (SettingsTab current in this.elements.Keys)
		{
			if (current != this.settingsTabsModel.Current)
			{
				this.elements[current].gameObject.SetActive(false);
			}
		}
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
		TabDefinition[] tabs = this.settingsTabsModel.Tabs;
		for (int i = 0; i < tabs.Length; i++)
		{
			TabDefinition def = tabs[i];
			TabHeader tabHeader = this.createTabHeader(def);
			this.tabHeaders.Add(tabHeader);
			this.setupTab(tabHeader, def);
		}
		this.tabHeadersDirty = true;
	}

	private bool allowInteraction(SettingsTab tab)
	{
		return this._currentTabDisplay == tab;
	}

	private void setupTab(TabHeader obj, TabDefinition def)
	{
		InputSettingsScreen._setupTab_c__AnonStorey0 _setupTab_c__AnonStorey = new InputSettingsScreen._setupTab_c__AnonStorey0();
		_setupTab_c__AnonStorey.obj = obj;
		_setupTab_c__AnonStorey._this = this;
		_setupTab_c__AnonStorey.obj.transform.SetParent(this.SettingsTabHeaderAnchor.transform, false);
		SettingsTabElement component = UnityEngine.Object.Instantiate<GameObject>(def.prefab).GetComponent<SettingsTabElement>();
		this.elements[(SettingsTab)def.id] = component;
		component.Def = def;
		component.AllowInteraction = new Func<SettingsTab, bool>(this.allowInteraction);
		component.transform.SetParent(this.SettingsTabAnchor.transform, false);
		Vector3 localPosition = component.transform.localPosition;
		localPosition.x += base.screenWidth * (float)def.ordinal;
		component.transform.localPosition = localPosition;
		this.settingsTabsController.AddButton(_setupTab_c__AnonStorey.obj.Button, new Action(_setupTab_c__AnonStorey.__m__0));
	}

	private TabHeader createTabHeader(TabDefinition def)
	{
		TabHeader component = UnityEngine.Object.Instantiate<GameObject>(this.SettingsTabHeaderPrefab).GetComponent<TabHeader>();
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
			this.SettingsTabHeaderAnchor.Redraw();
			this.settingsDrawComplete();
		}
		this.SettingsTabsDivSeparator.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)this.calculateHeaderWidth());
	}

	private bool isAnyTabHeaderDirty()
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
		num += (int)this.SettingsTabHeaderAnchor.spacing * (this.tabHeaders.Count - 1);
		return num;
	}

	private void settingsDrawComplete()
	{
		if (!this.settingsDrawCompleted)
		{
			this.settingsDrawCompleted = true;
			this.SettingsTabHeaderAnchor.transform.localPosition = this.settingsTabAnchorOriginalPosition;
			this.alignTabInstructions();
			foreach (SettingsTabElement current in this.elements.Values)
			{
				current.OnDrawComplete();
			}
			base.onDrawComplete();
			base.lockInput();
			this.tweenIn();
		}
	}

	private void onTabClicked(TabHeader header)
	{
		this.setTab((SettingsTab)header.def.id);
	}

	private void setTab(SettingsTab tab)
	{
		this.settingsTabsModel.Current = tab;
	}

	public override void OnRightTriggerPressed()
	{
		if (!this.currentTab.OnRightTriggerPressed())
		{
			this.settingsTabsModel.Shift(1);
		}
	}

	public override void OnLeftTriggerPressed()
	{
		if (!this.currentTab.OnLeftTriggerPressed())
		{
			this.settingsTabsModel.Shift(-1);
		}
	}

	public override void OnRightStickLeft()
	{
		if (this.currentTab != null)
		{
			this.currentTab.OnRightStickLeft();
		}
	}

	public override void OnRightStickRight()
	{
		if (this.currentTab != null)
		{
			this.currentTab.OnRightStickRight();
		}
	}

	public override void OnDPadLeft()
	{
		this.currentTab.OnDPadLeft();
	}

	public override void OnDPadRight()
	{
		this.currentTab.OnDPadRight();
	}

	public override void OnCancelPressed()
	{
		if (!this.currentTab.OnCancelPressed())
		{
			this.GoToPreviousScreen();
		}
	}

	public override void GoToPreviousScreen()
	{
		if (this.controlsAPI.SettingsChanged)
		{
			if (this.controlsAPI.IsMovementUnbound)
			{
				GenericDialog genericDialog = base.dialogController.ShowTwoButtonDialog(base.localization.GetText("ui.settings.movementUnbound.title"), base.localization.GetText("ui.settings.movementUnbound.body"), base.localization.GetText("ui.settings.movementUnbound.confirm"), base.localization.GetText("ui.settings.movementUnbound.cancel"));
				GenericDialog expr_6D = genericDialog;
				expr_6D.ConfirmCallback = (Action)Delegate.Combine(expr_6D.ConfirmCallback, new Action(this._GoToPreviousScreen_m__3));
			}
			else
			{
				GenericDialog genericDialog2 = base.dialogController.ShowTwoButtonDialog(base.localization.GetText("ui.settings.notSavedWarning.title"), base.localization.GetText("ui.settings.notSavedWarning.body"), base.localization.GetText("ui.settings.notSavedWarning.confirm"), base.localization.GetText("ui.settings.notSavedWarning.cancel"));
				GenericDialog expr_E0 = genericDialog2;
				expr_E0.ConfirmCallback = (Action)Delegate.Combine(expr_E0.ConfirmCallback, new Action(this._GoToPreviousScreen_m__4));
				GenericDialog expr_102 = genericDialog2;
				expr_102.CloseCallback = (Action)Delegate.Combine(expr_102.CloseCallback, new Action(this._GoToPreviousScreen_m__5));
			}
		}
		else
		{
			base.GoToPreviousScreen();
		}
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

	public override void OnXButtonPressed()
	{
		this.currentTab.OnXButtonPressed();
	}

	public override void OnDestroy()
	{
		this.killTabTween();
		base.OnDestroy();
	}

	private void _tweenIn_m__0()
	{
		this.onAnimationsComplete();
	}

	private Vector3 _tabTransition_m__1()
	{
		return this.SettingsTabAnchor.transform.localPosition;
	}

	private void _tabTransition_m__2(Vector3 valueIn)
	{
		this.SettingsTabAnchor.transform.localPosition = valueIn;
	}

	private void _GoToPreviousScreen___BaseCallProxy0()
	{
		base.GoToPreviousScreen();
	}

	private void _GoToPreviousScreen_m__3()
	{
		this._GoToPreviousScreen___BaseCallProxy0();
	}

	private void _GoToPreviousScreen_m__4()
	{
		this.controlsAPI.SaveControls();
	}

	private void _GoToPreviousScreen_m__5()
	{
		this._GoToPreviousScreen___BaseCallProxy0();
	}
}
