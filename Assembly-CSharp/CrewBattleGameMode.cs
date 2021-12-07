using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004A0 RID: 1184
public class CrewBattleGameMode : GameModeBase, IGameMode, ITickable, IRollbackStateOwner
{
	// Token: 0x06001A05 RID: 6661 RVA: 0x00086ABC File Offset: 0x00084EBC
	public override void Init(GameModeData modeData, ConfigData config, BattleSettings settings, IEvents events, List<PlayerReference> playerReferences, IFrameOwner frameOwner)
	{
		base.Init(modeData, config, settings, events, playerReferences, frameOwner);
		this.lives = new LivesEndGameCondition();
		base.injector.Inject(this.lives);
		this.lives.Init(settings);
		this.EndGameConditions.Add(this.lives);
		this.timer = new TimeEndGameCondition();
		base.injector.Inject(this.timer);
		this.timer.Init(settings);
		this.EndGameConditions.Add(this.timer);
		this.modeInputProcessor = new CrewBattleInputProcessor();
		(this.modeInputProcessor as GameModeInputProcessor).Start(modeData, config, events);
		base.injector.Inject(this.modeInputProcessor);
		events.Subscribe(typeof(CharacterInitEvent), new Events.EventHandler(this.onCharacterInit));
	}

	// Token: 0x06001A06 RID: 6662 RVA: 0x00086B98 File Offset: 0x00084F98
	public override PlayerSpawner CreateSpawner(GameManager manager, Dictionary<PlayerNum, PlayerReference> references)
	{
		CrewBattlePlayerSpawner instance = base.injector.GetInstance<CrewBattlePlayerSpawner>();
		instance.Init(manager.events, manager, references, this.playerReferences);
		return instance;
	}

	// Token: 0x06001A07 RID: 6663 RVA: 0x00086BC8 File Offset: 0x00084FC8
	public void onCharacterInit(GameEvent message)
	{
		CharacterInitEvent characterInitEvent = message as CharacterInitEvent;
		CharacterComponent characterComponent = ScriptableObject.CreateInstance<CrewBattlePlayerComponent>();
		characterComponent.Init(characterInitEvent.character);
		characterInitEvent.character.AddCharacterComponent(characterComponent);
	}

	// Token: 0x17000565 RID: 1381
	// (get) Token: 0x06001A08 RID: 6664 RVA: 0x00086BFA File Offset: 0x00084FFA
	public override float CurrentSeconds
	{
		get
		{
			return this.timer.CurrentSeconds;
		}
	}

	// Token: 0x17000566 RID: 1382
	// (get) Token: 0x06001A09 RID: 6665 RVA: 0x00086C07 File Offset: 0x00085007
	public override bool ShouldDisplayTimer
	{
		get
		{
			return this.timer.ShouldDisplayTimer;
		}
	}

	// Token: 0x06001A0A RID: 6666 RVA: 0x00086C14 File Offset: 0x00085014
	public override bool ExportState(ref RollbackStateContainer container)
	{
		base.ExportState(ref container);
		this.timer.ExportState(ref container);
		this.lives.ExportState(ref container);
		return true;
	}

	// Token: 0x06001A0B RID: 6667 RVA: 0x00086C39 File Offset: 0x00085039
	public override bool LoadState(RollbackStateContainer container)
	{
		base.LoadState(container);
		this.timer.LoadState(container);
		this.lives.LoadState(container);
		return true;
	}

	// Token: 0x06001A0C RID: 6668 RVA: 0x00086C5E File Offset: 0x0008505E
	public override void Destroy()
	{
		base.Destroy();
		if (this.events != null)
		{
			this.events.Unsubscribe(typeof(CharacterInitEvent), new Events.EventHandler(this.onCharacterInit));
		}
	}

	// Token: 0x04001377 RID: 4983
	private TimeEndGameCondition timer;

	// Token: 0x04001378 RID: 4984
	private LivesEndGameCondition lives;
}
