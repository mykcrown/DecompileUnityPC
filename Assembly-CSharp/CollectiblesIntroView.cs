using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009ED RID: 2541
public class CollectiblesIntroView : ClientBehavior
{
	// Token: 0x17001157 RID: 4439
	// (get) Token: 0x06004875 RID: 18549 RVA: 0x00139D8F File Offset: 0x0013818F
	// (set) Token: 0x06004876 RID: 18550 RVA: 0x00139D97 File Offset: 0x00138197
	[Inject]
	public ICollectiblesTabAPI collectiblesTabAPI { get; set; }

	// Token: 0x17001158 RID: 4440
	// (get) Token: 0x06004877 RID: 18551 RVA: 0x00139DA0 File Offset: 0x001381A0
	// (set) Token: 0x06004878 RID: 18552 RVA: 0x00139DA8 File Offset: 0x001381A8
	[Inject("CollectiblesEquipView")]
	public IEquipModuleAPI equipAPI { get; set; }

	// Token: 0x17001159 RID: 4441
	// (get) Token: 0x06004879 RID: 18553 RVA: 0x00139DB1 File Offset: 0x001381B1
	// (set) Token: 0x0600487A RID: 18554 RVA: 0x00139DB9 File Offset: 0x001381B9
	[Inject]
	public IEquipmentModel equipModel { get; set; }

	// Token: 0x1700115A RID: 4442
	// (get) Token: 0x0600487B RID: 18555 RVA: 0x00139DC2 File Offset: 0x001381C2
	// (set) Token: 0x0600487C RID: 18556 RVA: 0x00139DCA File Offset: 0x001381CA
	[Inject]
	public IUserInventory inventory { get; set; }

	// Token: 0x1700115B RID: 4443
	// (get) Token: 0x0600487D RID: 18557 RVA: 0x00139DD3 File Offset: 0x001381D3
	// (set) Token: 0x0600487E RID: 18558 RVA: 0x00139DDB File Offset: 0x001381DB
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x1700115C RID: 4444
	// (get) Token: 0x0600487F RID: 18559 RVA: 0x00139DE4 File Offset: 0x001381E4
	// (set) Token: 0x06004880 RID: 18560 RVA: 0x00139DEC File Offset: 0x001381EC
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x06004881 RID: 18561 RVA: 0x00139DF8 File Offset: 0x001381F8
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
		StoreIntroPanelView introPanelView = this.IntroPanelView;
		introPanelView.OnItemClicked = (Action<int>)Delegate.Combine(introPanelView.OnItemClicked, new Action<int>(delegate(int itemIndex)
		{
			if (this.allowInteraction())
			{
				this.EquipmentTypeSelected(this.equipmentSpriteList[itemIndex].Type);
			}
		}));
		this.refreshDataLists();
		base.injector.Inject(this.IntroPanelView);
		this.IntroPanelView.Initialize(this.itemDataBuffer.Count, new Func<StoreIntroPanel>(this.findPanelForCurrentSelection));
		base.listen("CollectiblesTabAPI.UPDATED", new Action(this.onUpdated));
		base.listen("StoreTabSelectionModel.STORE_TAB_UPDATED", new Action(this.onUpdated));
	}

	// Token: 0x06004882 RID: 18562 RVA: 0x00139F88 File Offset: 0x00138388
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

	// Token: 0x06004883 RID: 18563 RVA: 0x0013A05C File Offset: 0x0013845C
	private StoreIntroPanel findPanelForCurrentSelection()
	{
		foreach (StoreIntroPanel storeIntroPanel in this.IntroPanelView.elements)
		{
			if (this.equipAPI.SelectedEquipType == (storeIntroPanel.itemData as StoreIntroPanel.CollectibleItemLayoutData).equipmentType)
			{
				return storeIntroPanel;
			}
		}
		return null;
	}

	// Token: 0x06004884 RID: 18564 RVA: 0x0013A0E0 File Offset: 0x001384E0
	private void refreshDataLists()
	{
		this.itemDataBuffer.Clear();
		foreach (CollectiblesIntroView.EquipmentSpritePair equipmentSpritePair in this.equipmentSpriteList)
		{
			StoreIntroPanel.CollectibleItemLayoutData collectibleItemLayoutData = new StoreIntroPanel.CollectibleItemLayoutData();
			collectibleItemLayoutData.image = equipmentSpritePair.Sprite;
			collectibleItemLayoutData.equipmentType = equipmentSpritePair.Type;
			collectibleItemLayoutData.itemName = this.localization.GetText("equipType.plural." + equipmentSpritePair.Type);
			collectibleItemLayoutData.amount = this.inventory.GetOwnedGlobalItemCount(equipmentSpritePair.Type);
			collectibleItemLayoutData.maxAmount = this.equipModel.GetGlobalItems(equipmentSpritePair.Type).Length;
			collectibleItemLayoutData.useUnlocked = false;
			collectibleItemLayoutData.isUnlocked = false;
			collectibleItemLayoutData.isNew = this.inventory.HasNewGlobalItem(equipmentSpritePair.Type);
			this.itemDataBuffer.Add(collectibleItemLayoutData);
		}
	}

	// Token: 0x06004885 RID: 18565 RVA: 0x0013A1E8 File Offset: 0x001385E8
	public void OnActivate()
	{
		if (this.allowInteraction())
		{
			this.IntroPanelView.SyncToPreviousSelection();
		}
	}

	// Token: 0x06004886 RID: 18566 RVA: 0x0013A200 File Offset: 0x00138600
	public void ReleaseSelections()
	{
		this.IntroPanelView.RemoveSelection();
	}

	// Token: 0x06004887 RID: 18567 RVA: 0x0013A20D File Offset: 0x0013860D
	protected bool allowInteraction()
	{
		return this._allowInteraction();
	}

	// Token: 0x06004888 RID: 18568 RVA: 0x0013A21A File Offset: 0x0013861A
	public void OnDrawComplete()
	{
		this.onUpdated();
	}

	// Token: 0x06004889 RID: 18569 RVA: 0x0013A222 File Offset: 0x00138622
	public void UpdateMouseMode()
	{
		if (this.allowInteraction())
		{
			this.IntroPanelView.UpdateMouseMode();
		}
	}

	// Token: 0x1700115D RID: 4445
	// (set) Token: 0x0600488A RID: 18570 RVA: 0x0013A23A File Offset: 0x0013863A
	public Func<bool> AllowInteraction
	{
		set
		{
			this._allowInteraction = value;
		}
	}

	// Token: 0x0600488B RID: 18571 RVA: 0x0013A243 File Offset: 0x00138643
	public void PlayTransition()
	{
		this.ZoomAnimator.SetBool("visible", false);
		this.IntroPanelView.Disable();
		this.hideAfterTransition();
	}

	// Token: 0x0600488C RID: 18572 RVA: 0x0013A267 File Offset: 0x00138667
	private void showBeforeTransition()
	{
		base.transform.localPosition = Vector3.zero;
		this.timer.CancelTimeout(new Action(this.onHideComplete));
	}

	// Token: 0x0600488D RID: 18573 RVA: 0x0013A290 File Offset: 0x00138690
	private void hideAfterTransition()
	{
		this.timer.SetTimeout(250, new Action(this.onHideComplete));
	}

	// Token: 0x0600488E RID: 18574 RVA: 0x0013A2AE File Offset: 0x001386AE
	private void onHideComplete()
	{
		base.transform.localPosition = new Vector3(1000000f, 0f, 0f);
	}

	// Token: 0x0600488F RID: 18575 RVA: 0x0013A2CF File Offset: 0x001386CF
	private void OnEnable()
	{
		this.onUpdated();
	}

	// Token: 0x04002FE9 RID: 12265
	public Animator ZoomAnimator;

	// Token: 0x04002FEA RID: 12266
	public StoreIntroPanelView IntroPanelView;

	// Token: 0x04002FEB RID: 12267
	public Sprite announcersSprite;

	// Token: 0x04002FEC RID: 12268
	public Sprite blastZoneSprite;

	// Token: 0x04002FED RID: 12269
	public Sprite loadingScreenSprite;

	// Token: 0x04002FEE RID: 12270
	public Sprite netsukeSprite;

	// Token: 0x04002FEF RID: 12271
	public Sprite playerProfileSprite;

	// Token: 0x04002FF0 RID: 12272
	public Sprite tokenSprite;

	// Token: 0x04002FF1 RID: 12273
	public Action<EquipmentTypes> EquipmentTypeSelected;

	// Token: 0x04002FF2 RID: 12274
	private Func<bool> _allowInteraction;

	// Token: 0x04002FF3 RID: 12275
	private List<CollectiblesIntroView.EquipmentSpritePair> equipmentSpriteList = new List<CollectiblesIntroView.EquipmentSpritePair>();

	// Token: 0x04002FF4 RID: 12276
	private List<StoreIntroPanel.ItemLayoutData> itemDataBuffer = new List<StoreIntroPanel.ItemLayoutData>();

	// Token: 0x020009EE RID: 2542
	private class EquipmentSpritePair
	{
		// Token: 0x04002FF5 RID: 12277
		public EquipmentTypes Type;

		// Token: 0x04002FF6 RID: 12278
		public Sprite Sprite;
	}
}
