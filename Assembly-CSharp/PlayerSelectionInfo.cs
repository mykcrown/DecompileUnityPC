using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000610 RID: 1552
[Serializable]
public class PlayerSelectionInfo : ICloneable
{
	// Token: 0x17000971 RID: 2417
	// (get) Token: 0x06002645 RID: 9797 RVA: 0x000BC74F File Offset: 0x000BAB4F
	// (set) Token: 0x06002646 RID: 9798 RVA: 0x000BC757 File Offset: 0x000BAB57
	public TeamNum team { get; private set; }

	// Token: 0x06002647 RID: 9799 RVA: 0x000BC760 File Offset: 0x000BAB60
	public void SetEquipment(List<EquipmentID> playerEquipment, List<EquipmentID> characterEquipment)
	{
		if (this.characterID == CharacterID.None)
		{
			Debug.LogError("Failed to set equipment because characterID was not set before calling PlayerSelectionInfo.SetEquipment.");
			return;
		}
		this.playerEquipment = playerEquipment;
		this.characterEquipment = characterEquipment;
		UserTaunts userTaunts = new UserTaunts();
		SerializableDictionary<TauntSlot, EquipmentID> serializableDictionary = new SerializableDictionary<TauntSlot, EquipmentID>();
		for (int i = 0; i < 4; i++)
		{
			EquipmentID value = characterEquipment[3 + i];
			if (value.id != 0L)
			{
				serializableDictionary.Add((TauntSlot)i, value);
			}
		}
		userTaunts.Add(this.characterID, serializableDictionary);
		this.tauntData = userTaunts;
	}

	// Token: 0x06002648 RID: 9800 RVA: 0x000BC7E3 File Offset: 0x000BABE3
	public void SetTeam(GameMode mode, TeamNum team)
	{
		this.team = team;
		this.teamMap[mode] = team;
	}

	// Token: 0x06002649 RID: 9801 RVA: 0x000BC7F9 File Offset: 0x000BABF9
	public TeamNum GetTeam(GameMode mode)
	{
		if (this.teamMap.ContainsKey(mode))
		{
			return this.teamMap[mode];
		}
		return TeamNum.None;
	}

	// Token: 0x0600264A RID: 9802 RVA: 0x000BC81C File Offset: 0x000BAC1C
	public void SetCharacter(CharacterID characterID, ISkinDataManager skinDataManager, SkinDefinition skin = null)
	{
		this.characterID = characterID;
		if (skin == null && characterID != CharacterID.None)
		{
			skin = skinDataManager.GetDefaultSkin(characterID);
		}
		this.skinKey = ((!(skin == null)) ? skin.uniqueKey : null);
		this.characterIndex = 0;
	}

	// Token: 0x0600264B RID: 9803 RVA: 0x000BC870 File Offset: 0x000BAC70
	public object Clone()
	{
		return base.MemberwiseClone();
	}

	// Token: 0x04001BFC RID: 7164
	public PlayerNum playerNum;

	// Token: 0x04001BFD RID: 7165
	public PlayerType type;

	// Token: 0x04001BFE RID: 7166
	public PlayerProfile curProfile;

	// Token: 0x04001BFF RID: 7167
	public bool isLocal = true;

	// Token: 0x04001C00 RID: 7168
	public bool isSpectator;

	// Token: 0x04001C01 RID: 7169
	public ulong userID;

	// Token: 0x04001C02 RID: 7170
	public List<EquipmentID> playerEquipment;

	// Token: 0x04001C03 RID: 7171
	public List<EquipmentID> characterEquipment;

	// Token: 0x04001C04 RID: 7172
	public UserTaunts tauntData;

	// Token: 0x04001C05 RID: 7173
	public CharacterID characterID;

	// Token: 0x04001C06 RID: 7174
	public string skinKey;

	// Token: 0x04001C07 RID: 7175
	public int characterIndex;

	// Token: 0x04001C08 RID: 7176
	public bool isRandom;

	// Token: 0x04001C0A RID: 7178
	private Dictionary<GameMode, TeamNum> teamMap = new Dictionary<GameMode, TeamNum>();
}
