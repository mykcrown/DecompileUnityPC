using System;
using UnityEngine;

// Token: 0x02000995 RID: 2453
public class LoginScreenAPI : ILoginScreenAPI
{
	// Token: 0x17000FBF RID: 4031
	// (get) Token: 0x060042CB RID: 17099 RVA: 0x001291BC File Offset: 0x001275BC
	// (set) Token: 0x060042CC RID: 17100 RVA: 0x001291C4 File Offset: 0x001275C4
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000FC0 RID: 4032
	// (get) Token: 0x060042CD RID: 17101 RVA: 0x001291CD File Offset: 0x001275CD
	// (set) Token: 0x060042CE RID: 17102 RVA: 0x001291D5 File Offset: 0x001275D5
	[Inject]
	public IServerConnectionManager serverManager { get; set; }

	// Token: 0x17000FC1 RID: 4033
	// (get) Token: 0x060042CF RID: 17103 RVA: 0x001291DE File Offset: 0x001275DE
	// (set) Token: 0x060042D0 RID: 17104 RVA: 0x001291E6 File Offset: 0x001275E6
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x060042D1 RID: 17105 RVA: 0x001291EF File Offset: 0x001275EF
	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(ServerConnectionManager.UPDATED, new Action(this.onServerUpdate));
	}

	// Token: 0x060042D2 RID: 17106 RVA: 0x0012920D File Offset: 0x0012760D
	public void OnScreenShown()
	{
		this.screenDisplayedTime = Time.realtimeSinceStartup + 0.1f;
	}

	// Token: 0x060042D3 RID: 17107 RVA: 0x00129220 File Offset: 0x00127620
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

	// Token: 0x17000FC2 RID: 4034
	// (get) Token: 0x060042D4 RID: 17108 RVA: 0x001292B9 File Offset: 0x001276B9
	// (set) Token: 0x060042D5 RID: 17109 RVA: 0x001292C1 File Offset: 0x001276C1
	public bool ShowingAccountCreateComplete { get; set; }

	// Token: 0x17000FC3 RID: 4035
	// (get) Token: 0x060042D6 RID: 17110 RVA: 0x001292CA File Offset: 0x001276CA
	// (set) Token: 0x060042D7 RID: 17111 RVA: 0x001292D2 File Offset: 0x001276D2
	public bool ShowingAccountCreateError { get; set; }

	// Token: 0x17000FC4 RID: 4036
	// (get) Token: 0x060042D8 RID: 17112 RVA: 0x001292DB File Offset: 0x001276DB
	// (set) Token: 0x060042D9 RID: 17113 RVA: 0x001292E3 File Offset: 0x001276E3
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

	// Token: 0x17000FC5 RID: 4037
	// (get) Token: 0x060042DA RID: 17114 RVA: 0x001292FE File Offset: 0x001276FE
	// (set) Token: 0x060042DB RID: 17115 RVA: 0x00129306 File Offset: 0x00127706
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

	// Token: 0x060042DC RID: 17116 RVA: 0x00129324 File Offset: 0x00127724
	public void RetryConnection()
	{
		ConnectionAttemptType type = ConnectionAttemptType.USER_INITIATED;
		float minLoadingTime = this.getMinLoadingTime(type);
		this.timer.SetTimeout((int)(minLoadingTime * 1000f + 100f), new Action(this.dispatchUpdate));
	}

	// Token: 0x060042DD RID: 17117 RVA: 0x00129360 File Offset: 0x00127760
	private float getMinLoadingTime(ConnectionAttemptType type)
	{
		if (type != ConnectionAttemptType.USER_INITIATED)
		{
			return LoginScreenAPI.MIN_AUTOMATIC_LOADING;
		}
		return LoginScreenAPI.MIN_RETRY_LOADING;
	}

	// Token: 0x060042DE RID: 17118 RVA: 0x00129379 File Offset: 0x00127779
	private void dispatchUpdate()
	{
		this.signalBus.Dispatch(LoginScreenAPI.UPDATED);
	}

	// Token: 0x17000FC6 RID: 4038
	// (get) Token: 0x060042DF RID: 17119 RVA: 0x0012938B File Offset: 0x0012778B
	public bool HasAccount
	{
		get
		{
			return this.serverManager.HasAccount && this.serverManager.IsConnectedToAuth;
		}
	}

	// Token: 0x17000FC7 RID: 4039
	// (get) Token: 0x060042E0 RID: 17120 RVA: 0x001293AB File Offset: 0x001277AB
	public bool IsConnected
	{
		get
		{
			return this.serverManager.IsConnectedToAuth && !this.IsConnectingInProgress;
		}
	}

	// Token: 0x17000FC8 RID: 4040
	// (get) Token: 0x060042E1 RID: 17121 RVA: 0x001293CC File Offset: 0x001277CC
	public bool IsAccountCreateInProgress
	{
		get
		{
			if (this.createAccountStartTime != 0f)
			{
				float min_ACCOUNT_CREATE_LOADING = LoginScreenAPI.MIN_ACCOUNT_CREATE_LOADING;
				float num = this.createAccountStartTime;
				float num2 = Time.realtimeSinceStartup - num;
				if (num2 > 0f && num2 < min_ACCOUNT_CREATE_LOADING)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x17000FC9 RID: 4041
	// (get) Token: 0x060042E2 RID: 17122 RVA: 0x00129413 File Offset: 0x00127813
	public bool IsShowingAccountCreateResult
	{
		get
		{
			return this.ShowingAccountCreateComplete || this.ShowingAccountCreateError;
		}
	}

	// Token: 0x17000FCA RID: 4042
	// (get) Token: 0x060042E3 RID: 17123 RVA: 0x00129429 File Offset: 0x00127829
	public bool CanEnterGame
	{
		get
		{
			return this.HasAccount && !this.IsShowingAccountCreateResult && !this.IsConnectingInProgress && !this.IsAccountCreateInProgress && this.IsCoreConnectionReady;
		}
	}

	// Token: 0x17000FCB RID: 4043
	// (get) Token: 0x060042E4 RID: 17124 RVA: 0x00129460 File Offset: 0x00127860
	public bool IsConnectedToNexus
	{
		get
		{
			return this.serverManager.IsConnectedToNexus;
		}
	}

	// Token: 0x17000FCC RID: 4044
	// (get) Token: 0x060042E5 RID: 17125 RVA: 0x0012946D File Offset: 0x0012786D
	public bool IsCoreConnectionReady
	{
		get
		{
			return this.serverManager.IsCoreConnectionReady;
		}
	}

	// Token: 0x17000FCD RID: 4045
	// (get) Token: 0x060042E6 RID: 17126 RVA: 0x0012947A File Offset: 0x0012787A
	public bool IsDataSyncError
	{
		get
		{
			return this.serverManager.IsDataLoadError;
		}
	}

	// Token: 0x17000FCE RID: 4046
	// (get) Token: 0x060042E7 RID: 17127 RVA: 0x00129488 File Offset: 0x00127888
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

	// Token: 0x060042E8 RID: 17128 RVA: 0x0012951F File Offset: 0x0012791F
	public void OnEnterGame()
	{
	}

	// Token: 0x04002C9C RID: 11420
	public static string UPDATED = "LoginScreenAPI.UPDATED";

	// Token: 0x04002C9D RID: 11421
	private static float MIN_RETRY_LOADING = 4f;

	// Token: 0x04002C9E RID: 11422
	private static float MIN_AUTOMATIC_LOADING = 1.75f;

	// Token: 0x04002C9F RID: 11423
	private static float MIN_ACCOUNT_CREATE_LOADING = 0.75f;

	// Token: 0x04002CA3 RID: 11427
	private float screenDisplayedTime;

	// Token: 0x04002CA4 RID: 11428
	private float createAccountStartTime;

	// Token: 0x04002CA5 RID: 11429
	private bool rememberMeState;

	// Token: 0x04002CA6 RID: 11430
	private bool termsAcceptedState;
}
