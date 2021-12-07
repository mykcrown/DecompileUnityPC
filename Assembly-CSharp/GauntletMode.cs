using System;
using System.Collections.Generic;

// Token: 0x020004AA RID: 1194
public class GauntletMode : GameModeBase, IGameMode, ITickable, IRollbackStateOwner
{
	// Token: 0x06001A65 RID: 6757 RVA: 0x000892A0 File Offset: 0x000876A0
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

	// Token: 0x06001A66 RID: 6758 RVA: 0x00089325 File Offset: 0x00087725
	public override bool ExportState(ref RollbackStateContainer container)
	{
		base.ExportState(ref container);
		this.lives.ExportState(ref container);
		return true;
	}

	// Token: 0x06001A67 RID: 6759 RVA: 0x0008933D File Offset: 0x0008773D
	public override bool LoadState(RollbackStateContainer container)
	{
		base.LoadState(container);
		this.lives.LoadState(container);
		return true;
	}

	// Token: 0x040013B6 RID: 5046
	private GauntletEndGameCondition lives;
}
