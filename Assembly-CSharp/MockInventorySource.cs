using System;
using System.Collections.Generic;

// Token: 0x02000733 RID: 1843
public class MockInventorySource : IInventorySource
{
	// Token: 0x17000B1C RID: 2844
	// (get) Token: 0x06002D7A RID: 11642 RVA: 0x000E91B4 File Offset: 0x000E75B4
	// (set) Token: 0x06002D7B RID: 11643 RVA: 0x000E91BC File Offset: 0x000E75BC
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000B1D RID: 2845
	// (get) Token: 0x06002D7C RID: 11644 RVA: 0x000E91C5 File Offset: 0x000E75C5
	// (set) Token: 0x06002D7D RID: 11645 RVA: 0x000E91CD File Offset: 0x000E75CD
	[Inject]
	public IServerConnectionManager serverConnectionManager { get; set; }

	// Token: 0x17000B1E RID: 2846
	// (get) Token: 0x06002D7E RID: 11646 RVA: 0x000E91D6 File Offset: 0x000E75D6
	// (set) Token: 0x06002D7F RID: 11647 RVA: 0x000E91DE File Offset: 0x000E75DE
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x17000B1F RID: 2847
	// (get) Token: 0x06002D80 RID: 11648 RVA: 0x000E91E7 File Offset: 0x000E75E7
	// (set) Token: 0x06002D81 RID: 11649 RVA: 0x000E91EF File Offset: 0x000E75EF
	[Inject]
	public IEquipmentSource equipmentSource { get; set; }

	// Token: 0x06002D82 RID: 11650 RVA: 0x000E91F8 File Offset: 0x000E75F8
	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(ServerConnectionManager.UPDATED, new Action(this.checkForInventoryUpdate));
		this.signalBus.AddListener(EquipmentModel.SOURCE_UPDATED, new Action(this.updateInventory));
		this.updateInventory();
	}

	// Token: 0x06002D83 RID: 11651 RVA: 0x000E9238 File Offset: 0x000E7638
	private void checkForInventoryUpdate()
	{
		if (!this.isConnected || this.serverConnectionManager.IsConnectedToNexus != this.isConnected)
		{
			this.isConnected = this.serverConnectionManager.IsConnectedToNexus;
			if (this.isConnected)
			{
				this.updateInventory();
			}
		}
	}

	// Token: 0x06002D84 RID: 11652 RVA: 0x000E9288 File Offset: 0x000E7688
	private void updateInventory()
	{
		if (this.gameDataManager.GameData.IsFeatureEnabled(FeatureID.UnlockEverything))
		{
			List<EquipmentID> list = new List<EquipmentID>();
			foreach (EquippableItem equippableItem in this.equipmentSource.GetAll())
			{
				list.Add(equippableItem.id);
			}
			this.ownedItems = list.ToArray();
		}
		else
		{
			this.ownedItems = new EquipmentID[0];
		}
		this.signalBus.Dispatch(UserInventoryModel.SOURCE_UPDATED);
	}

	// Token: 0x06002D85 RID: 11653 RVA: 0x000E930E File Offset: 0x000E770E
	public EquipmentID[] GetOwned()
	{
		return this.ownedItems;
	}

	// Token: 0x0400204C RID: 8268
	private EquipmentID[] ownedItems;

	// Token: 0x0400204D RID: 8269
	private bool isConnected;
}
