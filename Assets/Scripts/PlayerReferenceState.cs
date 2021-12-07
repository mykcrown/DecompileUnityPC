// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class PlayerReferenceState : RollbackStateTyped<PlayerReferenceState>
{
	public int temporaryEngagementDurationFrames = -1;

	public int lives;

	public PlayerEngagementState engagementState;

	public override void CopyTo(PlayerReferenceState target)
	{
		target.temporaryEngagementDurationFrames = this.temporaryEngagementDurationFrames;
		target.lives = this.lives;
		target.engagementState = this.engagementState;
	}
}
