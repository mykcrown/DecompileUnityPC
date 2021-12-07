// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class VFXLineRenderer : VFXBehavior
{
	public Vector3 lineRendererOffset = Vector3.zero;

	private LineRenderer lineRenderer;

	private Vector3[] vertexBuffer;

	private Transform targetA;

	private Transform targetB;

	public override void OnVFXInit()
	{
	}

	public override void OnVFXStart()
	{
	}

	public void SetTargets(Transform newTargetA, Transform newTargetB)
	{
		this.targetA = newTargetA;
		this.targetB = newTargetB;
		this.lineRenderer.enabled = true;
		this.updateLineRenderer();
	}

	public void Awake()
	{
		this.lineRenderer = base.GetComponent<LineRenderer>();
		this.lineRenderer.enabled = false;
		int num = this.lineRenderer.widthCurve.keys.Length + 1;
		int num2 = num + 1;
		this.vertexBuffer = new Vector3[num2];
		this.lineRenderer.positionCount = num2;
	}

	public void Update()
	{
		this.updateLineRenderer();
	}

	private void updateLineRenderer()
	{
		if (this.targetA != null && this.targetB != null)
		{
			Vector3 vector = this.targetA.position + this.lineRendererOffset;
			Vector3 vector2 = this.targetB.position - this.targetA.position;
			this.vertexBuffer[0] = vector;
			this.vertexBuffer[this.vertexBuffer.Length - 1] = vector + vector2;
			for (int i = 0; i < this.lineRenderer.widthCurve.keys.Length; i++)
			{
				float time = this.lineRenderer.widthCurve.keys[i].time;
				this.vertexBuffer[i + 1] = vector + vector2 * time;
			}
			this.lineRenderer.SetPositions(this.vertexBuffer);
		}
	}
}
