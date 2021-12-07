// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class GameModeBase : IGameMode, ITickable, IRollbackStateOwner
{
	private const long FAILSAFE_GAME_END = 3000L;

	protected BattleSettings settings;

	protected IEvents events;

	protected IInputProcessor modeInputProcessor;

	protected List<PlayerReference> playerReferences;

	protected IFrameOwner frameOwner;

	protected GameModeData modeData;

	protected ConfigData config;

	private GameEndEvent cachedGameEndEvent;

	private List<EndGameCondition> endGameConditions = new List<EndGameCondition>();

	private GameModeState state = new GameModeState();

	[Inject]
	public IDependencyInjection injector
	{
		get;
		set;
	}

	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	[Inject]
	public IRollbackStatus rollbackStatus
	{
		get;
		set;
	}

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	public virtual float CurrentSeconds
	{
		get
		{
			return 0f;
		}
	}

	public virtual bool ShouldDisplayTimer
	{
		get
		{
			return false;
		}
	}

	public virtual List<EndGameCondition> EndGameConditions
	{
		get
		{
			return this.endGameConditions;
		}
	}

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
			UnityEngine.Debug.Log(string.Concat(new object[]
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

	public bool IsGameComplete
	{
		get;
		private set;
	}

	public virtual void Init(GameModeData modeData, ConfigData config, BattleSettings settings, IEvents events, List<PlayerReference> playerReferences, IFrameOwner frameOwner)
	{
		this.playerReferences = playerReferences;
		this.settings = settings;
		this.config = config;
		this.events = events;
		this.frameOwner = frameOwner;
		events.Subscribe(typeof(MatchResultMessage), new Events.EventHandler(this.onMatchResult));
	}

	public virtual PlayerSpawner CreateSpawner(GameManager manager, Dictionary<PlayerNum, PlayerReference> references)
	{
		return null;
	}

	public void TickUpdate()
	{
		if (this.state != null && this.state.endedGameFrame != -1 && this.cachedGameEndEvent != null)
		{
			long num = WTime.currentTimeMs - this.state.endedGameTime;
			if (num >= 3000L)
			{
				UnityEngine.Debug.LogWarning("Game ended because of failsafe system");
				this.gameEnd();
				this.cachedGameEndEvent = null;
			}
		}
	}

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
					UnityEngine.Debug.Log("END GAME CONDITION " + this.frameOwner.Frame);
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
			UnityEngine.Debug.LogWarning(string.Concat(new object[]
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

	private void gameEnd()
	{
		this.IsGameComplete = true;
		this.events.Broadcast(this.cachedGameEndEvent);
	}

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

	public virtual void Destroy()
	{
		this.events.Unsubscribe(typeof(MatchResultMessage), new Events.EventHandler(this.onMatchResult));
		foreach (EndGameCondition current in this.EndGameConditions)
		{
			current.Destroy();
		}
		this.EndGameConditions.Clear();
		this.state.Clear();
		this.state = null;
		this.IsGameComplete = false;
		this.events = null;
		this.config = null;
	}

	public virtual bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<GameModeState>(this.state));
		return true;
	}

	public virtual bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<GameModeState>(ref this.state);
		return true;
	}
}
