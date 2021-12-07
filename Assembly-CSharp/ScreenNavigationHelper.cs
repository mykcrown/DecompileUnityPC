using System;
using System.Linq;

// Token: 0x020009C6 RID: 2502
public class ScreenNavigationHelper : IScreenNavigationHelper
{
	// Token: 0x170010CE RID: 4302
	// (get) Token: 0x06004618 RID: 17944 RVA: 0x00132485 File Offset: 0x00130885
	// (set) Token: 0x06004619 RID: 17945 RVA: 0x0013248D File Offset: 0x0013088D
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x170010CF RID: 4303
	// (get) Token: 0x0600461A RID: 17946 RVA: 0x00132496 File Offset: 0x00130896
	// (set) Token: 0x0600461B RID: 17947 RVA: 0x0013249E File Offset: 0x0013089E
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x170010D0 RID: 4304
	// (get) Token: 0x0600461C RID: 17948 RVA: 0x001324A7 File Offset: 0x001308A7
	// (set) Token: 0x0600461D RID: 17949 RVA: 0x001324AF File Offset: 0x001308AF
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x170010D1 RID: 4305
	// (get) Token: 0x0600461E RID: 17950 RVA: 0x001324B8 File Offset: 0x001308B8
	// (set) Token: 0x0600461F RID: 17951 RVA: 0x001324C0 File Offset: 0x001308C0
	[Inject]
	public IStoreAPI storeAPI { get; set; }

	// Token: 0x170010D2 RID: 4306
	// (get) Token: 0x06004620 RID: 17952 RVA: 0x001324C9 File Offset: 0x001308C9
	// (set) Token: 0x06004621 RID: 17953 RVA: 0x001324D1 File Offset: 0x001308D1
	[Inject]
	public IStoreTabsModel storeTabsModel { get; set; }

	// Token: 0x170010D3 RID: 4307
	// (get) Token: 0x06004622 RID: 17954 RVA: 0x001324DA File Offset: 0x001308DA
	// (set) Token: 0x06004623 RID: 17955 RVA: 0x001324E2 File Offset: 0x001308E2
	[Inject]
	public ICharactersTabAPI characterTabAPI { get; set; }

	// Token: 0x170010D4 RID: 4308
	// (get) Token: 0x06004624 RID: 17956 RVA: 0x001324EB File Offset: 0x001308EB
	// (set) Token: 0x06004625 RID: 17957 RVA: 0x001324F3 File Offset: 0x001308F3
	[Inject]
	public ICollectiblesTabAPI collectiblesTabAPI { get; set; }

	// Token: 0x170010D5 RID: 4309
	// (get) Token: 0x06004626 RID: 17958 RVA: 0x001324FC File Offset: 0x001308FC
	// (set) Token: 0x06004627 RID: 17959 RVA: 0x00132504 File Offset: 0x00130904
	[Inject]
	public ICharacterEquipViewAPI characterEquipViewAPI { get; set; }

	// Token: 0x170010D6 RID: 4310
	// (get) Token: 0x06004628 RID: 17960 RVA: 0x0013250D File Offset: 0x0013090D
	// (set) Token: 0x06004629 RID: 17961 RVA: 0x00132515 File Offset: 0x00130915
	[Inject]
	public ICollectiblesEquipViewAPI collectiblesEquipViewAPI { get; set; }

	// Token: 0x170010D7 RID: 4311
	// (get) Token: 0x0600462A RID: 17962 RVA: 0x0013251E File Offset: 0x0013091E
	// (set) Token: 0x0600462B RID: 17963 RVA: 0x00132526 File Offset: 0x00130926
	[Inject("CharacterEquipView")]
	public IEquipModuleAPI characterEquipModuleAPI { get; set; }

	// Token: 0x170010D8 RID: 4312
	// (get) Token: 0x0600462C RID: 17964 RVA: 0x0013252F File Offset: 0x0013092F
	// (set) Token: 0x0600462D RID: 17965 RVA: 0x00132537 File Offset: 0x00130937
	[Inject("CollectiblesEquipView")]
	public IEquipModuleAPI collectiblesEquipModuleAPI { get; set; }

	// Token: 0x0600462E RID: 17966 RVA: 0x00132540 File Offset: 0x00130940
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

	// Token: 0x0600462F RID: 17967 RVA: 0x00132690 File Offset: 0x00130A90
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
