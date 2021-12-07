using System;
using UnityEngine;

// Token: 0x02000AA1 RID: 2721
[Serializable]
public class BoundsRect : MonoBehaviour
{
	// Token: 0x06005000 RID: 20480 RVA: 0x0014DFCC File Offset: 0x0014C3CC
	private void OnDrawGizmos()
	{
		GizmoUtil.GizmosDrawRectangle(new Rect(base.gameObject.transform.position, new Vector2(this.dimensions.x, -this.dimensions.y)), Color.red, false);
	}

	// Token: 0x170012E1 RID: 4833
	// (get) Token: 0x06005001 RID: 20481 RVA: 0x0014E01A File Offset: 0x0014C41A
	// (set) Token: 0x06005002 RID: 20482 RVA: 0x0014E054 File Offset: 0x0014C454
	public Rect bounds
	{
		get
		{
			return new Rect(base.gameObject.transform.position, new Vector2(this.dimensions.x, this.dimensions.y));
		}
		set
		{
			base.transform.position = new Vector2(value.x, value.y);
			this.dimensions.x = value.width;
			this.dimensions.y = value.height;
		}
	}

	// Token: 0x040033A8 RID: 13224
	public Vector2 dimensions = new Vector2(10f, 10f);

	// Token: 0x040033A9 RID: 13225
	public BoundsRect.BoundsType boundsType;

	// Token: 0x02000AA2 RID: 2722
	public enum BoundsType
	{
		// Token: 0x040033AB RID: 13227
		None,
		// Token: 0x040033AC RID: 13228
		Camera,
		// Token: 0x040033AD RID: 13229
		BlastZone
	}
}
