// Decompile from assembly: Assembly-CSharp.dll

using AI;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReference : IRollbackStateOwner, ITickable
{
	public static string TOGGLE_AI = "PlayerReference.TOGGLE_AI";

	private List<PlayerController> controllers = new List<PlayerController>();

	private PlayerReferenceState state;

	public SpawnPointReference spawnReference;

	public BehaviorTree ActiveBehaviorTree;

	public BehaviorTree baseTree;

	public BehaviorTree passiveTree;

	[Inject]
	public RespawnController respawnController
	{
		get;
		set;
	}

	[Inject]
	public IAIManager aiManager
	{
		private get;
		set;
	}

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
	{
		private get;
		set;
	}

	[Inject]
	public GameController gameController
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

	[Inject]
	public IDependencyInjection injector
	{
		get;
		set;
	}

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

	[Inject]
	public ICharacterDataLoader characterDataLoader
	{
		private get;
		set;
	}

	public PlayerSelectionInfo PlayerInfo
	{
		get;
		private set;
	}

	public InputController InputController
	{
		get;
		private set;
	}

	public bool IsEliminated
	{
		get
		{
			return this.state.lives == 0 && (this.gameController.currentGame == null || this.gameController.currentGame.ModeData.settings.usesLives);
		}
	}

	public int Lives
	{
		get
		{
			return this.state.lives;
		}
		set
		{
			this.state.lives = value;
		}
	}

	public PlayerNum PlayerNum
	{
		get
		{
			return this.PlayerInfo.playerNum;
		}
	}

	public TeamNum Team
	{
		get
		{
			return this.PlayerInfo.team;
		}
	}

	public PlayerType Type
	{
		get
		{
			return this.PlayerInfo.type;
		}
	}

	public PlayerController Controller
	{
		get
		{
			if (this.controllers.Count == 0)
			{
				return null;
			}
			if (this.controllers.Count == 1)
			{
				return this.controllers[0];
			}
			foreach (PlayerController current in this.controllers)
			{
				if (current.IsActive)
				{
					return current;
				}
			}
			return null;
		}
	}

	public IEnumerable<PlayerController> AllControllers
	{
		get
		{
			return this.controllers;
		}
	}

	public bool IsInBattle
	{
		get
		{
			return !this.PlayerInfo.isSpectator && (this.EngagementState == PlayerEngagementState.Primary || this.EngagementState == PlayerEngagementState.Temporary || this.EngagementState == PlayerEngagementState.AssistMove);
		}
	}

	public bool IsPrimary
	{
		get
		{
			return this.EngagementState == PlayerEngagementState.Primary;
		}
	}

	public bool IsBenched
	{
		get
		{
			return this.EngagementState == PlayerEngagementState.Benched;
		}
	}

	public bool IsSpectating
	{
		get
		{
			return this.PlayerInfo.isSpectator;
		}
	}

	public bool IsTemporary
	{
		get
		{
			return this.EngagementState == PlayerEngagementState.Temporary;
		}
	}

	public bool IsAllyAssistMove
	{
		get
		{
			return this.EngagementState == PlayerEngagementState.AssistMove;
		}
	}

	public bool IsDisconnected
	{
		get
		{
			return this.EngagementState == PlayerEngagementState.Disconnected;
		}
	}

	public bool CanHostTeamMove
	{
		get
		{
			return this.Controller.IsActive && this.IsPrimary;
		}
	}

	public bool IsPassiveAI
	{
		get
		{
			return this.ActiveBehaviorTree != null && this.ActiveBehaviorTree == this.passiveTree;
		}
	}

	public PlayerEngagementState EngagementState
	{
		get
		{
			return this.state.engagementState;
		}
		set
		{
			if (value == this.state.engagementState)
			{
				return;
			}
			if (this.state.engagementState == PlayerEngagementState.Disconnected)
			{
				throw new Exception("Trying to change engagement state of disconnected player");
			}
			this.state.engagementState = value;
			this.events.Broadcast(new PlayerEngagementStateChangedEvent(this.PlayerNum, this.Team, value));
		}
	}

	public void UnsafeSetInputController(InputController newController)
	{
		this.InputController = newController;
	}

	private void assignCpuPlayerInputController()
	{
		GameManager currentGame = this.gameController.currentGame;
		bool flag = false;
		if (currentGame.BattleSettings.mode == GameMode.CrewBattle)
		{
			foreach (PlayerReference current in currentGame.PlayerReferences)
			{
				if (current.Type == PlayerType.Human && current.Team == this.Team)
				{
					this.InputController = null;
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			this.InputController = currentGame.GameContainer.AddComponent<AIInput>();
			AIInput aIInput = this.InputController as AIInput;
			this.injector.Inject(aIInput);
			aIInput.PlayerReference = this;
			aIInput.Initialize(this.config.inputConfig);
			this.setupBehaviorTrees();
		}
	}

	public void Init(PlayerSelectionInfo playerInfo)
	{
		this.PlayerInfo = playerInfo;
		this.state = new PlayerReferenceState();
		GameManager currentGame = this.gameController.currentGame;
		if (currentGame.BattleSettings.mode == GameMode.CrewBattle)
		{
			this.state.lives = currentGame.BattleSettings.crewBattle_lives;
		}
		else if (currentGame.BattleSettings.rules == GameRules.Stock)
		{
			this.state.lives = currentGame.BattleSettings.lives;
		}
		else
		{
			this.state.lives = 2147483647;
		}
		switch (playerInfo.type)
		{
		case PlayerType.Human:
			this.InputController = currentGame.Client.Input.GetPort(this.PlayerNum);
			goto IL_110;
		case PlayerType.CPU:
			this.assignCpuPlayerInputController();
			goto IL_110;
		}
		UnityEngine.Debug.LogWarning(string.Concat(new object[]
		{
			"Failed to find input controller for player ",
			playerInfo.playerNum,
			" with type ",
			playerInfo.type
		}));
		IL_110:
		if (this.PlayerInfo.isSpectator)
		{
			this.EngagementState = PlayerEngagementState.Spectating;
			playerInfo.type = PlayerType.Spectator;
		}
		this.events.Subscribe(typeof(TagInPlayerEvent), new Events.EventHandler(this.onTagPlayerIn));
		this.signalBus.AddListener(PlayerReference.TOGGLE_AI, new Action(this.onToggleAI));
	}

	private void setupBehaviorTrees()
	{
		CharacterData data = this.characterDataLoader.GetData(this.PlayerInfo.characterID);
		CompositeNodeData rootNode;
		if (data.overrideAIBehavior.obj != null)
		{
			rootNode = data.overrideAIBehavior.obj;
		}
		else
		{
			rootNode = this.aiManager.GetDefaultRootNode();
		}
		this.baseTree = this.injector.GetInstance<BehaviorTree>();
		this.baseTree.Init(this, rootNode);
		CompositeNodeData passiveRootNode = this.aiManager.GetPassiveRootNode();
		this.passiveTree = this.injector.GetInstance<BehaviorTree>();
		this.passiveTree.Init(this, passiveRootNode);
		this.ActiveBehaviorTree = this.baseTree;
	}

	public void TickFrame()
	{
		if (this.ActiveBehaviorTree != null)
		{
			this.ActiveBehaviorTree.TickFrame();
		}
		if (this.InputController != null)
		{
			this.InputController.TickFrame();
		}
		this.respawnController.TickFrame();
	}

	public void AddController(PlayerController controller)
	{
		this.controllers.Add(controller);
	}

	private void onTagPlayerIn(GameEvent message)
	{
		TagInPlayerEvent tagInPlayerEvent = message as TagInPlayerEvent;
		if (tagInPlayerEvent.taggedPlayerNum == this.PlayerNum)
		{
			this.EngagementState = PlayerEngagementState.Primary;
		}
	}

	private void onToggleAI()
	{
		if (this.ActiveBehaviorTree == this.baseTree)
		{
			this.ActiveBehaviorTree = this.passiveTree;
		}
		else
		{
			this.ActiveBehaviorTree = this.baseTree;
		}
	}

	public void Destroy()
	{
		this.events.Unsubscribe(typeof(TagInPlayerEvent), new Events.EventHandler(this.onTagPlayerIn));
		this.signalBus.RemoveListener(PlayerReference.TOGGLE_AI, new Action(this.onToggleAI));
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<PlayerReferenceState>(ref this.state);
		return true;
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.rollbackStatePooling.Clone<PlayerReferenceState>(this.state));
	}
}
