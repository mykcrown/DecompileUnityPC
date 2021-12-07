// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class PlayerSpawnerState : RollbackStateTyped<PlayerSpawnerState>
{
	[IgnoreCopyValidation, IsClonedManually]
	public Dictionary<PlayerNum, PlayerEngagementState> queuedSpawnType = new Dictionary<PlayerNum, PlayerEngagementState>(4, default(PlayerNumComparer));

	[IgnoreCopyValidation, IsClonedManually]
	public Dictionary<PlayerNum, int> respawnQueuedFrame = new Dictionary<PlayerNum, int>(4, default(PlayerNumComparer));

	public override void CopyTo(PlayerSpawnerState targetIn)
	{
		base.copyDictionary<PlayerNum, PlayerEngagementState>(this.queuedSpawnType, targetIn.queuedSpawnType);
		base.copyDictionary<PlayerNum, int>(this.respawnQueuedFrame, targetIn.respawnQueuedFrame);
	}

	public override object Clone()
	{
		PlayerSpawnerState playerSpawnerState = new PlayerSpawnerState();
		this.CopyTo(playerSpawnerState);
		return playerSpawnerState;
	}
}
