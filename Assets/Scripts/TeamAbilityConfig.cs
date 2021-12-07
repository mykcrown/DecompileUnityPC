// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class TeamAbilityConfig
{
	public ParticleData dynamicMoveParticle = new ParticleData();

	public ParticleData powerMoveParticle = new ParticleData();

	public CreateArticleAction[] assistArticles = new CreateArticleAction[0];

	public int powerMoveCooldownFrames = 300;

	public int dynamicMoveCooldownFrames = 300;

	public int powerMoveImmuneFrames = 12;

	[NonSerialized]
	public bool toggleArticles;

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
		CreateArticleAction[] array = this.assistArticles;
		for (int i = 0; i < array.Length; i++)
		{
			CreateArticleAction createArticleAction = array[i];
			if (createArticleAction != null)
			{
				createArticleAction.RegisterPreload(context);
			}
		}
	}
}
