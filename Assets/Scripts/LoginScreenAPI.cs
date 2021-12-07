// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class LoginScreenAPI : ILoginScreenAPI
{
	public static string UPDATED = "LoginScreenAPI.UPDATED";

	private static float MIN_RETRY_LOADING = 4f;

	private static float MIN_AUTOMATIC_LOADING = 1.75f;

	private static float MIN_ACCOUNT_CREATE_LOADING = 0.75f;

	private float screenDisplayedTime;

	private float createAccountStartTime;

	private bool rememberMeState;

	private bool termsAcceptedState;

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public IServerConnectionManager serverManager
	{
		get;
		set;
	}

	[Inject]
	public IMainThreadTimer timer
	{
		get;
		set;
	}

	public bool ShowingAccountCreateComplete
	{
		get;
		set;
	}

	public bool ShowingAccountCreateError
	{
		get;
		set;
	}

	public bool TermsAccepted
	{
		get
		{
			return this.termsAcceptedState;
		}
		set
		{
			if (this.termsAcceptedState != value)
			{
				this.termsAcceptedState = value;
				this.dispatchUpdate();
			}
		}
	}

	public bool RememberMe
	{
		get
		{
			return this.rememberMeState;
		}
		set
		{
			if (this.rememberMeState != value)
			{
				this.rememberMeState = value;
				this.dispatchUpdate();
			}
		}
	}

	public bool HasAccount
	{
		get
		{
			return this.serverManager.HasAccount && this.serverManager.IsConnectedToAuth;
		}
	}

	public bool IsConnected
	{
		get
		{
			return this.serverManager.IsConnectedToAuth && !this.IsConnectingInProgress;
		}
	}

	public bool IsAccountCreateInProgress
	{
		get
		{
			if (this.createAccountStartTime != 0f)
			{
				float mIN_ACCOUNT_CREATE_LOADING = LoginScreenAPI.MIN_ACCOUNT_CREATE_LOADING;
				float num = this.createAccountStartTime;
				float num2 = Time.realtimeSinceStartup - num;
				if (num2 > 0f && num2 < mIN_ACCOUNT_CREATE_LOADING)
				{
					return true;
				}
			}
			return false;
		}
	}

	public bool IsShowingAccountCreateResult
	{
		get
		{
			return this.ShowingAccountCreateComplete || this.ShowingAccountCreateError;
		}
	}

	public bool CanEnterGame
	{
		get
		{
			return this.HasAccount && !this.IsShowingAccountCreateResult && !this.IsConnectingInProgress && !this.IsAccountCreateInProgress && this.IsCoreConnectionReady;
		}
	}

	public bool IsConnectedToNexus
	{
		get
		{
			return this.serverManager.IsConnectedToNexus;
		}
	}

	public bool IsCoreConnectionReady
	{
		get
		{
			return this.serverManager.IsCoreConnectionReady;
		}
	}

	public bool IsDataSyncError
	{
		get
		{
			return this.serverManager.IsDataLoadError;
		}
	}

	public bool IsConnectingInProgress
	{
		get
		{
			if (this.serverManager.IsConnectingInProgress)
			{
				return true;
			}
			if (this.serverManager.HasAccount)
			{
				return false;
			}
			if (this.screenDisplayedTime != 0f && this.serverManager.ConnectionAttemptType != ConnectionAttemptType.NONE)
			{
				float minLoadingTime = this.getMinLoadingTime(this.serverManager.ConnectionAttemptType);
				float num = Mathf.Max(this.screenDisplayedTime, this.serverManager.ConnectionAttemptTime);
				float num2 = Time.realtimeSinceStartup - num;
				if (num2 > 0f && num2 < minLoadingTime)
				{
					return true;
				}
			}
			return false;
		}
	}

	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(ServerConnectionManager.UPDATED, new Action(this.onServerUpdate));
	}

	public void OnScreenShown()
	{
		this.screenDisplayedTime = Time.realtimeSinceStartup + 0.1f;
	}

	private void onServerUpdate()
	{
		if (this.serverManager.ConnectionAttemptType != ConnectionAttemptType.NONE)
		{
			ConnectionAttemptType connectionAttemptType = this.serverManager.ConnectionAttemptType;
			float num = Mathf.Max(this.screenDisplayedTime, this.serverManager.ConnectionAttemptTime);
			float minLoadingTime = this.getMinLoadingTime(connectionAttemptType);
			float num2 = num + minLoadingTime - Time.realtimeSinceStartup;
			if (num2 > 0f)
			{
				this.timer.CancelTimeout(new Action(this.dispatchUpdate));
				this.timer.SetTimeout((int)(num2 * 1000f + 33f), new Action(this.dispatchUpdate));
			}
		}
	}

	public void RetryConnection()
	{
		ConnectionAttemptType type = ConnectionAttemptType.USER_INITIATED;
		float minLoadingTime = this.getMinLoadingTime(type);
		this.timer.SetTimeout((int)(minLoadingTime * 1000f + 100f), new Action(this.dispatchUpdate));
	}

	private float getMinLoadingTime(ConnectionAttemptType type)
	{
		if (type != ConnectionAttemptType.USER_INITIATED)
		{
			return LoginScreenAPI.MIN_AUTOMATIC_LOADING;
		}
		return LoginScreenAPI.MIN_RETRY_LOADING;
	}

	private void dispatchUpdate()
	{
		this.signalBus.Dispatch(LoginScreenAPI.UPDATED);
	}

	public void OnEnterGame()
	{
	}
}
