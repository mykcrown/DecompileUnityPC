// Decompile from assembly: Assembly-CSharp.dll

using Auth;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class LoginScreen : GameScreen
{
	private sealed class _panelTween_c__AnonStorey0
	{
		internal LoginScreenPanel panel;

		internal bool active;

		internal Action callback;

		internal LoginScreen _this;

		internal float __m__0()
		{
			return this.panel.alpha;
		}

		internal void __m__1(float x)
		{
			this.panel.alpha = x;
		}

		internal void __m__2()
		{
			this._this.killTweensOfPanel(this.panel);
			if (!this.active)
			{
				this.panel.gameObject.SetActive(false);
			}
			if (this.callback != null)
			{
				this.callback();
			}
		}
	}

	public LoginScreenPanel MainPanel;

	public LoginScreenPanel NewUserPanel;

	public LoginLoadingPanel LoadingPanel;

	public LoginScreenPanel ErrorPanel;

	public LoginTextInputField LoginTextInputFieldPrefab;

	public GameObject KeyboardMouseNotifyBar;

	public GameObject TextAlertArea;

	public CursorTargetButton QuitButton;

	private List<LoginScreenPanel> allPanels = new List<LoginScreenPanel>();

	private LoginScreenPanel currentPanel;

	private Dictionary<LoginScreenPanel, Tweener> panelTweens = new Dictionary<LoginScreenPanel, Tweener>();

	[Inject]
	public ILoginScreenAPI api
	{
		get;
		set;
	}

	[Inject]
	public ILoginValidator validator
	{
		get;
		set;
	}

	[Inject]
	public IWindowDisplay windowManager
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
	public IDialogController dialog
	{
		get;
		set;
	}

	[Inject]
	public ISoundFileManager soundFileManager
	{
		get;
		set;
	}

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
		foreach (LoginScreenPanel current in this.allPanels)
		{
			base.injector.Inject(current);
		}
	}

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

	private void onUpdate()
	{
	}

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

	private void initPanel(LoginScreenPanel thePanel)
	{
		if (thePanel != null && !this.allPanels.Contains(thePanel))
		{
			throw new UnityException("Unmapped panel");
		}
		foreach (LoginScreenPanel current in this.allPanels)
		{
			current.gameObject.SetActive(false);
		}
		this.currentPanel = thePanel;
		if (this.currentPanel != null)
		{
			this.currentPanel.gameObject.SetActive(true);
		}
	}

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

	private void onPanelAnimationComplete()
	{
		base.unlockInput();
	}

	private void panelTween(LoginScreenPanel panel, bool active, Action callback = null)
	{
		LoginScreen._panelTween_c__AnonStorey0 _panelTween_c__AnonStorey = new LoginScreen._panelTween_c__AnonStorey0();
		_panelTween_c__AnonStorey.panel = panel;
		_panelTween_c__AnonStorey.active = active;
		_panelTween_c__AnonStorey.callback = callback;
		_panelTween_c__AnonStorey._this = this;
		this.killTweensOfPanel(_panelTween_c__AnonStorey.panel);
		float endValue = (float)((!_panelTween_c__AnonStorey.active) ? 0 : 1);
		float duration = 0.12f;
		if (_panelTween_c__AnonStorey.active)
		{
			_panelTween_c__AnonStorey.panel.gameObject.SetActive(true);
		}
		this.panelTweens[_panelTween_c__AnonStorey.panel] = DOTween.To(new DOGetter<float>(_panelTween_c__AnonStorey.__m__0), new DOSetter<float>(_panelTween_c__AnonStorey.__m__1), endValue, duration).SetEase(Ease.Linear).OnComplete(new TweenCallback(_panelTween_c__AnonStorey.__m__2));
	}

	private void killTweensOfPanel(LoginScreenPanel panel)
	{
		if (this.panelTweens.ContainsKey(panel))
		{
			Tweener tweener = this.panelTweens[panel];
			TweenUtil.Destroy(ref tweener);
			this.panelTweens.Remove(panel);
		}
	}

	private void enterGame()
	{
		base.events.Broadcast(new LoadScreenCommand(ScreenType.MainMenu, this.payload, ScreenUpdateType.Next));
	}

	private void onQuitButtonClick(CursorTargetButton button, PointerEventData eventData)
	{
		Application.Quit();
	}

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
					UnityEngine.Debug.LogError("CONNECTION ERROR - NetIOClient plugin is the wrong version.");
				}
				else if (error.steamLoginError == LoginSteamAckMsg.EResult.ClosedToPublic)
				{
					UnityEngine.Debug.LogError("CONNECTION ERROR - Closed to public.");
				}
				else
				{
					UnityEngine.Debug.LogError("CONNECTION ERROR - Unknown");
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

	public override void OnDestroy()
	{
		this.onPanelAnimationComplete();
		foreach (LoginScreenPanel current in this.allPanels)
		{
			this.killTweensOfPanel(current);
			current.OnDestroy();
		}
		if (base.signalBus != null)
		{
			base.signalBus.GetSignal<ServerLoginError>().RemoveListener(new Action<LoginError>(this.onServerLoginError));
		}
		base.OnDestroy();
	}
}
