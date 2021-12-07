// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class ComboManager : ITickable, IRollbackStateOwner
{
	private List<ComboTracker> comboTrackers = new List<ComboTracker>();

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public IDependencyInjection injector
	{
		get;
		set;
	}

	public void Setup(List<PlayerReference> players)
	{
		for (int i = 0; i < players.Count; i++)
		{
			ComboTracker instance = this.injector.GetInstance<ComboTracker>();
			instance.Setup(players[i].PlayerNum, players[i].Team, players);
			this.comboTrackers.Add(instance);
		}
		this.events.Subscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
	}

	public void Destroy()
	{
		for (int i = 0; i < this.comboTrackers.Count; i++)
		{
			ComboTracker comboTracker = this.comboTrackers[i];
			comboTracker.Destroy();
		}
		this.comboTrackers.Clear();
		this.events.Unsubscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
	}

	public ComboTracker GetTracker(PlayerNum player)
	{
		for (int i = 0; i < this.comboTrackers.Count; i++)
		{
			ComboTracker comboTracker = this.comboTrackers[i];
			if (comboTracker.Player == player)
			{
				return comboTracker;
			}
		}
		return null;
	}

	public bool LoadState(RollbackStateContainer container)
	{
		foreach (ComboTracker current in this.comboTrackers)
		{
			current.LoadState(container);
		}
		return true;
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		foreach (ComboTracker current in this.comboTrackers)
		{
			current.ExportState(ref container);
		}
		return true;
	}

	public void TickFrame()
	{
		for (int i = 0; i < this.comboTrackers.Count; i++)
		{
			ComboTracker comboTracker = this.comboTrackers[i];
			comboTracker.TickFrame();
		}
	}

	private void onCharacterDeath(GameEvent message)
	{
		CharacterDeathEvent characterDeathEvent = message as CharacterDeathEvent;
		bool flag = false;
		for (int i = 0; i < this.comboTrackers.Count; i++)
		{
			ComboTracker comboTracker = this.comboTrackers[i];
			flag |= comboTracker.OnCharacterDeath(characterDeathEvent.character.PlayerNum, characterDeathEvent.character.Team);
		}
		if (!flag)
		{
			this.events.Broadcast(new LogStatEvent(StatType.Suicide, 1, PointsValueType.Addition, characterDeathEvent.character.PlayerNum, characterDeathEvent.character.Team));
		}
	}
}
