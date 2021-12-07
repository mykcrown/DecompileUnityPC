// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TrajectoryRenderer : DebugEffect
{
	private PhysicsContext context;

	[AllowCachedState]
	private PhysicsModel phsyicsModel;

	[AllowCachedState]
	private PlayerPhysicsModel playerModel;

	private List<Vector3> predictedPoints = new List<Vector3>();

	private int GRANULARITY = 1;

	private Material lineMaterial;

	private LineRenderer lineRenderer;

	private IPlayerDelegate character;

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
}
