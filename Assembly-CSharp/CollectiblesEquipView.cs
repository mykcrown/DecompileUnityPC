using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

// Token: 0x020009EA RID: 2538
public class CollectiblesEquipView : StoreTabWithEquipModule
{
	// Token: 0x17001148 RID: 4424
	// (get) Token: 0x06004823 RID: 18467 RVA: 0x00138D83 File Offset: 0x00137183
	// (set) Token: 0x06004824 RID: 18468 RVA: 0x00138D8B File Offset: 0x0013718B
	[Inject]
	public ICollectiblesEquipViewAPI api { get; set; }

	// Token: 0x17001149 RID: 4425
	// (get) Token: 0x06004825 RID: 18469 RVA: 0x00138D94 File Offset: 0x00137194
	// (set) Token: 0x06004826 RID: 18470 RVA: 0x00138D9C File Offset: 0x0013719C
	[Inject("CollectiblesEquipView")]
	public IEquipModuleAPI equipModuleAPI { get; set; }

	// Token: 0x1700114A RID: 4426
	// (get) Token: 0x06004827 RID: 18471 RVA: 0x00138DA5 File Offset: 0x001371A5
	// (set) Token: 0x06004828 RID: 18472 RVA: 0x00138DAD File Offset: 0x001371AD
	[Inject]
	public ICollectiblesTabAPI collectiblesTabAPI { get; set; }

	// Token: 0x1700114B RID: 4427
	// (get) Token: 0x06004829 RID: 18473 RVA: 0x00138DB6 File Offset: 0x001371B6
	// (set) Token: 0x0600482A RID: 18474 RVA: 0x00138DBE File Offset: 0x001371BE
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x1700114C RID: 4428
	// (get) Token: 0x0600482B RID: 18475 RVA: 0x00138DC7 File Offset: 0x001371C7
	// (set) Token: 0x0600482C RID: 18476 RVA: 0x00138DCF File Offset: 0x001371CF
	[Inject]
	public IUserGlobalEquippedModel globalEquippedModel { get; set; }

	// Token: 0x1700114D RID: 4429
	// (get) Token: 0x0600482D RID: 18477 RVA: 0x00138DD8 File Offset: 0x001371D8
	// (set) Token: 0x0600482E RID: 18478 RVA: 0x00138DE0 File Offset: 0x001371E0
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x1700114E RID: 4430
	// (get) Token: 0x0600482F RID: 18479 RVA: 0x00138DE9 File Offset: 0x001371E9
	// (set) Token: 0x06004830 RID: 18480 RVA: 0x00138DF1 File Offset: 0x001371F1
	[Inject]
	public IStoreTabsModel storeTabAPI { get; set; }

	// Token: 0x1700114F RID: 4431
	// (get) Token: 0x06004831 RID: 18481 RVA: 0x00138DFA File Offset: 0x001371FA
	// (set) Token: 0x06004832 RID: 18482 RVA: 0x00138E02 File Offset: 0x00137202
	[Inject]
	public IStoreAPI storeAPI { get; set; }

	// Token: 0x17001150 RID: 4432
	// (get) Token: 0x06004833 RID: 18483 RVA: 0x00138E0B File Offset: 0x0013720B
	// (set) Token: 0x06004834 RID: 18484 RVA: 0x00138E13 File Offset: 0x00137213
	[Inject]
	public ICharactersTabAPI charactersTabAPI { get; set; }

	// Token: 0x06004835 RID: 18485 RVA: 0x00138E1C File Offset: 0x0013721C
	[PostConstruct]
	public override void Init()
	{
		base.Init();
		this.storeScene = base.uiAdapter.GetUIScene<StoreScene3D>();
		this.MainMode.Init(new Action<MenuItemButton, InputEventData>(this.clickedPreviewItemButton), this.storeScene);
		this.GridMode.Init(new Action<MenuItemButton, InputEventData>(this.clickedPreviewItemButton), this.storeScene);
		this.configureModules();
		this.mapSpriteToEquip(EquipmentTypes.ANNOUNCERS, this.AnnouncersIcon);
		this.mapSpriteToEquip(EquipmentTypes.BLAST_ZONE, this.BlastZonesIcon);
		this.mapSpriteToEquip(EquipmentTypes.LOADING_SCREEN, this.LoadingScreenIcon);
		this.mapSpriteToEquip(EquipmentTypes.NETSUKE, this.NetsukeIcon);
		this.mapSpriteToEquip(EquipmentTypes.PLAYER_ICON, this.PlayerProfilesIcon);
		this.mapSpriteToEquip(EquipmentTypes.TOKEN, this.TokenIcon);
		this.equipModuleAPI.SelectedEquipType = EquipmentTypes.NETSUKE;
		this.equipModuleAPI.LoadTypeList(this.api.GetValidEquipTypes());
		this.addEquipment();
		this.EquipModule.ListFilter = new Func<List<EquipmentLine>, List<EquipmentLine>>(this.listFilter);
		this.GridModule.ListFilter = new Func<List<EquipmentLine>, List<EquipmentLine>>(this.listFilter);
		CursorTargetButton charactersArrowButton = this.CharactersArrowButton;
		charactersArrowButton.ClickCallback = (Action<CursorTargetButton>)Delegate.Combine(charactersArrowButton.ClickCallback, new Action<CursorTargetButton>(this.onCharactersArrowClicked));
		this.initialPosition = base.transform.localPosition;
		base.listen(EquipmentSelectorAPI.UPDATED, new Action(this.onDataUpdate));
		base.listen("CollectiblesTabAPI.UPDATED", new Action(this.onDataUpdate));
		base.listen("StoreTabSelectionModel.STORE_TAB_UPDATED", new Action(this.onTabUpdate));
		base.listen(EquipmentModel.UPDATED, new Action(this.onDataUpdate));
		base.listen(EquipFlow.NETSUKE, new Action(this.onNetsukeEquipRequested));
	}

	// Token: 0x06004836 RID: 18486 RVA: 0x00138FD2 File Offset: 0x001373D2
	private void onNetsukeEquipRequested()
	{
		base.audioManager.PlayMenuSound(SoundKey.store_itemEquip, 0f);
		this.collectiblesTabAPI.SetState(CollectiblesTabState.NetsukeEquipView, false);
	}

	// Token: 0x06004837 RID: 18487 RVA: 0x00138FF4 File Offset: 0x001373F4
	private void configureModules()
	{
		base.injector.Inject(this.GridModule);
		this.module = new TwoModuleManager();
		(this.module as TwoModuleManager).AddModule(this.EquipModule);
		(this.module as TwoModuleManager).AddModule(this.GridModule);
		this.GridModule.itemDisplayMode = EquipmentSelectorModule.ItemDisplay.GRID;
		this.switchToModule(ModuleMode.GRID);
	}

	// Token: 0x06004838 RID: 18488 RVA: 0x0013905C File Offset: 0x0013745C
	private void switchToModule(ModuleMode mode)
	{
		if (this.currentModuleMode != mode)
		{
			ModuleMode moduleMode = this.currentModuleMode;
			EquipmentSelectorModule equipmentSelectorModule = (mode != ModuleMode.MAIN) ? this.GridModule : this.EquipModule;
			equipmentSelectorModule.Activate();
			(this.module as TwoModuleManager).SetActive(equipmentSelectorModule);
			this.MainMode.gameObject.SetActive(true);
			this.GridMode.gameObject.SetActive(true);
			if (mode == ModuleMode.MAIN)
			{
				this.animateTransition(this.GridMode, this.MainMode);
			}
			else
			{
				this.animateTransition(this.MainMode, this.GridMode);
			}
			this.currentModuleMode = mode;
			if (moduleMode != ModuleMode.UNINITIALIZED)
			{
				this.timer.EndOfFrame(new Action(this.syncModuleAtEndOfFrame));
			}
		}
	}

	// Token: 0x06004839 RID: 18489 RVA: 0x00139124 File Offset: 0x00137524
	private void animateTransition(CollectiblesModuleMode from, CollectiblesModuleMode to)
	{
		float duration = 0.07f;
		this.killFromModuleTweens();
		to.transform.localPosition = Vector3.zero;
		to.Activate();
		this._fromModuleTween = DOTween.To(() => from.CanvasGroup.alpha, delegate(float valueIn)
		{
			from.CanvasGroup.alpha = valueIn;
		}, 0f, duration).SetEase(Ease.Linear);
		this._toModuleTween = DOTween.To(() => to.CanvasGroup.alpha, delegate(float valueIn)
		{
			to.CanvasGroup.alpha = valueIn;
		}, 1f, duration).SetEase(Ease.Linear).OnComplete(delegate
		{
			from.transform.localPosition = new Vector3(100000f, 0f, 0f);
			this.killFromModuleTweens();
		});
	}

	// Token: 0x0600483A RID: 18490 RVA: 0x001391E7 File Offset: 0x001375E7
	private void killFromModuleTweens()
	{
		TweenUtil.Destroy(ref this._fromModuleTween);
		TweenUtil.Destroy(ref this._toModuleTween);
	}

	// Token: 0x0600483B RID: 18491 RVA: 0x001391FF File Offset: 0x001375FF
	protected override bool isEquipViewActive()
	{
		return this.collectiblesTabAPI.GetState() == CollectiblesTabState.EquipView;
	}

	// Token: 0x0600483C RID: 18492 RVA: 0x00139210 File Offset: 0x00137610
	private void syncModuleAtEndOfFrame()
	{
		EquipmentSelectorModule current = (this.module as TwoModuleManager).GetCurrent();
		current.ForceSyncButtonSelection();
		current.ForceRedraws();
	}

	// Token: 0x0600483D RID: 18493 RVA: 0x0013923A File Offset: 0x0013763A
	private void mapSpriteToEquip(EquipmentTypes id, Sprite icon)
	{
		this.equipModuleAPI.MapEquipTypeIcon(id, icon);
	}

	// Token: 0x0600483E RID: 18494 RVA: 0x00139249 File Offset: 0x00137649
	private bool shouldCollectibleTypeSpin(EquipmentTypes equipmentType)
	{
		return !CollectiblesEquipView.nonSpinnedEquipmentTypes.Contains(equipmentType);
	}

	// Token: 0x0600483F RID: 18495 RVA: 0x00139259 File Offset: 0x00137659
	public void OnDrawComplete()
	{
		this.module.OnDrawComplete();
		this.onDataUpdate();
	}

	// Token: 0x06004840 RID: 18496 RVA: 0x0013926C File Offset: 0x0013766C
	private void addEquipment()
	{
		this.EquipModule.Init(this.equipModuleAPI);
		this.GridModule.Init(this.equipModuleAPI);
		List<EquippableItem> list = new List<EquippableItem>();
		List<EquippableItem> list2 = new List<EquippableItem>();
		foreach (EquipTypeDefinition equipTypeDefinition in this.equipModuleAPI.GetValidEquipTypes())
		{
			foreach (EquippableItem item in this.api.GetItems(equipTypeDefinition.type))
			{
				if (CollectiblesEquipView.gridModeEquipmentTypes.Contains(equipTypeDefinition.type))
				{
					list2.Add(item);
				}
				else
				{
					list.Add(item);
				}
			}
		}
		this.EquipModule.LoadItems(list);
		this.GridModule.LoadItems(list2);
	}

	// Token: 0x06004841 RID: 18497 RVA: 0x00139345 File Offset: 0x00137745
	private void Update()
	{
		this.DisabledGallerySpinErrorContainer.SetActive(UIInputModule.DisableMenuRightStick);
	}

	// Token: 0x06004842 RID: 18498 RVA: 0x00139357 File Offset: 0x00137757
	private void onDataUpdate()
	{
		this.updateModule();
		this.updateAssetPreview();
		this.updateTabMode();
	}

	// Token: 0x06004843 RID: 18499 RVA: 0x0013936C File Offset: 0x0013776C
	private void updateModule()
	{
		ModuleMode mode = (!CollectiblesEquipView.gridModeEquipmentTypes.Contains(this.equipModuleAPI.SelectedEquipType)) ? ModuleMode.MAIN : ModuleMode.GRID;
		this.switchToModule(mode);
	}

	// Token: 0x06004844 RID: 18500 RVA: 0x001393A4 File Offset: 0x001377A4
	private void updateAssetPreview()
	{
		this.alignDisplays();
		if (this.equipModuleAPI.SelectedEquipType == EquipmentTypes.NETSUKE)
		{
			if (this.api.GetNetsukeFromItem(this.equipModuleAPI.SelectedEquipment) != null)
			{
				this.storeScene.ShowNetsuke(this.equipModuleAPI.SelectedEquipment);
			}
			else
			{
				this.storeScene.HideCollectiblesObject();
			}
		}
		else if (this.equipModuleAPI.SelectedEquipType == EquipmentTypes.TOKEN)
		{
			if (this.equipModuleAPI.SelectedEquipment == null)
			{
				EquipmentID equippedByType = this.globalEquippedModel.GetEquippedByType(EquipmentTypes.TOKEN, this.storeAPI.Port);
				if (!equippedByType.IsNull())
				{
					EquippableItem item = this.equipmentModel.GetItem(equippedByType);
					this.storeScene.ShowPlayerToken(item);
				}
				else
				{
					this.storeScene.ShowPlayerToken(this.equipmentModel.GetDefaultItem(EquipmentTypes.TOKEN));
				}
			}
			else if (this.api.GetPlayerTokenFromItem(this.equipModuleAPI.SelectedEquipment) != null)
			{
				this.storeScene.ShowPlayerToken(this.equipModuleAPI.SelectedEquipment);
			}
			else
			{
				this.storeScene.ShowPlayerToken(this.equipmentModel.GetDefaultItem(EquipmentTypes.TOKEN));
			}
		}
		else if (this.equipModuleAPI.SelectedEquipType == EquipmentTypes.PLAYER_ICON)
		{
			if (this.equipModuleAPI.SelectedEquipment == null)
			{
				EquipmentID equippedByType2 = this.globalEquippedModel.GetEquippedByType(EquipmentTypes.PLAYER_ICON, this.storeAPI.Port);
				if (!equippedByType2.IsNull())
				{
					EquippableItem item2 = this.equipmentModel.GetItem(equippedByType2);
					this.storeScene.ShowPlayerCardIcon(item2);
				}
				else
				{
					this.storeScene.ShowPlayerCardIcon(this.equipmentModel.GetDefaultItem(EquipmentTypes.PLAYER_ICON));
				}
			}
			else if (this.api.GetPlayerIconDataFromItem(this.equipModuleAPI.SelectedEquipment) != null)
			{
				this.storeScene.ShowPlayerCardIcon(this.equipModuleAPI.SelectedEquipment);
			}
			else
			{
				this.storeScene.ShowPlayerCardIcon(this.equipmentModel.GetDefaultItem(EquipmentTypes.PLAYER_ICON));
			}
		}
		if (CollectiblesEquipView.gridModeEquipmentTypes.Contains(this.equipModuleAPI.SelectedEquipType) && this.equipModuleAPI.SelectedEquipment != null)
		{
			string text = this.equipModuleAPI.SelectedEquipment.backupNameText;
			if (text != null)
			{
				text = text.ToUpper();
			}
			this.GridMode.ItemTitle.text = ((text != null) ? text : string.Empty);
			this.GridMode.ItemTitle.gameObject.SetActive(true);
		}
		else
		{
			this.GridMode.ItemTitle.gameObject.SetActive(false);
		}
	}

	// Token: 0x06004845 RID: 18501 RVA: 0x00139661 File Offset: 0x00137A61
	private void onTabUpdate()
	{
		this.updateTabMode();
	}

	// Token: 0x06004846 RID: 18502 RVA: 0x0013966C File Offset: 0x00137A6C
	private void updateTabMode()
	{
		if (this.ZoomAnimator.gameObject.activeInHierarchy)
		{
			this.ZoomAnimator.SetBool("initialize", true);
			this.ZoomAnimator.SetBool("skip animation", this.collectiblesTabAPI.SkipAnimation);
			this.ZoomAnimator.SetBool("netsukeview", false);
			if (this.collectiblesTabAPI.GetState() == CollectiblesTabState.EquipView)
			{
				this.ZoomAnimator.SetBool("visible", true);
				if (this.previousStateTransition != CollectiblesTabState.EquipView)
				{
					this.showBeforeTransition();
				}
				this.timer.CancelTimeout(new Action(this.zoomGoDeeperComplete));
			}
			else if (this.collectiblesTabAPI.GetState() == CollectiblesTabState.NetsukeEquipView)
			{
				this.ZoomAnimator.SetBool("netsukeview", true);
				this.ZoomAnimator.SetBool("visible", false);
				base.transform.localPosition = this.initialPosition;
				this.timer.SetTimeout(200, new Action(this.zoomGoDeeperComplete));
			}
			else
			{
				this.ZoomAnimator.SetBool("visible", false);
				base.transform.localPosition = this.initialPosition + new Vector3(0f, 2000f, 0f);
				this.timer.CancelTimeout(new Action(this.zoomGoDeeperComplete));
			}
			this.previousStateTransition = this.collectiblesTabAPI.GetState();
		}
	}

	// Token: 0x06004847 RID: 18503 RVA: 0x001397E3 File Offset: 0x00137BE3
	private void zoomGoDeeperComplete()
	{
		base.transform.localPosition = new Vector3(100000f, 0f, 0f);
	}

	// Token: 0x06004848 RID: 18504 RVA: 0x00139804 File Offset: 0x00137C04
	private void showBeforeTransition()
	{
		base.transform.localPosition = this.initialPosition;
		this.storeScene.SetShadowMode(false, true);
		this.timer.SetTimeout(150, new Action(this.onShowComplete));
	}

	// Token: 0x06004849 RID: 18505 RVA: 0x00139840 File Offset: 0x00137C40
	private void onShowComplete()
	{
		this.storeScene.SetShadowMode(true, false);
	}

	// Token: 0x0600484A RID: 18506 RVA: 0x00139850 File Offset: 0x00137C50
	private void LateUpdate()
	{
		if (this.NetsukeFadeout != this._netsukeFadeout)
		{
			this._netsukeFadeout = this.NetsukeFadeout;
			this.storeScene.FadeOutCollectible(this._netsukeFadeout);
		}
		if (this.UIContainer.localScale != this._transformScale)
		{
			this._transformScale = this.UIContainer.localScale;
			this.storeScene.ScaleCollectible(this._transformScale);
		}
	}

	// Token: 0x0600484B RID: 18507 RVA: 0x001398C8 File Offset: 0x00137CC8
	private void alignDisplays()
	{
		if (this.equipModuleAPI.SelectedEquipType != EquipmentTypes.NONE)
		{
			this.alignTransform(this.MainMode.ItemDisplay);
			this.alignTransform(this.GridMode.ItemDisplay);
		}
	}

	// Token: 0x0600484C RID: 18508 RVA: 0x00139900 File Offset: 0x00137D00
	private void alignTransform(Transform theTransform)
	{
		GalleryDisplayParameters galleryParameters = base.config.storeSettings.GetGalleryParameters(this.equipModuleAPI.SelectedEquipType);
		if (galleryParameters != null)
		{
			theTransform.localPosition = galleryParameters.position;
		}
		else
		{
			theTransform.localPosition = Vector3.zero;
		}
	}

	// Token: 0x0600484D RID: 18509 RVA: 0x0013994B File Offset: 0x00137D4B
	public void PlayTransition()
	{
		this.ZoomAnimator.SetBool("visible", true);
		base.transform.localPosition = this.initialPosition;
	}

	// Token: 0x0600484E RID: 18510 RVA: 0x0013996F File Offset: 0x00137D6F
	public void ReleaseSelections()
	{
		this.module.ReleaseSelections();
	}

	// Token: 0x0600484F RID: 18511 RVA: 0x0013997C File Offset: 0x00137D7C
	public void OnStateActivate()
	{
		this.module.Activate();
		this.module.BeginMenuFocus();
	}

	// Token: 0x06004850 RID: 18512 RVA: 0x00139994 File Offset: 0x00137D94
	public bool OnCancelPressed()
	{
		return this.isEquipModuleFocused() && this.module.OnCancelPressed();
	}

	// Token: 0x06004851 RID: 18513 RVA: 0x001399AE File Offset: 0x00137DAE
	public bool OnLeft()
	{
		if (this.isEquipModuleFocused())
		{
			this.module.OnLeft();
			return true;
		}
		return false;
	}

	// Token: 0x06004852 RID: 18514 RVA: 0x001399CA File Offset: 0x00137DCA
	public override void UpdateRightStick(float x, float y)
	{
		if (this.shouldCollectibleTypeSpin(this.equipModuleAPI.SelectedEquipType) && x != 0f)
		{
			this.storeScene.SpinItemManual(StoreScene3D.ItemType.COLLECTIBLE, x);
		}
	}

	// Token: 0x06004853 RID: 18515 RVA: 0x001399FA File Offset: 0x00137DFA
	private void clickedPreviewItemButton(MenuItemButton button, InputEventData eventData)
	{
		if (this.shouldCollectibleTypeSpin(this.equipModuleAPI.SelectedEquipType))
		{
			this.spinItem();
		}
		this.storeScene.OnItemClick(StoreScene3D.ItemType.COLLECTIBLE);
	}

	// Token: 0x06004854 RID: 18516 RVA: 0x00139A24 File Offset: 0x00137E24
	private void spinItem()
	{
		this.storeScene.SpinItem(StoreScene3D.ItemType.COLLECTIBLE);
	}

	// Token: 0x06004855 RID: 18517 RVA: 0x00139A32 File Offset: 0x00137E32
	private void onCharactersArrowClicked(CursorTargetButton button)
	{
		this.storeTabAPI.Current = StoreTab.CHARACTERS;
		this.charactersTabAPI.SetState(CharactersTabState.EquipView, true);
	}

	// Token: 0x06004856 RID: 18518 RVA: 0x00139A4D File Offset: 0x00137E4D
	private void OnEnable()
	{
		this.onTabUpdate();
	}

	// Token: 0x06004857 RID: 18519 RVA: 0x00139A55 File Offset: 0x00137E55
	public override void OnDestroy()
	{
		if (this.storeScene != null)
		{
			this.storeScene.HideCollectiblesObject();
		}
		base.OnDestroy();
	}

	// Token: 0x06004858 RID: 18520 RVA: 0x00139A7C File Offset: 0x00137E7C
	private List<EquipmentLine> listFilter(List<EquipmentLine> inlist)
	{
		List<EquipmentLine> list = new List<EquipmentLine>();
		foreach (EquipmentLine equipmentLine in inlist)
		{
			bool flag = (this.equipModuleAPI.HasPrice(equipmentLine.Item) && equipmentLine.Item.promoted) || this.equipModuleAPI.HasItem(equipmentLine.Item.id);
			if (flag)
			{
				list.Add(equipmentLine);
			}
		}
		return list;
	}

	// Token: 0x04002FBA RID: 12218
	public const string INJECTION_TAG = "CollectiblesEquipView";

	// Token: 0x04002FC4 RID: 12228
	public Animator ZoomAnimator;

	// Token: 0x04002FC5 RID: 12229
	public Sprite AnnouncersIcon;

	// Token: 0x04002FC6 RID: 12230
	public Sprite BlastZonesIcon;

	// Token: 0x04002FC7 RID: 12231
	public Sprite LoadingScreenIcon;

	// Token: 0x04002FC8 RID: 12232
	public Sprite NetsukeIcon;

	// Token: 0x04002FC9 RID: 12233
	public Sprite PlayerProfilesIcon;

	// Token: 0x04002FCA RID: 12234
	public Sprite TokenIcon;

	// Token: 0x04002FCB RID: 12235
	public Transform UIContainer;

	// Token: 0x04002FCC RID: 12236
	public EquipmentSelectorModule GridModule;

	// Token: 0x04002FCD RID: 12237
	public CollectiblesModuleMode MainMode;

	// Token: 0x04002FCE RID: 12238
	public CollectiblesModuleMode GridMode;

	// Token: 0x04002FCF RID: 12239
	public GameObject DisabledGallerySpinErrorContainer;

	// Token: 0x04002FD0 RID: 12240
	public CursorTargetButton CharactersArrowButton;

	// Token: 0x04002FD1 RID: 12241
	private Vector3 initialPosition;

	// Token: 0x04002FD2 RID: 12242
	private ModuleMode currentModuleMode;

	// Token: 0x04002FD3 RID: 12243
	private Tweener _fromModuleTween;

	// Token: 0x04002FD4 RID: 12244
	private Tweener _toModuleTween;

	// Token: 0x04002FD5 RID: 12245
	private StoreScene3D storeScene;

	// Token: 0x04002FD6 RID: 12246
	private CollectiblesTabState previousStateTransition;

	// Token: 0x04002FD7 RID: 12247
	public float NetsukeFadeout;

	// Token: 0x04002FD8 RID: 12248
	private float _netsukeFadeout;

	// Token: 0x04002FD9 RID: 12249
	private Vector3 _transformScale = new Vector3(1f, 1f, 1f);

	// Token: 0x04002FDA RID: 12250
	private static HashSet<EquipmentTypes> nonSpinnedEquipmentTypes = new HashSet<EquipmentTypes>
	{
		EquipmentTypes.TOKEN,
		EquipmentTypes.PLAYER_ICON
	};

	// Token: 0x04002FDB RID: 12251
	private static HashSet<EquipmentTypes> gridModeEquipmentTypes = new HashSet<EquipmentTypes>
	{
		EquipmentTypes.NETSUKE,
		EquipmentTypes.PLAYER_ICON,
		EquipmentTypes.TOKEN
	};
}
