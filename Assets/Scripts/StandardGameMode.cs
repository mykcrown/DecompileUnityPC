// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class StandardGameMode : GameModeBase, IGameMode, ITickable, IRollbackStateOwner
{
	private TimeEndGameCondition timeCondition;

	private LivesEndGameCondition livesCondition;

	public override float CurrentSeconds
	{
		get
		{
			return this.timeCondition.CurrentSeconds;
		}
	}

	public override bool ShouldDisplayTimer
	{
		get
		{
			return this.timeCondition.ShouldDisplayTimer;
		}
	}

	public override void Init(GameModeData modeData, ConfigData config, BattleSettings settings, IEvents events, List<PlayerReference> playerReferences, IFrameOwner frameOwner)
	{
		base.Init(modeData, config, settings, events, playerReferences, frameOwner);
		this.timeCondition = new TimeEndGameCondition();
		base.injector.Inject(this.timeCondition);
		this.timeCondition.Init(settings);
		this.livesCondition = new LivesEndGameCondition();
		base.injector.Inject(this.livesCondition);
		this.livesCondition.Init(settings);
		this.EndGameConditions.Add(this.livesCondition);
		this.EndGameConditions.Add(this.timeCondition);
		this.modeInputProcessor = new LivesModeInputProcessor();
		(this.modeInputProcessor as GameModeInputProcessor).Start(modeData, config, events);
		base.injector.Inject(this.modeInputProcessor);
	}

	public override bool ExportState(ref RollbackStateContainer container)
	{
		base.ExportState(ref container);
		this.timeCondition.ExportState(ref container);
		this.livesCondition.ExportState(ref container);
		return true;
	}

	public override bool LoadState(RollbackStateContainer container)
	{
		base.LoadState(container);
		this.timeCondition.LoadState(container);
		this.livesCondition.LoadState(container);
		return true;
	}
}
