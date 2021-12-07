// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class RollbackSettings
{
	public int playerCount;

	public List<RollbackPlayerData> playerDataList;

	public IBattleServerAPI room;

	public IReplaySystem replaySystem;

	public GenericResetObjectPool<RollbackInput> rollbackInputPool;

	public bool simulate;

	public int autoRollbackSimulationRate;

	public int autoRollbackSimulationAmount = 4;

	public int inputDelayPing;

	public int initialTimestepDelta;

	public int inputDelayFrames;

	public NetworkSettings networkSettings;

	public RollbackSettings(int playerCount, List<RollbackPlayerData> playerDataList, IReplaySystem replaySystem, GenericResetObjectPool<RollbackInput> rollbackInputPool, NetworkSettings networkSettings)
	{
		this.playerCount = playerCount;
		this.playerDataList = playerDataList;
		this.replaySystem = replaySystem;
		this.rollbackInputPool = rollbackInputPool;
		this.networkSettings = networkSettings;
	}

	public void UpdatePerMatchNetworkSettings(IBattleServerAPI battleServerAPI)
	{
		this.inputDelayFrames = ((!battleServerAPI.IsConnected) ? 0 : this.networkSettings.inputDelayFrames);
		if (battleServerAPI.IsConnected)
		{
			this.inputDelayPing = (int)battleServerAPI.ServerPing;
			this.initialTimestepDelta = battleServerAPI.ServerTimestepDelta;
			if (this.inputDelayPing > this.networkSettings.baseLatency)
			{
				this.inputDelayFrames = Math.Min(this.networkSettings.maxInputDelay, this.inputDelayFrames + (this.inputDelayPing - this.networkSettings.baseLatency) / this.networkSettings.latencyPerAdditionalInputFrame);
			}
		}
		else
		{
			this.inputDelayPing = 0;
			this.initialTimestepDelta = 0;
			if (this.networkSettings.simulateRollback)
			{
				this.simulate = true;
				this.autoRollbackSimulationRate = this.networkSettings.autoRollbackSimulationRate;
				this.autoRollbackSimulationAmount = this.networkSettings.autoRollbackSimulationAmount;
			}
		}
	}
}
