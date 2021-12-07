// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IServerConnectionManager
{
	float ConnectionAttemptTime
	{
		get;
	}

	ConnectionAttemptType ConnectionAttemptType
	{
		get;
	}

	ServerEnvironment ServerEnv
	{
		get;
	}

	string EndPoint
	{
		get;
	}

	bool IsConnectedToNexus
	{
		get;
	}

	bool IsDataLoadComplete
	{
		get;
	}

	bool IsDataLoadError
	{
		get;
	}

	bool IsCoreConnectionReady
	{
		get;
	}

	bool IsConnectedToAuth
	{
		get;
	}

	bool IsConnectionError
	{
		get;
	}

	bool IsConnectingInProgress
	{
		get;
	}

	bool HasAccount
	{
		get;
		set;
	}

	void Startup(NetworkSettings settings);

	void Disconnect();

	void ErrorReconnect();

	void SendAllMessages();

	void ReceiveAllMessages();

	void ClearAllMessages();
}
