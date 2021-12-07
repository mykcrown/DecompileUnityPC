using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002EC RID: 748
public class DebugKeys
{
	// Token: 0x170002AF RID: 687
	// (get) Token: 0x06000F82 RID: 3970 RVA: 0x0005DC63 File Offset: 0x0005C063
	// (set) Token: 0x06000F83 RID: 3971 RVA: 0x0005DC6B File Offset: 0x0005C06B
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x170002B0 RID: 688
	// (get) Token: 0x06000F84 RID: 3972 RVA: 0x0005DC74 File Offset: 0x0005C074
	// (set) Token: 0x06000F85 RID: 3973 RVA: 0x0005DC7C File Offset: 0x0005C07C
	[Inject]
	public ISignalBus signalBus { private get; set; }

	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x06000F86 RID: 3974 RVA: 0x0005DC85 File Offset: 0x0005C085
	// (set) Token: 0x06000F87 RID: 3975 RVA: 0x0005DC8D File Offset: 0x0005C08D
	[Inject]
	public SaveReplay saveReplay { get; set; }

	// Token: 0x170002B2 RID: 690
	// (get) Token: 0x06000F88 RID: 3976 RVA: 0x0005DC96 File Offset: 0x0005C096
	// (set) Token: 0x06000F89 RID: 3977 RVA: 0x0005DC9E File Offset: 0x0005C09E
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x170002B3 RID: 691
	// (get) Token: 0x06000F8A RID: 3978 RVA: 0x0005DCA7 File Offset: 0x0005C0A7
	// (set) Token: 0x06000F8B RID: 3979 RVA: 0x0005DCAF File Offset: 0x0005C0AF
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x170002B4 RID: 692
	// (get) Token: 0x06000F8C RID: 3980 RVA: 0x0005DCB8 File Offset: 0x0005C0B8
	// (set) Token: 0x06000F8D RID: 3981 RVA: 0x0005DCC0 File Offset: 0x0005C0C0
	[Inject]
	public IDevConsole devConsole { get; set; }

	// Token: 0x170002B5 RID: 693
	// (get) Token: 0x06000F8E RID: 3982 RVA: 0x0005DCC9 File Offset: 0x0005C0C9
	// (set) Token: 0x06000F8F RID: 3983 RVA: 0x0005DCD1 File Offset: 0x0005C0D1
	[Inject]
	public UIManager uiManager { get; set; }

	// Token: 0x170002B6 RID: 694
	// (get) Token: 0x06000F90 RID: 3984 RVA: 0x0005DCDA File Offset: 0x0005C0DA
	// (set) Token: 0x06000F91 RID: 3985 RVA: 0x0005DCE2 File Offset: 0x0005C0E2
	[Inject]
	public DeveloperConfig devConfig { get; set; }

	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x06000F92 RID: 3986 RVA: 0x0005DCEB File Offset: 0x0005C0EB
	// (set) Token: 0x06000F93 RID: 3987 RVA: 0x0005DCF3 File Offset: 0x0005C0F3
	[Inject]
	public IEvents eventSystem { get; set; }

	// Token: 0x170002B8 RID: 696
	// (get) Token: 0x06000F94 RID: 3988 RVA: 0x0005DCFC File Offset: 0x0005C0FC
	// (set) Token: 0x06000F95 RID: 3989 RVA: 0x0005DD04 File Offset: 0x0005C104
	[Inject]
	public IKeyBindingManager keyBindingManager { get; set; }

	// Token: 0x170002B9 RID: 697
	// (get) Token: 0x06000F96 RID: 3990 RVA: 0x0005DD0D File Offset: 0x0005C10D
	// (set) Token: 0x06000F97 RID: 3991 RVA: 0x0005DD15 File Offset: 0x0005C115
	[Inject]
	public IReplaySystem replaySystem { get; set; }

	// Token: 0x170002BA RID: 698
	// (get) Token: 0x06000F98 RID: 3992 RVA: 0x0005DD1E File Offset: 0x0005C11E
	// (set) Token: 0x06000F99 RID: 3993 RVA: 0x0005DD26 File Offset: 0x0005C126
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x170002BB RID: 699
	// (get) Token: 0x06000F9A RID: 3994 RVA: 0x0005DD2F File Offset: 0x0005C12F
	// (set) Token: 0x06000F9B RID: 3995 RVA: 0x0005DD65 File Offset: 0x0005C165
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

	// Token: 0x06000F9C RID: 3996 RVA: 0x0005DD6E File Offset: 0x0005C16E
	[PostConstruct]
	public void AutoInit()
	{
		this.proxyObject = ProxyMono.CreateNew("DebugKeys");
		ProxyMono proxyMono = this.proxyObject;
		proxyMono.OnUpdate = (Action)Delegate.Combine(proxyMono.OnUpdate, new Action(this.update));
	}

	// Token: 0x06000F9D RID: 3997 RVA: 0x0005DDA8 File Offset: 0x0005C1A8
	public void Init()
	{
		this.whitelistedOnlineKeys.Clear();
		this.devConsoleShortcuts = new Dictionary<KeyCode, string>();
		this.targetPlayer = 1;
		this.AddKeyCommand(new Action(this.decreasePlaybackSpeed), KeyCode.Minus, "decrease_playback_speed", "debug", null, 0f, 0f, false);
		this.AddKeyCommand(new Action(this.increasePlaybackSpeed), KeyCode.Equals, "increase_playback_speed", "debug", null, 0f, 0f, false);
		this.AddKeyCommand(new Action(this.toggleDebugText), KeyCode.Alpha0, "toggle_debug_text", "debug", null, 0f, 0f, true);
		this.AddKeyCommand(delegate
		{
			SceneUtil.SetRenderersVisbile(false);
		}, KeyCode.F9, "disable_rendering", "debug", null, 0f, 0f, false);
		this.AddKeyToExistingCommand(KeyCode.F1, "debug.draw_physics toggle", 0f, 0f);
		this.AddKeyToExistingCommand(KeyCode.F2, "debug.draw_hit_boxes toggle", 0f, 0f);
		this.AddKeyToExistingCommand(KeyCode.F3, "debug.draw_hurt_boxes toggle", 0f, 0f);
		this.AddKeyToExistingCommand(KeyCode.F4, "debug.draw_bounds toggle", 0f, 0f);
		this.AddKeyToExistingCommand(KeyCode.F5, "debug.draw_input toggle", 0f, 0f);
		this.AddKeyToExistingCommand(KeyCode.F6, "debug.draw_camera toggle", 0f, 0f);
		this.AddKeyToExistingCommand(KeyCode.F7, "debug.draw_impact toggle", 0f, 0f);
		this.AddKeyCommand(new Action(this.toggleAIProfiles), KeyCode.Alpha5, "debug", "toggle_ai_profiles", null, 0f, 0f, false);
		this.AddKeyCommand(new Action(this.debugTogglePlayerVisibility), KeyCode.Alpha6, "toggle_player_visibility", "debug", null, 0f, 0f, false);
		this.AddKeyCommand(delegate
		{
			this.events.Broadcast(new DebugAdvanceFrameEvent(1));
		}, KeyCode.PageDown, "debug", "advance_frame", "Advance frame while debug paused.", 0.05f, 0.25f, false);
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

	// Token: 0x06000F9E RID: 3998 RVA: 0x0005E192 File Offset: 0x0005C592
	public bool HasControllerShortcut(ButtonPress press)
	{
		return this.controllerShortcuts.ContainsKey(press);
	}

	// Token: 0x06000F9F RID: 3999 RVA: 0x0005E1A0 File Offset: 0x0005C5A0
	public Action FindControllerShortcut(ButtonPress press)
	{
		if (this.controllerShortcuts.ContainsKey(press))
		{
			return this.controllerShortcuts[press];
		}
		return null;
	}

	// Token: 0x06000FA0 RID: 4000 RVA: 0x0005E1C1 File Offset: 0x0005C5C1
	public void AddControllerShortcut(ButtonPress press, Action callback)
	{
		if (!this.controllerShortcuts.ContainsKey(press))
		{
			this.controllerShortcuts[press] = callback;
		}
		else
		{
			Debug.LogError("Duplicate controller button binding " + press);
		}
	}

	// Token: 0x06000FA1 RID: 4001 RVA: 0x0005E1FB File Offset: 0x0005C5FB
	public void RemoveControllerShortcut(ButtonPress press)
	{
		if (this.controllerShortcuts.ContainsKey(press))
		{
			this.controllerShortcuts.Remove(press);
		}
		else
		{
			Debug.LogError("Cannot remove missing button binding " + press);
		}
	}

	// Token: 0x06000FA2 RID: 4002 RVA: 0x0005E235 File Offset: 0x0005C635
	public void AddKeyCommand(Action action, KeyCode keyCode, string category, string command, string help = null, float repeatInterval = 0f, float repeatDelay = 0f, bool allowedOnline = false)
	{
		if (allowedOnline)
		{
			this.whitelistedOnlineKeys.Add(keyCode);
		}
		this.devConsole.AddCommand(action, category, command, help);
		this.AddKeyToExistingCommand(keyCode, string.Format("{0}.{1}", category, command), repeatInterval, repeatDelay);
	}

	// Token: 0x06000FA3 RID: 4003 RVA: 0x0005E274 File Offset: 0x0005C674
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

	// Token: 0x06000FA4 RID: 4004 RVA: 0x0005E2EA File Offset: 0x0005C6EA
	public void RemoveKeyFromExisitingCommand(KeyCode keyCode)
	{
		if (this.devConsoleShortcuts.ContainsKey(keyCode))
		{
			this.devConsoleShortcuts.Remove(keyCode);
		}
	}

	// Token: 0x06000FA5 RID: 4005 RVA: 0x0005E30C File Offset: 0x0005C70C
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

	// Token: 0x06000FA6 RID: 4006 RVA: 0x0005E3B3 File Offset: 0x0005C7B3
	public bool IsFrameAdvanceOn()
	{
		return this.HasControllerShortcut(ButtonPress.TauntDown);
	}

	// Token: 0x06000FA7 RID: 4007 RVA: 0x0005E3BD File Offset: 0x0005C7BD
	private void update()
	{
		if (this.uiManager.IsTextEntryMode || this.keyBindingManager.IsBindingKey)
		{
			return;
		}
		this.updateDebug();
		this.updateNondebug();
	}

	// Token: 0x06000FA8 RID: 4008 RVA: 0x0005E3EC File Offset: 0x0005C7EC
	private void updateNondebug()
	{
		if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.Y) && !this.EnableReleaseReplays)
		{
			this.enableReleaseReplays();
		}
	}

	// Token: 0x06000FA9 RID: 4009 RVA: 0x0005E454 File Offset: 0x0005C854
	private void updateDebug()
	{
		if (this.uiManager.IsTextEntryMode || this.keyBindingManager.IsBindingKey)
		{
			return;
		}
		bool flag = this.gameController.currentGame != null && this.gameController.currentGame.IsNetworkGame;
		foreach (KeyValuePair<KeyCode, string> keyValuePair in this.devConsoleShortcuts)
		{
			KeyCode key = keyValuePair.Key;
			if (!flag || this.whitelistedOnlineKeys.Contains(key))
			{
				if (!this.repeatData.ContainsKey(key))
				{
					if (Input.GetKeyDown(key))
					{
						this.devConsole.ExecuteCommand(keyValuePair.Value);
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
							this.devConsole.ExecuteCommand(keyValuePair.Value);
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

	// Token: 0x06000FAA RID: 4010 RVA: 0x0005E614 File Offset: 0x0005CA14
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

	// Token: 0x06000FAB RID: 4011 RVA: 0x0005E669 File Offset: 0x0005CA69
	private void toggleDebugControls()
	{
		this.debugControlsEnabled = !this.debugControlsEnabled;
		this.devConsole.PrintLn("Debug controls " + ((!this.debugControlsEnabled) ? "OFF" : "ON"));
	}

	// Token: 0x06000FAC RID: 4012 RVA: 0x0005E6A9 File Offset: 0x0005CAA9
	private void changeServerEnv()
	{
	}

	// Token: 0x06000FAD RID: 4013 RVA: 0x0005E6AB File Offset: 0x0005CAAB
	private void toggleStaleMoves()
	{
		this.StaleMovesDisabled = !this.StaleMovesDisabled;
		this.devConsole.PrintLn("Stale moves are now " + ((!this.StaleMovesDisabled) ? "ENABLED" : "DISABLED"));
	}

	// Token: 0x06000FAE RID: 4014 RVA: 0x0005E6EC File Offset: 0x0005CAEC
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

	// Token: 0x170002BC RID: 700
	// (get) Token: 0x06000FAF RID: 4015 RVA: 0x0005E765 File Offset: 0x0005CB65
	public bool DebugControlsEnabled
	{
		get
		{
			return this.debugControlsEnabled;
		}
	}

	// Token: 0x06000FB0 RID: 4016 RVA: 0x0005E770 File Offset: 0x0005CB70
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

	// Token: 0x06000FB1 RID: 4017 RVA: 0x0005E830 File Offset: 0x0005CC30
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

	// Token: 0x06000FB2 RID: 4018 RVA: 0x0005E8B0 File Offset: 0x0005CCB0
	private void cycleTargetPlayer()
	{
		if (this.gameController.currentGame == null)
		{
			return;
		}
		this.targetPlayer++;
		this.devConsole.PrintLn("Targetting player " + this.gameController.currentGame.CharacterControllers[this.targetPlayer].PlayerNum);
	}

	// Token: 0x06000FB3 RID: 4019 RVA: 0x0005E91C File Offset: 0x0005CD1C
	private void testAssistAttack()
	{
		this.devConsole.PrintLn("Test assist attack.");
		this.TestAssistAttack();
	}

	// Token: 0x06000FB4 RID: 4020 RVA: 0x0005E934 File Offset: 0x0005CD34
	private void forceRollbackFourFrames()
	{
		this.devConsole.PrintLn("Forced Rollback 4 Frames.");
		this.events.Broadcast(new ForceRollbackCommand(-4, 0));
	}

	// Token: 0x06000FB5 RID: 4021 RVA: 0x0005E959 File Offset: 0x0005CD59
	private void decreasePlaybackSpeed()
	{
		this.devConsole.PrintLn("Decreased playback speed.");
		this.events.Broadcast(new ChangePlaybackSpeedCommand(ChangePlaybackSpeedType.Decrease, 1f));
	}

	// Token: 0x06000FB6 RID: 4022 RVA: 0x0005E981 File Offset: 0x0005CD81
	private void increasePlaybackSpeed()
	{
		this.devConsole.PrintLn("Increased playback speed.");
		this.events.Broadcast(new ChangePlaybackSpeedCommand(ChangePlaybackSpeedType.Increase, 1f));
	}

	// Token: 0x06000FB7 RID: 4023 RVA: 0x0005E9AC File Offset: 0x0005CDAC
	private void toggleDebugText()
	{
		this.gameController.currentGame.UI.DebugTextEnabled = !this.gameController.currentGame.UI.DebugTextEnabled;
		this.devConsole.PrintLn(string.Format("Debug text {0}.", StringUtil.OnOrOff(this.gameController.currentGame.UI.DebugTextEnabled)));
	}

	// Token: 0x06000FB8 RID: 4024 RVA: 0x0005EA15 File Offset: 0x0005CE15
	private void framePause()
	{
		this.events.Broadcast(new ToggleFrameControlModeCommand());
	}

	// Token: 0x06000FB9 RID: 4025 RVA: 0x0005EA27 File Offset: 0x0005CE27
	private void frameAdvance()
	{
		this.events.Broadcast(new DebugAdvanceFrameEvent(1));
	}

	// Token: 0x06000FBA RID: 4026 RVA: 0x0005EA3C File Offset: 0x0005CE3C
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

	// Token: 0x06000FBB RID: 4027 RVA: 0x0005EAEC File Offset: 0x0005CEEC
	private void debugTogglePlayerVisibility()
	{
		this.debugHidePlayers = !this.debugHidePlayers;
		this.devConsole.PrintLn(string.Format("Hide players {0}.", StringUtil.OnOrOff(this.debugHidePlayers)));
		List<PlayerNum> list = new List<PlayerNum>();
		foreach (PlayerController playerController in this.gameController.currentGame.CharacterControllers)
		{
			list.Add(playerController.PlayerNum);
		}
		this.events.Broadcast(new TogglePlayerVisibilityCommand(list, !this.debugHidePlayers));
	}

	// Token: 0x06000FBC RID: 4028 RVA: 0x0005EBA8 File Offset: 0x0005CFA8
	private void debugToggleUIVisibility()
	{
		this.debugHideUI = !this.debugHideUI;
		this.devConsole.PrintLn(string.Format("Hide UI {0}.", StringUtil.OnOrOff(this.debugHideUI)));
		this.events.Broadcast(new ToggleUIVisibilityCommand(!this.debugHideUI));
	}

	// Token: 0x06000FBD RID: 4029 RVA: 0x0005EBFD File Offset: 0x0005CFFD
	private void toggleAIProfiles()
	{
		this.signalBus.Dispatch(PlayerReference.TOGGLE_AI);
	}

	// Token: 0x06000FBE RID: 4030 RVA: 0x0005EC10 File Offset: 0x0005D010
	private void TestAssistAttack()
	{
		List<PlayerReference> playerReferences = this.gameController.currentGame.PlayerReferences;
		PlayerReference playerReference = playerReferences[3];
		this.events.Broadcast(new AttemptFriendlyAssistCommand(playerReference.PlayerNum, playerReference.Team, true));
	}

	// Token: 0x04000A3C RID: 2620
	private int _targetPlayer;

	// Token: 0x04000A3D RID: 2621
	private bool debugHidePlayers;

	// Token: 0x04000A3E RID: 2622
	private bool debugHideUI;

	// Token: 0x04000A3F RID: 2623
	private bool debugControlsEnabled;

	// Token: 0x04000A40 RID: 2624
	public bool StaleMovesDisabled;

	// Token: 0x04000A41 RID: 2625
	public bool EnableReleaseReplays;

	// Token: 0x04000A42 RID: 2626
	public bool CameraMovementDisabled;

	// Token: 0x04000A43 RID: 2627
	private ProxyMono proxyObject;

	// Token: 0x04000A44 RID: 2628
	private Dictionary<KeyCode, string> devConsoleShortcuts = new Dictionary<KeyCode, string>(default(KeyCodeComparer));

	// Token: 0x04000A45 RID: 2629
	private Dictionary<ButtonPress, Action> controllerShortcuts = new Dictionary<ButtonPress, Action>(default(ButtonPressComparer));

	// Token: 0x04000A46 RID: 2630
	private HashSet<KeyCode> whitelistedOnlineKeys = new HashSet<KeyCode>(default(KeyCodeComparer));

	// Token: 0x04000A47 RID: 2631
	private Dictionary<KeyCode, DebugKeys.RepeatData> repeatData = new Dictionary<KeyCode, DebugKeys.RepeatData>(default(KeyCodeComparer));

	// Token: 0x020002ED RID: 749
	private class RepeatData
	{
		// Token: 0x04000A49 RID: 2633
		public float interval;

		// Token: 0x04000A4A RID: 2634
		public float delay;

		// Token: 0x04000A4B RID: 2635
		public float timer;

		// Token: 0x04000A4C RID: 2636
		public bool isRepeating;
	}
}
