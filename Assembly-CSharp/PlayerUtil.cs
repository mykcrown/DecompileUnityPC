using System;
using System.Collections.Generic;
using FixedPoint;
using IconsServer;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000B3F RID: 2879
public class PlayerUtil
{
	// Token: 0x060053A7 RID: 21415 RVA: 0x001AFA06 File Offset: 0x001ADE06
	public static ECharacterType GetCharacterTypeFromCharacterID(CharacterID characterID)
	{
		return (ECharacterType)(characterID - 1);
	}

	// Token: 0x060053A8 RID: 21416 RVA: 0x001AFA0C File Offset: 0x001ADE0C
	public static int FirstEquipmentSlotForType(EquipmentTypes type)
	{
		switch (type)
		{
		case EquipmentTypes.SKIN:
		case EquipmentTypes.NETSUKE:
			return 0;
		case EquipmentTypes.EMOTE:
		case EquipmentTypes.HOLOGRAM:
		case EquipmentTypes.VOICE_TAUNT:
		case EquipmentTypes.TOKEN:
			return 3;
		case EquipmentTypes.VICTORY_POSE:
			return 2;
		case EquipmentTypes.PLATFORM:
			return 1;
		case EquipmentTypes.PLAYER_ICON:
			return 4;
		case EquipmentTypes.BLAST_ZONE:
			return 5;
		default:
			return -1;
		}
	}

	// Token: 0x060053A9 RID: 21417 RVA: 0x001AFA59 File Offset: 0x001ADE59
	public static CharacterID GetCharacterIDFromCharacterType(ECharacterType characterType)
	{
		return (CharacterID)(characterType + 1);
	}

	// Token: 0x060053AA RID: 21418 RVA: 0x001AFA60 File Offset: 0x001ADE60
	public static PlayerNum GetPointerEventOwner(PointerEventData eventData)
	{
		int pointerId = 0;
		if (eventData != null)
		{
			pointerId = eventData.pointerId;
		}
		return PlayerUtil.GetPlayerNumFromPointer(pointerId);
	}

	// Token: 0x060053AB RID: 21419 RVA: 0x001AFA84 File Offset: 0x001ADE84
	public static string GetPlayerNumText(ILocalization localization, PlayerSelectionInfo playerInfo)
	{
		PlayerType type = playerInfo.type;
		if (type == PlayerType.Human)
		{
			return localization.GetText("ui.characterSelect.playerNumDisplay", PlayerUtil.GetIntFromPlayerNum(playerInfo.playerNum, false) + string.Empty);
		}
		if (type != PlayerType.CPU)
		{
			return string.Empty;
		}
		return localization.GetText("ui.characterSelect.cpuNumDisplay");
	}

	// Token: 0x060053AC RID: 21420 RVA: 0x001AFAE4 File Offset: 0x001ADEE4
	public static string GetPlayerName(ILocalization localization, PlayerController playerController)
	{
		if (playerController == null)
		{
			return string.Empty;
		}
		if (playerController.Reference.Type == PlayerType.CPU)
		{
			return localization.GetText("ui.characterSelect.cpuNumDisplay");
		}
		if (playerController.thisProfile != null && playerController.thisProfile.profileName != null)
		{
			return playerController.thisProfile.profileName;
		}
		if (playerController.thisProfile != null && playerController.thisProfile.localName != null)
		{
			return playerController.thisProfile.localName;
		}
		return localization.GetText("ui.characterSelect.playerName", PlayerUtil.GetIntFromPlayerNum(playerController.PlayerNum, false) + string.Empty);
	}

	// Token: 0x060053AD RID: 21421 RVA: 0x001AFB94 File Offset: 0x001ADF94
	public static string GetPlayerNametag(ILocalization localization, PlayerController playerController)
	{
		if (playerController.Reference.Type == PlayerType.CPU)
		{
			return localization.GetText("ui.characterSelect.cpuNumDisplay");
		}
		if (PlayerUtil.IsProfileNametag(playerController))
		{
			return playerController.thisProfile.profileName;
		}
		if (PlayerUtil.IsLocalNametag(playerController))
		{
			return playerController.thisProfile.localName;
		}
		return localization.GetText("ui.characterSelect.playerNumDisplay", PlayerUtil.GetIntFromPlayerNum(playerController.PlayerNum, false) + string.Empty);
	}

	// Token: 0x060053AE RID: 21422 RVA: 0x001AFC12 File Offset: 0x001AE012
	public static bool IsNametag(PlayerController playerController)
	{
		return PlayerUtil.IsProfileNametag(playerController) || PlayerUtil.IsLocalNametag(playerController);
	}

	// Token: 0x060053AF RID: 21423 RVA: 0x001AFC28 File Offset: 0x001AE028
	public static bool IsProfileNametag(PlayerController playerController)
	{
		return PlayerUtil.EnableOnlineNameTags && playerController.Reference.Type == PlayerType.Human && playerController.thisProfile != null && playerController.thisProfile.profileName != null;
	}

	// Token: 0x060053B0 RID: 21424 RVA: 0x001AFC62 File Offset: 0x001AE062
	public static bool IsLocalNametag(PlayerController playerController)
	{
		return playerController.Reference.Type == PlayerType.Human && playerController.thisProfile != null && !string.IsNullOrEmpty(playerController.thisProfile.localName);
	}

	// Token: 0x060053B1 RID: 21425 RVA: 0x001AFC98 File Offset: 0x001AE098
	public static string GetPlayerNametag(ILocalization localization, PlayerSelectionInfo info, bool useLongNames = false)
	{
		if (info.type == PlayerType.CPU)
		{
			return localization.GetText("ui.characterSelect.cpuNumDisplay");
		}
		if ((useLongNames || PlayerUtil.EnableOnlineNameTags) && info.curProfile != null && info.curProfile.profileName != null)
		{
			return info.curProfile.profileName;
		}
		if (info.curProfile != null && info.curProfile.localName != null)
		{
			return info.curProfile.localName;
		}
		if (useLongNames)
		{
			return localization.GetText("ui.characterSelect.playerName", PlayerUtil.GetIntFromPlayerNum(info.playerNum, false) + string.Empty);
		}
		return localization.GetText("ui.characterSelect.playerNumDisplay", PlayerUtil.GetIntFromPlayerNum(info.playerNum, false) + string.Empty);
	}

	// Token: 0x060053B2 RID: 21426 RVA: 0x001AFD6E File Offset: 0x001AE16E
	public static int GetIntFromPlayerNum(PlayerNum player, bool isIndex = false)
	{
		return (!isIndex) ? PlayerUtil.playerNumToIntTable[(int)player] : (PlayerUtil.playerNumToIntTable[(int)player] - 1);
	}

	// Token: 0x060053B3 RID: 21427 RVA: 0x001AFD8B File Offset: 0x001AE18B
	public static PlayerNum GetPlayerNumFromPointer(int pointerId)
	{
		if (pointerId == -1)
		{
			return PlayerNum.All;
		}
		return PlayerUtil.GetPlayerNumFromInt(pointerId, false);
	}

	// Token: 0x060053B4 RID: 21428 RVA: 0x001AFD9D File Offset: 0x001AE19D
	public static PlayerNum GetPlayerNumFromInt(int player, bool isIndex = false)
	{
		if (isIndex)
		{
			player++;
		}
		return PlayerUtil.intToPlayerNumTable[player];
	}

	// Token: 0x060053B5 RID: 21429 RVA: 0x001AFDB1 File Offset: 0x001AE1B1
	public static Color GetColorFromPlayerNum(PlayerNum player)
	{
		return PlayerUtil.GetColorFromUIColor(PlayerUtil.GetUIColorFromPlayerNum(player));
	}

	// Token: 0x060053B6 RID: 21430 RVA: 0x001AFDBE File Offset: 0x001AE1BE
	public static UIColor GetUIColorFromPlayerNum(PlayerNum player)
	{
		switch (player)
		{
		default:
			return UIColor.Red;
		case PlayerNum.Player2:
			return UIColor.Blue;
		case PlayerNum.Player3:
			return UIColor.Yellow;
		case PlayerNum.Player4:
			return UIColor.Green;
		case PlayerNum.Player5:
			return UIColor.Purple;
		case PlayerNum.Player6:
			return UIColor.Pink;
		case PlayerNum.Player7:
			return UIColor.Red;
		case PlayerNum.Player8:
			return UIColor.Yellow;
		}
	}

	// Token: 0x060053B7 RID: 21431 RVA: 0x001AFDFA File Offset: 0x001AE1FA
	public static Color GetColorFromTeam(TeamNum team)
	{
		return PlayerUtil.GetColorFromUIColor(PlayerUtil.GetUIColorFromTeam(team));
	}

	// Token: 0x060053B8 RID: 21432 RVA: 0x001AFE08 File Offset: 0x001AE208
	public static Color GetColorFromUIColor(UIColor color)
	{
		switch (color)
		{
		default:
			return WColor.UIBlue;
		case UIColor.Red:
			return WColor.UIRed;
		case UIColor.Yellow:
			return WColor.UIYellow;
		case UIColor.Green:
			return WColor.UIGreen;
		case UIColor.Purple:
			return WColor.UIPurple;
		case UIColor.Pink:
			return WColor.UIPink;
		case UIColor.Grey:
			return WColor.UIGrey;
		}
	}

	// Token: 0x060053B9 RID: 21433 RVA: 0x001AFE68 File Offset: 0x001AE268
	public static Color GetLightColorFromUIColor(UIColor color)
	{
		switch (color)
		{
		default:
			return WColor.UILightBlue;
		case UIColor.Red:
			return WColor.UILightRed;
		case UIColor.Yellow:
			return WColor.UILightYellow;
		case UIColor.Green:
			return WColor.UILightGreen;
		case UIColor.Purple:
			return WColor.UIPurple;
		case UIColor.Pink:
			return WColor.UIPink;
		case UIColor.Grey:
			return WColor.UIGrey;
		}
	}

	// Token: 0x060053BA RID: 21434 RVA: 0x001AFEC5 File Offset: 0x001AE2C5
	public static UIColor GetUIColorFromTeam(TeamNum team)
	{
		switch (team)
		{
		case TeamNum.Team1:
			return UIColor.Red;
		case TeamNum.Team2:
			return UIColor.Blue;
		case TeamNum.Team3:
			return UIColor.Yellow;
		case TeamNum.Team4:
			return UIColor.Green;
		case TeamNum.Team5:
			return UIColor.Red;
		case TeamNum.Team6:
			return UIColor.Blue;
		case TeamNum.Team7:
			return UIColor.Yellow;
		case TeamNum.Team8:
			return UIColor.Green;
		default:
			return UIColor.Blue;
		}
	}

	// Token: 0x060053BB RID: 21435 RVA: 0x001AFF04 File Offset: 0x001AE304
	public static string GetNameFromUIColor(UIColor color)
	{
		switch (color)
		{
		case UIColor.Blue:
			return "Blue";
		case UIColor.Red:
			return "Red";
		case UIColor.Yellow:
			return "Yellow";
		case UIColor.Green:
			return "Green";
		case UIColor.Grey:
			return "Grey";
		}
		return "Blue";
	}

	// Token: 0x060053BC RID: 21436 RVA: 0x001AFF5B File Offset: 0x001AE35B
	public static string GetNameFromTeam(TeamNum team)
	{
		return PlayerUtil.GetNameFromUIColor(PlayerUtil.GetUIColorFromTeam(team));
	}

	// Token: 0x060053BD RID: 21437 RVA: 0x001AFF68 File Offset: 0x001AE368
	public static int GetIntFromTeamNum(TeamNum team)
	{
		switch (team)
		{
		case TeamNum.Team1:
			return 1;
		case TeamNum.Team2:
			return 2;
		case TeamNum.Team3:
			return 3;
		case TeamNum.Team4:
			return 4;
		case TeamNum.Team5:
			return 5;
		case TeamNum.Team6:
			return 6;
		case TeamNum.Team7:
			return 7;
		case TeamNum.Team8:
			return 8;
		default:
			return 1;
		}
	}

	// Token: 0x060053BE RID: 21438 RVA: 0x001AFFA8 File Offset: 0x001AE3A8
	public static TeamNum GetTeamNumFromInt(int team, bool isIndex = false)
	{
		if (isIndex)
		{
			team++;
		}
		switch (team)
		{
		case 1:
			return TeamNum.Team1;
		case 2:
			return TeamNum.Team2;
		case 3:
			return TeamNum.Team3;
		case 4:
			return TeamNum.Team4;
		case 5:
			return TeamNum.Team5;
		case 6:
			return TeamNum.Team6;
		case 7:
			return TeamNum.Team7;
		case 8:
			return TeamNum.Team8;
		default:
			return TeamNum.Team1;
		}
	}

	// Token: 0x060053BF RID: 21439 RVA: 0x001B0000 File Offset: 0x001AE400
	public static Color GetColor(PlayerSelectionInfo info, bool usesTeams = false)
	{
		if (info == null || info.type == PlayerType.None)
		{
			return WColor.WDWhite;
		}
		if (info.team == TeamNum.None || !usesTeams)
		{
			return PlayerUtil.GetColorFromPlayerNum(info.playerNum);
		}
		return PlayerUtil.GetColorFromTeam(info.team);
	}

	// Token: 0x060053C0 RID: 21440 RVA: 0x001B004F File Offset: 0x001AE44F
	public static UIColor GetUIColor(PlayerSelectionInfo info, bool usesTeams = false)
	{
		if (info == null || info.type == PlayerType.None)
		{
			return UIColor.Red;
		}
		return PlayerUtil.GetUIColor(info.playerNum, info.team, info.type, usesTeams);
	}

	// Token: 0x060053C1 RID: 21441 RVA: 0x001B007D File Offset: 0x001AE47D
	public static UIColor GetUIColor(PlayerController controller, bool usesTeams = false)
	{
		if (controller == null)
		{
			return UIColor.Red;
		}
		return PlayerUtil.GetUIColor(controller.PlayerNum, controller.Team, controller.Reference.Type, usesTeams);
	}

	// Token: 0x060053C2 RID: 21442 RVA: 0x001B00AA File Offset: 0x001AE4AA
	public static UIColor GetUIColor(PlayerNum player, TeamNum team, PlayerType type, bool usesTeams = false)
	{
		if (team == TeamNum.None || !usesTeams)
		{
			return PlayerUtil.GetUIColorFromPlayerNum(player);
		}
		return PlayerUtil.GetUIColorFromTeam(team);
	}

	// Token: 0x060053C3 RID: 21443 RVA: 0x001B00C7 File Offset: 0x001AE4C7
	public static bool IsValidTeam(TeamNum teamNum)
	{
		return teamNum != TeamNum.None && teamNum != TeamNum.All;
	}

	// Token: 0x060053C4 RID: 21444 RVA: 0x001B00DB File Offset: 0x001AE4DB
	public static bool IsValidPlayer(PlayerNum player)
	{
		return player != PlayerNum.None && player != PlayerNum.All;
	}

	// Token: 0x060053C5 RID: 21445 RVA: 0x001B00F0 File Offset: 0x001AE4F0
	public static PlayerReference FindClosestEnemy(PlayerController player, List<PlayerReference> references)
	{
		PlayerReference result = null;
		float num = 1000000f;
		for (int i = 0; i < references.Count; i++)
		{
			PlayerReference playerReference = references[i];
			if (!(playerReference.Controller == null) && !playerReference.IsAllyAssistMove && playerReference.IsInBattle && playerReference.Controller.IsActive)
			{
				if ((player.Team == TeamNum.None || playerReference.Team != player.Team) && !playerReference.Controller.State.IsDead && !playerReference.Controller.State.IsRespawning)
				{
					float sqrMagnitude = (player.transform.position - playerReference.Controller.transform.position).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						result = playerReference;
						num = sqrMagnitude;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x060053C6 RID: 21446 RVA: 0x001B01E8 File Offset: 0x001AE5E8
	public static bool IsOffstage(IPlayerDelegate player, GameManager gameManager, Fixed velocityAdjustment)
	{
		if (gameManager.Physics == null)
		{
			return false;
		}
		Fixed maxDistance = (Fixed)100.0;
		int num = gameManager.PhysicsWorld.RaycastTerrain(player.Physics.Center, Vector3F.down, maxDistance, PhysicsSimulator.GroundAndPlatformMask, null, RaycastFlags.Default, default(Fixed));
		if (num > 0)
		{
			return false;
		}
		num += gameManager.PhysicsWorld.RaycastTerrain(player.Physics.Center + player.Physics.Velocity * velocityAdjustment, Vector3F.down, maxDistance, PhysicsSimulator.GroundAndPlatformMask, null, RaycastFlags.Default, default(Fixed));
		return num == 0;
	}

	// Token: 0x04003511 RID: 13585
	private static bool EnableOnlineNameTags = true;

	// Token: 0x04003512 RID: 13586
	public const uint TotalNetsukeEquipmentSlots = 3U;

	// Token: 0x04003513 RID: 13587
	public const uint FirstTauntEquipmentSlot = 3U;

	// Token: 0x04003514 RID: 13588
	private static readonly int[] playerNumToIntTable = new int[]
	{
		1,
		2,
		3,
		4,
		5,
		6,
		7,
		8,
		0,
		1
	};

	// Token: 0x04003515 RID: 13589
	private static readonly PlayerNum[] intToPlayerNumTable = new PlayerNum[]
	{
		PlayerNum.All,
		PlayerNum.Player1,
		PlayerNum.Player2,
		PlayerNum.Player3,
		PlayerNum.Player4,
		PlayerNum.Player5,
		PlayerNum.Player6,
		PlayerNum.Player7,
		PlayerNum.Player8,
		PlayerNum.Player1
	};
}
