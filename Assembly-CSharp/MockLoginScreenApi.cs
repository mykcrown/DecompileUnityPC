using System;

// Token: 0x0200099D RID: 2461
public class MockLoginScreenApi : ILoginScreenAPI
{
	// Token: 0x17000FE0 RID: 4064
	// (get) Token: 0x0600431D RID: 17181 RVA: 0x001299FB File Offset: 0x00127DFB
	// (set) Token: 0x0600431E RID: 17182 RVA: 0x00129A03 File Offset: 0x00127E03
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000FE1 RID: 4065
	// (get) Token: 0x0600431F RID: 17183 RVA: 0x00129A0C File Offset: 0x00127E0C
	public bool CanEnterGame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000FE2 RID: 4066
	// (get) Token: 0x06004320 RID: 17184 RVA: 0x00129A0F File Offset: 0x00127E0F
	public bool HasAccount
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000FE3 RID: 4067
	// (get) Token: 0x06004321 RID: 17185 RVA: 0x00129A12 File Offset: 0x00127E12
	public bool IsAccountCreateInProgress
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000FE4 RID: 4068
	// (get) Token: 0x06004322 RID: 17186 RVA: 0x00129A15 File Offset: 0x00127E15
	public bool IsConnected
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000FE5 RID: 4069
	// (get) Token: 0x06004323 RID: 17187 RVA: 0x00129A18 File Offset: 0x00127E18
	public bool IsConnectingInProgress
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000FE6 RID: 4070
	// (get) Token: 0x06004324 RID: 17188 RVA: 0x00129A1B File Offset: 0x00127E1B
	public bool IsDataSyncError
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000FE7 RID: 4071
	// (get) Token: 0x06004325 RID: 17189 RVA: 0x00129A1E File Offset: 0x00127E1E
	public bool IsShowingAccountCreateResult
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000FE8 RID: 4072
	// (get) Token: 0x06004326 RID: 17190 RVA: 0x00129A21 File Offset: 0x00127E21
	// (set) Token: 0x06004327 RID: 17191 RVA: 0x00129A24 File Offset: 0x00127E24
	public bool RememberMe
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	// Token: 0x17000FE9 RID: 4073
	// (get) Token: 0x06004328 RID: 17192 RVA: 0x00129A26 File Offset: 0x00127E26
	// (set) Token: 0x06004329 RID: 17193 RVA: 0x00129A29 File Offset: 0x00127E29
	public bool ShowingAccountCreateComplete
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	// Token: 0x17000FEA RID: 4074
	// (get) Token: 0x0600432A RID: 17194 RVA: 0x00129A2B File Offset: 0x00127E2B
	// (set) Token: 0x0600432B RID: 17195 RVA: 0x00129A2E File Offset: 0x00127E2E
	public bool ShowingAccountCreateError
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	// Token: 0x17000FEB RID: 4075
	// (get) Token: 0x0600432C RID: 17196 RVA: 0x00129A30 File Offset: 0x00127E30
	// (set) Token: 0x0600432D RID: 17197 RVA: 0x00129A38 File Offset: 0x00127E38
	public bool TermsAccepted
	{
		get
		{
			return this.termsAccepted;
		}
		set
		{
			this.termsAccepted = value;
			this.dispatchUpdate();
		}
	}

	// Token: 0x0600432E RID: 17198 RVA: 0x00129A47 File Offset: 0x00127E47
	public void OnEnterGame()
	{
	}

	// Token: 0x0600432F RID: 17199 RVA: 0x00129A49 File Offset: 0x00127E49
	public void OnScreenShown()
	{
	}

	// Token: 0x06004330 RID: 17200 RVA: 0x00129A4B File Offset: 0x00127E4B
	public void RetryConnection()
	{
	}

	// Token: 0x06004331 RID: 17201 RVA: 0x00129A4D File Offset: 0x00127E4D
	private void dispatchUpdate()
	{
		this.signalBus.Dispatch(LoginScreenAPI.UPDATED);
	}

	// Token: 0x04002CCB RID: 11467
	private bool termsAccepted;
}
