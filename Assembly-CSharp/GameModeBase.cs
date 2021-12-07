using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004AD RID: 1197
public class GameModeBase : IGameMode, ITickable, IRollbackStateOwner
{
	// Token: 0x1700057B RID: 1403
	// (get) Token: 0x06001A73 RID: 6771 RVA: 0x000864B4 File Offset: 0x000848B4
	// (set) Token: 0x06001A74 RID: 6772 RVA: 0x000864BC File Offset: 0x000848BC
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x1700057C RID: 1404
	// (get) Token: 0x06001A75 RID: 6773 RVA: 0x000864C5 File Offset: 0x000848C5
	// (set) Token: 0x06001A76 RID: 6774 RVA: 0x000864CD File Offset: 0x000848CD
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x1700057D RID: 1405
	// (get) Token: 0x06001A77 RID: 6775 RVA: 0x000864D6 File Offset: 0x000848D6
	// (set) Token: 0x06001A78 RID: 6776 RVA: 0x000864DE File Offset: 0x000848DE
	[Inject]
	public IRollbackStatus rollbackStatus { get; set; }

	// Token: 0x1700057E RID: 1406
	// (get) Token: 0x06001A79 RID: 6777 RVA: 0x000864E7 File Offset: 0x000848E7
	// (set) Token: 0x06001A7A RID: 6778 RVA: 0x000864EF File Offset: 0x000848EF
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x06001A7B RID: 6779 RVA: 0x000864F8 File Offset: 0x000848F8
	public virtual void Init(GameModeData modeData, ConfigData config, BattleSettings settings, IEvents events, List<PlayerReference> playerReferences, IFrameOwner frameOwner)
	{
		this.playerReferences = playerReferences;
		this.settings = settings;
		this.config = config;
		this.events = events;
		this.frameOwner = frameOwner;
		events.Subscribe(typeof(MatchResultMessage), new Events.EventHandler(this.onMatchResult));
	}

	// Token: 0x1700057F RID: 1407
	// (get) Token: 0x06001A7C RID: 6780 RVA: 0x00086548 File Offset: 0x00084948
	public virtual float CurrentSeconds
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000580 RID: 1408
	// (get) Token: 0x06001A7D RID: 6781 RVA: 0x0008654F File Offset: 0x0008494F
	public virtual bool ShouldDisplayTimer
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06001A7E RID: 6782 RVA: 0x00086552 File Offset: 0x00084952
	public virtual PlayerSpawner CreateSpawner(GameManager manager, Dictionary<PlayerNum, PlayerReference> references)
	{
		return null;
	}

	// Token: 0x17000581 RID: 1409
	// (get) Token: 0x06001A7F RID: 6783 RVA: 0x00086555 File Offset: 0x00084955
	public virtual List<EndGameCondition> EndGameConditions
	{
		get
		{
			return this.endGameConditions;
		}
	}

	// Token: 0x17000582 RID: 1410
	// (get) Token: 0x06001A80 RID: 6784 RVA: 0x00086560 File Offset: 0x00084960
	private int highestFinishedFrame
	{
		get
		{
			if (!this.rollbackStatus.RollbackEnabled)
			{
				return this.frameOwner.Frame;
			}
			int num = Math.Min(this.frameOwner.Frame, this.rollbackStatus.FullyConfirmedFrame);
			num = Math.Min(num, this.rollbackStatus.LowestInputAckFrame);
			Debug.Log(string.Concat(new object[]
			{
				"Highest finished frame ",
				num,
				" ",
				this.frameOwner.Frame,
				" ",
				this.rollbackStatus.LowestInputAckFrame,
				" ",
				this.rollbackStatus.FullyConfirmedFrame
			}));
			return num;
		}
	}

	// Token: 0x17000583 RID: 1411
	// (get) Token: 0x06001A81 RID: 6785 RVA: 0x0008662A File Offset: 0x00084A2A
	// (set) Token: 0x06001A82 RID: 6786 RVA: 0x00086632 File Offset: 0x00084A32
	public bool IsGameComplete { get; private set; }

	// Token: 0x06001A83 RID: 6787 RVA: 0x0008663C File Offset: 0x00084A3C
	public void TickUpdate()
	{
		if (this.state != null && this.state.endedGameFrame != -1 && this.cachedGameEndEvent != null)
		{
			long num = WTime.currentTimeMs - this.state.endedGameTime;
			if (num >= 3000L)
			{
				Debug.LogWarning("Game ended because of failsafe system");
				this.gameEnd();
				this.cachedGameEndEvent = null;
			}
		}
	}

	// Token: 0x06001A84 RID: 6788 RVA: 0x000866A8 File Offset: 0x00084AA8
	public virtual void TickFrame()
	{
		if (this.state.endedGameFrame == -1)
		{
			for (int i = 0; i < this.EndGameConditions.Count; i++)
			{
				EndGameCondition endGameCondition = this.EndGameConditions[i];
				endGameCondition.TickFrame();
				if (endGameCondition.IsFinished)
				{
					Debug.Log("END GAME CONDITION " + this.frameOwner.Frame);
					this.state.endedGameFrame = this.frameOwner.Frame;
					this.state.endedGameTime = WTime.currentTimeMs;
					this.cachedGameEndEvent = new GameEndEvent(endGameCondition.Victors, endGameCondition.WinningTeams);
					if (!this.rollbackStatus.RollbackEnabled)
					{
						this.gameEnd();
						this.cachedGameEndEvent = null;
					}
					break;
				}
			}
		}
		if (this.rollbackStatus.RollbackEnabled && this.state.endedGameFrame != -1 && this.cachedGameEndEvent != null && this.highestFinishedFrame >= this.state.endedGameFrame)
		{
			Debug.LogWarning(string.Concat(new object[]
			{
				"Passed game end ",
				this.highestFinishedFrame,
				" ",
				this.frameOwner.Frame
			}));
			this.gameEnd();
			this.cachedGameEndEvent = null;
		}
		if (this.modeInputProcessor != null)
		{
			HashSet<InputController> hashSet = new HashSet<InputController>();
			for (int j = 0; j < this.playerReferences.Count; j++)
			{
				PlayerReference playerReference = this.playerReferences[j];
				if (playerReference.IsInBattle && playerReference.InputController != null && !hashSet.Contains(playerReference.InputController))
				{
					hashSet.Add(playerReference.InputController);
					this.modeInputProcessor.ProcessInput(this.frameOwner.Frame, playerReference.InputController, playerReference, false);
				}
			}
			for (int k = 0; k < this.playerReferences.Count; k++)
			{
				PlayerReference playerReference2 = this.playerReferences[k];
				if (!playerReference2.IsInBattle && playerReference2.InputController != null && !hashSet.Contains(playerReference2.InputController))
				{
					hashSet.Add(playerReference2.InputController);
					this.modeInputProcessor.ProcessInput(this.frameOwner.Frame, playerReference2.InputController, playerReference2, false);
				}
			}
		}
	}

	// Token: 0x06001A85 RID: 6789 RVA: 0x00086934 File Offset: 0x00084D34
	private void gameEnd()
	{
		this.IsGameComplete = true;
		this.events.Broadcast(this.cachedGameEndEvent);
	}

	// Token: 0x06001A86 RID: 6790 RVA: 0x00086950 File Offset: 0x00084D50
	private void onMatchResult(GameEvent message)
	{
		if (this.cachedGameEndEvent == null)
		{
			MatchResultMessage matchResultMessage = message as MatchResultMessage;
			List<PlayerNum> list = new List<PlayerNum>();
			for (int i = 0; i < this.playerReferences.Count; i++)
			{
				PlayerReference playerReference = this.playerReferences[i];
				if (matchResultMessage.winningTeams.Contains(playerReference.Team))
				{
					list.Add(playerReference.PlayerNum);
				}
			}
			this.events.Broadcast(new GameEndEvent(list, matchResultMessage.winningTeams));
		}
	}

	// Token: 0x06001A87 RID: 6791 RVA: 0x000869D8 File Offset: 0x00084DD8
	public virtual void Destroy()
	{
		this.events.Unsubscribe(typeof(MatchResultMessage), new Events.EventHandler(this.onMatchResult));
		foreach (EndGameCondition endGameCondition in this.EndGameConditions)
		{
			endGameCondition.Destroy();
		}
		this.EndGameConditions.Clear();
		this.state.Clear();
		this.state = null;
		this.IsGameComplete = false;
		this.events = null;
		this.config = null;
	}

	// Token: 0x06001A88 RID: 6792 RVA: 0x00086A88 File Offset: 0x00084E88
	public virtual bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<GameModeState>(this.state));
		return true;
	}

	// Token: 0x06001A89 RID: 6793 RVA: 0x00086AA4 File Offset: 0x00084EA4
	public virtual bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<GameModeState>(ref this.state);
		return true;
	}

	// Token: 0x040013B9 RID: 5049
	private const long FAILSAFE_GAME_END = 3000L;

	// Token: 0x040013BE RID: 5054
	protected BattleSettings settings;

	// Token: 0x040013BF RID: 5055
	protected IEvents events;

	// Token: 0x040013C0 RID: 5056
	protected IInputProcessor modeInputProcessor;

	// Token: 0x040013C1 RID: 5057
	protected List<PlayerReference> playerReferences;

	// Token: 0x040013C2 RID: 5058
	protected IFrameOwner frameOwner;

	// Token: 0x040013C3 RID: 5059
	protected GameModeData modeData;

	// Token: 0x040013C4 RID: 5060
	protected ConfigData config;

	// Token: 0x040013C5 RID: 5061
	private GameEndEvent cachedGameEndEvent;

	// Token: 0x040013C6 RID: 5062
	private List<EndGameCondition> endGameConditions = new List<EndGameCondition>();

	// Token: 0x040013C7 RID: 5063
	private GameModeState state = new GameModeState();
}
