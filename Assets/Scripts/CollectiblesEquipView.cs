// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CollectiblesEquipView : StoreTabWithEquipModule
{
	private sealed class _animateTransition_c__AnonStorey0
	{
		internal CollectiblesModuleMode from;

		internal CollectiblesModuleMode to;

		internal CollectiblesEquipView _this;

		internal float __m__0()
		{
			return this.from.CanvasGroup.alpha;
		}

		internal void __m__1(float valueIn)
		{
			this.from.CanvasGroup.alpha = valueIn;
		}

		internal float __m__2()
		{
			return this.to.CanvasGroup.alpha;
		}

		internal void __m__3(float valueIn)
		{
			this.to.CanvasGroup.alpha = valueIn;
		}

		internal void __m__4()
		{
			this.from.transform.localPosition = new Vector3(100000f, 0f, 0f);
			this._this.killFromModuleTweens();
		}
	}

	public const string INJECTION_TAG = "CollectiblesEquipView";

	public Animator ZoomAnimator;

	public Sprite AnnouncersIcon;

	public Sprite BlastZonesIcon;

	public Sprite LoadingScreenIcon;

	public Sprite NetsukeIcon;

	public Sprite PlayerProfilesIcon;

	public Sprite TokenIcon;

	public Transform UIContainer;

	public EquipmentSelectorModule GridModule;

	public CollectiblesModuleMode MainMode;

	public CollectiblesModuleMode GridMode;

	public GameObject DisabledGallerySpinErrorContainer;

	public CursorTargetButton CharactersArrowButton;

	private Vector3 initialPosition;

	private ModuleMode currentModuleMode;

	private Tweener _fromModuleTween;

	private Tweener _toModuleTween;

	private StoreScene3D storeScene;

	private CollectiblesTabState previousStateTransition;

	public float NetsukeFadeout;

	private float _netsukeFadeout;

	private Vector3 _transformScale = new Vector3(1f, 1f, 1f);

	private static HashSet<EquipmentTypes> nonSpinnedEquipmentTypes = new HashSet<EquipmentTypes>
	{
		EquipmentTypes.TOKEN,
		EquipmentTypes.PLAYER_ICON
	};

	private static HashSet<EquipmentTypes> gridModeEquipmentTypes = new HashSet<EquipmentTypes>
	{
		EquipmentTypes.NETSUKE,
		EquipmentTypes.PLAYER_ICON,
		EquipmentTypes.TOKEN
	};

	[Inject]
	public ICollectiblesEquipViewAPI api
	{
		get;
		set;
	}

	[Inject("CollectiblesEquipView")]
	public IEquipModuleAPI equipModuleAPI
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
	public IEquipmentModel equipmentModel
	{
		get;
		set;
	}

	[Inject]
	public IUserGlobalEquippedModel globalEquippedModel
	{
		get;
		set;
	}

	[Inject]
	public IMainThreadTimer timer
	{
		get;
		set;
	}

	[Inject]
	public IStoreTabsModel storeTabAPI
	{
		get;
		set;
	}

	[Inject]
	public IStoreAPI storeAPI
	{
		get;
		set;
	}

	[Inject]
	public ICharactersTabAPI charactersTabAPI
	{
		get;
		set;
	}

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
		CursorTargetButton expr_104 = this.CharactersArrowButton;
		expr_104.ClickCallback = (Action<CursorTargetButton>)Delegate.Combine(expr_104.ClickCallback, new Action<CursorTargetButton>(this.onCharactersArrowClicked));
		this.initialPosition = base.transform.localPosition;
		base.listen(EquipmentSelectorAPI.UPDATED, new Action(this.onDataUpdate));
		base.listen("CollectiblesTabAPI.UPDATED", new Action(this.onDataUpdate));
		base.listen("StoreTabSelectionModel.STORE_TAB_UPDATED", new Action(this.onTabUpdate));
		base.listen(EquipmentModel.UPDATED, new Action(this.onDataUpdate));
		base.listen(EquipFlow.NETSUKE, new Action(this.onNetsukeEquipRequested));
	}

	private void onNetsukeEquipRequested()
	{
		base.audioManager.PlayMenuSound(SoundKey.store_itemEquip, 0f);
		this.collectiblesTabAPI.SetState(CollectiblesTabState.NetsukeEquipView, false);
	}

	private void configureModules()
	{
		base.injector.Inject(this.GridModule);
		this.module = new TwoModuleManager();
		(this.module as TwoModuleManager).AddModule(this.EquipModule);
		(this.module as TwoModuleManager).AddModule(this.GridModule);
		this.GridModule.itemDisplayMode = EquipmentSelectorModule.ItemDisplay.GRID;
		this.switchToModule(ModuleMode.GRID);
	}

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

	private void animateTransition(CollectiblesModuleMode from, CollectiblesModuleMode to)
	{
		CollectiblesEquipView._animateTransition_c__AnonStorey0 _animateTransition_c__AnonStorey = new CollectiblesEquipView._animateTransition_c__AnonStorey0();
		_animateTransition_c__AnonStorey.from = from;
		_animateTransition_c__AnonStorey.to = to;
		_animateTransition_c__AnonStorey._this = this;
		float duration = 0.07f;
		this.killFromModuleTweens();
		_animateTransition_c__AnonStorey.to.transform.localPosition = Vector3.zero;
		_animateTransition_c__AnonStorey.to.Activate();
		this._fromModuleTween = DOTween.To(new DOGetter<float>(_animateTransition_c__AnonStorey.__m__0), new DOSetter<float>(_animateTransition_c__AnonStorey.__m__1), 0f, duration).SetEase(Ease.Linear);
		this._toModuleTween = DOTween.To(new DOGetter<float>(_animateTransition_c__AnonStorey.__m__2), new DOSetter<float>(_animateTransition_c__AnonStorey.__m__3), 1f, duration).SetEase(Ease.Linear).OnComplete(new TweenCallback(_animateTransition_c__AnonStorey.__m__4));
	}

	private void killFromModuleTweens()
	{
		TweenUtil.Destroy(ref this._fromModuleTween);
		TweenUtil.Destroy(ref this._toModuleTween);
	}

	protected override bool isEquipViewActive()
	{
		return this.collectiblesTabAPI.GetState() == CollectiblesTabState.EquipView;
	}

	private void syncModuleAtEndOfFrame()
	{
		EquipmentSelectorModule current = (this.module as TwoModuleManager).GetCurrent();
		current.ForceSyncButtonSelection();
		current.ForceRedraws();
	}

	private void mapSpriteToEquip(EquipmentTypes id, Sprite icon)
	{
		this.equipModuleAPI.MapEquipTypeIcon(id, icon);
	}

	private bool shouldCollectibleTypeSpin(EquipmentTypes equipmentType)
	{
		return !CollectiblesEquipView.nonSpinnedEquipmentTypes.Contains(equipmentType);
	}

	public void OnDrawComplete()
	{
		this.module.OnDrawComplete();
		this.onDataUpdate();
	}

	private void addEquipment()
	{
		this.EquipModule.Init(this.equipModuleAPI);
		this.GridModule.Init(this.equipModuleAPI);
		List<EquippableItem> list = new List<EquippableItem>();
		List<EquippableItem> list2 = new List<EquippableItem>();
		EquipTypeDefinition[] validEquipTypes = this.equipModuleAPI.GetValidEquipTypes();
		for (int i = 0; i < validEquipTypes.Length; i++)
		{
			EquipTypeDefinition equipTypeDefinition = validEquipTypes[i];
			EquippableItem[] items = this.api.GetItems(equipTypeDefinition.type);
			for (int j = 0; j < items.Length; j++)
			{
				EquippableItem item = items[j];
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

	private void Update()
	{
		this.DisabledGallerySpinErrorContainer.SetActive(UIInputModule.DisableMenuRightStick);
	}

	private void onDataUpdate()
	{
		this.updateModule();
		this.updateAssetPreview();
		this.updateTabMode();
	}

	private void updateModule()
	{
		ModuleMode mode = (!CollectiblesEquipView.gridModeEquipmentTypes.Contains(this.equipModuleAPI.SelectedEquipType)) ? ModuleMode.MAIN : ModuleMode.GRID;
		this.switchToModule(mode);
	}

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

	private void onTabUpdate()
	{
		this.updateTabMode();
	}

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

	private void zoomGoDeeperComplete()
	{
		base.transform.localPosition = new Vector3(100000f, 0f, 0f);
	}

	private void showBeforeTransition()
	{
		base.transform.localPosition = this.initialPosition;
		this.storeScene.SetShadowMode(false, true);
		this.timer.SetTimeout(150, new Action(this.onShowComplete));
	}

	private void onShowComplete()
	{
		this.storeScene.SetShadowMode(true, false);
	}

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

	private void alignDisplays()
	{
		if (this.equipModuleAPI.SelectedEquipType != EquipmentTypes.NONE)
		{
			this.alignTransform(this.MainMode.ItemDisplay);
			this.alignTransform(this.GridMode.ItemDisplay);
		}
	}

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

	public void PlayTransition()
	{
		this.ZoomAnimator.SetBool("visible", true);
		base.transform.localPosition = this.initialPosition;
	}

	public void ReleaseSelections()
	{
		this.module.ReleaseSelections();
	}

	public void OnStateActivate()
	{
		this.module.Activate();
		this.module.BeginMenuFocus();
	}

	public bool OnCancelPressed()
	{
		return this.isEquipModuleFocused() && this.module.OnCancelPressed();
	}

	public bool OnLeft()
	{
		if (this.isEquipModuleFocused())
		{
			this.module.OnLeft();
			return true;
		}
		return false;
	}

	public override void UpdateRightStick(float x, float y)
	{
		if (this.shouldCollectibleTypeSpin(this.equipModuleAPI.SelectedEquipType) && x != 0f)
		{
			this.storeScene.SpinItemManual(StoreScene3D.ItemType.COLLECTIBLE, x);
		}
	}

	private void clickedPreviewItemButton(MenuItemButton button, InputEventData eventData)
	{
		if (this.shouldCollectibleTypeSpin(this.equipModuleAPI.SelectedEquipType))
		{
			this.spinItem();
		}
		this.storeScene.OnItemClick(StoreScene3D.ItemType.COLLECTIBLE);
	}

	private void spinItem()
	{
		this.storeScene.SpinItem(StoreScene3D.ItemType.COLLECTIBLE);
	}

	private void onCharactersArrowClicked(CursorTargetButton button)
	{
		this.storeTabAPI.Current = StoreTab.CHARACTERS;
		this.charactersTabAPI.SetState(CharactersTabState.EquipView, true);
	}

	private void OnEnable()
	{
		this.onTabUpdate();
	}

	public override void OnDestroy()
	{
		if (this.storeScene != null)
		{
			this.storeScene.HideCollectiblesObject();
		}
		base.OnDestroy();
	}

	private List<EquipmentLine> listFilter(List<EquipmentLine> inlist)
	{
		List<EquipmentLine> list = new List<EquipmentLine>();
		foreach (EquipmentLine current in inlist)
		{
			bool flag = (this.equipModuleAPI.HasPrice(current.Item) && current.Item.promoted) || this.equipModuleAPI.HasItem(current.Item.id);
			if (flag)
			{
				list.Add(current);
			}
		}
		return list;
	}
}
