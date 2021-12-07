using System;
using FixedPoint;
using UnityEngine;

// Token: 0x02000548 RID: 1352
public interface IPhysicsCollider
{
	// Token: 0x17000653 RID: 1619
	// (get) Token: 0x06001DAE RID: 7598
	EdgeData Edge { get; }

	// Token: 0x17000654 RID: 1620
	// (get) Token: 0x06001DAF RID: 7599
	// (set) Token: 0x06001DB0 RID: 7600
	bool Enabled { get; set; }

	// Token: 0x17000655 RID: 1621
	// (get) Token: 0x06001DB1 RID: 7601
	GameObject GameObject { get; }

	// Token: 0x17000656 RID: 1622
	// (get) Token: 0x06001DB2 RID: 7602
	int Layer { get; }

	// Token: 0x17000657 RID: 1623
	// (get) Token: 0x06001DB3 RID: 7603
	int Mask { get; }

	// Token: 0x06001DB4 RID: 7604
	bool LayerIntersects(int mask);

	// Token: 0x06001DB5 RID: 7605
	bool ContainsPoint(Vector2F point);

	// Token: 0x17000658 RID: 1624
	// (get) Token: 0x06001DB6 RID: 7606
	FixedRect BoundingBox { get; }

	// Token: 0x17000659 RID: 1625
	// (get) Token: 0x06001DB7 RID: 7607
	IMovingObject MovingObject { get; }

	// Token: 0x06001DB8 RID: 7608
	void SetPoints(params Vector2F[] points);

	// Token: 0x06001DB9 RID: 7609
	void SetPointsRelative(Vector2F rootPosition, Vector2F point1, Vector2F point2, Vector2F point3, Vector2F point4);

	// Token: 0x06001DBA RID: 7610
	void SetPointsRelative(Vector2F rootPosition, params Vector2F[] points);
}
