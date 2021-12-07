// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class UserTaunts : SerializableDictionary<CharacterID, SerializableDictionary<TauntSlot, EquipmentID>>
{
	public UserTaunts()
	{
	}

	public UserTaunts(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}

	public UserTaunts(Dictionary<CharacterID, Dictionary<TauntSlot, EquipmentID>> dict)
	{
		foreach (KeyValuePair<CharacterID, Dictionary<TauntSlot, EquipmentID>> current in dict)
		{
			base[current.Key] = new SerializableDictionary<TauntSlot, EquipmentID>();
			foreach (KeyValuePair<TauntSlot, EquipmentID> current2 in current.Value)
			{
				base[current.Key][current2.Key] = current2.Value;
			}
		}
	}

	public SerializableDictionary<TauntSlot, EquipmentID> GetSlotsForCharacter(CharacterID characterId)
	{
		if (characterId == CharacterID.None || characterId == CharacterID.Any)
		{
			UnityEngine.Debug.LogError("Attempt to get taunts for character " + characterId);
		}
		if (!base.ContainsKey(characterId))
		{
			base[characterId] = new SerializableDictionary<TauntSlot, EquipmentID>();
		}
		return base[characterId];
	}

	public EquipmentID GetItemInSlot(CharacterID characterId, TauntSlot slot)
	{
		SerializableDictionary<TauntSlot, EquipmentID> slotsForCharacter = this.GetSlotsForCharacter(characterId);
		if (slotsForCharacter.ContainsKey(slot))
		{
			return slotsForCharacter[slot];
		}
		return default(EquipmentID);
	}

	public void Copy(CharacterID characterId, SerializableDictionary<TauntSlot, EquipmentID> data, IIconsServerAPI iconsServerAPI = null)
	{
		SerializableDictionary<TauntSlot, EquipmentID> slotsForCharacter = this.GetSlotsForCharacter(characterId);
		slotsForCharacter.Clear();
		foreach (KeyValuePair<TauntSlot, EquipmentID> current in data)
		{
			TauntSlot key = current.Key;
			EquipmentID value = current.Value;
			slotsForCharacter[key] = value;
			if (iconsServerAPI != null)
			{
			}
		}
	}

	public bool IsEquipped(EquippableItem item, CharacterID characterId)
	{
		SerializableDictionary<TauntSlot, EquipmentID> slotsForCharacter = this.GetSlotsForCharacter(characterId);
		foreach (EquipmentID current in slotsForCharacter.Values)
		{
			if (item.id == current)
			{
				return true;
			}
		}
		return false;
	}
}
