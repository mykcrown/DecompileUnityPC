using System;
using System.Collections.Generic;
using IconsServer;
using UnityEngine;

// Token: 0x0200073D RID: 1853
public class UserCharacterEquippedModel : IUserCharacterEquippedModel, IStartupLoader
{
	// Token: 0x17000B3D RID: 2877
	// (get) Token: 0x06002DE3 RID: 11747 RVA: 0x000E9EB9 File Offset: 0x000E82B9
	// (set) Token: 0x06002DE4 RID: 11748 RVA: 0x000E9EC1 File Offset: 0x000E82C1
	[Inject]
	public IIconsServerAPI iconsServerAPI { get; set; }

	// Token: 0x17000B3E RID: 2878
	// (get) Token: 0x06002DE5 RID: 11749 RVA: 0x000E9ECA File Offset: 0x000E82CA
	// (set) Token: 0x06002DE6 RID: 11750 RVA: 0x000E9ED2 File Offset: 0x000E82D2
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x17000B3F RID: 2879
	// (get) Token: 0x06002DE7 RID: 11751 RVA: 0x000E9EDB File Offset: 0x000E82DB
	// (set) Token: 0x06002DE8 RID: 11752 RVA: 0x000E9EE3 File Offset: 0x000E82E3
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000B40 RID: 2880
	// (get) Token: 0x06002DE9 RID: 11753 RVA: 0x000E9EEC File Offset: 0x000E82EC
	// (set) Token: 0x06002DEA RID: 11754 RVA: 0x000E9EF4 File Offset: 0x000E82F4
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x17000B41 RID: 2881
	// (get) Token: 0x06002DEB RID: 11755 RVA: 0x000E9EFD File Offset: 0x000E82FD
	// (set) Token: 0x06002DEC RID: 11756 RVA: 0x000E9F05 File Offset: 0x000E8305
	[Inject]
	public ICharacterDataHelper characterDataHelper { get; set; }

	// Token: 0x17000B42 RID: 2882
	// (get) Token: 0x06002DED RID: 11757 RVA: 0x000E9F0E File Offset: 0x000E830E
	// (set) Token: 0x06002DEE RID: 11758 RVA: 0x000E9F16 File Offset: 0x000E8316
	[Inject]
	public ISkinDataManager skinDataManager { get; set; }

	// Token: 0x17000B43 RID: 2883
	// (get) Token: 0x06002DEF RID: 11759 RVA: 0x000E9F1F File Offset: 0x000E831F
	// (set) Token: 0x06002DF0 RID: 11760 RVA: 0x000E9F27 File Offset: 0x000E8327
	[Inject]
	public IUserCharacterUnlockModel unlockModel { get; set; }

	// Token: 0x17000B44 RID: 2884
	// (get) Token: 0x06002DF1 RID: 11761 RVA: 0x000E9F30 File Offset: 0x000E8330
	// (set) Token: 0x06002DF2 RID: 11762 RVA: 0x000E9F38 File Offset: 0x000E8338
	[Inject]
	public ISaveFileData saveFileData { get; set; }

	// Token: 0x06002DF3 RID: 11763 RVA: 0x000E9F44 File Offset: 0x000E8344
	private CharacterEquipmentMap getMap(int portId)
	{
		CharacterEquipmentMap result;
		if (!this.index.TryGetValue(portId, out result))
		{
			this.index[portId] = new CharacterEquipmentMap();
			result = this.index[portId];
		}
		return result;
	}

	// Token: 0x06002DF4 RID: 11764 RVA: 0x000E9F84 File Offset: 0x000E8384
	public bool IsEquipped(EquippableItem item, CharacterID characterID, int portId)
	{
		if (characterID != CharacterID.Any)
		{
			return this.GetEquippedByType(item.type, characterID, portId) == item.id;
		}
		CharacterEquipmentMap map = this.getMap(portId);
		foreach (KeyValuePair<CharacterID, SerializableDictionary<EquipmentTypes, EquipmentID>> keyValuePair in map)
		{
			EquipmentID a;
			if (keyValuePair.Value.TryGetValue(item.type, out a))
			{
				if (a == item.id)
				{
					return true;
				}
			}
			else if (this.equipmentModel.GetDefaultItem(keyValuePair.Key, item.type).id == item.id)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002DF5 RID: 11765 RVA: 0x000EA070 File Offset: 0x000E8470
	private Dictionary<EquipmentTypes, EquipmentID> getEquipsForCharacter(CharacterID id, int portId)
	{
		CharacterEquipmentMap map = this.getMap(portId);
		if (!map.ContainsKey(id))
		{
			map[id] = new SerializableDictionary<EquipmentTypes, EquipmentID>();
		}
		return map[id];
	}

	// Token: 0x06002DF6 RID: 11766 RVA: 0x000EA0A4 File Offset: 0x000E84A4
	public void Equip(EquippableItem item, CharacterID characterID, int portId, bool alertServer = true)
	{
		if (characterID == CharacterID.Any)
		{
			CharacterEquipmentMap map = this.getMap(portId);
			foreach (KeyValuePair<CharacterID, SerializableDictionary<EquipmentTypes, EquipmentID>> keyValuePair in map)
			{
				if (this.unlockModel.IsUnlocked(keyValuePair.Key))
				{
					this.Equip(item, keyValuePair.Key, portId, alertServer);
				}
			}
		}
		else
		{
			Dictionary<EquipmentTypes, EquipmentID> equipsForCharacter = this.getEquipsForCharacter(characterID, portId);
			equipsForCharacter[item.type] = item.id;
			if (alertServer)
			{
				ulong num = (ulong)((item.id.id >= 0L) ? item.id.id : 0L);
				int num2 = PlayerUtil.FirstEquipmentSlotForType(item.type);
				if (num2 < 0)
				{
					Debug.LogErrorFormat("Invalid Character Equipment Type to Slot: {0} {1}", new object[]
					{
						num,
						item.type.ToString()
					});
				}
			}
			this.saveToDisk();
			this.signalBus.Dispatch(UserCharacterEquippedModel.UPDATED);
		}
	}

	// Token: 0x06002DF7 RID: 11767 RVA: 0x000EA1D8 File Offset: 0x000E85D8
	public EquippableItem GetEquippedItem(EquipmentTypes type, CharacterID characterId, int portId)
	{
		EquipmentID equippedByType = this.GetEquippedByType(type, characterId, portId);
		if (!equippedByType.IsNull())
		{
			return this.equipmentModel.GetItem(equippedByType);
		}
		return null;
	}

	// Token: 0x06002DF8 RID: 11768 RVA: 0x000EA20C File Offset: 0x000E860C
	public EquipmentID GetEquippedByType(EquipmentTypes type, CharacterID characterId, int portId)
	{
		if (characterId != CharacterID.None && characterId != CharacterID.Any && characterId != CharacterID.Random)
		{
			Dictionary<EquipmentTypes, EquipmentID> equipsForCharacter = this.getEquipsForCharacter(characterId, portId);
			if (equipsForCharacter.ContainsKey(type))
			{
				return equipsForCharacter[type];
			}
			EquippableItem defaultItem = this.equipmentModel.GetDefaultItem(characterId, type);
			if (defaultItem != null)
			{
				return defaultItem.id;
			}
		}
		return default(EquipmentID);
	}

	// Token: 0x06002DF9 RID: 11769 RVA: 0x000EA270 File Offset: 0x000E8670
	public SkinDefinition GetEquippedSkin(CharacterID characterId, int portId)
	{
		EquipmentID equippedByType = this.GetEquippedByType(EquipmentTypes.SKIN, characterId, portId);
		if (!equippedByType.IsNull())
		{
			return this.equipmentModel.GetSkinFromItem(equippedByType);
		}
		return this.skinDataManager.GetDefaultSkin(characterId);
	}

	// Token: 0x06002DFA RID: 11770 RVA: 0x000EA2AC File Offset: 0x000E86AC
	public void StartupLoad(Action callback)
	{
		this.index = this.saveFileData.GetFromFile<AllUserEquipmentIndex>(UserCharacterEquippedModel.FILENAME);
		if (this.index == null)
		{
			this.index = new AllUserEquipmentIndex();
		}
		callback();
	}

	// Token: 0x06002DFB RID: 11771 RVA: 0x000EA2E0 File Offset: 0x000E86E0
	private void saveToDisk()
	{
		this.saveFileData.SaveToFile<AllUserEquipmentIndex>(UserCharacterEquippedModel.FILENAME, this.index);
	}

	// Token: 0x0400206D RID: 8301
	public static string UPDATED = "UserCharacterEquippedModel.UPDATED";

	// Token: 0x0400206E RID: 8302
	private static string FILENAME = "charEquipped_local";

	// Token: 0x04002077 RID: 8311
	private AllUserEquipmentIndex index = new AllUserEquipmentIndex();
}
