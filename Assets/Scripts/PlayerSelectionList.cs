// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerSelectionList : IEnumerable
{
	private PlayerSelectionInfo[] players;

	public int Length
	{
		get
		{
			return this.players.Length;
		}
	}

	public PlayerSelectionInfo this[int i]
	{
		get
		{
			return this.players[i];
		}
		set
		{
			this.players[i] = value;
		}
	}

	public PlayerSelectionList(int size)
	{
		this.players = new PlayerSelectionInfo[size];
	}

	public void Resize(int size)
	{
		PlayerSelectionInfo[] array = this.players;
		this.players = new PlayerSelectionInfo[size];
		for (int i = 0; i < Mathf.Min(array.Length, this.players.Length); i++)
		{
			this.players[i] = array[i];
		}
	}

	public bool IsSkinInUse(CharacterID characterId, SkinDefinition skin, PlayerNum playerNum)
	{
		PlayerSelectionInfo[] array = this.players;
		for (int i = 0; i < array.Length; i++)
		{
			PlayerSelectionInfo playerSelectionInfo = array[i];
			if (playerSelectionInfo.playerNum != playerNum)
			{
				if (playerSelectionInfo.characterID == characterId && playerSelectionInfo.skinKey == skin.uniqueKey)
				{
					return true;
				}
			}
		}
		return false;
	}

	public PlayerSelectionInfo GetPlayer(PlayerNum playerNum)
	{
		PlayerSelectionInfo[] array = this.players;
		for (int i = 0; i < array.Length; i++)
		{
			PlayerSelectionInfo playerSelectionInfo = array[i];
			if (playerSelectionInfo.playerNum == playerNum)
			{
				return playerSelectionInfo;
			}
		}
		return null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.players.GetEnumerator();
	}

	public void ResetPlayers()
	{
		PlayerSelectionInfo[] array = this.players;
		for (int i = 0; i < array.Length; i++)
		{
			PlayerSelectionInfo playerSelectionInfo = array[i];
			playerSelectionInfo.curProfile.profileName = null;
			playerSelectionInfo.curProfile.localName = null;
			playerSelectionInfo.type = PlayerType.None;
			playerSelectionInfo.characterID = CharacterID.None;
			playerSelectionInfo.skinKey = null;
			playerSelectionInfo.isRandom = false;
			playerSelectionInfo.isLocal = true;
			playerSelectionInfo.characterEquipment = new List<EquipmentID>();
			playerSelectionInfo.playerEquipment = new List<EquipmentID>();
		}
	}
}
