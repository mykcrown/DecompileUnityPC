// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UserCharacterEquippedModel : IUserCharacterEquippedModel, IStartupLoader
{
	public static string UPDATED = "UserCharacterEquippedModel.UPDATED";

	private static string FILENAME = "charEquipped_local";

	private AllUserEquipmentIndex index = new AllUserEquipmentIndex();

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
	public ICharacterDataHelper characterDataHelper
	{
		get;
		set;
	}

	[Inject]
	public ISkinDataManager skinDataManager
	{
		get;
		set;
	}

	[Inject]
	public IUserCharacterUnlockModel unlockModel
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

	public bool IsEquipped(EquippableItem item, CharacterID characterID, int portId)
	{
		if (characterID != CharacterID.Any)
		{
			return this.GetEquippedByType(item.type, characterID, portId) == item.id;
		}
		CharacterEquipmentMap map = this.getMap(portId);
		foreach (KeyValuePair<CharacterID, SerializableDictionary<EquipmentTypes, EquipmentID>> current in map)
		{
			EquipmentID a;
			if (current.Value.TryGetValue(item.type, out a))
			{
				if (a == item.id)
				{
					bool result = true;
					return result;
				}
			}
			else if (this.equipmentModel.GetDefaultItem(current.Key, item.type).id == item.id)
			{
				bool result = true;
				return result;
			}
		}
		return false;
	}

	private Dictionary<EquipmentTypes, EquipmentID> getEquipsForCharacter(CharacterID id, int portId)
	{
		CharacterEquipmentMap map = this.getMap(portId);
		if (!map.ContainsKey(id))
		{
			map[id] = new SerializableDictionary<EquipmentTypes, EquipmentID>();
		}
		return map[id];
	}

	public void Equip(EquippableItem item, CharacterID characterID, int portId, bool alertServer = true)
	{
		if (characterID == CharacterID.Any)
		{
			CharacterEquipmentMap map = this.getMap(portId);
			foreach (KeyValuePair<CharacterID, SerializableDictionary<EquipmentTypes, EquipmentID>> current in map)
			{
				if (this.unlockModel.IsUnlocked(current.Key))
				{
					this.Equip(item, current.Key, portId, alertServer);
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
					UnityEngine.Debug.LogErrorFormat("Invalid Character Equipment Type to Slot: {0} {1}", new object[]
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

	public EquippableItem GetEquippedItem(EquipmentTypes type, CharacterID characterId, int portId)
	{
		EquipmentID equippedByType = this.GetEquippedByType(type, characterId, portId);
		if (!equippedByType.IsNull())
		{
			return this.equipmentModel.GetItem(equippedByType);
		}
		return null;
	}

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

	public SkinDefinition GetEquippedSkin(CharacterID characterId, int portId)
	{
		EquipmentID equippedByType = this.GetEquippedByType(EquipmentTypes.SKIN, characterId, portId);
		if (!equippedByType.IsNull())
		{
			return this.equipmentModel.GetSkinFromItem(equippedByType);
		}
		return this.skinDataManager.GetDefaultSkin(characterId);
	}

	public void StartupLoad(Action callback)
	{
		this.index = this.saveFileData.GetFromFile<AllUserEquipmentIndex>(UserCharacterEquippedModel.FILENAME);
		if (this.index == null)
		{
			this.index = new AllUserEquipmentIndex();
		}
		callback();
	}

	private void saveToDisk()
	{
		this.saveFileData.SaveToFile<AllUserEquipmentIndex>(UserCharacterEquippedModel.FILENAME, this.index);
	}
}
