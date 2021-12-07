using System;

// Token: 0x02000834 RID: 2100
public interface IServerConnectionManager
{
	// Token: 0x0600345F RID: 13407
	void Startup(NetworkSettings settings);

	// Token: 0x06003460 RID: 13408
	void Disconnect();

	// Token: 0x06003461 RID: 13409
	void ErrorReconnect();

	// Token: 0x17000CA4 RID: 3236
	// (get) Token: 0x06003462 RID: 13410
	float ConnectionAttemptTime { get; }

	// Token: 0x17000CA5 RID: 3237
	// (get) Token: 0x06003463 RID: 13411
	ConnectionAttemptType ConnectionAttemptType { get; }

	// Token: 0x17000CA6 RID: 3238
	// (get) Token: 0x06003464 RID: 13412
	ServerEnvironment ServerEnv { get; }

	// Token: 0x17000CA7 RID: 3239
	// (get) Token: 0x06003465 RID: 13413
	string EndPoint { get; }

	// Token: 0x06003466 RID: 13414
	void SendAllMessages();

	// Token: 0x06003467 RID: 13415
	void ReceiveAllMessages();

	// Token: 0x06003468 RID: 13416
	void ClearAllMessages();

	// Token: 0x17000CA8 RID: 3240
	// (get) Token: 0x06003469 RID: 13417
	bool IsConnectedToNexus { get; }

	// Token: 0x17000CA9 RID: 3241
	// (get) Token: 0x0600346A RID: 13418
	bool IsDataLoadComplete { get; }

	// Token: 0x17000CAA RID: 3242
	// (get) Token: 0x0600346B RID: 13419
	bool IsDataLoadError { get; }

	// Token: 0x17000CAB RID: 3243
	// (get) Token: 0x0600346C RID: 13420
	bool IsCoreConnectionReady { get; }

	// Token: 0x17000CAC RID: 3244
	// (get) Token: 0x0600346D RID: 13421
	bool IsConnectedToAuth { get; }

	// Token: 0x17000CAD RID: 3245
	// (get) Token: 0x0600346E RID: 13422
	bool IsConnectionError { get; }

	// Token: 0x17000CAE RID: 3246
	// (get) Token: 0x0600346F RID: 13423
	bool IsConnectingInProgress { get; }

	// Token: 0x17000CAF RID: 3247
	// (get) Token: 0x06003470 RID: 13424
	// (set) Token: 0x06003471 RID: 13425
	bool HasAccount { get; set; }
}
