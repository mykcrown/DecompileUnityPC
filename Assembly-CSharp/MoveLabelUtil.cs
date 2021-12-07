using System;
using System.Collections.Generic;

// Token: 0x020004B8 RID: 1208
public static class MoveLabelUtil
{
	// Token: 0x1700058C RID: 1420
	// (get) Token: 0x06001AB5 RID: 6837 RVA: 0x000897D0 File Offset: 0x00087BD0
	// (set) Token: 0x06001AB6 RID: 6838 RVA: 0x000897D7 File Offset: 0x00087BD7
	public static MoveLabel[] OrderedMoveLabels { get; private set; } = MoveLabelUtil.MoveEditorOrder;

	// Token: 0x06001AB7 RID: 6839 RVA: 0x000897DF File Offset: 0x00087BDF
	public static string GetMoveLabelString(MoveLabel moveLabel)
	{
		if (MoveLabelUtil.moveLabelNames.ContainsKey(moveLabel))
		{
			return MoveLabelUtil.moveLabelNames[moveLabel];
		}
		return moveLabel.ToString();
	}

	// Token: 0x06001AB8 RID: 6840 RVA: 0x0008980C File Offset: 0x00087C0C
	private static int orderOf(MoveLabel moveLabel)
	{
		for (int i = 0; i < MoveLabelUtil.MoveEditorOrder.Length; i++)
		{
			if (moveLabel == MoveLabelUtil.MoveEditorOrder[i])
			{
				return i;
			}
		}
		return int.MaxValue;
	}

	// Token: 0x0400142B RID: 5163
	public static readonly MoveLabel[] MoveEditorOrder = new MoveLabel[]
	{
		MoveLabel.NeutralAttack,
		MoveLabel.DownAttack,
		MoveLabel.SideAttack,
		MoveLabel.UpAttack,
		MoveLabel.DashAttack,
		MoveLabel.DownStrikeAttack,
		MoveLabel.SideStrikeAttack,
		MoveLabel.UpStrikeAttack,
		MoveLabel.BackwardAir,
		MoveLabel.ForwardAir,
		MoveLabel.NeutralAir,
		MoveLabel.UpAir,
		MoveLabel.DownAir,
		MoveLabel.DownSpecial,
		MoveLabel.NeutralSpecial,
		MoveLabel.SideSpecial,
		MoveLabel.UpSpecial,
		MoveLabel.Grab,
		MoveLabel.RunGrab,
		MoveLabel.PivotGrab,
		MoveLabel.GrabPummel,
		MoveLabel.ThrowBackward,
		MoveLabel.ThrowDown,
		MoveLabel.ThrowForward,
		MoveLabel.ThrowUp,
		MoveLabel.LedgeAttack,
		MoveLabel.LedgeJump,
		MoveLabel.LedgeRoll,
		MoveLabel.LedgeStand,
		MoveLabel.GetUp,
		MoveLabel.GetUpAttack,
		MoveLabel.GetUpBackward,
		MoveLabel.GetUpForward,
		MoveLabel.Tech,
		MoveLabel.TechForwardRoll,
		MoveLabel.TechBackwardRoll,
		MoveLabel.TechWall,
		MoveLabel.TechCeiling,
		MoveLabel.ShieldForwardRoll,
		MoveLabel.ShieldBackwardRoll,
		MoveLabel.Sidestep,
		MoveLabel.ShieldGust,
		MoveLabel.AirDodge,
		MoveLabel.WallJump,
		MoveLabel.ThrowItem,
		MoveLabel.ThrowItemLeft,
		MoveLabel.ThrowItemUp,
		MoveLabel.ThrowItemDown,
		MoveLabel.ThrowItemRight,
		MoveLabel.AssistAttack,
		MoveLabel.AllyAssist,
		MoveLabel.Hologram,
		MoveLabel.VoiceLineTaunt,
		MoveLabel.Emote
	};

	// Token: 0x0400142D RID: 5165
	private static Dictionary<MoveLabel, string> moveLabelNames = new Dictionary<MoveLabel, string>
	{
		{
			MoveLabel.NeutralAttack,
			"Jab"
		},
		{
			MoveLabel.DownAttack,
			"D Tilt"
		},
		{
			MoveLabel.SideAttack,
			"S Tilt"
		},
		{
			MoveLabel.UpAttack,
			"U Tilt"
		},
		{
			MoveLabel.DashAttack,
			"Dash Attack"
		},
		{
			MoveLabel.DownStrikeAttack,
			"D Strike"
		},
		{
			MoveLabel.SideStrikeAttack,
			"S Strike"
		},
		{
			MoveLabel.UpStrikeAttack,
			"U Strike"
		},
		{
			MoveLabel.BackwardAir,
			"B Aerial"
		},
		{
			MoveLabel.ForwardAir,
			"F Aerial"
		},
		{
			MoveLabel.NeutralAir,
			"N Aerial"
		},
		{
			MoveLabel.UpAir,
			"U Aerial"
		},
		{
			MoveLabel.DownAir,
			"D Aerial"
		},
		{
			MoveLabel.DownSpecial,
			"D Special"
		},
		{
			MoveLabel.NeutralSpecial,
			"N Special"
		},
		{
			MoveLabel.SideSpecial,
			"S Special"
		},
		{
			MoveLabel.UpSpecial,
			"U Special"
		},
		{
			MoveLabel.Grab,
			"Grab"
		},
		{
			MoveLabel.RunGrab,
			"Dash Grab"
		},
		{
			MoveLabel.PivotGrab,
			"Pivot Grab"
		},
		{
			MoveLabel.GrabPummel,
			"Grab Pummel"
		},
		{
			MoveLabel.ThrowBackward,
			"B Throw"
		},
		{
			MoveLabel.ThrowDown,
			"D Throw"
		},
		{
			MoveLabel.ThrowForward,
			"F Throw"
		},
		{
			MoveLabel.ThrowUp,
			"U Throw"
		},
		{
			MoveLabel.LedgeAttack,
			"Ledge Attack"
		},
		{
			MoveLabel.LedgeJump,
			"Ledge Jump"
		},
		{
			MoveLabel.LedgeRoll,
			"Ledge Roll"
		},
		{
			MoveLabel.LedgeStand,
			"Ledge Stand"
		},
		{
			MoveLabel.GetUpAttack,
			"Get Up Attack"
		},
		{
			MoveLabel.GetUpBackward,
			"Get Up B"
		},
		{
			MoveLabel.GetUpForward,
			"Get Up F"
		},
		{
			MoveLabel.Tech,
			"Tech N"
		},
		{
			MoveLabel.TechForwardRoll,
			"Tech F"
		},
		{
			MoveLabel.TechBackwardRoll,
			"Tech B"
		},
		{
			MoveLabel.TechWall,
			"Tech Wall"
		},
		{
			MoveLabel.TechCeiling,
			"Tech Ceiling"
		},
		{
			MoveLabel.ShieldForwardRoll,
			"F Roll"
		},
		{
			MoveLabel.ShieldBackwardRoll,
			"B Roll"
		},
		{
			MoveLabel.Sidestep,
			"Sidestep"
		},
		{
			MoveLabel.ShieldGust,
			"Gust Shield"
		},
		{
			MoveLabel.AirDodge,
			"Air Dodge"
		},
		{
			MoveLabel.WallJump,
			"Wall Jump"
		},
		{
			MoveLabel.VoiceLineTaunt,
			"Voice Line Taunt"
		},
		{
			MoveLabel.AllyAssist,
			"Crew Power Assist"
		},
		{
			MoveLabel.AssistAttack,
			"Tag In Move"
		}
	};
}
