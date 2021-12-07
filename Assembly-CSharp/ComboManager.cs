using System;
using System.Collections.Generic;

// Token: 0x02000665 RID: 1637
public class ComboManager : ITickable, IRollbackStateOwner
{
	// Token: 0x170009D2 RID: 2514
	// (get) Token: 0x0600280D RID: 10253 RVA: 0x000C2C12 File Offset: 0x000C1012
	// (set) Token: 0x0600280E RID: 10254 RVA: 0x000C2C1A File Offset: 0x000C101A
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x170009D3 RID: 2515
	// (get) Token: 0x0600280F RID: 10255 RVA: 0x000C2C23 File Offset: 0x000C1023
	// (set) Token: 0x06002810 RID: 10256 RVA: 0x000C2C2B File Offset: 0x000C102B
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x06002811 RID: 10257 RVA: 0x000C2C34 File Offset: 0x000C1034
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

	// Token: 0x06002812 RID: 10258 RVA: 0x000C2CB0 File Offset: 0x000C10B0
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

	// Token: 0x06002813 RID: 10259 RVA: 0x000C2D18 File Offset: 0x000C1118
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

	// Token: 0x06002814 RID: 10260 RVA: 0x000C2D60 File Offset: 0x000C1160
	public bool LoadState(RollbackStateContainer container)
	{
		foreach (ComboTracker comboTracker in this.comboTrackers)
		{
			comboTracker.LoadState(container);
		}
		return true;
	}

	// Token: 0x06002815 RID: 10261 RVA: 0x000C2DC0 File Offset: 0x000C11C0
	public bool ExportState(ref RollbackStateContainer container)
	{
		foreach (ComboTracker comboTracker in this.comboTrackers)
		{
			comboTracker.ExportState(ref container);
		}
		return true;
	}

	// Token: 0x06002816 RID: 10262 RVA: 0x000C2E20 File Offset: 0x000C1220
	public void TickFrame()
	{
		for (int i = 0; i < this.comboTrackers.Count; i++)
		{
			ComboTracker comboTracker = this.comboTrackers[i];
			comboTracker.TickFrame();
		}
	}

	// Token: 0x06002817 RID: 10263 RVA: 0x000C2E5C File Offset: 0x000C125C
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

	// Token: 0x04001D39 RID: 7481
	private List<ComboTracker> comboTrackers = new List<ComboTracker>();
}
