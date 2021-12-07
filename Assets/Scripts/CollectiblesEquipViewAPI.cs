// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class CollectiblesEquipViewAPI : ICollectiblesEquipViewAPI
{
	private EquipmentTypes[] validEquipmentTypes;

	[Inject]
	public IEquipmentModel equipmentModel
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
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
	public IUserInventory userInventory
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

	[Inject("CollectiblesEquipView")]
	public IEquipModuleAPI equipAPI
	{
		get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		this.configureEquipment();
		this.signalBus.AddListener("CollectiblesTabAPI.UPDATED", new Action(this.onCollectiblesTabUpdated));
	}

	private void onCollectiblesTabUpdated()
	{
		if (this.collectiblesTabAPI.GetState() == CollectiblesTabState.EquipView)
		{
			this.userInventory.MarkAsNotNewGlobal(this.equipAPI.SelectedEquipType, true);
		}
	}

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

	public EquipmentTypes[] GetValidEquipTypes()
	{
		return this.validEquipmentTypes;
	}

	public EquippableItem[] GetItems(EquipmentTypes type)
	{
		return this.equipmentModel.GetGlobalItems(type);
	}

	public Netsuke GetNetsukeFromItem(EquippableItem item)
	{
		if (item == null)
		{
			return null;
		}
		return this.equipmentModel.GetNetsukeFromItem(item.id);
	}

	public PlayerToken GetPlayerTokenFromItem(EquippableItem item)
	{
		if (item == null)
		{
			return null;
		}
		return this.equipmentModel.GetPlayerTokenFromItem(item.id);
	}

	public PlayerCardIconData GetPlayerIconDataFromItem(EquippableItem item)
	{
		if (item == null)
		{
			return null;
		}
		return this.equipmentModel.GetPlayerIconFromItem(item.id);
	}
}
