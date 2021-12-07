using System;
using System.Text;
using UnityEngine;

// Token: 0x02000A85 RID: 2693
[Serializable]
public class PlayerStats : RollbackStateTyped<PlayerStats>
{
	// Token: 0x06004EEC RID: 20204 RVA: 0x0014AE24 File Offset: 0x00149224
	public override void CopyTo(PlayerStats targetIn)
	{
		targetIn.playerInfo = this.playerInfo;
		for (int i = 0; i < this.stats.Length; i++)
		{
			targetIn.stats[i] = this.stats[i];
		}
	}

	// Token: 0x06004EED RID: 20205 RVA: 0x0014AE68 File Offset: 0x00149268
	public override object Clone()
	{
		PlayerStats playerStats = new PlayerStats();
		this.CopyTo(playerStats);
		return playerStats;
	}

	// Token: 0x06004EEE RID: 20206 RVA: 0x0014AE83 File Offset: 0x00149283
	public int GetStat(StatType stat)
	{
		return this.stats[(int)stat];
	}

	// Token: 0x06004EEF RID: 20207 RVA: 0x0014AE90 File Offset: 0x00149290
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
			Debug.LogWarning(string.Concat(new object[]
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

	// Token: 0x06004EF0 RID: 20208 RVA: 0x0014AF38 File Offset: 0x00149338
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

	// Token: 0x0400337F RID: 13183
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[IgnoreCopyValidation]
	public PlayerSelectionInfo playerInfo;

	// Token: 0x04003380 RID: 13184
	[IsClonedManually(IsClonedManuallyType.ShouldAutomate)]
	[IgnoreCopyValidation]
	public int[] stats = new int[Enum.GetValues(typeof(StatType)).Length];
}
