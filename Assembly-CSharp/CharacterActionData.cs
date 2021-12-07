using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020004F3 RID: 1267
[Serializable]
public class CharacterActionData
{
	// Token: 0x06001BA3 RID: 7075 RVA: 0x0008BEEC File Offset: 0x0008A2EC
	public CharacterActionData(ActionState actionState, string name)
	{
		this.characterActionState = actionState;
		this.name = name;
	}

	// Token: 0x06001BA4 RID: 7076 RVA: 0x0008BF3C File Offset: 0x0008A33C
	public CharacterActionData()
	{
	}

	// Token: 0x170005DA RID: 1498
	// (get) Token: 0x06001BA5 RID: 7077 RVA: 0x0008BF73 File Offset: 0x0008A373
	public string LeftAnimationName
	{
		get
		{
			return this.name + "_left";
		}
	}

	// Token: 0x170005DB RID: 1499
	// (get) Token: 0x06001BA6 RID: 7078 RVA: 0x0008BF85 File Offset: 0x0008A385
	public float animationDuration
	{
		get
		{
			if (this.animationSpeed == 0f || this.animation == null)
			{
				return 0f;
			}
			return this.animation.length / this.animationSpeed;
		}
	}

	// Token: 0x170005DC RID: 1500
	// (get) Token: 0x06001BA7 RID: 7079 RVA: 0x0008BFC0 File Offset: 0x0008A3C0
	public int frameDuration
	{
		get
		{
			return (int)(this.animationDuration * 60f);
		}
	}

	// Token: 0x0400154C RID: 5452
	[FormerlySerializedAs("clip1")]
	public AnimationClip animation;

	// Token: 0x0400154D RID: 5453
	public AnimationClip leftAnimation;

	// Token: 0x0400154E RID: 5454
	public WrapMode wrapMode;

	// Token: 0x0400154F RID: 5455
	public float animationSpeed = 1f;

	// Token: 0x04001550 RID: 5456
	public int skipFrames;

	// Token: 0x04001551 RID: 5457
	public SoundEffect[] soundEffects = new SoundEffect[0];

	// Token: 0x04001552 RID: 5458
	[FormerlySerializedAs("reference")]
	public ActionState characterActionState;

	// Token: 0x04001553 RID: 5459
	public string name;

	// Token: 0x04001554 RID: 5460
	public bool useRootTranslation;

	// Token: 0x04001555 RID: 5461
	public int interruptibleFrames;

	// Token: 0x04001556 RID: 5462
	public bool overrideBlendIn;

	// Token: 0x04001557 RID: 5463
	public bool overrideBlendOut;

	// Token: 0x04001558 RID: 5464
	public float blendInDuration;

	// Token: 0x04001559 RID: 5465
	public float blendOutDuration;

	// Token: 0x0400155A RID: 5466
	public bool readAnyBufferedInput;

	// Token: 0x0400155B RID: 5467
	public bool triggerHeldInputAsTap;

	// Token: 0x0400155C RID: 5468
	public MoveLabel[] validBufferedMoveLabels = new MoveLabel[0];

	// Token: 0x0400155D RID: 5469
	public ButtonPress[] maskedBufferButtons = new ButtonPress[0];
}
