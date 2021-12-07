// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class ComboTracker : ITickable, IRollbackStateOwner
{
	public int WindowFrames = 5;

	private TeamNum team;

	private List<PlayerReference> victims;

	private Dictionary<PlayerNum, ComboState> combosByVictimNum;

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
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

	public PlayerNum Player
	{
		get;
		private set;
	}

	public void Setup(PlayerNum player, TeamNum team, List<PlayerReference> players)
	{
		this.Player = player;
		this.team = team;
		this.victims = new List<PlayerReference>(players);
		for (int i = this.victims.Count - 1; i >= 0; i--)
		{
			if (i == PlayerUtil.GetIntFromPlayerNum(player, true) || this.victims[i] == null || this.victims[i].IsSpectating)
			{
				this.victims.RemoveAt(i);
				break;
			}
		}
		this.combosByVictimNum = new Dictionary<PlayerNum, ComboState>(default(PlayerNumComparer));
		foreach (PlayerReference current in this.victims)
		{
			this.combosByVictimNum[current.PlayerNum] = this.injector.GetInstance<ComboState>();
			this.combosByVictimNum[current.PlayerNum].Setup(this.WindowFrames);
		}
		this.signalBus.GetSignal<PlayerHitConfirmSignal>().AddListener(new Action<HitData, IPlayerDelegate, IHitOwner>(this.onPlayerHitConfirm));
	}

	public void Destroy()
	{
		this.signalBus.GetSignal<PlayerHitConfirmSignal>().RemoveListener(new Action<HitData, IPlayerDelegate, IHitOwner>(this.onPlayerHitConfirm));
	}

	public void TickFrame()
	{
		for (int i = 0; i < this.victims.Count; i++)
		{
			PlayerReference playerReference = this.victims[i];
			if (playerReference != null && !playerReference.IsSpectating)
			{
				ComboState comboState = this.combosByVictimNum[playerReference.PlayerNum];
				comboState.UpdateFrame(null, null, playerReference.Controller.State.IsRecovered, !playerReference.Controller.State.IsInControl);
			}
		}
	}

	public ComboState GetComboState()
	{
		if (this.victims.Count > 0)
		{
			return this.combosByVictimNum[this.victims[0].PlayerNum];
		}
		return null;
	}

	public ComboState GetComboState(PlayerNum player)
	{
		if (this.combosByVictimNum.ContainsKey(player))
		{
			return this.combosByVictimNum[player];
		}
		return null;
	}

	public bool OnCharacterDeath(PlayerNum player, TeamNum team)
	{
		if (this.combosByVictimNum.ContainsKey(player) && team != this.team)
		{
			ComboState comboState = this.combosByVictimNum[player];
			if (comboState.IsActive)
			{
				this.events.Broadcast(new LogStatEvent(StatType.Kill, 1, PointsValueType.Addition, this.Player, this.team));
				this.events.Broadcast(new ComboKillEvent(player, team, this.Player, this.team));
				comboState.Clear();
				return true;
			}
		}
		return false;
	}

	private void onPlayerHitConfirm(HitData hit, IPlayerDelegate victim, IHitOwner attacker)
	{
		if (this.combosByVictimNum.ContainsKey(victim.PlayerNum))
		{
			if (attacker.PlayerNum == this.Player)
			{
				this.combosByVictimNum[victim.PlayerNum].ForceRecordHit(hit, attacker);
			}
			else if (attacker.PlayerNum != PlayerNum.None)
			{
				this.combosByVictimNum[victim.PlayerNum].Clear();
			}
		}
	}

	public bool LoadState(RollbackStateContainer container)
	{
		foreach (KeyValuePair<PlayerNum, ComboState> current in this.combosByVictimNum)
		{
			current.Value.LoadState(container);
		}
		return true;
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		foreach (KeyValuePair<PlayerNum, ComboState> current in this.combosByVictimNum)
		{
			current.Value.ExportState(ref container);
		}
		return true;
	}
}
