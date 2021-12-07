// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerUtil
{
	private static bool EnableOnlineNameTags = true;

	public const uint TotalNetsukeEquipmentSlots = 3u;

	public const uint FirstTauntEquipmentSlot = 3u;

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

	public static ECharacterType GetCharacterTypeFromCharacterID(CharacterID characterID)
	{
		return (ECharacterType)(characterID - 1);
	}

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

	public static CharacterID GetCharacterIDFromCharacterType(ECharacterType characterType)
	{
		return (CharacterID)(characterType + 1);
	}

	public static PlayerNum GetPointerEventOwner(PointerEventData eventData)
	{
		int pointerId = 0;
		if (eventData != null)
		{
			pointerId = eventData.pointerId;
		}
		return PlayerUtil.GetPlayerNumFromPointer(pointerId);
	}

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

	public static bool IsNametag(PlayerController playerController)
	{
		return PlayerUtil.IsProfileNametag(playerController) || PlayerUtil.IsLocalNametag(playerController);
	}

	public static bool IsProfileNametag(PlayerController playerController)
	{
		return PlayerUtil.EnableOnlineNameTags && playerController.Reference.Type == PlayerType.Human && playerController.thisProfile != null && playerController.thisProfile.profileName != null;
	}

	public static bool IsLocalNametag(PlayerController playerController)
	{
		return playerController.Reference.Type == PlayerType.Human && playerController.thisProfile != null && !string.IsNullOrEmpty(playerController.thisProfile.localName);
	}

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

	public static int GetIntFromPlayerNum(PlayerNum player, bool isIndex = false)
	{
		return (!isIndex) ? PlayerUtil.playerNumToIntTable[(int)player] : (PlayerUtil.playerNumToIntTable[(int)player] - 1);
	}

	public static PlayerNum GetPlayerNumFromPointer(int pointerId)
	{
		if (pointerId == -1)
		{
			return PlayerNum.All;
		}
		return PlayerUtil.GetPlayerNumFromInt(pointerId, false);
	}

	public static PlayerNum GetPlayerNumFromInt(int player, bool isIndex = false)
	{
		if (isIndex)
		{
			player++;
		}
		return PlayerUtil.intToPlayerNumTable[player];
	}

	public static Color GetColorFromPlayerNum(PlayerNum player)
	{
		return PlayerUtil.GetColorFromUIColor(PlayerUtil.GetUIColorFromPlayerNum(player));
	}

	public static UIColor GetUIColorFromPlayerNum(PlayerNum player)
	{
		switch (player)
		{
		case PlayerNum.Player1:
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
		return UIColor.Red;
	}

	public static Color GetColorFromTeam(TeamNum team)
	{
		return PlayerUtil.GetColorFromUIColor(PlayerUtil.GetUIColorFromTeam(team));
	}

	public static Color GetColorFromUIColor(UIColor color)
	{
		switch (color)
		{
		case UIColor.Blue:
			IL_27:
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
		goto IL_27;
	}

	public static Color GetLightColorFromUIColor(UIColor color)
	{
		switch (color)
		{
		case UIColor.Blue:
			IL_27:
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
		goto IL_27;
	}

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

	public static string GetNameFromTeam(TeamNum team)
	{
		return PlayerUtil.GetNameFromUIColor(PlayerUtil.GetUIColorFromTeam(team));
	}

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

	public static UIColor GetUIColor(PlayerSelectionInfo info, bool usesTeams = false)
	{
		if (info == null || info.type == PlayerType.None)
		{
			return UIColor.Red;
		}
		return PlayerUtil.GetUIColor(info.playerNum, info.team, info.type, usesTeams);
	}

	public static UIColor GetUIColor(PlayerController controller, bool usesTeams = false)
	{
		if (controller == null)
		{
			return UIColor.Red;
		}
		return PlayerUtil.GetUIColor(controller.PlayerNum, controller.Team, controller.Reference.Type, usesTeams);
	}

	public static UIColor GetUIColor(PlayerNum player, TeamNum team, PlayerType type, bool usesTeams = false)
	{
		if (team == TeamNum.None || !usesTeams)
		{
			return PlayerUtil.GetUIColorFromPlayerNum(player);
		}
		return PlayerUtil.GetUIColorFromTeam(team);
	}

	public static bool IsValidTeam(TeamNum teamNum)
	{
		return teamNum != TeamNum.None && teamNum != TeamNum.All;
	}

	public static bool IsValidPlayer(PlayerNum player)
	{
		return player != PlayerNum.None && player != PlayerNum.All;
	}

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
}
