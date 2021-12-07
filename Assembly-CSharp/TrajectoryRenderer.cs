using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000460 RID: 1120
public class TrajectoryRenderer : DebugEffect
{
	// Token: 0x06001726 RID: 5926 RVA: 0x0007D140 File Offset: 0x0007B540
	public void Init(IPlayerDelegate character, IPhysicsDelegate physicsDelegate, PhysicsModel templateModel, PlayerPhysicsModel playerTemplateModel, PhysicsSimulator globalPhysics, int frames, Color color)
	{
		base.Init(frames, null, 1f);
		this.character = character;
		this.phsyicsModel = new PhysicsModel();
		templateModel.CopyTo(this.phsyicsModel);
		this.playerModel = new PlayerPhysicsModel();
		this.playerModel.Load(playerTemplateModel);
		this.context = character.Physics.CreateContext(this.phsyicsModel, this.playerModel, physicsDelegate, false);
		for (int i = 0; i < frames; i++)
		{
			globalPhysics.AdvanceState(this.context, 1);
			if (i % this.GRANULARITY == 0)
			{
				this.predictedPoints.Add((Vector3)this.phsyicsModel.position + (Vector3)this.context.model.bounds.centerOffset);
			}
		}
		this.lineMaterial = (Resources.Load("Game/Effects/TrajectoryRendererMaterial") as Material);
		if (this.lineMaterial != null)
		{
			this.lineRenderer = base.gameObject.AddComponent<LineRenderer>();
			this.lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
			this.lineRenderer.receiveShadows = false;
			Color red = Color.red;
			this.lineRenderer.startColor = color;
			this.lineRenderer.endColor = red;
			this.lineRenderer.startWidth = 0.5f;
			this.lineRenderer.endWidth = 0f;
			this.lineRenderer.material = this.lineMaterial;
			this.lineRenderer.useWorldSpace = true;
		}
	}

	// Token: 0x06001727 RID: 5927 RVA: 0x0007D2C4 File Offset: 0x0007B6C4
	public override void TickFrame()
	{
		if (this.character.State.IsHitLagPaused)
		{
			return;
		}
		base.TickFrame();
		if (!this.lineRenderer.enabled)
		{
			return;
		}
		if (!this.character.State.IsHitLagPaused)
		{
			this.predictedPoints.RemoveAt(0);
		}
		if (this.predictedPoints.Count > 1)
		{
			this.lineRenderer.enabled = true;
			this.lineRenderer.positionCount = this.predictedPoints.Count;
			this.lineRenderer.SetPositions(this.predictedPoints.ToArray());
		}
		else
		{
			this.lineRenderer.enabled = false;
		}
	}

	// Token: 0x040011ED RID: 4589
	private PhysicsContext context;

	// Token: 0x040011EE RID: 4590
	[AllowCachedState]
	private PhysicsModel phsyicsModel;

	// Token: 0x040011EF RID: 4591
	[AllowCachedState]
	private PlayerPhysicsModel playerModel;

	// Token: 0x040011F0 RID: 4592
	private List<Vector3> predictedPoints = new List<Vector3>();

	// Token: 0x040011F1 RID: 4593
	private int GRANULARITY = 1;

	// Token: 0x040011F2 RID: 4594
	private Material lineMaterial;

	// Token: 0x040011F3 RID: 4595
	private LineRenderer lineRenderer;

	// Token: 0x040011F4 RID: 4596
	private IPlayerDelegate character;
}
