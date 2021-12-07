using System;

// Token: 0x0200049D RID: 1181
[Serializable]
public class TeamAbilityConfig
{
	// Token: 0x060019FB RID: 6651 RVA: 0x00085FC0 File Offset: 0x000843C0
	public void RegisterPreload(PreloadContext context)
	{
		if (this.dynamicMoveParticle != null)
		{
			this.dynamicMoveParticle.RegisterPreload(context);
		}
		if (this.powerMoveParticle != null)
		{
			this.powerMoveParticle.RegisterPreload(context);
		}
		foreach (CreateArticleAction createArticleAction in this.assistArticles)
		{
			if (createArticleAction != null)
			{
				createArticleAction.RegisterPreload(context);
			}
		}
	}

	// Token: 0x04001354 RID: 4948
	public ParticleData dynamicMoveParticle = new ParticleData();

	// Token: 0x04001355 RID: 4949
	public ParticleData powerMoveParticle = new ParticleData();

	// Token: 0x04001356 RID: 4950
	public CreateArticleAction[] assistArticles = new CreateArticleAction[0];

	// Token: 0x04001357 RID: 4951
	public int powerMoveCooldownFrames = 300;

	// Token: 0x04001358 RID: 4952
	public int dynamicMoveCooldownFrames = 300;

	// Token: 0x04001359 RID: 4953
	public int powerMoveImmuneFrames = 12;

	// Token: 0x0400135A RID: 4954
	[NonSerialized]
	public bool toggleArticles;
}
