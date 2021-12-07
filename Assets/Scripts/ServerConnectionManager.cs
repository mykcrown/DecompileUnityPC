// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;
using UnityEngine;

public class ServerConnectionManager : IServerConnectionManager
{
	public static string UPDATED = "ServerConnectionManager.UPDATE";

	private NetworkSettings netSettings;

	private float connectionAttemptTime;

	private ConnectionAttemptType connectionAttemptType;

	private bool isConnectedToNexus;

	private bool isConnectedToAuth;

	private bool isConnectingInProgress;

	private bool hasAccount;

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public IIconsServerAPI iconsServerAPI
	{
		get;
		set;
	}

	[Inject]
	public IOfflineModeDetector offlineMode
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

	[Inject]
	public IStartupArgs startupArgs
	{
		get;
		set;
	}

	[Inject]
	public IPreviousCrashDetector previousCrashDetector
	{
		get;
		set;
	}

	[Inject]
	public P2PServerMgr p2PServerMgr
	{
		get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	public bool IsConnectedToNexus
	{
		get
		{
			return this.isConnectedToNexus;
		}
	}

	public bool IsConnectedToAuth
	{
		get
		{
			return this.isConnectedToAuth;
		}
	}

	public bool IsConnectingInProgress
	{
		get
		{
			return this.isConnectingInProgress;
		}
	}

	public bool IsConnectionError
	{
		get
		{
			return false;
		}
	}

	public bool IsDataLoadComplete
	{
		get
		{
			return true;
		}
	}

	public bool IsDataLoadError
	{
		get
		{
			return false;
		}
	}

	public bool IsCoreConnectionReady
	{
		get
		{
			return this.IsConnectedToNexus && this.IsDataLoadComplete;
		}
	}

	public float ConnectionAttemptTime
	{
		get
		{
			return this.connectionAttemptTime;
		}
	}

	public ConnectionAttemptType ConnectionAttemptType
	{
		get
		{
			return this.connectionAttemptType;
		}
	}

	public bool HasAccount
	{
		get
		{
			return this.hasAccount;
		}
		set
		{
			if (this.hasAccount != value)
			{
				this.hasAccount = value;
				this.signalBus.Dispatch(ServerConnectionManager.UPDATED);
			}
		}
	}

	public ServerEnvironment ServerEnv
	{
		get
		{
			if (BuildConfig.environmentType == BuildEnvironment.Local && this.netSettings != null && this.netSettings.overrideServerEnv)
			{
				return this.netSettings.overrideEnv;
			}
			if (this.startupArgs.HasArg(StartupArgs.StartupArgType.OverrideServerEnv))
			{
				int argIntValue = this.startupArgs.GetArgIntValue(StartupArgs.StartupArgType.OverrideServerEnv);
				ServerEnvironment serverEnvironment = (ServerEnvironment)argIntValue;
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"Using override server environment: ",
					argIntValue,
					" ",
					serverEnvironment
				}));
				return serverEnvironment;
			}
			return BuildConfig.serverEnvironment;
		}
	}

	private string ip
	{
		get
		{
			return this.getIpForServerEnv(this.ServerEnv);
		}
	}

	public uint port
	{
		get
		{
			return this.getPortForServerEnv(this.ServerEnv);
		}
	}

	public string EndPoint
	{
		get
		{
			return this.ip + ":" + this.port;
		}
	}

	public void Startup(NetworkSettings netSettings)
	{
		this.netSettings = netSettings;
	}

	public void Disconnect()
	{
	}

	public void ErrorReconnect()
	{
		this.isConnectingInProgress = false;
		this.isConnectedToAuth = false;
		this.isConnectedToNexus = false;
		this.hasAccount = false;
		this.iconsServerAPI.CloseAllConnections();
		this.signalBus.Dispatch(ServerConnectionManager.UPDATED);
	}

	private void onInitialDataRequestUpdate()
	{
		this.signalBus.Dispatch(ServerConnectionManager.UPDATED);
	}

	public void SendAllMessages()
	{
		this.p2PServerMgr.SendNetworkMessages();
	}

	public void ReceiveAllMessages()
	{
		this.p2PServerMgr.ReceiveNetworkMessages();
	}

	public void ClearAllMessages()
	{
		this.p2PServerMgr.ClearNetworkMessages();
	}

	private string getIpForServerEnv(ServerEnvironment env)
	{
		if (env == ServerEnvironment.CUSTOM)
		{
			return this.netSettings.customIP;
		}
		return this.netSettings.serverLocations[env].ip;
	}

	private uint getPortForServerEnv(ServerEnvironment env)
	{
		if (env == ServerEnvironment.CUSTOM)
		{
			return this.netSettings.customPort;
		}
		return this.netSettings.serverLocations[env].port;
	}
}
