using System;
using FixedPoint;
using UnityEngine;

// Token: 0x0200057E RID: 1406
[Serializable]
public class CharacterBoundsData
{
	// Token: 0x06001F95 RID: 8085 RVA: 0x000A10AC File Offset: 0x0009F4AC
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

	// Token: 0x040018F6 RID: 6390
	public LedgeGrabBox ledgeGrabBox;

	// Token: 0x040018F7 RID: 6391
	public Vector3F ledgeReleaseOffset = new Vector3F(-(Fixed)0.5, -(Fixed)1.5);

	// Token: 0x040018F8 RID: 6392
	[SerializeField]
	public FixedRect CameraBoxData = new FixedRect(-2, 3, 6, 3);

	// Token: 0x040018F9 RID: 6393
	public Vector3 floatyUIOffset = Vector3.zero;

	// Token: 0x040018FA RID: 6394
	public Vector3F rotationCenterOffset = new Vector3F(0, 1);
}
