using System;
using UnityEngine;

// Token: 0x02000927 RID: 2343
public class DebugCircle : MonoBehaviour
{
	// Token: 0x06003D21 RID: 15649 RVA: 0x0011AA0A File Offset: 0x00118E0A
	public void Init(Vector2 center, float radius, Color color, int lifespanFrames)
	{
		this.center = center;
		this.radius = radius;
		this.color = color;
		this.lifetime = (float)lifespanFrames * WTime.frameTime;
	}

	// Token: 0x06003D22 RID: 15650 RVA: 0x0011AA30 File Offset: 0x00118E30
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

	// Token: 0x06003D23 RID: 15651 RVA: 0x0011AA6F File Offset: 0x00118E6F
	private void OnDrawGizmos()
	{
		GizmoUtil.GizmosDrawCircle(this.center, this.radius, this.color, 10);
	}

	// Token: 0x040029BC RID: 10684
	private Vector2 center;

	// Token: 0x040029BD RID: 10685
	private float radius;

	// Token: 0x040029BE RID: 10686
	private Color color;

	// Token: 0x040029BF RID: 10687
	private float lifetime;
}
