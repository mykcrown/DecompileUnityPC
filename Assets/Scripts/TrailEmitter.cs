// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public class TrailEmitter : MonoBehaviour, ITickable, IRollbackStateOwner, IExpirable
{
	private TrailEmitterModel model = new TrailEmitterModel();

	private ITrailOwner emitter;

	private int order;

	private static int TRAIL_POOL_SIZE = 30;

	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	private TrailEmitterData activeData
	{
		get
		{
			return (!(this.model.overrideData == null)) ? this.model.overrideData : this.model.defaultData;
		}
	}

	public virtual bool IsExpired
	{
		get
		{
			return this.model.isDead;
		}
	}

	public void Init(ITrailOwner emitter, TrailEmitterData data)
	{
		this.emitter = emitter;
		this.model.defaultData = data;
	}

	public void LoadEmitterData(TrailEmitterData data)
	{
		this.model.overrideData = UnityEngine.Object.Instantiate<TrailEmitterData>(data);
		this.model.overrideData.transform.SetParent(base.transform);
	}

	public void ResetData()
	{
		this.model.overrideData = null;
	}

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

	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.rollbackStatePooling.Clone<TrailEmitterModel>(this.model));
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<TrailEmitterModel>(ref this.model);
		return true;
	}

	private void emitTrail(Vector3 atPosition)
	{
		Effect effect = this.gameController.currentGame.DynamicObjects.InstantiateDynamicObject<Effect>(this.activeData.particlePrefab, TrailEmitter.TRAIL_POOL_SIZE, true);
		effect.transform.position = atPosition;
		effect.Init(this.activeData.particleLifespanFrames, null, 1f);
		this.order++;
		effect.particleSystemRenderer.sortingOrder = this.order;
	}

	public void Kill()
	{
		this.model.isDead = true;
	}
}
