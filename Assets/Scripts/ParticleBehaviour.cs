// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class ParticleBehaviour : StageBehaviour
{
	public enum ParticleBehaviourType
	{
		Play,
		Stop,
		PlayForTime
	}

	[SerializeField]
	public StageParticleSystem StageParticleSystem;

	public ParticleBehaviour.ParticleBehaviourType Type;

	public bool ClearAndReset;

	public int FrameDuration;

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
}
