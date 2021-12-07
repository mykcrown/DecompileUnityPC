// Decompile from assembly: Assembly-CSharp.dll

using System;

public class PlayerDeathTrigger : StageTrigger
{
	public enum LivesLeftTriggerType
	{
		Off,
		PerPlayer,
		AllPlayers
	}

	public bool triggersOnElimination;

	public PlayerNum playerNum = PlayerNum.All;

	public PlayerDeathTrigger.LivesLeftTriggerType livesLeftTriggerType;

	public int livesLeft;

	public override void Init(IStageTriggerDependency triggerDependency, bool isSimulation)
	{
		base.Init(triggerDependency, isSimulation);
		base.events.Subscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
	}

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
			foreach (PlayerController current in this.triggerDependency.GetPlayers())
			{
				if (this.livesLeftTriggerType == PlayerDeathTrigger.LivesLeftTriggerType.PerPlayer && this.playerNum == current.PlayerNum)
				{
					num += current.Lives;
				}
				else if (this.livesLeftTriggerType == PlayerDeathTrigger.LivesLeftTriggerType.AllPlayers)
				{
					num += current.Lives;
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

	public override void Destroy()
	{
		base.Destroy();
		if (this.triggerDependency != null && base.events != null)
		{
			base.events.Unsubscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
		}
	}
}
