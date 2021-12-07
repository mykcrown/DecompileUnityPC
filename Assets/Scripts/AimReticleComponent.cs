// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public class AimReticleComponent : MoveComponent, IMoveTickGameFrameComponent, IMoveEndComponent, IMoveStartComponent, IRollbackStateOwner
{
	public GameObject prefab;

	public int startFrame;

	public int endFrame;

	public float radius;

	private AimReticleComponentModel model;

	public MoveArticleSpawnCalculator articleSpawnController
	{
		get;
		set;
	}

	public override void Init(IMoveDelegate moveDelegate, IPlayerDelegate playerDelegate, InputButtonsData input)
	{
		base.Init(moveDelegate, playerDelegate, input);
		this.model = new AimReticleComponentModel();
	}

	public void OnStart(IPlayerDelegate player, InputButtonsData input)
	{
		this.checkCreateOrDestroy();
		this.updateVfx(input);
	}

	public void TickGameFrame(InputButtonsData input)
	{
		this.checkCreateOrDestroy();
		this.updateVfx(input);
	}

	private void checkCreateOrDestroy()
	{
		if (this.model.vfx == null && this.moveDelegate.Model.internalFrame >= this.startFrame && this.moveDelegate.Model.internalFrame < this.endFrame)
		{
			ParticleData particleData = new ParticleData();
			particleData.prefab = this.prefab;
			particleData.particleFacing = ParticleFacing.Default;
			particleData.frames = -1;
			particleData.prewarm = true;
			particleData.prewarmFrames = Math.Max(this.moveDelegate.Model.gameFrame - this.startFrame, 0);
			this.model.vfx = this.playerDelegate.GameVFX.PlayParticle(particleData, false, TeamNum.None);
		}
		if (this.model.vfx != null && this.moveDelegate.Model.internalFrame >= this.endFrame)
		{
			this.model.vfx.EffectController.Destroy();
			this.model.vfx = null;
		}
	}

	private void updateVfx(InputButtonsData input)
	{
		if (this.model.vfx == null)
		{
			return;
		}
		CreateArticleAction nextArticle = this.getNextArticle();
		ArticleSpawnParameters articleSpawnParameters = this.articleSpawnController.Calculate(nextArticle, input, this.playerDelegate, this.moveDelegate);
		Vector3F velocity = articleSpawnParameters.velocity;
		velocity.Normalize();
		Vector3F v = articleSpawnParameters.sourcePosition + velocity * (Fixed)((double)this.radius);
		this.model.vfx.EffectObject.transform.position = (Vector3)v;
		this.model.vfx.EffectObject.transform.rotation = Quaternion.Euler(0f, 0f, (float)articleSpawnParameters.rotation);
	}

	private CreateArticleAction getNextArticle()
	{
		CreateArticleAction[] articles = this.moveDelegate.Data.articles;
		int num = 0;
		if (num >= articles.Length)
		{
			return null;
		}
		return articles[num];
	}

	public void OnEnd()
	{
		if (this.model.vfx != null)
		{
			this.model.vfx.EffectController.Destroy();
			this.model.vfx = null;
		}
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(base.rollbackStatePooling.Clone<AimReticleComponentModel>(this.model));
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<AimReticleComponentModel>(ref this.model);
		return true;
	}

	public override void RegisterPreload(PreloadContext context)
	{
		context.Register(new PreloadDef(this.prefab, PreloadType.EFFECT), 4);
	}
}
