using System;

// Token: 0x020005F3 RID: 1523
[Serializable]
public class PlayerReferenceState : RollbackStateTyped<PlayerReferenceState>
{
	// Token: 0x060023F2 RID: 9202 RVA: 0x000B5884 File Offset: 0x000B3C84
	public override void CopyTo(PlayerReferenceState target)
	{
		target.temporaryEngagementDurationFrames = this.temporaryEngagementDurationFrames;
		target.lives = this.lives;
		target.engagementState = this.engagementState;
	}

	// Token: 0x04001B66 RID: 7014
	public int temporaryEngagementDurationFrames = -1;

	// Token: 0x04001B67 RID: 7015
	public int lives;

	// Token: 0x04001B68 RID: 7016
	public PlayerEngagementState engagementState;
}
