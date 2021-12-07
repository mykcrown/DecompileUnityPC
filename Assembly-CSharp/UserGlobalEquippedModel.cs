using System;
using IconsServer;
using UnityEngine;

// Token: 0x02000742 RID: 1858
public class UserGlobalEquippedModel : IUserGlobalEquippedModel, IStartupLoader
{
	// Token: 0x17000B45 RID: 2885
	// (get) Token: 0x06002E07 RID: 11783 RVA: 0x000EA351 File Offset: 0x000E8751
	// (set) Token: 0x06002E08 RID: 11784 RVA: 0x000EA359 File Offset: 0x000E8759
	[Inject]
	public IIconsServerAPI iconsServerAPI { get; set; }

	// Token: 0x17000B46 RID: 2886
	// (get) Token: 0x06002E09 RID: 11785 RVA: 0x000EA362 File Offset: 0x000E8762
	// (set) Token: 0x06002E0A RID: 11786 RVA: 0x000EA36A File Offset: 0x000E876A
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x17000B47 RID: 2887
	// (get) Token: 0x06002E0B RID: 11787 RVA: 0x000EA373 File Offset: 0x000E8773
	// (set) Token: 0x06002E0C RID: 11788 RVA: 0x000EA37B File Offset: 0x000E877B
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000B48 RID: 2888
	// (get) Token: 0x06002E0D RID: 11789 RVA: 0x000EA384 File Offset: 0x000E8784
	// (set) Token: 0x06002E0E RID: 11790 RVA: 0x000EA38C File Offset: 0x000E878C
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x17000B49 RID: 2889
	// (get) Token: 0x06002E0F RID: 11791 RVA: 0x000EA395 File Offset: 0x000E8795
	// (set) Token: 0x06002E10 RID: 11792 RVA: 0x000EA39D File Offset: 0x000E879D
	[Inject]
	public ISaveFileData saveFileData { get; set; }

	// Token: 0x06002E11 RID: 11793 RVA: 0x000EA3A8 File Offset: 0x000E87A8
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

	// Token: 0x06002E12 RID: 11794 RVA: 0x000EA3E8 File Offset: 0x000E87E8
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

	// Token: 0x06002E13 RID: 11795 RVA: 0x000EA428 File Offset: 0x000E8828
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

	// Token: 0x06002E14 RID: 11796 RVA: 0x000EA498 File Offset: 0x000E8898
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
				Debug.LogErrorFormat("Invalid Global Equipment Type to Slot: {0} {1}", new object[]
				{
					num,
					item.type.ToString()
				});
			}
		}
		this.saveToDisk();
	}

	// Token: 0x06002E15 RID: 11797 RVA: 0x000EA55C File Offset: 0x000E895C
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
				Debug.LogErrorFormat("Invalid Global Equipment Type to Slot: {0} {1} NETSUKE", new object[]
				{
					num,
					num2
				});
			}
		}
		this.saveToDisk();
		this.signalBus.Dispatch(UserGlobalEquippedModel.UPDATED);
	}

	// Token: 0x06002E16 RID: 11798 RVA: 0x000EA614 File Offset: 0x000E8A14
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

	// Token: 0x06002E17 RID: 11799 RVA: 0x000EA674 File Offset: 0x000E8A74
	public EquipmentID GetEquippedNetsuke(int index, int portId)
	{
		UserGlobalEquippedNetsuke netsuke = this.getNetsuke(portId);
		if (netsuke.ContainsKey(index))
		{
			return netsuke[index];
		}
		return default(EquipmentID);
	}

	// Token: 0x06002E18 RID: 11800 RVA: 0x000EA6A6 File Offset: 0x000E8AA6
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

	// Token: 0x06002E19 RID: 11801 RVA: 0x000EA6D8 File Offset: 0x000E8AD8
	private bool isOpenNetsukeSlot(int index, int portId)
	{
		UserGlobalEquippedNetsuke netsuke = this.getNetsuke(portId);
		return !netsuke.ContainsKey(index);
	}

	// Token: 0x06002E1A RID: 11802 RVA: 0x000EA6FC File Offset: 0x000E8AFC
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

	// Token: 0x06002E1B RID: 11803 RVA: 0x000EA767 File Offset: 0x000E8B67
	private void saveToDisk()
	{
		this.saveFileData.SaveToFile<UserGlobalEquippedIndex>(UserGlobalEquippedModel.FILENAME, this.index);
		this.saveFileData.SaveToFile<UserGlobalEquippedNetsukeIndex>(UserGlobalEquippedModel.FILENAME_NETSUKE, this.netsukeIndex);
	}

	// Token: 0x0400207C RID: 8316
	public static string UPDATED = "UserGlobalEquippedModel.UPDATED";

	// Token: 0x0400207D RID: 8317
	public static int NETSUKE_SLOTS = 3;

	// Token: 0x0400207E RID: 8318
	private static string FILENAME = "globalEquip_local";

	// Token: 0x0400207F RID: 8319
	private static string FILENAME_NETSUKE = "netsukeEquip_local";

	// Token: 0x04002085 RID: 8325
	private UserGlobalEquippedIndex index = new UserGlobalEquippedIndex();

	// Token: 0x04002086 RID: 8326
	private UserGlobalEquippedNetsukeIndex netsukeIndex = new UserGlobalEquippedNetsukeIndex();
}
