// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class HologramPoint : MonoBehaviour
{
	public bool IsInvalid;

	private void OnDrawGizmos()
	{
		Color color = Color.yellow;
		if (this.IsInvalid)
		{
			color = Color.red;
		}
		GizmoUtil.GizmosDrawSphere(base.transform.position, 0.1f, color);
	}
}
