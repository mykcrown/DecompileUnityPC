using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200097D RID: 2429
public class InputSettingsScreen : GameScreen
{
	// Token: 0x17000F5B RID: 3931
	// (get) Token: 0x06004132 RID: 16690 RVA: 0x0012557D File Offset: 0x0012397D
	// (set) Token: 0x06004133 RID: 16691 RVA: 0x00125585 File Offset: 0x00123985
	[Inject]
	public ISettingsScreenAPI api { get; set; }

	// Token: 0x17000F5C RID: 3932
	// (get) Token: 0x06004134 RID: 16692 RVA: 0x0012558E File Offset: 0x0012398E
	// (set) Token: 0x06004135 RID: 16693 RVA: 0x00125596 File Offset: 0x00123996
	[Inject]
	public IInputSettingsScreenAPI controlsAPI { get; set; }

	// Token: 0x17000F5D RID: 3933
	// (get) Token: 0x06004136 RID: 16694 RVA: 0x0012559F File Offset: 0x0012399F
	// (set) Token: 0x06004137 RID: 16695 RVA: 0x001255A7 File Offset: 0x001239A7
	[Inject]
	public ISettingsTabsModel settingsTabsModel { get; set; }

	// Token: 0x06004138 RID: 16696 RVA: 0x001255B0 File Offset: 0x001239B0
	protected override void AddScreenListeners()
	{
		base.AddScreenListeners();
		this.mapPrefab(SettingsTab.CONTROLS, this.ControlsScreenPrefab);
		this.mapPrefab(SettingsTab.VIDEO, this.VidoScreenPrefab);
		this.mapPrefab(SettingsTab.AUDIO, this.AudioScreenPrefab);
		base.listen("SettingsTabSelectionModel.SETTINGS_TAB_UPDATED", new Action(this.updateTab));
	}

	// Token: 0x06004139 RID: 16697 RVA: 0x00125601 File Offset: 0x00123A01
	private void mapPrefab(SettingsTab id, GameObject prefab)
	{
		this.settingsTabsModel.MapPrefab(id, prefab);
	}

	// Token: 0x0600413A RID: 16698 RVA: 0x00125610 File Offset: 0x00123A10
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

	// Token: 0x0600413B RID: 16699 RVA: 0x0012574E File Offset: 0x00123B4E
	private void addTabHeaderInputInstructions()
	{
		base.addInputInstuctionsForMenuScreen(this.TabNavigationAnchorLeft, this.TabNavigationPrefabLeft);
		base.addInputInstuctionsForMenuScreen(this.TabNavigationAnchorRight, this.TabNavigationPrefabRight);
	}

	// Token: 0x0600413C RID: 16700 RVA: 0x00125776 File Offset: 0x00123B76
	private void onAutoJoin()
	{
		base.events.Broadcast(new LoadScreenCommand(ScreenType.MainMenu, null, ScreenUpdateType.Next));
	}

	// Token: 0x0600413D RID: 16701 RVA: 0x0012578C File Offset: 0x00123B8C
	private void tweenIn()
	{
		this.deactivateUnusedTabs();
		base.tweenInItem(this.TweenInRight, 1, 0, 0.25f, 0f, null);
		base.tweenInItem(this.TweenInLeft, -1, 0, 0.25f, 0f, delegate
		{
			this.onAnimationsComplete();
		});
	}

	// Token: 0x0600413E RID: 16702 RVA: 0x001257DC File Offset: 0x00123BDC
	private void onAnimationsComplete()
	{
		base.unlockInput();
	}

	// Token: 0x0600413F RID: 16703 RVA: 0x001257E4 File Offset: 0x00123BE4
	protected override void handleMenuSelectionOnButtonMode()
	{
	}

	// Token: 0x06004140 RID: 16704 RVA: 0x001257E6 File Offset: 0x00123BE6
	public override void OnAnythingPressed()
	{
		base.OnAnythingPressed();
		this.updateMouseMode();
	}

	// Token: 0x06004141 RID: 16705 RVA: 0x001257F4 File Offset: 0x00123BF4
	public override void OnAnyMouseEvent()
	{
		base.OnAnyMouseEvent();
		this.updateMouseMode();
	}

	// Token: 0x06004142 RID: 16706 RVA: 0x00125804 File Offset: 0x00123C04
	private void updateMouseMode()
	{
		foreach (SettingsTabElement settingsTabElement in this.elements.Values)
		{
			settingsTabElement.UpdateMouseMode();
		}
	}

	// Token: 0x06004143 RID: 16707 RVA: 0x00125864 File Offset: 0x00123C64
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

	// Token: 0x06004144 RID: 16708 RVA: 0x00125920 File Offset: 0x00123D20
	private void updateTabHeader()
	{
		foreach (TabHeader tabHeader in this.tabHeaders)
		{
			tabHeader.SetCurrentlySelected(tabHeader.def.id == (int)this.settingsTabsModel.Current);
			tabHeader.SetTitle(base.localization.GetText("ui.settings.tab.title." + (SettingsTab)tabHeader.def.id));
			tabHeader.ShowNotification(false);
		}
	}

	// Token: 0x06004145 RID: 16709 RVA: 0x001259C8 File Offset: 0x00123DC8
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
			this._tabTween = DOTween.To(() => this.SettingsTabAnchor.transform.localPosition, delegate(Vector3 valueIn)
			{
				this.SettingsTabAnchor.transform.localPosition = valueIn;
			}, zero, duration).SetEase(Ease.OutExpo).OnComplete(new TweenCallback(this.onTabTweenComplete));
		}
	}

	// Token: 0x06004146 RID: 16710 RVA: 0x00125AE1 File Offset: 0x00123EE1
	private void onTabTweenComplete()
	{
		this.killTabTween();
		this.enableRaycastAllTabs();
		this.deactivateUnusedTabs();
	}

	// Token: 0x06004147 RID: 16711 RVA: 0x00125AF5 File Offset: 0x00123EF5
	public override void Deactivate()
	{
		base.uiManager.LockingPort = null;
		base.Deactivate();
	}

	// Token: 0x06004148 RID: 16712 RVA: 0x00125B0C File Offset: 0x00123F0C
	private void disableRaycastAllTabs()
	{
		foreach (SettingsTab key in this.elements.Keys)
		{
			this.elements[key].GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}

	// Token: 0x06004149 RID: 16713 RVA: 0x00125B80 File Offset: 0x00123F80
	private void enableRaycastAllTabs()
	{
		foreach (SettingsTab key in this.elements.Keys)
		{
			this.elements[key].GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
	}

	// Token: 0x0600414A RID: 16714 RVA: 0x00125BF4 File Offset: 0x00123FF4
	private void activateAllTabs()
	{
		foreach (SettingsTab key in this.elements.Keys)
		{
			this.elements[key].gameObject.SetActive(true);
		}
	}

	// Token: 0x0600414B RID: 16715 RVA: 0x00125C68 File Offset: 0x00124068
	private void deactivateUnusedTabs()
	{
		foreach (SettingsTab settingsTab in this.elements.Keys)
		{
			if (settingsTab != this.settingsTabsModel.Current)
			{
				this.elements[settingsTab].gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x0600414C RID: 16716 RVA: 0x00125CEC File Offset: 0x001240EC
	private void killTabTween()
	{
		if (this._tabTween != null && this._tabTween.IsPlaying())
		{
			this._tabTween.Kill(false);
		}
		this._tabTween = null;
	}

	// Token: 0x0600414D RID: 16717 RVA: 0x00125D1C File Offset: 0x0012411C
	private void addTabHeaders()
	{
		foreach (TabDefinition def in this.settingsTabsModel.Tabs)
		{
			TabHeader tabHeader = this.createTabHeader(def);
			this.tabHeaders.Add(tabHeader);
			this.setupTab(tabHeader, def);
		}
		this.tabHeadersDirty = true;
	}

	// Token: 0x0600414E RID: 16718 RVA: 0x00125D70 File Offset: 0x00124170
	private bool allowInteraction(SettingsTab tab)
	{
		return this._currentTabDisplay == tab;
	}

	// Token: 0x17000F5E RID: 3934
	// (get) Token: 0x0600414F RID: 16719 RVA: 0x00125D7B File Offset: 0x0012417B
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

	// Token: 0x06004150 RID: 16720 RVA: 0x00125D9C File Offset: 0x0012419C
	private void setupTab(TabHeader obj, TabDefinition def)
	{
		obj.transform.SetParent(this.SettingsTabHeaderAnchor.transform, false);
		SettingsTabElement component = UnityEngine.Object.Instantiate<GameObject>(def.prefab).GetComponent<SettingsTabElement>();
		this.elements[(SettingsTab)def.id] = component;
		component.Def = def;
		component.AllowInteraction = new Func<SettingsTab, bool>(this.allowInteraction);
		component.transform.SetParent(this.SettingsTabAnchor.transform, false);
		Vector3 localPosition = component.transform.localPosition;
		localPosition.x += base.screenWidth * (float)def.ordinal;
		component.transform.localPosition = localPosition;
		this.settingsTabsController.AddButton(obj.Button, delegate()
		{
			this.onTabClicked(obj);
		});
	}

	// Token: 0x06004151 RID: 16721 RVA: 0x00125E84 File Offset: 0x00124284
	private TabHeader createTabHeader(TabDefinition def)
	{
		TabHeader component = UnityEngine.Object.Instantiate<GameObject>(this.SettingsTabHeaderPrefab).GetComponent<TabHeader>();
		base.injector.Inject(component);
		component.Init(def);
		return component;
	}

	// Token: 0x06004152 RID: 16722 RVA: 0x00125EB8 File Offset: 0x001242B8
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

	// Token: 0x06004153 RID: 16723 RVA: 0x00125F0C File Offset: 0x0012430C
	private bool isAnyTabHeaderDirty()
	{
		foreach (TabHeader tabHeader in this.tabHeaders)
		{
			if (tabHeader.IsDirty)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004154 RID: 16724 RVA: 0x00125F78 File Offset: 0x00124378
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

	// Token: 0x06004155 RID: 16725 RVA: 0x00125FF8 File Offset: 0x001243F8
	private int calculateHeaderWidth()
	{
		int num = 0;
		foreach (TabHeader tabHeader in this.tabHeaders)
		{
			num += (int)tabHeader.GetComponent<RectTransform>().sizeDelta.x;
		}
		num += (int)this.SettingsTabHeaderAnchor.spacing * (this.tabHeaders.Count - 1);
		return num;
	}

	// Token: 0x06004156 RID: 16726 RVA: 0x00126084 File Offset: 0x00124484
	private void settingsDrawComplete()
	{
		if (!this.settingsDrawCompleted)
		{
			this.settingsDrawCompleted = true;
			this.SettingsTabHeaderAnchor.transform.localPosition = this.settingsTabAnchorOriginalPosition;
			this.alignTabInstructions();
			foreach (SettingsTabElement settingsTabElement in this.elements.Values)
			{
				settingsTabElement.OnDrawComplete();
			}
			base.onDrawComplete();
			base.lockInput();
			this.tweenIn();
		}
	}

	// Token: 0x17000F5F RID: 3935
	// (get) Token: 0x06004157 RID: 16727 RVA: 0x00126124 File Offset: 0x00124524
	public override bool WaitForDrawCallback
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06004158 RID: 16728 RVA: 0x00126127 File Offset: 0x00124527
	private void onTabClicked(TabHeader header)
	{
		this.setTab((SettingsTab)header.def.id);
	}

	// Token: 0x06004159 RID: 16729 RVA: 0x0012613A File Offset: 0x0012453A
	private void setTab(SettingsTab tab)
	{
		this.settingsTabsModel.Current = tab;
	}

	// Token: 0x0600415A RID: 16730 RVA: 0x00126148 File Offset: 0x00124548
	public override void OnRightTriggerPressed()
	{
		if (!this.currentTab.OnRightTriggerPressed())
		{
			this.settingsTabsModel.Shift(1);
		}
	}

	// Token: 0x0600415B RID: 16731 RVA: 0x00126174 File Offset: 0x00124574
	public override void OnLeftTriggerPressed()
	{
		if (!this.currentTab.OnLeftTriggerPressed())
		{
			this.settingsTabsModel.Shift(-1);
		}
	}

	// Token: 0x0600415C RID: 16732 RVA: 0x0012619F File Offset: 0x0012459F
	public override void OnRightStickLeft()
	{
		if (this.currentTab != null)
		{
			this.currentTab.OnRightStickLeft();
		}
	}

	// Token: 0x0600415D RID: 16733 RVA: 0x001261BE File Offset: 0x001245BE
	public override void OnRightStickRight()
	{
		if (this.currentTab != null)
		{
			this.currentTab.OnRightStickRight();
		}
	}

	// Token: 0x0600415E RID: 16734 RVA: 0x001261DD File Offset: 0x001245DD
	public override void OnDPadLeft()
	{
		this.currentTab.OnDPadLeft();
	}

	// Token: 0x0600415F RID: 16735 RVA: 0x001261EB File Offset: 0x001245EB
	public override void OnDPadRight()
	{
		this.currentTab.OnDPadRight();
	}

	// Token: 0x06004160 RID: 16736 RVA: 0x001261FC File Offset: 0x001245FC
	public override void OnCancelPressed()
	{
		if (!this.currentTab.OnCancelPressed())
		{
			this.GoToPreviousScreen();
		}
	}

	// Token: 0x06004161 RID: 16737 RVA: 0x00126224 File Offset: 0x00124624
	public override void GoToPreviousScreen()
	{
		if (this.controlsAPI.SettingsChanged)
		{
			if (this.controlsAPI.IsMovementUnbound)
			{
				GenericDialog genericDialog = base.dialogController.ShowTwoButtonDialog(base.localization.GetText("ui.settings.movementUnbound.title"), base.localization.GetText("ui.settings.movementUnbound.body"), base.localization.GetText("ui.settings.movementUnbound.confirm"), base.localization.GetText("ui.settings.movementUnbound.cancel"));
				GenericDialog genericDialog2 = genericDialog;
				genericDialog2.ConfirmCallback = (Action)Delegate.Combine(genericDialog2.ConfirmCallback, new Action(delegate()
				{
					this.<GoToPreviousScreen>__BaseCallProxy0();
				}));
			}
			else
			{
				GenericDialog genericDialog3 = base.dialogController.ShowTwoButtonDialog(base.localization.GetText("ui.settings.notSavedWarning.title"), base.localization.GetText("ui.settings.notSavedWarning.body"), base.localization.GetText("ui.settings.notSavedWarning.confirm"), base.localization.GetText("ui.settings.notSavedWarning.cancel"));
				GenericDialog genericDialog4 = genericDialog3;
				genericDialog4.ConfirmCallback = (Action)Delegate.Combine(genericDialog4.ConfirmCallback, new Action(delegate()
				{
					this.controlsAPI.SaveControls();
				}));
				GenericDialog genericDialog5 = genericDialog3;
				genericDialog5.CloseCallback = (Action)Delegate.Combine(genericDialog5.CloseCallback, new Action(delegate()
				{
					this.<GoToPreviousScreen>__BaseCallProxy0();
				}));
			}
		}
		else
		{
			base.GoToPreviousScreen();
		}
	}

	// Token: 0x06004162 RID: 16738 RVA: 0x0012635F File Offset: 0x0012475F
	public override void OnLeft()
	{
		this.currentTab.OnLeft();
	}

	// Token: 0x06004163 RID: 16739 RVA: 0x0012636C File Offset: 0x0012476C
	public override void OnRight()
	{
		this.currentTab.OnRight();
	}

	// Token: 0x06004164 RID: 16740 RVA: 0x00126379 File Offset: 0x00124779
	public override void OnUp()
	{
		this.currentTab.OnUp();
	}

	// Token: 0x06004165 RID: 16741 RVA: 0x00126386 File Offset: 0x00124786
	public override void OnDown()
	{
		this.currentTab.OnDown();
	}

	// Token: 0x06004166 RID: 16742 RVA: 0x00126393 File Offset: 0x00124793
	public override void OnYButtonPressed()
	{
		this.currentTab.OnYButtonPressed();
	}

	// Token: 0x06004167 RID: 16743 RVA: 0x001263A0 File Offset: 0x001247A0
	public override void OnXButtonPressed()
	{
		this.currentTab.OnXButtonPressed();
	}

	// Token: 0x06004168 RID: 16744 RVA: 0x001263AD File Offset: 0x001247AD
	public override void OnDestroy()
	{
		this.killTabTween();
		base.OnDestroy();
	}

	// Token: 0x04002BFF RID: 11263
	public GameObject Splash;

	// Token: 0x04002C00 RID: 11264
	public HorizontalLayoutGroup SettingsTabHeaderAnchor;

	// Token: 0x04002C01 RID: 11265
	public RectTransform SettingsTabsDivSeparator;

	// Token: 0x04002C02 RID: 11266
	public GameObject SettingsTabAnchor;

	// Token: 0x04002C03 RID: 11267
	public GameObject SettingsTabHeaderPrefab;

	// Token: 0x04002C04 RID: 11268
	public GameObject TweenInLeft;

	// Token: 0x04002C05 RID: 11269
	public GameObject TweenInRight;

	// Token: 0x04002C06 RID: 11270
	public GameObject GameplayScreenPrefab;

	// Token: 0x04002C07 RID: 11271
	public GameObject ControlsScreenPrefab;

	// Token: 0x04002C08 RID: 11272
	public GameObject VidoScreenPrefab;

	// Token: 0x04002C09 RID: 11273
	public GameObject AudioScreenPrefab;

	// Token: 0x04002C0A RID: 11274
	public Transform BackButtonAnchor;

	// Token: 0x04002C0B RID: 11275
	public Transform TabNavigationAnchorLeft;

	// Token: 0x04002C0C RID: 11276
	public Transform TabNavigationAnchorRight;

	// Token: 0x04002C0D RID: 11277
	public float TabInstructionsOffset = 54f;

	// Token: 0x04002C0E RID: 11278
	public GameObject BackButtonPrefab;

	// Token: 0x04002C0F RID: 11279
	public GameObject TabNavigationPrefabLeft;

	// Token: 0x04002C10 RID: 11280
	public GameObject TabNavigationPrefabRight;

	// Token: 0x04002C11 RID: 11281
	private MenuItemList settingsTabsController;

	// Token: 0x04002C12 RID: 11282
	private List<TabHeader> tabHeaders = new List<TabHeader>();

	// Token: 0x04002C13 RID: 11283
	private Dictionary<SettingsTab, SettingsTabElement> elements = new Dictionary<SettingsTab, SettingsTabElement>();

	// Token: 0x04002C14 RID: 11284
	private Vector3 settingsTabAnchorOriginalPosition;

	// Token: 0x04002C15 RID: 11285
	private bool settingsDrawCompleted;

	// Token: 0x04002C16 RID: 11286
	private bool tabHeadersDirty;

	// Token: 0x04002C17 RID: 11287
	private SettingsTab _currentTabDisplay;

	// Token: 0x04002C18 RID: 11288
	private Tweener _tabTween;

	// Token: 0x04002C19 RID: 11289
	private ScreenType previousScreen;
}
