using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A2F RID: 2607
public class StoreScreen : GameScreen
{
	// Token: 0x17001210 RID: 4624
	// (get) Token: 0x06004BF0 RID: 19440 RVA: 0x0014355E File Offset: 0x0014195E
	// (set) Token: 0x06004BF1 RID: 19441 RVA: 0x00143566 File Offset: 0x00141966
	[Inject]
	public IStoreTabsModel storeTabsModel { get; set; }

	// Token: 0x17001211 RID: 4625
	// (get) Token: 0x06004BF2 RID: 19442 RVA: 0x0014356F File Offset: 0x0014196F
	// (set) Token: 0x06004BF3 RID: 19443 RVA: 0x00143577 File Offset: 0x00141977
	[Inject]
	public IStoreAPI api { get; set; }

	// Token: 0x17001212 RID: 4626
	// (get) Token: 0x06004BF4 RID: 19444 RVA: 0x00143580 File Offset: 0x00141980
	// (set) Token: 0x06004BF5 RID: 19445 RVA: 0x00143588 File Offset: 0x00141988
	[Inject]
	public ICollectiblesTabAPI collectiblesTabAPI { get; set; }

	// Token: 0x17001213 RID: 4627
	// (get) Token: 0x06004BF6 RID: 19446 RVA: 0x00143591 File Offset: 0x00141991
	// (set) Token: 0x06004BF7 RID: 19447 RVA: 0x00143599 File Offset: 0x00141999
	[Inject]
	public IFeaturedTabAPI featuredTabAPI { get; set; }

	// Token: 0x17001214 RID: 4628
	// (get) Token: 0x06004BF8 RID: 19448 RVA: 0x001435A2 File Offset: 0x001419A2
	// (set) Token: 0x06004BF9 RID: 19449 RVA: 0x001435AA File Offset: 0x001419AA
	[Inject]
	public IRichPresence richPresence { get; set; }

	// Token: 0x17001215 RID: 4629
	// (get) Token: 0x06004BFA RID: 19450 RVA: 0x001435B3 File Offset: 0x001419B3
	protected override bool SupportsAutoJoin
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06004BFB RID: 19451 RVA: 0x001435B8 File Offset: 0x001419B8
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

	// Token: 0x06004BFC RID: 19452 RVA: 0x00143689 File Offset: 0x00141A89
	protected override void onAutoJoinRequest()
	{
		this.currentTab.OnCancelPressed();
		base.onAutoJoinRequest();
	}

	// Token: 0x06004BFD RID: 19453 RVA: 0x0014369D File Offset: 0x00141A9D
	private void mapPrefab(StoreTab id, GameObject prefab)
	{
		this.storeTabsModel.MapPrefab(id, prefab);
	}

	// Token: 0x06004BFE RID: 19454 RVA: 0x001436AC File Offset: 0x00141AAC
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

	// Token: 0x06004BFF RID: 19455 RVA: 0x001437E8 File Offset: 0x00141BE8
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

	// Token: 0x06004C00 RID: 19456 RVA: 0x00143850 File Offset: 0x00141C50
	private void tweenIn()
	{
		base.lockInput();
		this.deactivateUnusedTabs();
		base.tweenInItem(this.TweenInRight, 1, 0, 0.25f, 0f, null);
		base.tweenInItem(this.TweenInLeft, -1, 0, 0.25f, 0f, new Action(this.onAnimationsComplete));
	}

	// Token: 0x06004C01 RID: 19457 RVA: 0x001438A6 File Offset: 0x00141CA6
	private void onAnimationsComplete()
	{
		base.unlockInput();
	}

	// Token: 0x06004C02 RID: 19458 RVA: 0x001438AE File Offset: 0x00141CAE
	protected override void handleMenuSelectionOnButtonMode()
	{
	}

	// Token: 0x06004C03 RID: 19459 RVA: 0x001438B0 File Offset: 0x00141CB0
	public override void OnAnythingPressed()
	{
		base.OnAnythingPressed();
		this.updateMouseMode();
	}

	// Token: 0x06004C04 RID: 19460 RVA: 0x001438BE File Offset: 0x00141CBE
	public override void OnAnyMouseEvent()
	{
		base.OnAnyMouseEvent();
		this.updateMouseMode();
	}

	// Token: 0x06004C05 RID: 19461 RVA: 0x001438CC File Offset: 0x00141CCC
	public override void OnAnyNavigationButtonPressed()
	{
		base.OnAnyNavigationButtonPressed();
	}

	// Token: 0x06004C06 RID: 19462 RVA: 0x001438D4 File Offset: 0x00141CD4
	private void updateMouseMode()
	{
		foreach (KeyValuePair<StoreTab, StoreTabElement> keyValuePair in this.elements)
		{
			StoreTabElement value = keyValuePair.Value;
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

	// Token: 0x06004C07 RID: 19463 RVA: 0x001439B4 File Offset: 0x00141DB4
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
			this.fadeToBlack(1f, delegate
			{
				this.setCorrectCanvas();
				this.fadeToBlack(0f, delegate
				{
					this.FadeToBlack.gameObject.SetActive(false);
					base.unlockInput();
					this.storeScene.OnSceneTransitionComplete();
					this.syncMusic();
				});
			});
		}
	}

	// Token: 0x06004C08 RID: 19464 RVA: 0x00143A32 File Offset: 0x00141E32
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

	// Token: 0x06004C09 RID: 19465 RVA: 0x00143A64 File Offset: 0x00141E64
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

	// Token: 0x06004C0A RID: 19466 RVA: 0x00143B30 File Offset: 0x00141F30
	private void fadeToBlack(float alpha, Action callback)
	{
		float duration = 0.35f;
		this.killFadeToBlackTween();
		this.fadeToBlackTween = DOTween.To(() => this.FadeToBlack.alpha, delegate(float x)
		{
			this.FadeToBlack.alpha = x;
		}, alpha, duration).SetEase(Ease.OutSine).OnComplete(delegate
		{
			this.killFadeToBlackTween();
			callback();
		});
	}

	// Token: 0x06004C0B RID: 19467 RVA: 0x00143B99 File Offset: 0x00141F99
	private void killFadeToBlackTween()
	{
		TweenUtil.Destroy(ref this.fadeToBlackTween);
	}

	// Token: 0x06004C0C RID: 19468 RVA: 0x00143BA8 File Offset: 0x00141FA8
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

	// Token: 0x06004C0D RID: 19469 RVA: 0x00143C88 File Offset: 0x00142088
	private void updateHeaderState()
	{
		bool flag = this.api.Mode == StoreMode.NORMAL && this.storeTabsModel.Current == StoreTab.COLLECTIBLES && this.collectiblesTabAPI.GetState() == CollectiblesTabState.NetsukeEquipView;
		this.Header.SetActive(!flag);
	}

	// Token: 0x06004C0E RID: 19470 RVA: 0x00143CD8 File Offset: 0x001420D8
	private void updateTabHeader()
	{
		foreach (TabHeader tabHeader in this.tabHeaders)
		{
			tabHeader.SetCurrentlySelected(tabHeader.def.id == (int)this.storeTabsModel.Current);
			tabHeader.SetTitle(base.localization.GetText("ui.store.tab.title." + (StoreTab)tabHeader.def.id));
			tabHeader.ShowNotification(tabHeader.def.id == 1 && !this.featuredTabAPI.IsProAccountUnlocked());
		}
	}

	// Token: 0x06004C0F RID: 19471 RVA: 0x00143DA0 File Offset: 0x001421A0
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
			this._tabTween = DOTween.To(() => this.StoreTabAnchor.transform.localPosition, delegate(Vector3 valueIn)
			{
				this.StoreTabAnchor.transform.localPosition = valueIn;
			}, zero, duration).SetEase(Ease.OutExpo).OnComplete(new TweenCallback(this.onTabTweenComplete));
		}
	}

	// Token: 0x06004C10 RID: 19472 RVA: 0x00143EB9 File Offset: 0x001422B9
	private void onTabTweenComplete()
	{
		this.killTabTween();
		this.enableRaycastAllTabs();
		this.deactivateUnusedTabs();
	}

	// Token: 0x06004C11 RID: 19473 RVA: 0x00143ED0 File Offset: 0x001422D0
	private void disableRaycastAllTabs()
	{
		foreach (StoreTab key in this.elements.Keys)
		{
			this.elements[key].GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}

	// Token: 0x06004C12 RID: 19474 RVA: 0x00143F44 File Offset: 0x00142344
	private void enableRaycastAllTabs()
	{
		foreach (StoreTab key in this.elements.Keys)
		{
			this.elements[key].GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
	}

	// Token: 0x06004C13 RID: 19475 RVA: 0x00143FB8 File Offset: 0x001423B8
	private void activateAllTabs()
	{
		foreach (StoreTab key in this.elements.Keys)
		{
			this.elements[key].gameObject.SetActive(true);
		}
	}

	// Token: 0x06004C14 RID: 19476 RVA: 0x0014402C File Offset: 0x0014242C
	private void deactivateUnusedTabs()
	{
	}

	// Token: 0x06004C15 RID: 19477 RVA: 0x0014402E File Offset: 0x0014242E
	private void killTabTween()
	{
		if (this._tabTween != null && this._tabTween.IsPlaying())
		{
			this._tabTween.Kill(false);
		}
		this._tabTween = null;
	}

	// Token: 0x06004C16 RID: 19478 RVA: 0x00144060 File Offset: 0x00142460
	private void addTabHeaders()
	{
		foreach (TabDefinition def in this.storeTabsModel.Tabs)
		{
			TabHeader tabHeader = this.createTabHeader(def);
			this.tabHeaders.Add(tabHeader);
			this.setupTab(tabHeader, def);
		}
		this.tabHeadersDirty = true;
	}

	// Token: 0x06004C17 RID: 19479 RVA: 0x001440B4 File Offset: 0x001424B4
	private bool allowInteraction(StoreTab tab)
	{
		return this._currentTabDisplay == tab;
	}

	// Token: 0x17001216 RID: 4630
	// (get) Token: 0x06004C18 RID: 19480 RVA: 0x001440BF File Offset: 0x001424BF
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

	// Token: 0x06004C19 RID: 19481 RVA: 0x001440E0 File Offset: 0x001424E0
	private void setupTab(TabHeader obj, TabDefinition def)
	{
		obj.transform.SetParent(this.StoreTabHeaderAnchor.transform, false);
		StoreTabElement component = UnityEngine.Object.Instantiate<GameObject>(def.prefab).GetComponent<StoreTabElement>();
		this.elements[(StoreTab)def.id] = component;
		component.Def = def;
		component.AllowInteraction = new Func<StoreTab, bool>(this.allowInteraction);
		component.transform.SetParent(this.StoreTabAnchor.transform, false);
		Vector3 localPosition = component.transform.localPosition;
		localPosition.x += base.screenWidth * (float)def.ordinal;
		component.transform.localPosition = localPosition;
		this.storeTabsController.AddButton(obj.Button, delegate()
		{
			this.onTabClicked(obj);
		});
	}

	// Token: 0x06004C1A RID: 19482 RVA: 0x001441C8 File Offset: 0x001425C8
	private TabHeader createTabHeader(TabDefinition def)
	{
		TabHeader component = UnityEngine.Object.Instantiate<GameObject>(this.StoreTabHeaderPrefab).GetComponent<TabHeader>();
		base.injector.Inject(component);
		component.Init(def);
		return component;
	}

	// Token: 0x06004C1B RID: 19483 RVA: 0x001441FC File Offset: 0x001425FC
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

	// Token: 0x06004C1C RID: 19484 RVA: 0x00144250 File Offset: 0x00142650
	private bool isAnyTabHeaderDirty()
	{
		if (this.storeTabsModel.ShowTabs)
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
		return false;
	}

	// Token: 0x06004C1D RID: 19485 RVA: 0x001442CC File Offset: 0x001426CC
	private void storeDrawComplete()
	{
		if (!this.storeDrawCompleted)
		{
			this.storeDrawCompleted = true;
			this.StoreTabHeaderAnchor.transform.localPosition = this.storeTabAnchorOriginalPosition;
			this.alignTabInstructions();
			foreach (StoreTabElement storeTabElement in this.elements.Values)
			{
				storeTabElement.OnDrawComplete();
			}
			base.onDrawComplete();
			this.tweenIn();
		}
	}

	// Token: 0x06004C1E RID: 19486 RVA: 0x00144368 File Offset: 0x00142768
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

	// Token: 0x06004C1F RID: 19487 RVA: 0x001443E8 File Offset: 0x001427E8
	private int calculateHeaderWidth()
	{
		int num = 0;
		foreach (TabHeader tabHeader in this.tabHeaders)
		{
			num += (int)tabHeader.GetComponent<RectTransform>().sizeDelta.x;
		}
		num += (int)this.StoreTabHeaderAnchor.spacing * (this.tabHeaders.Count - 1);
		return num;
	}

	// Token: 0x17001217 RID: 4631
	// (get) Token: 0x06004C20 RID: 19488 RVA: 0x00144474 File Offset: 0x00142874
	public override bool WaitForDrawCallback
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06004C21 RID: 19489 RVA: 0x00144477 File Offset: 0x00142877
	private void onTabClicked(TabHeader header)
	{
		this.setTab((StoreTab)header.def.id);
	}

	// Token: 0x06004C22 RID: 19490 RVA: 0x0014448A File Offset: 0x0014288A
	private void setTab(StoreTab tab)
	{
		this.storeTabsModel.Current = tab;
	}

	// Token: 0x06004C23 RID: 19491 RVA: 0x00144498 File Offset: 0x00142898
	public override void OnRightTriggerPressed()
	{
		if (this.api.Mode == StoreMode.NORMAL && !this.currentTab.OnRightTriggerPressed())
		{
			this.storeTabsModel.Shift(1);
		}
	}

	// Token: 0x06004C24 RID: 19492 RVA: 0x001444D4 File Offset: 0x001428D4
	public override void OnLeftTriggerPressed()
	{
		if (this.api.Mode == StoreMode.NORMAL && !this.currentTab.OnLeftTriggerPressed())
		{
			this.storeTabsModel.Shift(-1);
		}
	}

	// Token: 0x06004C25 RID: 19493 RVA: 0x0014450F File Offset: 0x0014290F
	public override void OnRightStickLeft()
	{
		this.currentTab.OnRightStickLeft();
	}

	// Token: 0x06004C26 RID: 19494 RVA: 0x0014451D File Offset: 0x0014291D
	public override void OnRightStickRight()
	{
		this.currentTab.OnRightStickRight();
	}

	// Token: 0x06004C27 RID: 19495 RVA: 0x0014452B File Offset: 0x0014292B
	public override void OnRightStickUp()
	{
		this.currentTab.OnRightStickUp();
	}

	// Token: 0x06004C28 RID: 19496 RVA: 0x00144538 File Offset: 0x00142938
	public override void OnRightStickDown()
	{
		this.currentTab.OnRightStickDown();
	}

	// Token: 0x06004C29 RID: 19497 RVA: 0x00144545 File Offset: 0x00142945
	public override void OnDPadLeft()
	{
		this.currentTab.OnDPadLeft();
	}

	// Token: 0x06004C2A RID: 19498 RVA: 0x00144553 File Offset: 0x00142953
	public override void OnDPadRight()
	{
		this.currentTab.OnDPadRight();
	}

	// Token: 0x06004C2B RID: 19499 RVA: 0x00144561 File Offset: 0x00142961
	public override void UpdateRightStick(float x, float y)
	{
		if (this.currentTab != null)
		{
			this.currentTab.UpdateRightStick(x, y);
		}
	}

	// Token: 0x06004C2C RID: 19500 RVA: 0x00144584 File Offset: 0x00142984
	public override void OnCancelPressed()
	{
		bool flag = this.currentTab.OnCancelPressed();
		this.tryGoBack(ref flag);
	}

	// Token: 0x06004C2D RID: 19501 RVA: 0x001445A8 File Offset: 0x001429A8
	protected override void onBackButtonClicked(InputEventData inputEventData)
	{
		bool flag = this.currentTab.OnBackButtonClicked();
		this.tryGoBack(ref flag);
	}

	// Token: 0x06004C2E RID: 19502 RVA: 0x001445C9 File Offset: 0x001429C9
	private void tryGoBack(ref bool usedInput)
	{
		if (!usedInput)
		{
			this.richPresence.SetPresence(null, null, null, null);
			this.GoToPreviousScreen();
		}
	}

	// Token: 0x06004C2F RID: 19503 RVA: 0x001445E7 File Offset: 0x001429E7
	public override void OnSubmitPressed()
	{
		this.currentTab.OnSubmitPressed();
	}

	// Token: 0x06004C30 RID: 19504 RVA: 0x001445F4 File Offset: 0x001429F4
	public override void OnLeftBumperPressed()
	{
		this.currentTab.OnLeftBumperPressed();
	}

	// Token: 0x06004C31 RID: 19505 RVA: 0x00144601 File Offset: 0x00142A01
	public override void OnZPressed()
	{
		this.currentTab.OnZPressed();
	}

	// Token: 0x06004C32 RID: 19506 RVA: 0x0014460E File Offset: 0x00142A0E
	public override void OnLeft()
	{
		this.currentTab.OnLeft();
	}

	// Token: 0x06004C33 RID: 19507 RVA: 0x0014461B File Offset: 0x00142A1B
	public override void OnRight()
	{
		this.currentTab.OnRight();
	}

	// Token: 0x06004C34 RID: 19508 RVA: 0x00144628 File Offset: 0x00142A28
	public override void OnUp()
	{
		this.currentTab.OnUp();
	}

	// Token: 0x06004C35 RID: 19509 RVA: 0x00144635 File Offset: 0x00142A35
	public override void OnDown()
	{
		this.currentTab.OnDown();
	}

	// Token: 0x06004C36 RID: 19510 RVA: 0x00144642 File Offset: 0x00142A42
	public override void OnYButtonPressed()
	{
		this.currentTab.OnYButtonPressed();
	}

	// Token: 0x06004C37 RID: 19511 RVA: 0x0014464F File Offset: 0x00142A4F
	public override void OnDestroy()
	{
		this.killTabTween();
		base.OnDestroy();
	}

	// Token: 0x040031EB RID: 12779
	public HorizontalLayoutGroup StoreTabHeaderAnchor;

	// Token: 0x040031EC RID: 12780
	public RectTransform StoreTabsDivSeparator;

	// Token: 0x040031ED RID: 12781
	public GameObject StoreTabAnchor;

	// Token: 0x040031EE RID: 12782
	public GameObject StoreTabHeaderPrefab;

	// Token: 0x040031EF RID: 12783
	public GameObject TweenInLeft;

	// Token: 0x040031F0 RID: 12784
	public GameObject TweenInRight;

	// Token: 0x040031F1 RID: 12785
	public GameObject Header;

	// Token: 0x040031F2 RID: 12786
	public GameObject FeaturedScreenPrefab;

	// Token: 0x040031F3 RID: 12787
	public GameObject LootBoxScreenPrefab;

	// Token: 0x040031F4 RID: 12788
	public GameObject BundlesScreenPrefab;

	// Token: 0x040031F5 RID: 12789
	public GameObject CharactersScreenPrefab;

	// Token: 0x040031F6 RID: 12790
	public GameObject CollectiblesScreenPrefab;

	// Token: 0x040031F7 RID: 12791
	public Transform BackButtonAnchor;

	// Token: 0x040031F8 RID: 12792
	public Transform UnboxingBackButtonStub;

	// Token: 0x040031F9 RID: 12793
	public Transform TabNavigationAnchorLeft;

	// Token: 0x040031FA RID: 12794
	public Transform TabNavigationAnchorRight;

	// Token: 0x040031FB RID: 12795
	public float TabInstructionsOffset = 54f;

	// Token: 0x040031FC RID: 12796
	public GameObject BackButtonPrefab;

	// Token: 0x040031FD RID: 12797
	public GameObject TabNavigationPrefabLeft;

	// Token: 0x040031FE RID: 12798
	public GameObject TabNavigationPrefabRight;

	// Token: 0x040031FF RID: 12799
	public CanvasGroup MainCanvas;

	// Token: 0x04003200 RID: 12800
	public CanvasGroup UnboxingCanvas;

	// Token: 0x04003201 RID: 12801
	public CanvasGroup FadeToBlack;

	// Token: 0x04003202 RID: 12802
	public TextMeshProUGUI Title;

	// Token: 0x04003203 RID: 12803
	private InputInstructions tabInstructionsLeft;

	// Token: 0x04003204 RID: 12804
	private InputInstructions tabInstructionsRight;

	// Token: 0x04003205 RID: 12805
	private MenuItemList storeTabsController;

	// Token: 0x04003206 RID: 12806
	private List<TabHeader> tabHeaders = new List<TabHeader>();

	// Token: 0x04003207 RID: 12807
	private Dictionary<StoreTab, StoreTabElement> elements = new Dictionary<StoreTab, StoreTabElement>(default(StoreTabComparer));

	// Token: 0x04003208 RID: 12808
	private Vector3 storeTabAnchorOriginalPosition;

	// Token: 0x04003209 RID: 12809
	private bool storeDrawCompleted;

	// Token: 0x0400320A RID: 12810
	private bool tabHeadersDirty;

	// Token: 0x0400320B RID: 12811
	private StoreTab _currentTabDisplay;

	// Token: 0x0400320C RID: 12812
	private Tweener _tabTween;

	// Token: 0x0400320D RID: 12813
	private Tweener fadeToBlackTween;

	// Token: 0x0400320E RID: 12814
	private StoreMode currentMode;

	// Token: 0x0400320F RID: 12815
	private StoreScene3D storeScene;
}
