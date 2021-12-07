// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CollectiblesIntroView : ClientBehavior
{
	private class EquipmentSpritePair
	{
		public EquipmentTypes Type;

		public Sprite Sprite;
	}

	public Animator ZoomAnimator;

	public StoreIntroPanelView IntroPanelView;

	public Sprite announcersSprite;

	public Sprite blastZoneSprite;

	public Sprite loadingScreenSprite;

	public Sprite netsukeSprite;

	public Sprite playerProfileSprite;

	public Sprite tokenSprite;

	public Action<EquipmentTypes> EquipmentTypeSelected;

	private Func<bool> _allowInteraction;

	private List<CollectiblesIntroView.EquipmentSpritePair> equipmentSpriteList = new List<CollectiblesIntroView.EquipmentSpritePair>();

	private List<StoreIntroPanel.ItemLayoutData> itemDataBuffer = new List<StoreIntroPanel.ItemLayoutData>();

	[Inject]
	public ICollectiblesTabAPI collectiblesTabAPI
	{
		get;
		set;
	}

	[Inject("CollectiblesEquipView")]
	public IEquipModuleAPI equipAPI
	{
		get;
		set;
	}

	[Inject]
	public IEquipmentModel equipModel
	{
		get;
		set;
	}

	[Inject]
	public IUserInventory inventory
	{
		get;
		set;
	}

	[Inject]
	public ILocalization localization
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

	public Func<bool> AllowInteraction
	{
		set
		{
			this._allowInteraction = value;
		}
	}

	[PostConstruct]
	public void Init()
	{
		this.equipmentSpriteList.Add(new CollectiblesIntroView.EquipmentSpritePair
		{
			Type = EquipmentTypes.NETSUKE,
			Sprite = this.netsukeSprite
		});
		this.equipmentSpriteList.Add(new CollectiblesIntroView.EquipmentSpritePair
		{
			Type = EquipmentTypes.TOKEN,
			Sprite = this.tokenSprite
		});
		if (base.gameDataManager.IsFeatureEnabled(FeatureID.FutureCollectibles))
		{
			this.equipmentSpriteList.Add(new CollectiblesIntroView.EquipmentSpritePair
			{
				Type = EquipmentTypes.ANNOUNCERS,
				Sprite = this.announcersSprite
			});
			this.equipmentSpriteList.Add(new CollectiblesIntroView.EquipmentSpritePair
			{
				Type = EquipmentTypes.LOADING_SCREEN,
				Sprite = this.loadingScreenSprite
			});
			this.equipmentSpriteList.Add(new CollectiblesIntroView.EquipmentSpritePair
			{
				Type = EquipmentTypes.BLAST_ZONE,
				Sprite = this.blastZoneSprite
			});
		}
		this.equipmentSpriteList.Add(new CollectiblesIntroView.EquipmentSpritePair
		{
			Type = EquipmentTypes.PLAYER_ICON,
			Sprite = this.playerProfileSprite
		});
		StoreIntroPanelView expr_F9 = this.IntroPanelView;
		expr_F9.OnItemClicked = (Action<int>)Delegate.Combine(expr_F9.OnItemClicked, new Action<int>(this._Init_m__0));
		this.refreshDataLists();
		base.injector.Inject(this.IntroPanelView);
		this.IntroPanelView.Initialize(this.itemDataBuffer.Count, new Func<StoreIntroPanel>(this.findPanelForCurrentSelection));
		base.listen("CollectiblesTabAPI.UPDATED", new Action(this.onUpdated));
		base.listen("StoreTabSelectionModel.STORE_TAB_UPDATED", new Action(this.onUpdated));
	}

	private void onUpdated()
	{
		this.refreshDataLists();
		this.IntroPanelView.Setup(this.itemDataBuffer);
		if (this.ZoomAnimator.gameObject.activeInHierarchy)
		{
			this.ZoomAnimator.SetBool("initialize", true);
			this.ZoomAnimator.SetBool("skip animation", this.collectiblesTabAPI.SkipAnimation);
			if (this.ZoomAnimator.gameObject.activeInHierarchy)
			{
				if (this.collectiblesTabAPI.GetState() == CollectiblesTabState.IntroView)
				{
					this.ZoomAnimator.SetBool("visible", true);
					this.IntroPanelView.Enable();
					this.showBeforeTransition();
				}
				else
				{
					this.ZoomAnimator.SetBool("visible", false);
					this.IntroPanelView.Disable();
					this.hideAfterTransition();
				}
			}
		}
	}

	private StoreIntroPanel findPanelForCurrentSelection()
	{
		foreach (StoreIntroPanel current in this.IntroPanelView.elements)
		{
			if (this.equipAPI.SelectedEquipType == (current.itemData as StoreIntroPanel.CollectibleItemLayoutData).equipmentType)
			{
				return current;
			}
		}
		return null;
	}

	private void refreshDataLists()
	{
		this.itemDataBuffer.Clear();
		foreach (CollectiblesIntroView.EquipmentSpritePair current in this.equipmentSpriteList)
		{
			StoreIntroPanel.CollectibleItemLayoutData collectibleItemLayoutData = new StoreIntroPanel.CollectibleItemLayoutData();
			collectibleItemLayoutData.image = current.Sprite;
			collectibleItemLayoutData.equipmentType = current.Type;
			collectibleItemLayoutData.itemName = this.localization.GetText("equipType.plural." + current.Type);
			collectibleItemLayoutData.amount = this.inventory.GetOwnedGlobalItemCount(current.Type);
			collectibleItemLayoutData.maxAmount = this.equipModel.GetGlobalItems(current.Type).Length;
			collectibleItemLayoutData.useUnlocked = false;
			collectibleItemLayoutData.isUnlocked = false;
			collectibleItemLayoutData.isNew = this.inventory.HasNewGlobalItem(current.Type);
			this.itemDataBuffer.Add(collectibleItemLayoutData);
		}
	}

	public void OnActivate()
	{
		if (this.allowInteraction())
		{
			this.IntroPanelView.SyncToPreviousSelection();
		}
	}

	public void ReleaseSelections()
	{
		this.IntroPanelView.RemoveSelection();
	}

	protected bool allowInteraction()
	{
		return this._allowInteraction();
	}

	public void OnDrawComplete()
	{
		this.onUpdated();
	}

	public void UpdateMouseMode()
	{
		if (this.allowInteraction())
		{
			this.IntroPanelView.UpdateMouseMode();
		}
	}

	public void PlayTransition()
	{
		this.ZoomAnimator.SetBool("visible", false);
		this.IntroPanelView.Disable();
		this.hideAfterTransition();
	}

	private void showBeforeTransition()
	{
		base.transform.localPosition = Vector3.zero;
		this.timer.CancelTimeout(new Action(this.onHideComplete));
	}

	private void hideAfterTransition()
	{
		this.timer.SetTimeout(250, new Action(this.onHideComplete));
	}

	private void onHideComplete()
	{
		base.transform.localPosition = new Vector3(1000000f, 0f, 0f);
	}

	private void OnEnable()
	{
		this.onUpdated();
	}

	private void _Init_m__0(int itemIndex)
	{
		if (this.allowInteraction())
		{
			this.EquipmentTypeSelected(this.equipmentSpriteList[itemIndex].Type);
		}
	}
}
