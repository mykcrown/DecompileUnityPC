using System;
using FixedPoint;
using UnityEngine;

// Token: 0x0200045E RID: 1118
public class TrailEmitter : MonoBehaviour, ITickable, IRollbackStateOwner, IExpirable
{
	// Token: 0x17000478 RID: 1144
	// (get) Token: 0x06001714 RID: 5908 RVA: 0x0007CE27 File Offset: 0x0007B227
	// (set) Token: 0x06001715 RID: 5909 RVA: 0x0007CE2F File Offset: 0x0007B22F
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x17000479 RID: 1145
	// (get) Token: 0x06001716 RID: 5910 RVA: 0x0007CE38 File Offset: 0x0007B238
	// (set) Token: 0x06001717 RID: 5911 RVA: 0x0007CE40 File Offset: 0x0007B240
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x06001718 RID: 5912 RVA: 0x0007CE49 File Offset: 0x0007B249
	public void Init(ITrailOwner emitter, TrailEmitterData data)
	{
		this.emitter = emitter;
		this.model.defaultData = data;
	}

	// Token: 0x06001719 RID: 5913 RVA: 0x0007CE5E File Offset: 0x0007B25E
	public void LoadEmitterData(TrailEmitterData data)
	{
		this.model.overrideData = UnityEngine.Object.Instantiate<TrailEmitterData>(data);
		this.model.overrideData.transform.SetParent(base.transform);
	}

	// Token: 0x0600171A RID: 5914 RVA: 0x0007CE8C File Offset: 0x0007B28C
	public void ResetData()
	{
		this.model.overrideData = null;
	}

	// Token: 0x1700047A RID: 1146
	// (get) Token: 0x0600171B RID: 5915 RVA: 0x0007CE9A File Offset: 0x0007B29A
	private TrailEmitterData activeData
	{
		get
		{
			return (!(this.model.overrideData == null)) ? this.model.overrideData : this.model.defaultData;
		}
	}

	// Token: 0x0600171C RID: 5916 RVA: 0x0007CED0 File Offset: 0x0007B2D0
	public void TickFrame()
	{
		if (this.emitter == null || this.model.isDead)
		{
			return;
		}
		if (!this.emitter.EmitTrail)
		{
			this.model.lastEmitPosition = this.emitter.EmitPosition;
		}
		else
		{
			if (this.activeData != null)
			{
				if (this.activeData.distanceMode)
				{
					Vector2F vector2F = this.emitter.EmitPosition - this.model.lastEmitPosition;
					Fixed magnitude = vector2F.magnitude;
					int num = FixedMath.Floor(magnitude / (Fixed)((double)this.activeData.emitDistance));
					for (int i = 0; i < num; i++)
					{
						this.model.lastEmitPosition += (Fixed)((double)this.activeData.emitDistance) * vector2F.normalized;
						this.emitTrail((Vector3)this.model.lastEmitPosition);
					}
				}
				else if (this.model.frameCount % this.activeData.emitFrequencyFrames == 0)
				{
					this.emitTrail((Vector3)this.emitter.EmitPosition);
				}
			}
			this.model.frameCount++;
		}
	}

	// Token: 0x0600171D RID: 5917 RVA: 0x0007D035 File Offset: 0x0007B435
	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.rollbackStatePooling.Clone<TrailEmitterModel>(this.model));
	}

	// Token: 0x0600171E RID: 5918 RVA: 0x0007D04F File Offset: 0x0007B44F
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<TrailEmitterModel>(ref this.model);
		return true;
	}

	// Token: 0x0600171F RID: 5919 RVA: 0x0007D060 File Offset: 0x0007B460
	private void emitTrail(Vector3 atPosition)
	{
		Effect effect = this.gameController.currentGame.DynamicObjects.InstantiateDynamicObject<Effect>(this.activeData.particlePrefab, TrailEmitter.TRAIL_POOL_SIZE, true);
		effect.transform.position = atPosition;
		effect.Init(this.activeData.particleLifespanFrames, null, 1f);
		this.order++;
		effect.particleSystemRenderer.sortingOrder = this.order;
	}

	// Token: 0x06001720 RID: 5920 RVA: 0x0007D0D6 File Offset: 0x0007B4D6
	public void Kill()
	{
		this.model.isDead = true;
	}

	// Token: 0x1700047B RID: 1147
	// (get) Token: 0x06001721 RID: 5921 RVA: 0x0007D0E4 File Offset: 0x0007B4E4
	public virtual bool IsExpired
	{
		get
		{
			return this.model.isDead;
		}
	}

	// Token: 0x040011E3 RID: 4579
	private TrailEmitterModel model = new TrailEmitterModel();

	// Token: 0x040011E4 RID: 4580
	private ITrailOwner emitter;

	// Token: 0x040011E5 RID: 4581
	private int order;

	// Token: 0x040011E6 RID: 4582
	private static int TRAIL_POOL_SIZE = 30;
}
