// Decompile from assembly: Assembly-CSharp.dll

using System;

public class LivesModeInputProcessor : GameModeInputProcessor
{
	public override void ProcessInput(int currentFrame, InputController inputController, PlayerReference player, bool retainBuffer)
	{
		if (player.IsEliminated && this.modeData.settings.usesTeams && this.modeData.settings.allowLifeSharing && inputController.GetButtonDown(ButtonPress.Start))
		{
			this.events.Broadcast(new ShareLifeCommand(player.PlayerNum, player.Team));
		}
	}
}
