// Decompile from assembly: Assembly-CSharp.dll

using System;

public class MockLoginScreenApi : ILoginScreenAPI
{
	private bool termsAccepted;

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	public bool CanEnterGame
	{
		get
		{
			return false;
		}
	}

	public bool HasAccount
	{
		get
		{
			return false;
		}
	}

	public bool IsAccountCreateInProgress
	{
		get
		{
			return false;
		}
	}

	public bool IsConnected
	{
		get
		{
			return true;
		}
	}

	public bool IsConnectingInProgress
	{
		get
		{
			return false;
		}
	}

	public bool IsDataSyncError
	{
		get
		{
			return false;
		}
	}

	public bool IsShowingAccountCreateResult
	{
		get
		{
			return false;
		}
	}

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

	public void OnEnterGame()
	{
	}

	public void OnScreenShown()
	{
	}

	public void RetryConnection()
	{
	}

	private void dispatchUpdate()
	{
		this.signalBus.Dispatch(LoginScreenAPI.UPDATED);
	}
}
