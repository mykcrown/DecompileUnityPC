// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class DebugArrow : MonoBehaviour
{
	private Vector3 start;

	private Vector3 end;

	private Color color;

	private float lifetime;

	public void Init(Vector3 start, Vector3 end, Color color, int lifespanFrames)
	{
		this.start = start;
		this.end = end;
		this.color = color;
		this.lifetime = (float)lifespanFrames * WTime.frameTime;
	}

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

	private void OnDrawGizmos()
	{
		GizmoUtil.GizmosDrawArrow(this.start, this.end, this.color, false, 0f, 33f);
	}
}
