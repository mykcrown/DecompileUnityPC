// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerSelectionInfo : ICloneable
{
	public PlayerNum playerNum;

	public PlayerType type;

	public PlayerProfile curProfile;

	public bool isLocal = true;

	public bool isSpectator;

	public ulong userID;

	public List<EquipmentID> playerEquipment;

	public List<EquipmentID> characterEquipment;

	public UserTaunts tauntData;

	public CharacterID characterID;

	public string skinKey;

	public int characterIndex;

	public bool isRandom;

	private Dictionary<GameMode, TeamNum> teamMap = new Dictionary<GameMode, TeamNum>();

	public TeamNum team
	{
		get;
		private set;
	}

	public void SetEquipment(List<EquipmentID> playerEquipment, List<EquipmentID> characterEquipment)
	{
		if (this.characterID == CharacterID.None)
		{
			UnityEngine.Debug.LogError("Failed to set equipment because characterID was not set before calling PlayerSelectionInfo.SetEquipment.");
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

	public void SetTeam(GameMode mode, TeamNum team)
	{
		this.team = team;
		this.teamMap[mode] = team;
	}

	public TeamNum GetTeam(GameMode mode)
	{
		if (this.teamMap.ContainsKey(mode))
		{
			return this.teamMap[mode];
		}
		return TeamNum.None;
	}

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

	public object Clone()
	{
		return base.MemberwiseClone();
	}
}
