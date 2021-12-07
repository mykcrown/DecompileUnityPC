// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class CrewBattleGameMode : GameModeBase, IGameMode, ITickable, IRollbackStateOwner
{
	private TimeEndGameCondition timer;

	private LivesEndGameCondition lives;

	public override float CurrentSeconds
	{
		get
		{
			return this.timer.CurrentSeconds;
		}
	}

	public override bool ShouldDisplayTimer
	{
		get
		{
			return this.timer.ShouldDisplayTimer;
		}
	}

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

	public override PlayerSpawner CreateSpawner(GameManager manager, Dictionary<PlayerNum, PlayerReference> references)
	{
		CrewBattlePlayerSpawner instance = base.injector.GetInstance<CrewBattlePlayerSpawner>();
		instance.Init(manager.events, manager, references, this.playerReferences);
		return instance;
	}

	public void onCharacterInit(GameEvent message)
	{
		CharacterInitEvent characterInitEvent = message as CharacterInitEvent;
		CharacterComponent characterComponent = ScriptableObject.CreateInstance<CrewBattlePlayerComponent>();
		characterComponent.Init(characterInitEvent.character);
		characterInitEvent.character.AddCharacterComponent(characterComponent);
	}

	public override bool ExportState(ref RollbackStateContainer container)
	{
		base.ExportState(ref container);
		this.timer.ExportState(ref container);
		this.lives.ExportState(ref container);
		return true;
	}

	public override bool LoadState(RollbackStateContainer container)
	{
		base.LoadState(container);
		this.timer.LoadState(container);
		this.lives.LoadState(container);
		return true;
	}

	public override void Destroy()
	{
		base.Destroy();
		if (this.events != null)
		{
			this.events.Unsubscribe(typeof(CharacterInitEvent), new Events.EventHandler(this.onCharacterInit));
		}
	}
}
