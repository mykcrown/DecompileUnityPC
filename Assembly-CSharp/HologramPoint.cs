using System;
using UnityEngine;

// Token: 0x0200047C RID: 1148
public class HologramPoint : MonoBehaviour
{
	// Token: 0x060018DC RID: 6364 RVA: 0x00082EC8 File Offset: 0x000812C8
	private void OnDrawGizmos()
	{
		Color color = Color.yellow;
		if (this.IsInvalid)
		{
			color = Color.red;
		}
		GizmoUtil.GizmosDrawSphere(base.transform.position, 0.1f, color);
	}

	// Token: 0x040012BE RID: 4798
	public bool IsInvalid;
}
