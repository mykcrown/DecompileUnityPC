// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class BoundsRect : MonoBehaviour
{
	public enum BoundsType
	{
		None,
		Camera,
		BlastZone
	}

	public Vector2 dimensions = new Vector2(10f, 10f);

	public BoundsRect.BoundsType boundsType;

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

	private void OnDrawGizmos()
	{
		GizmoUtil.GizmosDrawRectangle(new Rect(base.gameObject.transform.position, new Vector2(this.dimensions.x, -this.dimensions.y)), Color.red, false);
	}
}
