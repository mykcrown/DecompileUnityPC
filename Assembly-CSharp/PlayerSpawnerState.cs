using System;
using System.Collections.Generic;

// Token: 0x020004B3 RID: 1203
[Serializable]
public class PlayerSpawnerState : RollbackStateTyped<PlayerSpawnerState>
{
	// Token: 0x06001AAA RID: 6826 RVA: 0x000893C7 File Offset: 0x000877C7
	public override void CopyTo(PlayerSpawnerState targetIn)
	{
		base.copyDictionary<PlayerNum, PlayerEngagementState>(this.queuedSpawnType, targetIn.queuedSpawnType);
		base.copyDictionary<PlayerNum, int>(this.respawnQueuedFrame, targetIn.respawnQueuedFrame);
	}

	// Token: 0x06001AAB RID: 6827 RVA: 0x000893F0 File Offset: 0x000877F0
	public override object Clone()
	{
		PlayerSpawnerState playerSpawnerState = new PlayerSpawnerState();
		this.CopyTo(playerSpawnerState);
		return playerSpawnerState;
	}

	// Token: 0x040013EB RID: 5099
	[IsClonedManually]
	[IgnoreCopyValidation]
	public Dictionary<PlayerNum, PlayerEngagementState> queuedSpawnType = new Dictionary<PlayerNum, PlayerEngagementState>(4, default(PlayerNumComparer));

	// Token: 0x040013EC RID: 5100
	[IsClonedManually]
	[IgnoreCopyValidation]
	public Dictionary<PlayerNum, int> respawnQueuedFrame = new Dictionary<PlayerNum, int>(4, default(PlayerNumComparer));
}
