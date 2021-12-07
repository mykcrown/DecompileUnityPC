using System;
using UnityEngine;

// Token: 0x02000B8A RID: 2954
public class VFXLineRendererLootBoxScene : VFXBehavior
{
	// Token: 0x06005546 RID: 21830 RVA: 0x001B50BC File Offset: 0x001B34BC
	public override void OnVFXInit()
	{
	}

	// Token: 0x06005547 RID: 21831 RVA: 0x001B50BE File Offset: 0x001B34BE
	public override void OnVFXStart()
	{
	}

	// Token: 0x06005548 RID: 21832 RVA: 0x001B50C0 File Offset: 0x001B34C0
	public void SetTargets(Transform newTargetA, Transform newTargetB)
	{
		this.TargetA = newTargetA;
		this.TargetB = newTargetB;
		this.lineRenderer.enabled = true;
		this.updateLineRenderer();
	}

	// Token: 0x06005549 RID: 21833 RVA: 0x001B50E4 File Offset: 0x001B34E4
	public void Awake()
	{
		this.lineRenderer = base.GetComponent<LineRenderer>();
		int num = this.lineRenderer.widthCurve.keys.Length + 1;
		int num2 = num + 1;
		this.vertexBuffer = new Vector3[num2];
		this.lineRenderer.positionCount = num2;
		this.keysCache = this.lineRenderer.widthCurve.keys;
	}

	// Token: 0x0600554A RID: 21834 RVA: 0x001B5144 File Offset: 0x001B3544
	public void Update()
	{
		this.updateLineRenderer();
	}

	// Token: 0x0600554B RID: 21835 RVA: 0x001B514C File Offset: 0x001B354C
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

	// Token: 0x0400362D RID: 13869
	public Vector3 lineRendererOffset = Vector3.zero;

	// Token: 0x0400362E RID: 13870
	private LineRenderer lineRenderer;

	// Token: 0x0400362F RID: 13871
	private Keyframe[] keysCache;

	// Token: 0x04003630 RID: 13872
	private Vector3[] vertexBuffer;

	// Token: 0x04003631 RID: 13873
	public Transform TargetA;

	// Token: 0x04003632 RID: 13874
	public Transform TargetB;
}
