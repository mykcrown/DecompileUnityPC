using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200060F RID: 1551
[Serializable]
public class PlayerSelectionList : IEnumerable
{
	// Token: 0x0600263B RID: 9787 RVA: 0x000BC588 File Offset: 0x000BA988
	public PlayerSelectionList(int size)
	{
		this.players = new PlayerSelectionInfo[size];
	}

	// Token: 0x1700096F RID: 2415
	// (get) Token: 0x0600263C RID: 9788 RVA: 0x000BC59C File Offset: 0x000BA99C
	public int Length
	{
		get
		{
			return this.players.Length;
		}
	}

	// Token: 0x0600263D RID: 9789 RVA: 0x000BC5A8 File Offset: 0x000BA9A8
	public void Resize(int size)
	{
		PlayerSelectionInfo[] array = this.players;
		this.players = new PlayerSelectionInfo[size];
		for (int i = 0; i < Mathf.Min(array.Length, this.players.Length); i++)
		{
			this.players[i] = array[i];
		}
	}

	// Token: 0x17000970 RID: 2416
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

	// Token: 0x06002640 RID: 9792 RVA: 0x000BC60C File Offset: 0x000BAA0C
	public bool IsSkinInUse(CharacterID characterId, SkinDefinition skin, PlayerNum playerNum)
	{
		foreach (PlayerSelectionInfo playerSelectionInfo in this.players)
		{
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

	// Token: 0x06002641 RID: 9793 RVA: 0x000BC670 File Offset: 0x000BAA70
	public PlayerSelectionInfo GetPlayer(PlayerNum playerNum)
	{
		foreach (PlayerSelectionInfo playerSelectionInfo in this.players)
		{
			if (playerSelectionInfo.playerNum == playerNum)
			{
				return playerSelectionInfo;
			}
		}
		return null;
	}

	// Token: 0x06002642 RID: 9794 RVA: 0x000BC6AB File Offset: 0x000BAAAB
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.players.GetEnumerator();
	}

	// Token: 0x06002643 RID: 9795 RVA: 0x000BC6B8 File Offset: 0x000BAAB8
	public void ResetPlayers()
	{
		foreach (PlayerSelectionInfo playerSelectionInfo in this.players)
		{
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

	// Token: 0x04001BFB RID: 7163
	private PlayerSelectionInfo[] players;
}
