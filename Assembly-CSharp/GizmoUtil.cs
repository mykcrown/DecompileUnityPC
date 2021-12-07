using System;
using FixedPoint;
using UnityEngine;

// Token: 0x02000B05 RID: 2821
public class GizmoUtil
{
	// Token: 0x06005104 RID: 20740 RVA: 0x00150CB0 File Offset: 0x0014F0B0
	public static void GizmosDrawRectangle(Rect rect, Color color, bool invertHeight = false)
	{
		Vector3 point = new Vector3(rect.x, rect.y);
		Vector3 point2 = new Vector3(rect.x + rect.width, rect.y);
		Vector3 point3 = new Vector3(rect.x, rect.y + (float)((!invertHeight) ? 1 : -1) * rect.height);
		Vector3 point4 = new Vector3(rect.x + rect.width, rect.y + (float)((!invertHeight) ? 1 : -1) * rect.height);
		GizmoUtil.GizmosDrawQuadrilateral(point, point3, point4, point2, color);
	}

	// Token: 0x06005105 RID: 20741 RVA: 0x00150D5C File Offset: 0x0014F15C
	public static void GizmosDrawRectangle(Rect localRect, Vector3 offset, Color color, bool invertHeight = false)
	{
		Vector3 point = new Vector3(localRect.x + offset.x, localRect.y + offset.y);
		Vector3 point2 = new Vector3(localRect.x + offset.x + localRect.width, localRect.y + offset.y);
		Vector3 point3 = new Vector3(localRect.x + offset.x, localRect.y + offset.y + (float)((!invertHeight) ? 1 : -1) * localRect.height);
		Vector3 point4 = new Vector3(localRect.x + offset.x + localRect.width, localRect.y + offset.y + (float)((!invertHeight) ? 1 : -1) * localRect.height);
		GizmoUtil.GizmosDrawQuadrilateral(point, point3, point4, point2, color);
	}

	// Token: 0x06005106 RID: 20742 RVA: 0x00150E45 File Offset: 0x0014F245
	public static void GizmosDrawRectangle(FixedRect rect, Color color, bool invertHeight = false)
	{
		GizmoUtil.GizmosDrawQuadrilateral(rect.TopLeft, rect.BottomLeft, rect.BottomRight, rect.TopRight, color);
	}

	// Token: 0x06005107 RID: 20743 RVA: 0x00150E80 File Offset: 0x0014F280
	public static void GizmosDrawRectangle(FixedRect rect, Vector3F offset, Color color, bool invertHeight = false)
	{
		GizmoUtil.GizmosDrawQuadrilateral(rect.TopLeft + offset, rect.BottomLeft + offset, rect.BottomRight + offset, rect.TopRight + offset, color);
	}

	// Token: 0x06005108 RID: 20744 RVA: 0x00150EF0 File Offset: 0x0014F2F0
	public static void GizmosDrawQuadrilateral(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4, Color color)
	{
		Color color2 = Gizmos.color;
		Gizmos.color = color;
		Gizmos.DrawLine(point1, point2);
		Gizmos.DrawLine(point2, point3);
		Gizmos.DrawLine(point3, point4);
		Gizmos.DrawLine(point4, point1);
		Gizmos.color = color2;
	}

	// Token: 0x06005109 RID: 20745 RVA: 0x00150F2C File Offset: 0x0014F32C
	public static void GizmosDrawQuadrilateral(Vector3F point1, Vector3F point2, Vector3F point3, Vector3F point4, Color color)
	{
		GizmoUtil.GizmosDrawQuadrilateral((Vector3)point1, (Vector3)point2, (Vector3)point3, (Vector3)point4, color);
	}

	// Token: 0x0600510A RID: 20746 RVA: 0x00150F50 File Offset: 0x0014F350
	public static void GizmosDrawSphere(Vector3 center, float radius, Color color)
	{
		Color color2 = Gizmos.color;
		Gizmos.color = color;
		Gizmos.DrawSphere(center, radius);
		Gizmos.color = color2;
	}

	// Token: 0x0600510B RID: 20747 RVA: 0x00150F78 File Offset: 0x0014F378
	public static void GizmosDrawDiamond(Vector3 center, Vector2 dimensions, Color color)
	{
		Vector3 point = new Vector3(center.x, center.y + dimensions.y / 2f);
		Vector3 point2 = new Vector3(center.x + dimensions.x / 2f, center.y);
		Vector3 point3 = new Vector3(center.x, center.y - dimensions.y / 2f);
		Vector3 point4 = new Vector3(center.x - dimensions.x / 2f, center.y);
		GizmoUtil.GizmosDrawQuadrilateral(point, point2, point3, point4, color);
	}

	// Token: 0x0600510C RID: 20748 RVA: 0x0015101C File Offset: 0x0014F41C
	public static void GizmosDrawLedge(Vector3 transform, bool invert, Color color, float depth)
	{
		Color color2 = Gizmos.color;
		Gizmos.color = color;
		Vector3 vector = transform - new Vector3(0f, 0f, depth * 0.5f);
		int num = (!invert) ? 1 : -1;
		Gizmos.DrawLine(vector, vector + new Vector3(0f, 0f, depth));
		Gizmos.DrawLine(vector, vector + new Vector3((float)num * 0.5f, 0f, 0f));
		Gizmos.DrawLine(vector, vector + new Vector3(0f, -0.5f, 0f));
		Gizmos.color = color2;
	}

	// Token: 0x0600510D RID: 20749 RVA: 0x001510C8 File Offset: 0x0014F4C8
	public static void GizmosConnectPoints(ref Vector3[] points, Color color, int firstIndex = 0)
	{
		firstIndex = Mathf.Max(0, Mathf.Min(points.Length, firstIndex));
		Color color2 = Gizmos.color;
		Gizmos.color = color;
		for (int i = firstIndex; i < points.Length - 1; i++)
		{
			Gizmos.DrawLine(points[i], points[i + 1]);
		}
		Gizmos.color = color2;
	}

	// Token: 0x0600510E RID: 20750 RVA: 0x00151134 File Offset: 0x0014F534
	public static void GizmosDrawCircle(Vector2 center, float radius, Color color, int segments = 10)
	{
		Color color2 = Gizmos.color;
		Gizmos.color = color;
		for (int i = 0; i < segments; i++)
		{
			float f = (float)i * (6.2831855f / (float)segments);
			float f2 = (float)(i + 1) * (6.2831855f / (float)segments);
			Vector3 from = new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * radius + center;
			Vector3 to = new Vector2(Mathf.Cos(f2), Mathf.Sin(f2)) * radius + center;
			Gizmos.DrawLine(from, to);
		}
		Gizmos.color = color2;
	}

	// Token: 0x0600510F RID: 20751 RVA: 0x001511D0 File Offset: 0x0014F5D0
	public static void GizmosDrawArrow(Vector3 start, Vector3 end, Color color, bool fixedArrowHeadSize = false, float arrowHeadSize = 0f, float arrowHeadAngle = 33f)
	{
		float d = arrowHeadSize;
		if (!fixedArrowHeadSize)
		{
			d = (end - start).magnitude / 2f;
		}
		Color color2 = Gizmos.color;
		Gizmos.color = color;
		Gizmos.DrawLine(start, end);
		Vector3 point = start - end;
		Vector3 normalized = (Quaternion.Euler(0f, 0f, arrowHeadAngle) * point).normalized;
		Vector3 normalized2 = (Quaternion.Euler(0f, 0f, -arrowHeadAngle) * point).normalized;
		Gizmos.DrawLine(end, end + normalized * d);
		Gizmos.DrawLine(end, end + normalized2 * d);
		Gizmos.color = color2;
	}

	// Token: 0x06005110 RID: 20752 RVA: 0x0015128C File Offset: 0x0014F68C
	public static void GizmosDrawLine(Vector3 start, Vector3 end, Color color)
	{
		Color color2 = Gizmos.color;
		Gizmos.color = color;
		Gizmos.DrawLine(start, end);
		Gizmos.color = color2;
	}

	// Token: 0x06005111 RID: 20753 RVA: 0x001512B4 File Offset: 0x0014F6B4
	public static void GizmoFillCapsule(Vector3 start, Vector3 end, float radius, Vector3 globalScale, Color color)
	{
		Gizmos.color = color;
		Gizmos.DrawSphere(start, radius);
		Vector3 vector = end - start;
		if (vector.sqrMagnitude != 0f)
		{
			Gizmos.DrawSphere(end, radius);
			Matrix4x4 matrix = Gizmos.matrix;
			Quaternion q = Quaternion.LookRotation(end - start);
			Matrix4x4 matrix2 = Matrix4x4.TRS(start, q, globalScale);
			Gizmos.matrix = matrix2;
			float num = vector.magnitude / globalScale.z;
			Gizmos.DrawCube(new Vector3(0f, 0f, num / 2f), new Vector3(radius / 2f, radius / 2f, num));
			Gizmos.matrix = matrix;
		}
	}

	// Token: 0x06005112 RID: 20754 RVA: 0x0015135C File Offset: 0x0014F75C
	public static void GizmoFillRectangle(Vector2 center, Vector2 size, Color color)
	{
		Color color2 = Gizmos.color;
		Gizmos.color = color;
		Gizmos.DrawCube(center, size);
		Gizmos.color = color2;
	}

	// Token: 0x06005113 RID: 20755 RVA: 0x0015138C File Offset: 0x0014F78C
	public static void GizmoDrawEdgeData(EdgeData edge, Color edgeColor, Color normalsColor = default(Color), Color unlandableColor = default(Color), Color boundingBoxColor = default(Color), bool showCastArrow = false, Color wallColor = default(Color), Color ceilingColor = default(Color))
	{
		if (edge == null || edge.Length < 2)
		{
			return;
		}
		bool flag = normalsColor != default(Color);
		bool flag2 = unlandableColor != default(Color);
		bool flag3 = wallColor != default(Color);
		bool flag4 = ceilingColor != default(Color);
		bool flag5 = boundingBoxColor != default(Color);
		Color color = Gizmos.color;
		for (int i = 0; i < edge.SegmentCount; i++)
		{
			Color color2 = edgeColor;
			if (flag2 && edge.GetSurfaceType(i) == SurfaceType.Other)
			{
				color2 = unlandableColor;
			}
			else if (flag3 && edge.GetSurfaceType(i) == SurfaceType.Wall)
			{
				color2 = wallColor;
			}
			else if (flag4 && edge.GetSurfaceType(i) == SurfaceType.Ceiling)
			{
				color2 = ceilingColor;
			}
			Gizmos.color = color2;
			Vector3 vector = edge.GetPoint(i).ToVector3();
			Vector3 vector2 = edge.GetNextPoint(i).ToVector3();
			Vector3 vector3 = (vector + vector2) / 2f;
			Vector3 normalized = (vector2 - vector).normalized;
			Vector3 a = new Vector3(-normalized.y, normalized.x);
			Gizmos.DrawWireSphere(vector, 0.05f);
			Gizmos.DrawLine(vector, vector2);
			if (showCastArrow)
			{
				Vector3 a2 = vector3 - normalized * 0.15f;
				Vector3 to = a2 + a * 0.15f;
				Vector3 to2 = a2 + a * -0.15f;
				Gizmos.DrawLine(vector3, to);
				Gizmos.DrawLine(vector3, to2);
			}
			if (flag)
			{
				Color color3 = Gizmos.color;
				Gizmos.color = normalsColor;
				Vector3 to3 = vector3 + edge.GetNormal(i).ToVector3();
				Gizmos.DrawLine(vector3, to3);
				Gizmos.color = color3;
			}
		}
		if (!edge.IsLoop)
		{
			Gizmos.DrawWireSphere(edge.GetPoint(edge.SegmentCount - 1).ToVector3(), 0.05f);
		}
		if (flag5)
		{
			GizmoUtil.GizmosDrawRectangle(edge.BoundingBox, boundingBoxColor, false);
		}
		Gizmos.color = color;
	}

	// Token: 0x17001303 RID: 4867
	// (get) Token: 0x06005114 RID: 20756 RVA: 0x001515DA File Offset: 0x0014F9DA
	public static bool AreGizmosVisible
	{
		get
		{
			return false;
		}
	}
}
