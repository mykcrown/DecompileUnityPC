// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class MockInventorySource : IInventorySource
{
	private EquipmentID[] ownedItems;

	private bool isConnected;

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public IServerConnectionManager serverConnectionManager
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
	public IEquipmentSource equipmentSource
	{
		get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(ServerConnectionManager.UPDATED, new Action(this.checkForInventoryUpdate));
		this.signalBus.AddListener(EquipmentModel.SOURCE_UPDATED, new Action(this.updateInventory));
		this.updateInventory();
	}

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

	private void updateInventory()
	{
		if (this.gameDataManager.GameData.IsFeatureEnabled(FeatureID.UnlockEverything))
		{
			List<EquipmentID> list = new List<EquipmentID>();
			EquippableItem[] all = this.equipmentSource.GetAll();
			for (int i = 0; i < all.Length; i++)
			{
				EquippableItem equippableItem = all[i];
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

	public EquipmentID[] GetOwned()
	{
		return this.ownedItems;
	}
}
