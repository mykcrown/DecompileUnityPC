using System;
using System.Collections.Generic;
using FixedPoint;
using network;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020002F0 RID: 752
public class DeveloperUtilityConsoleCommands
{
	// Token: 0x170002BD RID: 701
	// (get) Token: 0x06000FC6 RID: 4038 RVA: 0x0005EC89 File Offset: 0x0005D089
	// (set) Token: 0x06000FC7 RID: 4039 RVA: 0x0005EC91 File Offset: 0x0005D091
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x170002BE RID: 702
	// (get) Token: 0x06000FC8 RID: 4040 RVA: 0x0005EC9A File Offset: 0x0005D09A
	// (set) Token: 0x06000FC9 RID: 4041 RVA: 0x0005ECA2 File Offset: 0x0005D0A2
	[Inject]
	public ICharacterSelectModel characterSelectModel { get; set; }

	// Token: 0x170002BF RID: 703
	// (get) Token: 0x06000FCA RID: 4042 RVA: 0x0005ECAB File Offset: 0x0005D0AB
	// (set) Token: 0x06000FCB RID: 4043 RVA: 0x0005ECB3 File Offset: 0x0005D0B3
	[Inject]
	public IServerConnectionManager serverConnectionManager { get; set; }

	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x06000FCC RID: 4044 RVA: 0x0005ECBC File Offset: 0x0005D0BC
	// (set) Token: 0x06000FCD RID: 4045 RVA: 0x0005ECC4 File Offset: 0x0005D0C4
	[Inject]
	public IDevConsole devConsole { get; set; }

	// Token: 0x170002C1 RID: 705
	// (get) Token: 0x06000FCE RID: 4046 RVA: 0x0005ECCD File Offset: 0x0005D0CD
	// (set) Token: 0x06000FCF RID: 4047 RVA: 0x0005ECD5 File Offset: 0x0005D0D5
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x170002C2 RID: 706
	// (get) Token: 0x06000FD0 RID: 4048 RVA: 0x0005ECDE File Offset: 0x0005D0DE
	// (set) Token: 0x06000FD1 RID: 4049 RVA: 0x0005ECE6 File Offset: 0x0005D0E6
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x170002C3 RID: 707
	// (get) Token: 0x06000FD2 RID: 4050 RVA: 0x0005ECEF File Offset: 0x0005D0EF
	// (set) Token: 0x06000FD3 RID: 4051 RVA: 0x0005ECF7 File Offset: 0x0005D0F7
	[Inject]
	public global::ILogger logger { get; set; }

	// Token: 0x170002C4 RID: 708
	// (get) Token: 0x06000FD4 RID: 4052 RVA: 0x0005ED00 File Offset: 0x0005D100
	// (set) Token: 0x06000FD5 RID: 4053 RVA: 0x0005ED08 File Offset: 0x0005D108
	[Inject]
	public IDebugLatencyManager debugLatencyManager { get; set; }

	// Token: 0x170002C5 RID: 709
	// (get) Token: 0x06000FD6 RID: 4054 RVA: 0x0005ED11 File Offset: 0x0005D111
	// (set) Token: 0x06000FD7 RID: 4055 RVA: 0x0005ED19 File Offset: 0x0005D119
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x170002C6 RID: 710
	// (get) Token: 0x06000FD8 RID: 4056 RVA: 0x0005ED22 File Offset: 0x0005D122
	// (set) Token: 0x06000FD9 RID: 4057 RVA: 0x0005ED2A File Offset: 0x0005D12A
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x170002C7 RID: 711
	// (get) Token: 0x06000FDA RID: 4058 RVA: 0x0005ED33 File Offset: 0x0005D133
	// (set) Token: 0x06000FDB RID: 4059 RVA: 0x0005ED3B File Offset: 0x0005D13B
	[Inject]
	public UserAudioSettings audioSettings { get; set; }

	// Token: 0x170002C8 RID: 712
	// (get) Token: 0x06000FDC RID: 4060 RVA: 0x0005ED44 File Offset: 0x0005D144
	// (set) Token: 0x06000FDD RID: 4061 RVA: 0x0005ED4C File Offset: 0x0005D14C
	[Inject]
	public DebugKeys debugKeys { get; set; }

	// Token: 0x170002C9 RID: 713
	// (get) Token: 0x06000FDE RID: 4062 RVA: 0x0005ED55 File Offset: 0x0005D155
	// (set) Token: 0x06000FDF RID: 4063 RVA: 0x0005ED5D File Offset: 0x0005D15D
	[Inject]
	public IUserPurchaseEquipment userPurchase { get; set; }

	// Token: 0x170002CA RID: 714
	// (get) Token: 0x06000FE0 RID: 4064 RVA: 0x0005ED66 File Offset: 0x0005D166
	// (set) Token: 0x06000FE1 RID: 4065 RVA: 0x0005ED6E File Offset: 0x0005D16E
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x170002CB RID: 715
	// (get) Token: 0x06000FE2 RID: 4066 RVA: 0x0005ED77 File Offset: 0x0005D177
	// (set) Token: 0x06000FE3 RID: 4067 RVA: 0x0005ED7F File Offset: 0x0005D17F
	[Inject]
	public IApplicationFramerateManager framerateManager { get; set; }

	// Token: 0x170002CC RID: 716
	// (get) Token: 0x06000FE4 RID: 4068 RVA: 0x0005ED88 File Offset: 0x0005D188
	// (set) Token: 0x06000FE5 RID: 4069 RVA: 0x0005ED90 File Offset: 0x0005D190
	[Inject]
	public GameEnvironmentData environmentData { get; set; }

	// Token: 0x170002CD RID: 717
	// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x0005ED99 File Offset: 0x0005D199
	// (set) Token: 0x06000FE7 RID: 4071 RVA: 0x0005EDA1 File Offset: 0x0005D1A1
	[Inject]
	public ICharacterDataLoader characterDataLoader { get; set; }

	// Token: 0x170002CE RID: 718
	// (get) Token: 0x06000FE8 RID: 4072 RVA: 0x0005EDAA File Offset: 0x0005D1AA
	// (set) Token: 0x06000FE9 RID: 4073 RVA: 0x0005EDB2 File Offset: 0x0005D1B2
	[Inject]
	public P2PServerMgr p2pServerMgr { get; set; }

	// Token: 0x170002CF RID: 719
	// (get) Token: 0x06000FEA RID: 4074 RVA: 0x0005EDBB File Offset: 0x0005D1BB
	// (set) Token: 0x06000FEB RID: 4075 RVA: 0x0005EDC3 File Offset: 0x0005D1C3
	[Inject]
	public IPingManager pingManager { private get; set; }

	// Token: 0x06000FEC RID: 4076 RVA: 0x0005EDCC File Offset: 0x0005D1CC
	public void Init()
	{
		this.devConsole.AddCommand<string>(delegate(string id)
		{
			this.forcePlayerOn(PlayerNum.Player1, id, PlayerType.Human);
		}, "room", "set_p1", "Sets Player1 to a human player with character <characterId>, use 'empty' for <characterId> to clear player.");
		this.devConsole.AddCommand<string>(delegate(string id)
		{
			this.forcePlayerOn(PlayerNum.Player2, id, PlayerType.Human);
		}, "room", "set_p2", "Sets Player2 to a human player with character <characterId>, use 'empty' for <characterId> to clear player.");
		this.devConsole.AddCommand<string>(delegate(string id)
		{
			this.forcePlayerOn(PlayerNum.Player3, id, PlayerType.Human);
		}, "room", "set_p3", "Sets Player3 to a human player with character <characterId>, use 'empty' for <characterId> to clear player.");
		this.devConsole.AddCommand<string>(delegate(string id)
		{
			this.forcePlayerOn(PlayerNum.Player4, id, PlayerType.Human);
		}, "room", "set_p4", "Sets Player4 to a human player with character <characterId>, use 'empty' for <characterId> to clear player.");
		this.devConsole.AddCommand<string>(delegate(string id)
		{
			this.forcePlayerOn(PlayerNum.Player5, id, PlayerType.Human);
		}, "room", "set_p5", "Sets Player5 to a human player with character <characterId>, use 'empty' for <characterId> to clear player.");
		this.devConsole.AddCommand<string>(delegate(string id)
		{
			this.forcePlayerOn(PlayerNum.Player6, id, PlayerType.Human);
		}, "room", "set_p6", "Sets Player6 to a human player with character <characterId>, use 'empty' for <characterId> to clear player.");
		this.devConsole.AddCommand<string>(delegate(string id)
		{
			this.forcePlayerOn(PlayerNum.Player1, id, PlayerType.CPU);
		}, "room", "set_p1_comp", "Sets Player1 to a computer player with character <characterId>, use 'empty' for <characterId> to clear player.");
		this.devConsole.AddCommand<string>(delegate(string id)
		{
			this.forcePlayerOn(PlayerNum.Player2, id, PlayerType.CPU);
		}, "room", "set_p2_comp", "Sets Player2 to a computer player with character <characterId>, use 'empty' for <characterId> to clear player.");
		this.devConsole.AddCommand<string>(delegate(string id)
		{
			this.forcePlayerOn(PlayerNum.Player3, id, PlayerType.CPU);
		}, "room", "set_p3_comp", "Sets Player3 to a computer player with character <characterId>, use 'empty' for <characterId> to clear player.");
		this.devConsole.AddCommand<string>(delegate(string id)
		{
			this.forcePlayerOn(PlayerNum.Player4, id, PlayerType.CPU);
		}, "room", "set_p4_comp", "Sets Player4 to a computer player with character <characterId>, use 'empty' for <characterId> to clear player.");
		this.devConsole.AddCommand<string>(delegate(string id)
		{
			this.forcePlayerOn(PlayerNum.Player5, id, PlayerType.CPU);
		}, "room", "set_p5_comp", "Sets Player5 to a computer player with character <characterId>, use 'empty' for <characterId> to clear player.");
		this.devConsole.AddCommand<string>(delegate(string id)
		{
			this.forcePlayerOn(PlayerNum.Player6, id, PlayerType.CPU);
		}, "room", "set_p6_comp", "Sets Player6 to a computer player with character <characterId>, use 'empty' for <characterId> to clear player.");
		this.devConsole.AddCommand(delegate()
		{
			this.displayCharacterIds(PlayerNum.Player1);
		}, "room", "set_p1", "List available characterIds for setp1");
		this.devConsole.AddCommand(delegate()
		{
			this.displayCharacterIds(PlayerNum.Player2);
		}, "room", "set_p2", "List available characterIds for setp2");
		this.devConsole.AddCommand(delegate()
		{
			this.displayCharacterIds(PlayerNum.Player3);
		}, "room", "set_p3", "List available characterIds for setp3");
		this.devConsole.AddCommand(delegate()
		{
			this.displayCharacterIds(PlayerNum.Player4);
		}, "room", "set_p4", "List available characterIds for setp4");
		this.devConsole.AddCommand(delegate()
		{
			this.displayCharacterIds(PlayerNum.Player5);
		}, "room", "set_p5", "List available characterIds for setp5");
		this.devConsole.AddCommand(delegate()
		{
			this.displayCharacterIds(PlayerNum.Player6);
		}, "room", "set_p6", "List available characterIds for setp6");
		this.devConsole.AddConsoleVariable<int>("frames", "framerate", "Override Unity targetFramerate", string.Empty, new Func<int>(this.getFramerate), new Action<int>(this.setFramerate));
		this.devConsole.AddConsoleVariable<string>("game", "tag_p1", "Set player nametag", "Sets player nametag (only works on character select screen)", new Func<string>(this.getPlayerNametag), new Action<string>(this.setPlayerNametag1));
		this.devConsole.AddConsoleVariable<string>("game", "tag_p2", "Set player nametag", "Sets player nametag (only works on character select screen)", new Func<string>(this.getPlayerNametag), new Action<string>(this.setPlayerNametag2));
		this.devConsole.AddConsoleVariable<string>("game", "tag_p3", "Set player nametag", "Sets player nametag (only works on character select screen)", new Func<string>(this.getPlayerNametag), new Action<string>(this.setPlayerNametag3));
		this.devConsole.AddConsoleVariable<string>("game", "tag_p4", "Set player nametag", "Sets player nametag (only works on character select screen)", new Func<string>(this.getPlayerNametag), new Action<string>(this.setPlayerNametag4));
		this.devConsole.AddConsoleVariable<string>("game", "tag_p5", "Set player nametag", "Sets player nametag (only works on character select screen)", new Func<string>(this.getPlayerNametag), new Action<string>(this.setPlayerNametag5));
		this.devConsole.AddConsoleVariable<string>("game", "tag_p6", "Set player nametag", "Sets player nametag (only works on character select screen)", new Func<string>(this.getPlayerNametag), new Action<string>(this.setPlayerNametag6));
		this.devConsole.AddConsoleVariable<int>("network", "input_delay", "Network Input Delay", "Number of frames of input delay used during online matches.", () => this.gameDataManager.ConfigData.networkSettings.inputDelayFrames, delegate(int value)
		{
			this.gameDataManager.ConfigData.networkSettings.inputDelayFrames = value;
		});
		this.devConsole.AddConsoleVariable<int>("network", "outbound_delay", "Network Outbound Message Delay", "Add fake latency to outgoing messages", () => this.debugLatencyManager.AddOutboundLatency, delegate(int value)
		{
			this.debugLatencyManager.AddOutboundLatency = value;
		});
		this.devConsole.AddConsoleVariable<int>("network", "inbound_delay", "Network Inbound Message Delay", "Add fake latency to incoming messages", () => this.debugLatencyManager.AddInboundLatency, delegate(int value)
		{
			this.debugLatencyManager.AddInboundLatency = value;
		});
		this.devConsole.AddConsoleVariable<float>("network", "udp_drop_rate", "UDP Drop Rate", "Percentage of udp packets to drop", () => this.gameDataManager.ConfigData.networkSettings.debugUdpDropRate, delegate(float value)
		{
			this.gameDataManager.ConfigData.networkSettings.debugUdpDropRate = value;
		});
		this.devConsole.AddCommand(delegate()
		{
			this.events.Broadcast(new DebugThrowExceptionEvent());
		}, "debug", "throwException", null);
		this.devConsole.AddCommand(delegate()
		{
			this.events.Broadcast(new DebugDesyncEvent());
		}, "debug", "desync", null);
		this.devConsole.AddConsoleVariable<ServerEnvironment>("network", "server_env", "Server Environment", "Which server stack to connect to.", () => this.serverConnectionManager.ServerEnv, new Action<ServerEnvironment>(this.changeServerEnv));
		this.devConsole.AddConsoleVariable<string>("network", "server_envCustom", "Server Environment", "Which server stack to connect to.", () => this.serverConnectionManager.EndPoint, new Action<string>(this.changeServerConnectIP));
		this.devConsole.AddCommand(delegate()
		{
			this.devConsole.ExecuteCommand("room.set_p2 kidd");
			this.devConsole.ExecuteCommand("room.set_p3 xana");
			this.devConsole.ExecuteCommand("room.set_p4 zhurong");
		}, "room", "quick_fill", null);
		this.devConsole.AddCommand(delegate()
		{
			this.devConsole.ExecuteCommand("room.set_p1 ashani");
			this.devConsole.ExecuteCommand("room.set_p4_comp random");
		}, "room", "kris_fill", null);
		this.devConsole.AddCommand(new Action(this.forceTestingMode), "room", "testing", "Force the game mode to Testing.");
		this.devConsole.AddCommand(new Action(this.getMoney), "commerce", "get_money", "Cheat to get soft currency.");
		this.devConsole.AddCommand(new Action(this.forceSpectator), "network", "force_spectator", string.Empty);
		this.devConsole.AddCommand<int>(new Action<int>(this.advanceToFrame), "game", "advance_to", "Advance to frame N");
		this.devConsole.AddCommand<int>(new Action<int>(this.joinLobby), "network", "join", string.Empty);
		this.devConsole.AddCommand(new Action(this.onUpdateCustomLobby), "network", "updateLobby", null);
		this.devConsole.AddCommand(new Action(this.testP2PPing), "network", "ping", string.Empty);
		this.devConsole.AddCommand<int>(new Action<int>(this.advanceFrames), "game", "advance_frames", "Advance N frames.");
		this.devConsole.AddCommand(delegate()
		{
			this.advanceFrames(1);
		}, "game", "advance_frames", "Advance N frames.");
		this.devConsole.AddConsoleVariable<bool>("physics", "debug_raycasts", "Debug Physics Raycasts", "Show raycasts in physics debug view?", () => PhysicsWorld.EnableRaycastDebugging, delegate(bool value)
		{
			PhysicsWorld.EnableRaycastDebugging = value;
		});
		this.devConsole.AddConsoleVariable<LogLevel>("debug", "log_level", "Log Level", "Lowest log level that will be logged.  Everything below in priority will be ignored.", () => this.logger.LogLevel, delegate(LogLevel value)
		{
			this.logger.LogLevel = value;
		});
		this.devConsole.AddConsoleVariable<LogLevel>("debug", "console_log_level", "Console Log Level", "Lowest log level the dev console will log.", () => (this.logger as BroadcastingLogger).GetChildLogger<ConsoleLogger>().LogLevel, delegate(LogLevel value)
		{
			(this.logger as BroadcastingLogger).GetChildLogger<ConsoleLogger>().LogLevel = value;
		});
		this.devConsole.AddConsoleVariable<int>("debug", "grid_width", "Debug Grid Width", "Width of the debug grid.", () => DebugDraw.Instance.GridWidth, delegate(int value)
		{
			DebugDraw.Instance.GridWidth = value;
		});
		this.devConsole.AddConsoleVariable<int>("debug", "grid_height", "Debug Grid Height", "Height of the debug grid.", () => DebugDraw.Instance.GridHeight, delegate(int value)
		{
			DebugDraw.Instance.GridHeight = value;
		});
		this.devConsole.AddConsoleVariable<DebugDrawChannel>("debug", "draw_channel", "Active Draw Channels", "Flags enum of active debug draw channels.", () => DebugDraw.Instance.ActiveChannels, delegate(DebugDrawChannel value)
		{
			DebugDraw.Instance.ActiveChannels = value;
		});
		this.devConsole.AddConsoleVariable<bool>("debug", "draw_physics", "Draw physics", "Draw physics arrows (need to activate gizmos in editor)", () => DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Physics), delegate(bool value)
		{
			DebugDraw.Instance.SetChannelActive(DebugDrawChannel.Physics, value);
		});
		this.devConsole.AddConsoleVariable<bool>("debug", "draw_hit_boxes", "Draw hit boxes", "Draw player hit boxes", () => DebugDraw.Instance.IsChannelActive(DebugDrawChannel.HitBoxes), delegate(bool value)
		{
			DebugDraw.Instance.SetChannelActive(DebugDrawChannel.HitBoxes, value);
		});
		this.devConsole.AddConsoleVariable<bool>("debug", "draw_hurt_boxes", "Draw hurt boxes", "Draw player hurt boxes", () => DebugDraw.Instance.IsChannelActive(DebugDrawChannel.HurtBoxes), delegate(bool value)
		{
			DebugDraw.Instance.SetChannelActive(DebugDrawChannel.HurtBoxes, value);
		});
		this.devConsole.AddConsoleVariable<bool>("debug", "draw_bounds", "Draw bounds", "Draw player environment bounds (need to activate gizmos in editor)", () => DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Bounds), delegate(bool value)
		{
			DebugDraw.Instance.SetChannelActive(DebugDrawChannel.Bounds, value);
		});
		this.devConsole.AddConsoleVariable<bool>("debug", "draw_input", "Draw input", "Draw left axis input grid (need to activate gizmos in editor)", () => DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Input), delegate(bool value)
		{
			DebugDraw.Instance.SetChannelActive(DebugDrawChannel.Input, value);
		});
		this.devConsole.AddConsoleVariable<bool>("debug", "draw_camera", "Draw camera", "Draw camera bounds and camera influence zones (need to activate gizmos in editor)", () => DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Camera), delegate(bool value)
		{
			DebugDraw.Instance.SetChannelActive(DebugDrawChannel.Camera, value);
		});
		this.devConsole.AddConsoleVariable<bool>("debug", "draw_impact", "Draw impact", "Draw knockback vector (need to activate gizmos in editor)", () => DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Impact), delegate(bool value)
		{
			DebugDraw.Instance.SetChannelActive(DebugDrawChannel.Impact, value);
		});
		this.devConsole.AddConsoleVariable<bool>("debug", "draw_grid", "Draw grid", "Draw position grid.", () => DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Grid), delegate(bool value)
		{
			DebugDraw.Instance.SetChannelActive(DebugDrawChannel.Grid, value);
		});
		this.devConsole.AddConsoleVariable<bool>("demo", "demo_mode", "Demo Mode", "Enables or disables demo mode, which controls access to some data and features.", () => DemoSettings.DemoModeEnabled, delegate(bool value)
		{
			DemoSettings.DemoModeEnabled = value;
		});
		this.devConsole.AddConsoleVariable<bool>("demo", "playtest_logging", "Playtest Logging", "Enables logging useful for debugging issues in playtests.", () => DemoSettings.PlaytestLoggingEnabled, delegate(bool value)
		{
			DemoSettings.PlaytestLoggingEnabled = value;
		});
		this.devConsole.AddConsoleVariable<float>("audio", "music_volume", "Set music volume", "Sets the music volume", () => this.audioSettings.GetMusicVolume(), delegate(float value)
		{
			this.audioSettings.SetMusic(value);
		});
		this.devConsole.AddConsoleVariable<float>("audio", "sfx_volume", "Set sfx volume", "Sets the sfx volume", () => this.audioSettings.GetSoundEffectsVolume(), delegate(float value)
		{
			this.audioSettings.SetSoundEffects(value);
		});
		this.devConsole.AddPlayerConsoleVariable<float>("damage", "Damage", "Amount of damage on player.", new Func<PlayerNum, float>(this.getPlayerDamage), new Action<PlayerNum, float>(this.setPlayerDamage));
		this.devConsole.AddPlayerCommand<int>(new Action<PlayerNum, int>(this.impactPlayer), "impact", null);
		this.devConsole.AddPlayerConsoleVariable<string>("position", "Position", "Position of player.", new Func<PlayerNum, string>(this.getPlayerPosition), new Action<PlayerNum, string>(this.setPlayerPosition));
		this.devConsole.AddCommand(delegate()
		{
			Application.Quit();
		}, "game", "quit", null);
		this.devConsole.AddCommand(delegate()
		{
			this.events.Broadcast(new LoadScreenCommand(ScreenType.NoUI, null, ScreenUpdateType.Next));
			SceneManager.LoadScene("CharacterProfilerScene");
		}, "debug", "enter_character_profiler", null);
		this.devConsole.AddConsoleVariable<bool>("camera", "movement_disabled", "Disable camera movement", "Disables the automatic camera movement logic", () => this.debugKeys.CameraMovementDisabled, delegate(bool value)
		{
			this.debugKeys.CameraMovementDisabled = value;
		});
		this.devConsole.AddConsoleVariable<FrameSyncMode>("frames", "syncMode", "Frame Sync Mode", "Frame sync mode (technical)", () => this.framerateManager.frameSyncMode, delegate(FrameSyncMode value)
		{
			this.framerateManager.frameSyncMode = value;
		});
	}

	// Token: 0x06000FED RID: 4077 RVA: 0x0005FC28 File Offset: 0x0005E028
	private int getFramerate()
	{
		return this.framerateManager.overrideTargetFramerate;
	}

	// Token: 0x06000FEE RID: 4078 RVA: 0x0005FC35 File Offset: 0x0005E035
	private void setFramerate(int value)
	{
		this.framerateManager.overrideTargetFramerate = value;
	}

	// Token: 0x06000FEF RID: 4079 RVA: 0x0005FC43 File Offset: 0x0005E043
	private void changeServerEnv(ServerEnvironment value)
	{
		this.gameDataManager.ConfigData.networkSettings.overrideServerEnv = true;
		this.gameDataManager.ConfigData.networkSettings.overrideEnv = value;
		this.serverConnectionManager.Disconnect();
	}

	// Token: 0x06000FF0 RID: 4080 RVA: 0x0005FC7C File Offset: 0x0005E07C
	private void changeServerConnectIP(string netAddressRaw)
	{
		string[] array = netAddressRaw.Split(new char[]
		{
			':'
		});
		if (array.Length != 2)
		{
			this.devConsole.PrintLn("Invalid args. Enter in ip:port format");
			return;
		}
		this.gameDataManager.ConfigData.networkSettings.overrideServerEnv = true;
		this.gameDataManager.ConfigData.networkSettings.overrideEnv = ServerEnvironment.CUSTOM;
		this.gameDataManager.ConfigData.networkSettings.customIP = array[0];
		this.gameDataManager.ConfigData.networkSettings.customPort = uint.Parse(array[1]);
		this.serverConnectionManager.Disconnect();
	}

	// Token: 0x06000FF1 RID: 4081 RVA: 0x0005FD24 File Offset: 0x0005E124
	private void impactPlayer(PlayerNum playerNum, int angle)
	{
		if (this.gameController.currentGame == null)
		{
			this.devConsole.PrintLn("No battle found");
			return;
		}
		PlayerController playerController = this.gameController.currentGame.GetPlayerController(playerNum);
		if (playerController.Facing == HorizontalDirection.Left)
		{
			angle = 180 - angle;
		}
		HitData hitData = new HitData();
		hitData.baseKnockback = 100f;
		hitData.knockbackAngle = (float)angle;
		hitData.damage = 0f;
		hitData.knockbackScaling = 0f;
		Vector3F center = playerController.Physics.Center;
		HitContext hitContext = new HitContext();
		hitContext.collisionPosition = center;
		playerController.Combat.OnHitImpact(hitData, playerController, ImpactType.Hit, ref hitContext);
	}

	// Token: 0x06000FF2 RID: 4082 RVA: 0x0005FDD7 File Offset: 0x0005E1D7
	private float getPlayerDamage(PlayerNum playerNum)
	{
		return (float)this.gameController.currentGame.GetPlayerController(playerNum).Model.damage;
	}

	// Token: 0x06000FF3 RID: 4083 RVA: 0x0005FDF9 File Offset: 0x0005E1F9
	private void setPlayerDamage(PlayerNum playerNum, float damage)
	{
		this.gameController.currentGame.GetPlayerController(playerNum).Model.damage = (Fixed)((double)damage);
	}

	// Token: 0x06000FF4 RID: 4084 RVA: 0x0005FE20 File Offset: 0x0005E220
	private string getPlayerPosition(PlayerNum playerNum)
	{
		return this.gameController.currentGame.GetPlayerController(playerNum).Physics.GetPosition().ToString();
	}

	// Token: 0x06000FF5 RID: 4085 RVA: 0x0005FE58 File Offset: 0x0005E258
	private void setPlayerPosition(PlayerNum playerNum, string positionStr)
	{
		Vector2F v;
		if (Vector2F.TryParse(positionStr, out v))
		{
			this.gameController.currentGame.GetPlayerController(playerNum).Physics.SetPosition(v);
		}
		else
		{
			this.logger.LogFormat(LogLevel.Error, "Unable to parse {0} as Vector2F.  Must be in the form \"x,y\".", new object[]
			{
				positionStr
			});
		}
	}

	// Token: 0x06000FF6 RID: 4086 RVA: 0x0005FEB3 File Offset: 0x0005E2B3
	private string getPlayerNametag()
	{
		return string.Empty;
	}

	// Token: 0x06000FF7 RID: 4087 RVA: 0x0005FEBA File Offset: 0x0005E2BA
	private void setPlayerNametag1(string value)
	{
		this.setPlayerNametag(PlayerNum.Player1, value);
	}

	// Token: 0x06000FF8 RID: 4088 RVA: 0x0005FEC4 File Offset: 0x0005E2C4
	private void setPlayerNametag2(string value)
	{
		this.setPlayerNametag(PlayerNum.Player2, value);
	}

	// Token: 0x06000FF9 RID: 4089 RVA: 0x0005FECE File Offset: 0x0005E2CE
	private void setPlayerNametag3(string value)
	{
		this.setPlayerNametag(PlayerNum.Player3, value);
	}

	// Token: 0x06000FFA RID: 4090 RVA: 0x0005FED8 File Offset: 0x0005E2D8
	private void setPlayerNametag4(string value)
	{
		this.setPlayerNametag(PlayerNum.Player4, value);
	}

	// Token: 0x06000FFB RID: 4091 RVA: 0x0005FEE2 File Offset: 0x0005E2E2
	private void setPlayerNametag5(string value)
	{
		this.setPlayerNametag(PlayerNum.Player5, value);
	}

	// Token: 0x06000FFC RID: 4092 RVA: 0x0005FEEC File Offset: 0x0005E2EC
	private void setPlayerNametag6(string value)
	{
		this.setPlayerNametag(PlayerNum.Player6, value);
	}

	// Token: 0x06000FFD RID: 4093 RVA: 0x0005FEF6 File Offset: 0x0005E2F6
	private void setPlayerNametag(PlayerNum playerNum, string value)
	{
		this.signalBus.GetSignal<SetPlayerProfileNameSignal>().Dispatch(playerNum, value);
	}

	// Token: 0x06000FFE RID: 4094 RVA: 0x0005FF0C File Offset: 0x0005E30C
	private void forcePlayerOn(PlayerNum playerNum, string characterName, PlayerType playerType)
	{
		List<CharacterDefinition> characters = this.environmentData.characters;
		CharacterID characterID = CharacterID.None;
		CharacterDefinition characterDefinition = null;
		bool flag = false;
		if (characterName != null)
		{
			foreach (CharacterDefinition characterDefinition2 in characters)
			{
				if (characterName.EqualsIgnoreCase(characterDefinition2.characterName))
				{
					flag = true;
					characterName = characterDefinition2.characterName;
					characterID = characterDefinition2.characterID;
					characterDefinition = characterDefinition2;
					break;
				}
			}
		}
		if (!flag)
		{
			if (characterName.EqualsIgnoreCase("empty") || characterName.EqualsIgnoreCase("none") || characterName.EqualsIgnoreCase("null"))
			{
				this.userInputManager.Unbind(playerNum);
				this.events.Broadcast(new SetPlayerTypeRequest(playerNum, PlayerType.None, true));
				this.devConsole.PrintLn(string.Format("Cleared player assignment for {0}.", playerNum.ToString()));
				return;
			}
			this.devConsole.PrintLn("Invalid character id!");
			this.displayCharacterIds(playerNum);
			return;
		}
		else
		{
			if (!characterDefinition.enabled)
			{
				this.devConsole.PrintLn(string.Format("Character '{0}' is not enabled.", characterName));
				return;
			}
			if (!this.characterSelectModel.IsCharacterUnlocked(characterID))
			{
				this.devConsole.PrintLn(string.Format("Character '{0}' is not unlocked.", characterName));
				return;
			}
			this.userInputManager.ForceBindAvailablePortToPlayer(playerNum);
			this.events.Broadcast(new SetPlayerTypeRequest(playerNum, playerType, true));
			this.events.Broadcast(new SelectCharacterRequest(playerNum, characterID));
			this.devConsole.PrintLn(string.Format("Set {0} to {1}.", playerNum.ToString(), characterName));
			return;
		}
	}

	// Token: 0x06000FFF RID: 4095 RVA: 0x000600D4 File Offset: 0x0005E4D4
	private void displayCharacterIds(PlayerNum playerNum)
	{
		string arg;
		switch (playerNum)
		{
		default:
			arg = "1";
			break;
		case PlayerNum.Player2:
			arg = "2";
			break;
		case PlayerNum.Player3:
			arg = "3";
			break;
		case PlayerNum.Player4:
			arg = "4";
			break;
		case PlayerNum.Player5:
			arg = "5";
			break;
		case PlayerNum.Player6:
			arg = "6";
			break;
		case PlayerNum.Player7:
			arg = "7";
			break;
		case PlayerNum.Player8:
			arg = "8";
			break;
		}
		this.devConsole.PrintLn(string.Format("Available character ids: room.setp{0} <character id>", arg));
		foreach (CharacterDefinition characterDefinition in this.environmentData.characters)
		{
			if (characterDefinition.enabled && this.characterSelectModel.IsCharacterUnlocked(characterDefinition.characterID))
			{
				this.devConsole.PrintLn(string.Format("  {0}", characterDefinition.characterName));
			}
		}
	}

	// Token: 0x06001000 RID: 4096 RVA: 0x0006020C File Offset: 0x0005E60C
	private void getMoney()
	{
		this.userPurchase.PurchaseManual(1UL, CurrencyType.Soft, 0UL, delegate(UserPurchaseResult result)
		{
			Debug.Log("Soft currency result " + result);
		});
	}

	// Token: 0x06001001 RID: 4097 RVA: 0x0006023B File Offset: 0x0005E63B
	private void forceSpectator()
	{
	}

	// Token: 0x06001002 RID: 4098 RVA: 0x0006023D File Offset: 0x0005E63D
	private void forceTestingMode()
	{
		this.devConsole.PrintLn("Setting game rules to Testing mode... good luck with that bug you're stuck on!");
		this.events.Broadcast(new SetBattleSettingRequest(BattleSettingType.Mode, 6));
	}

	// Token: 0x06001003 RID: 4099 RVA: 0x00060261 File Offset: 0x0005E661
	private void advanceFrames(int frames)
	{
		this.devConsole.PrintLn(string.Format("Advancing {0} frames.", frames));
		this.events.Broadcast(new DebugAdvanceFrameEvent(frames));
	}

	// Token: 0x06001004 RID: 4100 RVA: 0x0006028F File Offset: 0x0005E68F
	private void joinLobby(int lobbySteamID)
	{
	}

	// Token: 0x06001005 RID: 4101 RVA: 0x00060291 File Offset: 0x0005E691
	private void onUpdateCustomLobby()
	{
		this.p2pServerMgr.OnUpdateCustomLobby();
	}

	// Token: 0x06001006 RID: 4102 RVA: 0x0006029E File Offset: 0x0005E69E
	private void testP2PPing()
	{
		this.pingManager.Ping();
	}

	// Token: 0x06001007 RID: 4103 RVA: 0x000602AC File Offset: 0x0005E6AC
	private void advanceToFrame(int frame)
	{
		if (this.gameController.currentGame == null)
		{
			this.devConsole.PrintLn("Cannot advance to a frame if there is no active game.");
			return;
		}
		int num = frame - this.gameController.currentGame.Frame;
		if (num <= 0)
		{
			this.devConsole.PrintLn("Cannot advance to a previous frame.");
			return;
		}
		this.devConsole.PrintLn("Advancing to frame " + frame + ".");
		this.events.Broadcast(new DebugAdvanceFrameEvent(num));
	}
}
