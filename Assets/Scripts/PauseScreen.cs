// Decompile from assembly: Assembly-CSharp.dll

using AI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : GameScreen
{
	public GameObject SelectorImage;

	public MenuItemButton ResumeButton;

	public MenuItemButton OptionsButton;

	public MenuItemButton QuitButton;

	public MenuItemButton StageButton;

	public MenuItemButton CharacterButton;

	public MenuItemButton AIButton;

	public MenuItemButton HitBoxButton;

	public MenuItemButton FrameAdvanceButton;

	public OptionToggle AIToggle;

	public OptionToggle HitboxToggle;

	public OptionToggle FrameAdvanceToggle;

	public TextMeshProUGUI PausedByText;

	public VerticalLayoutGroup List;

	public GameObject MouseMode;

	public GameObject ControllerMode;

	private MenuItemList menuController;

	private bool firstAlignComplete;

	[Inject]
	public IAIManager aiManager
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

	public GameObject PreviousSelected
	{
		get;
		set;
	}

	public bool IsTrainingUI
	{
		get
		{
			return base.gameController.currentGame.Mode == GameMode.Training && !base.gameController.currentGame.GameConfig.isReplay;
		}
	}

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

	public override void Start()
	{
		base.Start();
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)base.transform);
		this.List.Redraw();
		this.onUpdate();
		base.initMenuSelectionSystem();
		this.defaultSelection();
	}

	private void onUpdate()
	{
		bool toggle = !this.aiManager.IsAnyPassiveAIActive();
		this.AIToggle.SetToggle(toggle);
		bool toggle2 = DebugDraw.Instance.IsChannelActive(DebugDrawChannel.HitBoxes);
		this.HitboxToggle.SetToggle(toggle2);
		bool toggle3 = this.debugKeys.IsFrameAdvanceOn();
		this.FrameAdvanceToggle.SetToggle(toggle3);
	}

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

	private void setPausedByName()
	{
		PlayerController playerController = base.gameController.currentGame.GetPlayerController(base.gameController.currentGame.PausedPlayer);
		string playerName = PlayerUtil.GetPlayerName(base.localization, playerController);
		this.PausedByText.text = base.localization.GetText("ui.paused.pausedBy", playerName);
		this.PausedByText.gameObject.SetActive(playerController != null);
	}

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

	public void Initialize(PlayerNum pausedBy)
	{
		this.menuController.AutoSelect(this.FirstSelected.GetComponent<MenuItemButton>());
	}

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

	private void gotoCharacterSelect()
	{
		base.gameController.currentGame.ForfeitGame(ScreenType.CharacterSelect);
		this.resume();
	}

	private void gotoStageSelect()
	{
		base.gameController.currentGame.ForfeitGame(ScreenType.SelectStage);
		this.resume();
	}

	private void toggleAI()
	{
		base.signalBus.Dispatch(PlayerReference.TOGGLE_AI);
		this.onUpdate();
	}

	private void toggleHitBox()
	{
		bool flag = DebugDraw.Instance.IsChannelActive(DebugDrawChannel.HitBoxes);
		DebugDraw.Instance.SetChannelActive(DebugDrawChannel.HitBoxes, !flag);
		DebugDraw.Instance.SetChannelActive(DebugDrawChannel.HurtBoxes, !flag);
		this.onUpdate();
	}

	private void toggleFrameAdvance()
	{
		bool flag = this.debugKeys.IsFrameAdvanceOn();
		this.debugKeys.SetFrameAdvanceOn(!flag);
		this.onUpdate();
	}

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

	private void options()
	{
		UnityEngine.Debug.Log("Open options menu");
	}

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
}
