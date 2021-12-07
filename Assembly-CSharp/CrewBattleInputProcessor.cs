using System;

// Token: 0x0200049F RID: 1183
public class CrewBattleInputProcessor : GameModeInputProcessor
{
	// Token: 0x17000563 RID: 1379
	// (get) Token: 0x060019FF RID: 6655 RVA: 0x0008611D File Offset: 0x0008451D
	// (set) Token: 0x06001A00 RID: 6656 RVA: 0x00086125 File Offset: 0x00084525
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x17000564 RID: 1380
	// (get) Token: 0x06001A01 RID: 6657 RVA: 0x0008612E File Offset: 0x0008452E
	public GameManager gameManager
	{
		get
		{
			return this.gameController.currentGame;
		}
	}

	// Token: 0x06001A02 RID: 6658 RVA: 0x0008613B File Offset: 0x0008453B
	public override void Start(GameModeData modeData, ConfigData config, IEvents events)
	{
		base.Start(modeData, config, events);
		this.crewBattleData = (modeData.customData as CrewBattleCustomData);
	}

	// Token: 0x06001A03 RID: 6659 RVA: 0x00086158 File Offset: 0x00084558
	public override void ProcessInput(int currentFrame, InputController inputController, PlayerReference player, bool retainBuffer)
	{
		if (player.IsBenched && player.Lives > 0 && inputController.GetButtonDown(this.crewBattleData.tagInButton))
		{
			this.events.Broadcast(new RequestTagInCommand(player.PlayerNum, player.Team));
		}
		if (player.IsInBattle && player.Controller != null && player.Controller.State.IsDead && inputController != null && inputController.GetButtonDown(this.crewBattleData.tagInButton))
		{
			this.events.Broadcast(new RequestTagInCommand(player.PlayerNum, player.Team));
		}
		if (player.IsBenched && this.crewBattleData.friendlyAssistEnabled && inputController != null && inputController.GetButtonDown(this.crewBattleData.friendlyAssistButton))
		{
			this.events.Broadcast(new AttemptFriendlyAssistCommand(player.PlayerNum, player.Team));
		}
		if (player.IsBenched && this.crewBattleData.teamAbilitiesEnabled && inputController.GetButtonDown(this.crewBattleData.teamDynamicMoveButton))
		{
			this.events.Broadcast(new AttemptTeamDynamicMoveCommand(player.PlayerNum, player.Team, this.crewBattleData.teamAbilities.dynamicMoveParticle, this.crewBattleData.teamAbilities.dynamicMoveCooldownFrames));
		}
		if (player.IsBenched && this.crewBattleData.teamAbilitiesEnabled && inputController.GetButtonDown(this.crewBattleData.teamPowerMoveButton))
		{
			this.events.Broadcast(new AttemptTeamPowerMoveCommand(player.PlayerNum, player.Team, this.crewBattleData.teamAbilities.powerMoveParticle, this.crewBattleData.teamAbilities.powerMoveCooldownFrames, this.crewBattleData.teamAbilities.powerMoveImmuneFrames, this.crewBattleData.teamAbilities.assistArticles));
		}
		if (!player.IsBenched && this.crewBattleData.teamAbilitiesEnabled && inputController != null && inputController.GetButtonDown(ButtonPress.SoloAssist))
		{
			PlayerReference allyReferenceWithInvalidController = this.gameManager.getAllyReferenceWithInvalidController(player);
			if (allyReferenceWithInvalidController != null)
			{
				this.events.Broadcast(new AttemptTeamPowerMoveCommand(allyReferenceWithInvalidController.PlayerNum, allyReferenceWithInvalidController.Team, this.crewBattleData.teamAbilities.powerMoveParticle, this.crewBattleData.teamAbilities.powerMoveCooldownFrames, this.crewBattleData.teamAbilities.powerMoveImmuneFrames, this.crewBattleData.teamAbilities.assistArticles));
			}
		}
		if (!player.IsBenched && this.crewBattleData.teamAbilitiesEnabled && inputController != null && inputController.GetButtonDown(ButtonPress.SoloGust))
		{
			PlayerReference allyReferenceWithInvalidController2 = this.gameManager.getAllyReferenceWithInvalidController(player);
			if (allyReferenceWithInvalidController2 != null)
			{
				this.events.Broadcast(new AttemptTeamDynamicMoveCommand(allyReferenceWithInvalidController2.PlayerNum, allyReferenceWithInvalidController2.Team, this.crewBattleData.teamAbilities.dynamicMoveParticle, this.crewBattleData.teamAbilities.dynamicMoveCooldownFrames));
			}
		}
	}

	// Token: 0x04001375 RID: 4981
	private CrewBattleCustomData crewBattleData;
}
