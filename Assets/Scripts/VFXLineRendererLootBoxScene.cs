// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class VFXLineRendererLootBoxScene : VFXBehavior
{
	public Vector3 lineRendererOffset = Vector3.zero;

	private LineRenderer lineRenderer;

	private Keyframe[] keysCache;

	private Vector3[] vertexBuffer;

	public Transform TargetA;

	public Transform TargetB;

	public override void OnVFXInit()
	{
	}

	public override void OnVFXStart()
	{
	}

	public void SetTargets(Transform newTargetA, Transform newTargetB)
	{
		this.TargetA = newTargetA;
		this.TargetB = newTargetB;
		this.lineRenderer.enabled = true;
		this.updateLineRenderer();
	}

	public void Awake()
	{
		this.lineRenderer = base.GetComponent<LineRenderer>();
		int num = this.lineRenderer.widthCurve.keys.Length + 1;
		int num2 = num + 1;
		this.vertexBuffer = new Vector3[num2];
		this.lineRenderer.positionCount = num2;
		this.keysCache = this.lineRenderer.widthCurve.keys;
	}

	public void Update()
	{
		this.updateLineRenderer();
	}

	private void updateLineRenderer()
	{
		if (this.TargetA != null && this.TargetB != null)
		{
			Vector3 vector = this.TargetA.position + this.lineRendererOffset;
			Vector3 vector2 = this.TargetB.position - this.TargetA.position;
			this.vertexBuffer[0] = vector;
			this.vertexBuffer[this.vertexBuffer.Length - 1] = vector + vector2;
			for (int i = 0; i < this.keysCache.Length; i++)
			{
				float time = this.keysCache[i].time;
				this.vertexBuffer[i + 1] = vector + vector2 * time;
			}
			this.lineRenderer.SetPositions(this.vertexBuffer);
		}
	}
}
