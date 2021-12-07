using System;
using IconsServer;
using UnityEngine;

// Token: 0x02000832 RID: 2098
public class ServerConnectionManager : IServerConnectionManager
{
	// Token: 0x17000C8E RID: 3214
	// (get) Token: 0x06003435 RID: 13365 RVA: 0x000F75D5 File Offset: 0x000F59D5
	// (set) Token: 0x06003436 RID: 13366 RVA: 0x000F75DD File Offset: 0x000F59DD
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000C8F RID: 3215
	// (get) Token: 0x06003437 RID: 13367 RVA: 0x000F75E6 File Offset: 0x000F59E6
	// (set) Token: 0x06003438 RID: 13368 RVA: 0x000F75EE File Offset: 0x000F59EE
	[Inject]
	public IIconsServerAPI iconsServerAPI { get; set; }

	// Token: 0x17000C90 RID: 3216
	// (get) Token: 0x06003439 RID: 13369 RVA: 0x000F75F7 File Offset: 0x000F59F7
	// (set) Token: 0x0600343A RID: 13370 RVA: 0x000F75FF File Offset: 0x000F59FF
	[Inject]
	public IOfflineModeDetector offlineMode { get; set; }

	// Token: 0x17000C91 RID: 3217
	// (get) Token: 0x0600343B RID: 13371 RVA: 0x000F7608 File Offset: 0x000F5A08
	// (set) Token: 0x0600343C RID: 13372 RVA: 0x000F7610 File Offset: 0x000F5A10
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x17000C92 RID: 3218
	// (get) Token: 0x0600343D RID: 13373 RVA: 0x000F7619 File Offset: 0x000F5A19
	// (set) Token: 0x0600343E RID: 13374 RVA: 0x000F7621 File Offset: 0x000F5A21
	[Inject]
	public IStartupArgs startupArgs { get; set; }

	// Token: 0x17000C93 RID: 3219
	// (get) Token: 0x0600343F RID: 13375 RVA: 0x000F762A File Offset: 0x000F5A2A
	// (set) Token: 0x06003440 RID: 13376 RVA: 0x000F7632 File Offset: 0x000F5A32
	[Inject]
	public IPreviousCrashDetector previousCrashDetector { get; set; }

	// Token: 0x17000C94 RID: 3220
	// (get) Token: 0x06003441 RID: 13377 RVA: 0x000F763B File Offset: 0x000F5A3B
	// (set) Token: 0x06003442 RID: 13378 RVA: 0x000F7643 File Offset: 0x000F5A43
	[Inject]
	public P2PServerMgr p2PServerMgr { get; set; }

	// Token: 0x17000C95 RID: 3221
	// (get) Token: 0x06003443 RID: 13379 RVA: 0x000F764C File Offset: 0x000F5A4C
	// (set) Token: 0x06003444 RID: 13380 RVA: 0x000F7654 File Offset: 0x000F5A54
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x17000C96 RID: 3222
	// (get) Token: 0x06003445 RID: 13381 RVA: 0x000F765D File Offset: 0x000F5A5D
	public bool IsConnectedToNexus
	{
		get
		{
			return this.isConnectedToNexus;
		}
	}

	// Token: 0x17000C97 RID: 3223
	// (get) Token: 0x06003446 RID: 13382 RVA: 0x000F7665 File Offset: 0x000F5A65
	public bool IsConnectedToAuth
	{
		get
		{
			return this.isConnectedToAuth;
		}
	}

	// Token: 0x17000C98 RID: 3224
	// (get) Token: 0x06003447 RID: 13383 RVA: 0x000F766D File Offset: 0x000F5A6D
	public bool IsConnectingInProgress
	{
		get
		{
			return this.isConnectingInProgress;
		}
	}

	// Token: 0x17000C99 RID: 3225
	// (get) Token: 0x06003448 RID: 13384 RVA: 0x000F7675 File Offset: 0x000F5A75
	public bool IsConnectionError
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000C9A RID: 3226
	// (get) Token: 0x06003449 RID: 13385 RVA: 0x000F7678 File Offset: 0x000F5A78
	public bool IsDataLoadComplete
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000C9B RID: 3227
	// (get) Token: 0x0600344A RID: 13386 RVA: 0x000F767B File Offset: 0x000F5A7B
	public bool IsDataLoadError
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000C9C RID: 3228
	// (get) Token: 0x0600344B RID: 13387 RVA: 0x000F767E File Offset: 0x000F5A7E
	public bool IsCoreConnectionReady
	{
		get
		{
			return this.IsConnectedToNexus && this.IsDataLoadComplete;
		}
	}

	// Token: 0x17000C9D RID: 3229
	// (get) Token: 0x0600344C RID: 13388 RVA: 0x000F7694 File Offset: 0x000F5A94
	public float ConnectionAttemptTime
	{
		get
		{
			return this.connectionAttemptTime;
		}
	}

	// Token: 0x17000C9E RID: 3230
	// (get) Token: 0x0600344D RID: 13389 RVA: 0x000F769C File Offset: 0x000F5A9C
	public ConnectionAttemptType ConnectionAttemptType
	{
		get
		{
			return this.connectionAttemptType;
		}
	}

	// Token: 0x0600344E RID: 13390 RVA: 0x000F76A4 File Offset: 0x000F5AA4
	public void Startup(NetworkSettings netSettings)
	{
		this.netSettings = netSettings;
	}

	// Token: 0x17000C9F RID: 3231
	// (get) Token: 0x0600344F RID: 13391 RVA: 0x000F76AD File Offset: 0x000F5AAD
	// (set) Token: 0x06003450 RID: 13392 RVA: 0x000F76B5 File Offset: 0x000F5AB5
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

	// Token: 0x06003451 RID: 13393 RVA: 0x000F76DA File Offset: 0x000F5ADA
	public void Disconnect()
	{
	}

	// Token: 0x06003452 RID: 13394 RVA: 0x000F76DC File Offset: 0x000F5ADC
	public void ErrorReconnect()
	{
		this.isConnectingInProgress = false;
		this.isConnectedToAuth = false;
		this.isConnectedToNexus = false;
		this.hasAccount = false;
		this.iconsServerAPI.CloseAllConnections();
		this.signalBus.Dispatch(ServerConnectionManager.UPDATED);
	}

	// Token: 0x06003453 RID: 13395 RVA: 0x000F7715 File Offset: 0x000F5B15
	private void onInitialDataRequestUpdate()
	{
		this.signalBus.Dispatch(ServerConnectionManager.UPDATED);
	}

	// Token: 0x06003454 RID: 13396 RVA: 0x000F7727 File Offset: 0x000F5B27
	public void SendAllMessages()
	{
		this.p2PServerMgr.SendNetworkMessages();
	}

	// Token: 0x06003455 RID: 13397 RVA: 0x000F7734 File Offset: 0x000F5B34
	public void ReceiveAllMessages()
	{
		this.p2PServerMgr.ReceiveNetworkMessages();
	}

	// Token: 0x06003456 RID: 13398 RVA: 0x000F7741 File Offset: 0x000F5B41
	public void ClearAllMessages()
	{
		this.p2PServerMgr.ClearNetworkMessages();
	}

	// Token: 0x17000CA0 RID: 3232
	// (get) Token: 0x06003457 RID: 13399 RVA: 0x000F7750 File Offset: 0x000F5B50
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
				Debug.Log(string.Concat(new object[]
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

	// Token: 0x17000CA1 RID: 3233
	// (get) Token: 0x06003458 RID: 13400 RVA: 0x000F77E7 File Offset: 0x000F5BE7
	private string ip
	{
		get
		{
			return this.getIpForServerEnv(this.ServerEnv);
		}
	}

	// Token: 0x17000CA2 RID: 3234
	// (get) Token: 0x06003459 RID: 13401 RVA: 0x000F77F5 File Offset: 0x000F5BF5
	public uint port
	{
		get
		{
			return this.getPortForServerEnv(this.ServerEnv);
		}
	}

	// Token: 0x17000CA3 RID: 3235
	// (get) Token: 0x0600345A RID: 13402 RVA: 0x000F7803 File Offset: 0x000F5C03
	public string EndPoint
	{
		get
		{
			return this.ip + ":" + this.port;
		}
	}

	// Token: 0x0600345B RID: 13403 RVA: 0x000F7820 File Offset: 0x000F5C20
	private string getIpForServerEnv(ServerEnvironment env)
	{
		if (env == ServerEnvironment.CUSTOM)
		{
			return this.netSettings.customIP;
		}
		return this.netSettings.serverLocations[env].ip;
	}

	// Token: 0x0600345C RID: 13404 RVA: 0x000F784C File Offset: 0x000F5C4C
	private uint getPortForServerEnv(ServerEnvironment env)
	{
		if (env == ServerEnvironment.CUSTOM)
		{
			return this.netSettings.customPort;
		}
		return this.netSettings.serverLocations[env].port;
	}

	// Token: 0x04002434 RID: 9268
	public static string UPDATED = "ServerConnectionManager.UPDATE";

	// Token: 0x0400243D RID: 9277
	private NetworkSettings netSettings;

	// Token: 0x0400243E RID: 9278
	private float connectionAttemptTime;

	// Token: 0x0400243F RID: 9279
	private ConnectionAttemptType connectionAttemptType;

	// Token: 0x04002440 RID: 9280
	private bool isConnectedToNexus;

	// Token: 0x04002441 RID: 9281
	private bool isConnectedToAuth;

	// Token: 0x04002442 RID: 9282
	private bool isConnectingInProgress;

	// Token: 0x04002443 RID: 9283
	private bool hasAccount;
}
