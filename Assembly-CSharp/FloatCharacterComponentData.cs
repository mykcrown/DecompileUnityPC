using System;
using System.Collections.Generic;
using FixedPoint;

// Token: 0x020005C7 RID: 1479
[Serializable]
public class FloatCharacterComponentData
{
	// Token: 0x04001A0C RID: 6668
	public CharacterPhysicsOverride physicsData = new CharacterPhysicsOverride();

	// Token: 0x04001A0D RID: 6669
	public MoveLabel[] moveWhitelist = new MoveLabel[0];

	// Token: 0x04001A0E RID: 6670
	public CharacterAnimation floatForwardAnim = new CharacterAnimation();

	// Token: 0x04001A0F RID: 6671
	public CharacterAnimation floatBackAnim = new CharacterAnimation();

	// Token: 0x04001A10 RID: 6672
	public CharacterAnimation floatForwardToBackAnim = new CharacterAnimation();

	// Token: 0x04001A11 RID: 6673
	public CharacterAnimation floatBackToForwardAnim = new CharacterAnimation();

	// Token: 0x04001A12 RID: 6674
	public Fixed verticalAcceleration;

	// Token: 0x04001A13 RID: 6675
	public Fixed verticalFriction;

	// Token: 0x04001A14 RID: 6676
	public Fixed verticalMaxSpeed;

	// Token: 0x04001A15 RID: 6677
	public Fixed maxRollAngle;

	// Token: 0x04001A16 RID: 6678
	public bool deferEndUntilAerialMoveComplete;

	// Token: 0x04001A17 RID: 6679
	public bool allowSpecialInputCancel;

	// Token: 0x04001A18 RID: 6680
	public bool allowJumpCancel;

	// Token: 0x04001A19 RID: 6681
	public bool allowFastFallCancel;

	// Token: 0x04001A1A RID: 6682
	public bool allowFallThroughPlatform;

	// Token: 0x04001A1B RID: 6683
	public List<ParticleData> loopParticles = new List<ParticleData>();

	// Token: 0x04001A1C RID: 6684
	public AudioData loopAudio = default(AudioData);
}
