using System;
using System.Collections.Generic;

// Token: 0x02000666 RID: 1638
public class ComboTracker : ITickable, IRollbackStateOwner
{
	// Token: 0x170009D4 RID: 2516
	// (get) Token: 0x06002819 RID: 10265 RVA: 0x000C2EF8 File Offset: 0x000C12F8
	// (set) Token: 0x0600281A RID: 10266 RVA: 0x000C2F00 File Offset: 0x000C1300
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x170009D5 RID: 2517
	// (get) Token: 0x0600281B RID: 10267 RVA: 0x000C2F09 File Offset: 0x000C1309
	// (set) Token: 0x0600281C RID: 10268 RVA: 0x000C2F11 File Offset: 0x000C1311
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x170009D6 RID: 2518
	// (get) Token: 0x0600281D RID: 10269 RVA: 0x000C2F1A File Offset: 0x000C131A
	// (set) Token: 0x0600281E RID: 10270 RVA: 0x000C2F22 File Offset: 0x000C1322
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x170009D7 RID: 2519
	// (get) Token: 0x0600281F RID: 10271 RVA: 0x000C2F2B File Offset: 0x000C132B
	// (set) Token: 0x06002820 RID: 10272 RVA: 0x000C2F33 File Offset: 0x000C1333
	public PlayerNum Player { get; private set; }

	// Token: 0x06002821 RID: 10273 RVA: 0x000C2F3C File Offset: 0x000C133C
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
		foreach (PlayerReference playerReference in this.victims)
		{
			this.combosByVictimNum[playerReference.PlayerNum] = this.injector.GetInstance<ComboState>();
			this.combosByVictimNum[playerReference.PlayerNum].Setup(this.WindowFrames);
		}
		this.signalBus.GetSignal<PlayerHitConfirmSignal>().AddListener(new Action<HitData, IPlayerDelegate, IHitOwner>(this.onPlayerHitConfirm));
	}

	// Token: 0x06002822 RID: 10274 RVA: 0x000C307C File Offset: 0x000C147C
	public void Destroy()
	{
		this.signalBus.GetSignal<PlayerHitConfirmSignal>().RemoveListener(new Action<HitData, IPlayerDelegate, IHitOwner>(this.onPlayerHitConfirm));
	}

	// Token: 0x06002823 RID: 10275 RVA: 0x000C309C File Offset: 0x000C149C
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

	// Token: 0x06002824 RID: 10276 RVA: 0x000C3125 File Offset: 0x000C1525
	public ComboState GetComboState()
	{
		if (this.victims.Count > 0)
		{
			return this.combosByVictimNum[this.victims[0].PlayerNum];
		}
		return null;
	}

	// Token: 0x06002825 RID: 10277 RVA: 0x000C3156 File Offset: 0x000C1556
	public ComboState GetComboState(PlayerNum player)
	{
		if (this.combosByVictimNum.ContainsKey(player))
		{
			return this.combosByVictimNum[player];
		}
		return null;
	}

	// Token: 0x06002826 RID: 10278 RVA: 0x000C3178 File Offset: 0x000C1578
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

	// Token: 0x06002827 RID: 10279 RVA: 0x000C3200 File Offset: 0x000C1600
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

	// Token: 0x06002828 RID: 10280 RVA: 0x000C3274 File Offset: 0x000C1674
	public bool LoadState(RollbackStateContainer container)
	{
		foreach (KeyValuePair<PlayerNum, ComboState> keyValuePair in this.combosByVictimNum)
		{
			keyValuePair.Value.LoadState(container);
		}
		return true;
	}

	// Token: 0x06002829 RID: 10281 RVA: 0x000C32D8 File Offset: 0x000C16D8
	public bool ExportState(ref RollbackStateContainer container)
	{
		foreach (KeyValuePair<PlayerNum, ComboState> keyValuePair in this.combosByVictimNum)
		{
			keyValuePair.Value.ExportState(ref container);
		}
		return true;
	}

	// Token: 0x04001D3D RID: 7485
	public int WindowFrames = 5;

	// Token: 0x04001D3F RID: 7487
	private TeamNum team;

	// Token: 0x04001D40 RID: 7488
	private List<PlayerReference> victims;

	// Token: 0x04001D41 RID: 7489
	private Dictionary<PlayerNum, ComboState> combosByVictimNum;
}
