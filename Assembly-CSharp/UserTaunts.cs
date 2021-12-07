using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using IconsServer;
using UnityEngine;

// Token: 0x02000754 RID: 1876
[Serializable]
public class UserTaunts : SerializableDictionary<CharacterID, SerializableDictionary<TauntSlot, EquipmentID>>
{
	// Token: 0x06002E79 RID: 11897 RVA: 0x000EB0DA File Offset: 0x000E94DA
	public UserTaunts()
	{
	}

	// Token: 0x06002E7A RID: 11898 RVA: 0x000EB0E2 File Offset: 0x000E94E2
	public UserTaunts(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}

	// Token: 0x06002E7B RID: 11899 RVA: 0x000EB0EC File Offset: 0x000E94EC
	public UserTaunts(Dictionary<CharacterID, Dictionary<TauntSlot, EquipmentID>> dict)
	{
		foreach (KeyValuePair<CharacterID, Dictionary<TauntSlot, EquipmentID>> keyValuePair in dict)
		{
			base[keyValuePair.Key] = new SerializableDictionary<TauntSlot, EquipmentID>();
			foreach (KeyValuePair<TauntSlot, EquipmentID> keyValuePair2 in keyValuePair.Value)
			{
				base[keyValuePair.Key][keyValuePair2.Key] = keyValuePair2.Value;
			}
		}
	}

	// Token: 0x06002E7C RID: 11900 RVA: 0x000EB1BC File Offset: 0x000E95BC
	public SerializableDictionary<TauntSlot, EquipmentID> GetSlotsForCharacter(CharacterID characterId)
	{
		if (characterId == CharacterID.None || characterId == CharacterID.Any)
		{
			Debug.LogError("Attempt to get taunts for character " + characterId);
		}
		if (!base.ContainsKey(characterId))
		{
			base[characterId] = new SerializableDictionary<TauntSlot, EquipmentID>();
		}
		return base[characterId];
	}

	// Token: 0x06002E7D RID: 11901 RVA: 0x000EB20C File Offset: 0x000E960C
	public EquipmentID GetItemInSlot(CharacterID characterId, TauntSlot slot)
	{
		SerializableDictionary<TauntSlot, EquipmentID> slotsForCharacter = this.GetSlotsForCharacter(characterId);
		if (slotsForCharacter.ContainsKey(slot))
		{
			return slotsForCharacter[slot];
		}
		return default(EquipmentID);
	}

	// Token: 0x06002E7E RID: 11902 RVA: 0x000EB240 File Offset: 0x000E9640
	public void Copy(CharacterID characterId, SerializableDictionary<TauntSlot, EquipmentID> data, IIconsServerAPI iconsServerAPI = null)
	{
		SerializableDictionary<TauntSlot, EquipmentID> slotsForCharacter = this.GetSlotsForCharacter(characterId);
		slotsForCharacter.Clear();
		foreach (KeyValuePair<TauntSlot, EquipmentID> keyValuePair in data)
		{
			TauntSlot key = keyValuePair.Key;
			EquipmentID value = keyValuePair.Value;
			slotsForCharacter[key] = value;
			if (iconsServerAPI != null)
			{
			}
		}
	}

	// Token: 0x06002E7F RID: 11903 RVA: 0x000EB2C0 File Offset: 0x000E96C0
	public bool IsEquipped(EquippableItem item, CharacterID characterId)
	{
		SerializableDictionary<TauntSlot, EquipmentID> slotsForCharacter = this.GetSlotsForCharacter(characterId);
		foreach (EquipmentID b in slotsForCharacter.Values)
		{
			if (item.id == b)
			{
				return true;
			}
		}
		return false;
	}
}
