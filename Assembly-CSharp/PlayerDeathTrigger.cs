using System;

// Token: 0x02000634 RID: 1588
public class PlayerDeathTrigger : StageTrigger
{
	// Token: 0x060026FA RID: 9978 RVA: 0x000BEA31 File Offset: 0x000BCE31
	public override void Init(IStageTriggerDependency triggerDependency, bool isSimulation)
	{
		base.Init(triggerDependency, isSimulation);
		base.events.Subscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
	}

	// Token: 0x060026FB RID: 9979 RVA: 0x000BEA5C File Offset: 0x000BCE5C
	private void onCharacterDeath(GameEvent message)
	{
		CharacterDeathEvent characterDeathEvent = message as CharacterDeathEvent;
		bool flag = true;
		if (this.playerNum != characterDeathEvent.character.PlayerNum && this.playerNum != PlayerNum.All)
		{
			flag = false;
		}
		if (this.triggersOnElimination && !characterDeathEvent.wasEliminated)
		{
			flag = false;
		}
		if (this.livesLeftTriggerType != PlayerDeathTrigger.LivesLeftTriggerType.Off)
		{
			int num = 0;
			foreach (PlayerController playerController in this.triggerDependency.GetPlayers())
			{
				if (this.livesLeftTriggerType == PlayerDeathTrigger.LivesLeftTriggerType.PerPlayer && this.playerNum == playerController.PlayerNum)
				{
					num += playerController.Lives;
				}
				else if (this.livesLeftTriggerType == PlayerDeathTrigger.LivesLeftTriggerType.AllPlayers)
				{
					num += playerController.Lives;
				}
			}
			if (num != this.livesLeft)
			{
				flag = false;
			}
		}
		if (flag && this.Triggered != null)
		{
			base.CallTriggered(null);
		}
	}

	// Token: 0x060026FC RID: 9980 RVA: 0x000BEB70 File Offset: 0x000BCF70
	public override void Destroy()
	{
		base.Destroy();
		if (this.triggerDependency != null && base.events != null)
		{
			base.events.Unsubscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
		}
	}

	// Token: 0x04001C85 RID: 7301
	public bool triggersOnElimination;

	// Token: 0x04001C86 RID: 7302
	public PlayerNum playerNum = PlayerNum.All;

	// Token: 0x04001C87 RID: 7303
	public PlayerDeathTrigger.LivesLeftTriggerType livesLeftTriggerType;

	// Token: 0x04001C88 RID: 7304
	public int livesLeft;

	// Token: 0x02000635 RID: 1589
	public enum LivesLeftTriggerType
	{
		// Token: 0x04001C8A RID: 7306
		Off,
		// Token: 0x04001C8B RID: 7307
		PerPlayer,
		// Token: 0x04001C8C RID: 7308
		AllPlayers
	}
}
