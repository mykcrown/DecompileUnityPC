// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.Serialization;

[Serializable]
public class CharacterMoveSetData : IPreloadedGameAsset
{
	[FormerlySerializedAs("basicMoves")]
	public CharacterActionSet characterActions = new CharacterActionSet();

	[FormerlySerializedAs("attackMoves")]
	public MoveData[] moves = new MoveData[0];

	private static Func<MoveData, string> __f__am_cache0;

	public Dictionary<MoveLabel, List<MoveData>> GenerateMoveMap()
	{
		Dictionary<MoveLabel, List<MoveData>> dictionary = new Dictionary<MoveLabel, List<MoveData>>();
		MoveLabel[] values = EnumUtil.GetValues<MoveLabel>();
		MoveLabel[] array = values;
		for (int i = 0; i < array.Length; i++)
		{
			MoveLabel moveLabel = array[i];
			MoveData[] array2 = this.moves;
			for (int j = 0; j < array2.Length; j++)
			{
				MoveData moveData = array2[j];
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

	public void LoadMoveMap(Dictionary<MoveLabel, List<MoveData>> moveMap)
	{
		List<MoveData> list = new List<MoveData>();
		foreach (List<MoveData> current in moveMap.Values)
		{
			foreach (MoveData current2 in current)
			{
				if (current2 != null)
				{
					list.Add(current2);
				}
			}
		}
		IEnumerable<MoveData> arg_A3_0 = list;
		if (CharacterMoveSetData.__f__am_cache0 == null)
		{
			CharacterMoveSetData.__f__am_cache0 = new Func<MoveData, string>(CharacterMoveSetData._LoadMoveMap_m__0);
		}
		arg_A3_0.OrderBy(CharacterMoveSetData.__f__am_cache0);
		this.moves = list.ToArray();
	}

	public void RegisterPreload(PreloadContext context)
	{
		MoveData[] array = this.moves;
		for (int i = 0; i < array.Length; i++)
		{
			MoveData moveData = array[i];
			if (moveData.label != MoveLabel.Emote)
			{
				moveData.RegisterPreload(context);
			}
		}
	}

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
			return true;
		case MoveLabel.ShieldGust:
		case MoveLabel.Tech:
		case MoveLabel.TechForwardRoll:
		case MoveLabel.TechBackwardRoll:
		case MoveLabel.AssistAttack:
		case MoveLabel.PivotGrab:
		case MoveLabel.WallJump:
		case MoveLabel.GetUp:
		case MoveLabel.TechWall:
		case MoveLabel.TechCeiling:
			IL_65:
			if (label != MoveLabel.None)
			{
				return false;
			}
			return true;
		}
		goto IL_65;
	}

	private static string _LoadMoveMap_m__0(MoveData f)
	{
		return (!(f != null)) ? string.Empty : f.moveName;
	}
}
