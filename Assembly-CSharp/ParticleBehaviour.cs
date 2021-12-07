using System;
using UnityEngine;

// Token: 0x02000632 RID: 1586
public class ParticleBehaviour : StageBehaviour
{
	// Token: 0x060026F8 RID: 9976 RVA: 0x000BE99C File Offset: 0x000BCD9C
	public override void Play(object context)
	{
		if (this.StageParticleSystem != null)
		{
			ParticleBehaviour.ParticleBehaviourType type = this.Type;
			if (type != ParticleBehaviour.ParticleBehaviourType.Play)
			{
				if (type != ParticleBehaviour.ParticleBehaviourType.Stop)
				{
					if (type == ParticleBehaviour.ParticleBehaviourType.PlayForTime)
					{
						this.StageParticleSystem.PlayForFrames(this.ClearAndReset, this.FrameDuration);
					}
				}
				else
				{
					this.StageParticleSystem.Stop(this.ClearAndReset);
				}
			}
			else
			{
				this.StageParticleSystem.Play(this.ClearAndReset);
			}
		}
	}

	// Token: 0x04001C7D RID: 7293
	[SerializeField]
	public StageParticleSystem StageParticleSystem;

	// Token: 0x04001C7E RID: 7294
	public ParticleBehaviour.ParticleBehaviourType Type;

	// Token: 0x04001C7F RID: 7295
	public bool ClearAndReset;

	// Token: 0x04001C80 RID: 7296
	public int FrameDuration;

	// Token: 0x02000633 RID: 1587
	public enum ParticleBehaviourType
	{
		// Token: 0x04001C82 RID: 7298
		Play,
		// Token: 0x04001C83 RID: 7299
		Stop,
		// Token: 0x04001C84 RID: 7300
		PlayForTime
	}
}
