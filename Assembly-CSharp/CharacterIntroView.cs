using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009D8 RID: 2520
public class CharacterIntroView : ClientBehavior
{
	// Token: 0x17001112 RID: 4370
	// (get) Token: 0x06004737 RID: 18231 RVA: 0x00135C7E File Offset: 0x0013407E
	// (set) Token: 0x06004738 RID: 18232 RVA: 0x00135C86 File Offset: 0x00134086
	[Inject]
	public ICharactersTabAPI charactersTabAPI { get; set; }

	// Token: 0x17001113 RID: 4371
	// (get) Token: 0x06004739 RID: 18233 RVA: 0x00135C8F File Offset: 0x0013408F
	// (set) Token: 0x0600473A RID: 18234 RVA: 0x00135C97 File Offset: 0x00134097
	[Inject]
	public ICharacterEquipViewAPI characterEquipViewAPI { get; set; }

	// Token: 0x17001114 RID: 4372
	// (get) Token: 0x0600473B RID: 18235 RVA: 0x00135CA0 File Offset: 0x001340A0
	// (set) Token: 0x0600473C RID: 18236 RVA: 0x00135CA8 File Offset: 0x001340A8
	[Inject]
	public IEquipmentModel equipModel { get; set; }

	// Token: 0x17001115 RID: 4373
	// (get) Token: 0x0600473D RID: 18237 RVA: 0x00135CB1 File Offset: 0x001340B1
	// (set) Token: 0x0600473E RID: 18238 RVA: 0x00135CB9 File Offset: 0x001340B9
	[Inject]
	public IUserInventory inventory { get; set; }

	// Token: 0x17001116 RID: 4374
	// (get) Token: 0x0600473F RID: 18239 RVA: 0x00135CC2 File Offset: 0x001340C2
	// (set) Token: 0x06004740 RID: 18240 RVA: 0x00135CCA File Offset: 0x001340CA
	[Inject]
	public IUserCharacterUnlockModel unlockModel { get; set; }

	// Token: 0x17001117 RID: 4375
	// (get) Token: 0x06004741 RID: 18241 RVA: 0x00135CD3 File Offset: 0x001340D3
	// (set) Token: 0x06004742 RID: 18242 RVA: 0x00135CDB File Offset: 0x001340DB
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x17001118 RID: 4376
	// (get) Token: 0x06004743 RID: 18243 RVA: 0x00135CE4 File Offset: 0x001340E4
	// (set) Token: 0x06004744 RID: 18244 RVA: 0x00135CEC File Offset: 0x001340EC
	[Inject]
	public ICharacterDataHelper characterDataHelper { get; set; }

	// Token: 0x17001119 RID: 4377
	// (get) Token: 0x06004745 RID: 18245 RVA: 0x00135CF5 File Offset: 0x001340F5
	// (set) Token: 0x06004746 RID: 18246 RVA: 0x00135CFD File Offset: 0x001340FD
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x1700111A RID: 4378
	// (get) Token: 0x06004747 RID: 18247 RVA: 0x00135D06 File Offset: 0x00134106
	// (set) Token: 0x06004748 RID: 18248 RVA: 0x00135D0E File Offset: 0x0013410E
	[Inject]
	public UIManager uiManager { get; set; }

	// Token: 0x1700111B RID: 4379
	// (get) Token: 0x06004749 RID: 18249 RVA: 0x00135D17 File Offset: 0x00134117
	// (set) Token: 0x0600474A RID: 18250 RVA: 0x00135D1F File Offset: 0x0013411F
	[Inject]
	public IWindowDisplay windowDisplay { get; set; }

	// Token: 0x1700111C RID: 4380
	// (get) Token: 0x0600474B RID: 18251 RVA: 0x00135D28 File Offset: 0x00134128
	// (set) Token: 0x0600474C RID: 18252 RVA: 0x00135D30 File Offset: 0x00134130
	[Inject]
	public ICharacterLists characterLists { get; set; }

	// Token: 0x1700111D RID: 4381
	// (get) Token: 0x0600474D RID: 18253 RVA: 0x00135D39 File Offset: 0x00134139
	// (set) Token: 0x0600474E RID: 18254 RVA: 0x00135D41 File Offset: 0x00134141
	[Inject]
	public ICharacterMenusDataLoader characterMenusDataLoader { private get; set; }

	// Token: 0x0600474F RID: 18255 RVA: 0x00135D4C File Offset: 0x0013414C
	[PostConstruct]
	public void Init()
	{
		StoreIntroPanelView introPanelView = this.IntroPanelView;
		introPanelView.OnItemClicked = (Action<int>)Delegate.Combine(introPanelView.OnItemClicked, new Action<int>(delegate(int itemIndex)
		{
			if (this.allowInteraction())
			{
				this.CharacterSelected(this.charactersList[itemIndex]);
			}
		}));
		this.refreshDataLists();
		base.injector.Inject(this.IntroPanelView);
		this.IntroPanelView.Initialize(this.itemDataBuffer.Count, new Func<StoreIntroPanel>(this.findPanelForCurrentSelection));
		base.listen("StoreTabSelectionModel.STORE_TAB_UPDATED", new Action(this.onUpdated));
		base.listen("CharactersTabAPI.UPDATED", new Action(this.onUpdated));
	}

	// Token: 0x06004750 RID: 18256 RVA: 0x00135DE8 File Offset: 0x001341E8
	private void onUpdated()
	{
		this.refreshDataLists();
		this.IntroPanelView.Setup(this.itemDataBuffer);
		if (this.ZoomAnimator.gameObject.activeInHierarchy)
		{
			this.ZoomAnimator.SetBool("initialize", true);
			this.ZoomAnimator.SetBool("skip animation", this.charactersTabAPI.SkipAnimation);
			if (this.ZoomAnimator.gameObject.activeInHierarchy)
			{
				if (this.charactersTabAPI.GetState() == CharactersTabState.IntroView)
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

	// Token: 0x06004751 RID: 18257 RVA: 0x00135EBC File Offset: 0x001342BC
	private StoreIntroPanel findPanelForCurrentSelection()
	{
		foreach (StoreIntroPanel storeIntroPanel in this.IntroPanelView.elements)
		{
			if (this.characterEquipViewAPI.SelectedCharacter == (storeIntroPanel.itemData as StoreIntroPanel.CharacterItemLayoutData).characterID)
			{
				return storeIntroPanel;
			}
		}
		return null;
	}

	// Token: 0x06004752 RID: 18258 RVA: 0x00135F40 File Offset: 0x00134340
	private void refreshDataLists()
	{
		this.itemDataBuffer.Clear();
		this.charactersList.Clear();
		foreach (CharacterDefinition characterDefinition in this.characterLists.GetNonRandomCharacters())
		{
			this.charactersList.Add(characterDefinition.characterID);
			CharacterMenusData data = this.characterMenusDataLoader.GetData(characterDefinition);
			StoreIntroPanel.CharacterItemLayoutData characterItemLayoutData = new StoreIntroPanel.CharacterItemLayoutData();
			characterItemLayoutData.characterID = characterDefinition.characterID;
			characterItemLayoutData.image = data.generalPortrait;
			characterItemLayoutData.portraitData = data.storeIntroPortraitData;
			characterItemLayoutData.itemName = this.characterDataHelper.GetDisplayName(data.characterID);
			characterItemLayoutData.amount = this.inventory.GetAllOwnedCharacterItemCount(data.characterID);
			characterItemLayoutData.maxAmount = this.equipModel.GetAllCharacterItems(data.characterID).Length;
			characterItemLayoutData.useUnlocked = false;
			characterItemLayoutData.isDynamicAlignment = true;
			characterItemLayoutData.isUnlocked = this.unlockModel.IsUnlocked(data.characterID);
			characterItemLayoutData.isNew = this.inventory.HasNewCharacterItem(data.characterID);
			this.itemDataBuffer.Add(characterItemLayoutData);
		}
	}

	// Token: 0x06004753 RID: 18259 RVA: 0x0013606A File Offset: 0x0013446A
	public void OnActivate()
	{
		if (this.allowInteraction())
		{
			this.IntroPanelView.SyncToPreviousSelection();
		}
	}

	// Token: 0x06004754 RID: 18260 RVA: 0x00136082 File Offset: 0x00134482
	public void ReleaseSelections()
	{
		this.IntroPanelView.RemoveSelection();
	}

	// Token: 0x06004755 RID: 18261 RVA: 0x0013608F File Offset: 0x0013448F
	protected bool allowInteraction()
	{
		return this._allowInteraction();
	}

	// Token: 0x06004756 RID: 18262 RVA: 0x0013609C File Offset: 0x0013449C
	public void OnDrawComplete()
	{
		this.onUpdated();
	}

	// Token: 0x06004757 RID: 18263 RVA: 0x001360A4 File Offset: 0x001344A4
	public void UpdateMouseMode()
	{
		if (this.allowInteraction())
		{
			this.IntroPanelView.UpdateMouseMode();
		}
	}

	// Token: 0x1700111E RID: 4382
	// (set) Token: 0x06004758 RID: 18264 RVA: 0x001360BC File Offset: 0x001344BC
	public Func<bool> AllowInteraction
	{
		set
		{
			this._allowInteraction = value;
		}
	}

	// Token: 0x06004759 RID: 18265 RVA: 0x001360C5 File Offset: 0x001344C5
	public void PlayTransition()
	{
		this.ZoomAnimator.SetBool("visible", false);
		this.IntroPanelView.Disable();
		this.hideAfterTransition();
	}

	// Token: 0x0600475A RID: 18266 RVA: 0x001360E9 File Offset: 0x001344E9
	private void showBeforeTransition()
	{
		base.transform.localPosition = Vector3.zero;
		this.timer.CancelTimeout(new Action(this.onHideComplete));
	}

	// Token: 0x0600475B RID: 18267 RVA: 0x00136112 File Offset: 0x00134512
	private void hideAfterTransition()
	{
		this.timer.SetTimeout(250, new Action(this.onHideComplete));
	}

	// Token: 0x0600475C RID: 18268 RVA: 0x00136130 File Offset: 0x00134530
	private void onHideComplete()
	{
		base.transform.localPosition = new Vector3(1000000f, 0f, 0f);
	}

	// Token: 0x0600475D RID: 18269 RVA: 0x00136151 File Offset: 0x00134551
	private void OnEnable()
	{
		this.onUpdated();
	}

	// Token: 0x04002F1A RID: 12058
	public Animator ZoomAnimator;

	// Token: 0x04002F1B RID: 12059
	public StoreIntroPanelView IntroPanelView;

	// Token: 0x04002F1C RID: 12060
	public Action<CharacterID> CharacterSelected;

	// Token: 0x04002F1D RID: 12061
	private Func<bool> _allowInteraction;

	// Token: 0x04002F1E RID: 12062
	private List<CharacterID> charactersList = new List<CharacterID>();

	// Token: 0x04002F1F RID: 12063
	private List<StoreIntroPanel.ItemLayoutData> itemDataBuffer = new List<StoreIntroPanel.ItemLayoutData>();
}
