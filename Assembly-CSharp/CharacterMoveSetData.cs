using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;

// Token: 0x0200058D RID: 1421
[Serializable]
public class CharacterMoveSetData : IPreloadedGameAsset
{
	// Token: 0x06002007 RID: 8199 RVA: 0x000A257C File Offset: 0x000A097C
	public Dictionary<MoveLabel, List<MoveData>> GenerateMoveMap()
	{
		Dictionary<MoveLabel, List<MoveData>> dictionary = new Dictionary<MoveLabel, List<MoveData>>();
		MoveLabel[] values = EnumUtil.GetValues<MoveLabel>();
		foreach (MoveLabel moveLabel in values)
		{
			foreach (MoveData moveData in this.moves)
			{
				if (!(moveData == null))
				{
					if (moveData.label == moveLabel)
					{
						if (!dictionary.ContainsKey(moveLabel))
						{
							dictionary.Add(moveLabel, new List<MoveData>());
						}
						dictionary[moveLabel].Add(moveData);
					}
				}
			}
			if (!dictionary.ContainsKey(moveLabel))
			{
				dictionary.Add(moveLabel, new List<MoveData>());
			}
			dictionary[moveLabel].Add(null);
		}
		return dictionary;
	}

	// Token: 0x06002008 RID: 8200 RVA: 0x000A2648 File Offset: 0x000A0A48
	public void LoadMoveMap(Dictionary<MoveLabel, List<MoveData>> moveMap)
	{
		List<MoveData> list = new List<MoveData>();
		foreach (List<MoveData> list2 in moveMap.Values)
		{
			foreach (MoveData moveData in list2)
			{
				if (moveData != null)
				{
					list.Add(moveData);
				}
			}
		}
		from f in list
		orderby (!(f != null)) ? string.Empty : f.moveName
		select f;
		this.moves = list.ToArray();
	}

	// Token: 0x06002009 RID: 8201 RVA: 0x000A2728 File Offset: 0x000A0B28
	public void RegisterPreload(PreloadContext context)
	{
		foreach (MoveData moveData in this.moves)
		{
			if (moveData.label != MoveLabel.Emote)
			{
				moveData.RegisterPreload(context);
			}
		}
	}

	// Token: 0x0600200A RID: 8202 RVA: 0x000A2770 File Offset: 0x000A0B70
	public static bool IsLegalEmptyMove(MoveLabel label)
	{
		switch (label)
		{
		case MoveLabel.Emote:
		case MoveLabel.ThrowItem:
		case MoveLabel.ThrowItemLeft:
		case MoveLabel.ThrowItemUp:
		case MoveLabel.ThrowItemDown:
		case MoveLabel.ThrowItemRight:
		case MoveLabel.Hologram:
		case MoveLabel.DEPRECATED_1:
		case MoveLabel.DEPRECATED_2:
		case MoveLabel.DEPRECATED_3:
		case MoveLabel.DEPRECATED_4:
		case MoveLabel.VoiceLineTaunt:
		case MoveLabel.AllyAssist:
			break;
		default:
			if (label != MoveLabel.None)
			{
				return false;
			}
			break;
		}
		return true;
	}

	// Token: 0x04001973 RID: 6515
	[FormerlySerializedAs("basicMoves")]
	public CharacterActionSet characterActions = new CharacterActionSet();

	// Token: 0x04001974 RID: 6516
	[FormerlySerializedAs("attackMoves")]
	public MoveData[] moves = new MoveData[0];
}
