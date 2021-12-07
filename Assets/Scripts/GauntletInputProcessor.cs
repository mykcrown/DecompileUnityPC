// Decompile from assembly: Assembly-CSharp.dll

using System;

public class GauntletInputProcessor : GameModeInputProcessor
{
	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	private GauntletEndGameCondition gauntletConditions
	{
		get
		{
			foreach (EndGameCondition current in this.gameController.currentGame.CurrentGameMode.EndGameConditions)
			{
				if (current is GauntletEndGameCondition)
				{
					return current as GauntletEndGameCondition;
				}
			}
			return null;
		}
	}

	public override void Start(GameModeData modeData, ConfigData config, IEvents events)
	{
		base.Start(modeData, config, events);
	}

	public override void ProcessInput(int currentFrame, InputController inputController, PlayerReference player, bool retainBuffer)
	{
		GauntletEndGameCondition gauntletConditions = this.gauntletConditions;
		if (gauntletConditions.WaitingForSpawn)
		{
			gauntletConditions.WaitingForSpawn = false;
			player.Controller.GauntletProceed();
		}
	}
}
