// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Text;
using UnityEngine;

[Serializable]
public class PlayerStats : RollbackStateTyped<PlayerStats>
{
	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	public PlayerSelectionInfo playerInfo;

	[IgnoreCopyValidation, IsClonedManually(IsClonedManuallyType.ShouldAutomate)]
	public int[] stats = new int[Enum.GetValues(typeof(StatType)).Length];

	public override void CopyTo(PlayerStats targetIn)
	{
		targetIn.playerInfo = this.playerInfo;
		for (int i = 0; i < this.stats.Length; i++)
		{
			targetIn.stats[i] = this.stats[i];
		}
	}

	public override object Clone()
	{
		PlayerStats playerStats = new PlayerStats();
		this.CopyTo(playerStats);
		return playerStats;
	}

	public int GetStat(StatType stat)
	{
		return this.stats[(int)stat];
	}

	public void LogStat(StatType stat, int value, PointsValueType type)
	{
		switch (type)
		{
		case PointsValueType.Raw:
			this.stats[(int)stat] = value;
			break;
		case PointsValueType.Addition:
			this.stats[(int)stat] += value;
			break;
		case PointsValueType.Subtraction:
			this.stats[(int)stat] -= value;
			break;
		default:
			UnityEngine.Debug.LogWarning(string.Concat(new object[]
			{
				"Unhandled stat operation ",
				type,
				" on stat ",
				stat,
				" with value ",
				value
			}));
			break;
		}
	}

	public string FormatStatValue(StatType stat, int value)
	{
		StringBuilder stringBuilder = new StringBuilder();
		switch (stat)
		{
		case StatType.Death:
		case StatType.Suicide:
			return ((value <= 0) ? string.Empty : "-") + value;
		case StatType.Kill:
		case StatType.Points:
			return ((value <= 0) ? string.Empty : "+") + value;
		case StatType.StarSecond:
			TimeUtil.FormatTime((float)value, stringBuilder, 0, false);
			return stringBuilder.ToString();
		}
		return value.ToString();
	}
}
