using System;
using System.Collections.Generic;

// Token: 0x020009EB RID: 2539
public class CollectiblesEquipViewAPI : ICollectiblesEquipViewAPI
{
	// Token: 0x17001151 RID: 4433
	// (get) Token: 0x0600485B RID: 18523 RVA: 0x00139BF8 File Offset: 0x00137FF8
	// (set) Token: 0x0600485C RID: 18524 RVA: 0x00139C00 File Offset: 0x00138000
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x17001152 RID: 4434
	// (get) Token: 0x0600485D RID: 18525 RVA: 0x00139C09 File Offset: 0x00138009
	// (set) Token: 0x0600485E RID: 18526 RVA: 0x00139C11 File Offset: 0x00138011
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17001153 RID: 4435
	// (get) Token: 0x0600485F RID: 18527 RVA: 0x00139C1A File Offset: 0x0013801A
	// (set) Token: 0x06004860 RID: 18528 RVA: 0x00139C22 File Offset: 0x00138022
	[Inject]
	public ICollectiblesTabAPI collectiblesTabAPI { get; set; }

	// Token: 0x17001154 RID: 4436
	// (get) Token: 0x06004861 RID: 18529 RVA: 0x00139C2B File Offset: 0x0013802B
	// (set) Token: 0x06004862 RID: 18530 RVA: 0x00139C33 File Offset: 0x00138033
	[Inject]
	public IUserInventory userInventory { get; set; }

	// Token: 0x17001155 RID: 4437
	// (get) Token: 0x06004863 RID: 18531 RVA: 0x00139C3C File Offset: 0x0013803C
	// (set) Token: 0x06004864 RID: 18532 RVA: 0x00139C44 File Offset: 0x00138044
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x17001156 RID: 4438
	// (get) Token: 0x06004865 RID: 18533 RVA: 0x00139C4D File Offset: 0x0013804D
	// (set) Token: 0x06004866 RID: 18534 RVA: 0x00139C55 File Offset: 0x00138055
	[Inject("CollectiblesEquipView")]
	public IEquipModuleAPI equipAPI { get; set; }

	// Token: 0x06004867 RID: 18535 RVA: 0x00139C5E File Offset: 0x0013805E
	[PostConstruct]
	public void Init()
	{
		this.configureEquipment();
		this.signalBus.AddListener("CollectiblesTabAPI.UPDATED", new Action(this.onCollectiblesTabUpdated));
	}

	// Token: 0x06004868 RID: 18536 RVA: 0x00139C82 File Offset: 0x00138082
	private void onCollectiblesTabUpdated()
	{
		if (this.collectiblesTabAPI.GetState() == CollectiblesTabState.EquipView)
		{
			this.userInventory.MarkAsNotNewGlobal(this.equipAPI.SelectedEquipType, true);
		}
	}

	// Token: 0x06004869 RID: 18537 RVA: 0x00139CAC File Offset: 0x001380AC
	private void configureEquipment()
	{
		List<EquipmentTypes> list = new List<EquipmentTypes>();
		list.Add(EquipmentTypes.NETSUKE);
		list.Add(EquipmentTypes.TOKEN);
		if (this.gameDataManager.IsFeatureEnabled(FeatureID.FutureCollectibles))
		{
			list.Add(EquipmentTypes.ANNOUNCERS);
			list.Add(EquipmentTypes.LOADING_SCREEN);
			list.Add(EquipmentTypes.BLAST_ZONE);
		}
		list.Add(EquipmentTypes.PLAYER_ICON);
		this.validEquipmentTypes = list.ToArray();
	}

	// Token: 0x0600486A RID: 18538 RVA: 0x00139D0A File Offset: 0x0013810A
	public EquipmentTypes[] GetValidEquipTypes()
	{
		return this.validEquipmentTypes;
	}

	// Token: 0x0600486B RID: 18539 RVA: 0x00139D12 File Offset: 0x00138112
	public EquippableItem[] GetItems(EquipmentTypes type)
	{
		return this.equipmentModel.GetGlobalItems(type);
	}

	// Token: 0x0600486C RID: 18540 RVA: 0x00139D20 File Offset: 0x00138120
	public Netsuke GetNetsukeFromItem(EquippableItem item)
	{
		if (item == null)
		{
			return null;
		}
		return this.equipmentModel.GetNetsukeFromItem(item.id);
	}

	// Token: 0x0600486D RID: 18541 RVA: 0x00139D3B File Offset: 0x0013813B
	public PlayerToken GetPlayerTokenFromItem(EquippableItem item)
	{
		if (item == null)
		{
			return null;
		}
		return this.equipmentModel.GetPlayerTokenFromItem(item.id);
	}

	// Token: 0x0600486E RID: 18542 RVA: 0x00139D56 File Offset: 0x00138156
	public PlayerCardIconData GetPlayerIconDataFromItem(EquippableItem item)
	{
		if (item == null)
		{
			return null;
		}
		return this.equipmentModel.GetPlayerIconFromItem(item.id);
	}

	// Token: 0x04002FE2 RID: 12258
	private EquipmentTypes[] validEquipmentTypes;
}
