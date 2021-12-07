using System;
using System.Collections.Generic;

// Token: 0x020003ED RID: 1005
[Serializable]
public class NetworkSettings
{
	// Token: 0x04000F7E RID: 3966
	public Dictionary<ServerEnvironment, NetworkLocation> serverLocations = new Dictionary<ServerEnvironment, NetworkLocation>();

	// Token: 0x04000F7F RID: 3967
	public int simulatedLatencyFrames;

	// Token: 0x04000F80 RID: 3968
	public bool simulateRollback;

	// Token: 0x04000F81 RID: 3969
	public int autoRollbackSimulationRate;

	// Token: 0x04000F82 RID: 3970
	public int autoRollbackSimulationAmount;

	// Token: 0x04000F83 RID: 3971
	public bool debugStepIntoRollback;

	// Token: 0x04000F84 RID: 3972
	public int inputDelayFrames;

	// Token: 0x04000F85 RID: 3973
	public int baseLatency = 50;

	// Token: 0x04000F86 RID: 3974
	public int latencyPerAdditionalInputFrame = 16;

	// Token: 0x04000F87 RID: 3975
	public int maxInputDelay = 3;

	// Token: 0x04000F88 RID: 3976
	public long AppID = 684200L;

	// Token: 0x04000F89 RID: 3977
	public long DiscordAppID;

	// Token: 0x04000F8A RID: 3978
	public uint timeoutMs = 15000U;

	// Token: 0x04000F8B RID: 3979
	public string customIP;

	// Token: 0x04000F8C RID: 3980
	public uint customPort;

	// Token: 0x04000F8D RID: 3981
	public bool gatheringUdpStats = true;

	// Token: 0x04000F8E RID: 3982
	public float debugUdpDropRate;

	// Token: 0x04000F8F RID: 3983
	public bool overrideServerEnv;

	// Token: 0x04000F90 RID: 3984
	public ServerEnvironment overrideEnv = ServerEnvironment.STEAM;
}
