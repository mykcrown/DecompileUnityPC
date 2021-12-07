using System;
using UnityEngine;

// Token: 0x0200062A RID: 1578
public class AnimationBehaviour : StageBehaviour
{
	// Token: 0x060026DC RID: 9948 RVA: 0x000BE294 File Offset: 0x000BC694
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

	// Token: 0x060026DD RID: 9949 RVA: 0x000BE2EB File Offset: 0x000BC6EB
	public override void Play(object context)
	{
		if (this.stageAnimation != null)
		{
			this.stageAnimation.PlayClip(this.AnimationClip);
		}
	}

	// Token: 0x04001C62 RID: 7266
	public AnimationClip AnimationClip;

	// Token: 0x04001C63 RID: 7267
	public GameObject TargetGameObject;

	// Token: 0x04001C64 RID: 7268
	[SerializeField]
	private StageAnimation stageAnimation;
}
