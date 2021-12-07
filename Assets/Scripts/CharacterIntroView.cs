// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CharacterIntroView : ClientBehavior
{
	public Animator ZoomAnimator;

	public StoreIntroPanelView IntroPanelView;

	public Action<CharacterID> CharacterSelected;

	private Func<bool> _allowInteraction;

	private List<CharacterID> charactersList = new List<CharacterID>();

	private List<StoreIntroPanel.ItemLayoutData> itemDataBuffer = new List<StoreIntroPanel.ItemLayoutData>();

	[Inject]
	public ICharactersTabAPI charactersTabAPI
	{
		get;
		set;
	}

	[Inject]
	public ICharacterEquipViewAPI characterEquipViewAPI
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
	public IUserCharacterUnlockModel unlockModel
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
	public ICharacterDataHelper characterDataHelper
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
	public UIManager uiManager
	{
		get;
		set;
	}

	[Inject]
	public IWindowDisplay windowDisplay
	{
		get;
		set;
	}

	[Inject]
	public ICharacterLists characterLists
	{
		get;
		set;
	}

	[Inject]
	public ICharacterMenusDataLoader characterMenusDataLoader
	{
		private get;
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
		StoreIntroPanelView expr_06 = this.IntroPanelView;
		expr_06.OnItemClicked = (Action<int>)Delegate.Combine(expr_06.OnItemClicked, new Action<int>(this._Init_m__0));
		this.refreshDataLists();
		base.injector.Inject(this.IntroPanelView);
		this.IntroPanelView.Initialize(this.itemDataBuffer.Count, new Func<StoreIntroPanel>(this.findPanelForCurrentSelection));
		base.listen("StoreTabSelectionModel.STORE_TAB_UPDATED", new Action(this.onUpdated));
		base.listen("CharactersTabAPI.UPDATED", new Action(this.onUpdated));
	}

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

	private StoreIntroPanel findPanelForCurrentSelection()
	{
		foreach (StoreIntroPanel current in this.IntroPanelView.elements)
		{
			if (this.characterEquipViewAPI.SelectedCharacter == (current.itemData as StoreIntroPanel.CharacterItemLayoutData).characterID)
			{
				return current;
			}
		}
		return null;
	}

	private void refreshDataLists()
	{
		this.itemDataBuffer.Clear();
		this.charactersList.Clear();
		CharacterDefinition[] nonRandomCharacters = this.characterLists.GetNonRandomCharacters();
		for (int i = 0; i < nonRandomCharacters.Length; i++)
		{
			CharacterDefinition characterDefinition = nonRandomCharacters[i];
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
			this.CharacterSelected(this.charactersList[itemIndex]);
		}
	}
}
