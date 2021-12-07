using System;
using FixedPoint;
using UnityEngine;

// Token: 0x020004C7 RID: 1223
public class AimReticleComponent : MoveComponent, IMoveTickGameFrameComponent, IMoveEndComponent, IMoveStartComponent, IRollbackStateOwner
{
	// Token: 0x170005C0 RID: 1472
	// (get) Token: 0x06001B09 RID: 6921 RVA: 0x0008A07E File Offset: 0x0008847E
	// (set) Token: 0x06001B0A RID: 6922 RVA: 0x0008A086 File Offset: 0x00088486
	public MoveArticleSpawnCalculator articleSpawnController { get; set; }

	// Token: 0x06001B0B RID: 6923 RVA: 0x0008A08F File Offset: 0x0008848F
	public override void Init(IMoveDelegate moveDelegate, IPlayerDelegate playerDelegate, InputButtonsData input)
	{
		base.Init(moveDelegate, playerDelegate, input);
		this.model = new AimReticleComponentModel();
	}

	// Token: 0x06001B0C RID: 6924 RVA: 0x0008A0A5 File Offset: 0x000884A5
	public void OnStart(IPlayerDelegate player, InputButtonsData input)
	{
		this.checkCreateOrDestroy();
		this.updateVfx(input);
	}

	// Token: 0x06001B0D RID: 6925 RVA: 0x0008A0B4 File Offset: 0x000884B4
	public void TickGameFrame(InputButtonsData input)
	{
		this.checkCreateOrDestroy();
		this.updateVfx(input);
	}

	// Token: 0x06001B0E RID: 6926 RVA: 0x0008A0C4 File Offset: 0x000884C4
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

	// Token: 0x06001B0F RID: 6927 RVA: 0x0008A1CC File Offset: 0x000885CC
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

	// Token: 0x06001B10 RID: 6928 RVA: 0x0008A290 File Offset: 0x00088690
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

	// Token: 0x06001B11 RID: 6929 RVA: 0x0008A2C9 File Offset: 0x000886C9
	public void OnEnd()
	{
		if (this.model.vfx != null)
		{
			this.model.vfx.EffectController.Destroy();
			this.model.vfx = null;
		}
	}

	// Token: 0x06001B12 RID: 6930 RVA: 0x0008A2FC File Offset: 0x000886FC
	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(base.rollbackStatePooling.Clone<AimReticleComponentModel>(this.model));
	}

	// Token: 0x06001B13 RID: 6931 RVA: 0x0008A316 File Offset: 0x00088716
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<AimReticleComponentModel>(ref this.model);
		return true;
	}

	// Token: 0x06001B14 RID: 6932 RVA: 0x0008A326 File Offset: 0x00088726
	public override void RegisterPreload(PreloadContext context)
	{
		context.Register(new PreloadDef(this.prefab, PreloadType.EFFECT), 4);
	}

	// Token: 0x04001455 RID: 5205
	public GameObject prefab;

	// Token: 0x04001456 RID: 5206
	public int startFrame;

	// Token: 0x04001457 RID: 5207
	public int endFrame;

	// Token: 0x04001458 RID: 5208
	public float radius;

	// Token: 0x04001459 RID: 5209
	private AimReticleComponentModel model;
}
