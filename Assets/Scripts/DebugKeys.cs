// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DebugKeys
{
	private class RepeatData
	{
		public float interval;

		public float delay;

		public float timer;

		public bool isRepeating;
	}

	private int _targetPlayer;

	private bool debugHidePlayers;

	private bool debugHideUI;

	private bool debugControlsEnabled;

	public bool StaleMovesDisabled;

	public bool EnableReleaseReplays;

	public bool CameraMovementDisabled;

	private ProxyMono proxyObject;

	private Dictionary<KeyCode, string> devConsoleShortcuts = new Dictionary<KeyCode, string>(default(KeyCodeComparer));

	private Dictionary<ButtonPress, Action> controllerShortcuts = new Dictionary<ButtonPress, Action>(default(ButtonPressComparer));

	private HashSet<KeyCode> whitelistedOnlineKeys = new HashSet<KeyCode>(default(KeyCodeComparer));

	private Dictionary<KeyCode, DebugKeys.RepeatData> repeatData = new Dictionary<KeyCode, DebugKeys.RepeatData>(default(KeyCodeComparer));

	private static Action __f__am_cache0;

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
	public SaveReplay saveReplay
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
	public GameDataManager gameDataManager
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
	public UIManager uiManager
	{
		get;
		set;
	}

	[Inject]
	public DeveloperConfig devConfig
	{
		get;
		set;
	}

	[Inject]
	public IEvents eventSystem
	{
		get;
		set;
	}

	[Inject]
	public IKeyBindingManager keyBindingManager
	{
		get;
		set;
	}

	[Inject]
	public IReplaySystem replaySystem
	{
		get;
		set;
	}

	[Inject]
	public AudioManager audioManager
	{
		get;
		set;
	}

	private int targetPlayer
	{
		get
		{
			if (this.gameController.currentGame == null)
			{
				return 0;
			}
			return this._targetPlayer % this.gameController.currentGame.CharacterControllers.Count;
		}
		set
		{
			this._targetPlayer = value;
		}
	}

	public bool DebugControlsEnabled
	{
		get
		{
			return this.debugControlsEnabled;
		}
	}

	[PostConstruct]
	public void AutoInit()
	{
		this.proxyObject = ProxyMono.CreateNew("DebugKeys");
		ProxyMono expr_16 = this.proxyObject;
		expr_16.OnUpdate = (Action)Delegate.Combine(expr_16.OnUpdate, new Action(this.update));
	}

	public void Init()
	{
		this.whitelistedOnlineKeys.Clear();
		this.devConsoleShortcuts = new Dictionary<KeyCode, string>();
		this.targetPlayer = 1;
		this.AddKeyCommand(new Action(this.decreasePlaybackSpeed), KeyCode.Minus, "decrease_playback_speed", "debug", null, 0f, 0f, false);
		this.AddKeyCommand(new Action(this.increasePlaybackSpeed), KeyCode.Equals, "increase_playback_speed", "debug", null, 0f, 0f, false);
		this.AddKeyCommand(new Action(this.toggleDebugText), KeyCode.Alpha0, "toggle_debug_text", "debug", null, 0f, 0f, true);
		if (DebugKeys.__f__am_cache0 == null)
		{
			DebugKeys.__f__am_cache0 = new Action(DebugKeys._Init_m__0);
		}
		this.AddKeyCommand(DebugKeys.__f__am_cache0, KeyCode.F9, "disable_rendering", "debug", null, 0f, 0f, false);
		this.AddKeyToExistingCommand(KeyCode.F1, "debug.draw_physics toggle", 0f, 0f);
		this.AddKeyToExistingCommand(KeyCode.F2, "debug.draw_hit_boxes toggle", 0f, 0f);
		this.AddKeyToExistingCommand(KeyCode.F3, "debug.draw_hurt_boxes toggle", 0f, 0f);
		this.AddKeyToExistingCommand(KeyCode.F4, "debug.draw_bounds toggle", 0f, 0f);
		this.AddKeyToExistingCommand(KeyCode.F5, "debug.draw_input toggle", 0f, 0f);
		this.AddKeyToExistingCommand(KeyCode.F6, "debug.draw_camera toggle", 0f, 0f);
		this.AddKeyToExistingCommand(KeyCode.F7, "debug.draw_impact toggle", 0f, 0f);
		this.AddKeyCommand(new Action(this.toggleAIProfiles), KeyCode.Alpha5, "debug", "toggle_ai_profiles", null, 0f, 0f, false);
		this.AddKeyCommand(new Action(this.debugTogglePlayerVisibility), KeyCode.Alpha6, "toggle_player_visibility", "debug", null, 0f, 0f, false);
		this.AddKeyCommand(new Action(this._Init_m__1), KeyCode.PageDown, "debug", "advance_frame", "Advance frame while debug paused.", 0.05f, 0.25f, false);
		this.AddKeyToExistingCommand(KeyCode.F8, "room.kris_fill", 0f, 0f);
		this.AddKeyCommand(new Action(this.debugToggleUIVisibility), KeyCode.F10, "toggle_ui_visibility", "debug", "Toggles the UI visibility", 0f, 0f, false);
		this.AddKeyToExistingCommand(KeyCode.F11, "rollback.export", 0f, 0f);
		this.AddKeyToExistingCommand(KeyCode.F12, "rollback.load", 0f, 0f);
		this.AddKeyToExistingCommand(KeyCode.End, "room.set_p2 kidd", 0f, 0f);
		this.AddKeyToExistingCommand(KeyCode.Home, "room.testing", 0f, 0f);
		this.AddKeyToExistingCommand(KeyCode.Backslash, "player1.impact 90", 0f, 0f);
		this.AddKeyToExistingCommand(KeyCode.PageUp, "debug.enter_player_progression", 0f, 0f);
		this.devConsole.AddCommand(new Action(this.framePause), "debug", "frame_pause", null);
		this.devConsole.AddCommand(new Action(this.frameAdvance), "debug", "frame_advance", null);
		this.AddKeyCommand(new Action(this.printMemory), KeyCode.RightBracket, "debug", "memory_print", null, 0f, 0f, false);
		this.devConsole.AddCommand(new Action(this.toggleStaleMoves), "debug", "toggle_stale_moves", null);
		this.devConsole.AddCommand(new Action(this.toggleCrewBattleUI), "crewBattle", "toggle_ui", null);
		this.devConsole.AddAdminCommand(new Action(this.toggleDebugControls), "toggle_debug_controls", null);
		if (this.devConfig.autoEnableDebugControls)
		{
			this.toggleDebugControls();
		}
	}

	public bool HasControllerShortcut(ButtonPress press)
	{
		return this.controllerShortcuts.ContainsKey(press);
	}

	public Action FindControllerShortcut(ButtonPress press)
	{
		if (this.controllerShortcuts.ContainsKey(press))
		{
			return this.controllerShortcuts[press];
		}
		return null;
	}

	public void AddControllerShortcut(ButtonPress press, Action callback)
	{
		if (!this.controllerShortcuts.ContainsKey(press))
		{
			this.controllerShortcuts[press] = callback;
		}
		else
		{
			UnityEngine.Debug.LogError("Duplicate controller button binding " + press);
		}
	}

	public void RemoveControllerShortcut(ButtonPress press)
	{
		if (this.controllerShortcuts.ContainsKey(press))
		{
			this.controllerShortcuts.Remove(press);
		}
		else
		{
			UnityEngine.Debug.LogError("Cannot remove missing button binding " + press);
		}
	}

	public void AddKeyCommand(Action action, KeyCode keyCode, string category, string command, string help = null, float repeatInterval = 0f, float repeatDelay = 0f, bool allowedOnline = false)
	{
		if (allowedOnline)
		{
			this.whitelistedOnlineKeys.Add(keyCode);
		}
		this.devConsole.AddCommand(action, category, command, help);
		this.AddKeyToExistingCommand(keyCode, string.Format("{0}.{1}", category, command), repeatInterval, repeatDelay);
	}

	public void AddKeyToExistingCommand(KeyCode keyCode, string fullCommandString, float repeatInterval = 0f, float repeatDelay = 0f)
	{
		if (!this.devConsoleShortcuts.ContainsKey(keyCode))
		{
			this.devConsoleShortcuts.Add(keyCode, fullCommandString);
			if (repeatInterval > 0f || repeatDelay > 0f)
			{
				this.repeatData.Add(keyCode, new DebugKeys.RepeatData
				{
					interval = repeatInterval,
					delay = repeatDelay,
					timer = 0f,
					isRepeating = false
				});
			}
		}
	}

	public void RemoveKeyFromExisitingCommand(KeyCode keyCode)
	{
		if (this.devConsoleShortcuts.ContainsKey(keyCode))
		{
			this.devConsoleShortcuts.Remove(keyCode);
		}
	}

	public void SetFrameAdvanceOn(bool isOn)
	{
		bool flag = this.IsFrameAdvanceOn();
		if (isOn && !flag)
		{
			this.AddKeyToExistingCommand(KeyCode.N, "debug.frame_pause", 0f, 0f);
			this.AddKeyToExistingCommand(KeyCode.M, "debug.frame_advance", 0f, 0f);
			this.AddControllerShortcut(ButtonPress.TauntDown, new Action(this.framePause));
			this.AddControllerShortcut(ButtonPress.TauntRight, new Action(this.frameAdvance));
		}
		else if (!isOn && flag)
		{
			this.RemoveKeyFromExisitingCommand(KeyCode.N);
			this.RemoveKeyFromExisitingCommand(KeyCode.M);
			this.RemoveControllerShortcut(ButtonPress.TauntDown);
			this.RemoveControllerShortcut(ButtonPress.TauntRight);
		}
	}

	public bool IsFrameAdvanceOn()
	{
		return this.HasControllerShortcut(ButtonPress.TauntDown);
	}

	private void update()
	{
		if (this.uiManager.IsTextEntryMode || this.keyBindingManager.IsBindingKey)
		{
			return;
		}
		this.updateDebug();
		this.updateNondebug();
	}

	private void updateNondebug()
	{
		if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.Y) && !this.EnableReleaseReplays)
		{
			this.enableReleaseReplays();
		}
	}

	private void updateDebug()
	{
		if (this.uiManager.IsTextEntryMode || this.keyBindingManager.IsBindingKey)
		{
			return;
		}
		bool flag = this.gameController.currentGame != null && this.gameController.currentGame.IsNetworkGame;
		foreach (KeyValuePair<KeyCode, string> current in this.devConsoleShortcuts)
		{
			KeyCode key = current.Key;
			if (!flag || this.whitelistedOnlineKeys.Contains(key))
			{
				if (!this.repeatData.ContainsKey(key))
				{
					if (Input.GetKeyDown(key))
					{
						this.devConsole.ExecuteCommand(current.Value);
					}
				}
				else
				{
					DebugKeys.RepeatData repeatData = this.repeatData[key];
					if (repeatData.timer > 0f)
					{
						repeatData.timer -= Time.deltaTime;
					}
					if (Input.GetKey(key))
					{
						if (repeatData.timer <= 0f)
						{
							this.devConsole.ExecuteCommand(current.Value);
							if (repeatData.isRepeating)
							{
								repeatData.timer += repeatData.interval;
							}
							else
							{
								repeatData.timer += repeatData.delay;
								repeatData.isRepeating = true;
							}
						}
					}
					else
					{
						repeatData.timer = 0f;
						repeatData.isRepeating = false;
					}
				}
			}
		}
	}

	private void enableReleaseReplays()
	{
		this.audioManager.PlayMenuSound(SoundKey.generic_screenForwardTransition, 0f);
		this.EnableReleaseReplays = true;
		this.replaySystem.IsDirty = true;
		if (!this.gameController.MatchIsRunning)
		{
			this.events.Broadcast(new LoadScreenCommand(ScreenType.MainMenu, null, ScreenUpdateType.Other));
		}
	}

	private void toggleDebugControls()
	{
		this.debugControlsEnabled = !this.debugControlsEnabled;
		this.devConsole.PrintLn("Debug controls " + ((!this.debugControlsEnabled) ? "OFF" : "ON"));
	}

	private void changeServerEnv()
	{
	}

	private void toggleStaleMoves()
	{
		this.StaleMovesDisabled = !this.StaleMovesDisabled;
		this.devConsole.PrintLn("Stale moves are now " + ((!this.StaleMovesDisabled) ? "ENABLED" : "DISABLED"));
	}

	private void toggleCrewBattleUI()
	{
		if (this.gameDataManager.ConfigData.uiuxSettings.crewsGuiType == CrewBattleGuiType.VERSION2)
		{
			this.gameDataManager.ConfigData.uiuxSettings.crewsGuiType = CrewBattleGuiType.VERSION3;
			this.devConsole.PrintLn("Crew battle GUI 3");
		}
		else
		{
			this.gameDataManager.ConfigData.uiuxSettings.crewsGuiType = CrewBattleGuiType.VERSION2;
			this.devConsole.PrintLn("Crew battle GUI 2");
		}
	}

	private void toggleInvincibility()
	{
		if (this.gameController.currentGame == null)
		{
			return;
		}
		PlayerController playerController = this.gameController.currentGame.CharacterControllers[this.targetPlayer];
		bool isInvincible = playerController.IsInvincible;
		if (isInvincible)
		{
			playerController.Invincibility.BeginInvincibility(0);
		}
		else
		{
			playerController.Invincibility.BeginInvincibility(1000000);
		}
		this.devConsole.PrintLn("toggled Invinvibility for " + this.gameController.currentGame.CharacterControllers[this.targetPlayer].PlayerNum + ((!isInvincible) ? " on" : " off"));
	}

	private void clearDamage()
	{
		if (this.gameController.currentGame == null)
		{
			return;
		}
		this.devConsole.PrintLn("Cleared damage on " + this.gameController.currentGame.CharacterControllers[this.targetPlayer].PlayerNum);
		PlayerController playerController = this.gameController.currentGame.CharacterControllers[this.targetPlayer];
		playerController.ClearDamage();
	}

	private void cycleTargetPlayer()
	{
		if (this.gameController.currentGame == null)
		{
			return;
		}
		this.targetPlayer++;
		this.devConsole.PrintLn("Targetting player " + this.gameController.currentGame.CharacterControllers[this.targetPlayer].PlayerNum);
	}

	private void testAssistAttack()
	{
		this.devConsole.PrintLn("Test assist attack.");
		this.TestAssistAttack();
	}

	private void forceRollbackFourFrames()
	{
		this.devConsole.PrintLn("Forced Rollback 4 Frames.");
		this.events.Broadcast(new ForceRollbackCommand(-4, 0));
	}

	private void decreasePlaybackSpeed()
	{
		this.devConsole.PrintLn("Decreased playback speed.");
		this.events.Broadcast(new ChangePlaybackSpeedCommand(ChangePlaybackSpeedType.Decrease, 1f));
	}

	private void increasePlaybackSpeed()
	{
		this.devConsole.PrintLn("Increased playback speed.");
		this.events.Broadcast(new ChangePlaybackSpeedCommand(ChangePlaybackSpeedType.Increase, 1f));
	}

	private void toggleDebugText()
	{
		this.gameController.currentGame.UI.DebugTextEnabled = !this.gameController.currentGame.UI.DebugTextEnabled;
		this.devConsole.PrintLn(string.Format("Debug text {0}.", StringUtil.OnOrOff(this.gameController.currentGame.UI.DebugTextEnabled)));
	}

	private void framePause()
	{
		this.events.Broadcast(new ToggleFrameControlModeCommand());
	}

	private void frameAdvance()
	{
		this.events.Broadcast(new DebugAdvanceFrameEvent(1));
	}

	private void printMemory()
	{
		this.devConsole.PrintLn("MEMORY DUMP\n-------------------------------------");
		this.devConsole.PrintLn("\nEvents:\n" + ((Events)this.eventSystem).GenerateDebugString(true));
		if (this.gameController.currentGame != null)
		{
			this.devConsole.PrintLn("\nDynamic Objects:\n" + this.gameController.currentGame.DynamicObjects.GenerateDebugString(true));
			this.devConsole.PrintLn("\nObject Pools:\n" + this.gameController.currentGame.ObjectPools.GenerateDebugString(true));
		}
	}

	private void debugTogglePlayerVisibility()
	{
		this.debugHidePlayers = !this.debugHidePlayers;
		this.devConsole.PrintLn(string.Format("Hide players {0}.", StringUtil.OnOrOff(this.debugHidePlayers)));
		List<PlayerNum> list = new List<PlayerNum>();
		foreach (PlayerController current in this.gameController.currentGame.CharacterControllers)
		{
			list.Add(current.PlayerNum);
		}
		this.events.Broadcast(new TogglePlayerVisibilityCommand(list, !this.debugHidePlayers));
	}

	private void debugToggleUIVisibility()
	{
		this.debugHideUI = !this.debugHideUI;
		this.devConsole.PrintLn(string.Format("Hide UI {0}.", StringUtil.OnOrOff(this.debugHideUI)));
		this.events.Broadcast(new ToggleUIVisibilityCommand(!this.debugHideUI));
	}

	private void toggleAIProfiles()
	{
		this.signalBus.Dispatch(PlayerReference.TOGGLE_AI);
	}

	private void TestAssistAttack()
	{
		List<PlayerReference> playerReferences = this.gameController.currentGame.PlayerReferences;
		PlayerReference playerReference = playerReferences[3];
		this.events.Broadcast(new AttemptFriendlyAssistCommand(playerReference.PlayerNum, playerReference.Team, true));
	}

	private static void _Init_m__0()
	{
		SceneUtil.SetRenderersVisbile(false);
	}

	private void _Init_m__1()
	{
		this.events.Broadcast(new DebugAdvanceFrameEvent(1));
	}
}
