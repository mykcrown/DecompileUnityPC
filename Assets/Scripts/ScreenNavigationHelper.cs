// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Linq;

public class ScreenNavigationHelper : IScreenNavigationHelper
{
	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
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
	public IStoreAPI storeAPI
	{
		get;
		set;
	}

	[Inject]
	public IStoreTabsModel storeTabsModel
	{
		get;
		set;
	}

	[Inject]
	public ICharactersTabAPI characterTabAPI
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
	public ICharacterEquipViewAPI characterEquipViewAPI
	{
		get;
		set;
	}

	[Inject]
	public ICollectiblesEquipViewAPI collectiblesEquipViewAPI
	{
		get;
		set;
	}

	[Inject("CharacterEquipView")]
	public IEquipModuleAPI characterEquipModuleAPI
	{
		get;
		set;
	}

	[Inject("CollectiblesEquipView")]
	public IEquipModuleAPI collectiblesEquipModuleAPI
	{
		get;
		set;
	}

	public void GoToItemInStore(EquipmentID equipId, CharacterID overrideCharacterId = CharacterID.None)
	{
		if (this.gameDataManager.IsFeatureEnabled(FeatureID.Store))
		{
			EquippableItem item = this.equipmentModel.GetItem(equipId);
			if (item != null)
			{
				CharacterID selectedCharacter = (overrideCharacterId == CharacterID.None) ? item.character : overrideCharacterId;
				this.storeAPI.Mode = StoreMode.NORMAL;
				if (this.characterEquipViewAPI.GetValidEquipTypes().Contains(item.type))
				{
					this.storeTabsModel.Current = StoreTab.CHARACTERS;
					this.characterTabAPI.SetState(CharactersTabState.EquipView, false);
					this.characterEquipViewAPI.SelectedCharacter = selectedCharacter;
					this.characterEquipModuleAPI.SelectedEquipType = item.type;
					this.characterEquipModuleAPI.SelectedEquipment = item;
				}
				else if (this.collectiblesEquipViewAPI.GetValidEquipTypes().Contains(item.type))
				{
					this.storeTabsModel.Current = StoreTab.COLLECTIBLES;
					this.collectiblesTabAPI.SetState(CollectiblesTabState.EquipView, false);
					this.collectiblesEquipModuleAPI.SelectedEquipType = item.type;
					this.collectiblesEquipModuleAPI.SelectedEquipment = item;
				}
				else if (item.type == EquipmentTypes.CHARACTER)
				{
					this.storeTabsModel.Current = StoreTab.CHARACTERS;
					this.characterTabAPI.SetState(CharactersTabState.EquipView, false);
					this.characterEquipViewAPI.SelectedCharacter = selectedCharacter;
				}
				this.events.Broadcast(new LoadScreenCommand(ScreenType.StoreScreen, null, ScreenUpdateType.Next));
			}
		}
	}

	public void GoToOpenLootbox()
	{
		if (this.gameDataManager.IsFeatureEnabled(FeatureID.PortalPacks))
		{
			this.storeAPI.Mode = StoreMode.UNBOXING;
			if (this.gameDataManager.IsFeatureEnabled(FeatureID.LootBoxPurchase))
			{
				this.storeTabsModel.Current = StoreTab.LOOT_BOXES;
			}
			else
			{
				this.storeTabsModel.Current = StoreTab.FEATURED;
			}
			this.events.Broadcast(new LoadScreenCommand(ScreenType.StoreScreen, null, ScreenUpdateType.Next));
		}
	}
}
