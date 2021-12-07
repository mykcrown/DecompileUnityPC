// Decompile from assembly: Assembly-CSharp.dll

using System;

public class CrewBattleInputProcessor : GameModeInputProcessor
{
	private CrewBattleCustomData crewBattleData;

	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	public GameManager gameManager
	{
		get
		{
			return this.gameController.currentGame;
		}
	}

	public override void Start(GameModeData modeData, ConfigData config, IEvents events)
	{
		base.Start(modeData, config, events);
		this.crewBattleData = (modeData.customData as CrewBattleCustomData);
	}

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
}
