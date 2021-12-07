using System;
using FixedPoint;
using UnityEngine;

// Token: 0x02000547 RID: 1351
public class PhysicsCollider : IPhysicsCollider
{
	// Token: 0x06001D99 RID: 7577 RVA: 0x00097342 File Offset: 0x00095742
	public PhysicsCollider(GameObject gameObject, Vector2F[] points, bool isLoop, IMovingObject movingObject)
	{
		this.Edge = new EdgeData(points, isLoop, EdgeData.CacheFlag.All);
		this.MovingObject = movingObject;
		this.Enabled = true;
		this.GameObject = gameObject;
		this.Layer = gameObject.layer;
	}

	// Token: 0x06001D9A RID: 7578 RVA: 0x0009737A File Offset: 0x0009577A
	public PhysicsCollider(GameObject gameObject, Vector2F rootPosition, Vector2F[] points, bool isLoop, IMovingObject movingObject)
	{
		this.Edge = new EdgeData(rootPosition, points, isLoop, EdgeData.CacheFlag.All);
		this.MovingObject = movingObject;
		this.Enabled = true;
		this.GameObject = gameObject;
		this.Layer = gameObject.layer;
	}

	// Token: 0x06001D9B RID: 7579 RVA: 0x000973B4 File Offset: 0x000957B4
	public PhysicsCollider(EdgeData edgeData, int layer = 0)
	{
		this.Edge = edgeData;
		this.Enabled = true;
		this.Layer = layer;
	}

	// Token: 0x1700064B RID: 1611
	// (get) Token: 0x06001D9C RID: 7580 RVA: 0x000973D1 File Offset: 0x000957D1
	// (set) Token: 0x06001D9D RID: 7581 RVA: 0x000973D9 File Offset: 0x000957D9
	public EdgeData Edge { get; private set; }

	// Token: 0x1700064C RID: 1612
	// (get) Token: 0x06001D9E RID: 7582 RVA: 0x000973E2 File Offset: 0x000957E2
	// (set) Token: 0x06001D9F RID: 7583 RVA: 0x000973EA File Offset: 0x000957EA
	public bool Enabled { get; set; }

	// Token: 0x1700064D RID: 1613
	// (get) Token: 0x06001DA0 RID: 7584 RVA: 0x000973F3 File Offset: 0x000957F3
	// (set) Token: 0x06001DA1 RID: 7585 RVA: 0x000973FB File Offset: 0x000957FB
	public GameObject GameObject { get; private set; }

	// Token: 0x1700064E RID: 1614
	// (get) Token: 0x06001DA2 RID: 7586 RVA: 0x00097404 File Offset: 0x00095804
	public FixedRect BoundingBox
	{
		get
		{
			return this.Edge.BoundingBox;
		}
	}

	// Token: 0x1700064F RID: 1615
	// (get) Token: 0x06001DA3 RID: 7587 RVA: 0x00097411 File Offset: 0x00095811
	// (set) Token: 0x06001DA4 RID: 7588 RVA: 0x00097419 File Offset: 0x00095819
	public int Layer { get; set; }

	// Token: 0x17000650 RID: 1616
	// (get) Token: 0x06001DA5 RID: 7589 RVA: 0x00097422 File Offset: 0x00095822
	// (set) Token: 0x06001DA6 RID: 7590 RVA: 0x0009742A File Offset: 0x0009582A
	public IMovingObject MovingObject { get; private set; }

	// Token: 0x17000651 RID: 1617
	// (get) Token: 0x06001DA7 RID: 7591 RVA: 0x00097434 File Offset: 0x00095834
	public Vector2F Center
	{
		get
		{
			return this.BoundingBox.Center;
		}
	}

	// Token: 0x17000652 RID: 1618
	// (get) Token: 0x06001DA8 RID: 7592 RVA: 0x0009744F File Offset: 0x0009584F
	public int Mask
	{
		get
		{
			return 1 << this.Layer;
		}
	}

	// Token: 0x06001DA9 RID: 7593 RVA: 0x0009745C File Offset: 0x0009585C
	public bool LayerIntersects(int mask)
	{
		return Layers.Intersects(this.Layer, mask);
	}

	// Token: 0x06001DAA RID: 7594 RVA: 0x0009746C File Offset: 0x0009586C
	public bool ContainsPoint(Vector2F point)
	{
		if (this.Edge.IsLoop && this.BoundingBox.ContainsPoint(point))
		{
			Vector2F vector2F = this.BoundingBox.TopRight + Vector2F.one;
			Vector2F zero = Vector2F.zero;
			Fixed maxDistance = 0;
			(point - vector2F).Decompose(ref zero, ref maxDistance);
			int num = PhysicsWorld.RaycastCollider(this, vector2F, zero, maxDistance, this.Mask, null, RaycastFlags.EnableMultipleHits, default(Fixed));
			return num % 2 == 1;
		}
		return false;
	}

	// Token: 0x06001DAB RID: 7595 RVA: 0x000974FF File Offset: 0x000958FF
	public void SetPoints(params Vector2F[] points)
	{
		this.Edge.SetPoints(points);
	}

	// Token: 0x06001DAC RID: 7596 RVA: 0x0009750D File Offset: 0x0009590D
	public void SetPointsRelative(Vector2F rootPosition, Vector2F point1, Vector2F point2, Vector2F point3, Vector2F point4)
	{
		this.Edge.SetPointsRelative(rootPosition, point1, point2, point3, point4);
	}

	// Token: 0x06001DAD RID: 7597 RVA: 0x00097521 File Offset: 0x00095921
	public void SetPointsRelative(Vector2F rootPosition, params Vector2F[] points)
	{
		this.Edge.SetPointsRelative(rootPosition, points);
	}
}
