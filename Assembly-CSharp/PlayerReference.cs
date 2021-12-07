using System;
using System.Collections.Generic;
using AI;
using UnityEngine;

// Token: 0x020005F4 RID: 1524
public class PlayerReference : IRollbackStateOwner, ITickable
{
	// Token: 0x17000840 RID: 2112
	// (get) Token: 0x060023F4 RID: 9204 RVA: 0x000B58BD File Offset: 0x000B3CBD
	// (set) Token: 0x060023F5 RID: 9205 RVA: 0x000B58C5 File Offset: 0x000B3CC5
	[Inject]
	public RespawnController respawnController { get; set; }

	// Token: 0x17000841 RID: 2113
	// (get) Token: 0x060023F6 RID: 9206 RVA: 0x000B58CE File Offset: 0x000B3CCE
	// (set) Token: 0x060023F7 RID: 9207 RVA: 0x000B58D6 File Offset: 0x000B3CD6
	[Inject]
	public IAIManager aiManager { private get; set; }

	// Token: 0x17000842 RID: 2114
	// (get) Token: 0x060023F8 RID: 9208 RVA: 0x000B58DF File Offset: 0x000B3CDF
	// (set) Token: 0x060023F9 RID: 9209 RVA: 0x000B58E7 File Offset: 0x000B3CE7
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17000843 RID: 2115
	// (get) Token: 0x060023FA RID: 9210 RVA: 0x000B58F0 File Offset: 0x000B3CF0
	// (set) Token: 0x060023FB RID: 9211 RVA: 0x000B58F8 File Offset: 0x000B3CF8
	[Inject]
	public ISignalBus signalBus { private get; set; }

	// Token: 0x17000844 RID: 2116
	// (get) Token: 0x060023FC RID: 9212 RVA: 0x000B5901 File Offset: 0x000B3D01
	// (set) Token: 0x060023FD RID: 9213 RVA: 0x000B5909 File Offset: 0x000B3D09
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x17000845 RID: 2117
	// (get) Token: 0x060023FE RID: 9214 RVA: 0x000B5912 File Offset: 0x000B3D12
	// (set) Token: 0x060023FF RID: 9215 RVA: 0x000B591A File Offset: 0x000B3D1A
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x17000846 RID: 2118
	// (get) Token: 0x06002400 RID: 9216 RVA: 0x000B5923 File Offset: 0x000B3D23
	// (set) Token: 0x06002401 RID: 9217 RVA: 0x000B592B File Offset: 0x000B3D2B
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x17000847 RID: 2119
	// (get) Token: 0x06002402 RID: 9218 RVA: 0x000B5934 File Offset: 0x000B3D34
	// (set) Token: 0x06002403 RID: 9219 RVA: 0x000B593C File Offset: 0x000B3D3C
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x17000848 RID: 2120
	// (get) Token: 0x06002404 RID: 9220 RVA: 0x000B5945 File Offset: 0x000B3D45
	// (set) Token: 0x06002405 RID: 9221 RVA: 0x000B594D File Offset: 0x000B3D4D
	[Inject]
	public ICharacterDataLoader characterDataLoader { private get; set; }

	// Token: 0x17000849 RID: 2121
	// (get) Token: 0x06002406 RID: 9222 RVA: 0x000B5956 File Offset: 0x000B3D56
	// (set) Token: 0x06002407 RID: 9223 RVA: 0x000B595E File Offset: 0x000B3D5E
	public PlayerSelectionInfo PlayerInfo { get; private set; }

	// Token: 0x1700084A RID: 2122
	// (get) Token: 0x06002408 RID: 9224 RVA: 0x000B5967 File Offset: 0x000B3D67
	// (set) Token: 0x06002409 RID: 9225 RVA: 0x000B596F File Offset: 0x000B3D6F
	public InputController InputController { get; private set; }

	// Token: 0x0600240A RID: 9226 RVA: 0x000B5978 File Offset: 0x000B3D78
	public void UnsafeSetInputController(InputController newController)
	{
		this.InputController = newController;
	}

	// Token: 0x1700084B RID: 2123
	// (get) Token: 0x0600240B RID: 9227 RVA: 0x000B5984 File Offset: 0x000B3D84
	public bool IsEliminated
	{
		get
		{
			return this.state.lives == 0 && (this.gameController.currentGame == null || this.gameController.currentGame.ModeData.settings.usesLives);
		}
	}

	// Token: 0x1700084C RID: 2124
	// (get) Token: 0x0600240C RID: 9228 RVA: 0x000B59D7 File Offset: 0x000B3DD7
	// (set) Token: 0x0600240D RID: 9229 RVA: 0x000B59E4 File Offset: 0x000B3DE4
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

	// Token: 0x1700084D RID: 2125
	// (get) Token: 0x0600240E RID: 9230 RVA: 0x000B59F2 File Offset: 0x000B3DF2
	public PlayerNum PlayerNum
	{
		get
		{
			return this.PlayerInfo.playerNum;
		}
	}

	// Token: 0x1700084E RID: 2126
	// (get) Token: 0x0600240F RID: 9231 RVA: 0x000B59FF File Offset: 0x000B3DFF
	public TeamNum Team
	{
		get
		{
			return this.PlayerInfo.team;
		}
	}

	// Token: 0x1700084F RID: 2127
	// (get) Token: 0x06002410 RID: 9232 RVA: 0x000B5A0C File Offset: 0x000B3E0C
	public PlayerType Type
	{
		get
		{
			return this.PlayerInfo.type;
		}
	}

	// Token: 0x17000850 RID: 2128
	// (get) Token: 0x06002411 RID: 9233 RVA: 0x000B5A1C File Offset: 0x000B3E1C
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
			foreach (PlayerController playerController in this.controllers)
			{
				if (playerController.IsActive)
				{
					return playerController;
				}
			}
			return null;
		}
	}

	// Token: 0x17000851 RID: 2129
	// (get) Token: 0x06002412 RID: 9234 RVA: 0x000B5AB8 File Offset: 0x000B3EB8
	public IEnumerable<PlayerController> AllControllers
	{
		get
		{
			return this.controllers;
		}
	}

	// Token: 0x06002413 RID: 9235 RVA: 0x000B5AC0 File Offset: 0x000B3EC0
	private void assignCpuPlayerInputController()
	{
		GameManager currentGame = this.gameController.currentGame;
		bool flag = false;
		if (currentGame.BattleSettings.mode == GameMode.CrewBattle)
		{
			foreach (PlayerReference playerReference in currentGame.PlayerReferences)
			{
				if (playerReference.Type == PlayerType.Human && playerReference.Team == this.Team)
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
			AIInput aiinput = this.InputController as AIInput;
			this.injector.Inject(aiinput);
			aiinput.PlayerReference = this;
			aiinput.Initialize(this.config.inputConfig);
			this.setupBehaviorTrees();
		}
	}

	// Token: 0x06002414 RID: 9236 RVA: 0x000B5BB0 File Offset: 0x000B3FB0
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
			this.state.lives = int.MaxValue;
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
		Debug.LogWarning(string.Concat(new object[]
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

	// Token: 0x06002415 RID: 9237 RVA: 0x000B5D28 File Offset: 0x000B4128
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

	// Token: 0x17000852 RID: 2130
	// (get) Token: 0x06002416 RID: 9238 RVA: 0x000B5DD3 File Offset: 0x000B41D3
	public bool IsInBattle
	{
		get
		{
			return !this.PlayerInfo.isSpectator && (this.EngagementState == PlayerEngagementState.Primary || this.EngagementState == PlayerEngagementState.Temporary || this.EngagementState == PlayerEngagementState.AssistMove);
		}
	}

	// Token: 0x17000853 RID: 2131
	// (get) Token: 0x06002417 RID: 9239 RVA: 0x000B5E0B File Offset: 0x000B420B
	public bool IsPrimary
	{
		get
		{
			return this.EngagementState == PlayerEngagementState.Primary;
		}
	}

	// Token: 0x17000854 RID: 2132
	// (get) Token: 0x06002418 RID: 9240 RVA: 0x000B5E16 File Offset: 0x000B4216
	public bool IsBenched
	{
		get
		{
			return this.EngagementState == PlayerEngagementState.Benched;
		}
	}

	// Token: 0x17000855 RID: 2133
	// (get) Token: 0x06002419 RID: 9241 RVA: 0x000B5E21 File Offset: 0x000B4221
	public bool IsSpectating
	{
		get
		{
			return this.PlayerInfo.isSpectator;
		}
	}

	// Token: 0x17000856 RID: 2134
	// (get) Token: 0x0600241A RID: 9242 RVA: 0x000B5E2E File Offset: 0x000B422E
	public bool IsTemporary
	{
		get
		{
			return this.EngagementState == PlayerEngagementState.Temporary;
		}
	}

	// Token: 0x17000857 RID: 2135
	// (get) Token: 0x0600241B RID: 9243 RVA: 0x000B5E39 File Offset: 0x000B4239
	public bool IsAllyAssistMove
	{
		get
		{
			return this.EngagementState == PlayerEngagementState.AssistMove;
		}
	}

	// Token: 0x17000858 RID: 2136
	// (get) Token: 0x0600241C RID: 9244 RVA: 0x000B5E44 File Offset: 0x000B4244
	public bool IsDisconnected
	{
		get
		{
			return this.EngagementState == PlayerEngagementState.Disconnected;
		}
	}

	// Token: 0x17000859 RID: 2137
	// (get) Token: 0x0600241D RID: 9245 RVA: 0x000B5E4F File Offset: 0x000B424F
	public bool CanHostTeamMove
	{
		get
		{
			return this.Controller.IsActive && this.IsPrimary;
		}
	}

	// Token: 0x1700085A RID: 2138
	// (get) Token: 0x0600241E RID: 9246 RVA: 0x000B5E6A File Offset: 0x000B426A
	public bool IsPassiveAI
	{
		get
		{
			return this.ActiveBehaviorTree != null && this.ActiveBehaviorTree == this.passiveTree;
		}
	}

	// Token: 0x1700085B RID: 2139
	// (get) Token: 0x0600241F RID: 9247 RVA: 0x000B5E88 File Offset: 0x000B4288
	// (set) Token: 0x06002420 RID: 9248 RVA: 0x000B5E98 File Offset: 0x000B4298
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

	// Token: 0x06002421 RID: 9249 RVA: 0x000B5EFC File Offset: 0x000B42FC
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

	// Token: 0x06002422 RID: 9250 RVA: 0x000B5F3B File Offset: 0x000B433B
	public void AddController(PlayerController controller)
	{
		this.controllers.Add(controller);
	}

	// Token: 0x06002423 RID: 9251 RVA: 0x000B5F4C File Offset: 0x000B434C
	private void onTagPlayerIn(GameEvent message)
	{
		TagInPlayerEvent tagInPlayerEvent = message as TagInPlayerEvent;
		if (tagInPlayerEvent.taggedPlayerNum == this.PlayerNum)
		{
			this.EngagementState = PlayerEngagementState.Primary;
		}
	}

	// Token: 0x06002424 RID: 9252 RVA: 0x000B5F78 File Offset: 0x000B4378
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

	// Token: 0x06002425 RID: 9253 RVA: 0x000B5FA8 File Offset: 0x000B43A8
	public void Destroy()
	{
		this.events.Unsubscribe(typeof(TagInPlayerEvent), new Events.EventHandler(this.onTagPlayerIn));
		this.signalBus.RemoveListener(PlayerReference.TOGGLE_AI, new Action(this.onToggleAI));
	}

	// Token: 0x06002426 RID: 9254 RVA: 0x000B5FE7 File Offset: 0x000B43E7
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<PlayerReferenceState>(ref this.state);
		return true;
	}

	// Token: 0x06002427 RID: 9255 RVA: 0x000B5FF7 File Offset: 0x000B43F7
	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.rollbackStatePooling.Clone<PlayerReferenceState>(this.state));
	}

	// Token: 0x04001B69 RID: 7017
	public static string TOGGLE_AI = "PlayerReference.TOGGLE_AI";

	// Token: 0x04001B73 RID: 7027
	private List<PlayerController> controllers = new List<PlayerController>();

	// Token: 0x04001B74 RID: 7028
	private PlayerReferenceState state;

	// Token: 0x04001B76 RID: 7030
	public SpawnPointReference spawnReference;

	// Token: 0x04001B78 RID: 7032
	public BehaviorTree ActiveBehaviorTree;

	// Token: 0x04001B79 RID: 7033
	public BehaviorTree baseTree;

	// Token: 0x04001B7A RID: 7034
	public BehaviorTree passiveTree;
}
