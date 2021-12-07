using System;
using UnityEngine;

// Token: 0x02000B89 RID: 2953
public class VFXLineRenderer : VFXBehavior
{
	// Token: 0x0600553F RID: 21823 RVA: 0x001B4F1B File Offset: 0x001B331B
	public override void OnVFXInit()
	{
	}

	// Token: 0x06005540 RID: 21824 RVA: 0x001B4F1D File Offset: 0x001B331D
	public override void OnVFXStart()
	{
	}

	// Token: 0x06005541 RID: 21825 RVA: 0x001B4F1F File Offset: 0x001B331F
	public void SetTargets(Transform newTargetA, Transform newTargetB)
	{
		this.targetA = newTargetA;
		this.targetB = newTargetB;
		this.lineRenderer.enabled = true;
		this.updateLineRenderer();
	}

	// Token: 0x06005542 RID: 21826 RVA: 0x001B4F44 File Offset: 0x001B3344
	public void Awake()
	{
		this.lineRenderer = base.GetComponent<LineRenderer>();
		this.lineRenderer.enabled = false;
		int num = this.lineRenderer.widthCurve.keys.Length + 1;
		int num2 = num + 1;
		this.vertexBuffer = new Vector3[num2];
		this.lineRenderer.positionCount = num2;
	}

	// Token: 0x06005543 RID: 21827 RVA: 0x001B4F9A File Offset: 0x001B339A
	public void Update()
	{
		this.updateLineRenderer();
	}

	// Token: 0x06005544 RID: 21828 RVA: 0x001B4FA4 File Offset: 0x001B33A4
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

	// Token: 0x04003628 RID: 13864
	public Vector3 lineRendererOffset = Vector3.zero;

	// Token: 0x04003629 RID: 13865
	private LineRenderer lineRenderer;

	// Token: 0x0400362A RID: 13866
	private Vector3[] vertexBuffer;

	// Token: 0x0400362B RID: 13867
	private Transform targetA;

	// Token: 0x0400362C RID: 13868
	private Transform targetB;
}
