using System;

// Token: 0x020004A9 RID: 1193
public class GauntletInputProcessor : GameModeInputProcessor
{
	// Token: 0x17000575 RID: 1397
	// (get) Token: 0x06001A5F RID: 6751 RVA: 0x000891C8 File Offset: 0x000875C8
	// (set) Token: 0x06001A60 RID: 6752 RVA: 0x000891D0 File Offset: 0x000875D0
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x06001A61 RID: 6753 RVA: 0x000891D9 File Offset: 0x000875D9
	public override void Start(GameModeData modeData, ConfigData config, IEvents events)
	{
		base.Start(modeData, config, events);
	}

	// Token: 0x17000576 RID: 1398
	// (get) Token: 0x06001A62 RID: 6754 RVA: 0x000891E4 File Offset: 0x000875E4
	private GauntletEndGameCondition gauntletConditions
	{
		get
		{
			foreach (EndGameCondition endGameCondition in this.gameController.currentGame.CurrentGameMode.EndGameConditions)
			{
				if (endGameCondition is GauntletEndGameCondition)
				{
					return endGameCondition as GauntletEndGameCondition;
				}
			}
			return null;
		}
	}

	// Token: 0x06001A63 RID: 6755 RVA: 0x00089264 File Offset: 0x00087664
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
