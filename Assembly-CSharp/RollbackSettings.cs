using System;
using System.Collections.Generic;

// Token: 0x0200087C RID: 2172
public class RollbackSettings
{
	// Token: 0x06003691 RID: 13969 RVA: 0x000FF499 File Offset: 0x000FD899
	public RollbackSettings(int playerCount, List<RollbackPlayerData> playerDataList, IReplaySystem replaySystem, GenericResetObjectPool<RollbackInput> rollbackInputPool, NetworkSettings networkSettings)
	{
		this.playerCount = playerCount;
		this.playerDataList = playerDataList;
		this.replaySystem = replaySystem;
		this.rollbackInputPool = rollbackInputPool;
		this.networkSettings = networkSettings;
	}

	// Token: 0x06003692 RID: 13970 RVA: 0x000FF4D0 File Offset: 0x000FD8D0
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

	// Token: 0x0400251C RID: 9500
	public int playerCount;

	// Token: 0x0400251D RID: 9501
	public List<RollbackPlayerData> playerDataList;

	// Token: 0x0400251E RID: 9502
	public IBattleServerAPI room;

	// Token: 0x0400251F RID: 9503
	public IReplaySystem replaySystem;

	// Token: 0x04002520 RID: 9504
	public GenericResetObjectPool<RollbackInput> rollbackInputPool;

	// Token: 0x04002521 RID: 9505
	public bool simulate;

	// Token: 0x04002522 RID: 9506
	public int autoRollbackSimulationRate;

	// Token: 0x04002523 RID: 9507
	public int autoRollbackSimulationAmount = 4;

	// Token: 0x04002524 RID: 9508
	public int inputDelayPing;

	// Token: 0x04002525 RID: 9509
	public int initialTimestepDelta;

	// Token: 0x04002526 RID: 9510
	public int inputDelayFrames;

	// Token: 0x04002527 RID: 9511
	public NetworkSettings networkSettings;
}
