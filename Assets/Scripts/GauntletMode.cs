// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class GauntletMode : GameModeBase, IGameMode, ITickable, IRollbackStateOwner
{
	private GauntletEndGameCondition lives;

	public override void Init(GameModeData modeData, ConfigData config, BattleSettings settings, IEvents events, List<PlayerReference> playerReferences, IFrameOwner frameOwner)
	{
		base.Init(modeData, config, settings, events, playerReferences, frameOwner);
		this.lives = new GauntletEndGameCondition();
		base.injector.Inject(this.lives);
		this.lives.Init(settings);
		this.EndGameConditions.Add(this.lives);
		this.modeInputProcessor = new GauntletInputProcessor();
		(this.modeInputProcessor as GameModeInputProcessor).Start(modeData, config, events);
		base.injector.Inject(this.modeInputProcessor);
	}

	public override bool ExportState(ref RollbackStateContainer container)
	{
		base.ExportState(ref container);
		this.lives.ExportState(ref container);
		return true;
	}

	public override bool LoadState(RollbackStateContainer container)
	{
		base.LoadState(container);
		this.lives.LoadState(container);
		return true;
	}
}
