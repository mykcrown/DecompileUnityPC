using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000459 RID: 1113
public class PointListRenderer : DebugEffect
{
	// Token: 0x06001703 RID: 5891 RVA: 0x0007C794 File Offset: 0x0007AB94
	public void Init(List<Vector3> points, Color color, Color endColor, int lifespanFrames, float startWidth = 0.5f, float endWidth = 0f)
	{
		this.createRenderer(points.ToArray(), color, endColor, startWidth, endWidth);
		this.model.lifespanFrames = lifespanFrames;
		this.model.isDead = false;
	}

	// Token: 0x06001704 RID: 5892 RVA: 0x0007C7C4 File Offset: 0x0007ABC4
	private void createRenderer(Vector3[] points, Color color, Color endColor, float startWidth, float endWidth)
	{
		this.lineMaterial = (Resources.Load("Game/Effects/TrajectoryRendererMaterial") as Material);
		if (this.lineMaterial != null)
		{
			this.lineRenderer = base.gameObject.AddComponent<LineRenderer>();
			this.lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
			this.lineRenderer.receiveShadows = false;
			this.lineRenderer.startColor = color;
			this.lineRenderer.endColor = endColor;
			this.lineRenderer.startWidth = startWidth;
			this.lineRenderer.endWidth = endWidth;
			this.lineRenderer.material = this.lineMaterial;
			this.lineRenderer.useWorldSpace = true;
			this.lineRenderer.enabled = true;
			this.lineRenderer.positionCount = points.Length;
			this.lineRenderer.SetPositions(points);
		}
	}

	// Token: 0x040011CC RID: 4556
	private Material lineMaterial;

	// Token: 0x040011CD RID: 4557
	private LineRenderer lineRenderer;
}
