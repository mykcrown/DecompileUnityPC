// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class AnimationBehaviour : StageBehaviour
{
	public AnimationClip AnimationClip;

	public GameObject TargetGameObject;

	[SerializeField]
	private StageAnimation stageAnimation;

	public override void Awake()
	{
		base.Awake();
		if (this.TargetGameObject != null)
		{
			this.stageAnimation = this.TargetGameObject.GetComponent<StageAnimation>();
			if (this.stageAnimation == null)
			{
				this.stageAnimation = this.TargetGameObject.AddComponent<StageAnimation>();
			}
		}
	}

	public override void Play(object context)
	{
		if (this.stageAnimation != null)
		{
			this.stageAnimation.PlayClip(this.AnimationClip);
		}
	}
}
