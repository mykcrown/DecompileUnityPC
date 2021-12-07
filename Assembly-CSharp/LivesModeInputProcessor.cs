using System;

// Token: 0x02000693 RID: 1683
public class LivesModeInputProcessor : GameModeInputProcessor
{
	// Token: 0x0600298A RID: 10634 RVA: 0x000DDC10 File Offset: 0x000DC010
	public override void ProcessInput(int currentFrame, InputController inputController, PlayerReference player, bool retainBuffer)
	{
		if (player.IsEliminated && this.modeData.settings.usesTeams && this.modeData.settings.allowLifeSharing && inputController.GetButtonDown(ButtonPress.Start))
		{
			this.events.Broadcast(new ShareLifeCommand(player.PlayerNum, player.Team));
		}
	}
}
