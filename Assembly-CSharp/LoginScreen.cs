using System;
using System.Collections.Generic;
using Auth;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Steamworks;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000994 RID: 2452
public class LoginScreen : GameScreen
{
	// Token: 0x17000FB9 RID: 4025
	// (get) Token: 0x060042B0 RID: 17072 RVA: 0x001289CF File Offset: 0x00126DCF
	// (set) Token: 0x060042B1 RID: 17073 RVA: 0x001289D7 File Offset: 0x00126DD7
	[Inject]
	public ILoginScreenAPI api { get; set; }

	// Token: 0x17000FBA RID: 4026
	// (get) Token: 0x060042B2 RID: 17074 RVA: 0x001289E0 File Offset: 0x00126DE0
	// (set) Token: 0x060042B3 RID: 17075 RVA: 0x001289E8 File Offset: 0x00126DE8
	[Inject]
	public ILoginValidator validator { get; set; }

	// Token: 0x17000FBB RID: 4027
	// (get) Token: 0x060042B4 RID: 17076 RVA: 0x001289F1 File Offset: 0x00126DF1
	// (set) Token: 0x060042B5 RID: 17077 RVA: 0x001289F9 File Offset: 0x00126DF9
	[Inject]
	public IWindowDisplay windowManager { get; set; }

	// Token: 0x17000FBC RID: 4028
	// (get) Token: 0x060042B6 RID: 17078 RVA: 0x00128A02 File Offset: 0x00126E02
	// (set) Token: 0x060042B7 RID: 17079 RVA: 0x00128A0A File Offset: 0x00126E0A
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x17000FBD RID: 4029
	// (get) Token: 0x060042B8 RID: 17080 RVA: 0x00128A13 File Offset: 0x00126E13
	// (set) Token: 0x060042B9 RID: 17081 RVA: 0x00128A1B File Offset: 0x00126E1B
	[Inject]
	public IDialogController dialog { get; set; }

	// Token: 0x17000FBE RID: 4030
	// (get) Token: 0x060042BA RID: 17082 RVA: 0x00128A24 File Offset: 0x00126E24
	// (set) Token: 0x060042BB RID: 17083 RVA: 0x00128A2C File Offset: 0x00126E2C
	[Inject]
	public ISoundFileManager soundFileManager { get; set; }

	// Token: 0x060042BC RID: 17084 RVA: 0x00128A38 File Offset: 0x00126E38
	[PostConstruct]
	public void Init()
	{
		this.QuitButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onQuitButtonClick);
		this.payload = new MainMenuPayload();
		this.TextAlertArea.SetActive(false);
		this.allPanels.Add(this.MainPanel);
		this.allPanels.Add(this.NewUserPanel);
		this.allPanels.Add(this.LoadingPanel);
		this.allPanels.Add(this.ErrorPanel);
		foreach (LoginScreenPanel obj in this.allPanels)
		{
			base.injector.Inject(obj);
		}
	}

	// Token: 0x060042BD RID: 17085 RVA: 0x00128B0C File Offset: 0x00126F0C
	protected override void postStartAndPayload()
	{
		base.postStartAndPayload();
		this.initPanel(this.getIntendedPanel());
		if (this.currentPanel != null)
		{
			this.currentPanel.InitSelection();
		}
		this.api.OnScreenShown();
	}

	// Token: 0x060042BE RID: 17086 RVA: 0x00128B47 File Offset: 0x00126F47
	private void onUpdate()
	{
	}

	// Token: 0x060042BF RID: 17087 RVA: 0x00128B4C File Offset: 0x00126F4C
	private void updateLoadingPanel()
	{
		this.LoadingPanel.UpdateState(!this.api.IsShowingAccountCreateResult);
		if (this.api.IsConnectingInProgress)
		{
			this.LoadingPanel.UpdateText(base.localization.GetText("ui.login.loading.connecting"));
		}
		else if (this.api.IsAccountCreateInProgress)
		{
			this.LoadingPanel.UpdateText(base.localization.GetText("ui.login.loading.creating"));
		}
		else
		{
			this.LoadingPanel.UpdateText(string.Empty);
		}
	}

	// Token: 0x060042C0 RID: 17088 RVA: 0x00128BE4 File Offset: 0x00126FE4
	private LoginScreenPanel getIntendedPanel()
	{
		if (!this.api.IsConnected)
		{
			if (this.api.IsConnectingInProgress)
			{
				return this.LoadingPanel;
			}
			return this.ErrorPanel;
		}
		else
		{
			if (this.api.IsDataSyncError)
			{
				return this.ErrorPanel;
			}
			if (this.api.IsAccountCreateInProgress)
			{
				return this.LoadingPanel;
			}
			if (this.api.HasAccount)
			{
				return this.LoadingPanel;
			}
			return this.NewUserPanel;
		}
	}

	// Token: 0x060042C1 RID: 17089 RVA: 0x00128C6C File Offset: 0x0012706C
	private void initPanel(LoginScreenPanel thePanel)
	{
		if (thePanel != null && !this.allPanels.Contains(thePanel))
		{
			throw new UnityException("Unmapped panel");
		}
		foreach (LoginScreenPanel loginScreenPanel in this.allPanels)
		{
			loginScreenPanel.gameObject.SetActive(false);
		}
		this.currentPanel = thePanel;
		if (this.currentPanel != null)
		{
			this.currentPanel.gameObject.SetActive(true);
		}
	}

	// Token: 0x060042C2 RID: 17090 RVA: 0x00128D20 File Offset: 0x00127120
	private void switchPanel(LoginScreenPanel newPanel)
	{
		if (newPanel != null && !this.allPanels.Contains(newPanel))
		{
			throw new UnityException("Unmapped panel");
		}
		if (this.currentPanel != newPanel)
		{
			this.onPanelAnimationComplete();
			base.lockInput();
			LoginScreenPanel loginScreenPanel = this.currentPanel;
			this.currentPanel = newPanel;
			loginScreenPanel.OnHide();
			if (this.currentPanel == null)
			{
				this.panelTween(loginScreenPanel, false, new Action(this.onPanelAnimationComplete));
			}
			else
			{
				this.currentPanel.IsCurrentPanel = true;
				this.panelTween(loginScreenPanel, false, null);
				this.panelTween(this.currentPanel, true, new Action(this.onPanelAnimationComplete));
			}
		}
	}

	// Token: 0x060042C3 RID: 17091 RVA: 0x00128DDE File Offset: 0x001271DE
	private void onPanelAnimationComplete()
	{
		base.unlockInput();
	}

	// Token: 0x060042C4 RID: 17092 RVA: 0x00128DE8 File Offset: 0x001271E8
	private void panelTween(LoginScreenPanel panel, bool active, Action callback = null)
	{
		this.killTweensOfPanel(panel);
		float endValue = (float)((!active) ? 0 : 1);
		float duration = 0.12f;
		if (active)
		{
			panel.gameObject.SetActive(true);
		}
		this.panelTweens[panel] = DOTween.To(() => panel.alpha, delegate(float x)
		{
			panel.alpha = x;
		}, endValue, duration).SetEase(Ease.Linear).OnComplete(delegate
		{
			this.killTweensOfPanel(panel);
			if (!active)
			{
				panel.gameObject.SetActive(false);
			}
			if (callback != null)
			{
				callback();
			}
		});
	}

	// Token: 0x060042C5 RID: 17093 RVA: 0x00128EA0 File Offset: 0x001272A0
	private void killTweensOfPanel(LoginScreenPanel panel)
	{
		if (this.panelTweens.ContainsKey(panel))
		{
			Tweener tweener = this.panelTweens[panel];
			TweenUtil.Destroy(ref tweener);
			this.panelTweens.Remove(panel);
		}
	}

	// Token: 0x060042C6 RID: 17094 RVA: 0x00128EDF File Offset: 0x001272DF
	private void enterGame()
	{
		base.events.Broadcast(new LoadScreenCommand(ScreenType.MainMenu, this.payload, ScreenUpdateType.Next));
	}

	// Token: 0x060042C7 RID: 17095 RVA: 0x00128EF9 File Offset: 0x001272F9
	private void onQuitButtonClick(CursorTargetButton button, PointerEventData eventData)
	{
		Application.Quit();
	}

	// Token: 0x060042C8 RID: 17096 RVA: 0x00128F00 File Offset: 0x00127300
	private void onServerLoginError(LoginError error)
	{
		if (error.noConnection)
		{
			this.dialog.ShowOneButtonDialog(base.localization.GetText("ui.login.ErrorCode.title"), base.localization.GetText("ui.login.NoAuthConnection.body"), base.localization.GetText("ui.login.ErrorCode.ok"), WindowTransition.STANDARD_FADE, true, this.soundFileManager.GetSoundAsAudioData(SoundKey.login_accountCreationError));
		}
		else
		{
			int num = 0;
			if (error.steamTicketError != EResult.k_EResultOK)
			{
				num = (int)(1000 + error.steamTicketError);
			}
			else if (error.steamLoginError != LoginSteamAckMsg.EResult.Accepted)
			{
				num = (int)(2000 + error.steamLoginError);
				if (error.steamLoginError == LoginSteamAckMsg.EResult.BadGwVersion)
				{
					Debug.LogError("CONNECTION ERROR - NetIOClient plugin is the wrong version.");
				}
				else if (error.steamLoginError == LoginSteamAckMsg.EResult.ClosedToPublic)
				{
					Debug.LogError("CONNECTION ERROR - Closed to public.");
				}
				else
				{
					Debug.LogError("CONNECTION ERROR - Unknown");
				}
			}
			else if (error.wavedashLoginError != LoginWavedashAckMsg.EResult.Accepted)
			{
				num = (int)(3000 + error.wavedashLoginError);
			}
			string title = base.localization.GetText("ui.login.ErrorCode.title");
			string text = base.localization.GetText("ui.login.ErrorCode.title_" + num);
			if (text != null)
			{
				title = text;
			}
			string body = base.localization.GetText("ui.login.ErrorCode.body", num.ToString());
			string text2 = base.localization.GetText("ui.login.ErrorCode.body_" + num);
			if (text2 != null)
			{
				body = text2;
			}
			this.dialog.ShowOneButtonDialog(title, body, base.localization.GetText("ui.login.ErrorCode.ok"), WindowTransition.STANDARD_FADE, true, this.soundFileManager.GetSoundAsAudioData(SoundKey.login_accountCreationError));
		}
	}

	// Token: 0x060042C9 RID: 17097 RVA: 0x001290A8 File Offset: 0x001274A8
	public override void OnDestroy()
	{
		this.onPanelAnimationComplete();
		foreach (LoginScreenPanel loginScreenPanel in this.allPanels)
		{
			this.killTweensOfPanel(loginScreenPanel);
			loginScreenPanel.OnDestroy();
		}
		if (base.signalBus != null)
		{
			base.signalBus.GetSignal<ServerLoginError>().RemoveListener(new Action<LoginError>(this.onServerLoginError));
		}
		base.OnDestroy();
	}

	// Token: 0x04002C91 RID: 11409
	public LoginScreenPanel MainPanel;

	// Token: 0x04002C92 RID: 11410
	public LoginScreenPanel NewUserPanel;

	// Token: 0x04002C93 RID: 11411
	public LoginLoadingPanel LoadingPanel;

	// Token: 0x04002C94 RID: 11412
	public LoginScreenPanel ErrorPanel;

	// Token: 0x04002C95 RID: 11413
	public LoginTextInputField LoginTextInputFieldPrefab;

	// Token: 0x04002C96 RID: 11414
	public GameObject KeyboardMouseNotifyBar;

	// Token: 0x04002C97 RID: 11415
	public GameObject TextAlertArea;

	// Token: 0x04002C98 RID: 11416
	public CursorTargetButton QuitButton;

	// Token: 0x04002C99 RID: 11417
	private List<LoginScreenPanel> allPanels = new List<LoginScreenPanel>();

	// Token: 0x04002C9A RID: 11418
	private LoginScreenPanel currentPanel;

	// Token: 0x04002C9B RID: 11419
	private Dictionary<LoginScreenPanel, Tweener> panelTweens = new Dictionary<LoginScreenPanel, Tweener>();
}
