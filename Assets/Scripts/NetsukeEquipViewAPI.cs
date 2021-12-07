// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class NetsukeEquipViewAPI : INetsukeEquipViewAPI
{
	public static string UPDATED = "NetsukeEquipViewAPI.UPDATED";

	private int spinIndex;

	private int selectedIndex;

	private Dictionary<int, EquippableItem> changesTable = new Dictionary<int, EquippableItem>();

	[Inject]
	public IUserGlobalEquippedModel userGlobalEquippedModel
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
	public ISignalBus signalBus
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

	[Inject("CollectiblesEquipView")]
	public IEquipModuleAPI equipModuleAPI
	{
		get;
		set;
	}

	public int SelectedIndex
	{
		get
		{
			return this.selectedIndex;
		}
	}

	public int SpinIndex
	{
		get
		{
			return this.spinIndex;
		}
		set
		{
			if (this.spinIndex != value)
			{
				int num = value - this.spinIndex;
				this.spinIndex = value;
				this.selectedIndex += num;
				if (this.selectedIndex < 0)
				{
					this.selectedIndex += UserGlobalEquippedModel.NETSUKE_SLOTS;
				}
				else if (this.selectedIndex >= UserGlobalEquippedModel.NETSUKE_SLOTS)
				{
					this.selectedIndex -= UserGlobalEquippedModel.NETSUKE_SLOTS;
				}
				this.signalBus.Dispatch(NetsukeEquipViewAPI.UPDATED);
			}
		}
	}

	public EquippableItem SelectedItem
	{
		get
		{
			return this.equipModuleAPI.SelectedEquipment;
		}
	}

	public void BeginEdit()
	{
		this.changesTable.Clear();
		for (int i = 0; i < UserGlobalEquippedModel.NETSUKE_SLOTS; i++)
		{
			EquipmentID equippedNetsuke = this.userGlobalEquippedModel.GetEquippedNetsuke(i, this.storeAPI.Port);
			if (equippedNetsuke.IsNull())
			{
				this.changesTable[i] = null;
			}
			else
			{
				this.changesTable[i] = this.equipmentModel.GetItem(equippedNetsuke);
			}
		}
	}

	public void SaveEdit()
	{
		for (int i = 0; i < UserGlobalEquippedModel.NETSUKE_SLOTS; i++)
		{
			this.userGlobalEquippedModel.EquipNetsuke(this.changesTable[i], i, this.storeAPI.Port, true);
		}
	}

	public void DiscardEdit()
	{
		this.changesTable.Clear();
		this.signalBus.Dispatch(NetsukeEquipViewAPI.UPDATED);
	}

	public void EquipNetsuke(EquippableItem item, int index)
	{
		if (this.changesTable[index] == item)
		{
			this.changesTable[index] = null;
		}
		else
		{
			this.changesTable[index] = item;
		}
		this.signalBus.Dispatch(NetsukeEquipViewAPI.UPDATED);
	}

	public EquippableItem GetEquippedNetsuke(int index)
	{
		EquippableItem result;
		this.changesTable.TryGetValue(index, out result);
		return result;
	}

	public void TurnLeft()
	{
		this.SpinIndex--;
	}

	public void TurnRight()
	{
		this.SpinIndex++;
	}
}
