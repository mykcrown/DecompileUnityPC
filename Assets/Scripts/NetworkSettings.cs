// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class NetworkSettings
{
	public Dictionary<ServerEnvironment, NetworkLocation> serverLocations = new Dictionary<ServerEnvironment, NetworkLocation>();

	public int simulatedLatencyFrames;

	public bool simulateRollback;

	public int autoRollbackSimulationRate;

	public int autoRollbackSimulationAmount;

	public bool debugStepIntoRollback;

	public int inputDelayFrames;

	public int baseLatency = 50;

	public int latencyPerAdditionalInputFrame = 16;

	public int maxInputDelay = 3;

	public long AppID = 684200L;

	public long DiscordAppID;

	public uint timeoutMs = 15000u;

	public string customIP;

	public uint customPort;

	public bool gatheringUdpStats = true;

	public float debugUdpDropRate;

	public bool overrideServerEnv;

	public ServerEnvironment overrideEnv = ServerEnvironment.STEAM;
}
