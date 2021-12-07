// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class DebugCircle : MonoBehaviour
{
	private Vector2 center;

	private float radius;

	private Color color;

	private float lifetime;

	public void Init(Vector2 center, float radius, Color color, int lifespanFrames)
	{
		this.center = center;
		this.radius = radius;
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
		GizmoUtil.GizmosDrawCircle(this.center, this.radius, this.color, 10);
	}
}
