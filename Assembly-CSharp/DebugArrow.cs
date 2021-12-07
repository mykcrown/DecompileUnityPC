using System;
using UnityEngine;

// Token: 0x02000926 RID: 2342
public class DebugArrow : MonoBehaviour
{
	// Token: 0x06003D1D RID: 15645 RVA: 0x0011A979 File Offset: 0x00118D79
	public void Init(Vector3 start, Vector3 end, Color color, int lifespanFrames)
	{
		this.start = start;
		this.end = end;
		this.color = color;
		this.lifetime = (float)lifespanFrames * WTime.frameTime;
	}

	// Token: 0x06003D1E RID: 15646 RVA: 0x0011A99F File Offset: 0x00118D9F
	private void Update()
	{
		if (this.lifetime > 0f)
		{
			this.lifetime -= WTime.deltaTime;
			if (this.lifetime <= 0f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x06003D1F RID: 15647 RVA: 0x0011A9DE File Offset: 0x00118DDE
	private void OnDrawGizmos()
	{
		GizmoUtil.GizmosDrawArrow(this.start, this.end, this.color, false, 0f, 33f);
	}

	// Token: 0x040029B8 RID: 10680
	private Vector3 start;

	// Token: 0x040029B9 RID: 10681
	private Vector3 end;

	// Token: 0x040029BA RID: 10682
	private Color color;

	// Token: 0x040029BB RID: 10683
	private float lifetime;
}
