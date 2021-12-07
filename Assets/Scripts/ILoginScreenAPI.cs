// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ILoginScreenAPI
{
	bool RememberMe
	{
		get;
		set;
	}

	bool TermsAccepted
	{
		get;
		set;
	}

	bool HasAccount
	{
		get;
	}

	bool IsConnected
	{
		get;
	}

	bool IsDataSyncError
	{
		get;
	}

	bool IsConnectingInProgress
	{
		get;
	}

	bool IsAccountCreateInProgress
	{
		get;
	}

	bool CanEnterGame
	{
		get;
	}

	bool ShowingAccountCreateComplete
	{
		get;
		set;
	}

	bool ShowingAccountCreateError
	{
		get;
		set;
	}

	bool IsShowingAccountCreateResult
	{
		get;
	}

	void OnScreenShown();

	void OnEnterGame();

	void RetryConnection();
}
