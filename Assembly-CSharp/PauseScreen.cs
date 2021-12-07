using System;
using AI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009BB RID: 2491
public class PauseScreen : GameScreen
{
	// Token: 0x1700106B RID: 4203
	// (get) Token: 0x06004549 RID: 17737 RVA: 0x001305B6 File Offset: 0x0012E9B6
	// (set) Token: 0x0600454A RID: 17738 RVA: 0x001305BE File Offset: 0x0012E9BE
	[Inject]
	public IAIManager aiManager { get; set; }

	// Token: 0x1700106C RID: 4204
	// (get) Token: 0x0600454B RID: 17739 RVA: 0x001305C7 File Offset: 0x0012E9C7
	// (set) Token: 0x0600454C RID: 17740 RVA: 0x001305CF File Offset: 0x0012E9CF
	[Inject]
	public DebugKeys debugKeys { get; set; }

	// Token: 0x1700106D RID: 4205
	// (get) Token: 0x0600454D RID: 17741 RVA: 0x001305D8 File Offset: 0x0012E9D8
	// (set) Token: 0x0600454E RID: 17742 RVA: 0x001305E0 File Offset: 0x0012E9E0
	public GameObject PreviousSelected { get; set; }

	// Token: 0x1700106E RID: 4206
	// (get) Token: 0x0600454F RID: 17743 RVA: 0x001305E9 File Offset: 0x0012E9E9
	public bool IsTrainingUI
	{
		get
		{
			return base.gameController.currentGame.Mode == GameMode.Training && !base.gameController.currentGame.GameConfig.isReplay;
		}
	}

	// Token: 0x06004550 RID: 17744 RVA: 0x0013061C File Offset: 0x0012EA1C
	public override void Awake()
	{
		base.Awake();
		this.menuController = base.createMenuController();
		this.menuController.AddButton(this.ResumeButton, new Action(this.resume));
		this.menuController.AddButton(this.QuitButton, new Action(this.quit));
		this.OptionsButton.gameObject.SetActive(false);
		this.CharacterButton.gameObject.SetActive(false);
		this.StageButton.gameObject.SetActive(false);
		if (base.gameController.currentGame.Mode == GameMode.Training || base.gameController.currentGame.GameConfig.isReplay)
		{
			this.menuController.AddButton(this.HitBoxButton, new Action(this.toggleHitBox));
			this.menuController.AddButton(this.FrameAdvanceButton, new Action(this.toggleFrameAdvance));
		}
		else
		{
			this.HitBoxButton.gameObject.SetActive(false);
			this.FrameAdvanceButton.gameObject.SetActive(false);
		}
		if (this.IsTrainingUI && this.aiManager.IsAnyAIActive())
		{
			this.menuController.AddButton(this.AIButton, new Action(this.toggleAI));
		}
		else
		{
			this.AIButton.gameObject.SetActive(false);
		}
		this.menuController.SetSelectorImage(this.SelectorImage);
		this.menuController.ForbidNullSelection = true;
		this.menuController.Initialize();
		this.setPausedByName();
	}

	// Token: 0x06004551 RID: 17745 RVA: 0x001307B5 File Offset: 0x0012EBB5
	public override void Start()
	{
		base.Start();
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)base.transform);
		this.List.Redraw();
		this.onUpdate();
		base.initMenuSelectionSystem();
		this.defaultSelection();
	}

	// Token: 0x06004552 RID: 17746 RVA: 0x001307EC File Offset: 0x0012EBEC
	private void onUpdate()
	{
		bool toggle = !this.aiManager.IsAnyPassiveAIActive();
		this.AIToggle.SetToggle(toggle);
		bool toggle2 = DebugDraw.Instance.IsChannelActive(DebugDrawChannel.HitBoxes);
		this.HitboxToggle.SetToggle(toggle2);
		bool toggle3 = this.debugKeys.IsFrameAdvanceOn();
		this.FrameAdvanceToggle.SetToggle(toggle3);
	}

	// Token: 0x06004553 RID: 17747 RVA: 0x00130844 File Offset: 0x0012EC44
	protected override void Update()
	{
		base.Update();
		if (!this.firstAlignComplete)
		{
			this.firstAlignComplete = true;
			MenuItemButton currentSelection = this.menuController.CurrentSelection;
			this.menuController.RemoveSelection();
			this.menuController.AutoSelect(currentSelection);
		}
	}

	// Token: 0x06004554 RID: 17748 RVA: 0x0013088C File Offset: 0x0012EC8C
	private void setPausedByName()
	{
		PlayerController playerController = base.gameController.currentGame.GetPlayerController(base.gameController.currentGame.PausedPlayer);
		string playerName = PlayerUtil.GetPlayerName(base.localization, playerController);
		this.PausedByText.text = base.localization.GetText("ui.paused.pausedBy", playerName);
		this.PausedByText.gameObject.SetActive(playerController != null);
	}

	// Token: 0x06004555 RID: 17749 RVA: 0x001308FA File Offset: 0x0012ECFA
	protected override void handleMenuSelectionOnButtonMode()
	{
		if (this.menuController.CurrentSelection != null)
		{
			this.menuController.AutoSelect(this.menuController.CurrentSelection);
		}
		else
		{
			this.defaultSelection();
		}
	}

	// Token: 0x06004556 RID: 17750 RVA: 0x00130933 File Offset: 0x0012ED33
	private void defaultSelection()
	{
		if (this.FirstSelected == null)
		{
			return;
		}
		if (this.menuController.CurrentSelection == null)
		{
			this.menuController.AutoSelect(this.FirstSelected.GetComponent<MenuItemButton>());
		}
	}

	// Token: 0x06004557 RID: 17751 RVA: 0x00130973 File Offset: 0x0012ED73
	public void Initialize(PlayerNum pausedBy)
	{
		this.menuController.AutoSelect(this.FirstSelected.GetComponent<MenuItemButton>());
	}

	// Token: 0x06004558 RID: 17752 RVA: 0x0013098C File Offset: 0x0012ED8C
	private void quit()
	{
		if (base.battleServerAPI.IsConnected)
		{
			base.signalBus.GetSignal<QuitGameSignal>().Dispatch();
			base.signalBus.GetSignal<UpdatePauseScreenOnline>().Dispatch(false);
		}
		else
		{
			if (base.gameController.currentGame.Mode == GameMode.Training)
			{
				base.gameController.currentGame.ForfeitGame(ScreenType.CharacterSelect);
			}
			else
			{
				base.gameController.currentGame.ForfeitGame(ScreenType.VictoryGUI);
			}
			this.resume();
		}
	}

	// Token: 0x06004559 RID: 17753 RVA: 0x00130A12 File Offset: 0x0012EE12
	private void gotoCharacterSelect()
	{
		base.gameController.currentGame.ForfeitGame(ScreenType.CharacterSelect);
		this.resume();
	}

	// Token: 0x0600455A RID: 17754 RVA: 0x00130A2B File Offset: 0x0012EE2B
	private void gotoStageSelect()
	{
		base.gameController.currentGame.ForfeitGame(ScreenType.SelectStage);
		this.resume();
	}

	// Token: 0x0600455B RID: 17755 RVA: 0x00130A44 File Offset: 0x0012EE44
	private void toggleAI()
	{
		base.signalBus.Dispatch(PlayerReference.TOGGLE_AI);
		this.onUpdate();
	}

	// Token: 0x0600455C RID: 17756 RVA: 0x00130A5C File Offset: 0x0012EE5C
	private void toggleHitBox()
	{
		bool flag = DebugDraw.Instance.IsChannelActive(DebugDrawChannel.HitBoxes);
		DebugDraw.Instance.SetChannelActive(DebugDrawChannel.HitBoxes, !flag);
		DebugDraw.Instance.SetChannelActive(DebugDrawChannel.HurtBoxes, !flag);
		this.onUpdate();
	}

	// Token: 0x0600455D RID: 17757 RVA: 0x00130A9C File Offset: 0x0012EE9C
	private void toggleFrameAdvance()
	{
		bool flag = this.debugKeys.IsFrameAdvanceOn();
		this.debugKeys.SetFrameAdvanceOn(!flag);
		this.onUpdate();
	}

	// Token: 0x0600455E RID: 17758 RVA: 0x00130ACC File Offset: 0x0012EECC
	private void resume()
	{
		if (base.battleServerAPI.IsConnected)
		{
			base.signalBus.GetSignal<UpdatePauseScreenOnline>().Dispatch(false);
		}
		else
		{
			PlayerInputPort pausedPort = base.gameController.currentGame.PausedPort;
			if (base.gameController.currentGame.IsPaused)
			{
				if (pausedPort != null)
				{
					base.gameController.currentGame.TogglePaused(pausedPort);
				}
				else
				{
					base.gameController.currentGame.TogglePaused(null);
				}
			}
		}
	}

	// Token: 0x0600455F RID: 17759 RVA: 0x00130B58 File Offset: 0x0012EF58
	private void options()
	{
		Debug.Log("Open options menu");
	}

	// Token: 0x06004560 RID: 17760 RVA: 0x00130B64 File Offset: 0x0012EF64
	protected override void onMouseModeUpdate()
	{
		base.onMouseModeUpdate();
		if ((base.uiManager.CurrentInputModule as UIInputModule).IsMouseMode)
		{
			this.MouseMode.SetActive(true);
			this.ControllerMode.SetActive(false);
		}
		else
		{
			this.MouseMode.SetActive(false);
			this.ControllerMode.SetActive(true);
		}
	}

	// Token: 0x06004561 RID: 17761 RVA: 0x00130BC8 File Offset: 0x0012EFC8
	public override void OnDestroy()
	{
		if (this.PreviousSelected != null)
		{
			base.selectionManager.Select(this.PreviousSelected);
			this.PreviousSelected = null;
		}
		if (this.menuController != null)
		{
			this.menuController.OnDestroy();
		}
		base.OnDestroy();
	}

	// Token: 0x04002E0F RID: 11791
	public GameObject SelectorImage;

	// Token: 0x04002E10 RID: 11792
	public MenuItemButton ResumeButton;

	// Token: 0x04002E11 RID: 11793
	public MenuItemButton OptionsButton;

	// Token: 0x04002E12 RID: 11794
	public MenuItemButton QuitButton;

	// Token: 0x04002E13 RID: 11795
	public MenuItemButton StageButton;

	// Token: 0x04002E14 RID: 11796
	public MenuItemButton CharacterButton;

	// Token: 0x04002E15 RID: 11797
	public MenuItemButton AIButton;

	// Token: 0x04002E16 RID: 11798
	public MenuItemButton HitBoxButton;

	// Token: 0x04002E17 RID: 11799
	public MenuItemButton FrameAdvanceButton;

	// Token: 0x04002E18 RID: 11800
	public OptionToggle AIToggle;

	// Token: 0x04002E19 RID: 11801
	public OptionToggle HitboxToggle;

	// Token: 0x04002E1A RID: 11802
	public OptionToggle FrameAdvanceToggle;

	// Token: 0x04002E1B RID: 11803
	public TextMeshProUGUI PausedByText;

	// Token: 0x04002E1C RID: 11804
	public VerticalLayoutGroup List;

	// Token: 0x04002E1D RID: 11805
	public GameObject MouseMode;

	// Token: 0x04002E1E RID: 11806
	public GameObject ControllerMode;

	// Token: 0x04002E1F RID: 11807
	private MenuItemList menuController;

	// Token: 0x04002E20 RID: 11808
	private bool firstAlignComplete;
}
