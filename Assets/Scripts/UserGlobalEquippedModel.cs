// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;
using UnityEngine;

public class UserGlobalEquippedModel : IUserGlobalEquippedModel, IStartupLoader
{
	public static string UPDATED = "UserGlobalEquippedModel.UPDATED";

	public static int NETSUKE_SLOTS = 3;

	private static string FILENAME = "globalEquip_local";

	private static string FILENAME_NETSUKE = "netsukeEquip_local";

	private UserGlobalEquippedIndex index = new UserGlobalEquippedIndex();

	private UserGlobalEquippedNetsukeIndex netsukeIndex = new UserGlobalEquippedNetsukeIndex();

	[Inject]
	public IIconsServerAPI iconsServerAPI
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
	public ISignalBus signalBus
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
	public ISaveFileData saveFileData
	{
		get;
		set;
	}

	private UserGlobalEquipped getEquipped(int portId)
	{
		UserGlobalEquipped result;
		if (!this.index.TryGetValue(portId, out result))
		{
			this.index[portId] = new UserGlobalEquipped();
			result = this.index[portId];
		}
		return result;
	}

	private UserGlobalEquippedNetsuke getNetsuke(int portId)
	{
		UserGlobalEquippedNetsuke result;
		if (!this.netsukeIndex.TryGetValue(portId, out result))
		{
			this.netsukeIndex[portId] = new UserGlobalEquippedNetsuke();
			result = this.netsukeIndex[portId];
		}
		return result;
	}

	public bool IsEquipped(EquippableItem item, int portId)
	{
		if (item.type == EquipmentTypes.NETSUKE)
		{
			for (int i = 0; i < UserGlobalEquippedModel.NETSUKE_SLOTS; i++)
			{
				if (this.GetEquippedNetsuke(i, portId) == item.id)
				{
					return true;
				}
			}
		}
		else if (this.GetEquippedByType(item.type, portId) == item.id)
		{
			return true;
		}
		return false;
	}

	public void Equip(EquippableItem item, int portId, bool alertServer = true)
	{
		if (item.type == EquipmentTypes.NETSUKE)
		{
			throw new UnityException("Incorrect function for netsuke");
		}
		UserGlobalEquipped equipped = this.getEquipped(portId);
		equipped[item.type] = item.id;
		this.signalBus.Dispatch(UserGlobalEquippedModel.UPDATED);
		if (alertServer)
		{
			ulong num = (ulong)((item.id.id >= 0L) ? item.id.id : 0L);
			int num2 = PlayerUtil.FirstEquipmentSlotForType(item.type);
			if (num2 < 0)
			{
				UnityEngine.Debug.LogErrorFormat("Invalid Global Equipment Type to Slot: {0} {1}", new object[]
				{
					num,
					item.type.ToString()
				});
			}
		}
		this.saveToDisk();
	}

	public void EquipNetsuke(EquippableItem item, int index, int portId, bool alertServer = true)
	{
		UserGlobalEquippedNetsuke netsuke = this.getNetsuke(portId);
		netsuke[index] = ((item != null) ? item.id : default(EquipmentID));
		if (alertServer)
		{
			ulong num = (ulong)((item != null && item.id.id >= 0L) ? item.id.id : 0L);
			int num2 = PlayerUtil.FirstEquipmentSlotForType(EquipmentTypes.NETSUKE) + index;
			if (num2 < 0)
			{
				UnityEngine.Debug.LogErrorFormat("Invalid Global Equipment Type to Slot: {0} {1} NETSUKE", new object[]
				{
					num,
					num2
				});
			}
		}
		this.saveToDisk();
		this.signalBus.Dispatch(UserGlobalEquippedModel.UPDATED);
	}

	public EquipmentID GetEquippedByType(EquipmentTypes type, int portId)
	{
		if (type == EquipmentTypes.NETSUKE)
		{
			throw new UnityException("Incorrect function for netsuke");
		}
		UserGlobalEquipped equipped = this.getEquipped(portId);
		if (equipped.ContainsKey(type))
		{
			return equipped[type];
		}
		EquippableItem defaultItem = this.equipmentModel.GetDefaultItem(type);
		if (defaultItem == null)
		{
			return default(EquipmentID);
		}
		return defaultItem.id;
	}

	public EquipmentID GetEquippedNetsuke(int index, int portId)
	{
		UserGlobalEquippedNetsuke netsuke = this.getNetsuke(portId);
		if (netsuke.ContainsKey(index))
		{
			return netsuke[index];
		}
		return default(EquipmentID);
	}

	public int GetOpenNetsukeSlot(int portId)
	{
		if (this.isOpenNetsukeSlot(0, portId))
		{
			return 0;
		}
		if (this.isOpenNetsukeSlot(1, portId))
		{
			return 1;
		}
		if (this.isOpenNetsukeSlot(2, portId))
		{
			return 2;
		}
		return 0;
	}

	private bool isOpenNetsukeSlot(int index, int portId)
	{
		UserGlobalEquippedNetsuke netsuke = this.getNetsuke(portId);
		return !netsuke.ContainsKey(index);
	}

	public void StartupLoad(Action callback)
	{
		this.index = this.saveFileData.GetFromFile<UserGlobalEquippedIndex>(UserGlobalEquippedModel.FILENAME);
		if (this.index == null)
		{
			this.index = new UserGlobalEquippedIndex();
		}
		this.netsukeIndex = this.saveFileData.GetFromFile<UserGlobalEquippedNetsukeIndex>(UserGlobalEquippedModel.FILENAME_NETSUKE);
		if (this.netsukeIndex == null)
		{
			this.netsukeIndex = new UserGlobalEquippedNetsukeIndex();
		}
		callback();
	}

	private void saveToDisk()
	{
		this.saveFileData.SaveToFile<UserGlobalEquippedIndex>(UserGlobalEquippedModel.FILENAME, this.index);
		this.saveFileData.SaveToFile<UserGlobalEquippedNetsukeIndex>(UserGlobalEquippedModel.FILENAME_NETSUKE, this.netsukeIndex);
	}
}
