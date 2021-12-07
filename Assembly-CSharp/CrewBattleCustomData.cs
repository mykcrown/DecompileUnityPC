using System;

// Token: 0x0200049E RID: 1182
[GameModeDataType(GameMode.CrewBattle)]
public class CrewBattleCustomData : CustomGameModeData
{
	// Token: 0x060019FD RID: 6653 RVA: 0x000860E8 File Offset: 0x000844E8
	public override void RegisterPreload(PreloadContext context)
	{
		this.teamAbilities.RegisterPreload(context);
	}

	// Token: 0x0400135B RID: 4955
	public int respawnDelayFrames = 120;

	// Token: 0x0400135C RID: 4956
	public int benchedPlayerTagInDelayFrames = 120;

	// Token: 0x0400135D RID: 4957
	public int respawnBaseWait = 120;

	// Token: 0x0400135E RID: 4958
	public ButtonPress tagInButton = ButtonPress.Special;

	// Token: 0x0400135F RID: 4959
	public ButtonPress teamDynamicMoveButton = ButtonPress.Strike;

	// Token: 0x04001360 RID: 4960
	public ButtonPress teamPowerMoveButton = ButtonPress.Attack;

	// Token: 0x04001361 RID: 4961
	public bool rotateTagInPriority;

	// Token: 0x04001362 RID: 4962
	public bool allPlayersRotate;

	// Token: 0x04001363 RID: 4963
	public bool friendlyAssistEnabled = true;

	// Token: 0x04001364 RID: 4964
	public bool teamAbilitiesEnabled = true;

	// Token: 0x04001365 RID: 4965
	public TeamAbilityConfig teamAbilities = new TeamAbilityConfig();

	// Token: 0x04001366 RID: 4966
	public int friendlyAssistDurationFrames = 1200;

	// Token: 0x04001367 RID: 4967
	public int maxSimultaneousAssists = 1;

	// Token: 0x04001368 RID: 4968
	public ButtonPress friendlyAssistButton = ButtonPress.Start;

	// Token: 0x04001369 RID: 4969
	public int assistPlayerBaseSpawnDamage = 50;

	// Token: 0x0400136A RID: 4970
	public int assistBlastZoneImmunityFrames = 10;

	// Token: 0x0400136B RID: 4971
	public int assistTeammateDeathRefundFrames = 60;

	// Token: 0x0400136C RID: 4972
	public bool defaultAssistStealOn;

	// Token: 0x0400136D RID: 4973
	public bool endAssistOnKill = true;

	// Token: 0x0400136E RID: 4974
	public int grantFramesOnKill = 240;

	// Token: 0x0400136F RID: 4975
	public bool endAssistOnTeammateDeath = true;

	// Token: 0x04001370 RID: 4976
	public int endAssistDelayFrames = 10;

	// Token: 0x04001371 RID: 4977
	public int individualPlayerAssistCooldown;

	// Token: 0x04001372 RID: 4978
	public int teamAssistCooldown;

	// Token: 0x04001373 RID: 4979
	public int enemyAssistCooldown;

	// Token: 0x04001374 RID: 4980
	public int simultaneousAssistWindow = 10;
}
