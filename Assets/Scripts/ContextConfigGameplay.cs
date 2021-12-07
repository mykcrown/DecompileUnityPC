// Decompile from assembly: Assembly-CSharp.dll

using AI;
using System;

public class ContextConfigGameplay : ContextConfig
{
	public ContextConfigGameplay(MasterContext.InitContext initContext) : base(initContext)
	{
		this.injectionBinder.Bind<GameController>().ToSingleton();
		this.gameModeBinding<CrewBattleGameMode>(GameMode.CrewBattle);
		this.gameModeBinding<StandardGameMode>(GameMode.FreeForAll);
		this.gameModeBinding<StandardGameMode>(GameMode.Teams);
		this.gameModeBinding<TestingGameMode>(GameMode.Testing);
		this.gameModeBinding<GameModeBase>(GameMode.Preview);
		this.gameModeBinding<TrainingGameMode>(GameMode.Training);
		this.gameModeBinding<GauntletMode>(GameMode.Gauntlet);
		this.injectionBinder.Bind<IGameItemsPreloader>().To<GameItemsPreloader>();
		this.injectionBinder.Bind<StageTriggerController>().To<StageTriggerController>();
		this.injectionBinder.Bind<PlayerSpawner>().To<PlayerSpawner>();
		this.injectionBinder.Bind<CrewBattlePlayerSpawner>().To<CrewBattlePlayerSpawner>();
		this.injectionBinder.Bind<HitCollisionManager>().To<HitCollisionManager>();
		this.injectionBinder.Bind<RollbackStatePoolContainer>().To<RollbackStatePoolContainer>().ToSingleton();
		this.injectionBinder.Bind<MaterialAnimationController>().To<MaterialAnimationController>();
		this.injectionBinder.Bind<MaterialAnimationsController>().To<MaterialAnimationsController>();
		this.injectionBinder.Bind<AnnouncementHelper>().To<AnnouncementHelper>().ToSingleton();
		this.injectionBinder.Bind<ISpawnController>().To<SpawnController>().ToSingleton();
		this.injectionBinder.Bind<ICrewAssistSpawnHelper>().To<CrewAssistSpawnHelper>();
		this.injectionBinder.Bind<MoveController>().To<MoveController>();
		this.injectionBinder.Bind<ProjectileController>().To<ProjectileController>();
		this.injectionBinder.Bind<ArticleController>().To<ArticleController>();
		this.injectionBinder.Bind<ProjectilePhysicsImpactHandler>().To<ProjectilePhysicsImpactHandler>();
		this.injectionBinder.Bind<ProjectilePhysicsCollisionMotion>().To<ProjectilePhysicsCollisionMotion>();
		this.injectionBinder.Bind<MoveArticleSpawnCalculator>().To<MoveArticleSpawnCalculator>().ToSingleton();
		this.injectionBinder.Bind<DeveloperUtilityConsoleCommands>().To<DeveloperUtilityConsoleCommands>().ToSingleton();
		this.injectionBinder.Bind<IPlayerTauntsFinder>().To<PlayerTauntsFinder>().ToSingleton();
		this.injectionBinder.Bind<IMoveAnimationCalculator>().To<MoveAnimationCalculator>().ToSingleton();
		this.injectionBinder.Bind<IGameTauntsSetup>().To<GameTauntsSetup>().ToSingleton();
		this.injectionBinder.Bind<IVictoryPoseController>().To<VictoryPoseController>().ToSingleton();
		this.injectionBinder.Bind<ICombatCalculator>().To<CombatCalculator>().ToSingleton();
		this.injectionBinder.Bind<IHitContextPool>().To<HitContextPool>().ToSingleton();
		this.injectionBinder.Bind<IAIManager>().To<AIManager>().ToSingleton();
		this.injectionBinder.Bind<IAICalculator>().To<AICalculator>().ToSingleton();
		this.injectionBinder.Bind<IInputConverter>().To<InputConverter>().ToSingleton();
		this.injectionBinder.Bind<ITranslateTreeData>().To<TranslateTreeData>().ToSingleton();
		this.injectionBinder.Bind<BehaviorTree>().To<BehaviorTree>();
		this.injectionBinder.Bind<ComboManager>().To<ComboManager>();
		this.injectionBinder.Bind<ComboState>().To<ComboState>();
		this.injectionBinder.Bind<ComboTracker>().To<ComboTracker>();
		this.injectionBinder.Bind<PlayerCombatController>().To<PlayerCombatController>();
		this.injectionBinder.Bind<PlayerStateActor>().To<PlayerStateActor>();
		this.injectionBinder.Bind<RespawnController>().To<RespawnController>();
		this.injectionBinder.Bind<PlayerReference>().To<PlayerReference>();
	}

	private void gameModeBinding<T>(object name) where T : IGameMode
	{
		this.injectionBinder.Bind<IGameMode>().To<T>().ToName(name);
	}
}
