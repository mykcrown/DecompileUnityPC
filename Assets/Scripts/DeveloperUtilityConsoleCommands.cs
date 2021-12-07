// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using network;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeveloperUtilityConsoleCommands
{
	private static Func<bool> __f__am_cache0;

	private static Action<bool> __f__am_cache1;

	private static Func<int> __f__am_cache2;

	private static Action<int> __f__am_cache3;

	private static Func<int> __f__am_cache4;

	private static Action<int> __f__am_cache5;

	private static Func<DebugDrawChannel> __f__am_cache6;

	private static Action<DebugDrawChannel> __f__am_cache7;

	private static Func<bool> __f__am_cache8;

	private static Action<bool> __f__am_cache9;

	private static Func<bool> __f__am_cacheA;

	private static Action<bool> __f__am_cacheB;

	private static Func<bool> __f__am_cacheC;

	private static Action<bool> __f__am_cacheD;

	private static Func<bool> __f__am_cacheE;

	private static Action<bool> __f__am_cacheF;

	private static Func<bool> __f__am_cache10;

	private static Action<bool> __f__am_cache11;

	private static Func<bool> __f__am_cache12;

	private static Action<bool> __f__am_cache13;

	private static Func<bool> __f__am_cache14;

	private static Action<bool> __f__am_cache15;

	private static Func<bool> __f__am_cache16;

	private static Action<bool> __f__am_cache17;

	private static Func<bool> __f__am_cache18;

	private static Action<bool> __f__am_cache19;

	private static Func<bool> __f__am_cache1A;

	private static Action<bool> __f__am_cache1B;

	private static Action __f__am_cache1C;

	private static Action<UserPurchaseResult> __f__am_cache1D;

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	[Inject]
	public ICharacterSelectModel characterSelectModel
	{
		get;
		set;
	}

	[Inject]
	public IServerConnectionManager serverConnectionManager
	{
		get;
		set;
	}

	[Inject]
	public IDevConsole devConsole
	{
		get;
		set;
	}

	[Inject]
	public IUserInputManager userInputManager
	{
		get;
		set;
	}

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public global::ILogger logger
	{
		get;
		set;
	}

	[Inject]
	public IDebugLatencyManager debugLatencyManager
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
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
	public UserAudioSettings audioSettings
	{
		get;
		set;
	}

	[Inject]
	public DebugKeys debugKeys
	{
		get;
		set;
	}

	[Inject]
	public IUserPurchaseEquipment userPurchase
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
	public IApplicationFramerateManager framerateManager
	{
		get;
		set;
	}

	[Inject]
	public GameEnvironmentData environmentData
	{
		get;
		set;
	}

	[Inject]
	public ICharacterDataLoader characterDataLoader
	{
		get;
		set;
	}

	[Inject]
	public P2PServerMgr p2pServerMgr
	{
		get;
		set;
	}

	[Inject]
	public IPingManager pingManager
	{
		private get;
		set;
	}

	public void Init()
	{
		this.devConsole.AddCommand<string>(new Action<string>(this._Init_m__0), "room", "set_p1", "Sets Player1 to a human player with character <characterId>, use 'empty' for <characterId> to clear player.");
		this.devConsole.AddCommand<string>(new Action<string>(this._Init_m__1), "room", "set_p2", "Sets Player2 to a human player with character <characterId>, use 'empty' for <characterId> to clear player.");
		this.devConsole.AddCommand<string>(new Action<string>(this._Init_m__2), "room", "set_p3", "Sets Player3 to a human player with character <characterId>, use 'empty' for <characterId> to clear player.");
		this.devConsole.AddCommand<string>(new Action<string>(this._Init_m__3), "room", "set_p4", "Sets Player4 to a human player with character <characterId>, use 'empty' for <characterId> to clear player.");
		this.devConsole.AddCommand<string>(new Action<string>(this._Init_m__4), "room", "set_p5", "Sets Player5 to a human player with character <characterId>, use 'empty' for <characterId> to clear player.");
		this.devConsole.AddCommand<string>(new Action<string>(this._Init_m__5), "room", "set_p6", "Sets Player6 to a human player with character <characterId>, use 'empty' for <characterId> to clear player.");
		this.devConsole.AddCommand<string>(new Action<string>(this._Init_m__6), "room", "set_p1_comp", "Sets Player1 to a computer player with character <characterId>, use 'empty' for <characterId> to clear player.");
		this.devConsole.AddCommand<string>(new Action<string>(this._Init_m__7), "room", "set_p2_comp", "Sets Player2 to a computer player with character <characterId>, use 'empty' for <characterId> to clear player.");
		this.devConsole.AddCommand<string>(new Action<string>(this._Init_m__8), "room", "set_p3_comp", "Sets Player3 to a computer player with character <characterId>, use 'empty' for <characterId> to clear player.");
		this.devConsole.AddCommand<string>(new Action<string>(this._Init_m__9), "room", "set_p4_comp", "Sets Player4 to a computer player with character <characterId>, use 'empty' for <characterId> to clear player.");
		this.devConsole.AddCommand<string>(new Action<string>(this._Init_m__A), "room", "set_p5_comp", "Sets Player5 to a computer player with character <characterId>, use 'empty' for <characterId> to clear player.");
		this.devConsole.AddCommand<string>(new Action<string>(this._Init_m__B), "room", "set_p6_comp", "Sets Player6 to a computer player with character <characterId>, use 'empty' for <characterId> to clear player.");
		this.devConsole.AddCommand(new Action(this._Init_m__C), "room", "set_p1", "List available characterIds for setp1");
		this.devConsole.AddCommand(new Action(this._Init_m__D), "room", "set_p2", "List available characterIds for setp2");
		this.devConsole.AddCommand(new Action(this._Init_m__E), "room", "set_p3", "List available characterIds for setp3");
		this.devConsole.AddCommand(new Action(this._Init_m__F), "room", "set_p4", "List available characterIds for setp4");
		this.devConsole.AddCommand(new Action(this._Init_m__10), "room", "set_p5", "List available characterIds for setp5");
		this.devConsole.AddCommand(new Action(this._Init_m__11), "room", "set_p6", "List available characterIds for setp6");
		this.devConsole.AddConsoleVariable<int>("frames", "framerate", "Override Unity targetFramerate", string.Empty, new Func<int>(this.getFramerate), new Action<int>(this.setFramerate));
		this.devConsole.AddConsoleVariable<string>("game", "tag_p1", "Set player nametag", "Sets player nametag (only works on character select screen)", new Func<string>(this.getPlayerNametag), new Action<string>(this.setPlayerNametag1));
		this.devConsole.AddConsoleVariable<string>("game", "tag_p2", "Set player nametag", "Sets player nametag (only works on character select screen)", new Func<string>(this.getPlayerNametag), new Action<string>(this.setPlayerNametag2));
		this.devConsole.AddConsoleVariable<string>("game", "tag_p3", "Set player nametag", "Sets player nametag (only works on character select screen)", new Func<string>(this.getPlayerNametag), new Action<string>(this.setPlayerNametag3));
		this.devConsole.AddConsoleVariable<string>("game", "tag_p4", "Set player nametag", "Sets player nametag (only works on character select screen)", new Func<string>(this.getPlayerNametag), new Action<string>(this.setPlayerNametag4));
		this.devConsole.AddConsoleVariable<string>("game", "tag_p5", "Set player nametag", "Sets player nametag (only works on character select screen)", new Func<string>(this.getPlayerNametag), new Action<string>(this.setPlayerNametag5));
		this.devConsole.AddConsoleVariable<string>("game", "tag_p6", "Set player nametag", "Sets player nametag (only works on character select screen)", new Func<string>(this.getPlayerNametag), new Action<string>(this.setPlayerNametag6));
		this.devConsole.AddConsoleVariable<int>("network", "input_delay", "Network Input Delay", "Number of frames of input delay used during online matches.", new Func<int>(this._Init_m__12), new Action<int>(this._Init_m__13));
		this.devConsole.AddConsoleVariable<int>("network", "outbound_delay", "Network Outbound Message Delay", "Add fake latency to outgoing messages", new Func<int>(this._Init_m__14), new Action<int>(this._Init_m__15));
		this.devConsole.AddConsoleVariable<int>("network", "inbound_delay", "Network Inbound Message Delay", "Add fake latency to incoming messages", new Func<int>(this._Init_m__16), new Action<int>(this._Init_m__17));
		this.devConsole.AddConsoleVariable<float>("network", "udp_drop_rate", "UDP Drop Rate", "Percentage of udp packets to drop", new Func<float>(this._Init_m__18), new Action<float>(this._Init_m__19));
		this.devConsole.AddCommand(new Action(this._Init_m__1A), "debug", "throwException", null);
		this.devConsole.AddCommand(new Action(this._Init_m__1B), "debug", "desync", null);
		this.devConsole.AddConsoleVariable<ServerEnvironment>("network", "server_env", "Server Environment", "Which server stack to connect to.", new Func<ServerEnvironment>(this._Init_m__1C), new Action<ServerEnvironment>(this.changeServerEnv));
		this.devConsole.AddConsoleVariable<string>("network", "server_envCustom", "Server Environment", "Which server stack to connect to.", new Func<string>(this._Init_m__1D), new Action<string>(this.changeServerConnectIP));
		this.devConsole.AddCommand(new Action(this._Init_m__1E), "room", "quick_fill", null);
		this.devConsole.AddCommand(new Action(this._Init_m__1F), "room", "kris_fill", null);
		this.devConsole.AddCommand(new Action(this.forceTestingMode), "room", "testing", "Force the game mode to Testing.");
		this.devConsole.AddCommand(new Action(this.getMoney), "commerce", "get_money", "Cheat to get soft currency.");
		this.devConsole.AddCommand(new Action(this.forceSpectator), "network", "force_spectator", string.Empty);
		this.devConsole.AddCommand<int>(new Action<int>(this.advanceToFrame), "game", "advance_to", "Advance to frame N");
		this.devConsole.AddCommand<int>(new Action<int>(this.joinLobby), "network", "join", string.Empty);
		this.devConsole.AddCommand(new Action(this.onUpdateCustomLobby), "network", "updateLobby", null);
		this.devConsole.AddCommand(new Action(this.testP2PPing), "network", "ping", string.Empty);
		this.devConsole.AddCommand<int>(new Action<int>(this.advanceFrames), "game", "advance_frames", "Advance N frames.");
		this.devConsole.AddCommand(new Action(this._Init_m__20), "game", "advance_frames", "Advance N frames.");
		IDevConsole arg_7A5_0 = this.devConsole;
		string arg_7A5_1 = "physics";
		string arg_7A5_2 = "debug_raycasts";
		string arg_7A5_3 = "Debug Physics Raycasts";
		string arg_7A5_4 = "Show raycasts in physics debug view?";
		if (DeveloperUtilityConsoleCommands.__f__am_cache0 == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cache0 = new Func<bool>(DeveloperUtilityConsoleCommands._Init_m__21);
		}
		Func<bool> arg_7A5_5 = DeveloperUtilityConsoleCommands.__f__am_cache0;
		if (DeveloperUtilityConsoleCommands.__f__am_cache1 == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cache1 = new Action<bool>(DeveloperUtilityConsoleCommands._Init_m__22);
		}
		arg_7A5_0.AddConsoleVariable<bool>(arg_7A5_1, arg_7A5_2, arg_7A5_3, arg_7A5_4, arg_7A5_5, DeveloperUtilityConsoleCommands.__f__am_cache1);
		this.devConsole.AddConsoleVariable<LogLevel>("debug", "log_level", "Log Level", "Lowest log level that will be logged.  Everything below in priority will be ignored.", new Func<LogLevel>(this._Init_m__23), new Action<LogLevel>(this._Init_m__24));
		this.devConsole.AddConsoleVariable<LogLevel>("debug", "console_log_level", "Console Log Level", "Lowest log level the dev console will log.", new Func<LogLevel>(this._Init_m__25), new Action<LogLevel>(this._Init_m__26));
		IDevConsole arg_86C_0 = this.devConsole;
		string arg_86C_1 = "debug";
		string arg_86C_2 = "grid_width";
		string arg_86C_3 = "Debug Grid Width";
		string arg_86C_4 = "Width of the debug grid.";
		if (DeveloperUtilityConsoleCommands.__f__am_cache2 == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cache2 = new Func<int>(DeveloperUtilityConsoleCommands._Init_m__27);
		}
		Func<int> arg_86C_5 = DeveloperUtilityConsoleCommands.__f__am_cache2;
		if (DeveloperUtilityConsoleCommands.__f__am_cache3 == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cache3 = new Action<int>(DeveloperUtilityConsoleCommands._Init_m__28);
		}
		arg_86C_0.AddConsoleVariable<int>(arg_86C_1, arg_86C_2, arg_86C_3, arg_86C_4, arg_86C_5, DeveloperUtilityConsoleCommands.__f__am_cache3);
		IDevConsole arg_8C5_0 = this.devConsole;
		string arg_8C5_1 = "debug";
		string arg_8C5_2 = "grid_height";
		string arg_8C5_3 = "Debug Grid Height";
		string arg_8C5_4 = "Height of the debug grid.";
		if (DeveloperUtilityConsoleCommands.__f__am_cache4 == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cache4 = new Func<int>(DeveloperUtilityConsoleCommands._Init_m__29);
		}
		Func<int> arg_8C5_5 = DeveloperUtilityConsoleCommands.__f__am_cache4;
		if (DeveloperUtilityConsoleCommands.__f__am_cache5 == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cache5 = new Action<int>(DeveloperUtilityConsoleCommands._Init_m__2A);
		}
		arg_8C5_0.AddConsoleVariable<int>(arg_8C5_1, arg_8C5_2, arg_8C5_3, arg_8C5_4, arg_8C5_5, DeveloperUtilityConsoleCommands.__f__am_cache5);
		IDevConsole arg_91E_0 = this.devConsole;
		string arg_91E_1 = "debug";
		string arg_91E_2 = "draw_channel";
		string arg_91E_3 = "Active Draw Channels";
		string arg_91E_4 = "Flags enum of active debug draw channels.";
		if (DeveloperUtilityConsoleCommands.__f__am_cache6 == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cache6 = new Func<DebugDrawChannel>(DeveloperUtilityConsoleCommands._Init_m__2B);
		}
		Func<DebugDrawChannel> arg_91E_5 = DeveloperUtilityConsoleCommands.__f__am_cache6;
		if (DeveloperUtilityConsoleCommands.__f__am_cache7 == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cache7 = new Action<DebugDrawChannel>(DeveloperUtilityConsoleCommands._Init_m__2C);
		}
		arg_91E_0.AddConsoleVariable<DebugDrawChannel>(arg_91E_1, arg_91E_2, arg_91E_3, arg_91E_4, arg_91E_5, DeveloperUtilityConsoleCommands.__f__am_cache7);
		IDevConsole arg_977_0 = this.devConsole;
		string arg_977_1 = "debug";
		string arg_977_2 = "draw_physics";
		string arg_977_3 = "Draw physics";
		string arg_977_4 = "Draw physics arrows (need to activate gizmos in editor)";
		if (DeveloperUtilityConsoleCommands.__f__am_cache8 == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cache8 = new Func<bool>(DeveloperUtilityConsoleCommands._Init_m__2D);
		}
		Func<bool> arg_977_5 = DeveloperUtilityConsoleCommands.__f__am_cache8;
		if (DeveloperUtilityConsoleCommands.__f__am_cache9 == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cache9 = new Action<bool>(DeveloperUtilityConsoleCommands._Init_m__2E);
		}
		arg_977_0.AddConsoleVariable<bool>(arg_977_1, arg_977_2, arg_977_3, arg_977_4, arg_977_5, DeveloperUtilityConsoleCommands.__f__am_cache9);
		IDevConsole arg_9D0_0 = this.devConsole;
		string arg_9D0_1 = "debug";
		string arg_9D0_2 = "draw_hit_boxes";
		string arg_9D0_3 = "Draw hit boxes";
		string arg_9D0_4 = "Draw player hit boxes";
		if (DeveloperUtilityConsoleCommands.__f__am_cacheA == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cacheA = new Func<bool>(DeveloperUtilityConsoleCommands._Init_m__2F);
		}
		Func<bool> arg_9D0_5 = DeveloperUtilityConsoleCommands.__f__am_cacheA;
		if (DeveloperUtilityConsoleCommands.__f__am_cacheB == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cacheB = new Action<bool>(DeveloperUtilityConsoleCommands._Init_m__30);
		}
		arg_9D0_0.AddConsoleVariable<bool>(arg_9D0_1, arg_9D0_2, arg_9D0_3, arg_9D0_4, arg_9D0_5, DeveloperUtilityConsoleCommands.__f__am_cacheB);
		IDevConsole arg_A29_0 = this.devConsole;
		string arg_A29_1 = "debug";
		string arg_A29_2 = "draw_hurt_boxes";
		string arg_A29_3 = "Draw hurt boxes";
		string arg_A29_4 = "Draw player hurt boxes";
		if (DeveloperUtilityConsoleCommands.__f__am_cacheC == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cacheC = new Func<bool>(DeveloperUtilityConsoleCommands._Init_m__31);
		}
		Func<bool> arg_A29_5 = DeveloperUtilityConsoleCommands.__f__am_cacheC;
		if (DeveloperUtilityConsoleCommands.__f__am_cacheD == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cacheD = new Action<bool>(DeveloperUtilityConsoleCommands._Init_m__32);
		}
		arg_A29_0.AddConsoleVariable<bool>(arg_A29_1, arg_A29_2, arg_A29_3, arg_A29_4, arg_A29_5, DeveloperUtilityConsoleCommands.__f__am_cacheD);
		IDevConsole arg_A82_0 = this.devConsole;
		string arg_A82_1 = "debug";
		string arg_A82_2 = "draw_bounds";
		string arg_A82_3 = "Draw bounds";
		string arg_A82_4 = "Draw player environment bounds (need to activate gizmos in editor)";
		if (DeveloperUtilityConsoleCommands.__f__am_cacheE == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cacheE = new Func<bool>(DeveloperUtilityConsoleCommands._Init_m__33);
		}
		Func<bool> arg_A82_5 = DeveloperUtilityConsoleCommands.__f__am_cacheE;
		if (DeveloperUtilityConsoleCommands.__f__am_cacheF == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cacheF = new Action<bool>(DeveloperUtilityConsoleCommands._Init_m__34);
		}
		arg_A82_0.AddConsoleVariable<bool>(arg_A82_1, arg_A82_2, arg_A82_3, arg_A82_4, arg_A82_5, DeveloperUtilityConsoleCommands.__f__am_cacheF);
		IDevConsole arg_ADB_0 = this.devConsole;
		string arg_ADB_1 = "debug";
		string arg_ADB_2 = "draw_input";
		string arg_ADB_3 = "Draw input";
		string arg_ADB_4 = "Draw left axis input grid (need to activate gizmos in editor)";
		if (DeveloperUtilityConsoleCommands.__f__am_cache10 == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cache10 = new Func<bool>(DeveloperUtilityConsoleCommands._Init_m__35);
		}
		Func<bool> arg_ADB_5 = DeveloperUtilityConsoleCommands.__f__am_cache10;
		if (DeveloperUtilityConsoleCommands.__f__am_cache11 == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cache11 = new Action<bool>(DeveloperUtilityConsoleCommands._Init_m__36);
		}
		arg_ADB_0.AddConsoleVariable<bool>(arg_ADB_1, arg_ADB_2, arg_ADB_3, arg_ADB_4, arg_ADB_5, DeveloperUtilityConsoleCommands.__f__am_cache11);
		IDevConsole arg_B34_0 = this.devConsole;
		string arg_B34_1 = "debug";
		string arg_B34_2 = "draw_camera";
		string arg_B34_3 = "Draw camera";
		string arg_B34_4 = "Draw camera bounds and camera influence zones (need to activate gizmos in editor)";
		if (DeveloperUtilityConsoleCommands.__f__am_cache12 == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cache12 = new Func<bool>(DeveloperUtilityConsoleCommands._Init_m__37);
		}
		Func<bool> arg_B34_5 = DeveloperUtilityConsoleCommands.__f__am_cache12;
		if (DeveloperUtilityConsoleCommands.__f__am_cache13 == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cache13 = new Action<bool>(DeveloperUtilityConsoleCommands._Init_m__38);
		}
		arg_B34_0.AddConsoleVariable<bool>(arg_B34_1, arg_B34_2, arg_B34_3, arg_B34_4, arg_B34_5, DeveloperUtilityConsoleCommands.__f__am_cache13);
		IDevConsole arg_B8D_0 = this.devConsole;
		string arg_B8D_1 = "debug";
		string arg_B8D_2 = "draw_impact";
		string arg_B8D_3 = "Draw impact";
		string arg_B8D_4 = "Draw knockback vector (need to activate gizmos in editor)";
		if (DeveloperUtilityConsoleCommands.__f__am_cache14 == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cache14 = new Func<bool>(DeveloperUtilityConsoleCommands._Init_m__39);
		}
		Func<bool> arg_B8D_5 = DeveloperUtilityConsoleCommands.__f__am_cache14;
		if (DeveloperUtilityConsoleCommands.__f__am_cache15 == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cache15 = new Action<bool>(DeveloperUtilityConsoleCommands._Init_m__3A);
		}
		arg_B8D_0.AddConsoleVariable<bool>(arg_B8D_1, arg_B8D_2, arg_B8D_3, arg_B8D_4, arg_B8D_5, DeveloperUtilityConsoleCommands.__f__am_cache15);
		IDevConsole arg_BE6_0 = this.devConsole;
		string arg_BE6_1 = "debug";
		string arg_BE6_2 = "draw_grid";
		string arg_BE6_3 = "Draw grid";
		string arg_BE6_4 = "Draw position grid.";
		if (DeveloperUtilityConsoleCommands.__f__am_cache16 == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cache16 = new Func<bool>(DeveloperUtilityConsoleCommands._Init_m__3B);
		}
		Func<bool> arg_BE6_5 = DeveloperUtilityConsoleCommands.__f__am_cache16;
		if (DeveloperUtilityConsoleCommands.__f__am_cache17 == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cache17 = new Action<bool>(DeveloperUtilityConsoleCommands._Init_m__3C);
		}
		arg_BE6_0.AddConsoleVariable<bool>(arg_BE6_1, arg_BE6_2, arg_BE6_3, arg_BE6_4, arg_BE6_5, DeveloperUtilityConsoleCommands.__f__am_cache17);
		IDevConsole arg_C3F_0 = this.devConsole;
		string arg_C3F_1 = "demo";
		string arg_C3F_2 = "demo_mode";
		string arg_C3F_3 = "Demo Mode";
		string arg_C3F_4 = "Enables or disables demo mode, which controls access to some data and features.";
		if (DeveloperUtilityConsoleCommands.__f__am_cache18 == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cache18 = new Func<bool>(DeveloperUtilityConsoleCommands._Init_m__3D);
		}
		Func<bool> arg_C3F_5 = DeveloperUtilityConsoleCommands.__f__am_cache18;
		if (DeveloperUtilityConsoleCommands.__f__am_cache19 == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cache19 = new Action<bool>(DeveloperUtilityConsoleCommands._Init_m__3E);
		}
		arg_C3F_0.AddConsoleVariable<bool>(arg_C3F_1, arg_C3F_2, arg_C3F_3, arg_C3F_4, arg_C3F_5, DeveloperUtilityConsoleCommands.__f__am_cache19);
		IDevConsole arg_C98_0 = this.devConsole;
		string arg_C98_1 = "demo";
		string arg_C98_2 = "playtest_logging";
		string arg_C98_3 = "Playtest Logging";
		string arg_C98_4 = "Enables logging useful for debugging issues in playtests.";
		if (DeveloperUtilityConsoleCommands.__f__am_cache1A == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cache1A = new Func<bool>(DeveloperUtilityConsoleCommands._Init_m__3F);
		}
		Func<bool> arg_C98_5 = DeveloperUtilityConsoleCommands.__f__am_cache1A;
		if (DeveloperUtilityConsoleCommands.__f__am_cache1B == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cache1B = new Action<bool>(DeveloperUtilityConsoleCommands._Init_m__40);
		}
		arg_C98_0.AddConsoleVariable<bool>(arg_C98_1, arg_C98_2, arg_C98_3, arg_C98_4, arg_C98_5, DeveloperUtilityConsoleCommands.__f__am_cache1B);
		this.devConsole.AddConsoleVariable<float>("audio", "music_volume", "Set music volume", "Sets the music volume", new Func<float>(this._Init_m__41), new Action<float>(this._Init_m__42));
		this.devConsole.AddConsoleVariable<float>("audio", "sfx_volume", "Set sfx volume", "Sets the sfx volume", new Func<float>(this._Init_m__43), new Action<float>(this._Init_m__44));
		this.devConsole.AddPlayerConsoleVariable<float>("damage", "Damage", "Amount of damage on player.", new Func<PlayerNum, float>(this.getPlayerDamage), new Action<PlayerNum, float>(this.setPlayerDamage));
		this.devConsole.AddPlayerCommand<int>(new Action<PlayerNum, int>(this.impactPlayer), "impact", null);
		this.devConsole.AddPlayerConsoleVariable<string>("position", "Position", "Position of player.", new Func<PlayerNum, string>(this.getPlayerPosition), new Action<PlayerNum, string>(this.setPlayerPosition));
		IDevConsole arg_DBA_0 = this.devConsole;
		if (DeveloperUtilityConsoleCommands.__f__am_cache1C == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cache1C = new Action(DeveloperUtilityConsoleCommands._Init_m__45);
		}
		arg_DBA_0.AddCommand(DeveloperUtilityConsoleCommands.__f__am_cache1C, "game", "quit", null);
		this.devConsole.AddCommand(new Action(this._Init_m__46), "debug", "enter_character_profiler", null);
		this.devConsole.AddConsoleVariable<bool>("camera", "movement_disabled", "Disable camera movement", "Disables the automatic camera movement logic", new Func<bool>(this._Init_m__47), new Action<bool>(this._Init_m__48));
		this.devConsole.AddConsoleVariable<FrameSyncMode>("frames", "syncMode", "Frame Sync Mode", "Frame sync mode (technical)", new Func<FrameSyncMode>(this._Init_m__49), new Action<FrameSyncMode>(this._Init_m__4A));
	}

	private int getFramerate()
	{
		return this.framerateManager.overrideTargetFramerate;
	}

	private void setFramerate(int value)
	{
		this.framerateManager.overrideTargetFramerate = value;
	}

	private void changeServerEnv(ServerEnvironment value)
	{
		this.gameDataManager.ConfigData.networkSettings.overrideServerEnv = true;
		this.gameDataManager.ConfigData.networkSettings.overrideEnv = value;
		this.serverConnectionManager.Disconnect();
	}

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

	private float getPlayerDamage(PlayerNum playerNum)
	{
		return (float)this.gameController.currentGame.GetPlayerController(playerNum).Model.damage;
	}

	private void setPlayerDamage(PlayerNum playerNum, float damage)
	{
		this.gameController.currentGame.GetPlayerController(playerNum).Model.damage = (Fixed)((double)damage);
	}

	private string getPlayerPosition(PlayerNum playerNum)
	{
		return this.gameController.currentGame.GetPlayerController(playerNum).Physics.GetPosition().ToString();
	}

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

	private string getPlayerNametag()
	{
		return string.Empty;
	}

	private void setPlayerNametag1(string value)
	{
		this.setPlayerNametag(PlayerNum.Player1, value);
	}

	private void setPlayerNametag2(string value)
	{
		this.setPlayerNametag(PlayerNum.Player2, value);
	}

	private void setPlayerNametag3(string value)
	{
		this.setPlayerNametag(PlayerNum.Player3, value);
	}

	private void setPlayerNametag4(string value)
	{
		this.setPlayerNametag(PlayerNum.Player4, value);
	}

	private void setPlayerNametag5(string value)
	{
		this.setPlayerNametag(PlayerNum.Player5, value);
	}

	private void setPlayerNametag6(string value)
	{
		this.setPlayerNametag(PlayerNum.Player6, value);
	}

	private void setPlayerNametag(PlayerNum playerNum, string value)
	{
		this.signalBus.GetSignal<SetPlayerProfileNameSignal>().Dispatch(playerNum, value);
	}

	private void forcePlayerOn(PlayerNum playerNum, string characterName, PlayerType playerType)
	{
		List<CharacterDefinition> characters = this.environmentData.characters;
		CharacterID characterID = CharacterID.None;
		CharacterDefinition characterDefinition = null;
		bool flag = false;
		if (characterName != null)
		{
			foreach (CharacterDefinition current in characters)
			{
				if (characterName.EqualsIgnoreCase(current.characterName))
				{
					flag = true;
					characterName = current.characterName;
					characterID = current.characterID;
					characterDefinition = current;
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

	private void displayCharacterIds(PlayerNum playerNum)
	{
		string arg;
		switch (playerNum)
		{
		case PlayerNum.Player1:
			IL_31:
			arg = "1";
			goto IL_89;
		case PlayerNum.Player2:
			arg = "2";
			goto IL_89;
		case PlayerNum.Player3:
			arg = "3";
			goto IL_89;
		case PlayerNum.Player4:
			arg = "4";
			goto IL_89;
		case PlayerNum.Player5:
			arg = "5";
			goto IL_89;
		case PlayerNum.Player6:
			arg = "6";
			goto IL_89;
		case PlayerNum.Player7:
			arg = "7";
			goto IL_89;
		case PlayerNum.Player8:
			arg = "8";
			goto IL_89;
		}
		goto IL_31;
		IL_89:
		this.devConsole.PrintLn(string.Format("Available character ids: room.setp{0} <character id>", arg));
		foreach (CharacterDefinition current in this.environmentData.characters)
		{
			if (current.enabled && this.characterSelectModel.IsCharacterUnlocked(current.characterID))
			{
				this.devConsole.PrintLn(string.Format("  {0}", current.characterName));
			}
		}
	}

	private void getMoney()
	{
		IUserPurchaseEquipment arg_28_0 = this.userPurchase;
		ulong arg_28_1 = 1uL;
		CurrencyType arg_28_2 = CurrencyType.Soft;
		ulong arg_28_3 = 0uL;
		if (DeveloperUtilityConsoleCommands.__f__am_cache1D == null)
		{
			DeveloperUtilityConsoleCommands.__f__am_cache1D = new Action<UserPurchaseResult>(DeveloperUtilityConsoleCommands._getMoney_m__4B);
		}
		arg_28_0.PurchaseManual(arg_28_1, arg_28_2, arg_28_3, DeveloperUtilityConsoleCommands.__f__am_cache1D);
	}

	private void forceSpectator()
	{
	}

	private void forceTestingMode()
	{
		this.devConsole.PrintLn("Setting game rules to Testing mode... good luck with that bug you're stuck on!");
		this.events.Broadcast(new SetBattleSettingRequest(BattleSettingType.Mode, 6));
	}

	private void advanceFrames(int frames)
	{
		this.devConsole.PrintLn(string.Format("Advancing {0} frames.", frames));
		this.events.Broadcast(new DebugAdvanceFrameEvent(frames));
	}

	private void joinLobby(int lobbySteamID)
	{
	}

	private void onUpdateCustomLobby()
	{
		this.p2pServerMgr.OnUpdateCustomLobby();
	}

	private void testP2PPing()
	{
		this.pingManager.Ping();
	}

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

	private void _Init_m__0(string id)
	{
		this.forcePlayerOn(PlayerNum.Player1, id, PlayerType.Human);
	}

	private void _Init_m__1(string id)
	{
		this.forcePlayerOn(PlayerNum.Player2, id, PlayerType.Human);
	}

	private void _Init_m__2(string id)
	{
		this.forcePlayerOn(PlayerNum.Player3, id, PlayerType.Human);
	}

	private void _Init_m__3(string id)
	{
		this.forcePlayerOn(PlayerNum.Player4, id, PlayerType.Human);
	}

	private void _Init_m__4(string id)
	{
		this.forcePlayerOn(PlayerNum.Player5, id, PlayerType.Human);
	}

	private void _Init_m__5(string id)
	{
		this.forcePlayerOn(PlayerNum.Player6, id, PlayerType.Human);
	}

	private void _Init_m__6(string id)
	{
		this.forcePlayerOn(PlayerNum.Player1, id, PlayerType.CPU);
	}

	private void _Init_m__7(string id)
	{
		this.forcePlayerOn(PlayerNum.Player2, id, PlayerType.CPU);
	}

	private void _Init_m__8(string id)
	{
		this.forcePlayerOn(PlayerNum.Player3, id, PlayerType.CPU);
	}

	private void _Init_m__9(string id)
	{
		this.forcePlayerOn(PlayerNum.Player4, id, PlayerType.CPU);
	}

	private void _Init_m__A(string id)
	{
		this.forcePlayerOn(PlayerNum.Player5, id, PlayerType.CPU);
	}

	private void _Init_m__B(string id)
	{
		this.forcePlayerOn(PlayerNum.Player6, id, PlayerType.CPU);
	}

	private void _Init_m__C()
	{
		this.displayCharacterIds(PlayerNum.Player1);
	}

	private void _Init_m__D()
	{
		this.displayCharacterIds(PlayerNum.Player2);
	}

	private void _Init_m__E()
	{
		this.displayCharacterIds(PlayerNum.Player3);
	}

	private void _Init_m__F()
	{
		this.displayCharacterIds(PlayerNum.Player4);
	}

	private void _Init_m__10()
	{
		this.displayCharacterIds(PlayerNum.Player5);
	}

	private void _Init_m__11()
	{
		this.displayCharacterIds(PlayerNum.Player6);
	}

	private int _Init_m__12()
	{
		return this.gameDataManager.ConfigData.networkSettings.inputDelayFrames;
	}

	private void _Init_m__13(int value)
	{
		this.gameDataManager.ConfigData.networkSettings.inputDelayFrames = value;
	}

	private int _Init_m__14()
	{
		return this.debugLatencyManager.AddOutboundLatency;
	}

	private void _Init_m__15(int value)
	{
		this.debugLatencyManager.AddOutboundLatency = value;
	}

	private int _Init_m__16()
	{
		return this.debugLatencyManager.AddInboundLatency;
	}

	private void _Init_m__17(int value)
	{
		this.debugLatencyManager.AddInboundLatency = value;
	}

	private float _Init_m__18()
	{
		return this.gameDataManager.ConfigData.networkSettings.debugUdpDropRate;
	}

	private void _Init_m__19(float value)
	{
		this.gameDataManager.ConfigData.networkSettings.debugUdpDropRate = value;
	}

	private void _Init_m__1A()
	{
		this.events.Broadcast(new DebugThrowExceptionEvent());
	}

	private void _Init_m__1B()
	{
		this.events.Broadcast(new DebugDesyncEvent());
	}

	private ServerEnvironment _Init_m__1C()
	{
		return this.serverConnectionManager.ServerEnv;
	}

	private string _Init_m__1D()
	{
		return this.serverConnectionManager.EndPoint;
	}

	private void _Init_m__1E()
	{
		this.devConsole.ExecuteCommand("room.set_p2 kidd");
		this.devConsole.ExecuteCommand("room.set_p3 xana");
		this.devConsole.ExecuteCommand("room.set_p4 zhurong");
	}

	private void _Init_m__1F()
	{
		this.devConsole.ExecuteCommand("room.set_p1 ashani");
		this.devConsole.ExecuteCommand("room.set_p4_comp random");
	}

	private void _Init_m__20()
	{
		this.advanceFrames(1);
	}

	private static bool _Init_m__21()
	{
		return PhysicsWorld.EnableRaycastDebugging;
	}

	private static void _Init_m__22(bool value)
	{
		PhysicsWorld.EnableRaycastDebugging = value;
	}

	private LogLevel _Init_m__23()
	{
		return this.logger.LogLevel;
	}

	private void _Init_m__24(LogLevel value)
	{
		this.logger.LogLevel = value;
	}

	private LogLevel _Init_m__25()
	{
		return (this.logger as BroadcastingLogger).GetChildLogger<ConsoleLogger>().LogLevel;
	}

	private void _Init_m__26(LogLevel value)
	{
		(this.logger as BroadcastingLogger).GetChildLogger<ConsoleLogger>().LogLevel = value;
	}

	private static int _Init_m__27()
	{
		return DebugDraw.Instance.GridWidth;
	}

	private static void _Init_m__28(int value)
	{
		DebugDraw.Instance.GridWidth = value;
	}

	private static int _Init_m__29()
	{
		return DebugDraw.Instance.GridHeight;
	}

	private static void _Init_m__2A(int value)
	{
		DebugDraw.Instance.GridHeight = value;
	}

	private static DebugDrawChannel _Init_m__2B()
	{
		return DebugDraw.Instance.ActiveChannels;
	}

	private static void _Init_m__2C(DebugDrawChannel value)
	{
		DebugDraw.Instance.ActiveChannels = value;
	}

	private static bool _Init_m__2D()
	{
		return DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Physics);
	}

	private static void _Init_m__2E(bool value)
	{
		DebugDraw.Instance.SetChannelActive(DebugDrawChannel.Physics, value);
	}

	private static bool _Init_m__2F()
	{
		return DebugDraw.Instance.IsChannelActive(DebugDrawChannel.HitBoxes);
	}

	private static void _Init_m__30(bool value)
	{
		DebugDraw.Instance.SetChannelActive(DebugDrawChannel.HitBoxes, value);
	}

	private static bool _Init_m__31()
	{
		return DebugDraw.Instance.IsChannelActive(DebugDrawChannel.HurtBoxes);
	}

	private static void _Init_m__32(bool value)
	{
		DebugDraw.Instance.SetChannelActive(DebugDrawChannel.HurtBoxes, value);
	}

	private static bool _Init_m__33()
	{
		return DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Bounds);
	}

	private static void _Init_m__34(bool value)
	{
		DebugDraw.Instance.SetChannelActive(DebugDrawChannel.Bounds, value);
	}

	private static bool _Init_m__35()
	{
		return DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Input);
	}

	private static void _Init_m__36(bool value)
	{
		DebugDraw.Instance.SetChannelActive(DebugDrawChannel.Input, value);
	}

	private static bool _Init_m__37()
	{
		return DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Camera);
	}

	private static void _Init_m__38(bool value)
	{
		DebugDraw.Instance.SetChannelActive(DebugDrawChannel.Camera, value);
	}

	private static bool _Init_m__39()
	{
		return DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Impact);
	}

	private static void _Init_m__3A(bool value)
	{
		DebugDraw.Instance.SetChannelActive(DebugDrawChannel.Impact, value);
	}

	private static bool _Init_m__3B()
	{
		return DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Grid);
	}

	private static void _Init_m__3C(bool value)
	{
		DebugDraw.Instance.SetChannelActive(DebugDrawChannel.Grid, value);
	}

	private static bool _Init_m__3D()
	{
		return DemoSettings.DemoModeEnabled;
	}

	private static void _Init_m__3E(bool value)
	{
		DemoSettings.DemoModeEnabled = value;
	}

	private static bool _Init_m__3F()
	{
		return DemoSettings.PlaytestLoggingEnabled;
	}

	private static void _Init_m__40(bool value)
	{
		DemoSettings.PlaytestLoggingEnabled = value;
	}

	private float _Init_m__41()
	{
		return this.audioSettings.GetMusicVolume();
	}

	private void _Init_m__42(float value)
	{
		this.audioSettings.SetMusic(value);
	}

	private float _Init_m__43()
	{
		return this.audioSettings.GetSoundEffectsVolume();
	}

	private void _Init_m__44(float value)
	{
		this.audioSettings.SetSoundEffects(value);
	}

	private static void _Init_m__45()
	{
		Application.Quit();
	}

	private void _Init_m__46()
	{
		this.events.Broadcast(new LoadScreenCommand(ScreenType.NoUI, null, ScreenUpdateType.Next));
		SceneManager.LoadScene("CharacterProfilerScene");
	}

	private bool _Init_m__47()
	{
		return this.debugKeys.CameraMovementDisabled;
	}

	private void _Init_m__48(bool value)
	{
		this.debugKeys.CameraMovementDisabled = value;
	}

	private FrameSyncMode _Init_m__49()
	{
		return this.framerateManager.frameSyncMode;
	}

	private void _Init_m__4A(FrameSyncMode value)
	{
		this.framerateManager.frameSyncMode = value;
	}

	private static void _getMoney_m__4B(UserPurchaseResult result)
	{
		UnityEngine.Debug.Log("Soft currency result " + result);
	}
}
