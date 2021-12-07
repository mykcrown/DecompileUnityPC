// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PointListRenderer : DebugEffect
{
	private Material lineMaterial;

	private LineRenderer lineRenderer;

	public void Init(List<Vector3> points, Color color, Color endColor, int lifespanFrames, float startWidth = 0.5f, float endWidth = 0f)
	{
		this.createRenderer(points.ToArray(), color, endColor, startWidth, endWidth);
		this.model.lifespanFrames = lifespanFrames;
		this.model.isDead = false;
	}

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
}
