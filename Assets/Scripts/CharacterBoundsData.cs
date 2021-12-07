// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

[Serializable]
public class CharacterBoundsData
{
	public LedgeGrabBox ledgeGrabBox;

	public Vector3F ledgeReleaseOffset = new Vector3F(-(Fixed)0.5, -(Fixed)1.5);

	[SerializeField]
	public FixedRect CameraBoxData = new FixedRect(-2, 3, 6, 3);

	public Vector3 floatyUIOffset = Vector3.zero;

	public Vector3F rotationCenterOffset = new Vector3F(0, 1);

	public void DrawGizmos(Vector3 position, HorizontalDirection facing)
	{
		if (DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Bounds))
		{
			FixedRect rect = this.ledgeGrabBox.rect;
			FixedRect cameraBoxData = this.CameraBoxData;
			if (facing == HorizontalDirection.Left)
			{
				rect.position.x = -rect.position.x - rect.Width;
				cameraBoxData.position.x = -cameraBoxData.position.x - cameraBoxData.Width;
			}
			GizmoUtil.GizmosDrawRectangle(rect, (Vector3F)position, Color.red, false);
			GizmoUtil.GizmosDrawRectangle(cameraBoxData, (Vector3F)position, Color.white, false);
		}
	}
}
