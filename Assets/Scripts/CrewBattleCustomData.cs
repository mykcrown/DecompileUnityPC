// Decompile from assembly: Assembly-CSharp.dll

using System;

[GameModeDataType(GameMode.CrewBattle)]
public class CrewBattleCustomData : CustomGameModeData
{
	public int respawnDelayFrames = 120;

	public int benchedPlayerTagInDelayFrames = 120;

	public int respawnBaseWait = 120;

	public ButtonPress tagInButton = ButtonPress.Special;

	public ButtonPress teamDynamicMoveButton = ButtonPress.Strike;

	public ButtonPress teamPowerMoveButton = ButtonPress.Attack;

	public bool rotateTagInPriority;

	public bool allPlayersRotate;

	public bool friendlyAssistEnabled = true;

	public bool teamAbilitiesEnabled = true;

	public TeamAbilityConfig teamAbilities = new TeamAbilityConfig();

	public int friendlyAssistDurationFrames = 1200;

	public int maxSimultaneousAssists = 1;

	public ButtonPress friendlyAssistButton = ButtonPress.Start;

	public int assistPlayerBaseSpawnDamage = 50;

	public int assistBlastZoneImmunityFrames = 10;

	public int assistTeammateDeathRefundFrames = 60;

	public bool defaultAssistStealOn;

	public bool endAssistOnKill = true;

	public int grantFramesOnKill = 240;

	public bool endAssistOnTeammateDeath = true;

	public int endAssistDelayFrames = 10;

	public int individualPlayerAssistCooldown;

	public int teamAssistCooldown;

	public int enemyAssistCooldown;

	public int simultaneousAssistWindow = 10;

	public override void RegisterPreload(PreloadContext context)
	{
		this.teamAbilities.RegisterPreload(context);
	}
}
